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
    public class CadastroPecasController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /CadastroPecas/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var peca = db.peca_SAP;
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(peca.OrderByDescending(p => p.ZCODPECA).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /CadastroPecas2/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            peca_SAP peca_sap = db.peca_SAP.Find(id);
            if (peca_sap == null)
            {
                return HttpNotFound();
            }
            return View(peca_sap);
        }

        // GET: /CadastroPecas2/Create
        public ActionResult Create()
        {
            ViewBag.pecacluster = new MultiSelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE");
            return View();
        }

        // POST: /CadastroPecas2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANDT,ZCODPECA,ZNOMPECA,ZDESACTIV,descricao")] peca_SAP peca_sap, int[] pecacluster)
        {
            if (ModelState.IsValid)
            {

                var carac = new caracteristica_de_peca_SAP
                {
                    ZCODPIEZA = peca_sap.ZCODPECA,
                    ZCODIPECA = peca_sap.ZCODPECA
                };
                db.peca_SAP.Add(peca_sap);
                db.caracteristica_de_peca_SAP.Add(carac);                
                db.SaveChanges();

                foreach (var itemselecionado in pecacluster)
                {
                    var sqlinsert = "INSERT INTO cluster_peca_SAP (ZCODCLUST, ZCODPECA) VALUES (" + itemselecionado + "," + peca_sap.ZCODPECA + ");";
                    //executar sql meu amigo
                    db.Database.ExecuteSqlCommand(sqlinsert);
                }

                return RedirectToAction("Index");
            }

            return View(peca_sap);
        }

        public bool verificaigual(decimal ZCODPECA)
        {
            var check = db.peca_SAP.Where(u => u.ZCODPECA == ZCODPECA);

            if (check.Count() != 0)
            {
                return false;
            }
            return true;
        }

        // GET: /CadastroPecas2/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            peca_SAP peca_sap = db.peca_SAP.Find(id);
            if (peca_sap == null)
            {
                return HttpNotFound();
            }
            var retornopecacluster = db.cluster_peca_SAP.Where(cl => cl.ZCODPECA == id);
            int[] n = new int[1000];

            foreach (var item in retornopecacluster)
            {
                n[Convert.ToInt32(item.ZCODCLUST)] = Convert.ToInt32(item.ZCODCLUST);
            }

            ViewBag.pecacluster = new MultiSelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE", n);

            return View(peca_sap);
        }

        // POST: /CadastroPecas2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCODPECA,ZNOMPECA,ZDESACTIV,descricao")] peca_SAP peca_sap, int[] pecacluster)
        {
            if (ModelState.IsValid)
            {
                db.cluster_peca_SAP.RemoveRange(db.cluster_peca_SAP.Where(x => x.ZCODPECA == peca_sap.ZCODPECA));
                db.Entry(peca_sap).State = EntityState.Modified;
                db.SaveChanges();

                foreach (var itemselecionado in pecacluster)
                {
                    var sqlinsert = "INSERT INTO cluster_peca_SAP (ZCODCLUST, ZCODPECA) VALUES (" + itemselecionado + "," + peca_sap.ZCODPECA + ");";
                    //executar sql meu amigo
                    db.Database.ExecuteSqlCommand(sqlinsert);
                }
                return RedirectToAction("Index");
            }

            return View(peca_sap);
        }

        // GET: /CadastroPecas2/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            peca_SAP peca_sap = db.peca_SAP.Find(id);
            if (peca_sap == null)
            {
                return HttpNotFound();
            }
            var retornopecacluster = db.cluster_peca_SAP.Where(cl => cl.ZCODPECA == id);
            int[] n = new int[1000];

            foreach (var item in retornopecacluster)
            {
                n[Convert.ToInt32(item.ZCODCLUST)] = Convert.ToInt32(item.ZCODCLUST);
            }
            return View(peca_sap);
        }

        // POST: /CadastroPecas2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            peca_SAP peca_sap = db.peca_SAP.Find(id);
            db.cluster_peca_SAP.RemoveRange(db.cluster_peca_SAP.Where(x => x.ZCODPECA == peca_sap.ZCODPECA));
            db.peca_SAP.Remove(peca_sap);
            db.SaveChanges();
            return RedirectToAction("Index");
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
                        var ZCODPECA =  row["ZCODPECA"].ToString();
                        var ZNOMPECA = "'" + row["ZNOMPECA"] + "'";
                        var ZDESACTIV = "'" + row["ZDESACTIV"] + "'";
                        var descricao = "'" + row["descricao"] + "'";

                      //  var MANDT = "'" + row["MANDT"] + "'";
                        var ZCODPIEZA = "'" + row["ZCODPIEZA"] + "'";
                        var ZCODIPECA = "'" + row["ZCODPECA"] + "'";
                        var ZCODMARCA = "'" + row["ZCODMARCA"] + "'";
                        var ZCODEMBAL = "'" + row["ZCODEMBAL"] + "'";
                        var ZCODALIME = "'" + row["ZCODALIME"] + "'";
                        var ZCODIMAGEN = "'" + row["ZCODIMAGEN"] + "'";
                        var ZDETALLE = "'" + row["ZDETALLE"] + "'";
                        var ZCODKITS = "'" + row["ZCODKITS"] + "'";
                        var ZFORMCUS = "'" + row["ZFORMCUS"] + "'";
                        var ZCOSTOM2 = "'" + row["ZCOSTOM2"] + "'";
                        var ZANCHO = "'" + row["ZANCHO"] + "'";
                        var ZALTURA = "'" + row["ZALTURA"] + "'";
                        var ZDIAMETRO = "'" + row["ZDIAMETRO"] + "'";
                        var ZPROVEEDO = "'" + row["ZPROVEEDO"] + "'";
                        var ZVALORPZA = "'" + row["ZVALORPZA"] + "'";
                     //   var ZDESACTIV = "'" + row["ZDESACTIV"] + "'";
                        var valor = "'" + row["valor"] + "'";
                        var foto_url = "'" + row["foto_url"] + "'";


                    if (ZCODPECA != "")
                    {

                        if (MANDT == "''")
                        {
                            MANDT = "NULL";
                        }
                        if (ZNOMPECA == "''")
                        {
                            ZNOMPECA = "NULL";
                        }
                        if (ZDESACTIV == "''")
                        {
                            ZDESACTIV = "NULL";
                        }
                        if (descricao == "''")
                        {
                            descricao = "NULL";
                        }
                        if (ZCODPIEZA == "''")
                        {
                            ZCODPIEZA = "NULL";
                        }
                        if (ZCODIPECA == "''")
                        {
                            ZCODIPECA = "NULL";
                        }
                        if (ZCODMARCA == "''")
                        {
                            ZCODMARCA = "NULL";
                        }
                        if (ZCODEMBAL == "''")
                        {
                            ZCODEMBAL = "NULL";
                        }
                        if (ZCODALIME == "''")
                        {
                            ZCODALIME = "NULL";
                        }
                        if (ZCODIMAGEN == "''")
                        {
                            ZCODIMAGEN = "NULL";
                        }
                        if (ZDETALLE == "''")
                        {
                            ZDETALLE = "NULL";
                        }
                        if (ZCODKITS == "''")
                        {
                            ZCODKITS = "NULL";
                        }
                        if (ZFORMCUS == "''")
                        {
                            ZFORMCUS = "NULL";
                        }
                        if (ZCOSTOM2 == "''")
                        {
                            ZCOSTOM2 = "NULL";
                        }
                        if (ZANCHO == "''")
                        {
                            ZANCHO = "NULL";
                        }
                        if (ZALTURA == "''")
                        {
                            ZALTURA = "NULL";
                        }
                        if (ZDIAMETRO == "''")
                        {
                            ZDIAMETRO = "NULL";
                        }
                        if (ZPROVEEDO == "''")
                        {
                            ZPROVEEDO = "NULL";
                        }
                        if (ZVALORPZA == "''")
                        {
                            ZVALORPZA = "NULL";
                        }
                        if (valor == "''")
                        {
                            valor = "NULL";
                        }
                        if (foto_url == "''")
                        {
                            foto_url = "NULL";
                        }
                        
                            var ZCODPECA2 = Convert.ToDecimal(ZCODPECA);      

                            var checkpeca = db.peca_SAP.Where(a => a.ZCODPECA == ZCODPECA2).FirstOrDefault();
                            var checkpeca2 = db.caracteristica_de_peca_SAP.Where(a => a.ZCODIPECA == ZCODPECA2).FirstOrDefault();
                            
                            if (checkpeca == null)
                            {
                                var sqlinsert = "INSERT INTO [dbo].[peca_SAP] ([MANDT],[ZCODPECA],[ZNOMPECA],[ZDESACTIV],[descricao])VALUES (" + MANDT + "," + "'" + ZCODPECA + "'" + "," + ZNOMPECA + "," + ZDESACTIV + "," + descricao + ")";
                                db.Database.ExecuteSqlCommand(sqlinsert);
                                var sqlinsert2 = "INSERT INTO [dbo].[caracteristica_de_peca_SAP] ([MANDT],[ZCODPIEZA],[ZCODIPECA],[ZCODMARCA],[ZCODEMBAL],[ZCODALIME],[ZCODIMAGEN],[ZDETALLE],[ZCODKITS],[ZFORMCUS],[ZCOSTOM2],[ZANCHO],[ZALTURA],[ZDIAMETRO],[ZPROVEEDO],[ZVALORPZA],[ZDESACTIV],[valor],[foto_url]) VALUES (" + MANDT + "," + ZCODPIEZA + "," + "'" + ZCODPECA + "'" + "," + ZCODMARCA + "," + ZCODEMBAL + "," + ZCODALIME + "," + ZCODIMAGEN + "," + ZDETALLE + "," + ZCODKITS + "," + ZFORMCUS + "," + ZCOSTOM2 + "," + ZANCHO + "," + ZALTURA + "," + ZDIAMETRO + "," + ZPROVEEDO + "," + ZVALORPZA + "," + ZDESACTIV + "," + valor + "," + foto_url + ")";
                                db.Database.ExecuteSqlCommand(sqlinsert2);
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
                                if (checkpeca.ZNOMPECA != null && ZNOMPECA == "NULL")
                                {
                                    ZNOMPECA = "'" + checkpeca.ZNOMPECA + "'";
                                    if (ZNOMPECA == "''" || ZNOMPECA == "")
                                    {
                                        ZNOMPECA = "NULL";
                                    }
                                }
                                if (checkpeca.ZDESACTIV != null && ZDESACTIV == "NULL")
                                {
                                    ZDESACTIV = "'" + checkpeca.ZDESACTIV + "'";
                                    if (ZDESACTIV == "''" || ZDESACTIV == "")
                                    {
                                        ZDESACTIV = "NULL";
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
                               /* if (checkpeca2.ZCODPIEZA != null && ZCODPIEZA == "NULL")
                                {
                                    ZCODPIEZA = "'" + checkpeca2.ZCODPIEZA.ToString() + "'";
                                    if (ZCODPIEZA == "''" || ZCODPIEZA == "")
                                    {
                                        ZCODPIEZA = "NULL";
                                    }
                                }*/
                                if (checkpeca2.ZCODIPECA != null && ZCODIPECA == "NULL")
                                {
                                    ZCODIPECA = "'" + checkpeca2.ZCODIPECA.ToString() + "'";
                                    if (ZCODIPECA == "''" || ZCODIPECA == "")
                                    {
                                        ZCODIPECA = "NULL";
                                    }
                                }
                                if (checkpeca2.ZCODMARCA != null && ZCODMARCA == "NULL")
                                {
                                    ZCODMARCA = "'" + checkpeca2.ZCODMARCA.ToString() + "'";
                                    if (ZCODMARCA == "''" || ZCODMARCA == "")
                                    {
                                        ZCODMARCA = "NULL";
                                    }
                                }
                                if (checkpeca2.ZCODEMBAL != null && ZCODEMBAL == "NULL")
                                {
                                    ZCODEMBAL = "'" + checkpeca2.ZCODEMBAL.ToString() + "'";
                                    if (ZCODEMBAL == "''" || ZCODEMBAL == "")
                                    {
                                        ZCODEMBAL = "NULL";
                                    }
                                }
                                if (checkpeca2.ZCODALIME != null && ZCODALIME == "NULL")
                                {
                                    ZCODALIME = "'" + checkpeca2.ZCODALIME.ToString() + "'";
                                    if (ZCODALIME == "''" || ZCODALIME == "")
                                    {
                                        ZCODALIME = "NULL";
                                    }
                                }
                                if (checkpeca2.ZCODIMAGEN != null && ZCODIMAGEN == "NULL")
                                {
                                    ZCODIMAGEN = "'" + checkpeca2.ZCODIMAGEN + "'";
                                    if (ZCODIMAGEN == "''" || ZCODIMAGEN == "")
                                    {
                                        ZCODIMAGEN = "NULL";
                                    }
                                }
                                if (checkpeca2.ZDETALLE != null && ZDETALLE == "NULL")
                                {
                                    ZDETALLE = "'" + checkpeca2.ZDETALLE + "'";
                                    if (ZDETALLE == "''" || ZDETALLE == "")
                                    {
                                        ZDETALLE = "NULL";
                                    }
                                }
                                if (checkpeca2.ZCODKITS != null && ZCODKITS == "NULL")
                                {
                                    ZCODKITS = "'" + checkpeca2.ZCODKITS.ToString() + "'";
                                    if (ZCODKITS == "''" || ZCODKITS == "")
                                    {
                                        ZCODKITS = "NULL";
                                    }
                                }
                                if (checkpeca2.ZFORMCUS != null && ZFORMCUS == "NULL")
                                {
                                    ZFORMCUS = "'" + checkpeca2.ZFORMCUS + "'";
                                    if (ZFORMCUS == "''" || ZFORMCUS == "")
                                    {
                                        ZFORMCUS = "NULL";
                                    }
                                }
                                if (checkpeca2.ZCOSTOM2 != null && ZCOSTOM2 == "NULL")
                                {
                                    ZCOSTOM2 = "'" + checkpeca2.ZCOSTOM2.ToString() + "'";
                                    if (ZCOSTOM2 == "''" || ZCOSTOM2 == "")
                                    {
                                        ZCOSTOM2 = "NULL";
                                    }
                                }
                                if (checkpeca2.ZANCHO != null && ZANCHO == "NULL")
                                {
                                    ZANCHO = "'" + checkpeca2.ZANCHO.ToString() + "'";
                                    if (ZANCHO == "''" || ZANCHO == "")
                                    {
                                        ZANCHO = "NULL";
                                    }
                                }
                                if (checkpeca2.ZALTURA != null && ZALTURA == "NULL")
                                {
                                    ZALTURA = "'" + checkpeca2.ZALTURA.ToString() + "'";
                                    if (ZALTURA == "''" || ZALTURA == "")
                                    {
                                        ZALTURA = "NULL";
                                    }
                                }
                                if (checkpeca2.ZDIAMETRO != null && ZDIAMETRO == "NULL")
                                {
                                    ZDIAMETRO = "'" + checkpeca2.ZDIAMETRO.ToString() + "'";
                                    if (ZDIAMETRO == "''" || ZDIAMETRO == "")
                                    {
                                        ZDIAMETRO = "NULL";
                                    }
                                }
                                if (checkpeca2.ZPROVEEDO != null && ZPROVEEDO == "NULL")
                                {
                                    ZPROVEEDO = "'" + checkpeca2.ZPROVEEDO.ToString() + "'";
                                    if (ZPROVEEDO == "''" || ZPROVEEDO == "")
                                    {
                                        ZPROVEEDO = "NULL";
                                    }
                                }
                                if (checkpeca2.ZVALORPZA != null && ZVALORPZA == "NULL")
                                {
                                    ZVALORPZA = "'" + checkpeca2.ZVALORPZA.ToString() + "'";
                                    if (ZVALORPZA == "''" || ZVALORPZA == "")
                                    {
                                        ZVALORPZA = "NULL";
                                    }
                                }
                                if (checkpeca2.valor != null && valor == "NULL")
                                {
                                    valor = "'" + checkpeca2.valor + "'";
                                    if (valor == "''" || valor == "")
                                    {
                                        valor = "NULL";
                                    }
                                }
                                if (checkpeca2.foto_url != null && foto_url == "NULL")
                                {
                                    foto_url = "'" + checkpeca2.foto_url + "'";
                                    if (foto_url == "''" || foto_url == "")
                                    {
                                        foto_url = "NULL";
                                    }
                                }
                                /*  + ", [ZCODPIEZA] = " + ZCODPIEZA */
                                var sqlinsert = "UPDATE [dbo].[peca_SAP] SET" + " [MANDT] = " + MANDT + ", [ZNOMPECA] = " + ZNOMPECA + ", [ZDESACTIV] = " + ZDESACTIV + ", [descricao] = " + descricao + " WHERE ([ZCODPECA] = " + "'" + ZCODPECA + "'" + ")";
                                db.Database.ExecuteSqlCommand(sqlinsert);
                                var sqlinsert2 = "UPDATE [dbo].[caracteristica_de_peca_SAP] SET" + " [MANDT] = " + MANDT + ", [ZCODMARCA] = " + ZCODMARCA + ", [ZCODIPECA] = " + ZCODIPECA + ", [ZCODEMBAL] = " + ZCODEMBAL + ", [ZCODALIME] = " + ZCODALIME + ", [ZCODIMAGEN] = " + ZCODIMAGEN + ", [ZDETALLE] = " + ZDETALLE + ", [ZCODKITS] = " + ZCODKITS + ", [ZFORMCUS] = " + ZFORMCUS + ", [ZCOSTOM2] = " + ZCOSTOM2 + ", [ZANCHO] = " + ZANCHO + ", [ZALTURA] = " + ZALTURA + ", [ZDIAMETRO] = " + ZDIAMETRO + ", [ZPROVEEDO] = " + ZPROVEEDO + ", [ZVALORPZA] = " + ZVALORPZA + ", [ZDESACTIV] = " + ZDESACTIV + ", [valor] = " + valor + ", [foto_url] = " + foto_url + " WHERE ([ZCODPIEZA] = " + "" + ZCODPIEZA + "" + ")";
                                db.Database.ExecuteSqlCommand(sqlinsert2);

                            }
                        }
                    }
                }
            }
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
