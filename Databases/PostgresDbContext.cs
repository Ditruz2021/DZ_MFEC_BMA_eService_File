using Microsoft.EntityFrameworkCore;
using dotnet_starter.Models;

namespace dotnet_starter.Database.PostgresDbContext
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) { }

        public DbSet<LoggingUser> LoggingUsers { get; set; }
        public DbSet<Simple> Simples { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //------------------------ RolePermission Table --------------------------//
            //------------------------ RolePermission Table --------------------------//

            //------------------------ Role Table --------------------------//
            //------------------------ Role Table --------------------------//

            //------------------------ Permission Table --------------------------//
            //------------------------ Permission Table --------------------------//

            //------------------------ User Table --------------------------//
            //------------------------ User Table --------------------------//

            //------------------------ LoggingUser Table --------------------------//
            //------------------------ LoggingUser Table --------------------------//

            //------------------------ User Table --------------------------//
            modelBuilder.Entity<Simple>()
                .HasIndex(u => u.Name)
                .IsUnique();
            //------------------------ User Table --------------------------//

        }
    }
}
