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
    public class MotivoRecusaController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /MotivoRecusa/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var motivo_de_recuso = db.motivo_de_recuso_SAP;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(motivo_de_recuso.OrderBy(p => p.ZCODMOTIV).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /MotivoRecusa/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            motivo_de_recuso_SAP motivo_de_recuso_sap = db.motivo_de_recuso_SAP.Find(id);
            if (motivo_de_recuso_sap == null)
            {
                return HttpNotFound();
            }
            return View(motivo_de_recuso_sap);
        }

        // GET: /MotivoRecusa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /MotivoRecusa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MANDT,ZCODMOTIV,ZNOMMOTIV,ZDESACTIV,ZCODMOTI2,descricao")] motivo_de_recuso_SAP motivo_de_recuso_sap)
        {
            if (ModelState.IsValid)
            {
                db.motivo_de_recuso_SAP.Add(motivo_de_recuso_sap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(motivo_de_recuso_sap);
        }

        public bool verificaigual(decimal ZCODMOTIV)
        {
            var check = db.motivo_de_recuso_SAP.Where(u => u.ZCODMOTIV == ZCODMOTIV);

            if (check.Count() != 0)
            {
                return false;
            }
            return true;
        }

        // GET: /MotivoRecusa/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            motivo_de_recuso_SAP motivo_de_recuso_sap = db.motivo_de_recuso_SAP.Find(id);
            if (motivo_de_recuso_sap == null)
            {
                return HttpNotFound();
            }
            return View(motivo_de_recuso_sap);
        }

        // POST: /MotivoRecusa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MANDT,ZCODMOTIV,ZNOMMOTIV,ZDESACTIV,ZCODMOTI2,descricao")] motivo_de_recuso_SAP motivo_de_recuso_sap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(motivo_de_recuso_sap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(motivo_de_recuso_sap);
        }

        // GET: /MotivoRecusa/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            motivo_de_recuso_SAP motivo_de_recuso_sap = db.motivo_de_recuso_SAP.Find(id);
            if (motivo_de_recuso_sap == null)
            {
                return HttpNotFound();
            }
            return View(motivo_de_recuso_sap);
        }

        // POST: /MotivoRecusa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            motivo_de_recuso_SAP motivo_de_recuso_sap = db.motivo_de_recuso_SAP.Find(id);
            db.motivo_de_recuso_SAP.Remove(motivo_de_recuso_sap);
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
