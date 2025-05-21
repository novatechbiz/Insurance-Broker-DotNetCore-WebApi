using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("InsuranceCover")]
    public class InsuranceCover
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PremiumLineId { get; set; }

        [Required]
        public int InsuranceTypeId { get; set; }

        [Required]
        [StringLength(200)]
        public string CoverName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal DefaultBrokerCommissionRate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal DefaultAgentCommissionRate { get; set; }
    }
}
