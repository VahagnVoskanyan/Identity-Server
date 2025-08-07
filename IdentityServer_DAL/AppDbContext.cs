using IdentityServer_DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer_DAL
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public AppDbContext() { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Additional model configurations can be added here if needed
            builder.Entity<User>(entity =>
            {
                entity.Property(u => u.UserName).IsRequired();
                entity.Property(u => u.OrganizationName)
                    .HasMaxLength(256);
            });
        }
    }
}
