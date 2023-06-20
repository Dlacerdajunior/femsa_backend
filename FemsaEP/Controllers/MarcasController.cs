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
    public class MarcasController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Marcas/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var marca = db.marca_SAP.Include(m => m.familia);
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(marca.OrderBy(p => p.ZCODMARCA).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Marcas/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            marca_SAP marca_sap = db.marca_SAP.Find(id);
            if (marca_sap == null)
            {
                return HttpNotFound();
            }
            return View(marca_sap);
        }

        // GET: /Marcas/Create
        public ActionResult Create()
        {
            ViewBag.id_familia = new SelectList(db.familia, "id", "descricao");
            return View();
        }

        // POST: /Marcas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MANDT,ZCODMARCA,ZNOMMARCA,ZDESACTIV,id_familia,descricao")] marca_SAP marca_sap)
        {
            if (ModelState.IsValid)
            {
                db.marca_SAP.Add(marca_sap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_familia = new SelectList(db.familia, "id", "descricao", marca_sap.id_familia);
            return View(marca_sap);
        }

        public bool verificaigual(decimal ZCODMARCA)
        {
            var check = db.marca_SAP.Where(u => u.ZCODMARCA == ZCODMARCA);

            if (check.Count() != 0)
            {
                return false;
            }
            return true;
        }
       

        // GET: /Marcas/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            marca_SAP marca_sap = db.marca_SAP.Find(id);
            if (marca_sap == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_familia = new SelectList(db.familia, "id", "descricao", marca_sap.id_familia);
            return View(marca_sap);
        }

        // POST: /Marcas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MANDT,ZCODMARCA,ZNOMMARCA,ZDESACTIV,id_familia,descricao")] marca_SAP marca_sap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marca_sap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_familia = new SelectList(db.familia, "id", "descricao", marca_sap.id_familia);
            return View(marca_sap);
        }

        // GET: /Marcas/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            marca_SAP marca_sap = db.marca_SAP.Find(id);
            if (marca_sap == null)
            {
                return HttpNotFound();
            }
            return View(marca_sap);
        }

        // POST: /Marcas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            marca_SAP marca_sap = db.marca_SAP.Find(id);
            db.marca_SAP.Remove(marca_sap);
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
