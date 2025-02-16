using Domain.Contractors;

namespace Domain.Models.RoleSysem
{
    public class Page : ModelBase<Guid>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public virtual ICollection<UserPageRole> UserPageRoles { get; set; } = new List<UserPageRole>();
    }
}
