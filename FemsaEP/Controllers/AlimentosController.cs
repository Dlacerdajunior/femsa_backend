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
    public class AlimentosController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Alimentos/
        public ActionResult Index(int? pagina)
        {
            var alimento = db.alimento_SAP;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(alimento.OrderBy(p => p.ZCODIGOALIMENTO).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Alimentos/Details/5
        public ActionResult Details(int? ZCODIGOALIMENTO)
        {
            if (ZCODIGOALIMENTO == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            alimento_SAP alimento = db.alimento_SAP.Find(ZCODIGOALIMENTO);
            if (alimento == null)
            {
                return HttpNotFound();
            }
            return View(alimento);
        }

        // GET: /Alimentos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Alimentos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigo,descricao")] alimento_SAP alimento)
        {
            if (ModelState.IsValid)
            {
                db.alimento_SAP.Add(alimento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(alimento);
        }

        // GET: /Alimentos/Edit/5
        public ActionResult Edit(int? ZCODIGOALIMENTO)
        {
            if (ZCODIGOALIMENTO == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            alimento_SAP alimento = db.alimento_SAP.Find(ZCODIGOALIMENTO);
            if (alimento == null)
            {
                return HttpNotFound();
            }
            return View(alimento);
        }

        // POST: /Alimentos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigo,descricao")] alimento_SAP alimento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alimento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(alimento);
        }

        // GET: /Alimentos/Delete/5
        public ActionResult Delete(int? ZCODIGOALIMENTO)
        {
            if (ZCODIGOALIMENTO == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            alimento_SAP alimento = db.alimento_SAP.Find(ZCODIGOALIMENTO);
            if (alimento == null)
            {
                return HttpNotFound();
            }
            return View(alimento);
        }

        // POST: /Alimentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ZCODIGOALIMENTO)
        {
            alimento_SAP alimento = db.alimento_SAP.Find(ZCODIGOALIMENTO);
            db.alimento_SAP.Remove(alimento);
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
