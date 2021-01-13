using System.Collections.Generic;
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
    public class GetFoldersHandler : IRequestHandler<GetFoldersQuery, IQueryable<Folder>>
    {
        private readonly IMailService _mailService;
        private readonly MailSettings _mailSettings;

        public GetFoldersHandler(IMailService mailService, MailSettings mailSettings)
        {
            this._mailService = mailService;
            this._mailSettings = mailSettings;
        }

        public async Task<IQueryable<Folder>> Handle(GetFoldersQuery query, CancellationToken cancellationToken)
        {
                return await _mailService.GetFoldersImapAsync();
        }
    }
}