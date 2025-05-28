using Microsoft.AspNetCore.Identity;
using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
    public DateTime ModifiedDate { get; set; }

    // profile
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [NotMapped]
    public string Role { get; set; }

    public UserType? UserType { get; set; }

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


