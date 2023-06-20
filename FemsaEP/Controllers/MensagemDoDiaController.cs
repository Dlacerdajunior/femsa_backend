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
    public class MensagemDoDiaController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /MensagemDoDia/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var mensagem_do_dia = db.mensagem_do_dia;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(mensagem_do_dia.OrderByDescending(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));

        }

        // GET: /MensagemDoDia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mensagem_do_dia mensagem_do_dia = db.mensagem_do_dia.Find(id);
            if (mensagem_do_dia == null)
            {
                return HttpNotFound();
            }
            return View(mensagem_do_dia);
        }

        // GET: /MensagemDoDia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /MensagemDoDia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,mensagem,status")] mensagem_do_dia mensagem_do_dia)
        {
            if (ModelState.IsValid)
            {
                db.mensagem_do_dia.Add(mensagem_do_dia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mensagem_do_dia);
        }

        // GET: /MensagemDoDia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mensagem_do_dia mensagem_do_dia = db.mensagem_do_dia.Find(id);
            if (mensagem_do_dia == null)
            {
                return HttpNotFound();
            }
            return View(mensagem_do_dia);
        }

        // POST: /MensagemDoDia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,mensagem,status")] mensagem_do_dia mensagem_do_dia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mensagem_do_dia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mensagem_do_dia);
        }

        // GET: /MensagemDoDia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mensagem_do_dia mensagem_do_dia = db.mensagem_do_dia.Find(id);
            if (mensagem_do_dia == null)
            {
                return HttpNotFound();
            }
            return View(mensagem_do_dia);
        }

        // POST: /MensagemDoDia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            mensagem_do_dia mensagem_do_dia = db.mensagem_do_dia.Find(id);
            db.mensagem_do_dia.Remove(mensagem_do_dia);
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
