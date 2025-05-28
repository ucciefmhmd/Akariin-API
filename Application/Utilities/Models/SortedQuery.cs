
namespace Application.Utilities.Models
{
    public class SortedQuery
    {
        public string PropertyName { get; set; }
        public SortDirection Direction { get; set; } = SortDirection.ASC;
    }
}
