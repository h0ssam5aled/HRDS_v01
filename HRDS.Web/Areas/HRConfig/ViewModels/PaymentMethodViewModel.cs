namespace HRDS.Web.Areas.HRConfig.ViewModels
{
    public class PaymentMethodViewModel
    {
        public int PaymentMethodId { get; set; }
        public string PaymentMethodCode { get; set; } = null!;
        public string PaymentMethodNameAr { get; set; } = null!;
        public string? PaymentMethodNameEn { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}