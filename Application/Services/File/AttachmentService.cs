using Microsoft.AspNetCore.Http;
using Application.Utilities.Contractors;
using Application.Utilities.Models;

namespace Application.Services.File
{
    public sealed class AttachmentService
    {
        private readonly IFileService _fileService;

        public AttachmentService(IFileService fileService)
        {
            this._fileService = fileService;
        }
        public async Task DeleteFilesAsync(string Id)
        {
            await _fileService.DeleteFilesAsync(Id);
        }
        public async Task<UploadFileResult> UploadFilesAsync(string Id, IFormFile file)
        {
            return await _fileService.UploadFilesAsync(Id,file);
        }
        public async Task<GetFilesResult> GetFilesUrlAsync(string Id)
        {
            return await _fileService.GetFilesUrlAsync(Id);
        }
    
        public async Task<DownloadFileResult> DownloadFileAsync(string fileName)
        {
            return await _fileService.DownloadFileAsync(fileName);
        }
        public async Task DeleteFileAsync(string Url)
        {
            await _fileService.DeleteFileAsync(Url);
        }
    }
}
