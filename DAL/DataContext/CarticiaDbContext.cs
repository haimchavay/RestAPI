using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataContext
{
    public class CarticiaDbContext : DbContext
    {
        public class OptionsBuild
        {
            private AppConfiguration settings { get; set; }
            public  DbContextOptionsBuilder<CarticiaDbContext> opsBuilder { get; set; }
            public DbContextOptions<CarticiaDbContext> dbOptions { get; set; }

            public OptionsBuild()
            {
                settings = new AppConfiguration();
                opsBuilder = new DbContextOptionsBuilder<CarticiaDbContext>();
                opsBuilder.UseSqlServer(settings.sqlConnectionString);
                dbOptions = opsBuilder.Options;
            }

        }
        public static OptionsBuild ops = new OptionsBuild();

        public CarticiaDbContext(DbContextOptions<CarticiaDbContext> options)
            : base(options)
        {

        }
        public DbSet<Store> Stores { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
    }
}
