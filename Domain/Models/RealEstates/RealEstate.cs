using Domain.Contractors;
using Domain.Models.Owners;
using System.ComponentModel.DataAnnotations.Schema;
using static Domain.Common.Enums.RealEstateEnum;

namespace Domain.Models.RealEstates
{
    public class RealEstate : ModelBase<long>
    {
        public string Name { get; set; }
        public RealEstateType Type { get; set; }
        public RealEstateCategory Category { get; set; }
        public RealEstateService Service { get; set; }

        [ForeignKey("Owner")]
        public long OwnerId { get; set; }

        // Navigation property
        public virtual Owner Owner { get; set; }
    }

}
