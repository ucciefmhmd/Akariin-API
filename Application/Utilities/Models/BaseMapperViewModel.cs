using Domain.Contractors;
using Domain.Identity;
using Mapster;

namespace Application.Utilities.Models
{
    public class BaseMapperViewModel<T> : ModelBase<T>
    {
        public T Id { get; set; }
        [AdaptIgnore]
        public override ApplicationUser? CreatedBy { get; set; }
        public string? CreatedByFullName { get; set; }

        [AdaptIgnore]
        public override ApplicationUser? ModifiedBy { get; set; }
        public string? ModifiedByFullName { get; set; }
    }
}
