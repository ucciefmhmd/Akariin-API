﻿using Application.Utilities.Contractors;

namespace Application.Services.Notification
{
    public class NotificationSender
    {
        private readonly INotificationSender _notification;
        public NotificationSender(INotificationSender notification) { _notification = notification; }

        public async Task SendAsync(string To, string Title, string Body, Dictionary<string,string> Data=null)
        {
            await _notification.SendAsync(To, Title, Body, Data);
        }

    }
}
