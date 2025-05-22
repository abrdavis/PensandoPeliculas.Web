using Microsoft.AspNetCore.Mvc;
using RavelDev.PensadoPeliculas.Images.Service;
using RavelDev.PensandoPeliculas.Core.API;
using RavelDev.PensandoPeliculas.Core.Models;
using RavelDev.PensandoPeliculas.Core.Models.Requests;
using RavelDev.PensandoPeliculas.Entity;
using RavelDev.PensandoPeliculas.Entity.Models.Reviews;
using RavelDev.PensandoPeliculas.Users.Models;
using System.Security.Claims;
using static RavelDev.PensandoPeliculas.Core.Utility.ImageConstants;


namespace RavelDev.PensandoPeliculas.WebApi.Controllers
{
    [Route("[controller]")]
    public class ReviewController : BaseController
    {
        public ReviewApi ReviewApi { get; }
        public PeliculaDbContext DbContext { get; }
        public ImageUploadService ImageService { get; }

        public ReviewController(ReviewApi reviewApi, 
            ImageUploadService imageService,
            PeliculaDbContext dbContext) {
            ReviewApi = reviewApi;
            DbContext = dbContext;
            ImageService = imageService;
        }




        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateReview(UpdateReviewRequest request)
        {
            string token, headerImageUrl = string.Empty;
            try
            {
                
                if (request == null)
                {
                    return new JsonResult(new { success = false, requestError = true });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                UserModel? user = (UserModel?)HttpContext.Items["User"];
                token = HttpContext.Request.Cookies["X-Access-Token"];

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
                {
                    return new JsonResult(new { success = false, userIdNotFound = true });
                }



                var reviewTitle = DbContext.Titles.FirstOrDefault(title => title.TitleId == request.ReviewTitleId);

                if (reviewTitle == null)
                {
                    return new JsonResult(new { success = false, titleNotFound = true });
                }

                var reviewModel =  ReviewApi.UpdateReview(
                    request.ReviewId,
                    request.ReviewTitleId,
                    titleName: reviewTitle.TitleName,
                    userId: userId,
                    reviewTitle: request.ReviewTitle,
                    reviewText: request.ReviewText,
                    reviewRating: request.ReviewRating,
                    titleReleaseDate: reviewTitle.ReleaseDate);

                if(request.HeaderImage != null)
                {
                    headerImageUrl = await UploadHeaderImage(request.HeaderImage, token, request.ReviewId);
                    if (!string.IsNullOrEmpty(headerImageUrl))
                    {
                        reviewModel.HeaderImageUrl = headerImageUrl;
                    }
  
                }

                return new JsonResult(new { success = true, reviewModel = reviewModel });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            return new JsonResult(new { success = false, titleNotFound = true });
        }
        [HttpPost("Insert")]
        [Authorize]
        public async  Task<IActionResult> InsertReview(InsertReviewRequest request)
        {
           string token;
            ReviewModel review;
            try
            {
                if (request == null)
                {
                    return new JsonResult(new { success = false, requestError = true });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                UserModel? user = (UserModel?)HttpContext.Items["User"];
                token = HttpContext.Request.Cookies["X-Access-Token"];

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
                {
                    return new JsonResult(new { success = false, userIdNotFound = true });
                }

                var reviewTitle = DbContext.Titles.FirstOrDefault(title => title.TitleId == request.ReviewTitleId);

                if (reviewTitle == null)
                {
                    return new JsonResult(new { success = false, titleNotFound = true });
                }
                review = ReviewApi.InsertReview(request.ReviewTitleId,
                    titleName: reviewTitle.TitleName,
                    userId: userId,
                    reviewTitle: request.ReviewTitle,
                    reviewText: request.ReviewText,
                    reviewRating: request.ReviewRating,
                    titleReleaseDate: reviewTitle.ReleaseDate);

                if (request.HeaderImage != null)
                {
                   var headerImageUrl =  await UploadHeaderImage(request.HeaderImage, token, review.ReviewId);
                    if (!string.IsNullOrEmpty(headerImageUrl))
                    {
                        review.HeaderImageUrl = headerImageUrl;
                    }
                }

                return new JsonResult(new { success = true, review = review });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false });
            }
        }


        [HttpGet("GetReviews")]
        public IActionResult GetReviews([FromQuery] int? resultCount, int? resultOffset, string orderBy)
        {
            try
            {
                var reviews = ReviewApi.GetReviews(resultCount, resultOffset, orderBy);
                return new JsonResult(reviews);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new JsonResult(new { success = false });
            }
        }

        [HttpGet("GetForSlug")]
        public IActionResult GetForSlug([FromQuery] string reviewSlug)
        {
            try
            {
                var reviewModel = ReviewApi.GetReviewForSlug(reviewSlug);
                if(reviewModel == null)
                {
                    Console.WriteLine($"No review found for slug {reviewSlug}");
                    return new JsonResult(new { success = false, reviewNotFound = true });
                }
                return new JsonResult(new { success = true, reviewModel = reviewModel });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false });
            }
        }

        [HttpGet("Get")]
        [Authorize]
        public IActionResult Get([FromQuery] int reviewId)
        {
            try
            {
                var reviewModel = ReviewApi.GetReviewForId(reviewId);
                if (reviewModel == null)
                {
                    Console.WriteLine($"No review found for id {reviewId}");
                    return new JsonResult(new { success = false, reviewNotFound = true });
                }
                return new JsonResult(new { success = true, reviewModel = reviewModel });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false });
            }
        }

        [HttpGet("GetForHomePage")]
        public IActionResult GetForHomePage()
        {
            try
            {
                var reviewsHomeData = ReviewApi.GetReviewsForHomePage();
                return new JsonResult(new { success = true, pageModel = reviewsHomeData });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false });
            }
        }

        private async Task<string> UploadHeaderImage(IFormFile headerImage, string token, int reviewId)
        {
            var headerImgUrl = string.Empty;
            try
            {

                var imageResponse = await ImageService.UploadImage(ImageTypes.ReviewHeader, headerImage, token);
                if (imageResponse != null && imageResponse.Success)
                {
                    headerImgUrl = imageResponse.ImageUrl;
                    var revEntity = DbContext.Reviews.FirstOrDefault(rev => reviewId == rev.ReviewId)!;
                    revEntity.HeaderImageUrl = headerImgUrl;
                    DbContext.Update(revEntity);
                    DbContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error uploading review header image.", ex);
            }

            return headerImgUrl;
        }

    }
}
