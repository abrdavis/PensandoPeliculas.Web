using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavelDev.PensandoPeliculas.Entity.Models.Titles
{
    [Table("Titles")]
    public class Title
    {
        public int TitleId { get; set; }
        public string TitleName { get; set; }
        public DateTime ReleaseDate { get; set; }

        [DefaultValue("")]
        public string PosterUrl { get; set; }

        [DefaultValue("")]
        public string PosterThumbnailUrl { get; set; }
        public int LengthMinutes { get; set; }
    }
}
