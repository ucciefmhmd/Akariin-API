using Application.Utilities.Extensions;
using Application.Utilities.Filter;
using Application.Utilities.Models;
using Application.Utilities.Sort;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.RoleSystem.Queries.GetPages
{
    public record GetPagesQueryResult:BaseCommandResult
    {
        public BasePaginatedList<PageVM> Pages { get; set; }
    }
    public record GetPagesQuery:BasePaginatedQuery, IRequest<GetPagesQueryResult>
    {

    }
    public record PageVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
    public class GetPagesQueryHandler : IRequestHandler<GetPagesQuery, GetPagesQueryResult>
    {
        private readonly ApplicationDbContext _context;

        public GetPagesQueryHandler(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<GetPagesQueryResult> Handle(GetPagesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var pages = await _context.Pages.Search(request.SearchTerm)
                    .Select(p => new PageVM { Name = p.Name , Path = p.Path , Id = p.Id })
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Filter(request.Filters)
                    .Sort(request.Sorts)
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return new GetPagesQueryResult
                {
                    IsSuccess = true,
                    Pages = pages
                };
            }
            catch (Exception ex)
            {
                return new GetPagesQueryResult
                {
                    ErrorCode = Domain.Common.ErrorCode.Error,
                    Errors = { ex.Message },
                    IsSuccess = false
                };
            }
        }
    }
}
