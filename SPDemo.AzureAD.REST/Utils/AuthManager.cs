using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Configuration;
using System.Globalization;
using System.Security.Claims;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;

namespace SPDemo.AzureAD.REST.Utils
{
    public class AuthManager
    {
        private static readonly string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private const string LoginUrl = "https://login.windows.net/common";
        private static readonly string AppPrincipalId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static readonly string AppKey = ConfigurationManager.AppSettings["ida:ClientSecret"];

        public async Task<string> GetAuthorizedResponse(string EndpointUrl, string RequestUrl)
        {
            string result = string.Empty;

            try
            {
                //string tenantId = ClaimsPrincipal.Current.FindFirst(TenantIdClaimType).Value;
                AuthenticationContext authContext = new AuthenticationContext(LoginUrl);
                authContext.TokenCache.Clear();
                ClientCredential credential = new ClientCredential(AppPrincipalId, AppKey);
                AuthenticationResult assertionCredential = await authContext.AcquireTokenAsync(EndpointUrl, credential);

                string authHeader = assertionCredential.CreateAuthorizationHeader();

                using (HttpClient client = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, RequestUrl);
                    request.Headers.TryAddWithoutValidation("Authorization", authHeader);
                    HttpResponseMessage response = await client.SendAsync(request);
                    result = await response.Content.ReadAsStringAsync();

                    if (result.Contains("ExpiredAuthenticationToken"))
                    {
                        authContext.TokenCache.Clear();
                        response = await client.SendAsync(request);
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (System.Exception ex)
            {
                string _msg = ex.Message;
            }

            return result;
        }
    }
}