using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int CustomerIdentificationTypeId { get; set; }

        [Required]
        public int CustomerTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string CustomerName { get; set; }

        [StringLength(50)]
        public string IdentificationNo { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }

        [StringLength(50)]
        public string ContactNo { get; set; }

        [StringLength(50)]
        public string WhatsAppNo { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public int? GenderTypeId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        public int? RecordStatusId { get; set; }

        // [StringLength(100)]
        public int EnteredBy { get; set; } //string

        public DateTime? EnteredDate { get; set; }

        // [StringLength(100)]
        public int ModifiedBy { get; set; } //string

        public DateTime? ModifiedDate { get; set; }
    }
}
