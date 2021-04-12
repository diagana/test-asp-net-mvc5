using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using testApp.Models;

namespace testApp.DAL
{
    public class StockContext: DbContext
    {
        public StockContext() : base("StockContext")
        {

        }
        public DbSet<Product> products { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}