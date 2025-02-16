using Application.Utilities.Models;

namespace Application.Utilities.Contractors
{
    internal interface IFilteringStrategy
    {
        IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query, List<FilteredQuery> filters);
    }
}
