#r "..\bin\NotificationHubFunctionLibrary.dll"

using System;
using System.Threading.Tasks;
using NotificationHubFunctionLibrary; 

public static void Run(string mySbMsg, TraceWriter log)
{
    var logic = new Logic("Adendum test");

    logic.SendNotificationAsync(mySbMsg); 

    log.Info($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
}
