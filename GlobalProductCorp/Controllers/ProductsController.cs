using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GlobalProductCorp.Models;

namespace GlobalProductCorp.Controllers
{
    public class ProductsController : Controller
    {
        private GlobalProductCorpContext db = new GlobalProductCorpContext();

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            // Validar que haya ingresado id
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Busca el producto en la BD
            Product product = db.Products.Find(id);

            // Valida que producto exista
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Description,Price,Stock")] Product product)
        {
            // Valida el modelo
            if (ModelState.IsValid)
            {
                // Generamos el IVA
                //product.IVA = product.Price * 19 / 100;
                // Adiciona el producto a la BD
                db.Products.Add(product);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Retorna a la vista del producto si el modelo no es válido
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            // Valida que haya ingresado id
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Busca el producto en la BD
            Product product = db.Products.Find(id);

            // Valida que el producto exista
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // POST: Products/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Description,Price,Stock")] Product product)
        {
            // Valida el modelo
            if (ModelState.IsValid)
            {
                // Si el modelo es válido modifica el producto en la BD
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            // Valida que haya ingresado id
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Busca el producto en la BD
            Product product = db.Products.Find(id);

            // Valida que producto exista
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Busca que el producto exista
            Product product = db.Products.Find(id);
            // Elimina el producto
            db.Products.Remove(product);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex;

                throw;
            }

            return RedirectToAction("Index");
        }

        // Libera conexión a BD (cierra)
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
