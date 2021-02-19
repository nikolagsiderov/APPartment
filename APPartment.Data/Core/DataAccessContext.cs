using APPartment.Data.Server.Models.Core;
using APPartment.Data.Server.Models.MetaObjects;
using APPartment.Data.Server.Models.Objects;
using Microsoft.EntityFrameworkCore;
using System;
using Object = APPartment.Data.Server.Models.Core.Object;

namespace APPartment.Data.Core
{
    [Obsolete]
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
        public DbSet<HomeSetting> HomeSettings { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Hygiene> Hygienes { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Chore> Chores { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<LinkType> LinkTypes { get; set; }
        public DbSet<Link> Links { get; set; }
    }
}
