using System;
using System.Collections.Generic;

namespace BhuttaBoutiqueERP.Models.Auth.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Property
        public ICollection<User>? Users { get; set; }
    }
}