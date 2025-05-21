using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace InsuraNova.Models
{
    [Table("Company")]
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CompanyTypeId { get; set; }

        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; }

        [StringLength(300)]
        public string Address { get; set; }

        [StringLength(20)]
        public string ContactNo { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public int? ParentCompanyId { get; set; }
    }

}
