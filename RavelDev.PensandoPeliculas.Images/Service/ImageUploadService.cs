using Microsoft.AspNetCore.Http;
using RavelDev.PensadoPeliculas.Images.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using RavelDev.PensandoPeliculas.Core.Models;


namespace RavelDev.PensadoPeliculas.Images.Service
{

    public class ImageUploadService
    {
        ImageServiceConfig ServinceConfig;
        public ImageUploadService(ImageServiceConfig serviceConfig)
        {

            ServinceConfig = serviceConfig;
        }
        public async Task<ImageUploadResult?> UploadImage(int imageTypeId, IFormFile imageFile, string jwtToken)
        {
            var webService = new Uri($"{ServinceConfig.ServiceUrl}/upload");
            var imagePostContent = new StreamContent(imageFile.OpenReadStream());
            imagePostContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = imageFile.FileName };
            imagePostContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            using var client = new HttpClient();
            using var formData = new MultipartFormDataContent();

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            formData.Add(imagePostContent);
            formData.Add(new StringContent(imageTypeId.ToString()), "imageTypeId");
            var response = await client.PostAsync(webService, formData);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<ImageUploadResult?>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return responseModel;
        }
    }
}
