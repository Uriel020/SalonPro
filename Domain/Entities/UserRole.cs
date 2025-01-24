namespace Domain.Entities
{
    public class UserRole
    {
        public int UserId { get; set; } // Relación con el usuario
        public int RoleId { get; set; } // Relación con el rol

        public User User { get; set; } // Propiedad de navegación para el usuario
        public Role Role { get; set; } // Propiedad de navegación para el rol
    }
}
