using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
namespace Your_Ride.Repository.ForgetPasswordRepo
{
    public class TwilioSmsSender : ISmsSender
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromNumber;

        public TwilioSmsSender(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _fromNumber = configuration["Twilio:FromNumber"];
            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task SendSmsAsync(string number, string message)
        {
            await MessageResource.CreateAsync(
                body: message,
                from: new PhoneNumber(_fromNumber),
                to: new PhoneNumber(number)
            );
        }
    }
}