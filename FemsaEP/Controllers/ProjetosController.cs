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
    public class ProjetosController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Projetos/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var projeto = db.projeto;
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(projeto.OrderByDescending(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Projetos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            projeto projeto = db.projeto.Find(id);
            if (projeto == null)
            {
                return HttpNotFound();
            }
            return View(projeto);
        }

        // GET: /Projetos/Create
        public ActionResult Create()
        {
            ViewBag.pecacluster = new MultiSelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE");
            return View();
        }

        // POST: /Projetos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigo,descricao,observacao")] projeto projeto, int[] pecacluster)
        {
            if (ModelState.IsValid)
            {
                db.projeto.Add(projeto);
                db.SaveChanges();

                foreach (var itemselecionado in pecacluster)
                {
                    var sqlinsert = "INSERT INTO projeto_cluster (ZCODCLUST, id_projeto) VALUES (" + itemselecionado + "," + projeto.id + ");";
                    //executar sql meu amigo
                    db.Database.ExecuteSqlCommand(sqlinsert);
                }
                return RedirectToAction("Index");
            }

            return View(projeto);
        }

        // GET: /Projetos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            projeto projeto = db.projeto.Find(id);
            if (projeto == null)
            {
                return HttpNotFound();
            }
            var retornopecacluster = db.projeto_cluster.Where(cl => cl.id_projeto == id);
            int[] n = new int[1000];

            foreach (var item in retornopecacluster)
            {
                n[Convert.ToInt32(item.ZCODCLUST)] = Convert.ToInt32(item.ZCODCLUST);
            }
            ViewBag.pecacluster = new MultiSelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE", n);
            return View(projeto);
        }

        // POST: /Projetos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigo,descricao,observacao")] projeto projeto, int[] pecacluster)
        {
            if (ModelState.IsValid)
            {
                db.projeto_cluster.RemoveRange(db.projeto_cluster.Where(x => x.id_projeto == projeto.id));
                db.Entry(projeto).State = EntityState.Modified;
                db.SaveChanges();

                foreach (var itemselecionado in pecacluster)
                {
                    var sqlinsert = "INSERT INTO projeto_cluster (ZCODCLUST, id_projeto) VALUES (" + itemselecionado + "," + projeto.id + ");";
                    //executar sql meu amigo
                    db.Database.ExecuteSqlCommand(sqlinsert);
                }
                return RedirectToAction("Index");
            }
            return View(projeto);
        }

        // GET: /Projetos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            projeto projeto = db.projeto.Find(id);
            if (projeto == null)
            {
                return HttpNotFound();
            }
            var retornopecacluster = db.projeto_cluster.Where(cl => cl.id_projeto == id);
            int[] n = new int[1000];

            foreach (var item in retornopecacluster)
            {
                n[Convert.ToInt32(item.ZCODCLUST)] = Convert.ToInt32(item.ZCODCLUST);
            }
            return View(projeto);
        }

        // POST: /Projetos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            projeto projeto = db.projeto.Find(id);
            db.projeto_cluster.RemoveRange(db.projeto_cluster.Where(x => x.id_projeto == projeto.id));
            db.projeto.Remove(projeto);
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
