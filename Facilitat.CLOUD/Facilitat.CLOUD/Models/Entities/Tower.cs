using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Facilitat.CLOUD.Models.Entities
{
    [Table("Tower")]
    public class Tower
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("Address")]
        public string Address { get; set; }
    }
}
