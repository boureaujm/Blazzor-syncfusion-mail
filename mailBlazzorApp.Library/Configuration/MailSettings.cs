using System.Collections.Generic;

namespace mailBlazzorApp.Library.Configuration
{
    public  class MailSettings
    {
        public MailSettings()
        {
            Folders = new List<ImapFolderSettings>();
        }
        
        public string PopHost { get; set; }
        
        public string ImapHost { get; set; }

        public bool UseImap { get; set; }
        
        public string SmtpHost { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public List<ImapFolderSettings> Folders { get; set; }
    }
}