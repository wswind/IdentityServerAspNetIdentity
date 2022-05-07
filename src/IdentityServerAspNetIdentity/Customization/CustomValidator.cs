using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Customization
{
    public class CustomValidator : IExtensionGrantValidator
    {
        public string GrantType => "custom_pwd";

        private bool CheckPassword(string userName,string password)
        {
            if (userName == "user" && password == "password")
                return true;
            return false;
        }

        public Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            string userName = context.Request.Raw.Get("username");
            string password = context.Request.Raw.Get("password");

            if (CheckPassword(userName, password))
            {
                var claims = new List<Claim>() {
                        new Claim(JwtClaimTypes.Subject, userName),
                        new Claim(JwtClaimTypes.Name, userName)
                };
                context.Result = new GrantValidationResult(subject: userName, GrantType, claims);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "CheckPassword fail");
            }
            return Task.CompletedTask;
        }
    }
}
