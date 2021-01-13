using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using mailBlazzorApp.Library.Configuration;
using mailBlazzorApp.Library.Dto;
using mailBlazzorApp.Library.Queries;
using mailBlazzorApp.Library.Services;
using MediatR;

namespace mailBlazzorApp.Library.Handlers
{
    public class GetMailHandler : IRequestHandler<GetMailQuery, IQueryable<Mail>>
    {
        private readonly IMailService _mailService;
        private readonly MailSettings _mailSettings;

        public GetMailHandler(IMailService mailService, MailSettings mailSettings)
        {
            this._mailService = mailService;
            this._mailSettings = mailSettings;
        }

        public async Task<IQueryable<Mail>> Handle(GetMailQuery query, CancellationToken cancellationToken)
        {
            if (_mailSettings.UseImap)
            {
                return await _mailService.GetMailImapAsync(query.FolderName);
            }
            else
            {
                return await _mailService.GetMailPop3Async();
            }
        }
    }
}