using Domain.Contractors;
using Domain.Models.Tenants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.RealEstateUnits
{
    public class RealEstateUnit : ModelBase<long>
    {
        public string AnnualRent { get; set; }
        public string Area { get; set; }
        public string Floor { get; set; }
        public string UnitNumber { get; set; }
        public string NumOfRooms { get; set; }
        public string Type { get; set; }

        [ForeignKey("Tenant")]
        public long TenantId { get; set; }

        // NAVIGATION PROPERTY
        public Tenant Tenant { get; set; }
    }

}
