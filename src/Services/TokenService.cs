using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using Microsoft.IdentityModel.Tokens;
using Models.Token;
using MongoDB.Driver;
using MongoEntities;
using Services.Interface;

namespace Services
{
    public class TokenService : ITokenService
    {
        private readonly AppSettings _appSettings;

        private readonly IMongoCollection<TokenInfo> _tokenInfoCollection;

        public TokenService(AppSettings appSettings)
        {
            _appSettings = appSettings;
            var client = new MongoClient(appSettings.MongoDBSetting.ConnectionString);
            var database = client.GetDatabase(appSettings.MongoDBSetting.Databases.MediaDatabase.Name);
            _tokenInfoCollection = database.GetCollection<TokenInfo>(appSettings.MongoDBSetting.Databases.MediaDatabase.Collections.TokenContentCollectionName);
        }


        public string generateJwtToken(IdentityAuthenticate identityAuthenticate)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // var tokenDescriptor = new SecurityTokenDescriptor
            // {
            //     Subject = new ClaimsIdentity(new[] {
            //         new Claim("groupId", identityAuthenticate.GroupId),
            //         new Claim("project", identityAuthenticate.Project),
            //         new Claim("password", identityAuthenticate.Password),
            //         new Claim("role", identityAuthenticate.Role.ToString()),
            //         }),
            //     Expires = DateTime.UtcNow.AddYears(100),
            //     SigningCredentials = new SigningCredentials(
            //         new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret)),
            //         SecurityAlgorithms.HmacSha256Signature
            //     )
            // };

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("groupId", identityAuthenticate.GroupId),
                    new Claim("project", identityAuthenticate.Project),
                    new Claim("password", identityAuthenticate.Password),
                    new Claim("role", identityAuthenticate.Role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddYears(100),
                Issuer = "vcsjones",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<TokenInfo> GetTokenAsync(string token) =>
            await _tokenInfoCollection.Find<TokenInfo>(x => x.Token == token)
            .FirstOrDefaultAsync();


        public async Task<TokenInfo> GetAsync(string groupId, string project, string password) =>
            await _tokenInfoCollection.Find<TokenInfo>(x => x.GroupId == groupId &&
            x.Project == project && x.Password == password).FirstOrDefaultAsync();


        public async Task RemoveAsync(string id) =>
            await _tokenInfoCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<TokenInfo> InsertAsync(TokenInfo tokenInfo)
        {
            await _tokenInfoCollection.InsertOneAsync(tokenInfo);
            return tokenInfo;
        }
    }
}