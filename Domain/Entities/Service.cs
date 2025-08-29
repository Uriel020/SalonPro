using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Service
    {
        public int Id { get; set; } // Identificador único del servicio
        public int ServiceTypeId { get; set; } // Relación con el tipo de servicio
        public string Description { get; set; } // Descripción del servicio
        public decimal Price { get; set; } // Precio del servicio
        public bool IsActive { get; set; } // Estado del servicio (activo/inactivo)

        public ServiceType ServiceType { get; set; } // Propiedad de navegación para el tipo de servicio
        public ICollection<Appointment> Appointments { get; set; } // Relación con las citas
    }
}