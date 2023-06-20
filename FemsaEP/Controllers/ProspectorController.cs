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
    public class ProspectorController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Prospector/
        public ActionResult Index(int? pagina)
        {
            var prospector = db.prospector;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(prospector.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));

        }

        // GET: /Prospector/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            prospector prospector = db.prospector.Find(id);
            if (prospector == null)
            {
                return HttpNotFound();
            }
            return View(prospector);
        }

        // GET: /Prospector/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Prospector/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,codigo,nome")] prospector prospector)
        {
            if (ModelState.IsValid)
            {
                db.prospector.Add(prospector);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(prospector);
        }

        // GET: /Prospector/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            prospector prospector = db.prospector.Find(id);
            if (prospector == null)
            {
                return HttpNotFound();
            }
            return View(prospector);
        }

        // POST: /Prospector/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,codigo,nome")] prospector prospector)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prospector).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prospector);
        }

        // GET: /Prospector/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            prospector prospector = db.prospector.Find(id);
            if (prospector == null)
            {
                return HttpNotFound();
            }
            return View(prospector);
        }

        // POST: /Prospector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            prospector prospector = db.prospector.Find(id);
            db.prospector.Remove(prospector);
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
