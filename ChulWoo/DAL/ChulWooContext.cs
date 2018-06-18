using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChulWoo.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions; 

namespace ChulWoo.DAL
{
    public class ChulWooContext : DbContext
    {
        public ChulWooContext() : base("ChulWooContext")
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Personnel> Personnels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}