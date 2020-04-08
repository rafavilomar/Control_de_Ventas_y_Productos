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
            //var abc = from a in db.almacens
              //        select a;
            //return View(abc);
        }
        [HttpPost]
        public ActionResult Index(string producto, DateTime? fecha, string proveedor, bool sumatoria, bool conteo, bool promedio)
        {
            ViewBag.Sumatoria = "";
            ViewBag.Conteo = "";
            ViewBag.Promedio = "";
            if (producto == string.Empty && !fecha.HasValue && proveedor == string.Empty)
            {
                if (sumatoria == true) { ViewBag.Sumatoria = db.Stocks.ToList().Sum(a => a.Cantidad); }
                if (conteo == true) { ViewBag.Conteo = db.Stocks.ToList().Count(); }
                if (sumatoria == true)
                {
                    var abc = (from a in db.Productos
                               join b in db.Stocks on a.id equals b.idProducto
                               join c in db.Proveedores on b.idProveedores equals c.id
                               where a.Nombre == producto || b.Fecha == fecha || c.Nombre == proveedor
                               select b.Cantidad);
                    var precio = from a in db.Productos
                                 where a.Nombre == producto
                                 select a.Precio;
                    ViewBag.f = abc.Sum();
                    ViewBag.g = precio;
                    ViewBag.Promedio = ViewBag.f * ViewBag.g / ViewBag.f;
                }
                return View(db.Stocks.ToList());
            }
            else
            {
                if (producto != string.Empty && !fecha.HasValue && proveedor == string.Empty)
                {
                    var abc = (from a in db.Productos
                               join b in db.Stocks on a.id equals b.idProducto
                               join c in db.Proveedores on b.idProveedores equals c.id
                               where a.Nombre == producto || b.Fecha == fecha || c.Nombre == proveedor
                               select b.Cantidad);
                    var precio = from a in db.Productos
                                 where a.Nombre == producto
                                 select a.Precio;
                    ViewBag.f = abc.Sum();
                    ViewBag.g = precio;
                    ViewBag.Promedio = ViewBag.f * ViewBag.g / ViewBag.f;
                    return View(db.Stocks.ToList());
                }
                else if (producto == string.Empty && !fecha.HasValue && proveedor != string.Empty)
                {
                    var abc = (from a in db.Productos
                               join b in db.Stocks on a.id equals b.idProducto
                               join c in db.Proveedores on b.idProveedores equals c.id
                               where a.Nombre == producto || b.Fecha == fecha || c.Nombre == proveedor
                               select b.Cantidad);
                    var precio = from a in db.Productos
                                 where a.Nombre == producto
                                 select a.Precio;
                    ViewBag.f = abc.Sum();
                    ViewBag.g = precio;
                    ViewBag.Promedio = ViewBag.f * ViewBag.g / ViewBag.f;
                    return View(db.Stocks.ToList());
                }
                else if (producto == string.Empty && fecha.HasValue && proveedor == string.Empty)
                {
                    var abc = (from a in db.Productos
                               join b in db.Stocks on a.id equals b.idProducto
                               join c in db.Proveedores on b.idProveedores equals c.id
                               where a.Nombre == producto || b.Fecha == fecha || c.Nombre == proveedor
                               select b.Cantidad);
                    var precio = from a in db.Productos
                                 where a.Nombre == producto
                                 select a.Precio;
                    ViewBag.f = abc.Sum();
                    ViewBag.g = precio;
                    ViewBag.Promedio = ViewBag.f * ViewBag.g / ViewBag.f;
                    return View(db.Stocks.ToList());
                }
                else
                {
                    var abc = (from a in db.Productos
                               join b in db.Stocks on a.id equals b.idProducto
                               join c in db.Proveedores on b.idProveedores equals c.id
                               where a.Nombre == producto || b.Fecha == fecha || c.Nombre == proveedor
                               select b.Cantidad);
                    var precio = from a in db.Productos
                                 where a.Nombre == producto
                                 select a.Precio;
                    decimal? f = abc.Sum();
                    ViewBag.g = precio;
                    ViewBag.Promedio = f * ViewBag.g / f;
                    return View(db.Stocks.ToList());
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
        public ActionResult Crear(int cantidad, DateTime fecha, int idproducto, int idproveedores)
        {
            Stock datos = new Stock
            {
                Cantidad = cantidad,
                Fecha = fecha,
                idProducto = idproducto,
                idProveedores = idproveedores
            };
            db.Stocks.Add(datos);
            db.SaveChanges();
            return RedirectToAction("Index");
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
                var dato = (from a in db.almacens
                             where a.idProducto == stock.idProducto
                             select a).FirstOrDefault();

                if (dato.idProducto == stock.idProducto)
                {
                    var query = (from a in db.almacens
                                 where a.idProducto == stock.idProducto
                                 select a).FirstOrDefault();

                    query.cantidad = query.cantidad + stock.Cantidad;

                    db.SaveChanges();
                }
                else
                {
                    almacen datos = new almacen
                    {
                        cantidad = stock.Cantidad,
                        idProducto = stock.idProducto
                    };
                    db.almacens.Add(datos);
                    db.SaveChanges();
                }

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
