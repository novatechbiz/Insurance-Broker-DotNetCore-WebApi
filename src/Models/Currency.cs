using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("Currency")]
    public class Currency
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CurrencyName { get; set; }

        [Required]
        [StringLength(10)]
        public string CurrencyCode { get; set; }
    }
}
