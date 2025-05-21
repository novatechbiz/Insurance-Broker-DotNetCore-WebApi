using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("CompanyBrokerCode")]
    public class CompanyBrokerCode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InsuranceCompanyId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        [StringLength(50)]
        public string BrokerCode { get; set; }
    }
}
