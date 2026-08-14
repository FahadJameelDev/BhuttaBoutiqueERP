using System;

namespace BhuttaBoutiqueERP.Models.Auth.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        // Navigation Property
        public Role? Role { get; set; }
    }
}