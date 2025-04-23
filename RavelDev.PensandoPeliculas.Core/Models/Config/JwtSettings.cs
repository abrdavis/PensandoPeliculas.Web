using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavelDev.PensandoPeliculas.Core.Models.Config
{
    public class JwtSettings
    {
        public JwtSettings(string jwtKey, string jwtIssuer)
        {
            Key = jwtKey;
            Issuer = jwtIssuer;
        }

        public string Key { get; set; }
        public string Issuer { get; set; }
    }
}
