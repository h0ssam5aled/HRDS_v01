namespace HRDS.Web.Models
{
    public class SidebarMenuViewModel
    {
        public List<ModuleMenuItemDto> Modules { get; set; } = new();
    }

    public class ModuleMenuItemDto
    {
        public int ModuleId { get; set; }
        public string ModuleCode { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public string IconClass { get; set; } = "bi bi-folder2-open";
        public List<ModelMenuItemDto> Models { get; set; } = new();
    }

    public class ModelMenuItemDto
    {
        public int ModelId { get; set; }
        public string ModelCode { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}