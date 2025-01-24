using System.Collections.Generic;

namespace Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Relación con UserRole
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
