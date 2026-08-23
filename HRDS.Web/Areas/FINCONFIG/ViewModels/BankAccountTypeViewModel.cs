namespace HRDS.Web.Areas.Finance.ViewModels
{
    public class BankAccountTypeViewModel
    {
        public int BankAccountTypeId { get; set; }
        public string BankAccountTypeCode { get; set; } = null!;
        public string BankAccountTypeNameAr { get; set; } = null!;
        public string? BankAccountTypeNameEn { get; set; }
        public string DisplayBankAccountTypeName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}