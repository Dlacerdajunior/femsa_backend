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
    public class SubcanaisController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Subcanais/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var subcanal = db.subcanal_SAP;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(subcanal.OrderBy(p => p.ZCODSUBCA).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Subcanais/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcanal_SAP subcanal_sap = db.subcanal_SAP.Find(id);
            if (subcanal_sap == null)
            {
                return HttpNotFound();
            }
            return View(subcanal_sap);
        }

        // GET: /Subcanais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Subcanais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANDT,ZCODSUBCA,ZNOMSUBCA,descricao")] subcanal_SAP subcanal_sap)
        {
            if (ModelState.IsValid)
            {
                db.subcanal_SAP.Add(subcanal_sap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subcanal_sap);
        }

        public bool verificaigual(string ZCODSUBCA)
        {
            var check = db.subcanal_SAP.Where(u => u.ZCODSUBCA == ZCODSUBCA);

            if (check.Count() != 0)
            {
                return false;
            }
            return true;
        }

        // GET: /Subcanais/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcanal_SAP subcanal_sap = db.subcanal_SAP.Find(id);
            if (subcanal_sap == null)
            {
                return HttpNotFound();
            }
            return View(subcanal_sap);
        }

        // POST: /Subcanais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCODSUBCA,ZNOMSUBCA,descricao")] subcanal_SAP subcanal_sap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subcanal_sap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subcanal_sap);
        }

        // GET: /Subcanais/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcanal_SAP subcanal_sap = db.subcanal_SAP.Find(id);
            if (subcanal_sap == null)
            {
                return HttpNotFound();
            }
            return View(subcanal_sap);
        }

        // POST: /Subcanais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            subcanal_SAP subcanal_sap = db.subcanal_SAP.Find(id);
            db.subcanal_SAP.Remove(subcanal_sap);
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
