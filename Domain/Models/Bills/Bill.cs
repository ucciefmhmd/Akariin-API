using Domain.Contractors;
using Domain.Models.Contracts;

namespace Domain.Models.Bills
{
    public class Bill : ModelBase<long>
    {
        public string Amount { get; set; }
        public DateTime Date { get; set; }
        public long Number { get; set; }
        public float Salary { get; set; }
        public float Discount { get; set; }
        public float Tax { get; set; }
        public long ContractId { get; set; }
        
        // NAVIGATION PROPERTY
        public virtual Contract Contract { get; set; }
    }
}
