namespace HRDS.Web.Areas.HR.ViewModels
{
    public class AllowanceTypeViewModel
    {
        public int AllowanceTypeId { get; set; }
        public string AllowanceTypeCode { get; set; } = null!;
        public string AllowanceTypeNameAr { get; set; } = null!;
        public string? AllowanceTypeNameEn { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}