using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facilitat.CLOUD.Models.DTOs;
using Facilitat.CLOUD.Models.Entities;
using Facilitat.CLOUD.Repositories.Users;

namespace Facilitat.CLOUD.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            User selectedUser = await _userRepository.GetById(id);

            if (selectedUser == null)
            {
                return null; // or handle the null case as needed
            }

            return (UserDTO) selectedUser;
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await _userRepository.GetAll();

            // Check if users is null and return an empty list if it is
            if (users == null)
            {
                return Enumerable.Empty<UserDTO>();
            }

            var userDTOs = users.Select(user => new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Document = user.Document
                // Exclude the Password property for security reasons
            });

            return userDTOs;
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            User selectedUser = await _userRepository.GetUserByEmail(email);

            if (selectedUser == null)
            {
                return null; // or handle the null case as needed
            }

            return (UserDTO)selectedUser;
        }

        public async Task<bool> AddUserAsync(UserDTO userDTO)
        {
            var user = ToUserEntity(userDTO);

            return await _userRepository.Add(user);
        }

        private User ToUserEntity(UserDTO userDTO)
        {
            return new User
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                Document = userDTO.Document,
                Password = userDTO.Password
            };
        }


    }
}
