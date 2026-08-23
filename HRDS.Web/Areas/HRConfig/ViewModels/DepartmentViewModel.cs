namespace HRDS.Web.Areas.HRConfig.ViewModels
{
    public class DepartmentViewModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; } = null!;
        public string DepartmentNameAr { get; set; } = null!;
        public string? DepartmentNameEn { get; set; }
        public int? CompanyId { get; set; }
        public int? CompanyBranchId { get; set; }
        public bool IsActive { get; set; }
    }
}