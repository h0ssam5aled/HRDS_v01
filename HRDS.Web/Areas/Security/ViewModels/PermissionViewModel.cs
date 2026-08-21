using System.ComponentModel.DataAnnotations;
using HRDS.Web.Resources;

namespace HRDS.Web.Models
{
    public class PermissionViewModel
    {
        public int PermissionId { get; set; }

        [Display(Name = "Screen", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
        public int ModelId { get; set; }

        [Display(Name = "Action", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
        public int ActionId { get; set; }

        [Display(Name = "PermissionCode", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(150)]
        public string PermissionCode { get; set; } = string.Empty;

        [Display(Name = "ArabicName", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(200)]
        public string PermissionNameAr { get; set; } = string.Empty;

        [Display(Name = "EnglishName", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(200)]
        public string PermissionNameEn { get; set; } = string.Empty;

        public string? ModelNameAr { get; set; }
        public string? ModelNameEn { get; set; }
        public string? ActionNameAr { get; set; }
        public string? ActionNameEn { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resource))]
        [StringLength(300)]
        public string? Description { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resource))]
        public bool IsActive { get; set; } = true;
    }
}