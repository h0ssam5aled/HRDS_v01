namespace HRDS.Web.Models
{
    public class UserAccessItemViewModel
    {
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; } = string.Empty;

        public int? CompanyBranchId { get; set; }
        public string? BranchName { get; set; } = string.Empty;

        public bool IsSelected { get; set; }
        public bool IsDefault { get; set; }
    }
}