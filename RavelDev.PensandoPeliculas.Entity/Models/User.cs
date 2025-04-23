using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RavelDev.PensandoPeliculas.Entity.Models
{
    [Table("Users")]
    public class User
    {
        public Guid UserId { get; set; }

        public string UserName { get;set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        [JsonIgnore]
        public List<RefreshToken>? RefreshTokens { get; set; }

        public Role? Role { get; set; }

        public string SiteDisplayName { get; set; }
    }
}
