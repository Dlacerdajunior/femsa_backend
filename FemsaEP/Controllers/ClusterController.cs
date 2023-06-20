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
using Excel;
using System.IO;

namespace FemsaEP.Controllers
{
    public class ClusterController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Cluster/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var cluster = db.cluster_SAP;

            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(cluster.OrderBy(p => p.ZCODCLUST).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Cluster/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cluster_SAP cluster_sap = db.cluster_SAP.Find(id);
            if (cluster_sap == null)
            {
                return HttpNotFound();
            }
            return View(cluster_sap);
        }

        // GET: /Cluster/Create
        public ActionResult Create()
        {
            ViewBag.foco = new SelectList(db.foco, "id", "descricao");
            ViewBag.ZNSE = new SelectList(db.nse_SAP, "ZCODNSE", "ZNOMNSE");
            ViewBag.canalsp = new SelectList(db.canal_SAP, "ZCODCANAL", "ZNOMCANAL");
            return View();
        }

        // POST: /Cluster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANDT,ZCODCLUST,ZNOMBRE,ZGPOCANAL,ZCANAL,ZGEC,ZNSE,descricao,foco,sigla,atributo4")] cluster_SAP cluster_sap, string canalsp)
        {
            if (ModelState.IsValid)
            {
                var trim = canalsp.Trim();
                cluster_sap.ZCANAL = trim;
                db.cluster_SAP.Add(cluster_sap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.foco = new SelectList(db.foco, "id", "descricao", cluster_sap.foco);
            ViewBag.canalsp = new SelectList(db.canal_SAP, "ZCODCANAL", "ZNOMCANAL", cluster_sap.ZCANAL);
            ViewBag.ZNSE = new SelectList(db.nse_SAP, "ZCODNSE", "ZNOMNSE", cluster_sap.ZNSE);

            return View(cluster_sap);
        }

        public bool verificaigual(decimal ZCODCLUST)
        {
            var check = db.cluster_SAP.Where(u => u.ZCODCLUST == ZCODCLUST);

            if (check.Count() != 0)
            {
                return false;
            }
            return true;
        }

        // GET: /Cluster/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cluster_SAP cluster_sap = db.cluster_SAP.Find(id);
            if (cluster_sap == null)
            {
                return HttpNotFound();
            }
            ViewBag.foco = new SelectList(db.foco, "id", "descricao", cluster_sap.foco);
            ViewBag.canalsp = new SelectList(db.canal_SAP, "ZCODCANAL", "ZNOMCANAL", cluster_sap.ZCANAL);
            ViewBag.ZNSE = new SelectList(db.nse_SAP, "ZCODNSE", "ZNOMNSE", cluster_sap.ZNSE);
            return View(cluster_sap);
        }

        // POST: /Cluster/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCODCLUST,ZNOMBRE,ZGPOCANAL,ZCANAL,ZGEC,ZNSE,descricao,foco,sigla,atributo4")] cluster_SAP cluster_sap, string canalsp)
        {
            if (ModelState.IsValid)
            {
                var trim = canalsp.Trim();
                cluster_sap.ZCANAL = trim;
                db.Entry(cluster_sap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ZNSE = new SelectList(db.nse_SAP, "ZCODNSE", "ZNOMNSE", cluster_sap.ZNSE);
            ViewBag.canalsp = new SelectList(db.canal_SAP, "ZCODCANAL", "ZNOMCANAL", cluster_sap.ZCANAL);
            ViewBag.foco = new SelectList(db.foco, "id", "descricao", cluster_sap.foco);
            return View(cluster_sap);
        }

        // GET: /Cluster/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cluster_SAP cluster_sap = db.cluster_SAP.Find(id);
            if (cluster_sap == null)
            {
                return HttpNotFound();
            }
            return View(cluster_sap);
        }

        // POST: /Cluster/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            cluster_SAP cluster_sap = db.cluster_SAP.Find(id);
            db.cluster_SAP.Remove(cluster_sap);
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



        // GET: /Upload/Upload
        public ActionResult Upload()
        {
            return View();
        }

        // POST: /Upload/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (upload.ContentLength > 0)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                    // to get started. This is how we avoid dependencies on ACE or Interop:
                    Stream stream = upload.InputStream;

                    // We return the interface, so that
                    IExcelDataReader reader = null;


                    if (upload.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (upload.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        // return View();
                    }

                    reader.IsFirstRowAsColumnNames = true;

                    System.Data.DataSet result = reader.AsDataSet();
                    reader.Close();

                    var rslista = result.Tables[0];

                    foreach (DataRow row in rslista.Rows)
                    {


                        var MANDT = "'" + row["MANDT"] + "'";
                        var ZCODCLUST = row["ZCODCLUST"].ToString();
                        var ZNOMBRE = "'" + row["ZNOMBRE"] + "'";
                        var ZGPOCANAL = "'" + row["ZGPOCANAL"] + "'";
                        var ZCANAL = "'" + row["ZCANAL"] + "'";
                        var ZGEC = "'" + row["ZGEC"] + "'";
                        var ZNSE = "'" + row["ZNSE"] + "'";
                        var descricao = "'" + row["descricao"] + "'";
                        var foco = "'" + row["foco"] + "'";
                        var sigla = "'" + row["sigla"] + "'";
                        var atributo4 = "'" + row["atributo4"] + "'";


                        if (ZCODCLUST != "")
                        {

                            if (MANDT == "''")
                            {
                                MANDT = "NULL";
                            }
                            if (ZNOMBRE == "''")
                            {
                                ZNOMBRE = "NULL";
                            }
                            if (ZGPOCANAL == "''")
                            {
                                ZGPOCANAL = "NULL";
                            }
                            if (ZCANAL == "''")
                            {
                                ZCANAL = "NULL";
                            }
                            if (ZGEC == "''")
                            {
                                ZGEC = "NULL";
                            }
                            if (ZNSE == "''")
                            {
                                ZNSE = "NULL";
                            }
                            if (descricao == "''")
                            {
                                descricao = "NULL";
                            }
                            if (foco == "''")
                            {
                                foco = "NULL";
                            }
                            if (sigla == "''")
                            {
                                sigla = "NULL";
                            }
                            if (atributo4 == "''")
                            {
                                atributo4 = "NULL";
                            }

                            var ZCODCLUST2 = Convert.ToDecimal(ZCODCLUST);

                            var checkpeca = db.cluster_SAP.Where(a => a.ZCODCLUST == ZCODCLUST2).FirstOrDefault();

                            if (checkpeca == null)
                            {
                                var sqlinsert = "INSERT INTO [dbo].[cluster_SAP] ([MANDT],[ZCODCLUST],[ZNOMBRE],[ZGPOCANAL],[ZCANAL],[ZGEC],[ZNSE],[descricao],[foco],[sigla],[atributo4])VALUES (" + MANDT + "," + "'" + ZCODCLUST + "'" + "," + ZNOMBRE + "," + ZGPOCANAL + "," + ZCANAL + "," + ZGEC + "," + ZNSE + "," + descricao + "," + foco + "," + sigla + "," + atributo4 + ")";
                                db.Database.ExecuteSqlCommand(sqlinsert);
                            }
                            else
                            {
                                if (checkpeca.MANDT != null && MANDT == "NULL")
                                {
                                    MANDT = "'" + checkpeca.MANDT.ToString() + "'";
                                    if (MANDT == "''" || MANDT == "")
                                    {
                                        MANDT = "NULL";
                                    }
                                }
                                if (checkpeca.ZNOMBRE != null && ZNOMBRE == "NULL")
                                {
                                    ZNOMBRE = "'" + checkpeca.ZNOMBRE + "'";
                                    if (ZNOMBRE == "''" || ZNOMBRE == "")
                                    {
                                        ZNOMBRE = "NULL";
                                    }
                                }
                                if (checkpeca.ZGPOCANAL != null && ZGPOCANAL == "NULL")
                                {
                                    ZGPOCANAL = "'" + checkpeca.ZGPOCANAL + "'";
                                    if (ZGPOCANAL == "''" || ZGPOCANAL == "")
                                    {
                                        ZGPOCANAL = "NULL";
                                    }
                                }
                                if (checkpeca.ZCANAL != null && ZCANAL == "NULL")
                                {
                                    ZCANAL = "'" + checkpeca.ZCANAL + "'";
                                    if (ZCANAL == "''" || ZCANAL == "")
                                    {
                                        ZCANAL = "NULL";
                                    }
                                }
                                if (checkpeca.ZGEC != null && ZGEC == "NULL")
                                {
                                    ZGEC = "'" + checkpeca.ZGEC + "'";
                                    if (ZGEC == "''" || ZGEC == "")
                                    {
                                        ZGEC = "NULL";
                                    }
                                }
                                if (checkpeca.ZNSE != null && ZNSE == "NULL")
                                {
                                    ZNSE = "'" + checkpeca.ZNSE + "'";
                                    if (ZNSE == "''" || ZNSE == "")
                                    {
                                        ZNSE = "NULL";
                                    }
                                }
                                if (checkpeca.descricao != null && descricao == "NULL")
                                {
                                    descricao = "'" + checkpeca.descricao + "'";
                                    if (descricao == "''" || descricao == "")
                                    {
                                        descricao = "NULL";
                                    }
                                }
                                if (checkpeca.foco != null && foco == "NULL")
                                {
                                    foco = "'" + checkpeca.foco + "'";
                                    if (foco == "''" || foco == "")
                                    {
                                        foco = "NULL";
                                    }
                                }
                                if (checkpeca.sigla != null && sigla == "NULL")
                                {
                                    sigla = "'" + checkpeca.sigla + "'";
                                    if (sigla == "''" || sigla == "")
                                    {
                                        sigla = "NULL";
                                    }
                                }
                                if (checkpeca.atributo4 != null && atributo4 == "NULL")
                                {
                                    atributo4 = "'" + checkpeca.atributo4 + "'";
                                    if (atributo4 == "''" || atributo4 == "")
                                    {
                                        atributo4 = "NULL";
                                    }
                                }

                                var sqlinsert = "UPDATE [dbo].[cluster_SAP] SET" + " [MANDT] = " + MANDT + ", [ZNOMBRE] = " + ZNOMBRE + ", [ZGPOCANAL] = " + ZGPOCANAL + ", [ZCANAL] = " + ZCANAL + ", [ZGEC] = " + ZGEC + ", [ZNSE] = " + ZNSE + ", [descricao] = " + descricao + ", [foco] = " + foco + ", [sigla] = " + sigla + ", [atributo4] = " + atributo4 + " WHERE ([ZCODCLUST] = " + "'" + ZCODCLUST + "'" + ")";
                                db.Database.ExecuteSqlCommand(sqlinsert);

                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

    }
}
