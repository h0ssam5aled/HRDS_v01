using System.ComponentModel.DataAnnotations;

namespace HRDS.Web.Models
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        [StringLength(254)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // UserAccess Details
        public int? CompanyId { get; set; }
        public int? CompanyBranchId { get; set; }

        // Selected Roles
        public List<int> SelectedRoleIds { get; set; } = new List<int>();
    }
}