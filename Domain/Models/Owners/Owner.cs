using Domain.Contractors;
using Domain.Models.RealEstates;
using System.ComponentModel.DataAnnotations;
using static Domain.Common.Enums.OwnerEnum;

namespace Domain.Models.Owners
{
    public class Owner : ModelBase<long>
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string City { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }
        public DateOnly Birthday { get; set; }
        public string Nationality { get; set; }
        public string Role { get; set; }
        public string IdNumber { get; set; }

        // NAVIGATION PROPERTY
        public virtual ICollection<RealEstate> RealEstates { get; set; } = new List<RealEstate>();

    }


}
