using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Windows.Networking.PushNotifications;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

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
            var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" + words + @"</text></binding></visual></toast>";
            await _hub.SendWindowsNativeNotificationAsync(toast);
        }

        public async Task SendNotificationAsync(string words, string tagExpression)
        {
            var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" + words + @"</text></binding></visual></toast>";
            await _hub.SendWindowsNativeNotificationAsync(toast, tagExpression);
        }


        public async void SubscribeToCategories(string categories)
        {
            var channel = "officer1";

            //if (categories == null)
            //{
            //    categories = RetrieveCategories();
            //}


            // Using a template registration to support notifications across platforms.
            // Any template notifications that contain messageParam and a corresponding tag expression
            // will be delivered for this registration.

            const string templateBodyWNS = "<toast><visual><binding template=\"ToastText01\"><text id=\"1\">$(messageParam)</text></binding></visual></toast>";

            await _hub.CreateWindowsTemplateRegistrationAsync(channel, templateBodyWNS);
        }

        public string template(string notificationText)
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

    }
}
