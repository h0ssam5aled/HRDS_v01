namespace HRDS.Web.Areas.Config.Models
{
    public class GovernorateViewModel
    {
        public int GovernorateId { get; set; }
        public int CountryId { get; set; }
        public string GovernorateCode { get; set; } = string.Empty;
        public string GovernorateNameAr { get; set; } = string.Empty;
        public string GovernorateNameEn { get; set; } = string.Empty;
        public string? CountryNameAr { get; set; }
        public string? CountryNameEn { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
    }

    public class CountryLookupDto
    {
        public int CountryId { get; set; }
        public string CountryNameAr { get; set; } = string.Empty;
        public string CountryNameEn { get; set; } = string.Empty;
    }
}