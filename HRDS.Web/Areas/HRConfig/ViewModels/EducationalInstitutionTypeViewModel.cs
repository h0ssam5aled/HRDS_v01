using System.ComponentModel.DataAnnotations;

namespace HRDS.Web.Areas.HR.ViewModels
{
    public class EducationalInstitutionTypeViewModel
    {
        public int InstitutionTypeId { get; set; }

        [Required(ErrorMessage = "رمز نوع المؤسسة مطلوب")]
        [StringLength(50)]
        public string InstitutionTypeCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم بالعربية مطلوب")]
        [StringLength(200)]
        public string InstitutionTypeNameAr { get; set; } = string.Empty;

        [StringLength(200)]
        public string? InstitutionTypeNameEn { get; set; }
    }
}
