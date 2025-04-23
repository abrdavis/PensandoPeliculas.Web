
namespace RavelDev.PensandoPeliculas.Core.Models
{
    public class ReviewModel
    {
        public ReviewModel()
        {

        }
        public int ReviewId { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ReviewText { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewTitleId { get; set; }
        public decimal ReviewRating { get; set; }
        public string PosterUrl { get; set; }
        public string PosterThumbnailUrl { get;set; }
        public string HeaderImageUrl { get; set; }
        public bool IsVisible { get; set; }
        public string TitleName { get; set; }
        public string Slug { get; set; }
        public string ReviewAuthor { get; set; }
        public DateTime ReleaseDate { get; set; }

    }
}
