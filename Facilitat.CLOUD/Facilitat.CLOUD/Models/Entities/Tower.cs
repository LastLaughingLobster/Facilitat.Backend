using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Facilitat.CLOUD.Models.Entities
{
    [Table("Tower")]
    public class Tower
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Adress { get; set; }
    }
}
