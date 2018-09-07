using System;
using System.Threading.Tasks;
using IdentityServer.Models;

namespace IdentityServer.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserAsync(string userName, string tenantName);
    }
}