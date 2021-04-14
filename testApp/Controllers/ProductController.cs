using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using testApp.DAL;
using testApp.Models;
using PagedList;

namespace testApp.Controllers
{
    public class ProductController : Controller
    {
        private StockContext db = new StockContext();

        // GET: Product
        public ActionResult Index(String sortOrder, String currentFilter, String searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var produits = from p in db.products.Include(p => p.Categorie) select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                produits = produits.Where(p => p.Name.Contains(searchString) || p.Categorie.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    produits = produits.OrderByDescending(p => p.Name);
                    break;
                default:
                    produits = produits.OrderBy(p => p.Price);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(produits.ToPagedList(pageNumber, pageSize));
            //var products = db.products.Include(p => p.Categorie);
            //return View(produits.ToList());
        }

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.CategorieId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Product/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Price,CategorieId")] Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.CategorieId = new SelectList(db.Categories, "Id", "Name", product.CategorieId);
            }
            catch (DataException ex)
            {
                ModelState.AddModelError("Error", "An Error ocurred while savig data : " + ex);

            }
            return View(product);

        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategorieId = new SelectList(db.Categories, "Id", "Name", product.CategorieId);
            return View(product);
        }

        // POST: Product/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Name,Price,CategorieId")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(product).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CategorieId = new SelectList(db.Categories, "Id", "Name", product.CategorieId);
        //    return View(product);
        //}
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productUpdate = db.products.Find(id);
            if (TryUpdateModel(productUpdate, "", new string[] { "Name", "Price", "CategorieId" }))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DataException ex)
                {
                    ModelState.AddModelError("error", "An error occured while updating product : " + ex);
                }
            }

            ViewBag.CategorieId = new SelectList(db.Categories, "Id", "Name", productUpdate.CategorieId);
            return View(productUpdate);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. An error occured while deleting operation running.";
            }
            Product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Product product = db.products.Find(id);
                db.products.Remove(product);
                db.SaveChanges();
            }
            catch (DataException ex)
            {
                ModelState.AddModelError("Error", "An error occured while deleting an element : " + ex);
                return RedirectToAction("Delete", new { id = id, saveChagesError = true });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
