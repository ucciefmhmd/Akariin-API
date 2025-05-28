using Microsoft.AspNetCore.Http;
using Application.Utilities.Models;

namespace Application.Utilities.Contractors
{
    public interface IFileService
    {
        Task<UploadFileResult> UploadFilesAsync(string Id, IFormFile file);
        Task<GetFilesResult> GetFilesUrlAsync(string Id);
        Task DeleteFilesAsync(string Id);
        Task DeleteFileAsync(string Url);
        Task<DownloadFileResult> DownloadFileAsync(string fileName);
    }

}
