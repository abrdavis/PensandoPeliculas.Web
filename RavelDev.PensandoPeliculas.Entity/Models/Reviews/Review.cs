using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavelDev.PensandoPeliculas.Entity.Models.Titles;

namespace RavelDev.PensandoPeliculas.Entity.Models.Reviews
{
    [Table("Reviews")]
    public class Review
    {
        public int ReviewId { get; set; }
        public DateTime ReviewDate { get; set; }
        public DateTime DateUpdated { get; set; }
        public Title Title { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewText { get; set; }
        public decimal ReviewRating { get; set; }
        public bool IsVisible { get; set; }
        public string Slug { get; set; }
        public string HeaderImageUrl { get; set; }
        public Guid ReviewAuthor { get; set; }
    }
}
