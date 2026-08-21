using System.ComponentModel.DataAnnotations;

namespace HRDS.Web.Models
{
    public class EditUserViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        [StringLength(254)]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; } // اختياري في حالة التغيير فقط

        public bool IsActive { get; set; }

        public int? CompanyId { get; set; }
        public int? CompanyBranchId { get; set; }

        public List<int> SelectedRoleIds { get; set; } = new List<int>();

        public List<UserAccessItemViewModel> UserBranchesAccess { get; set; } = new();
    }
}