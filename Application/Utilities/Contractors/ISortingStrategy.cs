using Application.Utilities.Models;

namespace Application.Utilities.Contractors
{
    internal interface ISortingStrategy
    {
        IQueryable<TEntity> Sort<TEntity>(IQueryable<TEntity> query, List<SortedQuery> sortCriteria);
    }
}
