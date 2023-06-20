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
    public class AtivacaoController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Ativacao/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var ativacao = db.ativacao.Where(a => a.ZCLIENTE != null).Include(a => a.cliente_SAP).Include(a => a.projeto).Include(a => a.status_ativacao);

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(ativacao.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }


        // GET: /Ativacao/
        public ActionResult Detalhes(int? id)
        {
           var rsativacao = db.ativacao.Where(ativacao => ativacao.id == id).FirstOrDefault();

           //var rsativacao = from st in db.ativacao
                          //where st.id == id
                         // select st;

            //var ativacao = rsativacao.FirstOrDefault<ativacao>();
            //var idcliente = ativacao.id;

           /*PEGAR CLIENTE*/
          //var rscliente = db.cliente.Where(cliente => cliente.id == id).First();

           //cliente cliente = db.cliente.Find(rsativacao.cliente.id); 

           cliente_SAP cliente = db.cliente_SAP.Where(rscliente => rscliente.ZCLIENTE == rsativacao.cliente_SAP.ZCLIENTE).FirstOrDefault();

            /*var rscliente = (from st in db.cliente
                             where st.id == id
                             select new { st.id, st.descricao, st.endereco, st.nome_red, st.razao_social }).ToList();*/
           //var cliente = rscliente.FirstOrDefault<cliente>();                
           //ViewData["data1"] = id;

           ViewBag.idativacao = id;

            return View(cliente);     
        }

        // GET: /Ativacao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ativacao ativacao = db.ativacao.Find(id);
            if (ativacao == null)
            {
                return HttpNotFound();
            }
            return View(ativacao);
        }

        // GET: /Ativacao/Create
        public ActionResult Create()
        {
            ViewBag.id_cliente = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE");
            ViewBag.id_projeto = new SelectList(db.projeto, "id", "descricao");
            ViewBag.status = new SelectList(db.status_ativacao, "id", "nome");
            return View();
        }

        // POST: /Ativacao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigo,custo_valor,descricao,id_cliente,status,id_projeto,situacao,executivo,criacao,mandante,data_finalizacao,ZCLIENTE")] ativacao ativacao)
        {
            if (ModelState.IsValid)
            {
                db.ativacao.Add(ativacao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_cliente = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE", ativacao.id_cliente);
            ViewBag.id_projeto = new SelectList(db.projeto, "id", "descricao", ativacao.id_projeto);
            ViewBag.status = new SelectList(db.status_ativacao, "id", "nome", ativacao.status);
            return View(ativacao);
        }

        // GET: /Ativacao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ativacao ativacao = db.ativacao.Find(id);
            if (ativacao == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cliente = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE", ativacao.id_cliente);
            ViewBag.id_projeto = new SelectList(db.projeto, "id", "descricao", ativacao.id_projeto);
            ViewBag.status = new SelectList(db.status_ativacao, "id", "nome", ativacao.status);
            return View(ativacao);
        }

        // POST: /Ativacao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigo,custo_valor,descricao,id_cliente,status,id_projeto,situacao,executivo,criacao,mandante,data_finalizacao,ZCLIENTE")] ativacao ativacao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ativacao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cliente = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE", ativacao.id_cliente);
            ViewBag.id_projeto = new SelectList(db.projeto, "id", "descricao", ativacao.id_projeto);
            ViewBag.status = new SelectList(db.status_ativacao, "id", "nome", ativacao.status);
            return View(ativacao);
        }

        // GET: /Ativacao/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ativacao ativacao = db.ativacao.Find(id);
            if (ativacao == null)
            {
                return HttpNotFound();
            }
            return View(ativacao);
        }

        // POST: /Ativacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ativacao ativacao = db.ativacao.Find(id);
            db.ativacao.Remove(ativacao);
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
