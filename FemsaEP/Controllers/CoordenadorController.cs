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
    public class CoordenadorController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Coordenador/
        public ActionResult Index(int? pagina)
        {
            var coordenador = db.coordenador;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(coordenador.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Coordenador/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            coordenador coordenador = db.coordenador.Find(id);
            if (coordenador == null)
            {
                return HttpNotFound();
            }
            return View(coordenador);
        }

        // GET: /Coordenador/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Coordenador/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,mandante,codigo,nome,login,email,senha")] coordenador coordenador)
        {
            if (ModelState.IsValid)
            {
                db.coordenador.Add(coordenador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(coordenador);
        }

        // GET: /Coordenador/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            coordenador coordenador = db.coordenador.Find(id);
            if (coordenador == null)
            {
                return HttpNotFound();
            }
            return View(coordenador);
        }

        // POST: /Coordenador/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,mandante,codigo,nome,login,email,senha")] coordenador coordenador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coordenador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(coordenador);
        }

        // GET: /Coordenador/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            coordenador coordenador = db.coordenador.Find(id);
            if (coordenador == null)
            {
                return HttpNotFound();
            }
            return View(coordenador);
        }

        // POST: /Coordenador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            coordenador coordenador = db.coordenador.Find(id);
            db.coordenador.Remove(coordenador);
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
