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
    public class GrupoUsuariosController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /GrupoUsuarios/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var grupo_SAP = db.grupo_SAP;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(grupo_SAP.OrderBy(p => p.ZCODGRUPO).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /GrupoUsuarios/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            grupo_SAP grupo_sap = db.grupo_SAP.Find(id);
            if (grupo_sap == null)
            {
                return HttpNotFound();
            }
            return View(grupo_sap);
        }

        // GET: /GrupoUsuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /GrupoUsuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MANDT,ZCODGRUPO,ZNOMGRUPO,descricao")] grupo_SAP grupo_sap)
        {
            if (ModelState.IsValid)
            {
                db.grupo_SAP.Add(grupo_sap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grupo_sap);
        }

        public bool verificaigual(decimal ZCODGRUPO)
        {
            var check = db.grupo_SAP.Where(u => u.ZCODGRUPO == ZCODGRUPO);

            if (check.Count() != 0)
            {
                return false;
            }
            return true;
        }

        // GET: /GrupoUsuarios/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            grupo_SAP grupo_sap = db.grupo_SAP.Find(id);
            if (grupo_sap == null)
            {
                return HttpNotFound();
            }
            return View(grupo_sap);
        }

        // POST: /GrupoUsuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MANDT,ZCODGRUPO,ZNOMGRUPO,descricao")] grupo_SAP grupo_sap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grupo_sap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grupo_sap);
        }

        // GET: /GrupoUsuarios/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            grupo_SAP grupo_sap = db.grupo_SAP.Find(id);
            if (grupo_sap == null)
            {
                return HttpNotFound();
            }
            return View(grupo_sap);
        }

        // POST: /GrupoUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            grupo_SAP grupo_sap = db.grupo_SAP.Find(id);
            db.grupo_SAP.Remove(grupo_sap);
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
