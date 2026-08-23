using System.ComponentModel.DataAnnotations;

namespace HRDS.Web.Areas.Finance.ViewModels
{
    public class AccountViewModel
    {
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Account Type is required")]
        public int AccountTypeId { get; set; }
        public string? AccountTypeName { get; set; }

        public int? ParentAccountId { get; set; }
        public string? ParentAccountName { get; set; }

        public int? CurrencyId { get; set; }
        public string? CurrencyName { get; set; }

        [Required(ErrorMessage = "Account Code is required")]
        [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
        public string AccountCode { get; set; } = null!;

        [Required(ErrorMessage = "Arabic Name is required")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string AccountNameAr { get; set; } = null!;

        [StringLength(200, ErrorMessage = "English Name cannot exceed 200 characters")]
        public string? AccountNameEn { get; set; }

        public byte AccountLevel { get; set; }

        public bool IsLeaf { get; set; } = true;

        [Required(ErrorMessage = "Account Nature is required")]
        public string AccountNature { get; set; } = "Debit"; // Debit or Credit

        public string? HierarchyPath { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}