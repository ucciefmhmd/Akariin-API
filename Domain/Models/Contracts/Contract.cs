using Domain.Contractors;
using Domain.Models.RealEstateUnits;
using Domain.Models.Tenants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Contracts
{
    public class Contract : ModelBase<long>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateOfConclusion { get; set; }
        public TimeOnly Duration { get; set; }
        public long Number { get; set; }
        public string Type { get; set; }
        public string TerminationMethod { get   ; set; }

        // Foreign Keys
        [ForeignKey(nameof(RealEstateUnit))]
        public long RealEstateUnitId { get; set; }

        [ForeignKey(nameof(Tenant))]
        public long TenantId { get; set; }

        // Navigation Properties
        public virtual RealEstateUnit RealEstateUnit { get; set; }
        public virtual Tenant Tenant { get; set; }
    }

}
