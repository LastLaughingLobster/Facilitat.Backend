using System.Collections.Generic;
using System.Threading.Tasks;
using Facilitat.CLOUD.Models.DTOs;

namespace Facilitat.CLOUD.Services.Users
{
    public interface IUserService
    {
        Task<UserDTO> GetByIdAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<bool> AddUserAsync(UserDTO userDTO);
    }
}
