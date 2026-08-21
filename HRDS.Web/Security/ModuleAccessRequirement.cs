using Microsoft.AspNetCore.Authorization;

namespace HRDS.Web.Security
{
    public class ModuleAccessRequirement : IAuthorizationRequirement
    {
        public string ModuleCode { get; }

        public ModuleAccessRequirement(string moduleCode)
        {
            ModuleCode = moduleCode;
        }
    }
}