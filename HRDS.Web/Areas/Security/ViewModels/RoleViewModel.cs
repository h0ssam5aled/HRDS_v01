using System.ComponentModel.DataAnnotations;
using HRDS.Web.Resources;

namespace HRDS.Web.Models
{
    public class RoleViewModel
    {
        public int RoleId { get; set; }

        [Display(Name = "RoleCode", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string RoleCode { get; set; } = string.Empty;

        [Display(Name = "ArabicName", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(200, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string RoleNameAr { get; set; } = string.Empty;

        [Display(Name = "EnglishName", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(200, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string RoleNameEn { get; set; } = string.Empty;

        [Display(Name = "Description", ResourceType = typeof(Resource))]
        [StringLength(300, ErrorMessageResourceName = "StringLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string? Description { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resource))]
        public bool IsActive { get; set; } = true;
    }
}