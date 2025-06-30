
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("CustomerHistoryLog")] 
    public class CustomerHistoryLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int CustomerIdentificationTypeId { get; set; }

        [Required]
        public int CustomerTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CustomerName { get; set; }

        [MaxLength(50)]
        public string? IdentificationNo { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContactNo { get; set; }

        [MaxLength(50)]
        public string? WhatsAppNo { get; set; }

        [MaxLength(50)]
        public string? EmailAddress { get; set; }

        [Required]
        public int GenderTypeId { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int RecordStatusId { get; set; }

        public int? EnteredBy { get; set; }
        public DateTime? EnteredDate { get; set; }

        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
