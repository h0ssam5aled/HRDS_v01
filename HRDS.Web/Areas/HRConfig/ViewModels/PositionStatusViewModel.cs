namespace HRDS.Web.Areas.HR.ViewModels
{
    public class PositionStatusViewModel
    {
        public int PositionStatusId { get; set; }
        public string PositionStatusCode { get; set; } = null!;
        public string PositionStatusNameAr { get; set; } = null!;
        public string? PositionStatusNameEn { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}