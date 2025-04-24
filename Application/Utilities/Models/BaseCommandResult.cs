using Domain.Common;

namespace Application.Utilities.Models
{
    public record BaseCommandResult
    {
        public bool IsSuccess { get; set; }
        public ErrorCode ErrorCode { get; set; }
        public string Message { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
