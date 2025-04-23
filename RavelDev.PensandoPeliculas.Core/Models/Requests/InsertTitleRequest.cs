using Microsoft.AspNetCore.Http;

namespace RavelDev.PensandoPeliculas.Core.Models.Requests
{
    public class InsertTitleRequest
    {
        public InsertTitleRequest() {
            TitleName = string.Empty;
        }
        public string TitleName { get; set; }
        public DateTime? ReleaseDate {  get; set; }

        public IFormFile TitleImage { get; set; }
        public GenreModel Genre { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
