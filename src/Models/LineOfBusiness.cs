using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("LineOfBusiness")]
    public class LineOfBusiness
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InsuranceTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string LobName { get; set; }
    }
}
