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
    public class EmbalagensController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Embalagens/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var embalagem = db.embalagem_SAP;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(embalagem.OrderBy(p => p.ZCODEMBAL).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Embalagens/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            embalagem_SAP embalagem_sap = db.embalagem_SAP.Find(id);
            if (embalagem_sap == null)
            {
                return HttpNotFound();
            }
            return View(embalagem_sap);
        }

        // GET: /Embalagens/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Embalagens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MANDT,ZCODEMBAL,ZNOMEMBAL,ZDESACTIVADO,descricao")] embalagem_SAP embalagem_sap)
        {
            if (ModelState.IsValid)
            {
                db.embalagem_SAP.Add(embalagem_sap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(embalagem_sap);
        }

        public bool verificaigual(decimal ZCODEMBAL)
        {
            var check = db.embalagem_SAP.Where(u => u.ZCODEMBAL == ZCODEMBAL);

            if (check.Count() != 0)
            {
                return false;
            }
            return true;
        }

        // GET: /Embalagens/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            embalagem_SAP embalagem_sap = db.embalagem_SAP.Find(id);
            if (embalagem_sap == null)
            {
                return HttpNotFound();
            }
            return View(embalagem_sap);
        }

        // POST: /Embalagens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MANDT,ZCODEMBAL,ZNOMEMBAL,ZDESACTIVADO,descricao")] embalagem_SAP embalagem_sap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(embalagem_sap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(embalagem_sap);
        }

        // GET: /Embalagens/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            embalagem_SAP embalagem_sap = db.embalagem_SAP.Find(id);
            if (embalagem_sap == null)
            {
                return HttpNotFound();
            }
            return View(embalagem_sap);
        }

        // POST: /Embalagens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            embalagem_SAP embalagem_sap = db.embalagem_SAP.Find(id);
            db.embalagem_SAP.Remove(embalagem_sap);
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
