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
    public class AcessoriosController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Acessorios/
        public ActionResult Index(int? pagina)
        {
            var acessorio = db.acessorio_SAP;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(acessorio.OrderBy(p => p.ZCODIGOACCESORIOS).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Acessorios/Details/5
        public ActionResult Details(int? ZCODIGOACCESORIOS)
        {
            if (ZCODIGOACCESORIOS == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            acessorio_SAP acessorio = db.acessorio_SAP.Find(ZCODIGOACCESORIOS);
            if (acessorio == null)
            {
                return HttpNotFound();
            }
            return View(acessorio);
        }

        // GET: /Acessorios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Acessorios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "MANDT,ZCODIGOACCESORIOS,ZNOMBREACCESORIOS,ZDESACTIVADO")] acessorio_SAP acessorio)
        {
            if (ModelState.IsValid)
            {
                db.acessorio_SAP.Add(acessorio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(acessorio);
        }

        // GET: /Acessorios/Edit/5
        public ActionResult Edit(int? ZCODIGOACCESORIOS)
        {
            if (ZCODIGOACCESORIOS == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            acessorio_SAP acessorio = db.acessorio_SAP.Find(ZCODIGOACCESORIOS);
            if (acessorio == null)
            {
                return HttpNotFound();
            }
            return View(acessorio);
        }

        // POST: /Acessorios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCODIGOACCESORIOS,ZNOMBREACCESORIOS,ZDESACTIVADO")] acessorio_SAP acessorio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acessorio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(acessorio);
        }

        // GET: /Acessorios/Delete/5
        public ActionResult Delete(int? ZCODIGOACCESORIOS)
        {
            if (ZCODIGOACCESORIOS == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            acessorio_SAP acessorio = db.acessorio_SAP.Find(ZCODIGOACCESORIOS);
            if (acessorio == null)
            {
                return HttpNotFound();
            }
            return View(acessorio);
        }

        // POST: /Acessorios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ZCODIGOACCESORIOS)
        {
            acessorio_SAP acessorio = db.acessorio_SAP.Find(ZCODIGOACCESORIOS);
            db.acessorio_SAP.Remove(acessorio);
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
