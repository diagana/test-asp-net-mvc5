using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace testApp.Models
{
    public class Categorie
    {
        public int Id { get; set;  }

        [Display(Name ="Categorie Name")]
        public string Name { get; set; }
        [Display(Name= "Date Creation")]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}",ApplyFormatInEditMode =true)]
        public DateTime DateCreation { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
} 