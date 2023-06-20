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
    public class ChatController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Chat/
        public ActionResult Index()
        {
            var chat_mensagem = db.chat_mensagem.Include(c => c.administrador).Include(c => c.mensagem).Include(c => c.colaborador_SAP);
            return View(chat_mensagem.ToList());
        }

        // GET: /Chat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chat_mensagem chat_mensagem = db.chat_mensagem.Find(id);
            if (chat_mensagem == null)
            {
                return HttpNotFound();
            }
            return View(chat_mensagem);
        }

        // GET: /Chat/Create
        public ActionResult Create()
        {
            ViewBag.remetente_id = new SelectList(db.administrador, "id", "codigo");
            ViewBag.mensagem_id = new SelectList(db.mensagem, "id", "texto");
            ViewBag.usuario_id = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE");
            return View();
        }

        //CreateInsert David's Methods to fazer o botão Enviar Funcionar legal!

        public ActionResult CreateInsert([Bind(Include = "id,mensagem_id,ZCODCOLAB,remetente_id,texto,hora,tipo,mandante")] chat_mensagem chat_mensagem)
        {
            if (ModelState.IsValid)
            {
                chat_mensagem.tipo = "1";
                chat_mensagem.hora = DateTime.Now;
                db.chat_mensagem.Add(chat_mensagem);
                db.SaveChanges();

                return Redirect("Chat/" + chat_mensagem.mensagem_id + "?idmesagem=" + chat_mensagem.mensagem_id + "&colaborador=" + chat_mensagem.ZCODCOLAB + "&adm=" + chat_mensagem.remetente_id);

                //return RedirectToAction("Chat/16?idmesagem=16&colaborador=10&adm=2"); 
            }

            return View();
        }
        // GET: /Chat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chat_mensagem chat_mensagem = db.chat_mensagem.Find(id);
            if (chat_mensagem == null)
            {
                return HttpNotFound();
            }
            ViewBag.remetente_id = new SelectList(db.administrador, "id", "codigo", chat_mensagem.remetente_id);
            ViewBag.mensagem_id = new SelectList(db.mensagem, "id", "texto", chat_mensagem.mensagem_id);
            ViewBag.usuario_id = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE", chat_mensagem.ZCODCOLAB);
            return View(chat_mensagem);
        }

        // POST: /Chat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,mensagem_id,ZCODCOLAB,remetente_id,texto,hora,tipo,mandante")] chat_mensagem chat_mensagem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chat_mensagem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.remetente_id = new SelectList(db.administrador, "id", "codigo", chat_mensagem.remetente_id);
            ViewBag.mensagem_id = new SelectList(db.mensagem, "id", "texto", chat_mensagem.mensagem_id);
            ViewBag.usuario_id = new SelectList(db.colaborador_SAP, "id", "ZNOMBRE", chat_mensagem.ZCODCOLAB);
            return View(chat_mensagem);
        }

        // GET: /Chat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chat_mensagem chat_mensagem = db.chat_mensagem.Find(id);
            if (chat_mensagem == null)
            {
                return HttpNotFound();
            }
            return View(chat_mensagem);
        }

        // POST: /Chat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            chat_mensagem chat_mensagem = db.chat_mensagem.Find(id);
            db.chat_mensagem.Remove(chat_mensagem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult chat(int id)
        {
            var mensagens = db.chat_mensagem.Where(c => c.mensagem_id == id);
            var list = new List<dynamic>();
            foreach (var item in mensagens)
            {
                list.Add(new
                {
                    @mensage = item.texto
                });
            }
            //return Json(new { teste=list}, JsonRequestBehavior.AllowGet);
            return View(mensagens);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult enviarchat(chat_mensagem chat_mensagem)
        {


            return RedirectToAction("Index");
        }
    }
}
