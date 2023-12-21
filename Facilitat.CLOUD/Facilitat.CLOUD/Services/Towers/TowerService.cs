using System.Collections.Generic;
using System.Threading.Tasks;
using Facilitat.CLOUD.Models.DTOs;
using Facilitat.CLOUD.Repositories.Towers;

namespace Facilitat.CLOUD.Services.Towers
{
    public class TowerService : ITowerService
    {
        private readonly ITowerRepository _towerRepository;

        public TowerService(ITowerRepository towerRepository)
        {
            _towerRepository = towerRepository;
        }

        public async Task<IEnumerable<TowerDTO>> GetTowersByUserId(int userId)
        {
            var towers = await _towerRepository.GetTowersByUserId(userId);

            return towers;
        }
    }
}
