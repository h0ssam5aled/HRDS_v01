using System.ComponentModel.DataAnnotations;

namespace HRDS.Web.Models
{
    public class ModuleViewModel
    {
        public int ModuleId { get; set; }

        [Required(ErrorMessage = "رمز الموديول مطلوب")]
        [StringLength(50, ErrorMessage = "رمز الموديول يجب ألا يتجاوز 50 حرفاً")]
        [Display(Name = "رمز الموديول")]
        public string ModuleCode { get; set; } = null!;

        [Required(ErrorMessage = "اسم الموديول بالعربية مطلوب")]
        [StringLength(200, ErrorMessage = "الاسم بالعربية يجب ألا يتجاوز 200 حرف")]
        [Display(Name = "الاسم بالعربية")]
        public string ModuleNameAr { get; set; } = null!;

        [Required(ErrorMessage = "اسم الموديول بالإنجليزية مطلوب")]
        [StringLength(200, ErrorMessage = "الاسم بالإنجليزية يجب ألا يتجاوز 200 حرف")]
        [Display(Name = "الاسم بالإنجليزية")]
        public string ModuleNameEn { get; set; } = null!;

        [StringLength(300, ErrorMessage = "الوصف يجب ألا يتجاوز 300 حرف")]
        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Display(Name = "مفعل")]
        public bool IsActive { get; set; } = true;
    }
}