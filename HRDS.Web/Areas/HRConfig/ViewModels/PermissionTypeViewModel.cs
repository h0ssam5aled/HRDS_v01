namespace HRDS.Web.Areas.HRConfig.ViewModels
{
    public class PermissionTypeViewModel
    {
        public int PermissionTypeId { get; set; }
        public string PermissionTypeCode { get; set; } = null!;
        public string PermissionTypeNameAr { get; set; } = null!;
        public string? PermissionTypeNameEn { get; set; }
        public decimal? MaxHoursPerMonth { get; set; }
        public short? MaxCountPerMonth { get; set; }
        public bool DeductFromSalary { get; set; }
        public bool DeductFromLeaveBalance { get; set; }
        public bool RequiresAttachment { get; set; }
        public bool IsActive { get; set; } = true;
    }
}