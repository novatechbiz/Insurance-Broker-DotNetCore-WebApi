using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("InsuranceProduct")]
    public class InsuranceProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InsuranceCompanyId { get; set; }

        [Required]
        public int LineOfBusinessId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }
    }
}
