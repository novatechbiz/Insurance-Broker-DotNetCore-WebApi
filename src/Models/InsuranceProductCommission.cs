using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("InsuranceProductCommission")]
    public class InsuranceProductCommission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InsuranceProductId { get; set; }

        [Required]
        public int PremiumLineId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal BrokerCommissionRate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal AgentCommissionRate { get; set; }
    }
}
