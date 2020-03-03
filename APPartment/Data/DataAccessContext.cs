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

        public DbSet<APPartment.Models.Object> Object { get; set; }
        public DbSet<APPartment.Models.User> User { get; set; }
        public DbSet<APPartment.Models.House> House { get; set; }
        public DbSet<APPartment.Models.HouseStatus> HouseStatuses { get; set; }
        public DbSet<APPartment.Models.HouseSettings> HouseSettings { get; set; }
        public DbSet<APPartment.Models.Inventory> Inventory { get; set; }
        public DbSet<APPartment.Models.Hygiene> Hygiene { get; set; }
        public DbSet<APPartment.Models.Issue> Issue { get; set; }
        public DbSet<APPartment.Models.Message> Message { get; set; }
        public DbSet<APPartment.Models.Comment> Comment { get; set; }
        public DbSet<APPartment.Models.Image> Image { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Object>().ToTable("Object");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<House>().ToTable("House");
            modelBuilder.Entity<HouseStatus>().ToTable("HouseStatuses");
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
