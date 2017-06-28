using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;
using Windows.Networking.PushNotifications;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace NotificationConsole
{
    class Program
    {
        private static string NotificationHubConnectionString = ConfigurationManager.AppSettings["NotificationHub.Connectionstring"];
        private static string HubName = ConfigurationManager.AppSettings["HubName"];
        static NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(NotificationHubConnectionString, HubName);

        private static string ServiceBusConnectionString = ConfigurationManager.AppSettings["ServiceBus.ConnectionString"];
        public static TopicClient Client = TopicClient.CreateFromConnectionString(ServiceBusConnectionString, "warnings"); // warnings is the topic for service bus

        static void Main(string[] args)
        {

            int menuchoice = 0;

            while (menuchoice != 7)
            {
                Console.WriteLine("Menu");
                Console.WriteLine("Enter a number for your selection");
                Console.WriteLine("1. Send message to Service Bus");
                Console.WriteLine("2. Send message to notification hub");
                Console.WriteLine("7. Quit");

                menuchoice = int.Parse(Console.ReadLine());

                switch (menuchoice)
                {
                    case 1:
                        Console.WriteLine("What do you want to put in Service Bus?");
                        var w = Console.ReadLine();
                        SendToSB(w);
                        break;
                    case 2:
                        Console.WriteLine("What do you want to be in the notification?");
                        var n = Console.ReadLine();
                        Console.WriteLine("Who should receive the notification? officer1 officer2");
                        var t = Console.ReadLine();
                        SendNotificationAsync(n, t);
                        break;
                    case 7:
                        break;
                    default:
                        Console.Write("Sorry, invalid selection.");
                        break;
                }
            }

        }

        private static async void SendToSB(string words)
        {
            BrokeredMessage message = new BrokeredMessage("Test: " + words);
            await Client.SendAsync(message);

        }

        private static async void SendNotificationAsync()
        {
            var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">Hello from a .NET App!</text></binding></visual></toast>";
            await hub.SendWindowsNativeNotificationAsync(toast);
        }

        private static async void SendNotificationAsync(string words)
        {
            var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" + words + @"</text></binding></visual></toast>";
            await hub.SendWindowsNativeNotificationAsync(toast);
        }

        private static async void SendNotificationAsync(string words, string tags)
        {
            var toast = TemplateMaker(words);

            var windowsResult = await hub.SendWindowsNativeNotificationAsync(toast, tags); 
        }

        public static string TemplateMaker(string notificationText)
        {
            var toast = new XElement("toast",
                new XElement("visual",
                new XElement("binding",
                new XAttribute("template", "ToastText01"),
                new XElement("text",
                new XAttribute("id", "1"),
                "$(message)")))).ToString(SaveOptions.DisableFormatting);

            var alert = new JObject(
              new JProperty("aps", new JObject(new JProperty("alert", "$(message)"))),
              new JProperty("inAppMessage", notificationText))
              .ToString(Newtonsoft.Json.Formatting.None);

            var payload = new JObject(
              new JProperty("data", new JObject(new JProperty("message", "$(message)"))))
              .ToString(Newtonsoft.Json.Formatting.None);

            return toast;
        }

        private async Task CreateChannel()
        {
            // var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync(); 

            
        }

        private async Task SetupNotifications()
        {
            var registrations = await hub.GetRegistrationsByChannelAsync("officer1", 1);
        }



    }
}
