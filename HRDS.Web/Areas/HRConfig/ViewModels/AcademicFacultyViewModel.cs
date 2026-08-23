using System.ComponentModel.DataAnnotations;

namespace HRDS.Web.Areas.HR.ViewModels
{
    public class AcademicFacultyViewModel
    {
        public int FacultyId { get; set; }

        [Required(ErrorMessage = "يرجى اختيار المؤسسة التعليمية")]
        public int InstitutionId { get; set; }

        public string? InstitutionNameAr { get; set; }
        public string? InstitutionNameEn { get; set; }

        [Required(ErrorMessage = "رمز الكلية مطلوب")]
        [StringLength(50)]
        public string FacultyCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم بالعربية مطلوب")]
        [StringLength(200)]
        public string FacultyNameAr { get; set; } = string.Empty;

        [StringLength(200)]
        public string? FacultyNameEn { get; set; }
    }
}
