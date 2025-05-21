using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("PremiumLine")]
    public class PremiumLine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string LineName { get; set; }

        [Required]
        public bool IsCommissionApplicable { get; set; }

        [Required]
        [StringLength(10)]
        public string Sign { get; set; }
    }
}
