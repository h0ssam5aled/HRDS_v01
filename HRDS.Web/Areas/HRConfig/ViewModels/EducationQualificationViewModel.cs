namespace HRDS.Web.Areas.HR.ViewModels
{
    public class EducationQualificationViewModel
    {
        public int QualificationId { get; set; }
        public string QualificationCode { get; set; } = null!;
        public string QualificationNameAr { get; set; } = null!;
        public string? QualificationNameEn { get; set; }
    }
}