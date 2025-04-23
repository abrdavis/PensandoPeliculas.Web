using Dapper;
using MySqlConnector;
using RavelDev.Core.Interfaces;
using RavelDev.PensandoPeliculas.Core.Models;
using System.Data;
using System.Data.SqlClient;

namespace RavelDev.PensandoPeliculas.Core.DataAccess
{
    public class TitleRepository
    {
        public TitleRepository(IRepositoryConfig repoConfig)
        {
            this.RepoConfig = repoConfig;
        }

        public IRepositoryConfig RepoConfig { get; }

        public TitleModel GetTitleForId(int titleId)
        {
            var result = new TitleModel();
            using (var connection = new MySqlConnection(RepoConfig.ConnectionString))
            {
                var sprocName = @"TitleGetForId";
                var qParams = new
                {
                    titleId = titleId,
                };
                result = connection.QuerySingle<TitleModel>(sprocName, qParams,
                    commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        public TitleModel GetTitleForNameAndDate(string titleName, DateTime releaseDate)
        {
            var result = new TitleModel();
            using (var connection = new MySqlConnection(RepoConfig.ConnectionString))
            {
                var sprocName = @"TitlesGetForNameAndReleaseDate";
                var qParams = new
                {
                    titleName = titleName,
                    releaseDate = releaseDate
                };
                result = connection.QuerySingle<TitleModel>(sprocName, qParams,
                    commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        public List<ReviewModel> GetTitles()
        {
            var result = new List<ReviewModel>();
            using (var connection = new MySqlConnection(RepoConfig.ConnectionString))
            {
                var sprocName = @"TitlesGetAll";
                result = connection.Query<ReviewModel>(sprocName, 
                    commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }

        public List<TitleModel> GetTitlesFilterByName(string filterText)
        {
            var result = new List<TitleModel>();
            using (var connection = new MySqlConnection(RepoConfig.ConnectionString))
            {
                var sprocName = @"TitlesGetForFilterName";
                var qParams = new
                {
                    filterText = filterText,
                };
                result = connection.Query<TitleModel>(sprocName, qParams,
                    commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }

        public TitleModel InsertTitle(string titleName, DateTime releaseDate, int genreId, string posterUrl = "", string posterThumbUrl = "")
        {
            TitleModel titleModel;
            using (var connection = new MySqlConnection(RepoConfig.ConnectionString))
            {
                var sprocName = @"TitleInsert";
                var qParams =  new {
                    posterImageUrl = posterUrl,
                    titleName = titleName,
                    releaseDate = releaseDate
                };
                titleModel = connection.QueryFirst<TitleModel>(sprocName, qParams,
                    commandType: CommandType.StoredProcedure);
            }
            return titleModel;
        }
    }
}
