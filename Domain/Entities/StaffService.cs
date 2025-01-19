using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class StaffService
    {
        public int StaffId { get; set; }
        public int ServiceId { get; set; }

        public Staff Staff { get; set; }
        public Service Service { get; set; }
    }
}
