using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public int StaffId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string AppointmentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string Comment { get; set; }
        public int DurationInMinutes { get; set; }
        public DateTime CreatedDate { get; set; }

        public Customer Customer { get; set; }
        public Service Service { get; set; }
        public Staff Staff { get; set; }
    }
}
