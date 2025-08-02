using Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.UserTransactionLogRepo
{
    public interface IUserTransactionLogRepository : IRepository<UserTransactionLog>
    {
        public Task<List<UserTransactionLog>> GetAllUserTransactionLogs();
        public Task<List<UserTransactionLog>> GetUserTransactionLogsWithSearch(int page, int pageSize, string searchQuery);
        public Task<int> GetTotalRecordsCount(string searchQuery);

        public Task<UserTransactionLog> GetUserTransactionLogsById(int id);
        public Task<List<UserTransactionLog>> GetAllUserTransactionLogsByUserID(string id);
        public Task<List<UserTransactionLog>> GetAllUserTransactionLogsByTimeID(int id);
        public Task<List<UserTransactionLog>> GetAllUserTransactionLogsByAppointmentID(int id);
        public Task<UserTransactionLog> CreateUserTransactionLog(UserTransactionLog userTransactionLog);
        public Task<UserTransactionLog> EditUserTransactionLog(UserTransactionLog userTransactionLog);

        public Task<int> DeleteUserTransactionLog(int id);
        public Task<bool> CheckUserLogTransactionLog(UserTransactionLog userTransactionLog);
        public Task<UserTransactionLog> GetUserTransactionLogsByTimeIdandUserId(int id, string userId);


    }
}