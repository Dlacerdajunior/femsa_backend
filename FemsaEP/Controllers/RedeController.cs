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
    public class RedeController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Rede/
        public ActionResult Index(int? pagina)
        {
            var rede = db.rede_SAP;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(rede.OrderBy(p => p.ZCODREDES).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Rede/Details/5
        public ActionResult Details(int? ZCODREDES)
        {
            if (ZCODREDES == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rede_SAP rede = db.rede_SAP.Find(ZCODREDES);
            if (rede == null)
            {
                return HttpNotFound();
            }
            return View(rede);
        }

        // GET: /Rede/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Rede/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "MANDT,ZCODREDES,ZNOMREDES,descricao")] rede_SAP rede)
        {
            if (ModelState.IsValid)
            {
                db.rede_SAP.Add(rede);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rede);
        }

        // GET: /Rede/Edit/5
        public ActionResult Edit(int? ZCODREDES)
        {
            if (ZCODREDES == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rede_SAP rede = db.rede_SAP.Find(ZCODREDES);
            if (rede == null)
            {
                return HttpNotFound();
            }
            return View(rede);
        }

        // POST: /Rede/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCODREDES,ZNOMREDES,descricao")] rede_SAP rede)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rede).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rede);
        }

        // GET: /Rede/Delete/5
        public ActionResult Delete(int? ZCODREDES)
        {
            if (ZCODREDES == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rede_SAP rede = db.rede_SAP.Find(ZCODREDES);
            if (rede == null)
            {
                return HttpNotFound();
            }
            return View(rede);
        }

        // POST: /Rede/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ZCODREDES)
        {
            rede_SAP rede = db.rede_SAP.Find(ZCODREDES);
            db.rede_SAP.Remove(rede);
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
