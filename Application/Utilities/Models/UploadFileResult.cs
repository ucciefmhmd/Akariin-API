
namespace Application.Utilities.Models
{
    public record UploadFileResult : BaseCommandResult
    {
        public List<UploadFile> UploadFiles { get; set; } = [];
    }

    public record UploadFile
    {
        public string Url { get; set; }
        public string FileName { get; set; }
    }
}
