namespace HRDS.Web.Areas.HRConfig.ViewModels
{
    public class ExpenseTypeViewModel
    {
        public int ExpenseTypeId { get; set; }
        public string ExpenseTypeCode { get; set; } = null!;
        public string ExpenseTypeNameAr { get; set; } = null!;
        public string? ExpenseTypeNameEn { get; set; }
        public string? Description { get; set; }
        public decimal? MaxLimit { get; set; }
        public bool RequiresAttachment { get; set; }
        public bool IsActive { get; set; } = true;
    }
}