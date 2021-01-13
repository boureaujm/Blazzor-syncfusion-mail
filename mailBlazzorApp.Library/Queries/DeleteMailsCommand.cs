using System;
using System.Collections.Generic;
using System.Linq;
using Limilabs.Client.SMTP;
using mailBlazzorApp.Library.Dto;
using MediatR;

namespace mailBlazzorApp.Library.Queries
{
    public class DeleteMailCommand : IRequest<bool>
    {
        public string _folderName { get; set; }
        public long[] _uids { get; set; }

        public DeleteMailCommand(string folderName, long[] uids)
        {
            this._uids = uids;
            this._folderName = folderName;
        }
    }
}