using System.Threading;
using System.Threading.Tasks;
using mailBlazzorApp.Library.Configuration;
using mailBlazzorApp.Library.Queries;
using mailBlazzorApp.Library.Services;
using MediatR;

namespace mailBlazzorApp.Library.Handlers
{
    public class DeleteMailHandler : IRequestHandler<DeleteMailCommand, bool>
    {
        private readonly IMailService _mailService;
        private readonly MailSettings _mailSettings;

        public DeleteMailHandler(IMailService mailService, MailSettings mailSettings)
        {
            this._mailService = mailService;
            this._mailSettings = mailSettings;
        }

        public async Task<bool> Handle(DeleteMailCommand command, CancellationToken cancellationToken)
        {
            // if (_mailSettings.UseImap)
            // {
                ;
                await Task.Run(() => _mailService.DeleteMail(command._folderName, command._uids));
                return true;
                // }
                // else
                // {
                //     return await _mailService.GetMailPop3Async();
                // }
        }
    }
}