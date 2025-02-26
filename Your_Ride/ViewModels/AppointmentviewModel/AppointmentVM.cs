using System.ComponentModel.DataAnnotations;
using Your_Ride.Models;

namespace Your_Ride.ViewModels.AppointmentviewModel
{
    public class AppointmentVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }
        public List<Time>? Times { get; set; }
        [Required(ErrorMessage ="Must insert Max Amount ")]
        [Range(0.01,double.MaxValue,ErrorMessage ="Must be more than Zero")]
        public double MaxAmount { get; set; }


  

        // Mark as completed when the appointment finishes
        public bool HasCompleted { get; set; } = false;
    }
}
