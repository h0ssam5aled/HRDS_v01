namespace HRDS.Web.Areas.HR.ViewModels
{
    public class EducationGradeViewModel
    {
        public decimal GradeId { get; set; }
        public string GradeCode { get; set; } = null!;
        public string GradeNameAr { get; set; } = null!;
        public string? GradeNameEn { get; set; }
    }
}