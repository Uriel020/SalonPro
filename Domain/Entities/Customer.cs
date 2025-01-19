using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime LastVisit { get; set; }
        public int State { get; set; }
        public string Photo { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
