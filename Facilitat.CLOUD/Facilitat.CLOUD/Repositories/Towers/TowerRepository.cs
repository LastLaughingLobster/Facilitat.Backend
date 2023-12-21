using Dapper;
using Facilitat.CLOUD.Models.DTOs;
using Facilitat.CLOUD.Models.Entities;
using Facilitat.CLOUD.Repositories.Generic;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Facilitat.CLOUD.Repositories.Towers
{
    public class TowerRepository : GenericRepository<Tower>, ITowerRepository
    {
        public TowerRepository(IDbConnection dbConnection)
            : base(dbConnection)
        {
        }

        public async Task<IEnumerable<TowerDTO>> GetTowersByUserId(int userId)
        {
            var query = @"
            SELECT Tower.Id, Tower.Name, Tower.Address, Apartment.Id, Apartment.Number
            FROM Tower
            INNER JOIN Apartment ON Tower.Id = Apartment.TowerId
            WHERE Apartment.UserID = @UserId";

            var towerDictionary = new Dictionary<int, TowerDTO>();

            var result = await _dbConnection.QueryAsync<Tower, Apartment, TowerDTO>(
                query,
                (tower, apartment) =>
                {
                    if (!towerDictionary.TryGetValue(tower.Id, out var towerEntry))
                    {
                        towerEntry = new TowerDTO
                        {
                            Id = tower.Id,
                            Name = tower.Name,
                            Address = tower.Address,
                            Apartments = new List<ApartmentDTO>()
                        };
                        towerDictionary.Add(tower.Id, towerEntry);
                    }

                    towerEntry.Apartments.Add(new ApartmentDTO
                    {
                        Id = apartment.Id,
                        TowerId = tower.Id,
                        Number = apartment.Number,
                        UserId = userId
                    });

                    return towerEntry;
                },
                new { UserId = userId },
                splitOn: "Id"
            );

            return result.Distinct();
        }
    }

}
