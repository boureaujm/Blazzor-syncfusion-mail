using System;
using System.Linq;
using mailBlazzorApp.Library.Dto;
using MediatR;

namespace mailBlazzorApp.Library.Queries
{
    public class GetFoldersQuery : IRequest<IQueryable<Folder>>
    {
        public DateTime Date { get; set; }

        public GetFoldersQuery()
        {
        }
    }
}