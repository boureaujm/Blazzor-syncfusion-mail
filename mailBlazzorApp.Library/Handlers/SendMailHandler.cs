using System.Threading;
using System.Threading.Tasks;
using Limilabs.Client.SMTP;
using mailBlazzorApp.Library.Queries;
using mailBlazzorApp.Library.Services;
using MediatR;

namespace mailBlazzorApp.Library.Handlers
{
    public class SendMailHandler : IRequestHandler<SendMailQuery, ISendMessageResult>
    {
        private readonly IMailService _mailService;

        public SendMailHandler(IMailService mailService)
        {
            this._mailService = mailService;
        }

        public async Task<ISendMessageResult> Handle(SendMailQuery query, CancellationToken cancellationToken)
        {
            return await _mailService.SendMail(query._email);
        }
    }
}