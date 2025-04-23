using Microsoft.AspNetCore.Mvc;
using RavelDev.PensandoPeliculas.Core.API;
using RavelDev.PensandoPeliculas.Core.Models;
using RavelDev.PensandoPeliculas.Core.Models.Requests;
using RavelDev.PensandoPeliculas.Entity;
using RavelDev.PensandoPeliculas.Users.Models;


namespace RavelDev.PensandoPeliculas.WebApi.Controllers
{
    [Route("[controller]")]
    public class MetaDataController : BaseController
    {
        public MetaDataApi MetaDataApi { get; }
        public PeliculaDbContext DbContext { get; }

        public MetaDataController(MetaDataApi metaDataApi,
            PeliculaDbContext dbContext)
        {
            MetaDataApi = metaDataApi;
            DbContext = dbContext;
        }

        [HttpPost("InsertGenre")]
        [Authorize]
        public async Task<IActionResult> InsertGenre([FromBody] InsertGenreRequest request)
        {
            int? genreId;
            string token;
            try
            {
                if (string.IsNullOrEmpty(request.GenreName))
                {
                    return new JsonResult(new { success = false, errorType = "emptyGenre" });
                }


                UserModel? user = (UserModel?)HttpContext.Items["User"];
                token = HttpContext.Request.Cookies["X-Access-Token"];

                if (user == null || string.IsNullOrEmpty(token))
                {
                    return new JsonResult(new { success = false, userError = true });
                }
                //var existingTitle = TitleRepository.GetTitleForNameAndDate(request.TitleName, request.ReleaseDate);

                genreId = MetaDataApi.InsertGenre(request.GenreName);

                if (!genreId.HasValue)
                {

                    return new JsonResult(new { success = false, couldNotCreate = true });
                }

                return new JsonResult(new { success = true, genre = new { genereId = genreId, genreName = request.GenreName } });

            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false });
            }
        }


        [HttpGet("GenresGetAll")]
        [Authorize]
        public  IActionResult GenresGetAll()
        {
            int? genreId;
            string token;
            try
            {

               var allGenres = MetaDataApi.GenresGetAll()?.OrderBy(genre => genre.GenreName);


                return new JsonResult(new { success = true, genres = allGenres });

            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false });
            }
        }


        [HttpGet("GetAdminViewModel")]
        [Authorize]
        public IActionResult GetAdminViewModel()
        {
            MetaDataAdminViewModel viewModel;
            try
            {
                viewModel = MetaDataApi.GetAdminViewModel();

                return new JsonResult(new { success = true, viewModel = viewModel });

            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false });
            }
        }

    }
}
