using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi1.AuthorPolicy
{
    public class ScopeRequirement : IAuthorizationRequirement
    {
        public string ScopeName { get; set; }

        public ScopeRequirement(string scope)
        {
            ScopeName = scope;
        }
    }
}
