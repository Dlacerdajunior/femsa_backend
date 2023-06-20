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
    public class PerguntasController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Perguntas/
        public ActionResult Index(int? id)
        {

            if (id != null)
            {
                var pergunta = db.enquete_perguntas.Where(c => c.id_enquete == id);
                var list = new List<dynamic>();
                foreach (var item in pergunta)
                {
                    list.Add(new
                    {
                        @mensagem = item.enquete
                    });
                }
                return View(pergunta);
            }

            var enquete_perguntas = db.enquete_perguntas.Include(e => e.enquete);
            return View(enquete_perguntas.ToList());
        }

        // GET: /Perguntas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            enquete_perguntas enquete_perguntas = db.enquete_perguntas.Find(id);
            if (enquete_perguntas == null)
            {
                return HttpNotFound();
            }
            return View(enquete_perguntas);
        }

        // GET: /Perguntas/Create
        public ActionResult Create()
        {
            ViewBag.id_enquete = new SelectList(db.enquete, "id", "titulo");
            return View();
        }

        //CreateInsert David's Methods to fazer o botão Enviar Funcionar legal!

        public ActionResult CreateInsert([Bind(Include = "id,nome,id_enquete")] enquete_perguntas enquete_perguntas)
        {
            if (ModelState.IsValid)
            {
                db.enquete_perguntas.Add(enquete_perguntas);
                db.SaveChanges();
               // return Redirect("Perguntas/" + enquete_perguntas.id_enquete + "?idpesquisa=" + enquete_perguntas.id_enquete);

                return Redirect("?id=" + enquete_perguntas.id_enquete);
            }

            ViewBag.id_enquete = new SelectList(db.enquete, "id", "titulo", enquete_perguntas.id_enquete);
            return View(enquete_perguntas);
        }

        // GET: /Perguntas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            enquete_perguntas enquete_perguntas = db.enquete_perguntas.Find(id);
            if (enquete_perguntas == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_enquete = new SelectList(db.enquete, "id", "titulo", enquete_perguntas.id_enquete);
            return View(enquete_perguntas);
        }

        // POST: /Perguntas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nome,id_enquete")] enquete_perguntas enquete_perguntas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enquete_perguntas).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.PathAndQuery);
               
                // return RedirectToAction("Index");
            }
            ViewBag.id_enquete = new SelectList(db.enquete, "id", "titulo", enquete_perguntas.id_enquete);
            return View(enquete_perguntas);
        }

        // GET: /Perguntas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            enquete_perguntas enquete_perguntas = db.enquete_perguntas.Find(id);
            if (enquete_perguntas == null)
            {
                return HttpNotFound();
            }
            return View(enquete_perguntas);
        }

        // POST: /Perguntas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            enquete_perguntas enquete_perguntas = db.enquete_perguntas.Find(id);
            db.enquete_perguntas.Remove(enquete_perguntas);
            db.SaveChanges();

            return Redirect(Request.UrlReferrer.PathAndQuery);
           
            // return RedirectToAction("Index");
        }

        /*public ActionResult Perguntas(int id)
        {
            

            var pergunta = db.enquete_perguntas.Where(c => c.id_enquete == id);
            var list = new List<dynamic>();
            foreach (var item in pergunta)
            {
                list.Add(new
                {
                    @mensagem = item.enquete
                });
            }
            return View(pergunta);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult enviarpergunta(enquete_perguntas enquete_perguntas)
        {
            return RedirectToAction("Index");
        }
    }
}
