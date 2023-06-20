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
    public class TipoManutencaoController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /TipoManutencao/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var tipo_de_manutencao = db.tipo_de_manutencao;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(tipo_de_manutencao.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /TipoManutencao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_de_manutencao tipo_de_manutencao = db.tipo_de_manutencao.Find(id);
            if (tipo_de_manutencao == null)
            {
                return HttpNotFound();
            }
            return View(tipo_de_manutencao);
        }

        // GET: /TipoManutencao/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /TipoManutencao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include="id,codigo,descricao")] tipo_de_manutencao tipo_de_manutencao)
        {
            if (ModelState.IsValid)
            {
                db.tipo_de_manutencao.Add(tipo_de_manutencao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipo_de_manutencao);
        }

        // GET: /TipoManutencao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_de_manutencao tipo_de_manutencao = db.tipo_de_manutencao.Find(id);
            if (tipo_de_manutencao == null)
            {
                return HttpNotFound();
            }
            return View(tipo_de_manutencao);
        }

        // POST: /TipoManutencao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,codigo,descricao")] tipo_de_manutencao tipo_de_manutencao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipo_de_manutencao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipo_de_manutencao);
        }

        // GET: /TipoManutencao/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipo_de_manutencao tipo_de_manutencao = db.tipo_de_manutencao.Find(id);
            if (tipo_de_manutencao == null)
            {
                return HttpNotFound();
            }
            return View(tipo_de_manutencao);
        }

        // POST: /TipoManutencao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipo_de_manutencao tipo_de_manutencao = db.tipo_de_manutencao.Find(id);
            db.tipo_de_manutencao.Remove(tipo_de_manutencao);
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
