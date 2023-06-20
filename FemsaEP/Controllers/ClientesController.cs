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
using System.IO;
using Excel;

namespace FemsaEP.Controllers
{
    public class ClientesController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Clientes/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var cliente_sap = db.cliente_SAP.Include(c => c.ativacao).Include(c => c.canal_SAP).Include(c => c.cluster_SAP).Include(c => c.colaborador_SAP).Include(c => c.executivo1).Include(c => c.franquia).Include(c => c.prospector1).Include(c => c.motivo_de_recuso_SAP).Include(c => c.status_ativacao1).Include(c => c.localizacao_SAP).Include(c => c.territorio_SAP).Include(c => c.kit_SAP).Include(c => c.zat_SAP).Include(c => c.unidade_SAP);
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;

            return View(cliente_sap.OrderBy(p => p.ZCLIENTE).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Clientes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cliente_SAP cliente_sap = db.cliente_SAP.Find(id);
            if (cliente_sap == null)
            {
                return HttpNotFound();
            }
            return View(cliente_sap);
        }

        public bool verificaigual(string ZCLIENTE)
        {
            var check = db.cliente_SAP.Where(u => u.ZCLIENTE == ZCLIENTE);

            if (check.Count() != 0)
            {
                return false;
            }
            return true;
        }

        // GET: /Clientes/Create
        public ActionResult Create()
        {
            ViewBag.status_ativacao = new SelectList(db.ativacao, "id", "codigo");
            ViewBag.ZCANAL = new SelectList(db.canal_SAP, "ZCODCANAL", "ZNOMCANAL");
            ViewBag.id_cluster = new MultiSelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE");
            ViewBag.id_colaborador = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE");
            ViewBag.id_executivo = new SelectList(db.executivo, "id", "nome");
            ViewBag.id_franquia = new SelectList(db.franquia, "id", "descricao");
            ViewBag.id_prospector = new SelectList(db.prospector, "id", "nome");
            ViewBag.motivo_id = new SelectList(db.motivo_de_recuso_SAP, "ZCODMOTIV", "ZNOMMOTIV");
            ViewBag.status_ativacao = new SelectList(db.status_ativacao, "id", "nome");
            ViewBag.ZLOCALIZA = new SelectList(db.localizacao_SAP, "ZCODLOCA", "ZNOMLOCA");
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE");
            ViewBag.ZKIT = new SelectList(db.kit_SAP, "ZCODKITS", "ZNOMKITS");
            ViewBag.ZZAT = new SelectList(db.zat_SAP, "ZCODZATS", "ZNOMBRE");
            ViewBag.ZUNIDAD = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE");
            return View();
        }

        // POST: /Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANDT,ZCLIENTE,ZNOMBRE,ZNOMFANTA,ZCNPJCPFI,ZCEP,ZDIRECCIO,ZBARRIO,ZCIUDAD,ZUF,ZPUNTOREF,ZTERRITOR,ZUNIDAD,ZTELEFONO,ZCANAL,ZGEC,ZNSE,ZRUTA,ZZAT,ZKIT,ZMARCAGDM,ZLOCALIZA,ZNOMSHOPP,ZRED,ZNOMBRERE,ZHABACTIV,ZHABRMANT,ZHABSHOPP,descricao,id_novo,id_antigo,id_cluster,executivo,prospector,desenvolvedor,fornecedor,design,zona_transporte,regional,numero,lattext,longtext,status_ativacao,id_colaborador,custo_budget,motivo_id,motivotxt,cidade_limpa,segundo_telefone,id_executivo,id_prospector,ano_cliente,acessooffline,id_franquia,recusa_foto,ativacao_ultima_atualizacao,observacao")] cliente_SAP cliente_sap, int[] id_cluster)
        {

            if (ModelState.IsValid)
            {
                var data = DateTime.Now;
                cliente_sap.ano_cliente = data.Year.ToString();
                db.cliente_SAP.Add(cliente_sap);
                db.SaveChanges();

                foreach (var itemselecionado in id_cluster)
                {
                    var sqlinsert = "INSERT INTO cliente_cluster_SAP (ZCLIENTE, ZCODCLUSTER) VALUES ('" + cliente_sap.ZCLIENTE + "'," + itemselecionado + ");";

                    //execurar sql meu amigo
                    db.Database.ExecuteSqlCommand(sqlinsert);
                }

                return RedirectToAction("Index");
            }

            var codigo_cliente = db.cliente_SAP.Where(c => c.ZCLIENTE == cliente_sap.ZCLIENTE).Count();

            if (codigo_cliente >= 1)
            {

            }

            var retornoclientecluster = db.cliente_cluster_SAP.Where(cl => cl.ZCLIENTE == cliente_sap.ZCLIENTE);

            int[] n = new int[20];

            foreach (var item in retornoclientecluster)
            {
                n[Convert.ToInt32(item.ZCODCLUSTER)] = Convert.ToInt32(item.ZCODCLUSTER);
            }

            ViewBag.status_ativacao = new SelectList(db.ativacao, "id", "codigo", cliente_sap.status_ativacao);
            ViewBag.ZCANAL = new SelectList(db.canal_SAP, "ZCODCANAL", "ZNOMCANAL", cliente_sap.ZCANAL);
            ViewBag.id_cluster = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE", cliente_sap.id_cluster);
            ViewBag.id_colaborador = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE", cliente_sap.id_colaborador);
            ViewBag.id_executivo = new SelectList(db.executivo, "id", "nome", cliente_sap.id_executivo);
            ViewBag.id_franquia = new SelectList(db.franquia, "id", "descricao", cliente_sap.id_franquia);
            ViewBag.id_prospector = new SelectList(db.prospector, "id", "nome", cliente_sap.id_prospector);
            ViewBag.motivo_id = new SelectList(db.motivo_de_recuso_SAP, "ZCODMOTIV", "ZNOMMOTIV", cliente_sap.motivo_id);
            ViewBag.status_ativacao = new SelectList(db.status_ativacao, "id", "nome", cliente_sap.status_ativacao);
            ViewBag.ZLOCALIZA = new SelectList(db.localizacao_SAP, "ZCODLOCA", "ZNOMLOCA", cliente_sap.ZLOCALIZA);
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", cliente_sap.ZTERRITOR);
            ViewBag.ZKIT = new SelectList(db.kit_SAP, "ZCODKITS", "ZNOMKITS", cliente_sap.ZKIT);
            ViewBag.ZZAT = new SelectList(db.zat_SAP, "ZCODZATS", "ZNOMBRE", cliente_sap.ZZAT);
            ViewBag.ZUNIDAD = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE", cliente_sap.ZUNIDAD);
            return View(cliente_sap);
        }

        // GET: /Clientes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cliente_SAP cliente_sap = db.cliente_SAP.Find(id);
            if (cliente_sap == null)
            {
                return HttpNotFound();
            }
            var id2 = id.ToString();
            var retornoclientecluster = db.cliente_cluster_SAP.Where(cl => cl.ZCLIENTE == id2);

            int[] n = new int[20];

            foreach (var item in retornoclientecluster)
            {
                n[Convert.ToInt32(item.ZCODCLUSTER)] = Convert.ToInt32(item.ZCODCLUSTER);
            }

            ViewBag.status_ativacao = new SelectList(db.ativacao, "id", "codigo", cliente_sap.status_ativacao);
            ViewBag.ZCANAL = new SelectList(db.canal_SAP, "ZCODCANAL", "ZNOMCANAL", cliente_sap.ZCANAL);
            ViewBag.id_cluster = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE", cliente_sap.id_cluster);
            ViewBag.id_colaborador = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE", cliente_sap.id_colaborador);
            ViewBag.id_executivo = new SelectList(db.executivo, "id", "nome", cliente_sap.id_executivo);
            ViewBag.id_franquia = new SelectList(db.franquia, "id", "descricao", cliente_sap.id_franquia);
            ViewBag.id_prospector = new SelectList(db.prospector, "id", "nome", cliente_sap.id_prospector);
            ViewBag.motivo_id = new SelectList(db.motivo_de_recuso_SAP, "ZCODMOTIV", "ZNOMMOTIV", cliente_sap.motivo_id);
            ViewBag.status_ativacao = new SelectList(db.status_ativacao, "id", "nome", cliente_sap.status_ativacao);
            ViewBag.ZLOCALIZA = new SelectList(db.localizacao_SAP, "ZCODLOCA", "ZNOMLOCA", cliente_sap.ZLOCALIZA);
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", cliente_sap.ZTERRITOR);
            ViewBag.ZKIT = new SelectList(db.kit_SAP, "ZCODKITS", "ZNOMKITS", cliente_sap.ZKIT);
            ViewBag.ZZAT = new SelectList(db.zat_SAP, "ZCODZATS", "ZNOMBRE", cliente_sap.ZZAT);
            ViewBag.ZUNIDAD = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE", cliente_sap.ZUNIDAD);
            return View(cliente_sap);
        }

        // POST: /Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANDT,ZCLIENTE,ZNOMBRE,ZNOMFANTA,ZCNPJCPFI,ZCEP,ZDIRECCIO,ZBARRIO,ZCIUDAD,ZUF,ZPUNTOREF,ZTERRITOR,ZUNIDAD,ZTELEFONO,ZCANAL,ZGEC,ZNSE,ZRUTA,ZZAT,ZKIT,ZMARCAGDM,ZLOCALIZA,ZNOMSHOPP,ZRED,ZNOMBRERE,ZHABACTIV,ZHABRMANT,ZHABSHOPP,descricao,id_novo,id_antigo,id_cluster,executivo,prospector,desenvolvedor,fornecedor,design,zona_transporte,regional,numero,lattext,longtext,status_ativacao,id_colaborador,custo_budget,motivo_id,motivotxt,cidade_limpa,segundo_telefone,id_executivo,id_prospector,ano_cliente,acessooffline,id_franquia,recusa_foto,ativacao_ultima_atualizacao,observacao")] cliente_SAP cliente_sap, int[] id_cluster)
        {
            if (ModelState.IsValid)
            {
                db.cliente_cluster_SAP.RemoveRange(db.cliente_cluster_SAP.Where(x => x.ZCLIENTE == cliente_sap.ZCLIENTE));
                db.Entry(cliente_sap).State = EntityState.Modified;
                db.SaveChanges();

                foreach (var itemselecionado in id_cluster)
                {
                    var sqlinsert = "INSERT INTO cliente_cluster_SAP (ZCLIENTE, ZCODCLUSTER) VALUES ('" + cliente_sap.ZCLIENTE + "'," + itemselecionado + ");";

                    //execurar sql meu amigo
                    db.Database.ExecuteSqlCommand(sqlinsert);
                }
                return RedirectToAction("Index");
            }

            var retornoclientecluster = db.cliente_cluster_SAP.Where(cl => cl.ZCLIENTE == cliente_sap.ZCLIENTE);

            int[] n = new int[20];

            foreach (var item in retornoclientecluster)
            {
                n[Convert.ToInt32(item.ZCODCLUSTER)] = Convert.ToInt32(item.ZCODCLUSTER);
            }

            ViewBag.status_ativacao = new SelectList(db.ativacao, "id", "codigo", cliente_sap.status_ativacao);
            ViewBag.ZCANAL = new SelectList(db.canal_SAP, "ZCODCANAL", "ZNOMCANAL", cliente_sap.ZCANAL);
            ViewBag.id_cluster = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE", cliente_sap.id_cluster);
            ViewBag.id_colaborador = new SelectList(db.colaborador_SAP, "ZCODCOLAB", "ZNOMBRE", cliente_sap.id_colaborador);
            ViewBag.id_executivo = new SelectList(db.executivo, "id", "nome", cliente_sap.id_executivo);
            ViewBag.id_franquia = new SelectList(db.franquia, "id", "descricao", cliente_sap.id_franquia);
            ViewBag.id_prospector = new SelectList(db.prospector, "id", "nome", cliente_sap.id_prospector);
            ViewBag.motivo_id = new SelectList(db.motivo_de_recuso_SAP, "ZCODMOTIV", "ZNOMMOTIV", cliente_sap.motivo_id);
            ViewBag.status_ativacao = new SelectList(db.status_ativacao, "id", "nome", cliente_sap.status_ativacao);
            ViewBag.ZLOCALIZA = new SelectList(db.localizacao_SAP, "ZCODLOCA", "ZNOMLOCA", cliente_sap.ZLOCALIZA);
            ViewBag.ZTERRITOR = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE", cliente_sap.ZTERRITOR);
            ViewBag.ZKIT = new SelectList(db.kit_SAP, "ZCODKITS", "ZNOMKITS", cliente_sap.ZKIT);
            ViewBag.ZZAT = new SelectList(db.zat_SAP, "ZCODZATS", "ZNOMBRE", cliente_sap.ZZAT);
            ViewBag.ZUNIDAD = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE", cliente_sap.ZUNIDAD);
            return View(cliente_sap);
        }

        // GET: /Clientes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cliente_SAP cliente_sap = db.cliente_SAP.Find(id);
            if (cliente_sap == null)
            {
                return HttpNotFound();
            }
            var id2 = id.ToString();
            var retornoclientecluster = db.cliente_cluster_SAP.Where(cl => cl.ZCLIENTE == id2);

            int[] n = new int[20];

            foreach (var item in retornoclientecluster)
            {
                n[Convert.ToInt32(item.ZCODCLUSTER)] = Convert.ToInt32(item.ZCODCLUSTER);
            }

            return View(cliente_sap);
        }

        // POST: /Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            cliente_SAP cliente_sap = db.cliente_SAP.Find(id);
            db.cliente_cluster_SAP.RemoveRange(db.cliente_cluster_SAP.Where(x => x.ZCLIENTE == cliente_sap.ZCLIENTE));
            db.cliente_SAP.Remove(cliente_sap);
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

               /*     var fileName = Path.GetFileName(upload.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/Images"), fileName);
                    upload.SaveAs(path);
                */

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
                                //foreach (DataColumn col in rslista.Columns)
                                //{
                                //     sqlsample += "nome " +  row[col.ColumnName];                                    
                                //}
                            
                            var ZCLIENTE = row["ZCLIENTE"].ToString();
                            var descricao = "'" +row["descricao"]+ "'";
                            var id_novo ="'" + row["id_novo"]+ "'";
                            var id_antigo = "'" + row["id_antigo"] + "'";
                            var id_cluster = "'" + row["id_cluster"] + "'";
                            var desenvolvedor = "'" + row["desenvolvedor"] + "'";
                            var fornecedor = "'" + row["fornecedor"] + "'";
                            var design = "'" + row["design"] + "'";
                            var zona_transporte = "'" + row["zona_transporte"] + "'";
                            var regional = "'" + row["regional"] + "'";
                            var numero = "'" + row["numero"] + "'";
                            var lattext = "'" + row["lattext"] + "'";
                            var longtext = "'" + row["longtext"] + "'";
                            var status_ativacao = "'" + row["status_ativacao"] + "'";
                            var id_colaborador = "'" + row["id_colaborador"] + "'";
                            var custo_budget = "'" + row["custo_budget"] + "'";
                            var motivo_id = "'" + row["motivo_id"] + "'";
                            var motivotxt = "'" + row["motivotxt"] + "'";
                          //  var cidade_limpa = "'" + row["cidade_limpa"] + "'";
                            var segundo_telefone = "'" + row["segundo_telefone"] + "'";
                            var id_executivo = "'" + row["id_executivo"] + "'";
                            var id_prospector = "'" + row["id_prospector"] + "'";
                            var ano_cliente = "'" + row["ano_cliente"] + "'";
                         //   var acessooffline = "'" + row["acessooffline"] + "'";
                            var id_franquia = "'" + row["id_franquia"] + "'";
                            var recusa_foto = "'" + row["recusa_foto"] + "'";
                            var observacao = "'" + row["observacao"] + "'";
                            var pendente_retorno = "'" + row["pendente_retorno"] + "'";
                            var motivo_retorno = "'" + row["motivo_retorno"] + "'";
                            var carta_foto = "'" + row["carta_foto"] + "'";
                            var ZPUNTOREF = "'" + row["ZPUNTOREF"] + "'";
                            var ZTERRITOR = "'" + row["ZTERRITOR"] + "'";
                            var ZUNIDAD = "'" + row["ZUNIDAD"] + "'";
                            var ZTELEFONO = "'" + row["ZTELEFONO"] + "'";
                            var ZCANAL = "'" + row["ZCANAL"] + "'";
                            var ZGEC = "'" + row["ZGEC"] + "'";
                            var ZNSE = "'" + row["ZNSE"] + "'";
                            var ZRUTA = "'" + row["ZRUTA"] + "'";
                            var ZZAT = "'" + row["ZZAT"] + "'";
                            var ZKIT = "'" + row["ZKIT"] + "'";
                            var ZMARCAGDM = "'" + row["ZMARCAGDM"] + "'";
                            var ZLOCALIZA = "'" + row["ZLOCALIZA"] + "'";
                            var ZNOMSHOPP = "'" + row["ZNOMSHOPP"] + "'";
                            var ZRED = "'" + row["ZRED"] + "'";
                            var ZNOMBRERE = "'" + row["ZNOMBRERE"] + "'";
                            var ZHABACTIV = "'" + row["ZHABACTIV"] + "'";
                            var ZHABRMANT = "'" + row["ZHABRMANT"] + "'";
                            var ZHABSHOPP = "'" + row["ZHABSHOPP"] + "'";

                            if (ZCLIENTE != "")
                            {

                                if (ZPUNTOREF == "''")
                                {
                                    ZPUNTOREF = "NULL";
                                }
                                if (ZTERRITOR == "''")
                                {
                                    ZTERRITOR = "NULL";
                                }
                                if (ZUNIDAD == "''")
                                {
                                    ZUNIDAD = "NULL";
                                }
                                if (ZTELEFONO == "''")
                                {
                                    ZTELEFONO = "NULL";
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
                                if (ZRUTA == "''")
                                {
                                    ZRUTA = "NULL";
                                }
                                if (ZZAT == "''")
                                {
                                    ZZAT = "NULL";
                                }
                                if (ZKIT == "''")
                                {
                                    ZKIT = "NULL";
                                }
                                if (ZMARCAGDM == "''")
                                {
                                    ZMARCAGDM = "NULL";
                                }
                                if (ZLOCALIZA == "''")
                                {
                                    ZLOCALIZA = "NULL";
                                }
                                if (ZNOMSHOPP == "''")
                                {
                                    ZNOMSHOPP = "NULL";
                                }
                                if (ZRED == "''")
                                {
                                    ZRED = "NULL";
                                }
                                if (ZNOMBRERE == "''")
                                {
                                    ZNOMBRERE = "NULL";
                                }
                                if (ZHABACTIV == "''")
                                {
                                    ZHABACTIV = "NULL";
                                }
                                if (ZHABRMANT == "''")
                                {
                                    ZHABRMANT = "NULL";
                                }
                                if (ZHABSHOPP == "''")
                                {
                                    ZHABSHOPP = "NULL";
                                }
                                if (descricao == "''")
                                {
                                    descricao = "NULL";
                                }
                                if (id_novo == "''")
                                {
                                    id_novo = "NULL";
                                }
                                if (id_antigo == "''")
                                {
                                    id_antigo = "NULL";
                                }
                                if (id_cluster == "''")
                                {
                                    id_cluster = "NULL";
                                }
                                if (desenvolvedor == "''")
                                {
                                    desenvolvedor = "NULL";
                                }
                                if (fornecedor == "''")
                                {
                                    fornecedor = "NULL";
                                }
                                if (design == "''")
                                {
                                    design = "NULL";
                                }
                                if (zona_transporte == "''")
                                {
                                    zona_transporte = "NULL";
                                }
                                if (regional == "''")
                                {
                                    regional = "NULL";
                                }
                                if (numero == "''")
                                {
                                    numero = "NULL";
                                }
                                if (lattext == "''")
                                {
                                    lattext = "NULL";
                                }
                                if (longtext == "''")
                                {
                                    longtext = "NULL";
                                }
                                if (status_ativacao == "''")
                                {
                                    status_ativacao = "NULL";
                                }
                                if (id_colaborador == "''")
                                {
                                    id_colaborador = "NULL";
                                }
                                if (custo_budget == "''")
                                {
                                    custo_budget = "NULL";
                                }
                                if (motivo_id == "''")
                                {
                                    motivo_id = "NULL";
                                }
                                if (motivotxt == "''")
                                {
                                    motivotxt = "NULL";
                                }
                                /*      if (cidade_limpa == "''")
                                      {
                                          cidade_limpa = "NULL";
                                      } */
                                if (segundo_telefone == "''")
                                {
                                    segundo_telefone = "NULL";
                                }
                                if (id_executivo == "''")
                                {
                                    id_executivo = "NULL";
                                }
                                if (id_prospector == "''")
                                {
                                    id_prospector = "NULL";
                                }
                                if (ano_cliente == "''")
                                {
                                    ano_cliente = "NULL";
                                }
                                /*   if (acessooffline == "''")
                                   {
                                       acessooffline = "NULL";
                                   } */
                                if (id_franquia == "''")
                                {
                                    id_franquia = "NULL";
                                }
                                if (recusa_foto == "''")
                                {
                                    recusa_foto = "NULL";
                                }
                                if (observacao == "''")
                                {
                                    observacao = "NULL";
                                }
                                if (pendente_retorno == "''")
                                {
                                    pendente_retorno = "NULL";
                                }
                                if (motivo_retorno == "''")
                                {
                                    motivo_retorno = "NULL";
                                }
                                if (carta_foto == "''")
                                {
                                    carta_foto = "NULL";
                                }
                                
                             var checkcliente = db.cliente_SAP.Where(a => a.ZCLIENTE == ZCLIENTE).FirstOrDefault();

                             if (checkcliente == null)
                             {
                                 var sqlinsert = "INSERT INTO [dbo].[cliente_SAP] ([ZCLIENTE],[ZPUNTOREF],[ZTERRITOR],[ZUNIDAD],[ZTELEFONO],[ZCANAL],[ZGEC],[ZNSE],[ZRUTA],[ZZAT],[ZKIT],[ZMARCAGDM],[ZLOCALIZA],[ZNOMSHOPP],[ZRED],[ZNOMBRERE],[ZHABACTIV],[ZHABRMANT],[ZHABSHOPP],[descricao],[id_novo],[id_antigo],[id_cluster],[desenvolvedor],[fornecedor],[design],[zona_transporte],[regional],[numero],[lattext],[longtext],[status_ativacao],[id_colaborador],[custo_budget],[motivo_id],[motivotxt],[segundo_telefone],[id_executivo],[id_prospector],[ano_cliente],[id_franquia],[recusa_foto],[observacao],[pendente_retorno],[motivo_retorno],[carta_foto])VALUES (" + "'" + ZCLIENTE + "'" + "," + ZPUNTOREF + "," + ZTERRITOR + "," + ZUNIDAD + "," + ZTELEFONO + "," + ZCANAL + "," + ZGEC + "," + ZNSE + "," + ZRUTA + "," + ZZAT + "," + ZKIT + "," + ZMARCAGDM + "," + ZLOCALIZA + "," + ZNOMSHOPP + "," + ZRED + "," + ZNOMBRERE + "," + ZHABACTIV + "," + ZHABRMANT + "," + ZHABSHOPP + "," + descricao + "," + id_novo + "," + id_antigo + "," + id_cluster + "," + desenvolvedor + "," + fornecedor + "," + design + "," + zona_transporte + "," + regional + "," + numero + "," + lattext + "," + longtext + "," + 2 + "," + id_colaborador + "," + custo_budget + "," + motivo_id + "," + motivotxt + "," + segundo_telefone + "," + id_executivo + "," + id_prospector + "," + ano_cliente + "," + id_franquia + "," + recusa_foto + "," + observacao + "," + pendente_retorno + "," + motivo_retorno + "," + carta_foto + ")";
                                 db.Database.ExecuteSqlCommand(sqlinsert);
                                 if (id_cluster != "NULL")
                                 {
                                     var sqlinsert2 = "INSERT INTO [dbo].[cliente_cluster_SAP] ([ZCLIENTE], [ZCODCLUSTER]) VALUES (" + "'" + ZCLIENTE + "'" + "," + id_cluster + ");";
                                     db.Database.ExecuteSqlCommand(sqlinsert2);
                                 }

                             }
                             else
                             {

                                 if (checkcliente.ZPUNTOREF != null && ZPUNTOREF == "NULL")
                                 {
                                     ZPUNTOREF = "'" + checkcliente.ZPUNTOREF.ToString() + "'";
                                     if (ZPUNTOREF == "''" || ZPUNTOREF == "")
                                     {
                                         ZPUNTOREF = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZTERRITOR != null && ZTERRITOR == "NULL")
                                 {
                                     ZTERRITOR = "'" + checkcliente.ZTERRITOR.ToString() + "'";
                                     if (ZTERRITOR == "''" || ZTERRITOR == "")
                                     {
                                         ZTERRITOR = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZUNIDAD != "" && ZUNIDAD == "NULL")
                                 {
                                     ZUNIDAD = "'" + checkcliente.ZUNIDAD + "'";
                                     if (ZUNIDAD == "''" || ZUNIDAD == "")
                                     {
                                         ZUNIDAD = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZTELEFONO != "" && ZTELEFONO == "NULL")
                                 {
                                     ZTELEFONO = "'" + checkcliente.ZTELEFONO + "'";
                                     if (ZTELEFONO == "''" || ZTELEFONO == "")
                                     {
                                         ZTELEFONO = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZCANAL != "" && ZCANAL == "NULL")
                                 {
                                     ZCANAL = "'" + checkcliente.ZCANAL + "'";
                                     if (ZCANAL == "''" || ZCANAL == "")
                                     {
                                         ZCANAL = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZGEC != "" && ZGEC == "NULL")
                                 {
                                     ZGEC = "'" + checkcliente.ZGEC + "'";
                                     if (ZGEC == "''" || ZGEC == "")
                                     {
                                         ZGEC = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZNSE != "" && ZNSE == "NULL")
                                 {
                                     ZNSE = "'" + checkcliente.ZNSE + "'";
                                     if (ZNSE == "''" || ZNSE == "")
                                     {
                                         ZNSE = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZRUTA != "" && ZRUTA == "NULL")
                                 {
                                     ZRUTA = "'" + checkcliente.ZRUTA + "'";
                                     if (ZRUTA == "''" || ZRUTA == "")
                                     {
                                         ZRUTA = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZZAT != null && ZZAT == "NULL")
                                 {
                                     ZZAT = "'" + checkcliente.ZZAT.ToString() + "'";
                                     if (ZZAT == "''" || ZZAT == "")
                                     {
                                         ZZAT = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZKIT != null && ZKIT == "NULL")
                                 {
                                     ZKIT = "'" + checkcliente.ZKIT.ToString() + "'";
                                     if (ZKIT == "''" || ZKIT == "")
                                     {
                                         ZKIT = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZMARCAGDM != null && ZMARCAGDM == "NULL")
                                 {
                                     ZMARCAGDM = "'" + checkcliente.ZMARCAGDM.ToString() + "'";
                                     if (ZMARCAGDM == "''" || ZMARCAGDM == "")
                                     {
                                         ZMARCAGDM = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZLOCALIZA != null && ZLOCALIZA == "NULL")
                                 {
                                     ZLOCALIZA = "'" + checkcliente.ZLOCALIZA.ToString() + "'";
                                     if (ZLOCALIZA == "''" || ZLOCALIZA == "")
                                     {
                                         ZLOCALIZA = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZNOMSHOPP != null && ZNOMSHOPP == "NULL")
                                 {
                                     ZNOMSHOPP = "'" + checkcliente.ZNOMSHOPP.ToString() + "'";
                                     if (ZNOMSHOPP == "''" || ZNOMSHOPP == "")
                                     {
                                         ZNOMSHOPP = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZRED != "" && ZRED == "NULL")
                                 {
                                     ZRED = "'" + checkcliente.ZRED + "'";
                                     if (ZRED == "''" || ZRED == "")
                                     {
                                         ZRED = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZNOMBRERE != "" && ZNOMBRERE == "NULL")
                                 {
                                     ZNOMBRERE = "'" + checkcliente.ZNOMBRERE + "'";
                                     if (ZNOMBRERE == "''" || ZNOMBRERE == "")
                                     {
                                         ZNOMBRERE = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZHABACTIV != "" && ZHABACTIV == "NULL")
                                 {
                                     ZHABACTIV = "'" + checkcliente.ZHABACTIV + "'";
                                     if (ZHABACTIV == "''" || ZHABACTIV == "")
                                     {
                                         ZHABACTIV = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZHABRMANT != "" && ZHABRMANT == "NULL")
                                 {
                                     ZHABRMANT = "'" + checkcliente.ZHABRMANT + "'";
                                     if (ZHABRMANT == "''" || ZHABRMANT == "")
                                     {
                                         ZHABRMANT = "NULL";
                                     }
                                 }
                                 if (checkcliente.ZHABSHOPP != "" && ZHABSHOPP == "NULL")
                                 {
                                     ZHABSHOPP = "'" + checkcliente.ZHABSHOPP + "'";
                                     if (ZHABSHOPP == "''" || ZHABSHOPP == "")
                                     {
                                         ZHABSHOPP = "NULL";
                                     }
                                 }
                                 if (checkcliente.descricao != "" && descricao == "NULL")
                                 {
                                     descricao = "'" + checkcliente.descricao + "'";
                                     if (descricao == "''" || descricao == "")
                                     {
                                         descricao = "NULL";
                                     }
                                 }
                                 if (checkcliente.id_novo != "" && id_novo == "NULL")
                                 {
                                     id_novo = "'" + checkcliente.id_novo + "'";
                                     if (id_novo == "''" || id_novo == "")
                                     {
                                         id_novo = "NULL";
                                     }
                                 }
                                 if (checkcliente.id_antigo != "" && id_antigo == "NULL")
                                 {
                                     id_antigo = "'" + checkcliente.id_antigo + "'";
                                     if (id_antigo == "''" || id_antigo == "")
                                     {
                                         id_antigo = "NULL";
                                     }
                                 }
                                 if (checkcliente.id_cluster != null && id_cluster == "NULL")
                                 {
                                     id_cluster = "'" + checkcliente.id_cluster.ToString() + "'";
                                     if (id_cluster == "''" || id_cluster == "")
                                     {
                                         id_cluster = "NULL";
                                     }
                                 }
                                 if (checkcliente.desenvolvedor != "" && desenvolvedor == "NULL")
                                 {
                                     desenvolvedor = "'" + checkcliente.desenvolvedor + "'";
                                     if (desenvolvedor == "''" || desenvolvedor == "")
                                     {
                                         desenvolvedor = "NULL";
                                     }
                                 }
                                 if (checkcliente.fornecedor != "" && fornecedor == "NULL")
                                 {
                                     fornecedor = "'" + checkcliente.fornecedor + "'";
                                     if (fornecedor == "''" || fornecedor == "")
                                     {
                                         fornecedor = "NULL";
                                     }
                                 }
                                 if (checkcliente.design != "" && design == "NULL")
                                 {
                                     design = "'" + checkcliente.design + "'";
                                     if (design == "''" || design == "")
                                     {
                                         design = "NULL";
                                     }

                                 }
                                 if (checkcliente.zona_transporte != "" && zona_transporte == "NULL")
                                 {
                                     zona_transporte = "'" + checkcliente.zona_transporte + "'";
                                     if (zona_transporte == "''" || zona_transporte == "")
                                     {
                                         zona_transporte = "NULL";
                                     }
                                 }
                                 if (checkcliente.regional != "" && regional == "NULL")
                                 {
                                     regional = "'" + checkcliente.regional + "'";
                                     if (regional == "''" || regional == "")
                                     {
                                         regional = "NULL";
                                     }
                                 }
                                 if (checkcliente.numero != "" && numero == "NULL")
                                 {
                                     numero = "'" + checkcliente.numero + "'";
                                     if (numero == "''" || numero == "")
                                     {
                                         numero = "NULL";
                                     }
                                 }
                                 if (checkcliente.lattext != "" && lattext == "NULL")
                                 {
                                     lattext = "'" + checkcliente.lattext + "'";
                                     if (lattext == "''" || lattext == "")
                                     {
                                         lattext = "NULL";
                                     }
                                 }
                                 if (checkcliente.longtext != "" && longtext == "NULL")
                                 {
                                     longtext = "'" + checkcliente.longtext + "'";
                                     if (longtext == "''" || longtext == "")
                                     {
                                         longtext = "NULL";
                                     }
                                 }
                                 if (checkcliente.status_ativacao != null && status_ativacao == "NULL")
                                 {
                                     status_ativacao = "'" + checkcliente.status_ativacao.ToString() + "'";
                                     if (status_ativacao == "''" || status_ativacao == "")
                                     {
                                         status_ativacao = "NULL";
                                     }
                                 }
                                 if (checkcliente.id_colaborador != null && id_colaborador == "NULL")
                                 {
                                     id_colaborador = "'" + checkcliente.id_colaborador.ToString() + "'";
                                     if (id_colaborador == "''" || id_colaborador == "")
                                     {
                                         id_colaborador = "NULL";
                                     }
                                 }
                                 if (checkcliente.custo_budget != null && custo_budget == "NULL")
                                 {
                                     custo_budget = "'" + checkcliente.custo_budget.ToString() + "'";
                                     if (custo_budget == "''" || custo_budget == "")
                                     {
                                         custo_budget = "NULL";
                                     }
                                 }
                                 if (checkcliente.motivo_id != null && motivo_id == "NULL")
                                 {
                                     motivo_id = "'" + checkcliente.motivo_id.ToString() + "'";
                                     if (motivo_id == "''" || motivo_id == "")
                                     {
                                         motivo_id = "NULL";
                                     }
                                 }
                                 if (checkcliente.motivotxt != "" && motivotxt == "NULL")
                                 {
                                     motivotxt = "'" + checkcliente.motivotxt + "'";
                                     if (motivotxt == "''" || motivotxt == "")
                                     {
                                         motivotxt = "NULL";
                                     }
                                 }
                                 if (checkcliente.segundo_telefone != "" && segundo_telefone == "NULL")
                                 {
                                     segundo_telefone = "'" + checkcliente.segundo_telefone + "'";
                                     if (segundo_telefone == "''" || segundo_telefone == "")
                                     {
                                         segundo_telefone = "NULL";
                                     }
                                 }
                                 if (checkcliente.id_executivo != null && id_executivo == "NULL")
                                 {
                                     id_executivo = "'" + checkcliente.id_executivo.ToString() + "'";
                                     if (id_executivo == "''" || id_executivo == "")
                                     {
                                         id_executivo = "NULL";
                                     }
                                 }
                                 if (checkcliente.id_prospector != null && id_prospector == "NULL")
                                 {
                                     id_prospector = "'" + checkcliente.id_prospector.ToString() + "'";
                                     if (id_prospector == "''" || id_prospector == "")
                                     {
                                         id_prospector = "NULL";
                                     }
                                 }
                                 if (checkcliente.ano_cliente != "" && ano_cliente == "NULL")
                                 {
                                     ano_cliente = "'" + checkcliente.ano_cliente + "'";
                                     if (ano_cliente == "''" || ano_cliente == "")
                                     {
                                         ano_cliente = "NULL";
                                     }
                                 }
                                 if (checkcliente.id_franquia != null && id_franquia == "NULL")
                                 {
                                     id_franquia = "'" + checkcliente.id_franquia.ToString() + "'";
                                     if (id_franquia == "''" || id_franquia == "")
                                     {
                                         id_franquia = "NULL";
                                     }
                                 }
                                 if (checkcliente.recusa_foto != "" && recusa_foto == "NULL")
                                 {
                                     recusa_foto = "'" + checkcliente.recusa_foto + "'";
                                     if (recusa_foto == "''" || recusa_foto == "")
                                     {
                                         recusa_foto = "NULL";
                                     }
                                 }
                                 if (checkcliente.observacao != "" && observacao == "NULL")
                                 {
                                     observacao = "'" + checkcliente.observacao + "'";
                                     if (observacao == "''" || observacao == "")
                                     {
                                         observacao = "NULL";
                                     }
                                 }
                                 if (checkcliente.pendente_retorno != null && pendente_retorno == "NULL")
                                 {
                                     pendente_retorno = "'" + checkcliente.pendente_retorno.ToString() + "'";
                                     if (pendente_retorno == "''" || pendente_retorno == "")
                                     {
                                         pendente_retorno = "NULL";
                                     }
                                 }
                                 if (checkcliente.motivo_retorno != "" && motivo_retorno == "NULL")
                                 {
                                     motivo_retorno = "'" + checkcliente.motivo_retorno + "'";
                                     if (motivo_retorno == "''" || motivo_retorno == "")
                                     {
                                         motivo_retorno = "NULL";
                                     }
                                 }
                                 if (checkcliente.carta_foto != "" && carta_foto == "NULL")
                                 {
                                     carta_foto = "'" + checkcliente.carta_foto + "'";
                                     if (carta_foto == "''" || carta_foto == "")
                                     {
                                         carta_foto = "NULL";
                                     }
                                 }

                                 var sqlinsert = "UPDATE [dbo].[cliente_SAP] SET" + "[ZPUNTOREF] = " + ZPUNTOREF + ", [ZTERRITOR] = " + ZTERRITOR + ", [ZUNIDAD] = " + ZUNIDAD + ", [ZTELEFONO] = " + ZTELEFONO + ", [ZCANAL] = " + ZCANAL + ", [ZGEC] = " + ZGEC + ", [ZNSE] = " + ZNSE + ", [ZRUTA] = " + ZRUTA + ", [ZZAT] = " + ZZAT + ", [ZKIT] = " + ZKIT + ", [ZMARCAGDM] = " + ZMARCAGDM + ", [ZLOCALIZA] = " + ZLOCALIZA + ", [ZNOMSHOPP] = " + ZNOMSHOPP + ", [ZRED] = " + ZRED + ", [ZNOMBRERE] = " + ZNOMBRERE + ", [ZHABACTIV] = " + ZHABACTIV + ", [ZHABRMANT] = " + ZHABRMANT + ", [ZHABSHOPP] = " + ZHABSHOPP + ", [descricao] = " + descricao + ", [id_novo] = " + id_novo + ", [id_antigo] = " + id_antigo + ", [id_cluster] = " + id_cluster + ", [desenvolvedor] = " + desenvolvedor + ", [fornecedor] = " + fornecedor + ", [design] = " + design + ", [zona_transporte] = " + zona_transporte + ", [regional] = " + regional + ", [numero] = " + numero + ", [lattext] = " + lattext + ", [longtext] = " + longtext + ", [status_ativacao] = " + status_ativacao + ", [id_colaborador] = " + id_colaborador + ", [custo_budget] = " + custo_budget + ", [motivo_id] = " + motivo_id + ", [motivotxt] = " + motivotxt + ", [segundo_telefone] = " + segundo_telefone + ", [id_executivo] = " + id_executivo + ", [id_prospector] = " + id_prospector + ", [ano_cliente] = " + ano_cliente + ", [id_franquia] = " + id_franquia + ", [recusa_foto] = " + recusa_foto + ", [observacao] = " + observacao + ", [pendente_retorno] = " + pendente_retorno + ", [motivo_retorno] = " + motivo_retorno + ", [carta_foto] = " + carta_foto + " WHERE ([ZCLIENTE] = " + "'" + ZCLIENTE + "'" + ")";
                                 db.Database.ExecuteSqlCommand(sqlinsert);
                                 if (id_cluster != "NULL")
                                 {
                                     var sqldelete = "DELETE cliente_cluster_SAP WHERE ZCLIENTE =" + "'" + ZCLIENTE + "' AND ZCODCLUSTER = " + id_cluster + "";
                                     db.Database.ExecuteSqlCommand(sqldelete);
                                     var sqlinsert2 = "INSERT INTO [dbo].[cliente_cluster_SAP] ([ZCLIENTE], [ZCODCLUSTER]) VALUES (" + "'" + ZCLIENTE + "'" + "," + id_cluster + ");";
                                     db.Database.ExecuteSqlCommand(sqlinsert2);
                                 }
                             }
                           }
                        }
                    } 
                }
                return RedirectToAction("Index");
        }
    }
}
