using Dapper;
using MySqlConnector;
using RavelDev.Core.Interfaces;
using RavelDev.PensandoPeliculas.Core.Models;
using RavelDev.PensandoPeliculas.Entity.Migrations;
using RavelDev.PensandoPeliculas.Entity.Models.Reviews;
using System.Data;


namespace RavelDev.PensandoPeliculas.Core.DataAccess
{
    public class ReviewRepository
    {
        public ReviewRepository(IRepositoryConfig repoConfig)
        {
            this.RepoConfig = repoConfig;
        }

        public IRepositoryConfig RepoConfig { get; }

        public List<ReviewModel> GetRecentReleases()
        {
            var result = new List<ReviewModel>();
            using (var con = new MySqlConnection(RepoConfig.ConnectionString))
            {
                result = con.Query<ReviewModel>("ReviewsGetRecentReleases", commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }

        public List<ReviewModel> GetRecentReviews()
        {
            var result = new List<ReviewModel>();
            using (var con = new MySqlConnection(RepoConfig.ConnectionString))
            {
                result = con.Query<ReviewModel>("ReviewsGetRecent", commandType: CommandType.StoredProcedure).ToList();
            }
            return result;
        }

        public ReviewModel InsertReview(int reviewTitleId,
            DateTime reviewDate,
            bool isVisible,
            string reviewAuthor,
            string reviewTitle,
            string reviewText,
            decimal reviewRating,
            string reviewSlug,
            string headerImageUrl)
        {
            ReviewModel result;
            using (var connection = new MySqlConnection(RepoConfig.ConnectionString))
            {
                var sprocName = @"ReviewInsert";

                var qParams = new
                {
                    titleId = reviewTitleId,
                    reviewDate = reviewDate,
                    reviewTitle = reviewTitle,
                    reviewText = reviewText,
                    isVisible = isVisible,
                    reviewAuthor = reviewAuthor,
                    reviewRating = reviewRating,
                    reviewSlug = reviewSlug,
                    headerImageUrl =  headerImageUrl
                };

                result = connection.QuerySingle<ReviewModel>(sprocName, qParams,
                    commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        public ReviewModel? GetReviewForSlug(string reviewSlug)
        {
            var result = new ReviewModel();
            using (var con = new MySqlConnection(RepoConfig.ConnectionString))
            {
                var qParams = new
                {
                    reviewSlug = reviewSlug
                };
                result = con.QuerySingleOrDefault<ReviewModel?>("ReviewGetForSlug", qParams,  commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        internal ReviewModel UpdateReview(int reviewId, 
            int reviewTitleId, 
            DateTime dateUpdated, 
            bool isVisible, 
            string reviewAuthor, 
            string reviewTitle, 
            string reviewText, 
            decimal reviewRating,
            string reviewSlug, 
            string headerImageUrl)
        {
            ReviewModel result;
            using (var connection = new MySqlConnection(RepoConfig.ConnectionString))
            {
                var sprocName = @"ReviewUpdate";

                var qParams = new
                {
                    reviewId = reviewId,
                    titleId = reviewTitleId,
                    dateUpdated = dateUpdated,
                    reviewTitle = reviewTitle,
                    reviewText = reviewText,
                    isVisible = isVisible,
                    reviewAuthor = reviewAuthor,
                    reviewRating = reviewRating,
                    reviewSlug = reviewSlug,
                    headerImageUrl = headerImageUrl
                };

                result = connection.QuerySingle<ReviewModel>(sprocName, qParams,
                    commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        internal List<ReviewModel> GetReviews(int? resultCount, int? resultOffset, string orderByColumn)
        {
            var result = new List<ReviewModel>();
            using (var con = new MySqlConnection(RepoConfig.ConnectionString))
            {
                var sprocName = @"ReviewsGet";

                var qParams = new
                {
                    resultCount = resultCount,
                    orderByColumn = orderByColumn,
                    resultOffset = resultOffset
                };
                result = con.Query<ReviewModel>(sprocName, qParams,  commandType: CommandType.StoredProcedure)?.ToList();
            }
            return result;
        }

        internal ReviewModel GetReview(int reviewId)
        {
            var result = new ReviewModel();
            using (var con = new MySqlConnection(RepoConfig.ConnectionString))
            {
                var qParams = new
                {
                    reviewId = reviewId
                };
                result = con.QuerySingleOrDefault<ReviewModel?>("ReviewGetForId", qParams, commandType: CommandType.StoredProcedure);
            }
            return result;
        }
    }
}
