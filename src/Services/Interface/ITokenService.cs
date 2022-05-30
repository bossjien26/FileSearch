using System.Threading.Tasks;
using Models.Token;
using MongoEntities;

namespace Services.Interface
{
    public interface ITokenService
    {
        Task<TokenInfo> GetTokenAsync(string token);

        Task<TokenInfo> GetAsync(string groupId, string project, string password);

        Task<TokenInfo> InsertAsync(TokenInfo tokenInfo);

        string generateJwtToken(IdentityAuthenticate identityAuthenticate);
    }
}