namespace HRDS.Web.Areas.HRConfig.ViewModels
{
    public class SectionViewModel
    {
        public int SectionId { get; set; }
        public string SectionCode { get; set; } = null!;
        public string SectionNameAr { get; set; } = null!;
        public string? SectionNameEn { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentNameAr { get; set; }
        public string? DepartmentNameEn { get; set; } // أضف هذا السطر
        public bool IsActive { get; set; }
    }
}