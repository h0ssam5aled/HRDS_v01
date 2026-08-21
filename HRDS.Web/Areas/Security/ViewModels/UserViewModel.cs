namespace HRDS.Web.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public string Roles { get; set; } = string.Empty; // تجميع الأدمج والـ Roles المربوطة بالمستخدم
    }
}