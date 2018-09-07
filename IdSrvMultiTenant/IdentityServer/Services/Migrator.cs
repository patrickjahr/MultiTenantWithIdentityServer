using IdentityServer.Configuration;
using IdentityServer.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace IdentityServer.Services
{
    public class Migrator
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly PersistedGrantDbContext _persistedGrantDbContext;
        private readonly ConfigurationDbContext _configurationDbContext;

        public Migrator(ApplicationDbContext applicationDbContext, PersistedGrantDbContext persistedGrantDbContext,
            ConfigurationDbContext configurationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _persistedGrantDbContext = persistedGrantDbContext;
            _configurationDbContext = configurationDbContext;
        }
        
        public void Migrate()
        {
            _applicationDbContext.Database.Migrate();
            _persistedGrantDbContext.Database.Migrate();
            _configurationDbContext.Database.Migrate();

            SeedData();
        }

        private void SeedData()
        {
            if (!_configurationDbContext.Clients.Any())
            {
                foreach (var client in Clients.Get())
                {
                    _configurationDbContext.Clients.Add(client.ToEntity());
                }
                _configurationDbContext.SaveChanges();
            }

            if (!_configurationDbContext.IdentityResources.Any())
            {
                foreach (var resource in IdentityResources.Get())
                {
                    _configurationDbContext.IdentityResources.Add(resource.ToEntity());
                }
                _configurationDbContext.SaveChanges();
            }

            if (!_configurationDbContext.ApiResources.Any())
            {
                foreach (var resource in ApiResources.Get())
                {
                    _configurationDbContext.ApiResources.Add(resource.ToEntity());
                }
                _configurationDbContext.SaveChanges();
            }
        }
    }
}
