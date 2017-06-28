using Microsoft.Azure.NotificationHubs;
using System.Xml.Linq;
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
