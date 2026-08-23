namespace HRDS.Web.Areas.Finance.ViewModels
{
    public class AccountTypeViewModel
    {
        public int AccountTypeId { get; set; }
        public string Code { get; set; } = null!;
        public string AccountTypeNameAr { get; set; } = null!;
        public string? AccountTypeNameEn { get; set; }
        public string DisplayAccountTypeName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}