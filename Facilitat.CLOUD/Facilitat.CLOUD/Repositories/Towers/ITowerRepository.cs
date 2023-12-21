using Facilitat.CLOUD.Repositories.Generic;
using System.Threading.Tasks;
using Facilitat.CLOUD.Models.Entities;
using Facilitat.CLOUD.Models.DTOs;
using System.Collections.Generic;

namespace Facilitat.CLOUD.Repositories.Towers
{
    public interface ITowerRepository : IGenericRepository<Tower>
    {
        // Define additional operations specific to the User entity
        Task<IEnumerable<TowerDTO>> GetTowersByUserId(int userId);
    }

}
