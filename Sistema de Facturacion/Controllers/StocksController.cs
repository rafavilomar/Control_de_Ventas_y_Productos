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
    public class StocksController : Controller
    {
        private SistemaFacturacionEntities db = new SistemaFacturacionEntities();

        // GET: Stocks
        public ActionResult Index()
        {
            var stocks = db.Stocks.Include(s => s.Producto).Include(s => s.Proveedore);
            return View(stocks.ToList());
        }
        [HttpPost]
        public ActionResult Index(string producto, DateTime? fecha, string proveedor)
        {
            if (producto == string.Empty && !fecha.HasValue && proveedor== string.Empty)
            {
                return View(db.Stocks.ToList());
            }
            else
            {
                if (producto != string.Empty && !fecha.HasValue && proveedor == string.Empty)
                {
                    var abc = from a in db.Stocks
                              join b in db.Productos on a.idProducto equals b.id
                              where b.Nombre == producto
                              orderby a.idProducto
                              select a;
                    return View(abc);
                }
                else if (producto == string.Empty && !fecha.HasValue && proveedor != string.Empty)
                {
                    var abc = from a in db.Stocks
                              join b in db.Proveedores on a.idProveedores equals b.id
                              where b.Nombre == proveedor
                              orderby a.idProveedores
                              select a;
                    return View(abc);
                }
                else if (producto == string.Empty && fecha.HasValue && proveedor == string.Empty)
                {
                    var abc = from a in db.Stocks
                              where a.Fecha == fecha
                              orderby a.Fecha
                              select a;
                    return View(abc);
                }
                else
                {
                    var abc = from a in db.Stocks
                              join b in db.Productos on a.idProducto equals b.id
                              join c in db.Proveedores on a.idProveedores equals c.id
                              where b.Nombre == producto && a.Fecha == fecha && c.Nombre == proveedor
                              select a;
                    return View(abc);
                }
            }
        }
        // GET: Stocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // GET: Stocks/Create
        public ActionResult Create()
        {
            ViewBag.idProducto = new SelectList(db.Productos, "id", "Nombre");
            ViewBag.idProveedores = new SelectList(db.Proveedores, "id", "Nombre");
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Cantidad,Fecha,idProducto,idProveedores")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Stocks.Add(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idProducto = new SelectList(db.Productos, "id", "Nombre", stock.idProducto);
            ViewBag.idProveedores = new SelectList(db.Proveedores, "id", "Nombre", stock.idProveedores);
            return View(stock);
        }

        // GET: Stocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProducto = new SelectList(db.Productos, "id", "Nombre", stock.idProducto);
            ViewBag.idProveedores = new SelectList(db.Proveedores, "id", "Nombre", stock.idProveedores);
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Cantidad,Fecha,idProducto,idProveedores")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProducto = new SelectList(db.Productos, "id", "Nombre", stock.idProducto);
            ViewBag.idProveedores = new SelectList(db.Proveedores, "id", "Nombre", stock.idProveedores);
            return View(stock);
        }

        // GET: Stocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stock stock = db.Stocks.Find(id);
            db.Stocks.Remove(stock);
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
