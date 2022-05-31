using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using Enums;
using MongoEntities;

namespace Middlewares.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    // IAsyncAuthorizationFilter
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<RoleEnum> _roles = new List<RoleEnum>();

        public AuthorizeAttribute(params RoleEnum[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            var httpContextTokenInfo = (TokenInfo)context.HttpContext.Items["httpContextTokenInfo"];
            if (httpContextTokenInfo == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                // await verifyUserToken(context, httpContextTokenInfo);
                verifyUserToken(context, httpContextTokenInfo);
            }
        }

        private void verifyUserToken(AuthorizationFilterContext context, TokenInfo httpContextTokenInfo)
        {
            if (httpContextTokenInfo == null && !_roles.Contains(httpContextTokenInfo.Role))
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}