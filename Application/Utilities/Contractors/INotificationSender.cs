
namespace Application.Utilities.Contractors
{
    public interface INotificationSender
    {
        Task SendAsync(string to, string title, string body, Dictionary<string, string>? Data = null);
    }
}
