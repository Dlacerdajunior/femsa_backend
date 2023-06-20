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
    public class BookOnlineController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /BookOnline/
        public ActionResult Index()
        {
            return View(db.book_online.ToList());
        }

        // GET: /BookOnline/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book_online book_online = db.book_online.Find(id);
            if (book_online == null)
            {
                return HttpNotFound();
            }
            return View(book_online);
        }

        // GET: /BookOnline/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /BookOnline/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,codigo,url_upload,titulo,descricao")] book_online book_online)
        {
            if (ModelState.IsValid)
            {
                db.book_online.Add(book_online);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book_online);
        }

        // GET: /BookOnline/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book_online book_online = db.book_online.Find(id);
            if (book_online == null)
            {
                return HttpNotFound();
            }
            return View(book_online);
        }

        // POST: /BookOnline/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,codigo,url_upload,titulo,descricao")] book_online book_online)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book_online).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book_online);
        }

        // GET: /BookOnline/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book_online book_online = db.book_online.Find(id);
            if (book_online == null)
            {
                return HttpNotFound();
            }
            return View(book_online);
        }

        // POST: /BookOnline/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            book_online book_online = db.book_online.Find(id);
            db.book_online.Remove(book_online);
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
