using Domain.Contractors;
using Domain.Models.Members;
using Domain.Models.RealEstates;
using Domain.Models.RealEstateUnits;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Contracts
{
    public class Contract : ModelBase<long>
    {
        public string ContractNumber { get; set; } = Guid.NewGuid().ToString().Substring(0, 8);
        public string PaymentCycle { get; set; }
        public string AutomaticRenewal { get; set; }
        public string ContractRent { get; set; }
        public DateTime DateOfConclusion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public decimal? TenantTax { get; set; }
        public string Status { get; set; }
        public string? ContractFile { get; set; }
        public bool IsExecute { get; set; }
        public bool IsFinished { get; set; }
        public decimal PaymentAmount { get; set; }


        // Foreign Keys
        [ForeignKey(nameof(RealEstateUnit))]
        public long RealEstateUnitId { get; set; }

        [ForeignKey(nameof(RealEstate))]
        public long RealEstateId { get; set; }

        [ForeignKey(nameof(Member))]
        public long TenantId { get; set; }

        [ForeignKey(nameof(Member))]
        public long MarketerId { get; set; }

        // Navigation Properties
        public virtual RealEstateUnit RealEstateUnit { get; set; }
        public virtual RealEstate RealEstate { get; set; }
        public virtual Member Tenant { get; set; }
        public virtual Member Marketer { get; set; }
    }

}
