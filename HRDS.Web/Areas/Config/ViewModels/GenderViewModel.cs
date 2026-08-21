namespace HRDS.Web.Areas.Config.Models
{
    public class GenderViewModel
    {
        public int GenderId { get; set; }
        public string GenderCode { get; set; } = string.Empty;
        public string GenderNameAr { get; set; } = string.Empty;
        public string GenderNameEn { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
    }
}