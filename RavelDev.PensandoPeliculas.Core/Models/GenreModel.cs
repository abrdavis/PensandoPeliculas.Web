using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavelDev.PensandoPeliculas.Core.Models
{
    public class GenreModel
    {
        public GenreModel() {
            GenreName = string.Empty;
        }
        public int? GenreId { get; set; }
        public string GenreName { get; set; }

    }
}
