using Facilitat.CLOUD.Repositories.Generic;
using System.Data;
using System.Threading.Tasks;
using Facilitat.CLOUD.Models.Entities;
using Dapper;

namespace Facilitat.CLOUD.Repositories.Users
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IDbConnection dbConnection)
            : base(dbConnection)
        {
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
        }


        // You can also override generic methods if needed
        /* public override Task<User> GetById(int id)
        {
            // Custom implementation for Users
            // ...
        } */
    }
}
