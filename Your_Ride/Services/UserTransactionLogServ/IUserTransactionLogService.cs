using Your_Ride.Models;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.UserTransactionLogViewModel;

namespace Your_Ride.Services.UserTransactionLogServ
{
    public  interface IUserTransactionLogService :IService<UserTransactionLog>
    {
        public Task<List<UserTransactionLogVM>> GetAllUserTransactioLogs();
        public Task<UserTransactionLogVM> GetUserTransactioLogById(int id);

        public Task<List<UserTransactionLogVM>> GetAllUserTransactioLogsByUserId(string id);
        public Task<List<UserTransactionLogVM>> GetAllUserTransactioLogsByTimeId(int id);
        public Task<List<UserTransactionLogVM>> GetAllUserTransactioLogsByAppointmentId(int id);
        public Task<UserTransactionLogVM> CreateUserTransactionLog(UserTransactionLogVM userTransactionVM);
        public Task<UserTransactionLogVM> EditUserTransactionLog(UserTransactionLogVM userTransactionVM);

        public Task<int> DeleteUserTransactionLog(int id);
        public Task<UserTransactionLogVM> GetUserTransactioLogByTimeIdUserId(int id, string userId);

    }
}