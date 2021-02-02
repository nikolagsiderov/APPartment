using APPartment.Data.Models.Core;
using APPartment.Data.Models.MetaObjects;
using APPartment.Data.Models.Objects;
using Microsoft.EntityFrameworkCore;

namespace APPartment.Data.Core
{
    public class DataAccessContext : DbContext
    {
        public DataAccessContext(DbContextOptions<DataAccessContext> options)
            : base(options)
        {
        }

        // Enables seeing sensetive data (Id, etc.) in SQL error
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.EnableSensitiveDataLogging(true);
        //}

        public DbSet<ObjectType> ObjectTypes { get; set; }
        public DbSet<Object> Objects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Home> Homes { get; set; }
        public DbSet<HomeUser> HomeUsers { get; set; }
        public DbSet<HomeStatus> HomeStatuses { get; set; }
        public DbSet<HomeSettings> HomeSettings { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Hygiene> Hygienes { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Chore> Chores { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Audit> Audits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // These values MUST be unique and in sync with APPartment.Enums.ObjectTypes
            modelBuilder.Entity<ObjectType>().ToTable("ObjectType").HasData(
                new ObjectType { Id = 1, Name = "User" },
                new ObjectType { Id = 2, Name = "Home" },
                new ObjectType { Id = 3, Name = "HomeStatus" },
                new ObjectType { Id = 4, Name = "HomeSettings" },
                new ObjectType { Id = 5, Name = "Inventory" },
                new ObjectType { Id = 6, Name = "Hygiene" },
                new ObjectType { Id = 7, Name = "Issue" },
                new ObjectType { Id = 8, Name = "Message" },
                new ObjectType { Id = 9, Name = "Comment" },
                new ObjectType { Id = 10, Name = "Image" },
                new ObjectType { Id = 12, Name = "Survey" },
                new ObjectType { Id = 13, Name = "Chore" },
                new ObjectType { Id = 14, Name = "HomeUser" },
                new ObjectType { Id = 15, Name = "Audit" }
            );
            modelBuilder.Entity<Object>().ToTable("Object");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Home>().ToTable("Home");
            modelBuilder.Entity<HomeUser>().ToTable("HomeUser");
            modelBuilder.Entity<HomeStatus>().ToTable("HomeStatus");
            modelBuilder.Entity<HomeSettings>().ToTable("HomeSettings");
            modelBuilder.Entity<Inventory>().ToTable("Inventory");
            modelBuilder.Entity<Hygiene>().ToTable("Hygiene");
            modelBuilder.Entity<Issue>().ToTable("Issue");
            modelBuilder.Entity<Survey>().ToTable("Survey");
            modelBuilder.Entity<Chore>().ToTable("Chore");
            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<Comment>().ToTable("Comment");
            modelBuilder.Entity<Image>().ToTable("Image");
            modelBuilder.Entity<Audit>().ToTable("Audit");
        }
    }
}
