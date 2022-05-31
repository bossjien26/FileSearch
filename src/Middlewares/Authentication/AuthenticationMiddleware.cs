using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enums;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Models.Token;
using MongoEntities;
using Services;
using Services.Interface;

namespace Middlewares.Authentication
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        private ITokenService _tokenService;

        private TokenInfo _tokenInfo = new TokenInfo();

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
            _tokenInfo = await _tokenService.GetTokenAsync(token);
            if (_tokenInfo == null || !VerifyToken(appSettings, token))
            {
                await _next.Invoke(httpContext);
                return;
            }

            // httpContext = attachUserToContext(httpContext, token, appSettings, logger);
            httpContext.Items["httpContextTokenInfo"] = _tokenInfo;

            await _next.Invoke(httpContext);
            return;
        }

        private HttpContext attachUserToContext(HttpContext httpContext, string token,
        AppSettings appSettings, ILogger<AuthenticationMiddleware> logger)
        {
            // try
            // {
            // var identityAuthenticates = GetVerifyTokenType(appSettings, token);
            // attach user to context on successful jwt validation
            return httpContext;
            // }
            // catch (Exception exception)
            // {
            //     logger.LogDebug(exception, "jwt validate is error");
            //     return httpContext;
            // }
        }

        // private IdentityAuthenticate GetVerifyTokenType(AppSettings appSettings, string token)
        // {
        //     JsonWebTokenHandler tokenHandler = new();
        //     var tokenS = GetJwtSecurityToken(appSettings, token);

        //     var identityAuthenticates = tokenS.Claims.
        //             Where(x => x.Type == "groupId" || x.Type == "customer" || x.Type == "password")
        //             .Select(s => s.Value).ToList();
        //     // var IdentityAuthenticates = tokenS.Claims.First(claim => claim.Type == "groupId");
        //     return CheckVerifyTokenType(identityAuthenticates, token);
        // }

        private JwtSecurityToken GetJwtSecurityToken(AppSettings appSettings, string token)
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

        private bool VerifyToken(AppSettings appSettings, string token)
        {
            JsonWebTokenHandler tokenHandler = new();
            var validatedToken = tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuers = new string[] { "vcsjones" },
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(appSettings.JwtSettings.Secret)
                    )
                }
            );

            return validatedToken.SecurityToken.ValidTo > DateTime.UtcNow ? true : false;
        }
    }
}