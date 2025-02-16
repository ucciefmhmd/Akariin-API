using Domain.Contractors;
using Domain.Models.RealEstates;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Owners
{
    public class Owner : ModelBase<long>
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
        public virtual ICollection<RealEstate> RealEstates { get; set; } = new List<RealEstate>();

    }


}
