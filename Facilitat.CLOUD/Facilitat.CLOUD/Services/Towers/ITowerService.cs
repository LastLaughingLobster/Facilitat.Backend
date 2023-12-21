using System.Collections.Generic;
using System.Threading.Tasks;
using Facilitat.CLOUD.Models.DTOs;

namespace Facilitat.CLOUD.Services.Towers
{
    public interface ITowerService
    {
        Task<IEnumerable<TowerDTO>> GetTowersByUserId(int userId);
    }
}
