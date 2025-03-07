using System.ComponentModel.DataAnnotations;
using Your_Ride.ViewModels.AppointmentviewModel;
using Your_Ride.ViewModels.TimeViewModel;

namespace Your_Ride.ViewModels.UserTransactionLogViewModel
{
    public class CreateUserTransactionLogVM
    {
        [Required]
        public int AppointmentId { get; set; }

        public int? TimeId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive.")]
        public double? Fee { get; set; }
        public List<AppointmentVM>? Appointments { get; set; }
        public List<TimeVM>? Times { get; set; }
    }
}
