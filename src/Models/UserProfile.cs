using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuraNova.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string IdentificationNo { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(400)]
        public string UserPassword { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(400)]
        public string? RefreshToken { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
