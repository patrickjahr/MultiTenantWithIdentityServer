using System.Threading.Tasks;
using IdentityServer.Models;

namespace IdentityServer.Services
{
    public interface IRegistrationService
    {
        Task<RegistrationResult> CreateTenantAndUserAsync(string email, string password, string tenantName);
    }
}