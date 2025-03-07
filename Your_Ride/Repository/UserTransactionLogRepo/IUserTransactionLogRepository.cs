using Your_Ride.Models;

namespace Your_Ride.Repository.UserTransactionLogRepo
{
    public interface IUserTransactionLogRepository
    {
        public Task<List<UserTransactionLog>> GetAllUserTransactionLogs();
        public Task<UserTransactionLog> GetUserTransactionLogsById(int id);
        public Task<List<UserTransactionLog>> GetAllUserTransactionLogsByUserID(string id);
        public Task<List<UserTransactionLog>> GetAllUserTransactionLogsByTimeID(int id);
        public Task<List<UserTransactionLog>> GetAllUserTransactionLogsByAppointmentID(int id);
        public Task<UserTransactionLog> CreateUserTransactionLog(UserTransactionLog userTransactionLog);
        public Task<int> DeleteUserTransactionLog(int id);
        public Task<bool> CheckUserLogTransactionLog(UserTransactionLog userTransactionLog);

    }
}