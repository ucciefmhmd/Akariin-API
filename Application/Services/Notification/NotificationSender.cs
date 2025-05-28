using Application.Utilities.Contractors;

namespace Application.Services.Notification
{
    public class NotificationSender(INotificationSender _notification)
    {
        public async Task SendAsync(string To, string Title, string Body, Dictionary<string, string>? Data = null)
        {
            await _notification.SendAsync(To, Title, Body, Data);
        }
    }
}
