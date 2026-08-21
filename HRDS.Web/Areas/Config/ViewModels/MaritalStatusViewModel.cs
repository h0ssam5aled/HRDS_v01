namespace HRDS.Web.Areas.Config.Models
{
    public class MaritalStatusViewModel
    {
        public int MaritalStatusId { get; set; }
        public string MaritalStatusCode { get; set; } = string.Empty;
        public string MaritalStatusNameAr { get; set; } = string.Empty;
        public string MaritalStatusNameEn { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
    }
}