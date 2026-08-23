namespace HRDS.Web.Areas.HR.ViewModels
{
    public class ShiftTypeViewModel
    {
        public int ShiftTypeId { get; set; }
        public string ShiftTypeCode { get; set; } = null!;
        public string ShiftTypeNameAr { get; set; } = null!;
        public string? ShiftTypeNameEn { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}