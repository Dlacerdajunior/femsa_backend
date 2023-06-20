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
    public class ContrapartidasController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Contrapartidas/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var contrapartida = db.contrapartida_SAP;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(contrapartida.OrderBy(p => p.ZCODCONPA).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Contrapartidas/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrapartida_SAP contrapartida = db.contrapartida_SAP.Find(id);
            if (contrapartida == null)
            {
                return HttpNotFound();
            }
            return View(contrapartida);
        }

        // GET: /Contrapartida/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Contrapartida/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANDT,ZCODCONPA,ZNOMCONPA,obrigatorio,descricao")] contrapartida_SAP contrapartida_sap)
        {
            if (ModelState.IsValid)
            {
                db.contrapartida_SAP.Add(contrapartida_sap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contrapartida_sap);
        }

        public bool verificaigual(decimal ZCODCONPA)
        {
            var check = db.contrapartida_SAP.Where(u => u.ZCODCONPA == ZCODCONPA);

            if (check.Count() != 0)
            {
                return false;
            }
            return true;
        }

        // GET: /Contrapartidas/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrapartida_SAP contrapartida = db.contrapartida_SAP.Find(id);
            if (contrapartida == null)
            {
                return HttpNotFound();
            }

            var retornoclientecluster = db.contrapartida_projeto.Where(cl => cl.ZCODCONPA == id);

            int[] n = new int[20];

            foreach (var item in retornoclientecluster)
            {
                n[Convert.ToInt32(item.id_projeto)] = Convert.ToInt32(item.id_projeto);
            }

            ViewBag.projetos = new MultiSelectList(db.projeto, "id", "descricao", n);

            return View(contrapartida);
        }

        // POST: /Contrapartidas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCODCONPA,ZNOMCONPA,obrigatorio,descricao")] contrapartida_SAP contrapartida, int[] projetos)
        {
            if (ModelState.IsValid)
            {
                db.contrapartida_projeto.RemoveRange(db.contrapartida_projeto.Where(x => x.ZCODCONPA == contrapartida.ZCODCONPA));  

                db.Entry(contrapartida).State = EntityState.Modified;

                db.SaveChanges();

                foreach (var itemselecionado in projetos){

                    var sqlinsert = "INSERT INTO contrapartida_projeto (ZCODCONPA, id_projeto) VALUES (" + contrapartida.ZCODCONPA + "," + itemselecionado + ");";
                    
                    //execurar sql meu amigo
                    db.Database.ExecuteSqlCommand(sqlinsert);
                }
                return RedirectToAction("Index");
            }
            return View(contrapartida);
        }

        // GET: /Contrapartidas/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrapartida_SAP contrapartida = db.contrapartida_SAP.Find(id);
            if (contrapartida == null)
            {
                return HttpNotFound();
            }

            var retornoclientecluster = db.contrapartida_projeto.Where(cl => cl.ZCODCONPA == id);

            int[] n = new int[20];

            foreach (var item in retornoclientecluster)
            {
                n[Convert.ToInt32(item.id_projeto)] = Convert.ToInt32(item.id_projeto);
            }

            return View(contrapartida);
        }

        // POST: /Contrapartidas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            contrapartida_SAP contrapartida_sap = db.contrapartida_SAP.Find(id);
            db.contrapartida_ativacoes.RemoveRange(db.contrapartida_ativacoes.Where(a => a.ZCODCONPA == id));
            db.contrapartida_projeto.RemoveRange(db.contrapartida_projeto.Where(x => x.ZCODCONPA == id));
            db.contrapartida_SAP.Remove(contrapartida_sap);
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
