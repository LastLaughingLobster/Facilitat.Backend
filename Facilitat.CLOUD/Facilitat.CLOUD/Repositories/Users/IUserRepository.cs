using Facilitat.CLOUD.Repositories.Generic;
using System.Threading.Tasks;
using Facilitat.CLOUD.Models.Entities;

namespace Facilitat.CLOUD.Repositories.Users
{
    public interface IUserRepository : IGenericRepository<User>
    {
        // Define additional operations specific to the User entity
        Task<User> GetUserByEmail(string email);
    }

}
