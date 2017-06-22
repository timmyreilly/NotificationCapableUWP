#r "..\bin\NotificationHubFunctionLibrary.dll"

using System;
using System.Threading.Tasks;
using NotificationHubFunctionLibrary; 

public static void Run(string mySbMsg, TraceWriter log)
{
    var notificationHubConnectionString = System.Environment.GetEnvironmentVariable("NotificationHubConnectionString");
    var hubName = System.Environment.GetEnvironmentVariable("HubName"); 

    var logic = new Logic(notificationHubConnectionString, hubName);

    logic.SendNotificationAsync(mySbMsg); 

    log.Info($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
}
