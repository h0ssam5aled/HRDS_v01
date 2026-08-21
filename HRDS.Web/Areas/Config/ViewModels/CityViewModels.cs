namespace HRDS.Web.Areas.Config.Models
{
    public class CityViewModel
    {
        public int CityId { get; set; }
        public int GovernorateId { get; set; }
        public string CityCode { get; set; } = string.Empty;
        public string CityNameAr { get; set; } = string.Empty;
        public string CityNameEn { get; set; } = string.Empty;
        public string? GovernorateNameAr { get; set; }
        public string? GovernorateNameEn { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
    }

    public class GovernorateLookupDto
    {
        public int GovernorateId { get; set; }
        public string GovernorateNameAr { get; set; } = string.Empty;
        public string GovernorateNameEn { get; set; } = string.Empty;
    }
}