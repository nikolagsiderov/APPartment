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

        public DbSet<APPartment.Models.Object> Objects { get; set; }
        public DbSet<APPartment.Models.User> Users { get; set; }
        public DbSet<APPartment.Models.House> Houses { get; set; }
        public DbSet<APPartment.Models.HouseStatus> HouseStatuses { get; set; }
        public DbSet<APPartment.Models.HouseSettings> HouseSettings { get; set; }
        public DbSet<APPartment.Models.Inventory> Inventories { get; set; }
        public DbSet<APPartment.Models.Hygiene> Hygienes { get; set; }
        public DbSet<APPartment.Models.Issue> Issues { get; set; }
        public DbSet<APPartment.Models.Message> Messages { get; set; }
        public DbSet<APPartment.Models.Comment> Comments { get; set; }
        public DbSet<APPartment.Models.Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Object>().ToTable("Object");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<House>().ToTable("House");
            modelBuilder.Entity<HouseStatus>().ToTable("HouseStatus");
            modelBuilder.Entity<HouseSettings>().ToTable("HouseSettings");
            modelBuilder.Entity<Inventory>().ToTable("Inventory");
            modelBuilder.Entity<Hygiene>().ToTable("Hygiene");
            modelBuilder.Entity<Issue>().ToTable("Issue");
            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<Comment>().ToTable("Comment");
            modelBuilder.Entity<Image>().ToTable("Image");
        }
    }
}
