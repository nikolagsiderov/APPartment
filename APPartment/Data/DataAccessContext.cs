using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using APPartment.Models;

namespace APPartment.Data
{
    public class DataAccessContext : IdentityDbContext
    {
        public DataAccessContext(DbContextOptions<DataAccessContext> options)
            : base(options)
        {
        }

        public DbSet<APPartment.Models.House> House { get; set; }
        public DbSet<APPartment.Models.Inventory> Inventory { get; set; }
        public DbSet<APPartment.Models.Hygiene> Hygiene { get; set; }
        public DbSet<APPartment.Models.Issue> Issue { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<House>().ToTable("House");
            modelBuilder.Entity<Inventory>().ToTable("Inventory");
            modelBuilder.Entity<Hygiene>().ToTable("Hygiene");
            modelBuilder.Entity<Issue>().ToTable("Issue");
        }
    }
}
