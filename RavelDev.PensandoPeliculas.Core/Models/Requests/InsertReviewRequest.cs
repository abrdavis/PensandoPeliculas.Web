using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavelDev.PensandoPeliculas.Core.Models.Requests
{
    public class InsertReviewRequest
    {
        public InsertReviewRequest() {
            ReviewText = string.Empty;
            ReviewTitle = string.Empty;
        }
        public int ReviewTitleId { get; set; }
        public string ReviewText { get; set; }
        public decimal ReviewRating {  get; set; }
        public string ReviewTitle {  get; set; }
        public bool IsVisible { get; set; }
        public IFormFile HeaderImage { get; set; }
        
    }
}
