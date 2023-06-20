using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FemsaEP.Models;
using PagedList;

namespace FemsaEP.Controllers
{
    public class FornecedorController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Fornecedor/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var provedor_sap = db.provedor_SAP.Include(p => p.territorio_SAP);
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(provedor_sap.OrderBy(p => p.ZCODPROVE).ToPagedList(numeroPagina, tamanhoPagina));
        }

        public bool verificaigual(decimal ZCODPROVE)
        {
            var check = db.provedor_SAP.Where(u => u.ZCODPROVE == ZCODPROVE);

            if (check.Count() != 0)
            {
                return false;
            }
            return true;
        }

        // GET: /Fornecedor/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            provedor_SAP provedor_sap = db.provedor_SAP.Find(id);
            if (provedor_sap == null)
            {
                return HttpNotFound();
            }
            return View(provedor_sap);
        }

        // GET: /Fornecedor/Create
        public ActionResult Create()
        {
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE");
            return View();
        }

        // POST: /Fornecedor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANDT,ZCODPROVE,ZNOMBRE,ZRAZONSOC,ZCNPJ,ZCONTRATO,ZTELEFONO,ZEMAIL,ZTERRITOR,ZCODSAP,ZVALLEITO,ZVALORPER,ZDESACTIV,VALORM2")] provedor_SAP provedor_sap)
        {
            if (ModelState.IsValid)
            {
                db.provedor_SAP.Add(provedor_sap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", provedor_sap.ZTERRITOR);
            return View(provedor_sap);
        }

        // GET: /Fornecedor/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            provedor_SAP provedor_sap = db.provedor_SAP.Find(id);
            if (provedor_sap == null)
            {
                return HttpNotFound();
            }
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", provedor_sap.ZTERRITOR);
            return View(provedor_sap);
        }

        // POST: /Fornecedor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCODPROVE,ZNOMBRE,ZRAZONSOC,ZCNPJ,ZCONTRATO,ZTELEFONO,ZEMAIL,ZTERRITOR,ZCODSAP,ZVALLEITO,ZVALORPER,ZDESACTIV,VALORM2")] provedor_SAP provedor_sap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(provedor_sap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", provedor_sap.ZTERRITOR);
            return View(provedor_sap);
        }

        // GET: /Fornecedor/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            provedor_SAP provedor_sap = db.provedor_SAP.Find(id);
            if (provedor_sap == null)
            {
                return HttpNotFound();
            }
            return View(provedor_sap);
        }

        // POST: /Fornecedor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            provedor_SAP provedor_sap = db.provedor_SAP.Find(id);
            db.provedor_SAP.Remove(provedor_sap);
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
