using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Your_Ride.Models;
using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Repository.AppointmentRepo;
using Your_Ride.Repository.TimeRepo;
using Your_Ride.Repository.UserTransactionLogRepo;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.UserTransactionLogViewModel;

namespace Your_Ride.Services.UserTransactionLogServ
{
    public class UserTransactionLogService :Service<UserTransactionLog> , IUserTransactionLogService
    {
        private readonly IUserTransactionLogRepository userTransactionLogRepository;
        private readonly IMapper autoMapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITimeRepository timeRepository;
        private readonly IAppointmentRepository appointmentRepository;

        public UserTransactionLogService(IUserTransactionLogRepository userTransactionLogRepository , IMapper autoMapper , UserManager<ApplicationUser> userManager , ITimeRepository timeRepository , IAppointmentRepository appointmentRepository )
        {
            this.userTransactionLogRepository = userTransactionLogRepository;
            this.autoMapper = autoMapper;
            this.userManager = userManager;
            this.timeRepository = timeRepository;
            this.appointmentRepository = appointmentRepository;
        }

        public async Task<List<UserTransactionLogVM>> GetAllUserTransactioLogs()
        {
            List<UserTransactionLog> userTransactionLogs = await userTransactionLogRepository.GetAllUserTransactionLogs();

            List<UserTransactionLogVM> userTransactionLogVMs = autoMapper.Map <List<UserTransactionLogVM>> (userTransactionLogs );

            return userTransactionLogVMs;
        }
        public async Task<UserTransactionLogVM> GetUserTransactioLogById(int id)
        {
            UserTransactionLog userTransactionLog = await userTransactionLogRepository.GetUserTransactionLogsById(id);

            UserTransactionLogVM userTransactionLogVM = autoMapper.Map<UserTransactionLogVM>(userTransactionLog);

            return userTransactionLogVM;
        }

        public async Task<UserTransactionLogVM> GetUserTransactioLogByTimeIdUserId(int id , string userId)
        {
            UserTransactionLog userTransactionLog = await userTransactionLogRepository.GetUserTransactionLogsByTimeIdandUserId(id , userId);

            UserTransactionLogVM userTransactionLogVM = autoMapper.Map<UserTransactionLogVM>(userTransactionLog);

            return userTransactionLogVM;
        }
        public async Task<List<UserTransactionLogVM>> GetAllUserTransactioLogsByUserId( string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user == null) return null;

            List<UserTransactionLog> userTransactionLogs = await userTransactionLogRepository.GetAllUserTransactionLogsByUserID(id);

            List<UserTransactionLogVM> userTransactionLogVMs = autoMapper.Map<List<UserTransactionLogVM>>(userTransactionLogs);

            return userTransactionLogVMs;
        }
        public async Task<List<UserTransactionLogVM>> GetAllUserTransactioLogsByTimeId(int id)
        {
            Time time  = await timeRepository.GetTimeByID(id);
            if (time == null) return null;

            List<UserTransactionLog> userTransactionLogs = await userTransactionLogRepository.GetAllUserTransactionLogsByTimeID(id);

            List<UserTransactionLogVM> userTransactionLogVMs = autoMapper.Map<List<UserTransactionLogVM>>(userTransactionLogs);

            return userTransactionLogVMs;
        }
        public async Task<List<UserTransactionLogVM>> GetAllUserTransactioLogsByAppointmentId(int id)
        {
            Appointment appointment = await appointmentRepository.GetAppointmentByID(id);
            if (appointment == null) return null;

            List<UserTransactionLog> userTransactionLogs = await userTransactionLogRepository.GetAllUserTransactionLogsByAppointmentID(id);

            List<UserTransactionLogVM> userTransactionLogVMs = autoMapper.Map<List<UserTransactionLogVM>>(userTransactionLogs);

            return userTransactionLogVMs;
        }

        public async Task<UserTransactionLogVM> CreateUserTransactionLog(UserTransactionLogVM userTransactionVM)
        {

            Appointment appointment = await appointmentRepository.GetAppointmentByID(userTransactionVM.AppointmentId);
            Time time = await timeRepository.GetTimeByID(userTransactionVM.TimeId);
            ApplicationUser user = await userManager.FindByIdAsync(userTransactionVM.UserId);

            if (appointment == null || time == null || user == null) return null;
            UserTransactionLog userTransactionLog = autoMapper.Map<UserTransactionLog>(userTransactionVM);

            bool UserTransactioExist = await userTransactionLogRepository.CheckUserLogTransactionLog(userTransactionLog);
            if (UserTransactioExist == true) return null;

            UserTransactionLog NewuserTransactionLog = await userTransactionLogRepository.CreateUserTransactionLog(userTransactionLog);
            UserTransactionLogVM userTransactionLogVM = autoMapper.Map<UserTransactionLogVM>(NewuserTransactionLog);
            return userTransactionLogVM;

        }
        public async Task<UserTransactionLogVM> EditUserTransactionLog(UserTransactionLogVM userTransactionVM)
        {

            Appointment appointment = await appointmentRepository.GetAppointmentByID(userTransactionVM.AppointmentId);
            Time time = await timeRepository.GetTimeByID(userTransactionVM.TimeId);
            ApplicationUser user = await userManager.FindByIdAsync(userTransactionVM.UserId);

            if (appointment == null || time == null || user == null) return null;
            UserTransactionLog userTransactionLog = autoMapper.Map<UserTransactionLog>(userTransactionVM);

            //UserTransactionLog UserTransactioExist = await userTransactionLogRepository.GetUserTransactionLogsById(userTransactionLog.Id);
            //if (UserTransactioExist == null) return null;

            //UserTransactioExist.AppointmentId = userTransactionVM.AppointmentId;
            //UserTransactioExist.TimeId = userTransactionVM.TimeId;
            //UserTransactioExist.UserId = userTransactionVM.UserId;
            //UserTransactioExist.WithdrawalAmount = userTransactionVM.WithdrawalAmount;

            //UserTransactionLog userTransaction = autoMapper.Map<UserTransactionLog>(userTransactionVM);

            UserTransactionLog EditeduserTransactionLog =await userTransactionLogRepository.EditUserTransactionLog(userTransactionLog);
            if (EditeduserTransactionLog == null) return null;

            await userTransactionLogRepository.SaveDB();

            return autoMapper.Map<UserTransactionLogVM>(EditeduserTransactionLog);
        }

        public async Task<int> DeleteUserTransactionLog(int id)
        {
            int result = await userTransactionLogRepository.DeleteUserTransactionLog(id);
            return result;
        }

    }
}
