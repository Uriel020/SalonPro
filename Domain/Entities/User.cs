using System;
using Domain.Entities;
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string Phone { get; set; }
    public string ResetPasswordToken { get; set; }
    public DateTime ResetPasswordExpires { get; set; }
    public bool PhoneConfirmed { get; set; }
    public string Password { get; set; }
    public string ImageUrl { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<UserLikedProduct> LikedProducts { get; set; } = new List<UserLikedProduct>();

    // Relación con Appointment
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    // Relación con Invoice
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    // Relación con ShoppingCart
    public ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}
