namespace HRDS.Web.Areas.Finance.ViewModels
{
    public class CurrencyRateViewModel
    {
        public int CurrencyRateId { get; set; }
        public int CurrencyId { get; set; }
        public string? CurrencyName { get; set; }
        public int BaseCurrencyId { get; set; }
        public string? BaseCurrencyName { get; set; }
        public decimal ExchangeRate { get; set; }
        public string RateDate { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}