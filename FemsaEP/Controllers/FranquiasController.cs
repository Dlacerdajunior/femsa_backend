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
    public class FranquiasController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Franquias/
        public ActionResult Index(int? pagina)
        {
            var franquia = db.franquia;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(franquia.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Franquias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            franquia franquia = db.franquia.Find(id);
            if (franquia == null)
            {
                return HttpNotFound();
            }
            return View(franquia);
        }

        // GET: /Franquias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Franquias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include="id,codigo,descricao")] franquia franquia)
        {
            if (ModelState.IsValid)
            {
                db.franquia.Add(franquia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(franquia);
        }

        // GET: /Franquias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            franquia franquia = db.franquia.Find(id);
            if (franquia == null)
            {
                return HttpNotFound();
            }
            return View(franquia);
        }

        // POST: /Franquias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,codigo,descricao")] franquia franquia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(franquia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(franquia);
        }

        // GET: /Franquias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            franquia franquia = db.franquia.Find(id);
            if (franquia == null)
            {
                return HttpNotFound();
            }
            return View(franquia);
        }

        // POST: /Franquias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            franquia franquia = db.franquia.Find(id);
            db.franquia.Remove(franquia);
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
