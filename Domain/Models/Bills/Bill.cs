using Domain.Common;
using Domain.Contractors;
using Domain.Models.Contracts;
using Domain.Models.Members;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Bills
{
    public class Bill : ModelBase<long>
    {
        public DateTime BillDate { get; set; }
        public string BillNumber { get; set; }
        public string? IssuedBy { get; set; }
        public StatusBills StatusBills { get; set; } = StatusBills.Pending;
        public decimal Salary { get; set; }
        public decimal? ConfirmSalary { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public decimal TotalAmount { get; set; }

        [ForeignKey(nameof(Contract))]
        public long ContractId { get; set; }

        [ForeignKey(nameof(Member))]
        public long TenantId { get; set; }

        [ForeignKey(nameof(Member))]
        public long MarketerId { get; set; }
        // NAVIGATION PROPERTY
        public virtual Contract Contract { get; set; }
        public virtual Member Tenant { get; set; }
        public virtual Member Marketer { get; set; }
    }
}
