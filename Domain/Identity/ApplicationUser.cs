using Microsoft.AspNetCore.Identity;
using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Professionals;
using Domain.Models.Products;
using Domain.Models.Settings;

namespace Domain.Identity;

public class ApplicationUser : IdentityUser
{

    [NotMapped]
    public string Name 
    {
        get
        {
            if(FirstName != null|| LastName != null)
            {
                return FirstName + " " + LastName;
            }
            else
            {
                return UserName ?? Email;
            }
           
        }
    }
    public bool IsActive { get; set; } = true;
    [DataType(DataType.DateTime)]
    public DateTime CreatedDate { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

    // profile
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AboutMe { get; set; }
    public string? MyFavStyle { get; set; }
    public string? MyNextHouseProject { get; set; }

    public string? LicenseNumber { get; set; }
    public string? BusinessDescription { get; set; }
    public string? CertificationAndAwards { get; set; }
    public string? Affiliations { get; set; }

    [Precision(18,4)]
    public decimal? JobCostFrom { get; set; }
    [Precision(18, 4)]
    public decimal? JobCostTo { get; set; }
    public string? CostDetails { get; set; }


    [NotMapped]
    public IList<string> Roles { get; set; }
    //Contact Info

    public Guid? DistrictId { get; set; }
    public virtual District? District { get; set; }
    public string? ZIP { get; set; }
    // userPro
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? Fax { get; set; }


    //social media
    public string? Facebook { get; set; }
    public string? Twitter { get; set; }
    public string? LinkedIn { get; set; }
    public string? WebsiteOrBlog { get; set; }

    public UserType? UserType { get; set; }


    //billingInfo
    public string? BillingFirstName { get; set; }
    public string? BillingFatherName { get; set; }
    public string? BillingFamilyName { get; set; }
    public string? BillingFullNameInEnglish { get; set; }
    public string? BillingMainPhoneNumber { get; set; }
    public string? BillingAlternativePhoneNumber { get; set; }


    public virtual ICollection<ApplicationUserCity> ApplicationUserCities { get; set; } = new List<ApplicationUserCity>();

    public virtual ICollection<ApplicationUserService> ApplicationUserServices { get; set; } = new List<ApplicationUserService>();


    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>();




    [NotMapped]
    public string Image { get; set; } 
    // hide from serialization
    [JsonIgnore]
    public override string SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }
    [JsonIgnore]
    public override string NormalizedEmail { get => base.NormalizedEmail; set => base.NormalizedEmail = value; }
    [JsonIgnore]
    public override string? NormalizedUserName { get => base.NormalizedUserName; set => base.NormalizedUserName = value; }
    [JsonIgnore]
    public override string ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }
    [JsonIgnore]
    public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }
    [JsonIgnore]
    public override bool TwoFactorEnabled { get => base.TwoFactorEnabled; set => base.TwoFactorEnabled = value; }
    [JsonIgnore]
    public override int AccessFailedCount { get => base.AccessFailedCount; set => base.AccessFailedCount = value; }
}


