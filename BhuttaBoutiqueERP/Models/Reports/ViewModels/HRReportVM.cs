namespace BhuttaBoutiqueERP.ViewModels.Reports
{
    public class HRReportVM
    {
        public int TotalEmployees { get; set; }
        public int PresentTodayCount { get; set; }
        public decimal TotalMonthlyPayroll { get; set; }
        public List<HREmployeeRowVM> Items { get; set; } = new();
    }

    public class HREmployeeRowVM
    {
        public int Id { get; set; }
        public required string EmployeeName { get; set; }
        public required string Designation { get; set; }
        public required string Phone { get; set; }
        public required string AttendanceStatus { get; set; }
    }
}