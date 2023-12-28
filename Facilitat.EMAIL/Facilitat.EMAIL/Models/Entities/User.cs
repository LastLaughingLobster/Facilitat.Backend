using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Facilitat.EMAIL.Models.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("FirstName")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("LastName")]
        public string LastName { get; set; }

        // Store a hashed password, never store plain text passwords
        [Required]
        [MaxLength(255)]
        [Column("Password")]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        // [EmailAddress] 
        [Column("Email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("Document")]
        public string Document { get; set; }
    }
}
