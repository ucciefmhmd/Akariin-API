
namespace Application.Utilities.Models
{
    public record DownloadFileResult : BaseCommandResult
    {
        public DownloadFile DownloadFile { get; set; }
    }

    public record DownloadFile
    {
        public string Url { get; set; }
        public byte[] Stream { get; set; }
    }
}
