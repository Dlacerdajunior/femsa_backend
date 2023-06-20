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
    public class ExecutivoController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Executivo/
        public ActionResult Index(int? pagina)
        {
            var executivo = db.executivo;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(executivo.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Executivo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            executivo executivo = db.executivo.Find(id);
            if (executivo == null)
            {
                return HttpNotFound();
            }
            return View(executivo);
        }

        // GET: /Executivo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Executivo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,codigo,nome")] executivo executivo)
        {
            if (ModelState.IsValid)
            {
                db.executivo.Add(executivo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(executivo);
        }

        // GET: /Executivo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            executivo executivo = db.executivo.Find(id);
            if (executivo == null)
            {
                return HttpNotFound();
            }
            return View(executivo);
        }

        // POST: /Executivo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,codigo,nome")] executivo executivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(executivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(executivo);
        }

        // GET: /Executivo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            executivo executivo = db.executivo.Find(id);
            if (executivo == null)
            {
                return HttpNotFound();
            }
            return View(executivo);
        }

        // POST: /Executivo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            executivo executivo = db.executivo.Find(id);
            db.executivo.Remove(executivo);
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
