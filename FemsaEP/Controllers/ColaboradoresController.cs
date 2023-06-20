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
    public class ColaboradoresController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Colaboradores/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var colaborador_sap = db.colaborador_SAP.Include(c => c.grupo_SAP).Include(c => c.territorio_SAP).Include(c => c.unidade_SAP);
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(colaborador_sap.OrderBy(p => p.ZCODCOLAB).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Colaboradores/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            colaborador_SAP colaborador_sap = db.colaborador_SAP.Find(id);
            if (colaborador_sap == null)
            {
                return HttpNotFound();
            }
            return View(colaborador_sap);
        }

        // GET: /Colaboradores/Create
        public ActionResult Create()
        {
            ViewBag.ZGRUPO = new SelectList(db.grupo_SAP, "ZCODGRUPO", "ZNOMGRUPO");
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE");
            ViewBag.ZUNIDAD = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE");
            return View();
        }

        // POST: /Colaboradores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MANDT,ZCODCOLAB,ZNOMBRE,ZTELEFONO,ZEMAIL,ZFUNCARGO,ZTERRITOR,ZUNIDAD,ZNUMPATRI,ZNUMSERIE,ZCONTRASE,ZGRUPO,ZLOGIN,ZDESACTIV,descricao,id_usuario,id_colaborador,foto,idioma,acessoffline,ultimosinc")] colaborador_SAP colaborador_sap)
        {
            if (ModelState.IsValid)
            {
                db.colaborador_SAP.Add(colaborador_sap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ZGRUPO = new SelectList(db.grupo_SAP, "ZCODGRUPO", "ZNOMGRUPO", colaborador_sap.ZGRUPO);
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", colaborador_sap.ZTERRITOR);
            ViewBag.ZUNIDAD = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE", colaborador_sap.ZUNIDAD);
            return View(colaborador_sap);
        }

        public bool verificaigual(decimal ZCODCOLAB)
        {
            var check = db.colaborador_SAP.Where(u => u.ZCODCOLAB == ZCODCOLAB);

            if (check.Count() != 0)
            {
                return false;
            }
            return true;
        }

        // GET: /Colaboradores/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            colaborador_SAP colaborador_sap = db.colaborador_SAP.Find(id);
            if (colaborador_sap == null)
            {
                return HttpNotFound();
            }
            ViewBag.ZGRUPO = new SelectList(db.grupo_SAP, "ZCODGRUPO", "ZNOMGRUPO", colaborador_sap.ZGRUPO);
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", colaborador_sap.ZTERRITOR);
            ViewBag.ZUNIDAD = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE", colaborador_sap.ZUNIDAD);
            return View(colaborador_sap);
        }

        // POST: /Colaboradores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MANDT,ZCODCOLAB,ZNOMBRE,ZTELEFONO,ZEMAIL,ZFUNCARGO,ZTERRITOR,ZUNIDAD,ZNUMPATRI,ZNUMSERIE,ZCONTRASE,ZGRUPO,ZLOGIN,ZDESACTIV,descricao,id_usuario,id_colaborador,foto,idioma,acessoffline,ultimosinc")] colaborador_SAP colaborador_sap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(colaborador_sap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ZGRUPO = new SelectList(db.grupo_SAP, "ZCODGRUPO", "ZNOMGRUPO", colaborador_sap.ZGRUPO);
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", colaborador_sap.ZTERRITOR);
            ViewBag.ZUNIDAD = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE", colaborador_sap.ZUNIDAD);
            return View(colaborador_sap);
        }

        // GET: /Colaboradores/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            colaborador_SAP colaborador_sap = db.colaborador_SAP.Find(id);
            if (colaborador_sap == null)
            {
                return HttpNotFound();
            }
            return View(colaborador_sap);
        }

        // POST: /Colaboradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            colaborador_SAP colaborador_sap = db.colaborador_SAP.Find(id);
            db.colaborador_SAP.Remove(colaborador_sap);
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
