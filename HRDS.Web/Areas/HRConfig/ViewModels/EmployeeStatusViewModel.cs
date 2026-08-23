namespace HRDS.Web.Areas.HR.ViewModels
{
    public class EmployeeStatusViewModel
    {
        public int EmployeeStatusId { get; set; }
        public string EmployeeStatusCode { get; set; } = null!;
        public string EmployeeStatusNameAr { get; set; } = null!;
        public string? EmployeeStatusNameEn { get; set; }
        public string? Description { get; set; }
    }
}