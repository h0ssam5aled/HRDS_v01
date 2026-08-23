using System.ComponentModel.DataAnnotations;

namespace HRDS.Web.ViewModels
{
    public class LeaveCategoryViewModel
    {
        public int LeaveCategoryId { get; set; }

        [Required(ErrorMessage = "رمز الفئة مطلوب")]
        [StringLength(20)]
        public string LeaveCategoryCode { get; set; } = null!;

        [Required(ErrorMessage = "الاسم بالعربية مطلوب")]
        [StringLength(100)]
        public string LeaveCategoryNameAr { get; set; } = null!;

        [StringLength(100)]
        public string? LeaveCategoryNameEn { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}