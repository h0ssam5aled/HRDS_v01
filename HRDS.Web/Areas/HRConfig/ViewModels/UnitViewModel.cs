namespace HRDS.Web.Areas.HRConfig.ViewModels
{
    public class UnitViewModel
    {
        public int UnitId { get; set; }
        public int SectionId { get; set; }
        public string? SectionNameAr { get; set; }
        public string UnitCode { get; set; } = null!;
        public string UnitNameAr { get; set; } = null!;
        public string? UnitNameEn { get; set; }
        public int? DisplayOrder { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}