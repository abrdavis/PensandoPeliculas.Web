using System.Data;
using System.Text.Json.Serialization;

namespace RavelDev.PensandoPeliculas.WebApi.Models.Auth
{
    public class User
    {
        public User(string? userName, Guid id)
        {
            UserName = userName;
            UserId = id;
        }

        public Guid UserId { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }


    }
}
