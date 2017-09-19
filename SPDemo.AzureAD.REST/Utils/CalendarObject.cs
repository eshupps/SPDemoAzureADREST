using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SPDemo.AzureAD.REST.Utils
{
    public class CalendarObject
    {
        //[JsonProperty(PropertyName ="@odata.id")]
        //public string odataId { get; set; }
        [JsonProperty(PropertyName = "@odata.etag")]
        public string odataEtag { get; set; }
        public string Id { get; set; }
        public string Subject { get; set; }
        public StartValue Start { get; set; }
        public EndValue End { get; set; }
        public OrganizerValue Organizer { get; set; }
    }

    public class StartValue
    {
        public string DateTime { get; set; }
        public string TimeZone { get; set; }
    }

    public class EndValue
    {
        public string DateTime { get; set; }
        public string TimeZone { get; set; }
    }

    public class OrganizerValue
    {
        public EmailValue EmailAddress { get; set; }
    }

    public class EmailValue
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}