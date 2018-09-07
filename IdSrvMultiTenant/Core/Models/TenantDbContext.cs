using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Core.Models
{
    public class TenantDbContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        
        public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options) { } 
    }
}