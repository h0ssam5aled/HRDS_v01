namespace HRDS.Web.Areas.HR.ViewModels
{
    public class SafetyTypeViewModel
    {
        public int SafetyTypeId { get; set; }
        public string SafetyTypeCode { get; set; } = null!;
        public string SafetyTypeNameAr { get; set; } = null!;
        public string? SafetyTypeNameEn { get; set; }
        public byte? SeverityLevel { get; set; }
        public bool IsActive { get; set; }
    }
}