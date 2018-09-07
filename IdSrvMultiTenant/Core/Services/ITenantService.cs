using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Http;

namespace Core.Services
{
    public interface ITenantService
    {
        IEnumerable<string> GetAllTenantNames();
        Task<Guid> CreateAsync(string tenant);
        Task RemoveTenantAsync(Guid id);
        Task<Guid> GetTenantIdAsync(string tenant, bool createIfNotExists = false);
        string GetTenantName(HttpContext httpContext);
        string GetTenantName(string url);
        Task<string> GetTenantNameAsync(Guid tenantId);
    }
}