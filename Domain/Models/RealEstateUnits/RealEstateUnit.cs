using Domain.Contractors;
using Domain.Models.RealEstates;
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
        public string? GasMeter { get; set; }
        public string? WaterMeter { get; set; }
        public string? ElectricityCalculation { get; set; }
        public string? Image { get; set; }

        [ForeignKey("Tenant")]
        public long TenantId { get; set; }

        [ForeignKey("RealEstate")]
        public long RealEstateId { get; set; }

        // NAVIGATION PROPERTY
        public Tenant Tenant { get; set; }

        public RealEstate RealEstate { get; set; }
    }

}
