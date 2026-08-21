using System.ComponentModel.DataAnnotations;

namespace HRDS.Web.Areas.CoreData.ViewModels
{
    public class CompanyBranchViewModel
    {
        public int CompanyBranchId { get; set; }

        [Required(ErrorMessage = "إجباري")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "إجباري")]
        [StringLength(20)]
        public string BranchCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "إجباري")]
        [StringLength(200)]
        public string BranchNameAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "إجباري")]
        [StringLength(200)]
        public string BranchNameEn { get; set; } = string.Empty;

        [Required(ErrorMessage = "إجباري")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "إجباري")]
        public int GovernorateId { get; set; }

        [Required(ErrorMessage = "إجباري")]
        public int CityId { get; set; }

        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool IsMainBranch { get; set; }
        public bool IsActive { get; set; } = true;
    }
}