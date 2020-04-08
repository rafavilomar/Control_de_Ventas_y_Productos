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
    public class ventasController : Controller
    {
        private SistemaFacturacionEntities db = new SistemaFacturacionEntities();

        // GET: ventas
        public ActionResult Index()
        {
            var facturas = db.Facturas.Include(f => f.Cliente);
            return View(facturas.ToList());
        }
        [HttpPost]
        public ActionResult Index(string cliente, DateTime? fecha, bool sumatoria, bool conteo, bool promedio, bool valorMax, bool valorMin)
        {
            ViewBag.Sumatoria = "";
            ViewBag.Conteo = "";
            ViewBag.Promedio = "";
            ViewBag.ValorMax = "";
            ViewBag.ValorMin = "";
            if (cliente == string.Empty && !fecha.HasValue)
            {
                if (sumatoria == true) { ViewBag.Sumatoria = db.Facturas.ToList().Sum(a => a.Total); }
                if (conteo == true) { ViewBag.Conteo = db.Facturas.ToList().Count(); }
                if (sumatoria == true) { ViewBag.Promedio = db.Facturas.ToList().Average(a => a.Total); }
                if (valorMax == true) { ViewBag.ValorMax = db.Facturas.ToList().Max(a=>a.Total); }
                if (valorMin == true) { ViewBag.ValorMin = db.Facturas.ToList().Min(a => a.Total); }
                return View(db.Facturas.ToList());
            }
            else
            {
                if (cliente!= string.Empty && !fecha.HasValue)
                {
                    var abc = from a in db.Facturas
                              join b in db.Clientes on a.idCliente equals b.id
                              where b.Nombre == cliente
                              select a;
                    if (sumatoria == true) { ViewBag.Sumatoria = abc.Sum(a => a.Total); }
                    if (conteo == true) { ViewBag.Conteo = abc.Count(); }
                    if (sumatoria == true) { ViewBag.Promedio = abc.Average(a => a.Total); }
                    if (valorMax == true) { ViewBag.ValorMax = abc.Max(a => a.Total); }
                    if (valorMin == true) { ViewBag.ValorMin = abc.Min(a => a.Total); }
                    return View(abc);
                }
                else if (cliente == string.Empty && fecha.HasValue)
                {
                    var abc = from a in db.Facturas
                              where a.Fecha == fecha
                              select a;
                    if (sumatoria == true) { ViewBag.Sumatoria = abc.Sum(a => a.Total); }
                    if (conteo == true) { ViewBag.Conteo = abc.Count(); }
                    if (sumatoria == true) { ViewBag.Promedio = abc.Average(a => a.Total); }
                    if (valorMax == true) { ViewBag.ValorMax = abc.Max(a => a.Total); }
                    if (valorMin == true) { ViewBag.ValorMin = abc.Min(a => a.Total); }
                    return View(abc);
                }
                else
                {
                    var abc = from a in db.Facturas
                              join b in db.Clientes on a.idCliente equals b.id
                              where a.Fecha == fecha & b.Nombre == cliente
                              select a;
                    if (sumatoria == true) { ViewBag.Sumatoria = abc.Sum(a => a.Total); }
                    if (conteo == true) { ViewBag.Conteo = abc.Count(); }
                    if (sumatoria == true) { ViewBag.Promedio = abc.Average(a => a.Total); }
                    if (valorMax == true) { ViewBag.ValorMax = abc.Max(a => a.Total); }
                    if (valorMin == true) { ViewBag.ValorMin = abc.Min(a => a.Total); }
                    return View(abc);
                }
            }
        }
        // GET: ventas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            venta venta = db.ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // GET: ventas/Create
        public ActionResult Create()
        {
            Factura product = new Factura
            {
            };
            db.Facturas.Add(product);
            db.SaveChanges();
            var dato = (from a in db.Facturas
                        select a.id);
            ViewBag.idmax = dato.Max();
            var dato2 = (from a in db.ventas
                         where a.idFactura == dato.Max()
                        select a);
            ViewBag.idCliente = new SelectList(db.Clientes, "id", "Nombre");
            ViewBag.idFactura = new SelectList(db.Facturas, "id", "id");
            ViewBag.idProducto = new SelectList(db.Productos, "id", "Nombre");
            var ventas = db.ventas.Include(v => v.Factura).Include(v => v.Producto);
            return View(dato2);
        }

        // POST: ventas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int cantidad, int idProducto, int idFactura)
        {
            venta product = new venta
            {
                cantidad = cantidad,
                idProducto = idProducto,
                idFactura = idFactura
            };
            db.ventas.Add(product);
                db.SaveChanges();
            var dato = (from a in db.Facturas
                        select a.id);
            ViewBag.idmax = dato.Max();
            var dato2 = (from a in db.ventas
                         where a.idFactura == dato.Max()
                             select a);
            ViewBag.idCliente = new SelectList(db.Clientes, "id", "Nombre");
            ViewBag.idFactura = new SelectList(db.Facturas, "id", "id", product.idFactura);
            ViewBag.idProducto = new SelectList(db.Productos, "id", "Nombre", product.idProducto);
            var ventas = db.ventas.Include(v => v.Factura).Include(v => v.Producto);
            return View(dato2);
            
        }
        //GET
        public ActionResult Facturar(int idFactura, DateTime fecha, int idCliente)
        {
            ViewBag.Fecha = fecha;
            ViewBag.idCliente = idCliente;
            var dato = (from a in db.Facturas
                        select a.id);
            ViewBag.idmax = dato.Max();
            var total = (from a in db.ventas
                         join b in db.Productos on a.idProducto equals b.id
                         where a.idFactura == idFactura
                         select b.Precio);
            var cantidades = (from a in db.ventas
                              join b in db.Productos on a.idProducto equals b.id
                              where a.idFactura == idFactura
                              select a.cantidad);
            var categoria = (from a in db.Clientes
                             join b in db.Facturas on a.id equals b.idCliente
                             where b.id == idFactura
                             select a.Categoria);
            ViewBag.categoria = categoria;
            ViewBag.sumprecios = total.Sum();
            decimal? sumprecios = total.Sum();
            decimal? descuento;
            bool desc;
            ViewBag.desc = "false";
            decimal? ImporteBruto = (cantidades.Sum() * sumprecios) / cantidades.Count();
            ViewBag.ImporteBruto = ImporteBruto;
            if (String.Equals(ViewBag.categoria, "Premium"))
            {
                ViewBag.desc = "true";
                descuento = ImporteBruto * 0.05M;
            }
            else { ViewBag.desc = "false"; descuento = 0; }
            decimal? importe = ImporteBruto - descuento;
            ViewBag.Importe = importe;
            decimal? itbis = sumprecios * 0.18M;
            ViewBag.itbis = itbis;
            decimal? totalFinal = importe + itbis;
            ViewBag.total = totalFinal;



            
            ViewBag.idFactura = idFactura;
            return View();
        }
        [HttpPost]
        public ActionResult Facturar(int idFactura, DateTime Fecha, decimal Total, decimal Importe, string Descuento, decimal ITBIS, int idCliente)
        {
            bool desc;
            if (String.Equals(Descuento, "true")) { desc = true; }
            else { desc = false; }
            
            /*Factura facturacion = new Factura
            {
                Total = ViewBag.total,
                Descuento = ViewBag.desc,
                Importe = ViewBag.Importe,
                ITBIS = ViewBag.itbis

            };
            db.Entry(facturacion).State = EntityState.Modified;
            db.SaveChanges();*/
            var dato = (from a in db.Facturas
                        select a.id);
            ViewBag.idmax = dato.Max();
            var query = (from a in db.Facturas
                         where a.id == dato.Max()
                         select a).FirstOrDefault();

            query.Total = Total;
            query.Descuento = desc;
            query.Importe = Importe;
            query.ITBIS = ITBIS;
            query.idCliente = idCliente;
            query.Fecha = Fecha;

            db.SaveChanges();
            ViewBag.idFactura = new SelectList(db.Facturas, "id", "id", idFactura);

            return RedirectToAction("Index");
        }

        // GET: ventas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            venta venta = db.ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            ViewBag.idFactura = new SelectList(db.Facturas, "id", "id", venta.idFactura);
            ViewBag.idProducto = new SelectList(db.Productos, "id", "Nombre", venta.idProducto);
            return View(venta);
        }

        // POST: ventas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,cantidad,idProducto,idFactura")] venta venta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(venta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idFactura = new SelectList(db.Facturas, "id", "id", venta.idFactura);
            ViewBag.idProducto = new SelectList(db.Productos, "id", "Nombre", venta.idProducto);
            return View(venta);
        }

        // GET: ventas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            venta venta = db.ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // POST: ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            venta venta = db.ventas.Find(id);
            db.ventas.Remove(venta);
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
