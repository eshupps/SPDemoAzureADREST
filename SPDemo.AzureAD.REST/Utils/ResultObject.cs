using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPDemo.AzureAD.REST.Utils
{
    public class ResultObject
    {
        public string TenantId { get; set; }
        public string EmailAddress { get; set; }
        public string ObjectId { get; set; }
        public string DisplayName { get; set; }
        public string AccessToken { get; set; }
    }
}