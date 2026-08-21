using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace HRDS.Web.Security
{
    public class ModulePolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public ModulePolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("Module_", StringComparison.OrdinalIgnoreCase))
            {
                var moduleCode = policyName.Substring("Module_".Length);
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new ModuleAccessRequirement(moduleCode));
                return Task.FromResult<AuthorizationPolicy?>(policy.Build());
            }

            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
            => FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
            => FallbackPolicyProvider.GetFallbackPolicyAsync();
    }
}