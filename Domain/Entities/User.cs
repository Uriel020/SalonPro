using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; } // Identificador �nico del usuario
        public string Username { get; set; } // Nombre de usuario
        public string Name { get; set; } // Nombre del usuario
        public string Lastname { get; set; } // Apellido del usuario
        public string Email { get; set; } // Correo electr�nico
        public bool IsEmailConfirmed { get; set; } // Confirmaci�n de correo electr�nico
        public string Phone { get; set; } // Tel�fono del usuario
        public string ResetPasswordToken { get; set; } // Token para restablecer la contrase�a
        public DateTime ResetPasswordExpires { get; set; } // Fecha de expiraci�n del token
        public bool IsPhoneConfirmed { get; set; } // Confirmaci�n del tel�fono
        public string Password { get; set; } // Contrase�a del usuario (debe ser almacenada de forma segura)
        public string ImageUrl { get; set; } // URL de la imagen de perfil
        public ICollection<UserLikedProduct> LikedProducts { get; set; } // Relaci�n con los productos que le gustan
    }
}
