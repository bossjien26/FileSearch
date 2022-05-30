using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enums;
using Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.Token;
using Services;
using Services.Interface;

namespace Middlewares.Authentication
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        private ITokenService _tokenService;

        public AuthenticationMiddleware(RequestDelegate next) => _next = next;

        //TODO:need refactor
        public async Task Invoke(HttpContext httpContext,
         AppSettings appSettings, ILogger<AuthenticationMiddleware> logger)
        {
            _tokenService = new TokenService(appSettings);
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrWhiteSpace(token))
            {
                await _next.Invoke(httpContext);
                return;
            }
            var tokenInfo = await _tokenService.GetTokenAsync(token);
            if (tokenInfo == null)
            {
                await _next.Invoke(httpContext);
                return;
            }
            if (!CheckJwtTokenIsExpire(token))
            {
                await _next.Invoke(httpContext);
                return;
            }
            else
            {
                httpContext = await attachUserToContext(httpContext, token, appSettings, logger);
                await _next.Invoke(httpContext);
                return;
            }
        }

        private async Task<HttpContext> attachUserToContext(HttpContext httpContext, string token,
        AppSettings appSettings, ILogger<AuthenticationMiddleware> logger)
        {
            try
            {
                var IdentityAuthenticates = GetVerifyTokenType(appSettings, token);
                // attach user to context on successful jwt validation
                httpContext.Items["httpContextTokenInfo"] = IdentityAuthenticates;
                // if (!string.IsNullOrEmpty(tokenInfo.Id))
                // {
                //     return httpContext;
                // }else{
                return httpContext;
                // }
            }
            catch (Exception exception)
            {
                logger.LogDebug(exception, "jwt validate is error");
                return httpContext;
            }
        }

        private IdentityAuthenticate GetVerifyTokenType(AppSettings appSettings, string token)
        {
            var IdentityAuthenticates = VerifyToken(appSettings, token).Claims.
                    Where(x => x.Type == "groupId" || x.Type == "customer" || x.Type == "password")
                    .Select(s => s.Value).ToList();
            return CheckVerifyTokenType(IdentityAuthenticates, token);
        }

        private IdentityAuthenticate CheckVerifyTokenType(List<string> IdentityAuthenticates, string token)
        {
            if (IdentityAuthenticates.Count() != 3)
            {
                return new IdentityAuthenticate();
            }

            return new IdentityAuthenticate()
            {
                GroupId = IdentityAuthenticates[0],
                Project = IdentityAuthenticates[1],
                Role = (RoleEnum)Enum.Parse(typeof(RoleEnum), IdentityAuthenticates[2], true),
            };
        }

        private JwtSecurityToken VerifyToken(AppSettings appSettings, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(appSettings.JwtSettings.Secret)
                ),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }

        private bool CheckJwtTokenIsExpire(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token);

            var expDate = jwtToken.ValidTo;
            return expDate > DateTime.UtcNow.AddHours(8) ? true : false;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}