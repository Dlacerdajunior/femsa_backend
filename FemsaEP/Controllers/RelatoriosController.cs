using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using FemsaEP.Models;
using System.Data.Entity;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Data.SqlClient;


namespace FemsaEP.Controllers
{
    public class RelatoriosController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /RelatorioAtivacao        
        public ActionResult RelatorioAtivacao()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.id_prospector = new SelectList(db.prospector, "id", "nome");
            ViewBag.id_executivo = new SelectList(db.executivo, "id", "nome");
            ViewBag.id_unidade = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE");
            ViewBag.id_cliente = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE");
            ViewBag.id_peca = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA");
            ViewBag.id_cluster = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE");
            ViewBag.id_status = new SelectList(db.status_ativacao, "id", "nome");
            ViewBag.id_franquia = new SelectList(db.franquia, "id", "descricao");
            ViewBag.id_projeto = new SelectList(db.projeto, "id", "descricao");
            ViewBag.id_territorio = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE");
            ViewBag.id_matricula = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZCLIENTE");
            return View();
        }

        //POST: /RelatorioAtivacao
        [HttpPost]
        public ActionResult RelatorioAtivacao(int? id)
        {
            var anocliente = Request.Form["anocliente"];

            var id_cliente = 0;

            if (Request.Form["id_cliente"] != "")
            {
                id_cliente = Convert.ToInt32(Request.Form["id_cliente"]);
            }

            var id_matricula = "";

            if (Request.Form["id_matricula"] != "")
            {
                id_matricula = Request.Form["id_matricula"];
            }

            var id_executivo = 0;

            if (Request.Form["id_executivo"] != "")
            {
                id_executivo = Convert.ToInt32(Request.Form["id_executivo"]);
            }

            var id_prospector = 0;

            if (Request.Form["id_prospector"] != "")
            {
                id_prospector = Convert.ToInt32(Request.Form["id_prospector"]);
            }

            var id_peca = 0;

            if (Request.Form["id_peca"] != "")
            {
                id_peca = Convert.ToInt32(Request.Form["id_peca"]);
            }

            var id_cluster = 0;

            if (Request.Form["id_cluster"] != "")
            {
                id_cluster = Convert.ToInt32(Request.Form["id_cluster"]);
            }

            var id_projeto = 0;

            if (Request.Form["id_projeto"] != "")
            {
                id_projeto = Convert.ToInt32(Request.Form["id_projeto"]);
            }

            var id_territorio = 0;

            if (Request.Form["id_territorio"] != "")
            {
                id_territorio = Convert.ToInt32(Request.Form["id_territorio"]);
            }

            var id_status = Request.Form["id_status"];
            var datainicial = Request.Form["datainicial"];
            var datafinal = Request.Form["datafinal"];

            string[] arrDate = datainicial.Split('/');

            string day = arrDate[0].ToString();
            string month = arrDate[1].ToString();
            string year = arrDate[2].ToString();

            string[] arrDate2 = datafinal.Split('/');

            string day2 = arrDate2[0].ToString();
            string month2 = arrDate2[1].ToString();
            string year2 = arrDate2[2].ToString();


            //INNER JOIN ativacao_peca ON ativacao.id = ativacao_peca.id_ativacao 
            var sql = "SELECT DISTINCT ativacao.* FROM ativacao INNER JOIN cliente_SAP ON ativacao.ZCLIENTE = cliente_SAP.ZCLIENTE INNER JOIN cliente_cluster_SAP ON cliente_SAP.ZCLIENTE = cliente_cluster_SAP.ZCLIENTE WHERE 1=1";

            if (id_cliente > 0)
            {
                sql = sql + " AND ativacao.ZCLIENTE = " + id_cliente;
            }

            if (id_projeto > 0)
            {
                sql = sql + " AND ativacao.id_projeto = " + id_projeto;
            }

            if (id_territorio > 0)
            {
                sql = sql + " AND cliente_SAP.ZTERRITOR = " + id_territorio;
            }

            if (id_prospector > 0)
            {
                sql = sql + " AND cliente_SAP.id_prospector = " + id_prospector;
            }
            if (id_executivo > 0)
            {
                sql = sql + " AND cliente_SAP.id_executivo = " + id_executivo;
            }
            if (id_matricula != "")
            {
                sql = sql + " AND cliente_SAP.ZCLIENTE = " + id_matricula;
            }
            if (id_cluster > 0)
            {
                sql = sql + " AND cliente_cluster_SAP.ZCODCLUSTER = " + id_cluster;
            }
            if (id_peca > 0)
            {
                sql = sql + " AND ativacao_peca.ZCODPECA = " + id_peca;
            }
            if (anocliente != null && anocliente != "")
            {
                sql = sql + " AND cliente_SAP.ano_cliente = '" + anocliente + "'";
            }
            if (id_status != null)
            {
                if (id_status.Count() == 1)
                {
                    foreach (var itemselecionado in id_status)
                    {
                        if (itemselecionado != ',')
                        {
                            sql = sql + " AND ativacao.status = " + itemselecionado;
                        }
                    }
                }
                else
                {
                    sql = sql + " AND (";

                    int numerocount = 1;

                    foreach (var itemselecionado in id_status)
                    {
                        if (numerocount == 1)
                        {

                            if (itemselecionado != ',')
                            {
                                sql = sql + " ativacao.status = " + itemselecionado;
                            }
                        }
                        else
                        {
                            if (itemselecionado != ',')
                            {
                                sql = sql + " OR ativacao.status = " + itemselecionado;
                            }
                        }

                        numerocount = numerocount + 1;
                    }

                    sql = sql + ")";
                }

            }
            if (datainicial != null || datafinal != null)
            {
                sql = sql + " AND ativacao.criacao BETWEEN  '" + year + "-" + month + "-" + day + "' AND '" + year2 + "-" + month2 + "-" + day2 + "'";
            }

            //sql = sql + " GROUP BY ativacao.id " ;

            DataTable dt = new DataTable();

            dt.Columns.Add("Matrícula_Cliente", typeof(string));
            dt.Columns.Add("Razão_Social", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("UF", typeof(string));
            dt.Columns.Add("Cidade", typeof(string));
            dt.Columns.Add("Franquia", typeof(string));
            dt.Columns.Add("Unidade", typeof(string));
          //dt.Columns.Add("Rota", typeof(string)); 
            dt.Columns.Add("Gec", typeof(string));
            dt.Columns.Add("Subcanal", typeof(string));
            dt.Columns.Add("Cluster", typeof(string));
          //dt.Columns.Add("Executivo", typeof(string));
            dt.Columns.Add("Prospector", typeof(string));
            dt.Columns.Add("Zat", typeof(string));
            dt.Columns.Add("Código_Ativação", typeof(Int32));
            dt.Columns.Add("Ano_Cliente", typeof(string));

          //dt.Columns.Add("Zat_Código", typeof(string));
           


            var itemAtivacao = db.ativacao.SqlQuery(sql).ToList();

            foreach (var item in itemAtivacao)
            {
                var razao_social = "";
                if (item.cliente_SAP.ZNOMBRE != null)
                {
                    razao_social = item.cliente_SAP.ZNOMBRE.ToString();
                }

                var franquia = "";
                if (item.cliente_SAP.ZTERRITOR != null)
                {
                    franquia = item.cliente_SAP.territorio_SAP.ZNOMBRE.ToString();
                } 
                
                var subcanal = "";
                if (item.cliente_SAP.ZCANAL != null)
                {
                    subcanal = item.cliente_SAP.canal_SAP.ZNOMCANAL.ToString();
                }

                var status_ativacao = "";
                if (item.status_ativacao != null)
                {
                    status_ativacao = item.status_ativacao.nome.ToString();
                }

                var uf_sigla = "";
                if (item.cliente_SAP.ZUF != null)
                {
                    uf_sigla = item.cliente_SAP.ZUF.ToString();
                }

                var cidade = "";
                if (item.cliente_SAP.ZCIUDAD != null)
                {
                    cidade = item.cliente_SAP.ZCIUDAD.ToString();
                }

                var id_unidade = "";
                if (item.cliente_SAP.ZUNIDAD != null)
                {
                    id_unidade = item.cliente_SAP.unidade_SAP.ZNOMBRE.ToString();
                }

                /*     var id_rota = "";
                     if (item.cliente_SAP.rota_SAP != null)
                     {
                         id_rota = item.cliente_SAP.rota.descricao.ToString();
                     } */

                var gec = "";

                if (item.cliente_SAP.ZGEC != "" && item.cliente_SAP.ZGEC != null)
                {
               /*     var tableName = "gec_SAP";
                    var sql3 = "select * from gec_SAP Where gec_SAP.ZCODGEC ='" + item.cliente_SAP.ZGEC + "'";
                 //   var itemgec = db.Database.SqlQuery(sql).ToList();
                    var results = db.Database.SqlQuery<string>(sql3, tableName).ToArray(); */

                    DataTable data = GetDataFromQuery("select * from gec_SAP Where gec_SAP.ZCODGEC ='" + item.cliente_SAP.ZGEC.Replace(" ", "") + "'");


                    string name = "";
                    foreach (DataRow row in data.Rows)
                    {
                       name = row["ZNOMGEC"].ToString();
                    }

                    gec = name;
                   // gec = item.cliente_SAP.ZGEC.ToString();
                }

                var executivo = "";
                if (item.cliente_SAP.executivo != null)
                {
                    executivo = item.cliente_SAP.executivo.ToString();
                }

                var prospector = "";
                if (item.cliente_SAP.prospector1.nome != null)
                {
                    prospector = item.cliente_SAP.prospector1.nome.ToString();
                }

                var id_zat = "";
                if (item.cliente_SAP.zat_SAP != null)
                {
                    id_zat = item.cliente_SAP.zat_SAP.ZNOMBRE.ToString();
                }

                var ano_cliente = "";
                if (item.cliente_SAP.ano_cliente != null)
                {
                    ano_cliente = item.cliente_SAP.ano_cliente.ToString();
                }

                var id_zat_codigo = "";
                if (item.cliente_SAP.zat_SAP != null)
                {
                    id_zat_codigo = item.cliente_SAP.zat_SAP.ZCODZATS.ToString();
                }

                var cliente_codigo = "";
                if (item.cliente_SAP.ZCLIENTE != null)
                {
                    cliente_codigo = item.cliente_SAP.ZCLIENTE.ToString();
                }

                var clientecluster = "";
                var checkcc = db.cliente_cluster_SAP.Where(a => a.ZCLIENTE == item.cliente_SAP.ZCLIENTE).ToList();
                if (checkcc != null)
                {
                    int i = 0;
                    var virgula = "";
                    foreach (var item9 in checkcc)
                    {
                        if (i == checkcc.Count)
                        {
                            virgula = "";
                        }
                        else
                        {
                            virgula = ",";
                        }
                        clientecluster = clientecluster + item9.cluster_SAP.ZNOMBRE.Replace(" ","") + virgula;
                        i++;
                    }                 
                } 

                dt.Rows.Add(
                    cliente_codigo,
                    razao_social,
                    status_ativacao,
                    uf_sigla,
                    cidade,
                    franquia,
                    id_unidade,
                    /* id_rota, */
                    gec,
                    subcanal,
                    clientecluster,
                  //  executivo,
                    prospector,
                    id_zat,
                    item.id,
                    ano_cliente
                    /*id_zat_codigo*/);
            }

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", "attachment;filename=relatorios.xls");
            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteExcelWithNPOI(dt, "xlsx", 1);
            //Response.End();

            return View("Relatorios/RelatorioAtivacao"); //left join ou right join
        }

        DataTable GetDataFromQuery(string query)
        {
            SqlDataAdapter adap =
                 new SqlDataAdapter(query, "Data Source=10.137.64.75;Initial Catalog=DEVEXECPOS;User ID=proatsql;Password=QWEdsa#1;Trusted_Connection=False;");
            DataTable data = new DataTable();
            adap.Fill(data);
            return data;
        } 

        // GET: /PecasCliente        
        public ActionResult PecasCliente()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.id_prospector = new SelectList(db.prospector, "id", "nome");
            ViewBag.id_executivo = new SelectList(db.executivo, "id", "nome");
            ViewBag.id_unidade = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE");
            ViewBag.id_cliente = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE");
            ViewBag.id_peca = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA");
            ViewBag.id_cluster = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE");
            ViewBag.id_status = new SelectList(db.status_ativacao, "id", "nome");
            ViewBag.id_franquia = new SelectList(db.franquia, "id", "descricao");
            ViewBag.id_projeto = new SelectList(db.projeto, "id", "descricao");

            return View();
        }

        //POST: /PecasCliente
        [HttpPost]
        public ActionResult PecasCliente(int? id)
        {
            var anocliente = Request.Form["anocliente"];

            var id_cliente = 0;

            if (Request.Form["id_cliente"] != "")
            {
                id_cliente = Convert.ToInt32(Request.Form["id_cliente"]);
            }

            var id_projeto = 0;

            if (Request.Form["id_projeto"] != "")
            {
                id_projeto = Convert.ToInt32(Request.Form["id_projeto"]);
            }

            var id_executivo = 0;

            if (Request.Form["id_executivo"] != "")
            {
                id_executivo = Convert.ToInt32(Request.Form["id_executivo"]);
            }

            var id_prospector = 0;

            if (Request.Form["id_prospector"] != "")
            {
                id_prospector = Convert.ToInt32(Request.Form["id_prospector"]);
            }

            var id_peca = 0;

            if (Request.Form["id_peca"] != "")
            {
                id_peca = Convert.ToInt32(Request.Form["id_peca"]);
            }

            var id_cluster = 0;

            if (Request.Form["id_cluster"] != "")
            {
                id_cluster = Convert.ToInt32(Request.Form["id_cluster"]);
            }

            var id_status = Request.Form["id_status"];
            var datainicial = Request.Form["datainicial"];
            var datafinal = Request.Form["datafinal"];

            string[] arrDate = datainicial.Split('/');

            string day = arrDate[0].ToString();
            string month = arrDate[1].ToString();
            string year = arrDate[2].ToString();

            string[] arrDate2 = datafinal.Split('/');

            string day2 = arrDate2[0].ToString();
            string month2 = arrDate2[1].ToString();
            string year2 = arrDate2[2].ToString();

            //INNER JOIN ativacao_peca ON ativacao.id = ativacao_peca.id_ativacao 
            var sql = "SELECT DISTINCT ativacao.* FROM ativacao INNER JOIN cliente_SAP ON ativacao.ZCLIENTE = cliente_SAP.ZCLIENTE INNER JOIN cliente_cluster_SAP ON cliente_SAP.ZCLIENTE = cliente_cluster_SAP.ZCLIENTE WHERE 1=1";

            if (id_cliente > 0)
            {
                sql = sql + " AND ativacao.ZCLIENTE = " + id_cliente;
            }

            if (id_projeto > 0)
            {
                sql = sql + " AND ativacao.id_projeto = " + id_projeto;
            }

            if (id_prospector > 0)
            {
                sql = sql + " AND cliente_SAP.id_prospector = " + id_prospector;
            }
            if (id_executivo > 0)
            {
                sql = sql + " AND cliente_SAP.id_executivo = " + id_executivo;
            }
            if (id_cluster > 0)
            {
                sql = sql + " AND cliente_cluster_SAP.ZCODCLUSTER = " + id_cluster;
            }
            if (id_peca > 0)
            {
                sql = sql + " AND ativacao_peca.ZCODPECA = " + id_peca;
            }
            if (anocliente != null && anocliente != "")
            {
                sql = sql + " AND cliente_SAP.ano_cliente = '" + anocliente + "'";
            }
            if (id_status != null)
            {
                if (id_status.Count() == 1)
                {
                    foreach (var itemselecionado in id_status)
                    {
                        if (itemselecionado != ',')
                        {
                            sql = sql + " AND ativacao.status = " + itemselecionado;
                        }
                    }
                }
                else
                {
                    sql = sql + " AND (";

                    int numerocount = 1;

                    foreach (var itemselecionado in id_status)
                    {
                        if (numerocount == 1)
                        {

                            if (itemselecionado != ',')
                            {
                                sql = sql + " ativacao.status = " + itemselecionado;
                            }
                        }
                        else
                        {
                            if (itemselecionado != ',')
                            {
                                sql = sql + " OR ativacao.status = " + itemselecionado;
                            }
                        }

                        numerocount = numerocount + 1;
                    }

                    sql = sql + ")";
                }

            }
            if (datainicial != null || datafinal != null)
            {
                sql = sql + " AND ativacao.criacao BETWEEN  '" + year + "-" + month + "-" + day + "' AND '" + year2 + "-" + month2 + "-" + day2 + "'";
            }

            //sql = sql + " GROUP BY ativacao.id " ;

            DataTable dt = new DataTable();

            dt.Columns.Add("Matrícula Cliente", typeof(string));
            dt.Columns.Add("Razão Social", typeof(string));
            dt.Columns.Add("Código Peça", typeof(string));
            dt.Columns.Add("Nome Peça", typeof(string));
            dt.Columns.Add("Ano", typeof(string));
            dt.Columns.Add("Data da Ativação", typeof(string));
            dt.Columns.Add("Cluster", typeof(string));

            var itemAtivacao = db.ativacao.SqlQuery(sql).ToList();

            foreach (var item in itemAtivacao)
            {
                var c = db.ativacao_peca.Where(a => a.id_ativacao == item.id);
                //var ccheck = db.ativacao_peca.Where(a => a.id_ativacao == item.id).Count();

                if (c.Count() == 0)
                {
                    var codigo_peca = "Sem peça relacionada para esta ativação";
                    var nome_peca = "Sem peça relacionada para esta ativação";

                    var cliente_codigo = "";
                    if (item.cliente_SAP.ZCLIENTE != null)
                    {
                        cliente_codigo = item.cliente_SAP.ZCLIENTE.ToString();
                    }

                    var razao_social = "";
                    if (item.cliente_SAP.ZNOMBRE != null)
                    {
                        razao_social = item.cliente_SAP.ZNOMBRE.ToString();
                    }

                    var ano_cliente = "";
                    if (item.cliente_SAP.ano_cliente != null)
                    {
                        ano_cliente = item.cliente_SAP.ano_cliente.ToString();
                    }

                    var itemcluster = db.cliente_cluster_SAP.Where(a => a.ZCLIENTE == item.ZCLIENTE).FirstOrDefault();
                    var cluster = "";
                    if (itemcluster.cluster_SAP.ZNOMBRE != null)
                    {
                        cluster = itemcluster.cluster_SAP.ZNOMBRE.ToString();
                    }

                    /* var codigo_peca = "";
                     if (item.ativacao_peca != null)
                     {
                         codigo_peca = item.ativacao_peca.ToString();
                     }
                     */
                    /*    var nome_peca = "";
                        if (item.ativacao_peca != null)
                        {
                            nome_peca = item.ativacao_peca.ToString();
                        } */

                    var data_ativacao = "";
                    if (item.criacao != null)
                    {
                        data_ativacao = item.criacao.ToString();
                    }


                    dt.Rows.Add(
                        cliente_codigo,
                        razao_social,
                        codigo_peca,
                        nome_peca,
                        ano_cliente,
                        data_ativacao,
                        cluster);
                }
                else
                {
                    foreach (var item2 in c)
                    {
                        var codigo_peca = item2.peca_SAP.ZCODPECA;
                        var nome_peca = item2.peca_SAP.ZNOMPECA;

                        var cliente_codigo = "";
                        if (item.cliente_SAP.ZCLIENTE != null)
                        {
                            cliente_codigo = item.cliente_SAP.ZCLIENTE.ToString();
                        }

                        var razao_social = "";
                        if (item.cliente_SAP.ZNOMBRE != null)
                        {
                            razao_social = item.cliente_SAP.ZNOMBRE.ToString();
                        }

                        var ano_cliente = "";
                        if (item.cliente_SAP.ano_cliente != null)
                        {
                            ano_cliente = item.cliente_SAP.ano_cliente.ToString();
                        }

                        var itemcluster = db.cliente_cluster_SAP.Where(a => a.ZCLIENTE == item.ZCLIENTE).FirstOrDefault();
                        var cluster = "";
                        if (itemcluster.cluster_SAP.ZNOMBRE != null)
                        {
                            cluster = itemcluster.cluster_SAP.ZNOMBRE.ToString();
                        }

                        /* var codigo_peca = "";
                         if (item.ativacao_peca != null)
                         {
                             codigo_peca = item.ativacao_peca.ToString();
                         }
                         */
                        /*    var nome_peca = "";
                            if (item.ativacao_peca != null)
                            {
                                nome_peca = item.ativacao_peca.ToString();
                            } */

                        var data_ativacao = "";
                        if (item.criacao != null)
                        {
                            data_ativacao = item.criacao.ToString();
                        }


                        dt.Rows.Add(
                            cliente_codigo,
                            razao_social,
                            codigo_peca,
                            nome_peca,
                            ano_cliente,
                            data_ativacao,
                            cluster);
                    }
                }


            }

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", "attachment;filename=relatorios.xls");
            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteExcelWithNPOI(dt, "xlsx", 2);
            //Response.End();

            return View("Relatorios/PecasCliente"); //left join ou right join
        }


        // GET: /MonitoramentoRevisitas        
        public ActionResult MonitoramentoRevisitas()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.id_prospector = new SelectList(db.prospector, "id", "nome");
            ViewBag.id_executivo = new SelectList(db.executivo, "id", "nome");
            ViewBag.id_unidade = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE");
            ViewBag.id_cliente = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE");
            ViewBag.id_peca = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA");
            ViewBag.id_cluster = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE");
            ViewBag.id_status = new SelectList(db.status_ativacao, "id", "nome");
            ViewBag.id_franquia = new SelectList(db.franquia, "id", "descricao");
            ViewBag.id_projeto = new SelectList(db.projeto, "id", "descricao");

            return View();
        }

        //POST: /MonitoramentoRevisitas
        [HttpPost]
        public ActionResult MonitoramentoRevisitas(int? id)
        {
            var anocliente = Request.Form["anocliente"];

            var id_cliente = 0;

            if (Request.Form["id_cliente"] != "")
            {
                id_cliente = Convert.ToInt32(Request.Form["id_cliente"]);
            }

            var id_executivo = 0;

            if (Request.Form["id_executivo"] != "")
            {
                id_executivo = Convert.ToInt32(Request.Form["id_executivo"]);
            }

            var id_projeto = 0;

            if (Request.Form["id_projeto"] != "")
            {
                id_projeto = Convert.ToInt32(Request.Form["id_projeto"]);
            }

            var id_prospector = 0;

            if (Request.Form["id_prospector"] != "")
            {
                id_prospector = Convert.ToInt32(Request.Form["id_prospector"]);
            }

            var id_peca = 0;

            if (Request.Form["id_peca"] != "")
            {
                id_peca = Convert.ToInt32(Request.Form["id_peca"]);
            }

            var id_cluster = 0;

            if (Request.Form["id_cluster"] != "")
            {
                id_cluster = Convert.ToInt32(Request.Form["id_cluster"]);
            }

            var id_status = Request.Form["id_status"];
            var datainicial = Request.Form["datainicial"];
            var datafinal = Request.Form["datafinal"];

            string[] arrDate = datainicial.Split('/');

            string day = arrDate[0].ToString();
            string month = arrDate[1].ToString();
            string year = arrDate[2].ToString();

            string[] arrDate2 = datafinal.Split('/');

            string day2 = arrDate2[0].ToString();
            string month2 = arrDate2[1].ToString();
            string year2 = arrDate2[2].ToString();


            var sql = "SELECT DISTINCT ativacao.* FROM ativacao INNER JOIN cliente_SAP ON ativacao.ZCLIENTE = cliente_SAP.ZCLIENTE INNER JOIN ativacao_foto ON ativacao.id = ativacao_foto.id_ativacao INNER JOIN cliente_cluster_SAP ON cliente_SAP.ZCLIENTE = cliente_cluster_SAP.ZCLIENTE WHERE 1=1";

            if (id_cliente > 0)
            {
                sql = sql + " AND ativacao.ZCLIENTE = " + id_cliente;
            }

            if (id_projeto > 0)
            {
                sql = sql + " AND ativacao.id_projeto = " + id_projeto;
            }

            if (id_prospector > 0)
            {
                sql = sql + " AND cliente_SAP.id_prospector = " + id_prospector;
            }
            if (id_executivo > 0)
            {
                sql = sql + " AND cliente_SAP.id_executivo = " + id_executivo;
            }
            if (id_cluster > 0)
            {
                sql = sql + " AND cliente_cluster_SAP.ZCODCLUST = " + id_cluster;
            }
            if (anocliente != null && anocliente != "")
            {
                sql = sql + " AND cliente_SAP.ano_cliente = '" + anocliente + "'";
            }
            if (id_status != null)
            {
                if (id_status.Count() == 1)
                {
                    foreach (var itemselecionado in id_status)
                    {
                        if (itemselecionado != ',')
                        {
                            sql = sql + " AND ativacao.status = " + itemselecionado;
                        }
                    }
                }
                else
                {
                    sql = sql + " AND (";

                    int numerocount = 1;

                    foreach (var itemselecionado in id_status)
                    {
                        if (numerocount == 1)
                        {

                            if (itemselecionado != ',')
                            {
                                sql = sql + " ativacao.status = " + itemselecionado;
                            }
                        }
                        else
                        {
                            if (itemselecionado != ',')
                            {
                                sql = sql + " OR ativacao.status = " + itemselecionado;
                            }
                        }

                        numerocount = numerocount + 1;
                    }

                    sql = sql + ")";
                }

            }
            if (datainicial != null || datafinal != null)
            {
                sql = sql + " AND ativacao.criacao BETWEEN  '" + year + "-" + month + "-" + day + "' AND '" + year2 + "-" + month2 + "-" + day2 + "'";
            }

            //sql = sql + " GROUP BY ativacao.id " ;

            DataTable dt = new DataTable();

            dt.Columns.Add("Matrícula Cliente", typeof(string));
            dt.Columns.Add("Razão Social", typeof(string));
            dt.Columns.Add("UF", typeof(string));
            dt.Columns.Add("Cidade", typeof(string));
            dt.Columns.Add("Franquia", typeof(string));
            dt.Columns.Add("Unidade", typeof(string));
            /*  dt.Columns.Add("Rota", typeof(string)); */
            dt.Columns.Add("Prospector", typeof(string));
            dt.Columns.Add("Cluster", typeof(string));
            dt.Columns.Add("Visita 1", typeof(string));
            dt.Columns.Add("Projeto", typeof(string));
            dt.Columns.Add("Etapa", typeof(string));
            dt.Columns.Add("Dias para Revisita 2", typeof(string));
            dt.Columns.Add("Revisita 2", typeof(string));
            dt.Columns.Add("Revisita Executivo 2", typeof(string));
            dt.Columns.Add("Dias para Revisita 3", typeof(string));
            dt.Columns.Add("Revisita 3", typeof(string));
            dt.Columns.Add("Revisita Executivo 3", typeof(string));
            dt.Columns.Add("Dias para Revisita 4", typeof(string));
            dt.Columns.Add("Revisita 4", typeof(string));
            dt.Columns.Add("Revisita Executivo 4", typeof(string));
            dt.Columns.Add("Dias para Revisita 5", typeof(string));
            dt.Columns.Add("Revisita 5", typeof(string));
            dt.Columns.Add("Revisita Executivo 5", typeof(string));
            dt.Columns.Add("Dias para Revisita 6", typeof(string));
            dt.Columns.Add("Revisita 6", typeof(string));
            dt.Columns.Add("Revisita Executivo 6", typeof(string));
            dt.Columns.Add("Dias para Revisita 7", typeof(string));
            dt.Columns.Add("Revisita 7", typeof(string));
            dt.Columns.Add("Revisita Executivo 7", typeof(string));
            dt.Columns.Add("Dias para Revisita 8", typeof(string));
            dt.Columns.Add("Revisita 8", typeof(string));
            dt.Columns.Add("Revisita Executivo 8", typeof(string));
            dt.Columns.Add("Dias para Revisita 9", typeof(string));
            dt.Columns.Add("Revisita 9", typeof(string));
            dt.Columns.Add("Revisita Executivo 9", typeof(string));
            dt.Columns.Add("Dias para Revisita 10", typeof(string));
            dt.Columns.Add("Revisita 10", typeof(string));
            dt.Columns.Add("Revisita Executivo 10", typeof(string));
            dt.Columns.Add("Dias para Revisita 11", typeof(string));
            dt.Columns.Add("Revisita 11", typeof(string));
            dt.Columns.Add("Revisita Executivo 11", typeof(string));
            dt.Columns.Add("Dias para Revisita 12", typeof(string));
            dt.Columns.Add("Revisita 12", typeof(string));
            dt.Columns.Add("Revisita Executivo 12", typeof(string));
            dt.Columns.Add("Data de Finalização", typeof(string));


            var itemAtivacao = db.ativacao.SqlQuery(sql).ToList();

            foreach (var item in itemAtivacao)
            {
                var rsvisita1 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 1).FirstOrDefault();
                var visita1 = "";

                if (rsvisita1 != null)
                {
                    visita1 = rsvisita1.criacao.ToString();
                }

                var rsvisita2 = db.ativacao_foto.Where(b => b.id_ativacao == item.id && b.etapa == 2).FirstOrDefault();
                var visita2 = "";

                if (rsvisita2 != null)
                {
                    visita2 = rsvisita2.criacao.ToString();
                }

                var rsvisita3 = db.ativacao_foto.Where(c => c.id_ativacao == item.id && c.etapa == 3).FirstOrDefault();
                var visita3 = "";

                if (rsvisita3 != null)
                {
                    visita3 = rsvisita3.criacao.ToString();
                }

                var rsvisita4 = db.ativacao_foto.Where(d => d.id_ativacao == item.id && d.etapa == 4).FirstOrDefault();
                var visita4 = "";

                if (rsvisita4 != null)
                {
                    visita4 = rsvisita4.criacao.ToString();
                }

                var rsvisita5 = db.ativacao_foto.Where(e => e.id_ativacao == item.id && e.etapa == 5).FirstOrDefault();
                var visita5 = "";

                if (rsvisita5 != null)
                {
                    visita5 = rsvisita5.criacao.ToString();
                }

                var rsvisita6 = db.ativacao_foto.Where(f => f.id_ativacao == item.id && f.etapa == 6).FirstOrDefault();
                var visita6 = "";

                if (rsvisita6 != null)
                {
                    visita6 = rsvisita6.criacao.ToString();
                }

                var rsvisita7 = db.ativacao_foto.Where(g => g.id_ativacao == item.id && g.etapa == 7).FirstOrDefault();
                var visita7 = "";

                if (rsvisita7 != null)
                {
                    visita7 = rsvisita7.criacao.ToString();
                }

                var rsvisita8 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 8).FirstOrDefault();
                var visita8 = "";

                if (rsvisita8 != null)
                {
                    visita8 = rsvisita8.criacao.ToString();
                }

                var rsvisita9 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 9).FirstOrDefault();
                var visita9 = "";

                if (rsvisita9 != null)
                {
                    visita9 = rsvisita9.criacao.ToString();
                }

                var rsvisita10 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 10).FirstOrDefault();
                var visita10 = "";

                if (rsvisita10 != null)
                {
                    visita10 = rsvisita10.criacao.ToString();
                }

                var rsvisita11 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 11).FirstOrDefault();
                var visita11 = "";

                if (rsvisita11 != null)
                {
                    visita11 = rsvisita11.criacao.ToString();
                }

                var rsvisita12 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 12).FirstOrDefault();
                var visita12 = "";

                if (rsvisita12 != null)
                {
                    visita12 = rsvisita12.criacao.ToString();
                }


                var rsvisitaex2 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 2 && a.executivo == 1).FirstOrDefault();
                var visitaex2 = "";

                if (rsvisitaex2 != null)
                {
                    visitaex2 = rsvisitaex2.criacao.ToString();
                }

                var rsvisitaex3 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 3 && a.executivo == 1).FirstOrDefault();
                var visitaex3 = "";

                if (rsvisitaex3 != null)
                {
                    visitaex3 = rsvisitaex3.criacao.ToString();
                }

                var rsvisitaex4 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 4 && a.executivo == 1).FirstOrDefault();
                var visitaex4 = "";

                if (rsvisitaex4 != null)
                {
                    visitaex4 = rsvisitaex4.criacao.ToString();
                }

                var rsvisitaex5 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 5 && a.executivo == 1).FirstOrDefault();
                var visitaex5 = "";

                if (rsvisitaex5 != null)
                {
                    visitaex5 = rsvisitaex5.criacao.ToString();
                }


                var rsvisitaex6 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 6 && a.executivo == 1).FirstOrDefault();
                var visitaex6 = "";

                if (rsvisitaex6 != null)
                {
                    visitaex6 = rsvisitaex6.criacao.ToString();
                }


                var rsvisitaex7 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 7 && a.executivo == 1).FirstOrDefault();
                var visitaex7 = "";

                if (rsvisitaex7 != null)
                {
                    visitaex7 = rsvisitaex7.criacao.ToString();
                }

                var rsvisitaex8 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 8 && a.executivo == 1).FirstOrDefault();
                var visitaex8 = "";

                if (rsvisitaex8 != null)
                {
                    visitaex8 = rsvisitaex8.criacao.ToString();
                }

                var rsvisitaex9 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 9 && a.executivo == 1).FirstOrDefault();
                var visitaex9 = "";

                if (rsvisitaex9 != null)
                {
                    visitaex9 = rsvisitaex9.criacao.ToString();
                }

                var rsvisitaex10 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 10 && a.executivo == 1).FirstOrDefault();
                var visitaex10 = "";

                if (rsvisitaex10 != null)
                {
                    visitaex10 = rsvisitaex10.criacao.ToString();
                }

                var rsvisitaex11 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 11 && a.executivo == 1).FirstOrDefault();
                var visitaex11 = "";

                if (rsvisitaex11 != null)
                {
                    visitaex11 = rsvisitaex11.criacao.ToString();
                }

                var rsvisitaex12 = db.ativacao_foto.Where(a => a.id_ativacao == item.id && a.etapa == 12 && a.executivo == 1).FirstOrDefault();
                var visitaex12 = "";

                if (rsvisitaex12 != null)
                {
                    visitaex12 = rsvisitaex12.criacao.ToString();
                }



                var itemetapa = db.ativacao_foto.Where(a => a.id_ativacao == item.id).FirstOrDefault();
                var etapa = "";

                if (itemetapa.etapa != null)
                {
                    etapa = itemetapa.etapa.ToString();
                }

                var projeto = "";
                if (item.projeto.descricao != null)
                {
                    projeto = item.projeto.descricao.ToString();
                }

                var data_finalizacao = "";
                if (item.data_finalizacao != null)
                {
                    data_finalizacao = item.data_finalizacao.ToString();
                }

                var itemcluster = db.cliente_cluster_SAP.Where(a => a.ZCLIENTE == item.ZCLIENTE).FirstOrDefault();
                var cluster = "";
                if (itemcluster.cluster_SAP.ZNOMBRE != null)
                {
                    cluster = itemcluster.cluster_SAP.ZNOMBRE.ToString();
                }

                var razao_social = "";
                if (item.cliente_SAP.ZNOMBRE != null)
                {
                    razao_social = item.cliente_SAP.ZNOMBRE.ToString();
                }

                var franquia = "";
                if (item.cliente_SAP.franquia != null)
                {
                    franquia = item.cliente_SAP.franquia.descricao.ToString();
                }

                var uf_sigla = "";
                if (item.cliente_SAP.ZUF != null)
                {
                    uf_sigla = item.cliente_SAP.ZUF.ToString();
                }

                var cidade = "";
                if (item.cliente_SAP.ZCIUDAD != null)
                {
                    cidade = item.cliente_SAP.ZCIUDAD.ToString();
                }

                var id_unidade = "";
                if (item.cliente_SAP.ZUNIDAD != null)
                {
                    id_unidade = item.cliente_SAP.unidade_SAP.ZNOMBRE.ToString();
                }

                /*   var id_rota = "";
                   if (item.cliente.id_rota != null)
                   {
                       id_rota = item.cliente.rota.descricao.ToString();
                   } */

                var prospector = "";
                if (item.cliente_SAP.prospector != null)
                {
                    prospector = item.cliente_SAP.prospector.ToString();
                }

                var cliente_codigo = "";
                if (item.cliente_SAP.ZCLIENTE != null)
                {
                    cliente_codigo = item.cliente_SAP.ZCLIENTE.ToString();
                }

                var dias2 = "";
                var situacao = "";
                int qntdias = 0;
                var data = item.cliente_SAP.ativacao_ultima_atualizacao.ToString();
                var dataUltima = DateTime.Parse(data);
                var dataHoje = DateTime.Now;

                var verifica = dataUltima.AddDays(21);
                if (rsvisita2 == null)
                {
                    if (dataHoje > verifica)
                    {
                        qntdias = (dataHoje - verifica).Days;
                        situacao = "atraso";
                        situacao = qntdias.ToString() + " dias em " + situacao;
                        dias2 = situacao;
                    }
                    else if (dataHoje < verifica)
                    {
                        qntdias = (verifica - dataHoje).Days;
                        situacao = "pendentes";
                        situacao = qntdias.ToString() + " dias " + situacao;
                        dias2 = situacao;

                    }

                    if (qntdias == 0)
                    {
                        situacao = " realizar ";
                        dias2 = situacao;

                    }
                }

                var dias3 = "";
                if (rsvisita3 == null && rsvisita2 != null)
                {
                    if (dataHoje > verifica)
                    {
                        qntdias = (dataHoje - verifica).Days;
                        situacao = "atraso";
                        situacao = qntdias.ToString() + " dias em " + situacao;
                        dias3 = situacao;
                    }
                    else if (dataHoje < verifica)
                    {
                        qntdias = (verifica - dataHoje).Days;
                        situacao = "pendentes";
                        situacao = qntdias.ToString() + " dias " + situacao;
                        dias3 = situacao;

                    }

                    if (qntdias == 0)
                    {
                        situacao = " realizar ";
                        dias3 = situacao;

                    }
                }

                var dias4 = "";
                if (rsvisita4 == null && rsvisita3 != null)
                {
                    if (dataHoje > verifica)
                    {
                        qntdias = (dataHoje - verifica).Days;
                        situacao = "atraso";
                        situacao = qntdias.ToString() + " dias em " + situacao;
                        dias4 = situacao;
                    }
                    else if (dataHoje < verifica)
                    {
                        qntdias = (verifica - dataHoje).Days;
                        situacao = "pendentes";
                        situacao = qntdias.ToString() + " dias " + situacao;
                        dias4 = situacao;

                    }

                    if (qntdias == 0)
                    {
                        situacao = " realizar ";
                        dias4 = situacao;

                    }
                }

                var dias5 = "";
                if (rsvisita5 == null && rsvisita4 != null)
                {
                    if (dataHoje > verifica)
                    {
                        qntdias = (dataHoje - verifica).Days;
                        situacao = "atraso";
                        situacao = qntdias.ToString() + " dias em " + situacao;
                        dias5 = situacao;
                    }
                    else if (dataHoje < verifica)
                    {
                        qntdias = (verifica - dataHoje).Days;
                        situacao = "pendentes";
                        situacao = qntdias.ToString() + " dias " + situacao;
                        dias5 = situacao;

                    }

                    if (qntdias == 0)
                    {
                        situacao = " realizar ";
                        dias5 = situacao;

                    }
                }

                var dias6 = "";
                if (rsvisita6 == null && rsvisita5 != null)
                {
                    if (dataHoje > verifica)
                    {
                        qntdias = (dataHoje - verifica).Days;
                        situacao = "atraso";
                        situacao = qntdias.ToString() + " dias em " + situacao;
                        dias6 = situacao;
                    }
                    else if (dataHoje < verifica)
                    {
                        qntdias = (verifica - dataHoje).Days;
                        situacao = "pendentes";
                        situacao = qntdias.ToString() + " dias " + situacao;
                        dias6 = situacao;

                    }

                    if (qntdias == 0)
                    {
                        situacao = " realizar ";
                        dias6 = situacao;

                    }
                }

                var dias7 = "";
                if (rsvisita7 == null && rsvisita6 != null)
                {
                    if (dataHoje > verifica)
                    {
                        qntdias = (dataHoje - verifica).Days;
                        situacao = "atraso";
                        situacao = qntdias.ToString() + " dias em " + situacao;
                        dias7 = situacao;
                    }
                    else if (dataHoje < verifica)
                    {
                        qntdias = (verifica - dataHoje).Days;
                        situacao = "pendentes";
                        situacao = qntdias.ToString() + " dias " + situacao;
                        dias7 = situacao;

                    }

                    if (qntdias == 0)
                    {
                        situacao = " realizar ";
                        dias7 = situacao;

                    }
                }

                var dias8 = "";
                if (rsvisita8 == null && rsvisita7 != null)
                {
                    if (dataHoje > verifica)
                    {
                        qntdias = (dataHoje - verifica).Days;
                        situacao = "atraso";
                        situacao = qntdias.ToString() + " dias em " + situacao;
                        dias8 = situacao;
                    }
                    else if (dataHoje < verifica)
                    {
                        qntdias = (verifica - dataHoje).Days;
                        situacao = "pendentes";
                        situacao = qntdias.ToString() + " dias " + situacao;
                        dias8 = situacao;

                    }

                    if (qntdias == 0)
                    {
                        situacao = " realizar ";
                        dias8 = situacao;

                    }
                }

                var dias9 = "";
                if (rsvisita9 == null && rsvisita8 != null)
                {
                    if (dataHoje > verifica)
                    {
                        qntdias = (dataHoje - verifica).Days;
                        situacao = "atraso";
                        situacao = qntdias.ToString() + " dias em " + situacao;
                        dias9 = situacao;
                    }
                    else if (dataHoje < verifica)
                    {
                        qntdias = (verifica - dataHoje).Days;
                        situacao = "pendentes";
                        situacao = qntdias.ToString() + " dias " + situacao;
                        dias9 = situacao;

                    }

                    if (qntdias == 0)
                    {
                        situacao = " realizar ";
                        dias9 = situacao;

                    }
                }

                var dias10 = "";
                if (rsvisita10 == null && rsvisita9 != null)
                {
                    if (dataHoje > verifica)
                    {
                        qntdias = (dataHoje - verifica).Days;
                        situacao = "atraso";
                        situacao = qntdias.ToString() + " dias em " + situacao;
                        dias10 = situacao;
                    }
                    else if (dataHoje < verifica)
                    {
                        qntdias = (verifica - dataHoje).Days;
                        situacao = "pendentes";
                        situacao = qntdias.ToString() + " dias " + situacao;
                        dias10 = situacao;

                    }

                    if (qntdias == 0)
                    {
                        situacao = " realizar ";
                        dias10 = situacao;

                    }
                }

                var dias11 = "";
                if (rsvisita11 == null && rsvisita10 != null)
                {
                    if (dataHoje > verifica)
                    {
                        qntdias = (dataHoje - verifica).Days;
                        situacao = "atraso";
                        situacao = qntdias.ToString() + " dias em " + situacao;
                        dias11 = situacao;
                    }
                    else if (dataHoje < verifica)
                    {
                        qntdias = (verifica - dataHoje).Days;
                        situacao = "pendentes";
                        situacao = qntdias.ToString() + " dias " + situacao;
                        dias11 = situacao;

                    }

                    if (qntdias == 0)
                    {
                        situacao = " realizar ";
                        dias11 = situacao;

                    }
                }

                var dias12 = "";
                if (rsvisita12 == null && rsvisita11 != null)
                {
                    if (dataHoje > verifica)
                    {
                        qntdias = (dataHoje - verifica).Days;
                        situacao = "atraso";
                        situacao = qntdias.ToString() + " dias em " + situacao;
                        dias12 = situacao;
                    }
                    else if (dataHoje < verifica)
                    {
                        qntdias = (verifica - dataHoje).Days;
                        situacao = "pendentes";
                        situacao = qntdias.ToString() + " dias " + situacao;
                        dias12 = situacao;

                    }

                    if (qntdias == 0)
                    {
                        situacao = " realizar ";
                        dias12 = situacao;

                    }
                }

                dt.Rows.Add(
                    cliente_codigo,
                    razao_social,
                    uf_sigla,
                    cidade,
                    franquia,
                    id_unidade,
                    /* id_rota,*/
                    prospector,
                    cluster,
                    visita1,
                    projeto,
                    etapa,
                    dias2,
                    visita2,
                    visitaex2,
                    dias3,
                    visita3,
                    visitaex3,
                    dias4,
                    visita4,
                    visitaex4,
                    dias5,
                    visita5,
                    visitaex5,
                    dias6,
                    visita6,
                    visitaex6,
                    dias7,
                    visita7,
                    visitaex7,
                    dias8,
                    visita8,
                    visitaex8,
                    dias9,
                    visita9,
                    visitaex9,
                    dias10,
                    visita10,
                    visitaex10,
                    dias11,
                    visita11,
                    visitaex11,
                    dias12,
                    visita12,
                    visitaex12,
                    data_finalizacao);

            }

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", "attachment;filename=relatorios.xls");
            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteExcelWithNPOI(dt, "xlsx", 3);
            //Response.End();

            return View("Relatorios/MonitoramentoRevisitas"); //left join ou right join
        }


        // GET: /RelatorioRecusas        
        public ActionResult RelatorioRecusas()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.id_prospector = new SelectList(db.prospector, "id", "nome");
            ViewBag.id_executivo = new SelectList(db.executivo, "id", "nome");
            ViewBag.id_unidade = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE");
            ViewBag.id_cliente = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE");
            ViewBag.id_peca = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA");
            ViewBag.id_cluster = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE");
            ViewBag.id_status = new SelectList(db.status_ativacao, "id", "nome");
            ViewBag.id_franquia = new SelectList(db.franquia, "id", "descricao");
            ViewBag.id_projeto = new SelectList(db.projeto, "id", "descricao");
            ViewBag.id_territorio = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE");
            ViewBag.id_matricula = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZCLIENTE");
            return View();
        }

        //POST: /RelatorioRecusas
        [HttpPost]
        public ActionResult RelatorioRecusas(int? id)
        {
            var datainicial = Request.Form["datainicial"];
            var datafinal = Request.Form["datafinal"];

            string[] arrDate = datainicial.Split('/');

            string day = arrDate[0].ToString();
            string month = arrDate[1].ToString();
            string year = arrDate[2].ToString();

            string[] arrDate2 = datafinal.Split('/');

            string day2 = arrDate2[0].ToString();
            string month2 = arrDate2[1].ToString();
            string year2 = arrDate2[2].ToString();


           // var sql = "SELECT * FROM ativacao WHERE ativacao.status = 6";
            var sql = "SELECT DISTINCT ativacao.* FROM ativacao INNER JOIN cliente_SAP ON ativacao.ZCLIENTE = cliente_SAP.ZCLIENTE WHERE ativacao.status = 6";

            if (datainicial != null || datafinal != null)
            {
                sql = sql + " AND ativacao.criacao BETWEEN  '" + year + "-" + month + "-" + day + "' AND '" + year2 + "-" + month2 + "-" + day2 + "'";
            }

            DataTable dt = new DataTable();

            dt.Columns.Add("Matrícula_Cliente", typeof(string));
            dt.Columns.Add("Razão_Social", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("UF", typeof(string));
            dt.Columns.Add("Unidade", typeof(string));
            dt.Columns.Add("Executivo", typeof(string));
            dt.Columns.Add("Prospector", typeof(string));
            dt.Columns.Add("Data_Recusa", typeof(string));
            dt.Columns.Add("Código_Motivo", typeof(string));
            dt.Columns.Add("Motivo", typeof(string));


            var itemAtivacao = db.ativacao.SqlQuery(sql).ToList();

            foreach (var item in itemAtivacao)
            {

                var data_recusa = "";
                if (item.cliente_SAP.ativacao_ultima_atualizacao != null)
                {
                    data_recusa = item.cliente_SAP.ativacao_ultima_atualizacao.ToString();
                }

                var codigo_motivo = "";
                if (item.cliente_SAP.motivo_id != null)
                {
                    codigo_motivo = item.cliente_SAP.motivo_id.ToString();
                }

                var motivo = "";
                if (item.cliente_SAP.motivotxt != null)
                {
                    motivo = item.cliente_SAP.motivotxt.ToString();
                }

                var razao_social = "";
                if (item.cliente_SAP.ZNOMBRE != null)
                {
                    razao_social = item.cliente_SAP.ZNOMBRE.ToString();
                }

                var status_ativacao = "";
                if (item.status_ativacao != null)
                {
                    status_ativacao = item.status_ativacao.nome.ToString();
                }

                var uf_sigla = "";
                if (item.cliente_SAP.ZUF != null)
                {
                    uf_sigla = item.cliente_SAP.ZUF.ToString();
                }

                var id_unidade = "";
                if (item.cliente_SAP.ZUNIDAD != null)
                {
                    id_unidade = item.cliente_SAP.unidade_SAP.ZNOMBRE.ToString();
                }

                var executivo = "";
                if (item.cliente_SAP.executivo != null)
                {
                    executivo = item.cliente_SAP.executivo.ToString();
                }

                var prospector = "";
                if (item.cliente_SAP.prospector != null)
                {
                    prospector = item.cliente_SAP.prospector.ToString();
                }

                var cliente_codigo = "";
                if (item.cliente_SAP.ZCLIENTE != null)
                {
                    cliente_codigo = item.cliente_SAP.ZCLIENTE.ToString();
                }

                dt.Rows.Add(
                    cliente_codigo,
                    razao_social,
                    status_ativacao,
                    uf_sigla,
                    id_unidade,
                    executivo,
                    prospector,
                    data_recusa,
                    codigo_motivo,
                    motivo);
            }

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", "attachment;filename=relatorios.xls");
            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteExcelWithNPOI(dt, "xlsx", 4);
            //Response.End();

            return View("Relatorios/RelatorioRecusas"); //left join ou right join
        }
        // GET: /RelatorioExtrapolacoes        
        public ActionResult RelatorioExtrapolacoes()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.id_prospector = new SelectList(db.prospector, "id", "nome");
            ViewBag.id_executivo = new SelectList(db.executivo, "id", "nome");
            ViewBag.id_unidade = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE");
            ViewBag.id_cliente = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE");
            ViewBag.id_peca = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA");
            ViewBag.id_cluster = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE");
            ViewBag.id_status = new SelectList(db.status_ativacao, "id", "nome");
            ViewBag.id_franquia = new SelectList(db.franquia, "id", "descricao");
            ViewBag.id_projeto = new SelectList(db.projeto, "id", "descricao");
            ViewBag.id_territorio = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE");
            ViewBag.id_matricula = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZCLIENTE");
            return View();
        }

        //POST: /RelatorioExtrapolacoes
        [HttpPost]
        public ActionResult RelatorioExtrapolacoes(int? id)
        {
            var datainicial = Request.Form["datainicial"];
            var datafinal = Request.Form["datafinal"];

            string[] arrDate = datainicial.Split('/');

            string day = arrDate[0].ToString();
            string month = arrDate[1].ToString();
            string year = arrDate[2].ToString();

            string[] arrDate2 = datafinal.Split('/');

            string day2 = arrDate2[0].ToString();
            string month2 = arrDate2[1].ToString();
            string year2 = arrDate2[2].ToString();




            var sql = "SELECT DISTINCT ativacao.* FROM ativacao INNER JOIN cliente_SAP ON ativacao.ZCLIENTE = cliente_SAP.ZCLIENTE WHERE 1=1";

            if (datainicial != null || datafinal != null)
            {
                sql = sql + " AND ativacao.criacao BETWEEN  '" + year + "-" + month + "-" + day + "' AND '" + year2 + "-" + month2 + "-" + day2 + "'";
            }

            DataTable dt = new DataTable();

            dt.Columns.Add("Franquia", typeof(string));
            dt.Columns.Add("Unidade", typeof(string));
            dt.Columns.Add("Matrícula_Cliente", typeof(string));
            dt.Columns.Add("Razão_Social", typeof(string));
            dt.Columns.Add("Ano", typeof(string));
            dt.Columns.Add("Autorizado", typeof(string));
            dt.Columns.Add("Não_Autorizado", typeof(string));
            dt.Columns.Add("Excedente", typeof(string));
            dt.Columns.Add("Data_Autorização", typeof(string));
            dt.Columns.Add("Data_Negação", typeof(string));
            dt.Columns.Add("Cluster", typeof(string));



            var itemAtivacao = db.ativacao.SqlQuery(sql).ToList();

            foreach (var item in itemAtivacao)
            {

                var clientecluster = "";
                var checkcc = db.cliente_cluster_SAP.Where(a => a.ZCLIENTE == item.cliente_SAP.ZCLIENTE).ToList();
                if (checkcc != null)
                {
                    int i = 0;
                    var virgula = "";
                    foreach (var item9 in checkcc)
                    {
                        if (i == checkcc.Count)
                        {
                            virgula = "";
                        }
                        else
                        {
                            virgula = ",";
                        }
                        clientecluster = clientecluster + item9.cluster_SAP.ZNOMBRE.Replace(" ", "") + virgula;
                        i++;
                    }
                }

                var ano_cliente = "";
                if (item.cliente_SAP.ano_cliente != null)
                {
                    ano_cliente = item.cliente_SAP.ano_cliente.ToString();
                }

                var data_recusa = "";
                if (item.cliente_SAP.ativacao_ultima_atualizacao != null)
                {
                    data_recusa = item.cliente_SAP.ativacao_ultima_atualizacao.ToString();
                }

                var codigo_motivo = "";
                if (item.cliente_SAP.motivo_id != null)
                {
                    codigo_motivo = item.cliente_SAP.motivo_id.ToString();
                }

                var motivo = "";
                if (item.cliente_SAP.motivotxt != null)
                {
                    motivo = item.cliente_SAP.motivotxt.ToString();
                }

                var razao_social = "";
                if (item.cliente_SAP.ZNOMBRE != null)
                {
                    razao_social = item.cliente_SAP.ZNOMBRE.ToString();
                }

                var status_ativacao = "";
                if (item.status_ativacao != null)
                {
                    status_ativacao = item.status_ativacao.nome.ToString();
                }

                var uf_sigla = "";
                if (item.cliente_SAP.ZUF != null)
                {
                    uf_sigla = item.cliente_SAP.ZUF.ToString();
                }

                var id_unidade = "";
                if (item.cliente_SAP.ZUNIDAD != null)
                {
                    id_unidade = item.cliente_SAP.unidade_SAP.ZNOMBRE.ToString();
                }

                var id_franquia = "";
                if (item.cliente_SAP.ZTERRITOR != null)
                {
                    id_franquia = item.cliente_SAP.territorio_SAP.ZNOMBRE.ToString();
                }


                var executivo = "";
                if (item.cliente_SAP.executivo != null)
                {
                    executivo = item.cliente_SAP.executivo.ToString();
                }

                var prospector = "";
                if (item.cliente_SAP.prospector != null)
                {
                    prospector = item.cliente_SAP.prospector.ToString();
                }

                var cliente_codigo = "";
                if (item.cliente_SAP.ZCLIENTE != null)
                {
                    cliente_codigo = item.cliente_SAP.ZCLIENTE.ToString();
                }

                var nulo = "";

                var excedente = Convert.ToDecimal(0);
                if (item.custo_valor >= item.cliente_SAP.custo_budget)
                {
                    var tem = Convert.ToDecimal(item.cliente_SAP.custo_budget);
                    var gastou = Convert.ToDecimal(item.custo_valor);
                    excedente = gastou - tem;
                }

                dt.Rows.Add(
                    id_franquia,
                    id_unidade,
                    cliente_codigo,
                    razao_social,
                    ano_cliente,
                    nulo,
                    nulo,
                    excedente,
                    nulo,
                    nulo,
                    clientecluster);
            }

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", "attachment;filename=relatorios.xls");
            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteExcelWithNPOI(dt, "xlsx", 5);
            //Response.End();

            return View("Relatorios/RelatorioExtrapolacoes"); //left join ou right join
        }

        // GET: /PecasUnidades        
        public ActionResult PecasUnidades()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.id_prospector = new SelectList(db.prospector, "id", "nome");
            ViewBag.id_executivo = new SelectList(db.executivo, "id", "nome");
            ViewBag.id_unidade = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE");
            ViewBag.id_cliente = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE");
            ViewBag.id_peca = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA");
            ViewBag.id_cluster = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE");
            ViewBag.id_status = new SelectList(db.status_ativacao, "id", "nome");
            ViewBag.id_franquia = new SelectList(db.franquia, "id", "descricao");
            ViewBag.id_projeto = new SelectList(db.projeto, "id", "descricao");
            ViewBag.id_territorio = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE");
            ViewBag.id_matricula = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZCLIENTE");
            return View();
        }


        //POST: /PecasUnidades
        [HttpPost]
        public ActionResult PecasUnidades(int? id)
        {
            var anocliente = Request.Form["anocliente"];

            var id_cliente = 0;

            if (Request.Form["id_cliente"] != "")
            {
                id_cliente = Convert.ToInt32(Request.Form["id_cliente"]);
            }

            var id_projeto = 0;

            if (Request.Form["id_projeto"] != "")
            {
                id_projeto = Convert.ToInt32(Request.Form["id_projeto"]);
            }

            var id_executivo = 0;

            if (Request.Form["id_executivo"] != "")
            {
                id_executivo = Convert.ToInt32(Request.Form["id_executivo"]);
            }

            var id_prospector = 0;

            if (Request.Form["id_prospector"] != "")
            {
                id_prospector = Convert.ToInt32(Request.Form["id_prospector"]);
            }

            var id_peca = 0;

            if (Request.Form["id_peca"] != "")
            {
                id_peca = Convert.ToInt32(Request.Form["id_peca"]);
            }

            var id_cluster = 0;

            if (Request.Form["id_cluster"] != "")
            {
                id_cluster = Convert.ToInt32(Request.Form["id_cluster"]);
            }

            var id_status = Request.Form["id_status"];
            var datainicial = Request.Form["datainicial"];
            var datafinal = Request.Form["datafinal"];

            string[] arrDate = datainicial.Split('/');

            string day = arrDate[0].ToString();
            string month = arrDate[1].ToString();
            string year = arrDate[2].ToString();

            string[] arrDate2 = datafinal.Split('/');

            string day2 = arrDate2[0].ToString();
            string month2 = arrDate2[1].ToString();
            string year2 = arrDate2[2].ToString();


            var sql = "SELECT DISTINCT ativacao.* FROM ativacao INNER JOIN cliente_SAP ON ativacao.ZCLIENTE = cliente_SAP.ZCLIENTE INNER JOIN ativacao_peca ON ativacao.id = ativacao_peca.id_ativacao INNER JOIN cliente_cluster_SAP ON cliente_SAP.ZCLIENTE = cliente_cluster_SAP.ZCLIENTE WHERE 1=1";

            if (id_cliente > 0)
            {
                sql = sql + " AND ativacao.ZCLIENTE = " + id_cliente;
            }

            if (id_projeto > 0)
            {
                sql = sql + " AND ativacao.id_projeto = " + id_projeto;
            }

            if (id_prospector > 0)
            {
                sql = sql + " AND cliente_SAP.id_prospector = " + id_prospector;
            }
            if (id_executivo > 0)
            {
                sql = sql + " AND cliente_SAP.id_executivo = " + id_executivo;
            }
            if (id_cluster > 0)
            {
                sql = sql + " AND cliente_cluster_SAP.ZCODCLUSTER = " + id_cluster;
            }
            if (id_peca > 0)
            {
                sql = sql + " AND ativacao_peca.ZCODPECA = " + id_peca;
            }
            if (anocliente != null && anocliente != "")
            {
                sql = sql + " AND cliente_SAP.ano_cliente = '" + anocliente + "'";
            }
            if (id_status != null)
            {
                if (id_status.Count() == 1)
                {
                    foreach (var itemselecionado in id_status)
                    {
                        if (itemselecionado != ',')
                        {
                            sql = sql + " AND ativacao.status = " + itemselecionado;
                        }
                    }
                }
                else
                {
                    sql = sql + " AND (";

                    int numerocount = 1;

                    foreach (var itemselecionado in id_status)
                    {
                        if (numerocount == 1)
                        {

                            if (itemselecionado != ',')
                            {
                                sql = sql + " ativacao.status = " + itemselecionado;
                            }
                        }
                        else
                        {
                            if (itemselecionado != ',')
                            {
                                sql = sql + " OR ativacao.status = " + itemselecionado;
                            }
                        }

                        numerocount = numerocount + 1;
                    }

                    sql = sql + ")";
                }

            }
            if (datainicial != null || datafinal != null)
            {
                sql = sql + " AND ativacao.criacao BETWEEN  '" + year + "-" + month + "-" + day + "' AND '" + year2 + "-" + month2 + "-" + day2 + "'";
            }

            //sql = sql + " GROUP BY ativacao.id " ;

            DataTable dt = new DataTable();

            dt.Columns.Add("Código_Peca", typeof(string));
            dt.Columns.Add("Nome_Peça", typeof(string));
            dt.Columns.Add("Matrícula Cliente", typeof(string));
            dt.Columns.Add("Razão_Social", typeof(string));
            dt.Columns.Add("Unidade", typeof(string));
            dt.Columns.Add("UF", typeof(string));
            dt.Columns.Add("Ano", typeof(string));
            dt.Columns.Add("Data_Negociação", typeof(string));
            dt.Columns.Add("Data_Instalação", typeof(string));
            dt.Columns.Add("Cluster", typeof(string));


            /*     dt.Columns.Add("Código Peça", typeof(string));
               dt.Columns.Add("Data da Ativação", typeof(string));
               dt.Columns.Add("Cluster", typeof(string)); */

            var itemAtivacao = db.ativacao.SqlQuery(sql).ToList();

            foreach (var item in itemAtivacao)
            {
                var c = db.ativacao_peca.Where(a => a.id_ativacao == item.id);

                if (c.Count() == 0)
                {
                    var codigo_peca = "Sem peça relacionada para esta ativação";
                    var nome_peca = "Sem peça relacionada para esta ativação";

                        var cliente_codigo = "";
                        if (item.cliente_SAP.ZCLIENTE != null)
                        {
                            cliente_codigo = item.cliente_SAP.ZCLIENTE.ToString();
                        }

                        var razao_social = "";
                        if (item.cliente_SAP.ZNOMBRE != null)
                        {
                            razao_social = item.cliente_SAP.ZNOMBRE.ToString();
                        }

                        var uf_sigla = "";
                        if (item.cliente_SAP.ZUF != null)
                        {
                            uf_sigla = item.cliente_SAP.ZUF.ToString();
                        }

                        var id_unidade = "";
                        if (item.cliente_SAP.ZUNIDAD != null)
                        {
                            id_unidade = item.cliente_SAP.unidade_SAP.ZNOMBRE.ToString();
                        }

                        var ano_cliente = "";
                        if (item.cliente_SAP.ano_cliente != null)
                        {
                            ano_cliente = item.cliente_SAP.ano_cliente.ToString();
                        }

                        var itemcluster = db.cliente_cluster_SAP.Where(a => a.ZCLIENTE == item.ZCLIENTE).FirstOrDefault();
                        var cluster = "";
                        if (itemcluster.cluster_SAP.ZNOMBRE != null)
                        {
                            cluster = itemcluster.cluster_SAP.ZNOMBRE.ToString();
                        }

                        /* var codigo_peca = "";
                         if (item.ativacao_peca != null)
                         {
                             codigo_peca = item.ativacao_peca.ToString();
                         }
                         */
                        /*    var nome_peca = "";
                            if (item.ativacao_peca != null)
                            {
                                nome_peca = item.ativacao_peca.ToString();
                            } */

                        var data_ativacao = "";
                        if (item.criacao != null)
                        {
                            data_ativacao = item.criacao.ToString();
                        }

                        var clientecluster = "";
                        var checkcc = db.cliente_cluster_SAP.Where(a => a.ZCLIENTE == item.cliente_SAP.ZCLIENTE).ToList();
                        if (checkcc != null)
                        {
                            int i = 0;
                            var virgula = "";
                            foreach (var item9 in checkcc)
                            {
                                if (i == checkcc.Count)
                                {
                                    virgula = "";
                                }
                                else
                                {
                                    virgula = ",";
                                }
                                clientecluster = clientecluster + item9.cluster_SAP.ZNOMBRE.Replace(" ", "") + virgula;
                                i++;
                            }
                        }

                        dt.Rows.Add(
                            codigo_peca,
                            nome_peca,
                            cliente_codigo,
                            razao_social,
                            id_unidade,
                            uf_sigla,
                            ano_cliente,
                            data_ativacao,
                            data_ativacao,
                            clientecluster);
                    
                }
                else
                {
                    foreach (var item2 in c)
                    {
                        var codigo_peca = item2.peca_SAP.ZCODPECA;
                        var nome_peca = item2.peca_SAP.ZNOMPECA;

                        var cliente_codigo = "";
                        if (item.cliente_SAP.ZCLIENTE != null)
                        {
                            cliente_codigo = item.cliente_SAP.ZCLIENTE.ToString();
                        }

                        var razao_social = "";
                        if (item.cliente_SAP.ZNOMBRE != null)
                        {
                            razao_social = item.cliente_SAP.ZNOMBRE.ToString();
                        }

                        var uf_sigla = "";
                        if (item.cliente_SAP.ZUF != null)
                        {
                            uf_sigla = item.cliente_SAP.ZUF.ToString();
                        }

                        var id_unidade = "";
                        if (item.cliente_SAP.ZUNIDAD != null)
                        {
                            id_unidade = item.cliente_SAP.unidade_SAP.ZNOMBRE.ToString();
                        }

                        var ano_cliente = "";
                        if (item.cliente_SAP.ano_cliente != null)
                        {
                            ano_cliente = item.cliente_SAP.ano_cliente.ToString();
                        }

                        var itemcluster = db.cliente_cluster_SAP.Where(a => a.ZCLIENTE == item.ZCLIENTE).FirstOrDefault();
                        var cluster = "";
                        if (itemcluster.cluster_SAP.ZNOMBRE != null)
                        {
                            cluster = itemcluster.cluster_SAP.ZNOMBRE.ToString();
                        }

                        /* var codigo_peca = "";
                         if (item.ativacao_peca != null)
                         {
                             codigo_peca = item.ativacao_peca.ToString();
                         }
                         */
                        /*    var nome_peca = "";
                            if (item.ativacao_peca != null)
                            {
                                nome_peca = item.ativacao_peca.ToString();
                            } */

                        var data_ativacao = "";
                        if (item.criacao != null)
                        {
                            data_ativacao = item.criacao.ToString();
                        }

                        var clientecluster = "";
                        var checkcc = db.cliente_cluster_SAP.Where(a => a.ZCLIENTE == item.cliente_SAP.ZCLIENTE).ToList();
                        if (checkcc != null)
                        {
                            int i = 0;
                            var virgula = "";
                            foreach (var item9 in checkcc)
                            {
                                if (i == checkcc.Count)
                                {
                                    virgula = "";
                                }
                                else
                                {
                                    virgula = ",";
                                }
                                clientecluster = clientecluster + item9.cluster_SAP.ZNOMBRE.Replace(" ", "") + virgula;
                                i++;
                            }
                        }

                        dt.Rows.Add(
                            codigo_peca,
                            nome_peca,
                            cliente_codigo,
                            razao_social,
                            id_unidade,
                            uf_sigla,
                            ano_cliente,
                            data_ativacao,
                            data_ativacao,
                            clientecluster);
                    }
                }




            }

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", "attachment;filename=relatorios.xls");
            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteExcelWithNPOI(dt, "xlsx", 6);
            //Response.End();

            return View("Relatorios/PecasUnidades"); //left join ou right join
        }

        // GET: /RelatorioVerbas        
        public ActionResult RelatorioVerbas()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.id_prospector = new SelectList(db.prospector, "id", "nome");
            ViewBag.id_executivo = new SelectList(db.executivo, "id", "nome");
            ViewBag.id_unidade = new SelectList(db.unidade_SAP, "ZCODUNIDA", "ZNOMBRE");
            ViewBag.id_cliente = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZNOMBRE");
            ViewBag.id_peca = new SelectList(db.peca_SAP, "ZCODPECA", "ZNOMPECA");
            ViewBag.id_cluster = new SelectList(db.cluster_SAP, "ZCODCLUST", "ZNOMBRE");
            ViewBag.id_status = new SelectList(db.status_ativacao, "id", "nome");
            ViewBag.id_franquia = new SelectList(db.franquia, "id", "descricao");
            ViewBag.id_projeto = new SelectList(db.projeto, "id", "descricao");
            ViewBag.id_territorio = new SelectList(db.territorio_SAP, "ZCODTERRI", "ZNOMBRE");
            ViewBag.id_matricula = new SelectList(db.cliente_SAP, "ZCLIENTE", "ZCLIENTE");
            return View();
        }


        //POST: /RelatorioVerbas
        [HttpPost]
        public ActionResult RelatorioVerbas(int? id)
        {
            var datainicial = Request.Form["datainicial"];
            var datafinal = Request.Form["datafinal"];

            string[] arrDate = datainicial.Split('/');

            string day = arrDate[0].ToString();
            string month = arrDate[1].ToString();
            string year = arrDate[2].ToString();

            string[] arrDate2 = datafinal.Split('/');

            string day2 = arrDate2[0].ToString();
            string month2 = arrDate2[1].ToString();
            string year2 = arrDate2[2].ToString();

            var id_cluster = 0;

            if (Request.Form["id_cluster"] != "" && Request.Form["id_cluster"] != ",")
            {
                id_cluster = Convert.ToInt32(Request.Form["id_cluster"]);
            }

            var id_unidade = 0;

            if (Request.Form["id_unidade"] != "" && Request.Form["id_unidade"] != ",")
            {
                id_unidade = Convert.ToInt32(Request.Form["id_unidade"]);
            }


            var sql = "SELECT DISTINCT ativacao.* FROM ativacao INNER JOIN cliente_SAP ON ativacao.ZCLIENTE = cliente_SAP.ZCLIENTE INNER JOIN cliente_cluster_SAP ON cliente_SAP.ZCLIENTE = cliente_cluster_SAP.ZCLIENTE WHERE 1=1";

            if (id_unidade > 0)
            {
                sql = sql + " AND cliente_SAP.ZUNIDAD = " + id_unidade;
            }

            if (id_cluster > 0)
            {
                sql = sql + " AND cliente_cluster_SAP.ZCODCLUST = " + id_cluster;
            }

            if (datainicial != null || datafinal != null)
            {
                sql = sql + " AND ativacao.criacao BETWEEN  '" + year + "-" + month + "-" + day + "' AND '" + year2 + "-" + month2 + "-" + day2 + "'";
            }

            DataTable dt = new DataTable();

            dt.Columns.Add("Franquia", typeof(string));
            dt.Columns.Add("Unidade", typeof(string));
            dt.Columns.Add("Código_Unidade", typeof(string));
            dt.Columns.Add("UF", typeof(string));
            dt.Columns.Add("Cluster", typeof(string));
            dt.Columns.Add("Fornecedor", typeof(string));
            dt.Columns.Add("Quantidade_PDV", typeof(string));
            dt.Columns.Add("Valor_Cluster", typeof(string));
            dt.Columns.Add("Total_Peças", typeof(string));
            dt.Columns.Add("Economia", typeof(string));
            dt.Columns.Add("Media_Gasta", typeof(string));

            var itemAtivacao = db.ativacao.SqlQuery(sql).ToList();

            foreach (var item in itemAtivacao)
            {
                var id_franquia = "";
                if (item.cliente_SAP.id_franquia != null)
                {
                    id_franquia = item.cliente_SAP.franquia.descricao.ToString();
                }

                var id_unidade2 = "";
                if (item.cliente_SAP.ZUNIDAD != null)
                {
                    id_unidade2 = item.cliente_SAP.unidade_SAP.ZNOMBRE.ToString();
                }

                var codunida = "";
                if (item.cliente_SAP.ZUNIDAD != null)
                {
                    codunida = item.cliente_SAP.unidade_SAP.ZCODUNIDA.ToString();
                }

                var uf_sigla = "";
                if (item.cliente_SAP.ZUF != null)
                {
                    uf_sigla = item.cliente_SAP.ZUF.ToString();
                }

                var itemcluster = db.cliente_cluster_SAP.Where(a => a.ZCLIENTE == item.ZCLIENTE).FirstOrDefault();
                var cluster = "";
                if (itemcluster.cluster_SAP.ZNOMBRE != null)
                {
                    cluster = itemcluster.cluster_SAP.ZNOMBRE.ToString();
                }

                var itemquantidade = db.cliente_cluster_SAP.Where(a => a.ZCLIENTE == item.ZCLIENTE).Count();

                var pvd = itemquantidade.ToString();

                var fornecedor = "";
                if (item.cliente_SAP.fornecedor != null)
                {
                    fornecedor = item.cliente_SAP.fornecedor.ToString();
                }
                
                var excedente = Convert.ToDecimal(0);
                var tem = Convert.ToDecimal(item.cliente_SAP.custo_budget);
                var gastou = Convert.ToDecimal(item.custo_valor);

                excedente = gastou - tem;

               var valorcluster = Convert.ToDecimal(0);
               if (item.cliente_SAP.custo_budget != null)
               {
                   valorcluster = Convert.ToDecimal(item.cliente_SAP.custo_budget);
               }

               var totalpecas = Convert.ToDecimal(0);
               if (item.custo_valor != null)
               {
                   totalpecas = Convert.ToDecimal(item.custo_valor);
               }

               var economia = Convert.ToDecimal(0);
               economia = valorcluster - totalpecas;

               var itemquantidadepeca = db.ativacao_peca.Where(a => a.id_ativacao == item.id).Count();



               var mediagasta = Convert.ToDecimal(0);

               if (itemquantidadepeca != 0)
               {
                   mediagasta = itemquantidade / itemquantidadepeca;                   
               }
                dt.Rows.Add(
                    id_franquia,
                    id_unidade2,
                    codunida,
                    uf_sigla,
                    cluster,
                    fornecedor,
                    pvd,
                    valorcluster,
                    totalpecas,
                    economia,
                    mediagasta);	                
            }

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", "attachment;filename=relatorios.xls");
            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteExcelWithNPOI(dt, "xlsx", 7);
            //Response.End();

            return View("Relatorios/RelatorioVerbas"); //left join ou right join
        }



        /* The Magic */

        public void WriteExcelWithNPOI(DataTable dt, String extension, int? tipo)
        {
            IWorkbook workbook;

            if (extension == "xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (extension == "xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                throw new Exception("This format is not supported");
            }

            ISheet sheet1 = workbook.CreateSheet("Sheet 1");

            //make a header row
            IRow row1 = sheet1.CreateRow(0);

            for (int j = 0; j < dt.Columns.Count; j++)
            {
                ICell cell = row1.CreateCell(j);
                String columnName = dt.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                }
            }

            using (var exportData = new MemoryStream())
            {

                var nome = "";
                var nome2 = "";

                if (tipo == 1)
                {
                    nome = "Relatorio-Ativação.xlsx";
                    nome2 = "Relatorio-Ativação.xls";
                }
                else if (tipo == 2)
                {
                    nome = "Relatorio-PeçasCliente.xlsx";
                    nome2 = "Relatorio-PeçasCliente.xls";
                }
                else if (tipo == 3)
                {
                    nome = "Relatorio-MonitoramentoRevisitas.xlsx";
                    nome2 = "Relatorio-MonitoramentoRevisitas.xls";
                }
                else if (tipo == 4)
                {
                    nome = "Relatorio-Recusas.xlsx";
                    nome2 = "Relatorio-Recusas.xls";
                }
                else if (tipo == 5)
                {
                    nome = "Relatorio-Extrapolações.xlsx";
                    nome2 = "Relatorio-Extrapolações.xls";
                }
                else if (tipo == 6)
                {
                    nome = "Relatorio-PeçasUnidade.xlsx";
                    nome2 = "Relatorio-PeçasUnidade.xls";
                }
                else if (tipo == 7)
                {
                    nome = "Relatorio-Verbas.xlsx";
                    nome2 = "Relatorio-Verbas.xls";
                }

                Response.Clear();
                workbook.Write(exportData);
                if (extension == "xlsx") //xlsx file format
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", nome));
                    Response.BinaryWrite(exportData.ToArray());
                }
                else if (extension == "xls")  //xls file format
                {
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", nome2));
                    Response.BinaryWrite(exportData.GetBuffer());
                }
                Response.End();
            }
        }

        public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.DisplayName); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.Write(prop.Converter.ConvertToString(
                         prop.GetValue(item)));
                    output.Write("\t");
                }
                output.WriteLine();
            }
        }
    }
}
