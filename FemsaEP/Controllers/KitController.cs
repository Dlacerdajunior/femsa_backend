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

namespace FemsaEP.Views
{
    public class KitController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Kit/
        public ActionResult Index(int? pagina)
        {
            var kit = db.kit_SAP;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(kit.OrderBy(p => p.ZCODKITS).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Kit/Details/5
        public ActionResult Details(int? ZCODKITS)
        {
            if (ZCODKITS == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kit_SAP kit = db.kit_SAP.Find(ZCODKITS);
            if (kit == null)
            {
                return HttpNotFound();
            }
            return View(kit);
        }

        // GET: /Kit/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Kit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "MANDT,ZCODKITS,ZNOMKITS,ZDESACTIV,descricao")] kit_SAP kit)
        {
            if (ModelState.IsValid)
            {
                db.kit_SAP.Add(kit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kit);
        }

        // GET: /Kit/Edit/5
        public ActionResult Edit(int? ZCODKITS)
        {
            if (ZCODKITS == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kit_SAP kit = db.kit_SAP.Find(ZCODKITS);
            if (kit == null)
            {
                return HttpNotFound();
            }
            return View(kit);
        }

        // POST: /Kit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCODKITS,ZNOMKITS,ZDESACTIV,descricao")] kit_SAP kit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kit);
        }

        // GET: /Kit/Delete/5
        public ActionResult Delete(int? ZCODKITS)
        {
            if (ZCODKITS == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kit_SAP kit = db.kit_SAP.Find(ZCODKITS);
            if (kit == null)
            {
                return HttpNotFound();
            }
            return View(kit);
        }

        // POST: /Kit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ZCODKITS)
        {
            kit_SAP kit = db.kit_SAP.Find(ZCODKITS);
            db.kit_SAP.Remove(kit);
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
