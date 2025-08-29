using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ServiceType
    {
        public int Id { get; set; } // Identificador único del tipo de servicio
        public int CategoryId { get; set; } // Relación con la categoría
        public string Name { get; set; } // Nombre del tipo de servicio
        public string Description { get; set; } // Descripción del tipo de servicio

        public Category Category { get; set; } // Propiedad de navegación para la relación con la categoría
        public ICollection<Service> Services { get; set; } // Relación con los servicios
    }
}
