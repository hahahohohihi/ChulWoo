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

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<Resign> Resigns { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<MaterialBuy> MaterialBuys { get; set; }
        public DbSet<MaterialBuyUnit> MaterialBuyUnits { get; set; }
        public DbSet<MaterialUnitPrice> MaterialUnitPrices { get; set; }
        public DbSet<MaterialName> MaterialNames { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<UploadFile> UploadFiles { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<DailyWork> DailyWorks { get; set; }
        public DbSet<WorkUnit> WorkUnits { get; set; }
        public DbSet<EquipmentUnit> EquipmentUnits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { 
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Employee>().MapToStoredProcedures();
//            modelBuilder.Entity<Personnel>().MapToStoredProcedures();
//           modelBuilder.Entity<Project>().MapToStoredProcedures();
//            modelBuilder.Entity<Company>().MapToStoredProcedures();
        }

    }
}