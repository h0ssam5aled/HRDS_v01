using System.ComponentModel.DataAnnotations;

namespace HRDS.Web.Models
{
    public class ModelViewModel
    {
        public int ModelId { get; set; }

        [Required(ErrorMessage = "يرجى اختيار الموديول")]
        public int ModuleId { get; set; }

        public string? ModuleNameAr { get; set; }
        public string? ModuleNameEn { get; set; }

        [Required(ErrorMessage = "رمز الشاشة مطلوب")]
        [StringLength(100, ErrorMessage = "رمز الشاشة لا يتجاوز 100 حرف")]
        public string ModelCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم الشاشة بالعربية مطلوب")]
        [StringLength(200, ErrorMessage = "الاسم بالعربية لا يتجاوز 200 حرف")]
        public string ModelNameAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم الشاشة بالإنجليزية مطلوب")]
        [StringLength(200, ErrorMessage = "الاسم بالإنجليزية لا يتجاوز 200 حرف")]
        public string ModelNameEn { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "الوصف لا يتجاوز 300 حرف")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}