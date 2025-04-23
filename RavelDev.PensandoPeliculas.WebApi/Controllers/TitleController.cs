using Microsoft.AspNetCore.Mvc;
using RavelDev.PensadoPeliculas.Images.Service;
using RavelDev.PensandoPeliculas.Core.API;
using RavelDev.PensandoPeliculas.Core.DataAccess;
using RavelDev.PensandoPeliculas.Core.Models;
using RavelDev.PensandoPeliculas.Core.Models.Requests;
using RavelDev.PensandoPeliculas.Entity;
using RavelDev.PensandoPeliculas.Users.Models;
using static RavelDev.PensandoPeliculas.Core.Utility.ImageConstants;


namespace RavelDev.PensandoPeliculas.WebApi.Controllers
{
    [Route("[controller]")]
    public class TitleController : BaseController
    {
        public TitleRepository TitleRepository { get; }
        public ImageUploadService ImageUploadService { get; }
        public MetaDataApi MetaDataApi { get; }
        public PeliculaDbContext DbContext { get; }

        public TitleController(TitleRepository titleRepository,
            MetaDataApi metaDataApi,
            ImageUploadService imageUploadService,
            PeliculaDbContext dbContext)
        {
            TitleRepository = titleRepository;
            ImageUploadService = imageUploadService;
            MetaDataApi = metaDataApi;
            DbContext = dbContext;
        }

        [HttpPost("InsertTitle")]
        [Authorize]
        public async Task<IActionResult> InsertTitle(InsertTitleRequest request)
        {
            string token;
            TitleModel title;

            try
            {
                if (string.IsNullOrEmpty(request.TitleName))
                {
                    return new JsonResult(new { success = false, emptyTitle = true});
                }

                UserModel? user = (UserModel?)HttpContext.Items["User"];
                token = HttpContext.Request.Cookies["X-Access-Token"];

                if (user == null || string.IsNullOrEmpty(token))
                {
                    return new JsonResult(new { success = false, userError = true });
                }

                if (!request.ReleaseDate.HasValue)
                {
                    return new JsonResult(new { success = false, dateRequired = true });
                }

                if(!request.Genre.GenreId.HasValue && string.IsNullOrEmpty(request.Genre.GenreName))
                {
                    return new JsonResult(new { success = false, genreRequired = true });
                }

                if (!request.Genre.GenreId.HasValue)
                {
                    request.Genre.GenreId = MetaDataApi.InsertGenre(request.Genre.GenreName);
                }

                title = TitleRepository.InsertTitle(request.TitleName,
                        request.ReleaseDate.Value,
                        request.Genre.GenreId.Value);

                try
                {
                    var posterImageUrl = string.Empty;
                    if (request.TitleImage != null)
                    {
                        var imageResponse = await ImageUploadService.UploadImage(ImageTypes.Poster, request.TitleImage, token);
                        if (imageResponse != null && imageResponse.Success)
                        {
                            posterImageUrl = imageResponse.ImageUrl;
                            var tModel = DbContext.Titles.FirstOrDefault(title => title.TitleId == title.TitleId)!;
                            title.PosterUrl = tModel.PosterUrl = posterImageUrl;
                            title.PosterThumbnailUrl = tModel.PosterThumbnailUrl = imageResponse.ThumbnailUrl;
                            DbContext.Update(tModel);
                            DbContext.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error when uploadimg iamge for title.", ex);
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false });
            }

            return new JsonResult(new { success = true,  title = title });
        }

        
        [HttpGet("GetTitlesFilterByName")]
        [Authorize]
        public IActionResult GetTitlesFilterByName(string filterText)
        {
            try
            {
                var titles = TitleRepository.GetTitlesFilterByName(filterText);
                return new JsonResult(new { success = true, titles = titles });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false });
            }
        }

    }
}
