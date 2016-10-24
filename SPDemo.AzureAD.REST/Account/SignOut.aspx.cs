using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;

namespace SPDemo.AzureAD.REST.Account
{
    public partial class SignOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                WsFederationConfiguration config = FederatedAuthentication.FederationConfiguration.WsFederationConfiguration;
                string callbackUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Response.ApplyAppPathModifier("~/");
                SignOutRequestMessage signoutMessage = new SignOutRequestMessage(new Uri(config.Issuer), callbackUrl);
                signoutMessage.SetParameter("wtrealm", IdentityConfig.Realm ?? config.Realm);
                FederatedAuthentication.SessionAuthenticationModule.SignOut();
                Response.Redirect(signoutMessage.WriteQueryString());
            }
        }
    }
}