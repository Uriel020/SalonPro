using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; } // Identificador único del usuario
        public string Username { get; set; } // Nombre de usuario
        public string Name { get; set; } // Nombre del usuario
        public string Lastname { get; set; } // Apellido del usuario
        public string Email { get; set; } // Correo electrónico
        public bool IsEmailConfirmed { get; set; } // Confirmación de correo electrónico
        public string Phone { get; set; } // Teléfono del usuario
        public string ResetPasswordToken { get; set; } // Token para restablecer la contraseña
        public DateTime ResetPasswordExpires { get; set; } // Fecha de expiración del token
        public bool IsPhoneConfirmed { get; set; } // Confirmación del teléfono
        public string Password { get; set; } // Contraseña del usuario (debe ser almacenada de forma segura)
        public string ImageUrl { get; set; } // URL de la imagen de perfil
        public ICollection<UserLikedProduct> LikedProducts { get; set; } // Relación con los productos que le gustan
    }
}
