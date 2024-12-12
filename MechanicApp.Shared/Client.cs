using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MechanicApp.Shared
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ClientId { get; set; }

        [Required]
        [WhiteSpaceAndNullVerification]
        [MaxLength(15)]
        public string Name { get; set; }

        [Required]
        [WhiteSpaceAndNullVerification]
        public string Address { get; set; }

        [Required]
        [WhiteSpaceAndNullVerification]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }
    }
}
