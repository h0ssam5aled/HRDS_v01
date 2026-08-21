namespace HRDS.Web.Areas.Config.Models
{
    public class MilitaryStatusViewModel
    {
        public int MilitaryStatusId { get; set; }
        public string MilitaryStatusCode { get; set; } = string.Empty;
        public string MilitaryStatusNameAr { get; set; } = string.Empty;
        public string MilitaryStatusNameEn { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
    }
}