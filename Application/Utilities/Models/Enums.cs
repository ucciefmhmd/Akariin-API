
namespace Application.Utilities.Models
{
    public enum FilterType
    {
        Equals = 1,
        NotEquals,
        Range,
        Contains,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual
    }
    public enum FilterOperator
    {
        And = 1, Or
    }

    public enum SortDirection { ASC = 1, DESC }
}
