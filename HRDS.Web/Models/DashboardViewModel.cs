namespace HRDS.Web.Models
{
    public class DashboardViewModel
    {
        public List<ModuleTileDto> AllowedModules { get; set; } = new List<ModuleTileDto>();
    }

    public class ModuleTileDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string IconClass { get; set; } = string.Empty;
        public string Color { get; set; } = "#3b82f6";
        public string ActionText { get; set; } = "فتح الوحدة";
    }
}