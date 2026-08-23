namespace HRDS.Web.Areas.HR.ViewModels
{
    public class DocumentTypeViewModel
    {
        public int DocumentTypeId { get; set; }
        public string TypeCode { get; set; } = null!;
        public string TypeNameAr { get; set; } = null!;
        public string? TypeNameEn { get; set; }
        public bool IsExpiryRequired { get; set; }
        public int? ExpiryAlertDays { get; set; }
        public bool IsMandatory { get; set; }
    }
}