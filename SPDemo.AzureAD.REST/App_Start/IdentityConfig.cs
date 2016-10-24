using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.IdentityModel.Services;
using SPDemo.AzureAD.REST.Utils;

namespace SPDemo.AzureAD.REST
{
    public class IdentityConfig
    {
        public static string AudienceUri { get; private set; }
        public static string Realm { get; private set; }

        public static void ConfigureIdentity()
        {
            RefreshValidationSettings();
            // Set the realm for the application
            Realm = ConfigurationManager.AppSettings["ida:realm"];

            // Set the audienceUri for the application
            AudienceUri = ConfigurationManager.AppSettings["ida:AudienceUri"];
            if (!String.IsNullOrEmpty(AudienceUri))
            {
                UpdateAudienceUri();
            }
        }

        public static void RefreshValidationSettings()
        {
            string metadataLocation = ConfigurationManager.AppSettings["ida:FederationMetadataLocation"];
            DatabaseIssuerNameRegistry.RefreshKeys(metadataLocation);
        }

        public static void UpdateAudienceUri()
        {
            int count = FederatedAuthentication.FederationConfiguration.IdentityConfiguration
                .AudienceRestriction.AllowedAudienceUris.Count(
                    uri => String.Equals(uri.OriginalString, AudienceUri, StringComparison.OrdinalIgnoreCase));
            if (count == 0)
            {
                FederatedAuthentication.FederationConfiguration.IdentityConfiguration
                    .AudienceRestriction.AllowedAudienceUris.Add(new Uri(IdentityConfig.AudienceUri));
            }
        }
    }
}