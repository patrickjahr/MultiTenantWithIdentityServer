using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IdentityServer.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Remove the obsolete Index
            var userNameIndex = builder.Entity<ApplicationUser>().Metadata.GetProperty(nameof(ApplicationUser.NormalizedUserName));
            builder.Entity<ApplicationUser>().Metadata.RemoveIndex(new List<IProperty>{userNameIndex});
            
            //Add the custom index for username/email and tenantid
            builder.Entity<ApplicationUser>(i => {
                i.ToTable("AspNetUsers");
                i.HasIndex(u => new {u.NormalizedUserName, u.TenantId}).IsUnique();
            });
        }
    }
}