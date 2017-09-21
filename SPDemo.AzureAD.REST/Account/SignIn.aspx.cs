using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPDemo.AzureAD.REST.Account
{
    public partial class SignIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                Response.Redirect("~/");
            }
            else
            {
                HttpCookie hc = new HttpCookie("ContentType");
                hc = HttpContext.Current.Request.Cookies[Request.Url.GetLeftPart(UriPartial.Authority)];

                if (hc != null)
                {
                    if (hc.Value == "authenticated")
                        AuthenticateUser();
                    else
                    {
                        //Handle IsAuthenticated bug from Azure app redirection page
                        if (HttpContext.Current.Request.UrlReferrer.ToString().Contains("account.activedirectory.windowsazure.com"))
                        {
                            AuthenticateUser();
                        }
                        else
                        {
                            signinContent.Visible = true;
                        }
                    }
                }
                else
                {
                    signinContent.Visible = true;
                }
            }
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            //Set cookie
            SetCookie();

            //Authenticate
            AuthenticateUser();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/");
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
            SetCookie();
            Response.Redirect(signInRequest.RequestUrl.ToString());
        }

        protected void SetCookie()
        {
            DateTime dt = DateTime.Now;
            HttpCookie licCookie = new HttpCookie(Request.Url.GetLeftPart(UriPartial.Authority));
            licCookie.Value = "authenticated";
            licCookie.Expires = dt.AddMinutes(86400);
            HttpContext.Current.Response.Cookies.Add(licCookie);
        }
    }
}