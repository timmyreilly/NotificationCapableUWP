using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Windows.Networking.PushNotifications;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using NotificationHubFunctionLibrary.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace NotificationHubFunctionLibrary
{
    public class Logic
    {
        private NotificationHubClient _hub;

        public Logic(string notificationHubConnectionString, string hubName)
        {
            _hub = NotificationHubClient.CreateClientFromConnectionString(notificationHubConnectionString, hubName);

        }

        //public async Task SendNotificationAsync(string words)
        //{
        //    var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" + words + @"</text></binding></visual></toast>";
        //    await _hub.SendWindowsNativeNotificationAsync(toast);
        //}

        public async void SendNotificationAsync(string mySbMsg)
        {
            var e = DeserializeMessage(mySbMsg);

            var toast = WindowsTemplateMaker(e);

            var windowsResult = await _hub.SendWindowsNativeNotificationAsync(toast, e.deviceId);
            Debug.WriteLine(windowsResult);
        }

        public EventData DeserializeMessage(string mySbMsg)
        {
            var e = JsonConvert.DeserializeObject<EventData>(mySbMsg);
            return e; 
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
        } // probably junk 

        public async void SendNotificationAsync(string words, string tags)
        {
            var e = DeserializeMessage(words); 

            var toast = WindowsTemplateMaker(e);

            var windowsResult = await _hub.SendWindowsNativeNotificationAsync(toast, tags);
            Debug.WriteLine(windowsResult); 
        }

        public static string WindowsTemplateMaker(EventData notification)
        {
            var toast = new XElement("toast",
                new XElement("visual",
                new XElement("binding",
                new XAttribute("template", "ToastText01"),
                new XElement("text",
                new XAttribute("id", "1"),
                $"deviceId: {notification.deviceId} alertText: {notification.alertText} alertLevel: {notification.alertLevel}")))).ToString(SaveOptions.DisableFormatting);

            return toast;
        }

    }
}
