using Domain.Contractors;
using Domain.Models.Members;
using Domain.Models.RealEstates;
using Domain.Models.RealEstateUnits;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.MaintenanceRequests
{
    public class MaintenanceRequest : ModelBase<long>
    {
        public string RequestNumber { get; set; } = Guid.NewGuid().ToString().Substring(0, 8);
        public DateTime RequestDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string CostBearer { get; set; }
        public string MaintenanceType { get; set; }
        public bool IsPrivateMaintenance { get; set; }
        public string? Description { get; set; }
        public string? MaintenanceRequestFile { get; set; }

        [ForeignKey(nameof(Tenant))]
        public long TenantId { get; set; }

        [ForeignKey(nameof(Member))]
        public long MemberId { get; set; }

        [ForeignKey(nameof(RealEstate))]
        public long RealEstateId { get; set; }

        [ForeignKey(nameof(RealEstateUnit))]
        public long RealEstateUnitId { get; set; }

        public virtual RealEstate RealEstate { get; set; }
        public virtual RealEstateUnit RealEstateUnit { get; set; }
        public virtual Member Member { get; set; }
        public virtual Member Tenant { get; set; }
    }
}
