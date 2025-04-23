using Microsoft.Extensions.Options;
using RavelDev.PensandoPeliculas.Core.Configs;


namespace RavelDev.PensandoPeliculas.ImageService.Utility
{

    public class ImageWriter 
    {
        public ImageWriter(IOptions<ImageUploadConfig> imageConfig) {
            ImageConfig = imageConfig.Value;
        }  
        public string WriteImageToRoot(Stream imageData,  string fileName)
        {
            string fullFilePath = $"{ImageConfig.BaseDirectory}/{fileName}";
            var imageUrl = string.Empty;
            if (!File.Exists(fullFilePath))
            {
                using var fileStream = File.Create(fullFilePath);
                imageData.Seek(0, SeekOrigin.Begin);
                imageData.CopyTo(fileStream);
            }

            imageUrl =  $"{ImageConfig.BaseImageUrl}/{fileName}"; 
            return imageUrl;
        }
        public ImageUploadConfig ImageConfig { get; }
    }
}
