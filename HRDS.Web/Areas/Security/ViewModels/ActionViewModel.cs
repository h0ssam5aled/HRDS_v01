namespace HRDS.Web.Models
{
    public class ActionViewModel
    {
        public int ActionId { get; set; }
        public string ActionCode { get; set; } = string.Empty;
        public string ActionNameAr { get; set; } = string.Empty;
        public string ActionNameEn { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}