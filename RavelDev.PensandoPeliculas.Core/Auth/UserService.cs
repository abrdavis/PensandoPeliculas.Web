
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RavelDev.Core.Models.Authentication;
using RavelDev.PensandoPeliculas.Core.Models.Config;
using RavelDev.PensandoPeliculas.Entity;
using RavelDev.PensandoPeliculas.Entity.Models;
using RavelDev.PensandoPeliculas.Users.Models;

using System.Data;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BC = BCrypt.Net.BCrypt;
namespace RavelDev.PensandoPeliculas.Users
{
    public interface IUserService
    {
        Guid? ValidateJwtToken(string? token);
        RefreshToken GenerateRefreshToken(string ipAddress);
        User? GetForUserNameAndPassword(string userName, string password);
        bool RevokeToken(string token, string ipAddress);
        UserModel? GetById(Guid userId);
        AuthenticateResponse? Authenticate(string username, string password, string ipAddress);
    }
    public class UserService : IUserService
    {
        private readonly IDbConnection _dbConnection;
        private readonly PeliculaDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;

        public UserService(IDbConnection dbConnection, 
            PeliculaDbContext context, 
            JwtSettings appSettings)
        {
            _dbConnection = dbConnection;
            _dbContext = context;
            _jwtSettings = appSettings;
        }
        public RefreshToken GenerateRefreshToken(string ipAddress)
        {

            using (var rngCryptoServiceProvider = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    ReplacedByToken = string.Empty,
                    RevokedByIp = string.Empty,
                    CreatedByIp = ipAddress
                };
            }
        }
        public bool RevokeToken(string token, string ipAddress)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _dbContext.Update(user);
            _dbContext.SaveChanges();

            return true;
        }
        public UserModel GetById(Guid userId)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.UserId == userId);
            if (user == null) return null;
            return new UserModel(user!.UserName, user.UserId);
        }

        public User? GetForUserNameAndPassword(string userName, string password)
        {
            var user = _dbContext.Users.SingleOrDefault(x => x.UserName.ToLower() == userName.ToLower());


            if (user == null) return null;
            // verify password
            var passwordMatch = BC.Verify(password, user.Password);
            if (user == null || !passwordMatch)
                throw new Exception("Username or password is incorrect");
            return user;

        }

        public AuthenticateResponse? Authenticate(string username, string password, string ipAddress)
        {
            var user = GetForUserNameAndPassword(username, password);
            if (user == null) return null;

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(ipAddress);
            user.RefreshTokens = new List<RefreshToken>();
            // save refresh token
            user.RefreshTokens.Add(refreshToken);
            _dbContext.Update(user);
            _dbContext.SaveChanges();

            return new AuthenticateResponse(user.UserId, jwtToken, refreshToken.Token, user.UserName);
        }
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                IssuedAt = DateTime.UtcNow,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(50),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Guid? ValidateJwtToken(string? token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key!);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);


                return userId;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error validating JWT", ex);
                return null;
            }
        }
    }
}
