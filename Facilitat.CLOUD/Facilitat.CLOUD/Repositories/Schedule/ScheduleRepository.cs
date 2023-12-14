using Facilitat.CLOUD.Models.Entities;
using Facilitat.CLOUD.Repositories.Generic;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Facilitat.CLOUD.Repositories.Schedule;
using Facilitat.CLOUD.Models.Enums;

namespace Facilitat.CLOUD.Repositories.ScheduleOrders
{
    public class ScheduleOrderRepository : GenericRepository<ScheduleOrder>, IScheduleRepository
    {
        public ScheduleOrderRepository(IDbConnection dbConnection)
            : base(dbConnection)
        {
        }

        public async Task<IEnumerable<ScheduleOrder>> GetByTowerAsync(int towerId)
        {
            var query = @"SELECT * FROM ScheduleOrder 
                          INNER JOIN Apartment ON ScheduleOrder.ApartmentID = Apartment.Id 
                          WHERE Apartment.TowerId = @TowerId";
            return await _dbConnection.QueryAsync<ScheduleOrder>(query, new { TowerId = towerId });
        }

        public async Task<IEnumerable<ScheduleOrder>> GetByUserAsync(int userId)
        {
            var query = "SELECT * FROM ScheduleOrder WHERE UserID = @UserId AND Status = @OpenedStatus";
            return await _dbConnection.QueryAsync<ScheduleOrder>(query, new { UserId = userId, OpenedStatus = (int)ScheduleStatus.Opened });
        }


        public async Task<bool> UpdateScheduleOrderAsync(int scheduleOrderId, ScheduleStatus status)
        {
            var query = @"UPDATE ScheduleOrder 
                  SET Status = @CanceledStatus 
                  WHERE Id = @ScheduleOrderId";

            int affectedRows = await _dbConnection.ExecuteAsync(query, new { ScheduleOrderId = scheduleOrderId, CanceledStatus = (int)status});

            return affectedRows > 0;
        }

    }
}
