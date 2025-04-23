using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavelDev.PensandoPeliculas.Core.Models
{
    public class ImageUploadResult
    {
        public ImageUploadResult()
        {
            Success = false;
            ThumbnailUrl = string.Empty;
            ImageUrl = string.Empty;
        }

        public ImageUploadResult(string imageUrl, bool success)
        {
            ImageUrl = imageUrl;
            Success = success;
            ThumbnailUrl = string.Empty;

        }

        public ImageUploadResult(string imageUrl, string thumbnailUrl, bool success)
        {
            ImageUrl = imageUrl;
            ThumbnailUrl = thumbnailUrl;
            Success = success;
        }

        public string ThumbnailUrl { get; set; }
        public string ImageUrl { get; set; }
        public bool Success { get; set; }
    }
}
