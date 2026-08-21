namespace HRDS.Web.Areas.Config.Models
{
    public class NationalityViewModel
    {
        public int NationalityId { get; set; }
        public string NationalityCode { get; set; } = string.Empty;
        public string NationalityNameAr { get; set; } = string.Empty;
        public string NationalityNameEn { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
    }
}