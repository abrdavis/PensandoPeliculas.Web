using Microsoft.AspNetCore.Http;


namespace RavelDev.PensandoPeliculas.Core.Models.Requests
{
    public class UpdateReviewRequest
    {
        public UpdateReviewRequest() {
            ReviewText = string.Empty;
            ReviewTitle = string.Empty;
        }
        public int ReviewId { get; set; }
        public int ReviewTitleId { get; set; }
        public string ReviewText { get; set; }
        public decimal ReviewRating {  get; set; }
        public string ReviewTitle {  get; set; }
        public bool IsVisible { get; set; }
        public IFormFile HeaderImage { get; set; }
        
    }
}
