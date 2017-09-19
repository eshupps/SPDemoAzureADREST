using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SPDemo.AzureAD.REST.Utils;

namespace SPDemo.AzureAD.REST.Utils
{
    public class CalendarManager
    {
        public static List<CalendarObject> GetCalendarItems(string EndpointUrl, string TargetUrl)
        {
            List<CalendarObject> result = null;

            try
            {
                string _requestUrl = string.Format("{0}{1}", EndpointUrl, TargetUrl);
                AuthManager am = new AuthManager();
                Task<string> _task = Task.Run(async () => await am.GetAuthorizedResponse(EndpointUrl, _requestUrl));
                _task.Wait();
                string _result = _task.Result;
                var _obj = JsonConvert.DeserializeObject<CalendarResponse>(_result);
                result = _obj.value;
            }
            catch (System.Exception ex)
            {

            }

            return result;

        }
    }
}