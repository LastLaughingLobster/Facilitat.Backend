using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Facilitat.EMAIL.Models.Entities
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string RoleName { get; set; }
    }
}
