using Domain.Contractors;
using Domain.Models.Bills;
using Domain.Models.Contracts;
using Domain.Models.MaintenanceRequests;
using Domain.Models.RealEstates;
using Domain.Models.RealEstateUnits;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Members
{
    public class Member : ModelBase<long>
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string? City { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }

        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string Role { get; set; }

        // NAVIGATION PROPERTY
        public virtual ICollection<RealEstate> OwnerRealEstate { get; set; } = [];
        public virtual ICollection<RealEstateUnit> TanentRealEstateUnit { get; set; } = [];
        public virtual ICollection<Contract> MarketerContract { get; set; } = [];
        public virtual ICollection<Contract> TanentContract { get; set; } = [];
        public virtual ICollection<MaintenanceRequest> TanentMaintenanceRequest { get; set; } = [];
        public virtual ICollection<MaintenanceRequest> MemberMaintenanceRequest { get; set; } = [];
        public virtual ICollection<Bill> TanentBill { get; set; } = [];
        public virtual ICollection<Bill> MarketerBill { get; set; } = [];

    }


}
