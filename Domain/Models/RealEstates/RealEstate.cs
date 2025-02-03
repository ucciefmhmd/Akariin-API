using Domain.Contractors;
using Domain.Models.Owners;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.RealEstates
{
    public class RealEstate : ModelBase<long>
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Service { get; set; }

        [ForeignKey("Owner")]
        public long OwnerId { get; set; }

        // Navigation property
        public virtual Owner Owner { get; set; }
    }

}
