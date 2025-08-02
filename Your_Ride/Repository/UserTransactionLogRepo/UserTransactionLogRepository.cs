using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Your_Ride.Models;
using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.UserTransactionLogRepo
{
    public class UserTransactionLogRepository :Repository<UserTransactionLog> , IUserTransactionLogRepository
    {
        private Context context;
        public UserTransactionLogRepository(Context context) : base(context)
        {
            this.context = context;
        }
        public async Task<List<UserTransactionLog>> GetAllUserTransactionLogs()
        {

            List<UserTransactionLog> userTransactionLogs = await context.userTransactionLogs.Include(x=>x.User).Include(x=>x.Appointment).Include(x=>x.Time).ToListAsync();
            return userTransactionLogs;

        }
        public async Task<List<UserTransactionLog>> GetUserTransactionLogsWithSearch(int page, int pageSize, string searchQuery)
        {
            var query = context.userTransactionLogs.Include(x => x.User)
                                                   .Include(x => x.Appointment)
                                                   .Include(x => x.Time)
                                                   .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(x => x.User.UserName.Contains(searchQuery) ||
                                         x.WithdrawalAmount.ToString().Contains(searchQuery) ||
                                         x.TransactionTime.ToString().Contains(searchQuery) ||
                                         x.Appointment.Date.ToString().Contains(searchQuery));
            }

            return await query.Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        }
        public async Task<int> GetTotalRecordsCount(string searchQuery)
        {
            var query = context.userTransactionLogs.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(x => x.User.UserName.Contains(searchQuery) ||
                                         x.WithdrawalAmount.ToString().Contains(searchQuery) ||
                                         x.TransactionTime.ToString().Contains(searchQuery) ||
                                         x.Appointment.Date.ToString().Contains(searchQuery));
            }

            return await query.CountAsync();
        }

        public async Task<UserTransactionLog> GetUserTransactionLogsById(int id)
        {

            UserTransactionLog userTransactionLog = await context.userTransactionLogs.Include(x => x.User).Include(x => x.Appointment).Include(x => x.Time).FirstOrDefaultAsync(x=>x.Id==id);
            return userTransactionLog;

        }
        public async Task<List<UserTransactionLog>> GetAllUserTransactionLogsByUserID( string id)
        {

            List<UserTransactionLog> userTransactionLogs = await context.userTransactionLogs.Where(x => x.UserId == id).Include(x => x.User).Include(x => x.Appointment).Include(x => x.Time).ToListAsync();
            return userTransactionLogs;

        }
        public async Task<UserTransactionLog> GetUserTransactionLogsByTimeIdandUserId(int id , string userId)
        {
            UserTransactionLog userTransactionLog = await context.userTransactionLogs.Include(x => x.User).Include(x => x.Appointment).Include(x => x.Time).FirstOrDefaultAsync(x => x.TimeId == id && x.UserId==userId);
            return userTransactionLog;
        }
        public async Task<List<UserTransactionLog>> GetAllUserTransactionLogsByTimeID(int id)
        {

            List<UserTransactionLog> userTransactionLogs = await context.userTransactionLogs.Where(x => x.TimeId == id).Include(x => x.User).Include(x => x.Appointment).Include(x => x.Time).ToListAsync();
            return userTransactionLogs;

        }
        public async Task<List<UserTransactionLog>> GetAllUserTransactionLogsByAppointmentID(int id)
        {

            List<UserTransactionLog> userTransactionLogs = await context.userTransactionLogs.Where(x => x.AppointmentId == id).Include(x => x.User).Include(x => x.Appointment).Include(x => x.Time).ToListAsync();
            return userTransactionLogs;

        }
        public async Task<UserTransactionLog> CreateUserTransactionLog(UserTransactionLog userTransactionLog)
        {
            // Get the user's wallet
            Wallet wallet = await context.Wallets.FirstOrDefaultAsync(x => x.UserId == userTransactionLog.UserId);

            // Get the appointment
            Appointment appointment = await context.Appointments.FirstOrDefaultAsync(x => x.Id == userTransactionLog.AppointmentId);

            // Calculate total withdrawn amount for this user in the same appointment
            double totalUserWithdrawals = await context.userTransactionLogs
                .Where(x => x.AppointmentId == userTransactionLog.AppointmentId && x.UserId == userTransactionLog.UserId)
                .SumAsync(x => x.WithdrawalAmount);

            // Ensure wallet and appointment exist
            if (wallet == null || appointment == null || wallet.Amount == 0.0)
            {
                return null;
            }

            // Ensure wallet has enough funds
            if (wallet.Amount < userTransactionLog.WithdrawalAmount)
            {
                return null; // Not enough funds
            }

            // Ensure total withdrawals do not exceed the max appointment amount
            if ((totalUserWithdrawals + userTransactionLog.WithdrawalAmount) > appointment.MaxAmount)
            {
                return null; // Exceeds max allowed withdrawal
            }

            // Deduct amount from wallet and save transaction
            wallet.Amount -= userTransactionLog.WithdrawalAmount;
            await context.AddAsync(userTransactionLog);
            await context.SaveChangesAsync();

            return userTransactionLog; // Successfully created transaction
        }
        public async Task<UserTransactionLog> EditUserTransactionLog(UserTransactionLog userTransactionLog)
        {
            // Get the user's wallet
            Wallet wallet = await context.Wallets.FirstOrDefaultAsync(x => x.UserId == userTransactionLog.UserId);

            // Get the appointment
            Appointment appointment = await context.Appointments.FirstOrDefaultAsync(x => x.Id == userTransactionLog.AppointmentId);

            // Get the existing transaction log from DB
            UserTransactionLog userTransactionLogFromDB = await context.userTransactionLogs
                .FirstOrDefaultAsync(x => x.Id == userTransactionLog.Id);

            if (wallet == null || appointment == null || userTransactionLogFromDB == null)
            {
                return null; // Return null if any required data is missing
            }

            userTransactionLogFromDB.AppointmentId = userTransactionLog.AppointmentId;
            userTransactionLogFromDB.TimeId = userTransactionLog.TimeId;
            userTransactionLogFromDB.UserId = userTransactionLog.UserId;
            //userTransactionLogFromDB.WithdrawalAmount = userTransactionVM.WithdrawalAmount;



            double currentWithdraw = userTransactionLogFromDB.WithdrawalAmount; // Old withdrawal
            double newWithdraw = userTransactionLog.WithdrawalAmount; // New withdrawal

            double withdrawDifference = newWithdraw - currentWithdraw; // Change in withdrawal amount

            // Calculate total withdrawn amount excluding the current transaction
            double totalUserWithdrawals = await context.userTransactionLogs
                .Where(x => x.AppointmentId == userTransactionLog.AppointmentId && x.UserId == userTransactionLog.UserId && x.Id != userTransactionLog.Id)
                .SumAsync(x => x.WithdrawalAmount);

            // Calculate the new total if we apply the new withdrawal amount
            double newTotalWithdrawals = totalUserWithdrawals + newWithdraw;

            // Ensure wallet has enough funds to cover only the difference
            if (wallet.Amount < withdrawDifference)
            {
                return null; // Not enough funds in wallet
            }

            // Ensure the new total withdrawals do not exceed the appointment max amount
            if (newTotalWithdrawals > appointment.MaxAmount)
            {
                return null; // Exceeds max allowed withdrawal
            }

            // Update wallet balance
            wallet.Amount -= withdrawDifference; // Adjust only the difference

            // Update transaction log with new withdrawal amount
            userTransactionLogFromDB.WithdrawalAmount = newWithdraw;
            context.Update(userTransactionLogFromDB);

            await context.SaveChangesAsync();

            return userTransactionLogFromDB; // Successfully updated transaction
        }


        public async Task<int> DeleteUserTransactionLog(int id)
        {
            UserTransactionLog userTransactionLog = await context.userTransactionLogs.FirstOrDefaultAsync(x => x.Id == id);

            if (userTransactionLog == null)
            {
                return -1;

            }
            else if (userTransactionLog.IsDeleted == true)
            {
                return 0;
            }
            else
            {
                userTransactionLog.IsDeleted = true;
                await SaveDB();
                return 1;
            }
        }

        public async Task<bool> CheckUserLogTransactionLog(UserTransactionLog userTransactionLog)
        {
            UserTransactionLog userTransactionLogFromDB = await context.userTransactionLogs.FirstOrDefaultAsync(x => x.AppointmentId == userTransactionLog.AppointmentId && x.TimeId==userTransactionLog.TimeId  && x.UserId==userTransactionLog.UserId);
            if (userTransactionLogFromDB == null)
            {
                return false;
            }
            else
            {
                return true; 
            }
        }
    }
}
