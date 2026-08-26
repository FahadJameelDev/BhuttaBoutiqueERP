using System;

namespace BhuttaBoutiqueERP.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty; // Create, Update, Delete, Login
        public string ModuleName { get; set; } = string.Empty; // Inventory, HR, Finance, etc.
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string IpAddress { get; set; } = string.Empty;
    }
}