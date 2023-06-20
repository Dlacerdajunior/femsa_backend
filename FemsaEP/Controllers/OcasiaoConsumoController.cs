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
    public class OcasiaoConsumoController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /OcasiaoConsumo/
        public ActionResult Index(int? pagina)
        {
            var ocasiao_de_consumo = db.ocasiao_de_consumo;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(ocasiao_de_consumo.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /OcasiaoConsumo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ocasiao_de_consumo ocasiao_de_consumo = db.ocasiao_de_consumo.Find(id);
            if (ocasiao_de_consumo == null)
            {
                return HttpNotFound();
            }
            return View(ocasiao_de_consumo);
        }

        // GET: /OcasiaoConsumo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /OcasiaoConsumo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,codigo,descricao")] ocasiao_de_consumo ocasiao_de_consumo)
        {
            if (ModelState.IsValid)
            {
                db.ocasiao_de_consumo.Add(ocasiao_de_consumo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ocasiao_de_consumo);
        }

        // GET: /OcasiaoConsumo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ocasiao_de_consumo ocasiao_de_consumo = db.ocasiao_de_consumo.Find(id);
            if (ocasiao_de_consumo == null)
            {
                return HttpNotFound();
            }
            return View(ocasiao_de_consumo);
        }

        // POST: /OcasiaoConsumo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,codigo,descricao")] ocasiao_de_consumo ocasiao_de_consumo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ocasiao_de_consumo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ocasiao_de_consumo);
        }

        // GET: /OcasiaoConsumo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ocasiao_de_consumo ocasiao_de_consumo = db.ocasiao_de_consumo.Find(id);
            if (ocasiao_de_consumo == null)
            {
                return HttpNotFound();
            }
            return View(ocasiao_de_consumo);
        }

        // POST: /OcasiaoConsumo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ocasiao_de_consumo ocasiao_de_consumo = db.ocasiao_de_consumo.Find(id);
            db.ocasiao_de_consumo.Remove(ocasiao_de_consumo);
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
