using Facilitat.EMAIL.Models.Entities;
using Facilitat.EMAIL.Repositories.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Facilitat.EMAIL.Models.DTOs;
using System.Linq;

namespace Facilitat.EMAIL.Repositories.Schedule
{
    public class ScheduleRepository : GenericRepository<ScheduleOrder>, IScheduleRepository
    {
        public ScheduleRepository(IDbConnection dbConnection)
            : base(dbConnection)
        {
        }

        public async Task<ScheduleEmailDTO> GetScheduleEmailDetailsAsync(ScheduleOrderDTO scheduleOrderDto)
        {
            string query = @"
                SELECT 
                    u.Email,
                    u.FirstName + ' ' + u.LastName as Username,
                    a.Number as Apartment,
                    t.Name as Tower,
                    so.ScheduledTime as Date
                FROM ScheduleOrder so
                INNER JOIN Users u ON so.UserID = u.Id
                INNER JOIN Apartment a ON so.ApartmentID = a.Id
                INNER JOIN Tower t ON a.TowerId = t.Id
                WHERE so.UserID = @UserId 
                  AND so.ApartmentID = @ApartmentId
                  AND so.ScheduledTime = @ScheduledTime
                ORDER BY so.ScheduledTime DESC";  // In case there are multiple records, order by date and take the latest

            var result = await _dbConnection.QueryAsync<ScheduleEmailDTO>(
                query,
                new
                {
                    UserId = scheduleOrderDto.UserId,
                    ApartmentId = scheduleOrderDto.ApartmentId,
                    ScheduledTime = scheduleOrderDto.Start
                });

            return result.FirstOrDefault();
        }

    }
}
