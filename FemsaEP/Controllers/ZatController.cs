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
    public class ZatController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();
        //private FemsadbEntities db = new FemsadbEntities();

        // GET: /Zat/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var zat = db.zat_SAP.Include(z => z.territorio_SAP).Include(z => z.unidade_SAP);
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(zat.OrderBy(p => p.ZCODZATS).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Zat/Create
        public ActionResult Create()
        {
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE");
            ViewBag.ZUNIDAD = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE");
            return View();
        }

        // POST: /Zat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANDT,ZCODZATS,ZNOMBRE,ZTERRITOR,ZUNIDAD,ZCIUDAD,ZUF,ZANO,descricao,regiao")] zat_SAP zat_sap)
        {
            if (ModelState.IsValid)
            {
                db.zat_SAP.Add(zat_sap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", zat_sap.ZTERRITOR);
            ViewBag.ZUNIDAD = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE", zat_sap.ZUNIDAD);
            return View(zat_sap);
        }

        // GET: /Zat/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            zat_SAP zat_sap = db.zat_SAP.Find(id);
            if (zat_sap == null)
            {
                return HttpNotFound();
            }
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", zat_sap.ZTERRITOR);
            ViewBag.ZUNIDAD = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE", zat_sap.ZUNIDAD);
            return View(zat_sap);
        }

        // POST: /Zat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCODZATS,ZNOMBRE,ZTERRITOR,ZUNIDAD,ZCIUDAD,ZUF,ZANO,descricao,regiao")] zat_SAP zat_sap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(zat_sap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", zat_sap.ZTERRITOR);
            ViewBag.ZUNIDAD = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE", zat_sap.ZUNIDAD);
            return View(zat_sap);
        }

        public bool verificaigual(decimal ZCODZATS)
        {
            var check = db.zat_SAP.Where(u => u.ZCODZATS == ZCODZATS);

            if (check.Count() != 0)
            {
                return false;
            }
            return true;
        }

        // GET: /Zat/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            zat_SAP zat_sap = db.zat_SAP.Find(id);
            if (zat_sap == null)
            {
                return HttpNotFound();
            }
            return View(zat_sap);
        }

        // POST: /Zat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            zat_SAP zat_sap = db.zat_SAP.Find(id);
            db.zat_SAP.Remove(zat_sap);
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
