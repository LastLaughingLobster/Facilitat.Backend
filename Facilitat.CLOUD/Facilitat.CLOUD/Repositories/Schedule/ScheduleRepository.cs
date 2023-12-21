using Facilitat.CLOUD.Models.Entities;
using Facilitat.CLOUD.Repositories.Generic;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Facilitat.CLOUD.Models.Enums;
using Facilitat.CLOUD.Models.DTOs;
using System;

namespace Facilitat.CLOUD.Repositories.Schedule
{
    public class ScheduleRepository : GenericRepository<ScheduleOrder>, IScheduleRepository
    {
        public ScheduleRepository(IDbConnection dbConnection)
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

        public async Task<IEnumerable<GridScheduleDTO>> GetByTowerForGridAsync(int towerId)
        {
            var query = @"
                SELECT 
                    SO.Id AS ScheduleId, 
                    A.Number AS ApartmentNumber, 
                    U.FirstName + ' ' + U.LastName AS UserName, 
                    SO.Title, 
                    CONVERT(VARCHAR, SO.ScheduledTime, 23) AS Date, 
                    CONVERT(VARCHAR(5), SO.ScheduledTime, 108) AS Time, 
                    SO.Description, 
                    SO.Status, 
                    '' AS Encoding
                FROM ScheduleOrder SO
                INNER JOIN Apartment A ON SO.ApartmentID = A.Id
                INNER JOIN Users U ON SO.UserID = U.Id
                WHERE A.TowerId = @TowerId
                ORDER BY A.Number, SO.ScheduledTime, U.FirstName, U.LastName;";

            return await _dbConnection.QueryAsync<GridScheduleDTO>(query, new { TowerId = towerId });
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

        public async Task<bool> CreateScheduleOrderAsync(ScheduleOrderDTO scheduleOrderDTO)
        {
            var query = @"
            INSERT INTO ScheduleOrder (
                ApartmentID, UserID, Title, ScheduledTime, EndTime, Description, RecurrenceRule, RecurrenceException, IsAllDay, Status
            ) VALUES (
                @ApartmentId, @UserId,  @Title, @Start, @End, @Description, @RecurrenceRule, @RecurrenceException, @IsAllDay, @Status
            )";

            int affectedRows = await _dbConnection.ExecuteAsync(query, new
            {
                ApartmentId = scheduleOrderDTO.ApartmentId,
                UserId = scheduleOrderDTO.UserId,
                Title = scheduleOrderDTO.Title,
                Start = scheduleOrderDTO.Start,
                End = scheduleOrderDTO.End,
                Description = scheduleOrderDTO.Description,
                RecurrenceRule = scheduleOrderDTO.RecurrenceRule,
                RecurrenceException = scheduleOrderDTO.RecurrenceException,
                IsAllDay = scheduleOrderDTO.IsAllDay,
                Status = (int)ScheduleStatus.Opened
            });

            return affectedRows > 0;
        }

        public async Task<bool> UpdateScheduleOrderAsync(ScheduleOrderDTO scheduleOrderDTO)
        {
            var query = @"
                UPDATE ScheduleOrder
                SET
                    Title = @Title, 
                    ScheduledTime = @Start, 
                    EndTime = @End, 
                    Description = @Description, 
                    RecurrenceRule = @RecurrenceRule, 
                    RecurrenceException = @RecurrenceException, 
                    IsAllDay = @IsAllDay, 
                    Status = @Status
                WHERE Id = @Id";

            int affectedRows = await _dbConnection.ExecuteAsync(query, new
            {
                Id = scheduleOrderDTO.Id,
                Title = scheduleOrderDTO.Title,
                Start = scheduleOrderDTO.Start,
                End = scheduleOrderDTO.End,
                Description = scheduleOrderDTO.Description,
                RecurrenceRule = scheduleOrderDTO.RecurrenceRule,
                RecurrenceException = scheduleOrderDTO.RecurrenceException,
                IsAllDay = scheduleOrderDTO.IsAllDay,
                Status = (int)scheduleOrderDTO.Status
            });

            return affectedRows > 0;
        }

    }
}
