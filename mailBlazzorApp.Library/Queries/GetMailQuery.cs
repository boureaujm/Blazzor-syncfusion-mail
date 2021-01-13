using System;
using System.Linq;
using mailBlazzorApp.Library.Dto;
using MediatR;

namespace mailBlazzorApp.Library.Queries
{
    public class GetMailQuery : IRequest<IQueryable<Mail>>
    {
        public string FolderName { get; set; }
        public GetMailQuery(string folderName)
        {
            this.FolderName = folderName;
        }
    }
}