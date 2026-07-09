using Microsoft.EntityFrameworkCore;
using dotnet_starter.Models;

namespace dotnet_starter.Database.SQLServerDbContext
{
    public class SQLServerDbContext : DbContext
    {
        public SQLServerDbContext(DbContextOptions<SQLServerDbContext> options) : base(options) { }

        public DbSet<LoggingUser> LoggingUsers { get; set; }
        public DbSet<Simple> Simples { get; set; }
        public DbSet<SystemMenu> SystemMenus { get; set; }
        public DbSet<DataUser> DataUsers { get; set; }

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

            //------------------------ Simple Table --------------------------//
            modelBuilder.Entity<Simple>()
                .HasIndex(u => u.Name)
                .IsUnique();
            //------------------------ Simple Table --------------------------//
        }
    }
}
