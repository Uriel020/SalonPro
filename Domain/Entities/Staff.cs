using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Staff
    {
        public int StaffID { get; set; }
        public string Name { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int State { get; set; }
        public DateTime BirthDate { get; set; }
        public int PhoneNumber { get; set; }
        public string Speciality { get; set; }
        public string AccessType { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<StaffService> StaffServices { get; set; }
    }
}
