using System.Threading.Tasks;
using Helpers;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Register(GenerateAuthenticateRequest request)
        {
            var identityAuthenticate = new IdentityAuthenticate()
            {
                GroupId = request.GroupId,
                Project = request.Project,
            };
            var token = _tokenService.generateJwtToken(identityAuthenticate);

            return Ok(await _tokenService.InsertAsync(new TokenInfo()
            {
                Token = token,
                GroupId = request.GroupId,
                Project = request.Project,
                Password = identityAuthenticate.Password
            }));
        }
    }
}