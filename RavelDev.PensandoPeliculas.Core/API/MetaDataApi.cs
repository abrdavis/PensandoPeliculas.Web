using RavelDev.PensandoPeliculas.Core.DataAccess;
using RavelDev.PensandoPeliculas.Core.Models;

namespace RavelDev.PensandoPeliculas.Core.API
{
    public class MetaDataApi
    {
        public MetaDataApi(MetaDataRepository metaDataRepo)
        {
            MetaDataRepo = metaDataRepo;
        }

        public MetaDataRepository MetaDataRepo { get; }

        public List<GenreModel> GenresGetAll()
        {
            return MetaDataRepo.GetAllGenres();
        }

        public MetaDataAdminViewModel GetAdminViewModel()
        {
            MetaDataAdminViewModel result;
            var genres = MetaDataRepo.GetAllGenres()?.OrderBy(genre => genre.GenreName).ToList();
            result = new MetaDataAdminViewModel(genres ?? new List<GenreModel>());
            return result;
        }

        public int InsertGenre(string genreName)
        {
            return MetaDataRepo.InsertGenre(genreName);
        }
    }
}
