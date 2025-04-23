using Dapper;
using MySqlConnector;
using RavelDev.Core.Interfaces;
using RavelDev.PensandoPeliculas.Core.Models;
using System.Data;


namespace RavelDev.PensandoPeliculas.Core.DataAccess
{
    public class MetaDataRepository
    {
        public MetaDataRepository(IRepositoryConfig repoConfig)
        {
            this.RepoConfig = repoConfig;
        }

        public IRepositoryConfig RepoConfig { get; }

        public int InsertGenre(string genreName)
        {
            int genreId;
            using (var connection = new MySqlConnection(RepoConfig.ConnectionString))
            {
                var sprocName = @"GenreInsert";

                var qParams = new
                {
                    genreName
                };

                genreId = connection.ExecuteScalar<int>(sprocName, qParams,
                    commandType: CommandType.StoredProcedure);
            }
            return genreId;
        }

        public List<GenreModel> GetAllGenres()
        {
            var result = new List<GenreModel>();
            using (var connection = new MySqlConnection(RepoConfig.ConnectionString))
            {
                var sprocName = @"GenreGetAll";

                result = connection.Query<GenreModel>(sprocName,
                    commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }
    }
}
