using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("CustomerCorrespondence")]
    public class CustomerCorrespondence
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string ContactPersonName { get; set; }

        [StringLength(100)]
        public string ContactPersonDesignation { get; set; }

        [StringLength(20)]
        public string ContactNo { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
