using DAL.Versions.V1.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace DAL.Versions.V1.DataContext
{
    public class DevTicketDatabaseContext : DbContext
    {
        private const bool ACTIVE_TICKET = true;
        private const int REGULAR_USER_TYPE_ID = 2;
        private const int EMPTY_PUNCHES = 0;
        private const string GET_DATE = "GetDate()";

        public class OptionsBuild
        {
            private AppConfiguration settings { get; set; }
            public DbContextOptionsBuilder<DevTicketDatabaseContext> opsBuilder { get; set; }
            public DbContextOptions<DevTicketDatabaseContext> dbOptions { get; set; }

            public OptionsBuild()
            {
                settings = new AppConfiguration("DevTicketDatabase");
                opsBuilder = new DbContextOptionsBuilder<DevTicketDatabaseContext>();
                opsBuilder.UseSqlServer(settings.sqlConnectionString);
                dbOptions = opsBuilder.Options;
            }

        }
        public static OptionsBuild ops = new OptionsBuild();

        public DevTicketDatabaseContext(DbContextOptions<DevTicketDatabaseContext> options)
            : base(options)
        {

        }

        #region Dufault Values
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedDate)
                .HasDefaultValueSql(GET_DATE);

            modelBuilder.Entity<User>()
                .Property(u => u.LastVisited)
                .HasDefaultValueSql(GET_DATE);

            modelBuilder.Entity<User>()
                .Property(u => u.UserTypeId)
                .HasDefaultValue(REGULAR_USER_TYPE_ID);
            #endregion

            #region Ticket User
            modelBuilder.Entity<TicketUser>()
                .Property(u => u.Punch)
                .HasDefaultValue(EMPTY_PUNCHES);

            modelBuilder.Entity<TicketUser>()
                .Property(u => u.Status)
                .HasDefaultValue(ACTIVE_TICKET);

            modelBuilder.Entity<TicketUser>()
                .Property(u => u.CreatedDate)
                .HasDefaultValueSql(GET_DATE);
            #endregion
        }
        #endregion

        public DbSet<Store> Stores { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TicketUser> TicketsUsers { get; set; }
        public DbSet<TicketStore> TicketsStores { get; set; }
    }
}
