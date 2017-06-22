A console application, Azure Function with Supporting Library, and UWP app for a pipelined notification service


Sample appsettings.json for Azure Function: 
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "",
    "AzureWebJobsServiceBus": "Endpoint=sb://trackbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=IPVzzzzz0OcTx+RLgqA+N8fPasdfaaxpQ7qB9R6s=",
    "NotificationHubConnectionString": "Endpoint=sb://tracktech.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=RaqN67dBIxHkUmcDZwP4Y3qoEfv3rniC0cVoDY2vfhk=",
    "hubName": "TTNHub"
  }
}
```