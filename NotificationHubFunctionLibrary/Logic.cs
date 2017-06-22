using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;

namespace NotificationHubFunctionLibrary
{
    public class Logic
    {
        private NotificationHubClient _hub;

        public Logic(string notificationHubConnectionString, string hubName)
        {
            _hub = NotificationHubClient.CreateClientFromConnectionString(notificationHubConnectionString, hubName);

        }

        public async Task SendNotificationAsync(string words)
        {
            var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" + _adendum + " " + words + @"</text></binding></visual></toast>";
            await _hub.SendWindowsNativeNotificationAsync(toast);
        }
    }
}
