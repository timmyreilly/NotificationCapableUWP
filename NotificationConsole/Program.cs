using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Microsoft.ServiceBus.Messaging;


namespace NotificationConsole
{
    class Program
    {

        private const string ServiceBusConnectionString = "Endpoint=sb://trackbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=IPVbf690OcTx+RLgqA+N8fPQofb9i9SraxpQ7qB9R6s=";
        private const string QueueName = "";
        public static TopicClient Client = TopicClient.CreateFromConnectionString(ServiceBusConnectionString, "warnings");

        static void Main(string[] args)
        {
            var words = Console.ReadLine();
            if (words == "s")
            {
                Console.WriteLine("what do you want to put in Service Bus? :");
                var w = Console.ReadLine();

                SendToSB(w);
            }
            SendNotificationAsync(words);
            Console.ReadLine();


        }

        private static async void SendToSB(string words)
        {
            BrokeredMessage message = new BrokeredMessage("Test: " + words);
            await Client.SendAsync(message); 

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
