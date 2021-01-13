using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Limilabs.Client.SMTP;
using mailBlazzorApp.Library.Dto;

namespace mailBlazzorApp.Library.Services
{
    public interface IMailService
    {
        Task<IQueryable<Mail>> GetMailPop3Async();
        Task<ISendMessageResult> SendMail(Mail newMail);
        
        void DeleteMail(string folderName, long[] uids);

        Task<IQueryable<Mail>> GetMailImapAsync(string folderName);
        
        Task<IQueryable<Folder>> GetFoldersImapAsync();
    }
}