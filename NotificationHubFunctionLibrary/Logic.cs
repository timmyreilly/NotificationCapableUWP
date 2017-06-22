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
        private static string _adendum;

        public Logic(string adendum)
        {
            _adendum = adendum; 
        }

        public async Task SendNotificationAsync(string words)
        {
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString("Endpoint=sb://tracktech.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=RaqN67dBIxHkUmcDZwP4Y3qoEfv3rniC0cVoDY2vfhk=", "TTNHub");
            var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" + _adendum + " "+ words + @"</text></binding></visual></toast>";
            await hub.SendWindowsNativeNotificationAsync(toast);
        }
    }
}
