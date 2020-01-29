using Microsoft.EntityFrameworkCore;
using APPartment.Models;

namespace APPartment.Data
{
    public class DataAccessContext : DbContext
    {
        public DataAccessContext(DbContextOptions<DataAccessContext> options)
            : base(options)
        {
        }

        public DbSet<APPartment.Models.User> User { get; set; }
        public DbSet<APPartment.Models.House> House { get; set; }
        public DbSet<APPartment.Models.HouseSettings> HouseSettings { get; set; }
        public DbSet<APPartment.Models.Inventory> Inventory { get; set; }
        public DbSet<APPartment.Models.Hygiene> Hygiene { get; set; }
        public DbSet<APPartment.Models.Issue> Issue { get; set; }
        public DbSet<APPartment.Models.Message> Message { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<House>().ToTable("House");
            modelBuilder.Entity<HouseSettings>().ToTable("HouseSettings");
            modelBuilder.Entity<Inventory>().ToTable("Inventory");
            modelBuilder.Entity<Hygiene>().ToTable("Hygiene");
            modelBuilder.Entity<Issue>().ToTable("Issue");
            modelBuilder.Entity<Message>().ToTable("Message");
        }
    }
}
