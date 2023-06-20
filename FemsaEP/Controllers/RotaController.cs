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
    public class RotaController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Rota/
        public ActionResult Index(int? pagina)
        {
            var rota = db.rota_SAP;
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(rota.OrderBy(p => p.ZCODIRUTA).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Rota/Details/5
        public ActionResult Details(int? ZCODIRUTA)
        {
            if (ZCODIRUTA == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rota_SAP rota = db.rota_SAP.Find(ZCODIRUTA);
            if (rota == null)
            {
                return HttpNotFound();
            }
            return View(rota);
        }

        // GET: /Rota/Create
        public ActionResult Create()
        {
            ViewBag.id_colaborador = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE");
            return View();
        }

        // POST: /Rota/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANDT,ZCODIRUTA,ZEJECVTAS,ZPOSPECTO,ZDESARROL,ZPROVEEDO,ZORIGENMW,ZDISENADO,ZGTECOMER,ZGTEREGIO,ZEJECTRAD,ZPROMOTOR,ZCONSULTO,ZUNIDAD,ZDESACTIV,descricao,id_colaborador")] rota_SAP rota)
        {
            if (ModelState.IsValid)
            {
                db.rota_SAP.Add(rota);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_colaborador = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE", rota.id_colaborador);
            return View(rota);
        }

        // GET: /Rota/Edit/5
        public ActionResult Edit(int? ZCODIRUTA)
        {
            if (ZCODIRUTA == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rota_SAP rota = db.rota_SAP.Find(ZCODIRUTA);
            if (rota == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_colaborador = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE", rota.id_colaborador);
            return View(rota);
        }

        // POST: /Rota/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCODIRUTA,ZEJECVTAS,ZPOSPECTO,ZDESARROL,ZPROVEEDO,ZORIGENMW,ZDISENADO,ZGTECOMER,ZGTEREGIO,ZEJECTRAD,ZPROMOTOR,ZCONSULTO,ZUNIDAD,ZDESACTIV,descricao,id_colaborador")] rota_SAP rota)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rota).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_colaborador = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE", rota.id_colaborador);
            return View(rota);
        }

        // GET: /Rota/Delete/5
        public ActionResult Delete(int? ZCODIRUTA)
        {
            if (ZCODIRUTA == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rota_SAP rota = db.rota_SAP.Find(ZCODIRUTA);
            if (rota == null)
            {
                return HttpNotFound();
            }
            return View(rota);
        }

        // POST: /Rota/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ZCODIRUTA)
        {
            rota_SAP rota = db.rota_SAP.Find(ZCODIRUTA);
            db.rota_SAP.Remove(rota);
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
