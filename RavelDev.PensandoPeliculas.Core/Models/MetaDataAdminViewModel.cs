namespace RavelDev.PensandoPeliculas.Core.Models
{
    public class MetaDataAdminViewModel
    {
        public MetaDataAdminViewModel() {
            Genres = new List<GenreModel>();
        }

        public MetaDataAdminViewModel(List<GenreModel> genres)
        {
            Genres = genres;
        }
        public List<GenreModel> Genres { get; set; }
    }
}
