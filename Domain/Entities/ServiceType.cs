﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ServiceType
    {
        public int ServiceTypeId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Category Category { get; set; }
        public ICollection<Service>? Services { get; set; }
    }
}
