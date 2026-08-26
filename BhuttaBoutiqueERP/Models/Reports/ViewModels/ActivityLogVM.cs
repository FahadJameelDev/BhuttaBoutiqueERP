using System;

namespace BhuttaBoutiqueERP.ViewModels.Reports
{
    public class ActivityLogVM
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; } = string.Empty;
    }
}