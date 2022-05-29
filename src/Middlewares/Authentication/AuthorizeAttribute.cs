using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using Enums;
using Services.Interface;
using Models.Token;

namespace Middlewares.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    // IAsyncAuthorizationFilter
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private ITokenService _tokenService;

        private readonly IList<RoleEnum> _roles;

        public AuthorizeAttribute(params RoleEnum[] roles)
        {
            _roles = roles ?? new RoleEnum[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            var httpContextTokenInfo = (IdentityAuthenticate)context.HttpContext.Items["httpContextTokenInfo"];
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

        private void verifyUserToken(AuthorizationFilterContext context, IdentityAuthenticate httpContextTokenInfo)
        {
            // httpContextTokenInfo.Id
            // var dbContext = context.HttpContext.RequestServices.GetRequiredService<DbContextEntity>();
            // var redis = context.HttpContext.RequestServices.GetRequiredService<IConnectionMultiplexer>();
            // _userService = new UserService(dbContext, redis);
            // var redisUserInfo = await _userService.GetRedisUserInfo(httpContextTokenInfo.Token);
            // var user = await _userService.GetVerifyUser(httpContextTokenInfo.Mail, httpContextTokenInfo.Password);

            // if (user == null || !redisUserInfo.HasValue  || (_roles.Any() && !_roles.Contains(user.Role)))
            // {
            //     // not logged in
            //     context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            // }
            if (httpContextTokenInfo == null && _roles.Contains(httpContextTokenInfo.Role))
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}