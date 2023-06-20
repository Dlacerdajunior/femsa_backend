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
    public class CanaisController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Canais/
        public ActionResult Index(int? pagina)
        {
            var canal = db.canal_SAP;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(canal.OrderBy(p => p.ZCODCANAL).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Canais/Details/5
        public ActionResult Details(int? ZCODCANAL)
        {
            if (ZCODCANAL == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            canal_SAP canal = db.canal_SAP.Find(ZCODCANAL);
            if (canal == null)
            {
                return HttpNotFound();
            }
            return View(canal);
        }

        // GET: /Canais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Canais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "MANDT,ZCODCANAL,ZNOMCANAL,ZDESACTIV")] canal_SAP canal)
        {
            if (ModelState.IsValid)
            {
                db.canal_SAP.Add(canal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(canal);
        }

        // GET: /Canais/Edit/5
        public ActionResult Edit(int? ZCODCANAL)
        {
            if (ZCODCANAL == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            canal_SAP canal = db.canal_SAP.Find(ZCODCANAL);
            if (canal == null)
            {
                return HttpNotFound();
            }
            return View(canal);
        }

       

        // POST: /Canais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCODCANAL,ZNOMCANAL,ZDESACTIV")] canal_SAP canal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(canal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(canal);
        }

        // GET: /Canais/Delete/5
        public ActionResult Delete(int? ZCODCANAL)
        {
            if (ZCODCANAL == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            canal_SAP canal = db.canal_SAP.Find(ZCODCANAL);
            if (canal == null)
            {
                return HttpNotFound();
            }
            return View(canal);
        }

        // POST: /Canais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ZCODCANAL)
        {
            canal_SAP canal = db.canal_SAP.Find(ZCODCANAL);
            db.canal_SAP.Remove(canal);
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
