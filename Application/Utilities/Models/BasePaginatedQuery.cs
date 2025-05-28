
namespace Application.Utilities.Models
{
    public record BasePaginatedQuery
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = int.MaxValue;

        public List<FilteredQuery>? Filters { get; init; }

        public List<SortedQuery>? Sorts { get; init; }

        public string? SearchTerm { get; init; }
    }
}
