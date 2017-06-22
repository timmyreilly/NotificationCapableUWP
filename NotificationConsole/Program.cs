﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;

namespace NotificationConsole
{
    class Program
    {
        private static string NotificationHubConnectionString = ConfigurationManager.AppSettings["NotificationHub.Connectionstring"];
        private static string HubName = ConfigurationManager.AppSettings["HubName"];
        static NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(NotificationHubConnectionString, HubName);

        private static string ServiceBusConnectionString = ConfigurationManager.AppSettings["ServiceBus.ConnectionString"];
        public static TopicClient Client = TopicClient.CreateFromConnectionString(ServiceBusConnectionString, "warnings");

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
                        SendNotificationAsync(n);
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




    }
}
