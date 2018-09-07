using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesService.Services
{
    public class MigrationService : IMigrationService
    {
        private readonly TenantDbContext _tenantDbContext;

        public MigrationService(TenantDbContext tenantDbContext)
        {
            _tenantDbContext = tenantDbContext;
        }

        public async Task MigrateAsync()
        {
            await _tenantDbContext.Database.MigrateAsync();
        }
    }
}