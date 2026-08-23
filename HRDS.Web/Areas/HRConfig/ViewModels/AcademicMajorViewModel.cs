using System.ComponentModel.DataAnnotations;

namespace HRDS.Web.Areas.HR.ViewModels
{
    public class AcademicMajorViewModel
    {
        public int MajorId { get; set; }

        [Required(ErrorMessage = "يرجى اختيار الكلية")]
        public int FacultyId { get; set; }

        public string? FacultyNameAr { get; set; }
        public string? FacultyNameEn { get; set; }

        [Required(ErrorMessage = "رمز التخصص مطلوب")]
        [StringLength(50)]
        public string MajorCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم بالعربية مطلوب")]
        [StringLength(200)]
        public string MajorNameAr { get; set; } = string.Empty;

        [StringLength(200)]
        public string? MajorNameEn { get; set; }
    }
}
