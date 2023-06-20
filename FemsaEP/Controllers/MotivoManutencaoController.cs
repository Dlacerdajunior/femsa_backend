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
    public class MotivoManutencaoController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /MotivoManutencao/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var motivo_de_manutencao = db.motivo_de_manutencao;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(motivo_de_manutencao.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /MotivoManutencao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            motivo_de_manutencao motivo_de_manutencao = db.motivo_de_manutencao.Find(id);
            if (motivo_de_manutencao == null)
            {
                return HttpNotFound();
            }
            return View(motivo_de_manutencao);
        }

        // GET: /MotivoManutencao/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /MotivoManutencao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include="id,codigo,descricao")] motivo_de_manutencao motivo_de_manutencao)
        {
            if (ModelState.IsValid)
            {
                db.motivo_de_manutencao.Add(motivo_de_manutencao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(motivo_de_manutencao);
        }

        // GET: /MotivoManutencao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            motivo_de_manutencao motivo_de_manutencao = db.motivo_de_manutencao.Find(id);
            if (motivo_de_manutencao == null)
            {
                return HttpNotFound();
            }
            return View(motivo_de_manutencao);
        }

        // POST: /MotivoManutencao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,codigo,descricao")] motivo_de_manutencao motivo_de_manutencao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(motivo_de_manutencao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(motivo_de_manutencao);
        }

        // GET: /MotivoManutencao/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            motivo_de_manutencao motivo_de_manutencao = db.motivo_de_manutencao.Find(id);
            if (motivo_de_manutencao == null)
            {
                return HttpNotFound();
            }
            return View(motivo_de_manutencao);
        }

        // POST: /MotivoManutencao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            motivo_de_manutencao motivo_de_manutencao = db.motivo_de_manutencao.Find(id);
            db.motivo_de_manutencao.Remove(motivo_de_manutencao);
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
