using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dasync.Collections;
using Limilabs.Client.IMAP;
using Limilabs.Client.POP3;
using Limilabs.Client.SMTP;
using Limilabs.Mail;
using Limilabs.Mail.Headers;
using mailBlazzorApp.Library.Configuration;
using mailBlazzorApp.Library.Dto;

namespace mailBlazzorApp.Library.Services
{
    public class MailService : IMailService
    {
        private MailSettings _mailSettings;
        
        public MailService(MailSettings mailSettings)
        {
            _mailSettings = mailSettings;
        }
        
        public async Task<IQueryable<Mail>> GetMailPop3Async()
        {
            var mails = new List<Mail>();

            try
            {
                using (Pop3 client = new Pop3())
                {
                    IAsyncResult result = client.BeginConnectSSL(_mailSettings.PopHost);
// 5 seconds timeout
                    bool success = result.AsyncWaitHandle.WaitOne(5000, true);

                    if (success == false)
                    {
                        throw new Exception("Failed to connect server.");
                    }

                    client.EndConnect(result);

                    client.UseBestLogin(_mailSettings.User, _mailSettings.Password);

                    var maiListUniqueId = client.GetAll().ToAsyncEnumerable();

                    await foreach (var uniqueId in maiListUniqueId)
                    {
                        MailBuilder builder = new MailBuilder();
                        var eml = client.GetMessageByUID(uniqueId);
                        IMail email = builder.CreateFromEml(eml);

                        var receivedMail = new Mail
                        {
                            Subject = email.Subject,
                            Date = email.Date,
                            Sender = email.Sender?.Address,
                            Html = email.Html
                        };


                        mails.Add(receivedMail);
                    }
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

   

            return mails.AsQueryable();
        }

        public async void DeleteMail(string folderName, long[] uids)
        {
            try
            {
                using (Imap client = new Imap())
                {
                    client.ConnectSSL(_mailSettings.ImapHost);
                    client.UseBestLogin(_mailSettings.User, _mailSettings.Password);

                    client.Select(folderName);
                    var tasks = new List<Task>();

                    foreach(var uid in uids)
                        tasks.Add(Task.Run(() => client.DeleteMessageByUID(uid)));

                    await Task.WhenAll(tasks);

                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Read mail from imap inbox
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IQueryable<Mail>> GetMailImapAsync(string folderName)
        {
            var mails = new List<Mail>();

            try
            {
                using (Imap client = new Imap())
                {
                    client.ConnectSSL(_mailSettings.ImapHost);
                    //client.StartTLS();
                    client.UseBestLogin(_mailSettings.User, _mailSettings.Password);

                    client.Select(folderName);
// 5 seconds timeout
                    var maiListUniqueId = client.Search(Flag.All);

                    await foreach (var uniqueId in maiListUniqueId)
                    {
                        MailBuilder builder = new MailBuilder();
                        var eml = client.GetMessageByUID(uniqueId);
                        IMail email = builder.CreateFromEml(eml);

                        var receivedMail = new Mail
                        {
                            Subject = email.Subject,
                            Date = email.Date,
                            Sender = email.Sender?.Address,
                            Html = email.Html,
                            Uid = uniqueId
                        };


                        mails.Add(receivedMail);
                    }
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

   

            return mails.OrderByDescending(m => m.Date).AsQueryable();
        }
        
        /// <summary>
        /// Read mail from imap inbox
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public  Task<IQueryable<Folder>> GetFoldersImapAsync()
        {
            try
            {
                using (Imap imap = new Imap())
                {
                    imap.ConnectSSL(_mailSettings.ImapHost);
                    imap.UseBestLogin(_mailSettings.User, _mailSettings.Password);

                    var folders = Task.FromResult<IQueryable<Folder>>(GetFolders(imap));
                    imap.Close();

                    return folders;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private IQueryable<Folder> GetFolders(Imap imapClient)
        {
            var folders = new List<Folder>();
            var foldersInfo = imapClient.GetFolders();
            foreach (FolderInfo folderInfo in foldersInfo.Where(f => f.CanSelect == true))
            {
                var folder = new Folder {OriginalName = folderInfo.Name};
                var imapFolderSetting = _mailSettings.Folders.FirstOrDefault(f => f.Name == folder.OriginalName);
                if (imapFolderSetting != null)
                {
                    folder.Name = imapFolderSetting.Translate;
                    folder.Order = imapFolderSetting.Order;
                    folders.Add(folder);
                }
            }

            return folders.OrderBy(f => f.Order).AsQueryable();
        }

        public async Task<ISendMessageResult> SendMail(Mail newMail)
        {
            ISendMessageResult res;
            
            // Use builder class to create new email message
            MailBuilder builder = new MailBuilder();
            builder.From.Add(new MailBox(newMail.Sender));
            builder.To.Add(new MailBox(newMail.Recipient));
            builder.Subject = newMail.Subject;
            builder.Html = newMail.Html;

            // Read attachment from disk...and add it to Visuals collection
            // MimeData image = builder.AddVisual(@"c:\image.jpg");
            // image.ContentId = "image1";

            IMail email = builder.Create();

            // Send the message
            using (Smtp smtp = new Smtp())
            {
                smtp.Connect(_mailSettings.SmtpHost); // or ConnectSSL for SSL
                smtp.UseBestLogin(_mailSettings.User, _mailSettings.Password);

                
                res = await Task.FromResult<ISendMessageResult>(smtp.SendMessage(email));  
                smtp.Close();
            }

            return res;
        }
    }
}