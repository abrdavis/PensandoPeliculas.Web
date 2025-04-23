using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavelDev.PensandoPeliculas.Core.Models
{
    public class TitleModel
    {
        public TitleModel() {
            TitleName = string.Empty;
            PosterThumbnailUrl = string.Empty;
            PosterUrl = string.Empty;
        }
        public int TitleId { get; set; }
        public string TitleName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterUrl { get; set; }
        public string PosterThumbnailUrl { get; set; }
    }
}
