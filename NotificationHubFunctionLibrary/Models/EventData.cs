using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHubFunctionLibrary.Models
{
    public class EventData
    {
        public string deviceId { get; set; }
        public string alertText { get; set; }
        public string alertLevel { get; set; }
    }
}
