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
    public class UnidadesController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Unidades/
        public ActionResult Index(int? pagina)
        {
            var unidade = db.unidade_SAP.Include(u => u.colaborador_SAP).Include(u => u.territorio_SAP);
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(unidade.OrderBy(p => p.ZCODUNIDA).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /unidadtes/Create
        public ActionResult Create()
        {
            ViewBag.ZGTECOMER = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE");
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE");
            return View();
        }

        // POST: /unidadtes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANDT,ZCODUNIDA,ZNOMBRE,ZTERRITOR,ZGTECOMER,ZDESACTIV,descricao")] unidade_SAP unidade_sap)
        {
                if (ModelState.IsValid)
                {
                    db.unidade_SAP.Add(unidade_sap);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            
            ViewBag.ZGTECOMER = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE", unidade_sap.ZGTECOMER);
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", unidade_sap.ZTERRITOR);
            return View(unidade_sap);
        }

        public bool verificaigual(string ZCODUNIDA)
        {
            var check = db.unidade_SAP.Where(u=>u.ZCODUNIDA == ZCODUNIDA);

            if (check.Count() != 0)
	        {
		        return false;
	        }
            return true;
        }

        // GET: /unidadtes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            unidade_SAP unidade_sap = db.unidade_SAP.Find(id);
            if (unidade_sap == null)
            {
                return HttpNotFound();
            }
            ViewBag.ZGTECOMER = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE", unidade_sap.ZGTECOMER);
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", unidade_sap.ZTERRITOR);
            return View(unidade_sap);
        }

        // POST: /unidadtes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCODUNIDA,ZNOMBRE,ZTERRITOR,ZGTECOMER,ZDESACTIV,descricao")] unidade_SAP unidade_sap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unidade_sap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ZGTECOMER = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE", unidade_sap.ZGTECOMER);
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", unidade_sap.ZTERRITOR);
            return View(unidade_sap);
        }

        // GET: /unidadtes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            unidade_SAP unidade_sap = db.unidade_SAP.Find(id);
            if (unidade_sap == null)
            {
                return HttpNotFound();
            }
            return View(unidade_sap);
        }

        // POST: /unidadtes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            unidade_SAP unidade_sap = db.unidade_SAP.Find(id);
            db.unidade_SAP.Remove(unidade_sap);
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
