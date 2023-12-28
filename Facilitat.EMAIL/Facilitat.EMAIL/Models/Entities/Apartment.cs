using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Facilitat.EMAIL.Models.Entities
{
    [Table("Apartment")]
    public class Apartment
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("Tower")]
        [Column("TowerId")]
        public int TowerId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("Number")]
        public string Number { get; set; }

        [ForeignKey("User")]
        [Column("UserID")]
        public int? UserId { get; set; }
    }

}
