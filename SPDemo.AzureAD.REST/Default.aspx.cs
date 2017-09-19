using System;
using System.Configuration;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
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

            string consentUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize?response_type=code&client_id={0}&scope=https%3A%2F%2Fgraph.windows.net&redirect_uri={1}&prompt=admin_consent";
            WsFederationConfiguration config = FederatedAuthentication.FederationConfiguration.WsFederationConfiguration;
            string callbackUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Response.ApplyAppPathModifier("~/");
            //string authUrl = string.Format(consentUrl, ConfigurationManager.AppSettings["ida:ClientId"], callbackUrl);
            string authUrl = string.Format("https://login.windows.net/common/adminconsent?client_id={0}&state={1}&redirect_uri={2}", ConfigurationManager.AppSettings["ida:ClientId"], "admin", callbackUrl);
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
            string endpointUrl = "https://graph.microsoft.com";
            string userId = HttpContext.Current.User.Identity.Name;
            string targetUrl = String.Format("/v1.0/users/{0}/events?$select=Subject,Organizer,Start,End", userId);

            List<CalendarObject> _events = CalendarManager.GetCalendarItems(endpointUrl, targetUrl);

            if (_events != null)
            {
                foreach (CalendarObject _event in _events)
                {
                    string _start = DateTime.SpecifyKind(DateTime.Parse(_event.Start.DateTime), DateTimeKind.Utc).ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss");
                    string _end = DateTime.SpecifyKind(DateTime.Parse(_event.End.DateTime), DateTimeKind.Utc).ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss");

                    string html = "<div class='eventContainer'>"
                        + "<div class='eventSubject'>" + _event.Subject + "</div>"
                        + "<div class='eventDate'>" + _start + "</div>"
                        + "<div class='eventDate'>" + _end + "</div>"
                        + "</div>";

                    ContentDiv.InnerHtml = html;
                }
            }

        }

        
    }
}