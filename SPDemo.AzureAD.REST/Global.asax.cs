using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.IdentityModel.Services;

namespace SPDemo.AzureAD.REST
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IdentityConfig.ConfigureIdentity();
         }

        void WSFederationAuthenticationModule_RedirectingToIdentityProvider(object sender, RedirectingToIdentityProviderEventArgs e)
        {
            if (!String.IsNullOrEmpty(IdentityConfig.Realm))
            {
                e.SignInRequestMessage.Realm = IdentityConfig.Realm;
            }
        }
    }
}