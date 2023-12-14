using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Facilitat.CLOUD.Models.Entities
{
    [Table("Apartment")]
    public class Apartment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Tower")]
        public int TowerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Number { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        public Tower Tower { get; set; }
        public User User { get; set; }
    }

}
