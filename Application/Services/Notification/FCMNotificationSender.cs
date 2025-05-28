using FirebaseAdmin.Messaging;
using Application.Utilities.Contractors;

namespace Application.Services.Notificationsd
{
    public class FCMNotificationSender : INotificationSender
    {
        public async Task SendAsync(string to, string title, string body, Dictionary<string, string>? data = null)
        {
            var message = new Message()
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body,
                },
                Token = to,
            };

            if (data != null)
            {
                message.Data = data;
            }

            try
            {
                var result = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine($"Successfully sent message: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }
    }
}
