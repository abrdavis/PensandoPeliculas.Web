using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RavelDev.PensandoPeliculas.Entity;
using RavelDev.PensandoPeliculas.Entity.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCryptNet = BCrypt.Net.BCrypt;

namespace RavelDev.PensandoPeliculas.Users
{

    public interface IUserManager
    {
        public string AuthenticateAndReturnJwtToken(string userName, string password);
        bool CreateUser(string userName, string password);
    }
    public class UserManager : IUserManager
    {
        private IConfiguration _config;
        private PeliculaDbContext context;

        public UserManager(IConfiguration config, PeliculaDbContext dbContext)
        {
            _config = config;
            this.context = dbContext;
        }
        public bool CreateUser(string userName, string password)
        {
            try
            {
                var user = new User();
                user.UserName = userName;
                user.Password = BCryptNet.HashPassword(password);

                // save user
                context.Users.Add(user);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public string AuthenticateAndReturnJwtToken(string userName, string password)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
