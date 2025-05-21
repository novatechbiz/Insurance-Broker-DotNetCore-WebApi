using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("InvoicePremiumLine")]
    public class InvoicePremiumLine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public int PremiumLineId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BrokerCommission { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AgentCommission { get; set; }
    }
}
