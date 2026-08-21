namespace HRDS.Web.Areas.Config.Models
{
    public class ReligionViewModel
    {
        public int ReligionId { get; set; }
        public string ReligionCode { get; set; } = string.Empty;
        public string ReligionNameAr { get; set; } = string.Empty;
        public string ReligionNameEn { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
    }
}