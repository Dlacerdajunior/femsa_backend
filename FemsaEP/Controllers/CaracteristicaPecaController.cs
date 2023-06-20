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
    public class CaracteristicaPecaController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /CaracteristicaPeca/
        public ActionResult Index()
        {
            var caracteristica_de_peca_sap = db.caracteristica_de_peca_SAP.Include(c => c.alimento_SAP).Include(c => c.embalagem_SAP).Include(c => c.peca_SAP).Include(c => c.marca_SAP);
            return View(caracteristica_de_peca_sap.ToList());
        }

        // GET: /CaracteristicaPeca/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            caracteristica_de_peca_SAP caracteristica_de_peca_sap = db.caracteristica_de_peca_SAP.Find(id);
            if (caracteristica_de_peca_sap == null)
            {
                return HttpNotFound();
            }
            return View(caracteristica_de_peca_sap);
        }

        // GET: /CaracteristicaPeca/Create
        public ActionResult Create()
        {
            ViewBag.ZCODALIME = new SelectList(db.alimento_SAP, "ZCODIGOALIMENTO", "ZNOMBREALIMENTO");
            ViewBag.ZCODEMBAL = new SelectList(db.embalagem_SAP, "ZCODEMBAL", "ZNOMEMBAL");
            ViewBag.ZCODIPECA = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA");
            ViewBag.ZCODMARCA = new SelectList(db.marca_SAP, "ZCODMARCA", "ZNOMMARCA");
            return View();
        }

        // POST: /CaracteristicaPeca/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MANDT,ZCODPIEZA,ZCODIPECA,ZCODMARCA,ZCODEMBAL,ZCODALIME,ZCODIMAGEN,ZDETALLE,ZCODKITS,ZFORMCUS,ZCOSTOM2,ZANCHO,ZALTURA,ZDIAMETRO,ZPROVEEDO,ZVALORPZA,ZDESACTIV,valor,foto_url")] caracteristica_de_peca_SAP caracteristica_de_peca_sap)
        {
            if (ModelState.IsValid)
            {
                db.caracteristica_de_peca_SAP.Add(caracteristica_de_peca_sap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ZCODALIME = new SelectList(db.alimento_SAP, "ZCODIGOALIMENTO", "ZNOMBREALIMENTO", caracteristica_de_peca_sap.ZCODALIME);
            ViewBag.ZCODEMBAL = new SelectList(db.embalagem_SAP, "ZCODEMBAL", "ZNOMEMBAL", caracteristica_de_peca_sap.ZCODEMBAL);
            ViewBag.ZCODIPECA = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA", caracteristica_de_peca_sap.ZCODIPECA);
            ViewBag.ZCODMARCA = new SelectList(db.marca_SAP, "ZCODMARCA", "ZNOMMARCA", caracteristica_de_peca_sap.ZCODMARCA);
            return View(caracteristica_de_peca_sap);
        }

        public ActionResult checkm2(decimal id_fornecedor)
        {
            var totalinfo = db.provedor_SAP.FirstOrDefault(a => a.ZCODPROVE == id_fornecedor).VALORM2;          

            return Json(totalinfo, JsonRequestBehavior.AllowGet);
        }

        // GET: /CaracteristicaPeca/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            caracteristica_de_peca_SAP caracteristica_de_peca_sap = db.caracteristica_de_peca_SAP.Find(id);
            if (caracteristica_de_peca_sap == null)
            {
                var criai = new caracteristica_de_peca_SAP();
                criai.ZCODIPECA = id;
                criai.ZCODPIEZA = id;
                db.caracteristica_de_peca_SAP.Add(criai);
                db.SaveChanges();                
                return View();
            }

            ViewBag.ZPROVEEDO = new SelectList(db.provedor_SAP, "ZCODPROVE", "ZNOMBRE", caracteristica_de_peca_sap.ZPROVEEDO);
            ViewBag.kits = new SelectList(db.kit_SAP, "ZCODKITS", "ZNOMKITS", caracteristica_de_peca_sap.ZCODKITS);
            ViewBag.ZCODALIME = new SelectList(db.alimento_SAP, "ZCODIGOALIMENTO", "ZNOMBREALIMENTO", caracteristica_de_peca_sap.ZCODALIME);
            ViewBag.ZCODEMBAL = new SelectList(db.embalagem_SAP, "ZCODEMBAL", "ZNOMEMBAL", caracteristica_de_peca_sap.ZCODEMBAL);
            ViewBag.ZCODIPECA = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA", caracteristica_de_peca_sap.ZCODIPECA);
            ViewBag.ZCODMARCA = new SelectList(db.marca_SAP, "ZCODMARCA", "ZNOMMARCA", caracteristica_de_peca_sap.ZCODMARCA);
            return View(caracteristica_de_peca_sap);
        }

        // POST: /CaracteristicaPeca/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCODPIEZA,ZCODIPECA,ZCODMARCA,ZCODEMBAL,ZCODALIME,ZCODIMAGEN,ZDETALLE,ZCODKITS,ZFORMCUS,ZCOSTOM2,ZANCHO,ZALTURA,ZDIAMETRO,ZPROVEEDO,ZVALORPZA,ZDESACTIV,valor,foto_url")] caracteristica_de_peca_SAP caracteristica_de_peca_sap, string kits)
        {
            if (ModelState.IsValid)
            {
                if (kits != "")
                {
                    caracteristica_de_peca_sap.ZCODKITS = Convert.ToDecimal(kits);                    
                }
                db.Entry(caracteristica_de_peca_sap).State = EntityState.Modified;
                
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }

            ViewBag.ZPROVEEDO = new SelectList(db.provedor_SAP, "ZCODPROVE", "ZNOMBRE", caracteristica_de_peca_sap.ZPROVEEDO);
            ViewBag.kits = new SelectList(db.kit_SAP, "ZCODKITS", "ZNOMKITS", caracteristica_de_peca_sap.ZCODKITS);
            ViewBag.ZCODALIME = new SelectList(db.alimento_SAP, "ZCODIGOALIMENTO", "ZNOMBREALIMENTO", caracteristica_de_peca_sap.ZCODALIME);
            ViewBag.ZCODEMBAL = new SelectList(db.embalagem_SAP, "ZCODEMBAL", "ZNOMEMBAL", caracteristica_de_peca_sap.ZCODEMBAL);
            ViewBag.ZCODIPECA = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA", caracteristica_de_peca_sap.ZCODIPECA);
            ViewBag.ZCODMARCA = new SelectList(db.marca_SAP, "ZCODMARCA", "ZNOMMARCA", caracteristica_de_peca_sap.ZCODMARCA);
            return View(caracteristica_de_peca_sap);
        }

        // GET: /CaracteristicaPeca/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            caracteristica_de_peca_SAP caracteristica_de_peca_sap = db.caracteristica_de_peca_SAP.Find(id);
            if (caracteristica_de_peca_sap == null)
            {
                return HttpNotFound();
            }
            return View(caracteristica_de_peca_sap);
        }

        // POST: /CaracteristicaPeca/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            caracteristica_de_peca_SAP caracteristica_de_peca_sap = db.caracteristica_de_peca_SAP.Find(id);
            db.caracteristica_de_peca_SAP.Remove(caracteristica_de_peca_sap);
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
