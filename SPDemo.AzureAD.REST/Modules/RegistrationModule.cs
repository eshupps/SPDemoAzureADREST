using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPDemo.AzureAD.REST.Utils;

namespace SPDemo.AzureAD.REST.Modules
{
    public class RegistrationModule : IHttpModule
    {
        HttpContext ctx = null;

        public void Init(HttpApplication application)
        {
            application.BeginRequest += (new EventHandler(this.application_BeginRequest));
        }

        private void application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            ctx = application.Context;
            if (ctx.Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string wresult = ctx.Request.Form["wresult"];
                if (!string.IsNullOrEmpty(wresult))
                {
                    ResultObject ro = Utility.GetValuesFromWResult(wresult);
                    if (!string.IsNullOrEmpty(ro.TenantId))
                    {
                        if (DatabaseIssuerNameRegistry.TryAddTenant(ro.TenantId, ro.TenantId))
                        {
                            ctx.Response.Redirect("/default.aspx?ISubscriptionToken=" + ro.AccessToken);
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        public void Dispose()
        { }
    }
}