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
    public class MensagemController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Mensagem/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var mensagem = db.mensagem.Include(m => m.administrador).Include(m => m.ativacao).Include(m => m.colaborador_SAP).Include(m => m.grupo_SAP);

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(mensagem.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Mensagem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mensagem mensagem = db.mensagem.Find(id);
            if (mensagem == null)
            {
                return HttpNotFound();
            }
            return View(mensagem);
        }

        // GET: /Mensagem/Create
        public ActionResult Create(int[] grupousuario)
        {
            ViewBag.administrador_id = new SelectList(db.administrador, "id", "nome");
            ViewBag.id_cliente = new SelectList(db.ativacao, "id", "descricao");
            ViewBag.colab_id = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE");
            ViewBag.grupocert_id = new SelectList(db.grupo_SAP, "ZCODGRUPO", "ZNOMGRUPO");
            return View();
        }

        // POST: /Mensagem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,texto,id_cliente,administrador_id,hora,status,titulo,grupo_id,destinatario,mandante,ZCODGRUPO,ZCODCOLAB")] mensagem mensagem)
        {
            if (ModelState.IsValid)
            {
                var id = System.Web.HttpContext.Current.Session["id"];
                var idd = Convert.ToInt32(id);
                mensagem.administrador_id = idd;

                if (mensagem.ZCODGRUPO >= 1)
                {
                    var rsCliente = db.colaborador_SAP.Where(g => g.ZGRUPO == mensagem.ZCODGRUPO);

                    foreach (var item in rsCliente)
                    {
                        mensagem.ZCODCOLAB = item.ZGRUPO;
                        db.mensagem.Add(mensagem);
                        db.SaveChanges();
                    }
                }
                else if (mensagem.id_cliente >= 1)
                {


                    var rsativacao = db.ativacao.Where(ativacao => ativacao.id == mensagem.id_cliente).FirstOrDefault();


                    var rsCliente = db.cliente_SAP.Where(g => g.ZCLIENTE == rsativacao.ZCLIENTE).FirstOrDefault();

                    mensagem.ZCODCOLAB = rsCliente.colaborador_SAP.ZCODCOLAB;
                    db.mensagem.Add(mensagem);
                    db.SaveChanges();

                }
                else if (mensagem.destinatario == "1")
                {
                    var rsCliente = db.colaborador_SAP;

                    foreach (var item in rsCliente)
                    {
                        mensagem.ZCODCOLAB = item.ZCODCOLAB;
                        db.mensagem.Add(mensagem);
                        db.SaveChanges();
                    }
                }
                else
                {
                    db.mensagem.Add(mensagem);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.administrador_id = new SelectList(db.administrador, "id", "nome", mensagem.administrador_id);
            ViewBag.id_cliente = new SelectList(db.ativacao, "id", "descricao", mensagem.id_cliente);
            ViewBag.colab_id = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE", mensagem.ZCODCOLAB);
            ViewBag.grupocert_id = new SelectList(db.grupo_SAP, "ZCODGRUPO", "ZNOMGRUPO", mensagem.ZCODGRUPO);
            return View(mensagem);
        }

        // GET: /Mensagem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mensagem mensagem = db.mensagem.Find(id);
            if (mensagem == null)
            {
                return HttpNotFound();
            }
            ViewBag.administrador_id = new SelectList(db.administrador, "id", "nome", mensagem.administrador_id);
            ViewBag.id_cliente = new SelectList(db.ativacao, "id", "descricao", mensagem.id_cliente);
            ViewBag.colab_id = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE", mensagem.ZCODCOLAB);
            ViewBag.grupocert_id = new SelectList(db.grupo_SAP, "ZCODGRUPO", "ZNOMGRUPO", mensagem.ZCODGRUPO);
            return View(mensagem);
        }

        // POST: /Mensagem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,texto,id_cliente,administrador_id,hora,status,titulo,grupo_id,destinatario,mandante,ZCODGRUPO,ZCODCOLAB")] mensagem mensagem)
        {
            if (ModelState.IsValid)
            {
                mensagem.administrador_id = 2;
                db.Entry(mensagem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.administrador_id = new SelectList(db.administrador, "id", "nome", mensagem.administrador_id);
            ViewBag.id_cliente = new SelectList(db.ativacao, "id", "descricao", mensagem.id_cliente);
            ViewBag.colab_id = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE", mensagem.ZCODCOLAB);
            ViewBag.grupocert_id = new SelectList(db.grupo_SAP, "ZCODGRUPO", "ZNOMGRUPO", mensagem.ZCODGRUPO);
            return View(mensagem);
        }

        // GET: /Mensagem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mensagem mensagem = db.mensagem.Find(id);
            if (mensagem == null)
            {
                return HttpNotFound();
            }
            return View(mensagem);
        }

        // POST: /Mensagem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            mensagem mensagem = db.mensagem.Find(id);
            db.mensagem.Remove(mensagem);
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
