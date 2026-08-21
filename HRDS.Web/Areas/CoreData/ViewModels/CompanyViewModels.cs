using System.ComponentModel.DataAnnotations;

namespace HRDS.Web.Models
{
    public class CompanyViewModel
    {
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "كود الشركة مطلوب")]
        [StringLength(20)]
        public string CompanyCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم الشركة بالعربية مطلوب")]
        [StringLength(200)]
        public string CompanyNameAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم الشركة بالإنجليزية مطلوب")]
        [StringLength(200)]
        public string CompanyNameEn { get; set; } = string.Empty;

        public string? TaxNumber { get; set; }
        public string? CommercialRegister { get; set; }

        public int CountryId { get; set; }
        public int GovernorateId { get; set; }
        public int CityId { get; set; }

        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class CompanyBranchViewModel
    {
        public int CompanyBranchId { get; set; }

        [Required(ErrorMessage = "اختر الشركة")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "كود الفرع مطلوب")]
        [StringLength(20)]
        public string BranchCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم الفرع بالعربية مطلوب")]
        [StringLength(200)]
        public string BranchNameAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم الفرع بالإنجليزية مطلوب")]
        [StringLength(200)]
        public string BranchNameEn { get; set; } = string.Empty;

        public int CountryId { get; set; }
        public int GovernorateId { get; set; }
        public int CityId { get; set; }

        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public bool IsMainBranch { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}