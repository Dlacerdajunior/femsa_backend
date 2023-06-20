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
    public class ClusterPecaController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /ClusterPeca/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var cluster_peca_sap = db.cluster_peca_SAP.Include(c => c.cluster_SAP).Include(c => c.peca_SAP);

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(cluster_peca_sap.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));

        }

        // GET: /ClusterPeca/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cluster_peca_SAP cluster_peca_sap = db.cluster_peca_SAP.Find(id);
            if (cluster_peca_sap == null)
            {
                return HttpNotFound();
            }
            return View(cluster_peca_sap);
        }

        // GET: /ClusterPeca/Create
        public ActionResult Create()
        {
            ViewBag.ZCODCLUST = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE");
            ViewBag.ZCODPECA = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA");
            return View();
        }

        // POST: /ClusterPeca/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ZCODCLUST,ZCODPECA,id,quantidade")] cluster_peca_SAP cluster_peca_sap)
        {
            if (ModelState.IsValid)
            {
                db.cluster_peca_SAP.Add(cluster_peca_sap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ZCODCLUST = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE", cluster_peca_sap.ZCODCLUST);
            ViewBag.ZCODPECA = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA", cluster_peca_sap.ZCODPECA);
            return View(cluster_peca_sap);
        }

        // GET: /ClusterPeca/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cluster_peca_SAP cluster_peca_sap = db.cluster_peca_SAP.Find(id);
            if (cluster_peca_sap == null)
            {
                return HttpNotFound();
            }
            ViewBag.ZCODCLUST = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE", cluster_peca_sap.ZCODCLUST);
            ViewBag.ZCODPECA = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA", cluster_peca_sap.ZCODPECA);
            return View(cluster_peca_sap);
        }

        // POST: /ClusterPeca/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ZCODCLUST,ZCODPECA,id,quantidade")] cluster_peca_SAP cluster_peca_sap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cluster_peca_sap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ZCODCLUST = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE", cluster_peca_sap.ZCODCLUST);
            ViewBag.ZCODPECA = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA", cluster_peca_sap.ZCODPECA);
            return View(cluster_peca_sap);
        }

        // GET: /ClusterPeca/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cluster_peca_SAP cluster_peca_sap = db.cluster_peca_SAP.Find(id);
            if (cluster_peca_sap == null)
            {
                return HttpNotFound();
            }
            return View(cluster_peca_sap);
        }

        // POST: /ClusterPeca/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cluster_peca_SAP cluster_peca_sap = db.cluster_peca_SAP.Find(id);
            db.cluster_peca_SAP.Remove(cluster_peca_sap);
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
