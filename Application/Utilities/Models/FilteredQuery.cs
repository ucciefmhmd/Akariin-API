

namespace Application.Utilities.Models
{
    public record FilteredQuery
    {
        public string? PropertyName { get; set; }
        public IEnumerable<string> Values { get; set; } = new List<string>();
        public FilterType Type { get; set; } = FilterType.Equals;
        public FilterOperator Operator { get; set; } = FilterOperator.Or;

    }
}
