using System.Threading.Tasks;

namespace Your_Ride.Repository.ForgetPasswordRepo
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
