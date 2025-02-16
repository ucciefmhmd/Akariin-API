using Domain.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Contractors
{
    public class ModelBase<T> : IModelBase<T>
    {
        [Key]
        public T Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; }

        public virtual ApplicationUser? CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public string? CreatedById { get; set; }

        public virtual ApplicationUser? ModifiedBy { get; set; }
        [ForeignKey("ModifiedBy")]
        public string? ModifiedById { get; set; }


    }
}
