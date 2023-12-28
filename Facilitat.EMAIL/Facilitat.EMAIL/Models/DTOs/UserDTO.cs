using Facilitat.EMAIL.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facilitat.EMAIL.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string LastName { get; set; }

        [MaxLength(255)]
        public string Password { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(255)]
        public string Document { get; set; }

        public static explicit operator UserDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Document = user.Document,
                Password = user.Password,   
            };
        }
    }

}
