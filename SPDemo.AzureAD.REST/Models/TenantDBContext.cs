using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SPDemo.AzureAD.REST.Models
{
    public class TenantDbContext : DbContext
    {
        public TenantDbContext()
            : base("AADRESTDB")
        {
        }

        public DbSet<TenantRegistrationModels.Tenant> Tenants { get; set; }

        public DbSet<TenantRegistrationModels.IssuingAuthorityKey> IssuingAuthorityKeys { get; set; }

        public DbSet<TenantRegistrationModels.SignupToken> SignupTokens { get; set; }
    }
}