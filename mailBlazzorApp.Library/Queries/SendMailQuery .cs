using System;
using System.Collections.Generic;
using System.Linq;
using Limilabs.Client.SMTP;
using mailBlazzorApp.Library.Dto;
using MediatR;

namespace mailBlazzorApp.Library.Queries
{
    public class SendMailQuery : IRequest<ISendMessageResult>
    {
        public Mail _email { get; set; }

        public SendMailQuery(Mail newMail)
        {
            this._email = newMail;
        }
    }
}