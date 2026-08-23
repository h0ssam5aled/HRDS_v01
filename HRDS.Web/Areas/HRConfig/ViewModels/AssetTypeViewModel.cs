namespace HRDS.Web.Areas.HR.ViewModels
{
    public class AssetTypeViewModel
    {
        public int AssetTypeId { get; set; }
        public string AssetTypeCode { get; set; } = null!;
        public string AssetTypeNameAr { get; set; } = null!;
        public string? AssetTypeNameEn { get; set; }
        public bool IsActive { get; set; }
    }
}