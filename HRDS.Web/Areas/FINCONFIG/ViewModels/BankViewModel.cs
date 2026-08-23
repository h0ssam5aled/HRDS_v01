namespace HRDS.Web.Areas.Finance.ViewModels
{
    public class BankViewModel
    {
        public int BankId { get; set; }
        public string BankCode { get; set; } = null!;
        public string BankNameAr { get; set; } = null!;
        public string? BankNameEn { get; set; }
        public string DisplayBankName { get; set; } = null!;
        public string? SwiftCode { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}