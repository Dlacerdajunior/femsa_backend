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
    public class FocosController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Focos/
        public ActionResult Index(int? pagina)
        {
            var foco = db.foco;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(foco.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Focos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foco foco = db.foco.Find(id);
            if (foco == null)
            {
                return HttpNotFound();
            }
            return View(foco);
        }

        // GET: /Focos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Focos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include="id,codigo,descricao")] foco foco)
        {
            if (ModelState.IsValid)
            {
                db.foco.Add(foco);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(foco);
        }

        // GET: /Focos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foco foco = db.foco.Find(id);
            if (foco == null)
            {
                return HttpNotFound();
            }
            return View(foco);
        }

        // POST: /Focos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,codigo,descricao")] foco foco)
        {
            if (ModelState.IsValid)
            {
                db.Entry(foco).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(foco);
        }

        // GET: /Focos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foco foco = db.foco.Find(id);
            if (foco == null)
            {
                return HttpNotFound();
            }
            return View(foco);
        }

        // POST: /Focos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            foco foco = db.foco.Find(id);
            db.foco.Remove(foco);
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
