namespace HRDS.Web.Areas.Config.ViewModels
{
    public class CountriesViewModel
    {
        public int CountryId { get; set; }
        public string CountryCode2 { get; set; } = string.Empty;
        public string CountryCode3 { get; set; } = string.Empty;
        public string CountryNameAr { get; set; } = string.Empty;
        public string CountryNameEn { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
    }
}