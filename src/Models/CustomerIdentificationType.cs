using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("CustomerIdentificationType")]
    public class CustomerIdentificationType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string IdentificationType { get; set; }
    }
}
