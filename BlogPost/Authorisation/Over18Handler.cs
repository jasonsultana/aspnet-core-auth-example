using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogPost.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BlogPost.Authorisation
{
    public class Over18Handler : AuthorizationHandler<Over18Requirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataProvider _tempDataProvider;

        public Over18Handler(IHttpContextAccessor httpContextAccessor, ITempDataProvider tempDataProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _tempDataProvider = tempDataProvider;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Over18Requirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == nameof(LoginViewModel.Age)))
            {
                var ageClaim = context.User.Claims.Single(c => c.Type == nameof(LoginViewModel.Age)).Value;
                var age = Convert.ToInt32(ageClaim);

                if (age >= 18)
                {
                    context.Succeed(requirement);
                }
            }

            if (!context.HasSucceeded)
            {
                _tempDataProvider.SaveTempData(_httpContextAccessor.HttpContext, new Dictionary<string, object>()
                {
                    { "message", "You need to be over 18 to buy beer." }
                });
                context.Fail();
            }
            
            return Task.CompletedTask;
        }
    }
}