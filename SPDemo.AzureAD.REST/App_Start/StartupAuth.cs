using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

namespace SPDemo.AzureAD.REST
{
    public partial class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientID"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string authority = aadInstance + "common";

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions { });

            // instead of using the default validation (validating against a single issuer value, as we do in line of business apps), 
            // we inject our own multitenant validation logic
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        // If the app needs access to the entire organization, then add the logic
                        // of validating the Issuer here.
                        // IssuerValidator
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {   
                        SecurityTokenValidated = (context) =>
                        {
                            // If your authentication logic is based on users
                            return Task.FromResult(0);
                        },
                        AuthenticationFailed = (context) =>
                        {
                            // Pass in the context back to the app
                            context.HandleResponse();
                            // Suppress the exception
                            return Task.FromResult(0);
                        }
                    }
                });

            // This makes any middleware defined above this line run before the Authorization rule is applied in web.config
            app.UseStageMarker(PipelineStage.Authenticate);
        }
    }
}
