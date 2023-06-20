using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FemsaEP.Models;

namespace FemsaEP.Controllers
{
    public class PlanilhasController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Planilhas/
        public ActionResult Index()
        {
            return View(db.planilhas.ToList());
        }

        // GET: /Planilhas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planilhas planilhas = db.planilhas.Find(id);
            if (planilhas == null)
            {
                return HttpNotFound();
            }
            return View(planilhas);
        }

        // GET: /Planilhas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Planilhas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,url_upload,descricao,codigo,titulo")] planilhas planilhas)
        {
            if (ModelState.IsValid)
            {
                db.planilhas.Add(planilhas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(planilhas);
        }

        // GET: /Planilhas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planilhas planilhas = db.planilhas.Find(id);
            if (planilhas == null)
            {
                return HttpNotFound();
            }
            return View(planilhas);
        }

        // POST: /Planilhas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,url_upload,descricao,codigo,titulo")] planilhas planilhas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(planilhas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(planilhas);
        }

        // GET: /Planilhas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planilhas planilhas = db.planilhas.Find(id);
            if (planilhas == null)
            {
                return HttpNotFound();
            }
            return View(planilhas);
        }

        // POST: /Planilhas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            planilhas planilhas = db.planilhas.Find(id);
            db.planilhas.Remove(planilhas);
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
