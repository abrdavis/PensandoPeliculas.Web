using Azure.Core;
using RavelDev.PensandoPeliculas.Core.DataAccess;
using RavelDev.PensandoPeliculas.Core.Models;
using RavelDev.PensandoPeliculas.Entity.Migrations;

namespace RavelDev.PensandoPeliculas.Core.API
{
    public class ReviewHomePageModel{
        public ReviewHomePageModel(List<ReviewModel> recentReviews, List<ReviewModel> recentReleasedReviews)
        {
            RecentReviews = recentReviews;
            RecentReleasedReviews = recentReleasedReviews;
        }

        public List<ReviewModel> RecentReviews { get; set; }
        public List<ReviewModel> RecentReleasedReviews { get; set; }
    }

    public class ReviewApi
    { 
        public ReviewApi(ReviewRepository reviewRepo)
        {
          ReviewRepo = reviewRepo;
        }

        public ReviewRepository ReviewRepo { get; }

        public ReviewModel GetReviewForId(int reviewId)
        {
            var reviewModel = ReviewRepo.GetReview(reviewId);
            return reviewModel;
        }

        public ReviewModel? GetReviewForSlug(string reviewSlug)
        {
            var reviewModel = ReviewRepo.GetReviewForSlug(reviewSlug);
            return reviewModel;
        }

        public List<ReviewModel> GetReviews(int? resultCount, int? resultOffset, string orderByColumn = "")
        {
            return ReviewRepo.GetReviews(resultCount, resultOffset, orderByColumn);
        }

        public ReviewHomePageModel GetReviewsForHomePage()
        {
            var recentReviews = ReviewRepo.GetRecentReviews();
            var recentReleasedReviews = ReviewRepo.GetRecentReleases();
            var result = new ReviewHomePageModel(recentReviews, recentReleasedReviews);
            return result;
        }

        public ReviewModel InsertReview(int reviewTitleId, 
            string titleName, 
            string userId, 
            string reviewTitle, 
            string reviewText, 
            decimal reviewRating, 
            DateTime titleReleaseDate,
            string headerImageUrl = "")
        {
            var sanatizedTitle = titleName.Replace(" ", "-");
            var reviewSlug = $"{sanatizedTitle}_{titleReleaseDate.ToString("yyyy_M_d")}";
            var review = ReviewRepo.InsertReview(reviewTitleId,
                DateTime.UtcNow,
                isVisible: true,
                reviewAuthor: userId,
                reviewTitle: reviewTitle,
                reviewText: reviewText,
                reviewRating: reviewRating,
                reviewSlug: reviewSlug,
                headerImageUrl: headerImageUrl);

            return review;
        }

        public ReviewModel UpdateReview(int reviewId, 
            int reviewTitleId,
            string titleName, 
            string userId,
            string reviewTitle, 
            string reviewText,
            decimal reviewRating,
            DateTime titleReleaseDate,
            string headerImageUrl = "")
        {
            var sanatizedTitle = titleName.Replace(" ", "-");
            var reviewSlug = $"{sanatizedTitle}_{titleReleaseDate.ToString("yyyy_M_d")}";
            var review = ReviewRepo.UpdateReview(
                reviewId,
                reviewTitleId,
                DateTime.UtcNow,
                isVisible: true,
                reviewAuthor: userId,
                reviewTitle: reviewTitle,
                reviewText: reviewText,
                reviewRating: reviewRating,
                reviewSlug: reviewSlug,
                headerImageUrl: headerImageUrl);

            return review;
        }
    }
}
