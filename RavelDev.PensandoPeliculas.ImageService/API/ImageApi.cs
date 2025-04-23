using Azure.Storage.Blobs;
using RavelDev.PensandoPeliculas.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace RavelDev.PensandoPeliculas.ImageService.API
{

    public class ImageApi
    {
        public async Task<ImageUploadResult> UploadMoviePoster(BlobContainerClient containerClient, Stream imageStream, string fileName, int thumbnailWidth = 360)
        {
            var blobClient = containerClient.GetBlobClient($"posters/{fileName}");
            var blobResult = await blobClient.UploadAsync(imageStream);
            var posterUrl = blobClient.Uri.ToString();
            imageStream.Position = 0;
            using var image = await Image.LoadAsync(imageStream);

            var thumbnailName = $"thumb_{fileName}";
            blobClient = containerClient.GetBlobClient($"posters/thumbnails/{thumbnailName}");
            var height = (thumbnailWidth * image.Height) / image.Width;
            image.Mutate(x => x.Resize(thumbnailWidth, height));

            var thumbnailStream = new MemoryStream();
            image.Save(thumbnailStream, image.Metadata.DecodedImageFormat!);
            thumbnailStream.Position = 0;
            blobResult = await blobClient.UploadAsync(thumbnailStream);

            var thumbnailUrl = blobClient.Uri.ToString();
            var result = new ImageUploadResult(posterUrl, thumbnailUrl, true);
            return result;
        }

        internal async Task<ImageUploadResult> UploadReviewHeader(BlobContainerClient containerClient, Stream imageStream, string fileName)
        {
            var blobClient = containerClient.GetBlobClient($"review/headers/{fileName}");
            var blobResult = await blobClient.UploadAsync(imageStream);
            var imageHeaderUrl = blobClient.Uri.ToString();

            var result = new ImageUploadResult(imageHeaderUrl, true);
            return result;
        }
    }
}
