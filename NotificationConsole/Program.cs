using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;


namespace NotificationConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var words = Console.ReadLine(); 
            SendNotificationAsync(words);
            Console.ReadLine();


        }

        private static async void SendNotificationAsync()
        {
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString("Endpoint=sb://tracktech.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=RaqN67dBIxHkUmcDZwP4Y3qoEfv3rniC0cVoDY2vfhk=", "TTNHub");
            var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">Hello from a .NET App!</text></binding></visual></toast>";
            await hub.SendWindowsNativeNotificationAsync(toast);
        }

        private static async void SendNotificationAsync(string words)
        {
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString("Endpoint=sb://tracktech.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=RaqN67dBIxHkUmcDZwP4Y3qoEfv3rniC0cVoDY2vfhk=", "TTNHub");
            var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" + words + @"</text></binding></visual></toast>";
            await hub.SendWindowsNativeNotificationAsync(toast);
        }

    }
}
