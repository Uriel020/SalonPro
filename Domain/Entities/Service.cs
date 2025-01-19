using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Service
    {
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public bool State { get; set; }

        public ServiceType ServiceType { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<StaffService> StaffServices { get; set; }
    }
}
