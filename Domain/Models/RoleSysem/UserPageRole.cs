using Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models.RoleSysem
{
    public class UserPageRole 
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Guid PageId { get; set; }
        public virtual Page Page { get; set; }

        public string RoleId { get; set; }
        public virtual IdentityRole Role { get; set; }

    }
}
