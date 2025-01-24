namespace Domain.Entities
{
    public class UserRole
    {
        public int UserId { get; set; } // Relaci�n con el usuario
        public int RoleId { get; set; } // Relaci�n con el rol

        public User User { get; set; } // Propiedad de navegaci�n para el usuario
        public Role Role { get; set; } // Propiedad de navegaci�n para el rol
    }
}
