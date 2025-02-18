using Domain.Contractors;
using Domain.Models.Contracts;
using Domain.Models.Members;
using Domain.Models.RealEstateUnits;

namespace Domain.Models.RealEstates
{
    public class RealEstate : ModelBase<long>
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Service { get; set; }

        // Additional properties
        public string? DocumentType { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? Guard { get; set; }
        public long? GuardId { get; set; }
        public string? GuardMobile { get; set; }
        public string? AdNumber { get; set; }
        public string? ElectricityCalculation { get; set; }
        public string? GasMeter { get; set; }
        public string? WaterMeter { get; set; }
        public string? Image { get; set; }
        public string Status { get; set; }
        public long OwnerId { get; set; }

        // Navigation property
        public virtual Member Owner { get; set; }
        public virtual ICollection<RealEstateUnit> RealEstateUnits { get; set; } = new HashSet<RealEstateUnit>();
        public virtual ICollection<Contract> Contracts { get; set; } = new HashSet<Contract>();
    }

}
