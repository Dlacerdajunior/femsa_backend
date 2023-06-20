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
    public class ContrapartidaProjetoController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /ContrapartidaProjeto/
        public ActionResult Index()
        {
            var projeto = db.projeto;
            return View(projeto.ToList());
        }


        // GET: /ContrapartidaProjeto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrapartida_projeto contrapartida_projeto = db.contrapartida_projeto.Find(id);
            if (contrapartida_projeto == null)
            {
                return HttpNotFound();
            }
            return View(contrapartida_projeto);
        }

        // GET: /ContrapartidaProjeto/Create
        public ActionResult Create(int? id)
        {
            ViewBag.id_contrapartida = new SelectList(db.contrapartida_SAP, "ZCODCONPA", "ZNOMCONPA");
            ViewBag.projetos = db.projeto;
            return View();
        }

        // POST: /ContrapartidaProjeto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create()
        {
            var lista = Request["id_projeto"].ToString();
            return View();
        }

        // GET: /ContrapartidaProjeto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrapartida_projeto contrapartida_projeto = db.contrapartida_projeto.Find(id);
            if (contrapartida_projeto == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_contrapartida = new SelectList(db.contrapartida_SAP, "ZCODCONPA", "ZNOMCONPA", contrapartida_projeto.ZCODCONPA);
            ViewBag.id_projeto = new SelectList(db.projeto, "id", "descricao", contrapartida_projeto.id_projeto);
            return View(contrapartida_projeto);
        }

        // POST: /ContrapartidaProjeto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id_projeto,id_contrapartida,id")] contrapartida_projeto contrapartida_projeto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contrapartida_projeto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_contrapartida = new SelectList(db.contrapartida_SAP, "ZCODCONPA", "ZNOMCONPA", contrapartida_projeto.ZCODCONPA);
            ViewBag.id_projeto = new SelectList(db.projeto, "id", "descricao", contrapartida_projeto.id_projeto);
            return View(contrapartida_projeto);
        }

        // GET: /ContrapartidaProjeto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrapartida_projeto contrapartida_projeto = db.contrapartida_projeto.Find(id);
            if (contrapartida_projeto == null)
            {
                return HttpNotFound();
            }
            return View(contrapartida_projeto);
        }

        // POST: /ContrapartidaProjeto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            contrapartida_projeto contrapartida_projeto = db.contrapartida_projeto.Find(id);
            db.contrapartida_projeto.Remove(contrapartida_projeto);
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
