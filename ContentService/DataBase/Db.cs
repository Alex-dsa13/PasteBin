using ContentService.Models.DataBase;
using ContentService.Properties;
using Microsoft.EntityFrameworkCore;

namespace ContentService.DataBase
{
    public class Db : DbContext
    {
        public DbSet<Record> Records { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Resources.ConnectionString);
        }
    }
}
