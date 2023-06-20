using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FemsaEP.Models;
using System.Threading.Tasks;
using PagedList;

namespace FemsaEP.Controllers
{
    public class ManutencaoController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Manutencao/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var manutencao = db.manutencao.Include(m => m.administrador).Include(m => m.cliente_SAP).Include(m => m.tipo_de_manutencao).Include(m => m.validacao).Include(m => m.peca_SAP);

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(manutencao.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Manutencao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            manutencao manutencao = db.manutencao.Find(id);
            if (manutencao == null)
            {
                return HttpNotFound();
            }
            return View(manutencao);
        }

        // GET: /Manutencao/Create
        public ActionResult Create()
        {
            ViewBag.id_administrador = new SelectList(db.administrador, "id", "nome");
            ViewBag.cliente_id = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE");
            ViewBag.manutencao_tipo = new SelectList(db.tipo_de_manutencao, "id", "descricao");
            ViewBag.manutencao_validacao = new SelectList(db.validacao, "id", "nome");
            ViewBag.peca_id = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA");
            return View();
        }

        // POST: /Manutencao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "data_manutencao,data_ativacao,meta_manutencao,observacao_1,observacao_2,numero_rc,data_rc,url_foto,solicitante,manutencao_tipo,manutencao_validacao,codigo,id,id_administrador,substituida,mandante,executivo_vendas,executivo_trade,prospector,pos_venda,ZCLIENTE,ZCODPECA")] manutencao manutencao, string cliente_id)
        {
            if (ModelState.IsValid)
            {
                if (cliente_id != "")
                {
                    manutencao.ZCLIENTE = cliente_id;
                }
                db.manutencao.Add(manutencao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_administrador = new SelectList(db.administrador, "id", "nome", manutencao.id_administrador);
            ViewBag.cliente_id = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE", manutencao.ZCLIENTE);
            ViewBag.manutencao_tipo = new SelectList(db.tipo_de_manutencao, "id", "descricao", manutencao.manutencao_tipo);
            ViewBag.manutencao_validacao = new SelectList(db.validacao, "id", "nome", manutencao.manutencao_validacao);
            ViewBag.peca_id = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA", manutencao.ZCODPECA);
            return View(manutencao);
        }

        //Auto Create Redirect to Edit
        public ActionResult EditarManutencao(manutencao manutencao)
        {
            db.manutencao.Add(manutencao);
            db.SaveChanges();


            return Redirect("Edit/" + manutencao.id + "?show=cliente");
        }

        // GET: /Manutencao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            manutencao manutencao = db.manutencao.Find(id);
            if (manutencao == null)
            {
                return HttpNotFound();
            }



            if (manutencao.ZCLIENTE != null)
            {
                ViewBag.mostrarbloco = 0;
                //ViewBag.cliente_id = new SelectList(db.cliente.Where(c => c.status_ativacao == 1), "id", "nome", manutencao.cliente_id);
            }
            else
            {
                ViewBag.mostrarbloco = 1;
               // ViewBag.cliente_id = new SelectList(db.cliente.Where(c => c.status_ativacao == 1), "id", "nome", manutencao.cliente_id);
            }
            
            //db.manutencao_instalacao
            var rsmanutencaofoto = db.manutencao_instalacao.Where(s => s.id_manutencao == id);
            var rsmanutencaofotocount = db.manutencao_instalacao.Where(s => s.id_manutencao == id).Count();

            if (rsmanutencaofotocount >= 1)
            {
                ViewBag.fotosmanutencao = rsmanutencaofoto;
                ViewBag.fotosmanutencaomostrar = 1;
            }
            else
            {
                ViewBag.fotosmanutencaomostrar = 0;
            }            
            
            //db.manutencao_peca
            var rsmanutencaopeca = db.manutencao_peca.Where(s => s.id_manutencao == id);
            var rsmanutencaopecacount = db.manutencao_peca.Where(s => s.id_manutencao == id).Count();

            if (rsmanutencaopecacount >= 1)
            {
                ViewBag.pecasmantencao = rsmanutencaopeca;
                ViewBag.pecasmantencaomotrar = 1;
            }
            else
            {
                ViewBag.pecasmantencaomotrar = 0;
            }



            var rscliente = db.cliente_SAP.Where(s => s.status_ativacao == 1);
            var rsclientecount = db.cliente_SAP.Where(s => s.status_ativacao == 1).Count();

            if (rsclientecount >= 1)
            {
                ViewBag.rsclientes = rscliente;
                ViewBag.rsclientesmostrar = 1;
            }
            else
            {   
                ViewBag.rsclientesmostrar = 0;
            }

            var dadoscliente = db.cliente_SAP.Where(s => s.ZCLIENTE == manutencao.ZCLIENTE);
            ViewBag.dadocliente = dadoscliente;



            ViewBag.id_administrador = new SelectList(db.administrador, "id", "nome", manutencao.id_administrador);
            ViewBag.cliente_id = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE", manutencao.ZCLIENTE);
            ViewBag.manutencao_tipo = new SelectList(db.tipo_de_manutencao, "id", "descricao", manutencao.manutencao_tipo);
            ViewBag.manutencao_validacao = new SelectList(db.validacao, "id", "nome", manutencao.manutencao_validacao);
            ViewBag.id_peca = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA", manutencao.ZCODPECA);
            return View(manutencao);
        }

        // POST: /Manutencao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "data_manutencao,data_ativacao,meta_manutencao,observacao_1,observacao_2,numero_rc,data_rc,url_foto,solicitante,manutencao_tipo,manutencao_validacao,codigo,id,id_administrador,substituida,mandante,executivo_vendas,executivo_trade,prospector,pos_venda,ZCLIENTE,ZCODPECA")] manutencao manutencao, string cliente_id)
        {


            if (ModelState.IsValid)
            {
                if (cliente_id != "")
                {
                    manutencao.ZCLIENTE = cliente_id;
                }
                db.Entry(manutencao).State = EntityState.Modified;
                db.SaveChanges();


                Response.Write("<script> window.location='/manutencao/edit/" + manutencao.id + "?show=" + manutencao.url_foto + "'; </script> ");
                Response.End();

                //return RedirectToAction("edit/" + manutencao.id + "?show=" + manutencao.url_foto);

            }


            var id = manutencao.id;

            manutencao manutencao2 = db.manutencao.Find(manutencao.id);
            if (manutencao2 == null)
            {
                return HttpNotFound();
            }

            if (manutencao2.ZCLIENTE != null)
            {
                ViewBag.mostrarbloco = 0;
                //ViewBag.cliente_id = new SelectList(db.cliente.Where(c => c.status_ativacao == 1), "id", "nome", manutencao.cliente_id);
            }
            else
            {
                ViewBag.mostrarbloco = 1;
                // ViewBag.cliente_id = new SelectList(db.cliente.Where(c => c.status_ativacao == 1), "id", "nome", manutencao.cliente_id);
            }

            //db.manutencao_instalacao
            var rsmanutencaofoto = db.manutencao_instalacao.Where(s => s.id_manutencao == id);
            var rsmanutencaofotocount = db.manutencao_instalacao.Where(s => s.id_manutencao == id).Count();

            if (rsmanutencaofotocount >= 1)
            {
                ViewBag.fotosmanutencao = rsmanutencaofoto;
                ViewBag.fotosmanutencaomostrar = 1;
            }
            else
            {
                ViewBag.fotosmanutencaomostrar = 0;
            }

            //db.manutencao_peca
            var rsmanutencaopeca = db.manutencao_peca.Where(s => s.id_manutencao == id);
            var rsmanutencaopecacount = db.manutencao_peca.Where(s => s.id_manutencao == id).Count();

            if (rsmanutencaopecacount >= 1)
            {
                ViewBag.pecasmantencao = rsmanutencaopeca;
                ViewBag.pecasmantencaomotrar = 1;
            }
            else
            {
                ViewBag.pecasmantencaomotrar = 0;
            }

            ViewBag.id_administrador = new SelectList(db.administrador, "id", "nome", manutencao.id_administrador);
            ViewBag.cliente_id = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE", manutencao.ZCLIENTE);
            ViewBag.manutencao_tipo = new SelectList(db.tipo_de_manutencao, "id", "descricao", manutencao.manutencao_tipo);
            ViewBag.manutencao_validacao = new SelectList(db.validacao, "id", "nome", manutencao.manutencao_validacao);
            ViewBag.id_peca = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA", manutencao.ZCODPECA);
            
            return View(manutencao);

        }

        // GET: /Manutencao/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            manutencao manutencao = db.manutencao.Find(id);
            if (manutencao == null)
            {
                return HttpNotFound();
            }
            return View(manutencao);
        }

        // POST: /Manutencao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            manutencao manutencao = db.manutencao.Find(id);
            db.manutencao.Remove(manutencao);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult adicionarpecar([Bind(Include = "id_manutencao,ZCODPECA")] manutencao_peca manutencao_peca, string id_peca)
        {

            if (id_peca != "")
            {
                manutencao_peca.ZCODPECA = Convert.ToDecimal(id_peca);
            }
            db.manutencao_peca.Add(manutencao_peca);            
            db.SaveChanges();

            Response.Write("<script> window.location='/manutencao/edit/" + manutencao_peca.id_manutencao + "?show=peca'; </script> ");
            Response.End();


            ViewBag.id_peca = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA", manutencao_peca.ZCODPECA);
            return RedirectToAction("Edit/" + manutencao_peca.id_manutencao);                               
        }

        [HttpPost]
        public ActionResult adicionarfoto([Bind(Include = "id_manutencao,foto")] manutencao_instalacao manutencao_instalacao)
        {
            db.manutencao_instalacao.Add(manutencao_instalacao);
            db.SaveChanges();

            Response.Write("<script> window.location='/manutencao/edit/" + manutencao_instalacao.id_manutencao + "?show=instalacao'; </script> ");
            Response.End();


            return RedirectToAction("Edit/" + manutencao_instalacao.id_manutencao);
        }


        
        public ActionResult editarpecar( int? substituido, int? id_manutencao, int? id_peca, int? id)
        {
            var manutencao_peca = db.manutencao_peca.FirstOrDefault(a => a.id == id);

            manutencao_peca.substituido = substituido;
            db.SaveChanges();

                
                //Response.Write("<script> window.location='/manutencao/edit/" + manutencao_peca.id_manutencao + "?show=peca'; </script> ");
                //Response.End();

                //return RedirectToAction("edit/" + manutencao.id + "?show=" + manutencao.url_foto);

                return Json(new { status = 1, msg = "Registro salvo com sucesso" }, JsonRequestBehavior.AllowGet);         
        

        }

        // GET: /Manutencao/DeletarPeca/5
        public ActionResult DeletarPeca(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            manutencao_peca manutencao_peca = db.manutencao_peca.Find(id);
            if (manutencao_peca == null)
            {
                return HttpNotFound();
            }
            return View(manutencao_peca);
        }

        // POST: /Manutencao/DeletarPeca/5
        [HttpPost, ActionName("DeletarPeca")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletarPecaConfirmed(int id)
        {
            manutencao_peca manutencao_peca = db.manutencao_peca.Find(id);
            db.manutencao_peca.Remove(manutencao_peca);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Manutencao/DeletarInstalacao/5
        public ActionResult DeletarInstalacao(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            manutencao_instalacao manutencao_instalacao = db.manutencao_instalacao.Find(id);
            if (manutencao_instalacao == null)
            {
                return HttpNotFound();
            }
            return View(manutencao_instalacao);
        }

        // POST: /Manutencao/DeletarInstalacao/5
        [HttpPost, ActionName("DeletarInstalacao")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletarInstalacaoConfirmed(int id, manutencao manutencao)
        {
            manutencao_instalacao manutencao_instalacao = db.manutencao_instalacao.Find(id);
            db.manutencao_instalacao.Remove(manutencao_instalacao);
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
