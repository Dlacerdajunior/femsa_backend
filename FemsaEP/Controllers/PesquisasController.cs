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
    public class PesquisasController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Pesquisas/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var enquete = db.enquete.Include(e => e.cluster_SAP);
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(enquete.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Pesquisas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            enquete enquete = db.enquete.Find(id);
            if (enquete == null)
            {
                return HttpNotFound();
            }
            return View(enquete);
        }

        // GET: /Pesquisas/Create
        public ActionResult Create()
        {
            ViewBag.id_cluster = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE");
            return View();
        }

        // POST: /Pesquisas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,titulo,vigencia,data_limite,status,data_inicio,mandante,ZCODCLUST")] enquete enquete)
        {
            if (ModelState.IsValid)
            {
                db.enquete.Add(enquete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_cluster = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE", enquete.ZCODCLUST);
            return View(enquete);
        }

        // GET: /Pesquisas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            enquete enquete = db.enquete.Find(id);
            if (enquete == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cluster = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE", enquete.ZCODCLUST);
            return View(enquete);
        }

        // POST: /Pesquisas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,titulo,vigencia,data_limite,status,ZCODCLUST,data_inicio")] enquete enquete)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enquete).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cluster = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE", enquete.ZCODCLUST);
            return View(enquete);
        }

        // GET: /Pesquisas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            enquete enquete = db.enquete.Find(id);
            if (enquete == null)
            {
                return HttpNotFound();
            }
            return View(enquete);
        }

        // POST: /Pesquisas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            enquete enquete = db.enquete.Find(id);
            db.enquete.Remove(enquete);
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
