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
using Newtonsoft.Json;

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
                        EventData e = new EventData(); 
                        Console.WriteLine("deviceId? officer1 officer2");
                        e.deviceID = Console.ReadLine();
                        Console.WriteLine("alertText? out of office");
                        e.alertText = Console.ReadLine();
                        Console.WriteLine("alertLevel? high");
                        e.alertLevel = Console.ReadLine();
                        SendToSB(JsonConvert.SerializeObject(e));
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
            BrokeredMessage message = new BrokeredMessage(words);
            await Client.SendAsync(message);

        }

        private static async void SendNotificationAsync(string words)
        {
            var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" + words + @"</text></binding></visual></toast>";
            await hub.SendWindowsNativeNotificationAsync(toast);
        }

        private static async void SendNotificationAsync(string words, string tags)
        {
            var toast = WindowsTemplateMaker(words);

            var windowsResult = await hub.SendWindowsNativeNotificationAsync(toast, tags);
        }

        public static string WindowsTemplateMaker(string notificationText)
        {
            //          var windowsResult =
            //            await hub.SendWindowsNativeNotificationAsync(toast, tags;

            var toast = new XElement("toast",
                new XElement("visual",
                new XElement("binding",
                new XAttribute("template", "ToastText01"),
                new XElement("text",
                new XAttribute("id", "1"),
                $"{notificationText}")))).ToString(SaveOptions.DisableFormatting);

            return toast;
        }

        public static string GoogleTeamplateMaker(string notificationText)
        {
            //          var googleResult =
            //            await hub.SendGcmNativeNotificationAsync(payload, tags);

            var payload = new JObject(
              new JProperty("data", new JObject(new JProperty("message", $"{notificationText}"))))
              .ToString(Newtonsoft.Json.Formatting.None);

            return payload;
        }

        public static string AppleTemplateMaker(string notificationText)
        {
            //          var appleResult =
            //            await hub.SendAppleNativeNotificationAsync(alert, tags);

            var alert = new JObject(
                new JProperty("aps", new JObject(new JProperty("alert", $"{ notificationText } "))),
                new JProperty("inAppMessage", notificationText)).ToString(Newtonsoft.Json.Formatting.None);

            return alert; 
        }

    }
}
