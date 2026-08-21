namespace HRDS.Web.Models
{
    public class RolePermissionMatrixViewModel
    {
        public int RoleId { get; set; }
        public string RoleNameAr { get; set; } = string.Empty;
        public string RoleNameEn { get; set; } = string.Empty;
        public List<ModulePermissionsGroupViewModel> Modules { get; set; } = new();
    }

    public class ModulePermissionsGroupViewModel
    {
        public int ModuleId { get; set; }
        public string ModuleNameAr { get; set; } = string.Empty;
        public List<ModelPermissionsGroupViewModel> Models { get; set; } = new();
    }

    public class ModelPermissionsGroupViewModel
    {
        public int ModelId { get; set; }
        public string ModelNameAr { get; set; } = string.Empty;
        public List<ActionPermissionItemViewModel> Actions { get; set; } = new();
    }

    public class ActionPermissionItemViewModel
    {
        public int PermissionId { get; set; }
        public int ActionId { get; set; }
        public string ActionNameAr { get; set; } = string.Empty;
        public bool IsGranted { get; set; }
    }

    public class SaveRolePermissionsRequest
    {
        public int RoleId { get; set; }
        public List<int> SelectedPermissionIds { get; set; } = new();
    }
}