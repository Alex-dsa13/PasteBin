using AuthService.Models;
using AuthService.Properties;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataBase
{
    public class Db : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Resources.ConnectionString);
        }
    }
}
