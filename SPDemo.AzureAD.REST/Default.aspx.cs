using System;
using System.Configuration;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using SPDemo.AzureAD.REST.Utils;

namespace SPDemo.AzureAD.REST
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                AuthenticateUser();
                DisplayCalendar();
            }
            else
            {
                DisplayCalendar();
            }

            string tenantId = System.Security.Claims.ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            WsFederationConfiguration config = FederatedAuthentication.FederationConfiguration.WsFederationConfiguration;
            string callbackUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Response.ApplyAppPathModifier("~/");
            string authUrl = string.Format("https://login.windows.net/{0}/adminconsent?client_id={1}&state={2}&redirect_uri={3}", tenantId, ConfigurationManager.AppSettings["ida:ClientId"], "12345", callbackUrl);
            authLink.NavigateUrl = authUrl;
        }

        protected void AuthenticateUser()
        {
            //Redirect to Home page after SignIn
            WsFederationConfiguration config = FederatedAuthentication.FederationConfiguration.WsFederationConfiguration;
            string callbackUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Response.ApplyAppPathModifier("~/");
            SignInRequestMessage signInRequest = FederatedAuthentication.WSFederationAuthenticationModule.CreateSignInRequest(
                uniqueId: String.Empty,
                returnUrl: callbackUrl,
                rememberMeSet: false);
            signInRequest.SetParameter("wtrealm", IdentityConfig.Realm ?? config.Realm);
            Response.Redirect(signInRequest.RequestUrl.ToString());
        }

        protected void DisplayCalendar()
        {
            try
            {
                string endpointUrl = "https://graph.microsoft.com";
                string userId = HttpContext.Current.User.Identity.Name;
                string tenantId = System.Security.Claims.ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
                //string targetUrl = String.Format("/v1.0/{0}/users/{1}/events?$select=Subject,Organizer,Start,End", tenantId, userId);
                string targetUrl = String.Format("/v1.0/users/{0}/events?$select=Subject,Organizer,Start,End", userId);
                //List<CalendarObject> _events = CalendarManager.GetCalendarItems(endpointUrl, targetUrl);
                string _requestUrl = string.Format("{0}{1}", endpointUrl, targetUrl);
                AuthManager am = new AuthManager();
                System.Threading.Tasks.Task<string> _task = System.Threading.Tasks.Task.Run(async () => await am.GetAuthorizedResponse(endpointUrl, _requestUrl));
                _task.Wait();
                string _result = _task.Result;
                //responseDiv.InnerText = _result;

                if (!string.IsNullOrEmpty(_result))
                {
                    var _obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CalendarResponse>(_result);
                    List<CalendarObject> _events = _obj.value;

                    if (_events != null)
                    {
                        StringBuilder html = new StringBuilder();

                        foreach (CalendarObject _event in _events)
                        {
                            string _start = DateTime.SpecifyKind(DateTime.Parse(_event.Start.DateTime), DateTimeKind.Utc).ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss");
                            string _end = DateTime.SpecifyKind(DateTime.Parse(_event.End.DateTime), DateTimeKind.Utc).ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss");

                            html.Append("<div class='eventWrapper'><div class='eventContainer'>"
                                + "<div class='eventSubject'>" + _event.Subject + "</div>"
                                + "<div class='eventDate'>" + _start + "</div>"
                                + "<div class='eventDate'>" + _end + "</div>"
                                + "</div></div>");
                        }

                        ContentDiv.InnerHtml = html.ToString();

                    }
                }
                else
                {
                    responseDiv.InnerText = "Query result is null or empty.";
                }
            }
            catch (System.Exception ex)
            {
                responseDiv.InnerText = ex.Message;
            }

        }

        
    }
}