using System.Threading.Tasks;
using Enums;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Middlewares.Authentication;
using Models.Requests;
using Models.Token;
using MongoEntities;
using Services;
using Services.Interface;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private ITokenService _tokenService;

        public TokenController(AppSettings appSettings)
        {
            _tokenService = new TokenService(appSettings);
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin)]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Register(GenerateAuthenticateRequest request)
        {
            var token = _tokenService.generateJwtToken(new IdentityAuthenticate()
            {
                GroupId = request.GroupId,
                Customer = request.Customer,
                Password = request.Password,
            });

            return Ok(await _tokenService.InsertAsync(new TokenInfo()
            {
                Token = token,
                GroupId = request.GroupId,
                Customer = request.Customer,
            }));
        }
    }
}