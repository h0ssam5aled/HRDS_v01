using Microsoft.AspNetCore.Authorization;

namespace HRDS.Web.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class HasModuleAccessAttribute : AuthorizeAttribute
    {
        public HasModuleAccessAttribute(string moduleCode)
            : base(policy: $"Module_{moduleCode.ToUpper()}")
        {
        }
    }
}