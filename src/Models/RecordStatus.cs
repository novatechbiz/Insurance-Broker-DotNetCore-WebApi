using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("RecordStatus")]
    public class RecordStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StatusValue { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }
    }
}
