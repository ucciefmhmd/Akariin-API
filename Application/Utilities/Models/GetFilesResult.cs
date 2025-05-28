
namespace Application.Utilities.Models
{
    public record GetFilesResult : BaseCommandResult
    {
        public List<string> Urls { get; set; } = [];
    }
}
