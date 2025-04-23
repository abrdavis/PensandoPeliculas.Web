namespace RavelDev.PensandoPeliculas.Core.Configs
{
    public class ImageUploadConfig
    {
        public required string ImagesFullPath { get; set; }
        public required string ImageSaveMode { get; set; }
        public required string BaseImageUrl { get; set; }

        public string BaseDirectory
        {
            get
            {
                return ImagesFullPath;
            }
        }
    }
}
