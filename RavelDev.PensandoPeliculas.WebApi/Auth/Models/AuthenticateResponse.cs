using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavelDev.PensandoPeliculas.Users.Models
{
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; }

        public AuthenticateResponse(Guid userId, string token, string refreshToken)
        {
            Id = userId;
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
