using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavelDev.PensandoPeliculas.Entity.Models.MetaData
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public bool IsVisible { get; set; }
    }
}
