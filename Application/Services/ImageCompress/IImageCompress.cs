using Microsoft.AspNetCore.Http;
namespace Application.Services.ImageCompress
{
    public interface IImageCompress
    {
        public Task Compress(IFormFile Image, string outputPath, int quality = 75);
    }
}
