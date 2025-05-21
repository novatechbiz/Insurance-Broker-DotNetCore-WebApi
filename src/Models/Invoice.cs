using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("Invoice")]
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int EntryTypeId { get; set; }

        [Required]
        public int TransactionTypeId { get; set; }

        public int? CustomerId { get; set; }

        public int? CustomerCorrespondenceId { get; set; }

        public int? InsuranceCompanyId { get; set; }

        public int? InsuranceProductId { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        public int? CompanyBrokerCodeId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfInvoice { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }

        [StringLength(100)]
        public string TransactionRefNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        [StringLength(100)]
        public string PolicyNo { get; set; }

        [StringLength(100)]
        public string VehicleNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CommencementDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PolicyStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PolicyEndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BalanceAmount { get; set; }

        public int? RecordStatusId { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(100)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
