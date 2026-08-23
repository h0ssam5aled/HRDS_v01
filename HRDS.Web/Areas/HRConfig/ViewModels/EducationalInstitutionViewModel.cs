using System.ComponentModel.DataAnnotations;

namespace HRDS.Web.Areas.HR.ViewModels
{
    public class EducationalInstitutionViewModel
    {
        public int InstitutionId { get; set; }

        [Required(ErrorMessage = "يرجى اختيار نوع المؤسسة")]
        public int InstitutionTypeId { get; set; }

        public string? InstitutionTypeNameAr { get; set; }
        public string? InstitutionTypeNameEn { get; set; }

        [Required(ErrorMessage = "رمز المؤسسة مطلوب")]
        [StringLength(50)]
        public string InstitutionCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم بالعربية مطلوب")]
        [StringLength(200)]
        public string InstitutionNameAr { get; set; } = string.Empty;

        [StringLength(200)]
        public string? InstitutionNameEn { get; set; }
    }
}
