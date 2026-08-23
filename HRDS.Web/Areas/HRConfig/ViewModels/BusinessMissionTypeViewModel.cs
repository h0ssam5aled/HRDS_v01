namespace HRDS.Web.Areas.HRConfig.ViewModels
{
    public class BusinessMissionTypeViewModel
    {
        public int MissionTypeId { get; set; }
        public string MissionTypeCode { get; set; } = null!;
        public string MissionTypeNameAr { get; set; } = null!;
        public string? MissionTypeNameEn { get; set; }
        public bool HasAllowance { get; set; }
        public bool IsActive { get; set; } = true;
    }
}