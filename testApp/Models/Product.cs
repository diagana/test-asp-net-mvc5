using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int CategorieId { get; set; }
        public virtual Categorie Categorie { get; set; }
    }
}