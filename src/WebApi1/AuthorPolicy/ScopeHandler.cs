using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace WebApi1.AuthorPolicy
{
    public class ScopeHandler : AuthorizationHandler<ScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "scope" ))
            {
                return Task.CompletedTask;
            }
            bool haveScope = context.User.HasClaim(c => c.Type == "scope" && c.Value == requirement.ScopeName);
            //bool haveAud = context.User.Claims.Any(c => c.Type == "aud" && c.Value == "ApiResource1");
            if(haveScope)
                context.Succeed(requirement); 
            return Task.CompletedTask;
        }
    }
}
