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
    public class LocalizacoesController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Localizacoes/
        public ActionResult Index(int? pagina)
        {
            var localizacao = db.localizacao_SAP;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(localizacao.OrderBy(p => p.ZCODLOCA).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Localizacoes/Details/5
        public ActionResult Details(int? ZCODLOCA)
        {
            if (ZCODLOCA == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            localizacao_SAP localizacao = db.localizacao_SAP.Find(ZCODLOCA);
            if (localizacao == null)
            {
                return HttpNotFound();
            }
            return View(localizacao);
        }

        // GET: /Localizacoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Localizacoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "MANDT,ZCODLOCA,ZNOMLOCA,ZDESACTIV,descricao")] localizacao_SAP localizacao)
        {
            if (ModelState.IsValid)
            {
                db.localizacao_SAP.Add(localizacao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(localizacao);
        }

        // GET: /Localizacoes/Edit/5
        public ActionResult Edit(int? ZCODLOCA)
        {
            if (ZCODLOCA == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            localizacao_SAP localizacao = db.localizacao_SAP.Find(ZCODLOCA);
            if (localizacao == null)
            {
                return HttpNotFound();
            }
            return View(localizacao);
        }

        // POST: /Localizacoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCODLOCA,ZNOMLOCA,ZDESACTIV,descricao")] localizacao_SAP localizacao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(localizacao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(localizacao);
        }

        // GET: /Localizacoes/Delete/5
        public ActionResult Delete(int? ZCODLOCA)
        {
            if (ZCODLOCA == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            localizacao_SAP localizacao = db.localizacao_SAP.Find(ZCODLOCA);
            if (localizacao == null)
            {
                return HttpNotFound();
            }
            return View(localizacao);
        }

        // POST: /Localizacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ZCODLOCA)
        {
            localizacao_SAP localizacao = db.localizacao_SAP.Find(ZCODLOCA);
            db.localizacao_SAP.Remove(localizacao);
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
