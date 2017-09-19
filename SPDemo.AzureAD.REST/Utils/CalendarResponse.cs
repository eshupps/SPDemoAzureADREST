using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SPDemo.AzureAD.REST.Utils
{
    public class CalendarResponse
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string context { get; set; }
        [JsonProperty(PropertyName = "@odata.nextlink")]
        public string nextlink { get; set; }
        public List<CalendarObject> value { get; set; }
    }
}