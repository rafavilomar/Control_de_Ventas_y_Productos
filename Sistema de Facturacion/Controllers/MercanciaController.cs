using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sistema_de_Facturacion.Models;

namespace Sistema_de_Facturacion.Controllers
{
    public class MercanciaController : Controller
    {
        private SistemaFacturacionEntities db = new SistemaFacturacionEntities();

        // GET: Mercancia
        public ActionResult Index()
        {
            var almacens = db.almacens.Include(a => a.Producto);
            return View(almacens.ToList());
        }

        // GET: Mercancia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            almacen almacen = db.almacens.Find(id);
            if (almacen == null)
            {
                return HttpNotFound();
            }
            return View(almacen);
        }

        // GET: Mercancia/Create
        public ActionResult Create()
        {
            ViewBag.idProducto = new SelectList(db.Productos, "id", "Nombre");
            return View();
        }

        // POST: Mercancia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,cantidad,idProducto")] almacen almacen)
        {
            if (ModelState.IsValid)
            {
                db.almacens.Add(almacen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idProducto = new SelectList(db.Productos, "id", "Nombre", almacen.idProducto);
            return View(almacen);
        }

        // GET: Mercancia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            almacen almacen = db.almacens.Find(id);
            if (almacen == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProducto = new SelectList(db.Productos, "id", "Nombre", almacen.idProducto);
            return View(almacen);
        }

        // POST: Mercancia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,cantidad,idProducto")] almacen almacen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(almacen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProducto = new SelectList(db.Productos, "id", "Nombre", almacen.idProducto);
            return View(almacen);
        }

        // GET: Mercancia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            almacen almacen = db.almacens.Find(id);
            if (almacen == null)
            {
                return HttpNotFound();
            }
            return View(almacen);
        }

        // POST: Mercancia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            almacen almacen = db.almacens.Find(id);
            db.almacens.Remove(almacen);
            db.SaveChanges();
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
