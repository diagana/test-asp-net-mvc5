using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using testApp.Models;
using System.Data.Entity;

namespace testApp.DAL
{
    public class StockInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<StockContext>
    {
        protected override void Seed(StockContext context)
        {
            base.Seed(context);
            var categories = new List<Categorie>{new Categorie { Name = "fruit", DateCreation = DateTime.Parse("2021/04/10") }, new Categorie { Name = "legume", DateCreation = DateTime.Today }};
            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();
        }
    }
}