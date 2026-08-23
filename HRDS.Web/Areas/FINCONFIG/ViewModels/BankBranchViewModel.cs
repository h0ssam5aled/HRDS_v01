using System.ComponentModel.DataAnnotations;

namespace HRDS.Web.Areas.Finance.ViewModels
{
    public class BankBranchViewModel
    {
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Bank is required")]
        public int BankId { get; set; }

        public string? BankName { get; set; }

        [Required(ErrorMessage = "Branch Code is required")]
        [StringLength(20, ErrorMessage = "Code cannot exceed 20 characters")]
        public string BankBranchCode { get; set; } = null!;

        [Required(ErrorMessage = "Arabic Name is required")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string BankBranchNameAr { get; set; } = null!;

        [StringLength(200, ErrorMessage = "English Name cannot exceed 200 characters")]
        public string? BankBranchNameEn { get; set; }

        public string? BankBranchAddress { get; set; }

        public string? BankBranchPhone { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}