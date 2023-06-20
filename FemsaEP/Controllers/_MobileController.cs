using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FemsaEP.Models;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net.Http;


namespace FemsaEP.Controllers
{

    public class MobileController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        public ActionResult loginMobile(string usuario, string senha)
        {
            if (usuario == "" || senha == "")
            {
                return Json(new { status = 0, msg = "Login e senha precisam ser preenchidos." });
            }

            var rsAdmin = db.colaborador_SAP.Where(colaborador => colaborador.ZLOGIN == usuario).Where(colaborador => colaborador.ZCONTRASE == senha).FirstOrDefault();

            if (rsAdmin != null)
            {
                return Json(new
                {
                    status = 1,
                    dados = new
                    {
                        id = rsAdmin.ZCODCOLAB.ToString(),
                        nome = rsAdmin.ZNOMBRE == null ? "" : rsAdmin.ZNOMBRE.ToString(),
                        acessooffline = rsAdmin.acessoffline == null ? "" : rsAdmin.acessoffline.ToString(),
                        ultimosinc = rsAdmin.ultimosinc == null ? "" : rsAdmin.ultimosinc.ToString(),
                        foto = rsAdmin.foto == null ? "" : rsAdmin.foto.ToString()

                    }
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {

                return Json(new { status = 0, msg = "Login ou senha incorretos" }, JsonRequestBehavior.AllowGet);

            }

        }


        public ActionResult dadosColaboradorMobile(int? id_colaborador)
        {
            var dadosColaborador = db.colaborador_SAP.FirstOrDefault(c => c.ZCODCOLAB == id_colaborador);
            if (dadosColaborador == null)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            return Json(
                new
                {
                    @id = dadosColaborador.ZCODCOLAB.ToString(),
                    @mandante = dadosColaborador.MANDT == null ? "" : dadosColaborador.MANDT.ToString(),
                    @codigo = dadosColaborador.ZCODCOLAB.ToString(),
                    @descricao = dadosColaborador.descricao == null ? "" : dadosColaborador.descricao.ToString(),
                    @nome = dadosColaborador.ZNOMBRE == null ? "" : dadosColaborador.ZNOMBRE.ToString(),
                    @login = dadosColaborador.ZLOGIN == null ? "" : dadosColaborador.ZLOGIN.ToString(),
                    @email = dadosColaborador.ZEMAIL == null ? "" : dadosColaborador.ZEMAIL.ToString(),
                    @telefone = dadosColaborador.ZTELEFONO == null ? "" : dadosColaborador.ZTELEFONO.ToString(),
                    @id_territorio = dadosColaborador.ZTERRITOR == null ? "" : dadosColaborador.ZTERRITOR.ToString(),
                    @id_unidade = dadosColaborador.ZUNIDAD == null ? "" : dadosColaborador.ZUNIDAD.ToString(),
                    @numero_patrimonio = dadosColaborador.ZNUMPATRI == null ? "" : dadosColaborador.ZNUMPATRI.ToString(),
                    @numero_serie_camera = dadosColaborador.ZNUMSERIE == null ? "" : dadosColaborador.ZNUMSERIE.ToString(),
                    @id_grupos = dadosColaborador.ZGRUPO == null ? "" : dadosColaborador.ZGRUPO.ToString(),
                    @desativado = dadosColaborador.ZDESACTIV == null ? "" : dadosColaborador.ZDESACTIV.ToString(),
                    @funcao = dadosColaborador.ZFUNCARGO == null ? "" : dadosColaborador.ZFUNCARGO.ToString(),
                    @id_usuario = dadosColaborador.id_usuario == null ? "" : dadosColaborador.id_usuario.ToString(),
                    @id_colaborador = dadosColaborador.id_colaborador == null ? "" : dadosColaborador.id_colaborador.ToString(),
                    @foto = dadosColaborador.foto == null ? "" : dadosColaborador.foto.ToString()
                }
                , JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaClientesMobile(int? id)
        {
            var list = new List<dynamic>();

            if (id == null)
            {
                return Json(new { status = 0, msg = "Id do prospector não foi definido" }, JsonRequestBehavior.AllowGet);
            }



            var rsAtivacoes = db.cliente_SAP.Where(cliente => cliente.id_colaborador == id && cliente.status_ativacao != 2).OrderBy(c => c.ZNOMBRE).OrderBy(c => c.status_ativacao);

            if (rsAtivacoes.Count() == 0)
            {
                return Json(new { status = 0, list = list }, JsonRequestBehavior.AllowGet);
            }

            if (rsAtivacoes != null)
            {


                foreach (var item in rsAtivacoes)
                {
                    list.Add(new
                    {
                        @id = item.ZCLIENTE,
                        @clienteid = item.ZCLIENTE,
                        @cliente = item.ZNOMBRE == null ? "" : item.ZNOMBRE.ToString(),
                        @clienteendereco = item.ZBARRIO == null ? "" : item.ZBARRIO.ToString(),
                        @clientenumero = item.numero == null ? "" : item.numero.ToString(),

                        @endereco1 = item.ZDIRECCIO == null ? "" : item.ZDIRECCIO.ToString().Trim(),
                        @bairro1 = item.ZBARRIO == null ? "" : item.ZBARRIO.ToString().Trim(),
                        @cidade1 = item.ZCIUDAD == null ? "" : item.ZCIUDAD.ToString().Trim(),


                        @clientebairro = item.ZBARRIO == null ? "" : item.ZBARRIO.ToString(),
                        @clientecidade = item.ZCIUDAD == null ? "" : item.ZCIUDAD.ToString(),
                        @clientecnpj = item.ZCNPJCPFI == null ? "" : item.ZCNPJCPFI.ToString(),
                        @clienteprimeiro_telefone = item.ZTELEFONO == null ? "" : item.ZTELEFONO.ToString(),
                        @status_ativacao_nome = item.status_ativacao1 == null ? "" : item.status_ativacao1.nome.ToString(),
                        @status_ativacao = item.status_ativacao1 == null ? "" : item.status_ativacao1.id.ToString(),
                        @ativacao_ultima_atualizacao = item.ativacao_ultima_atualizacao == null ? "" : item.ativacao_ultima_atualizacao.ToString()
                    });
                }

                return Json(new { status = 1, list = list }, JsonRequestBehavior.AllowGet);
            }

            else
            {

                return Json(new { status = 0, msg = "Nenhuma ativação encontrada" }, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult ativacaoMobile(int id)
        {
            var itemAtivacao = db.ativacao.Where(ativacao => ativacao.id_cliente == id);

            // return Json(new { teste = itemAtivacao.cliente_SAP.id }, JsonRequestBehavior.AllowGet);

            if (itemAtivacao.Count() == 0)
            {
                return Json(new { status = 0, msg = "Nenhuma ativação encontrada." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var list = new List<dynamic>();
                var fotos = new List<dynamic>();
                var msgs = new List<dynamic>();
                foreach (var item in itemAtivacao)
                {
                    list.Add(new
                    {
                        @idativacao = item.id,
                        @cliente_id = item.cliente_SAP.ZCLIENTE == null ? "" : item.cliente_SAP.ZCLIENTE.ToString(),
                        @cliente_codigo = item.cliente_SAP.ZCLIENTE == null ? "" : item.cliente_SAP.ZCLIENTE.ToString(),
                        @custo_valor = item.custo_valor == null ? "" : item.custo_valor.ToString(),
                        @descricao = item.descricao == null ? "" : item.descricao.ToString(),
                        @cliente_nome = item.cliente_SAP.ZNOMBRE == null ? "" : item.cliente_SAP.ZNOMBRE.ToString(),
                        @cliente_endereco = item.cliente_SAP.ZLOCALIZA == null ? "" : item.cliente_SAP.ZLOCALIZA.ToString(),
                        @cliente_telefone = item.cliente_SAP.ZTELEFONO == null ? "" : item.cliente_SAP.ZTELEFONO.ToString(),
                        @cliente_unidade = item.cliente_SAP.unidade_SAP == null ? "" : item.cliente_SAP.unidade_SAP.ZNOMBRE.ToString(),
                        @cliente_referencia = item.cliente_SAP.ZPUNTOREF == null ? "" : item.cliente_SAP.ZPUNTOREF.ToString(),
                        //@cliente_cluster = item.cliente_SAP.cluster.nome,
                        //@cliente_contato = item.cliente_SAP.
                    });

                    foreach (var msg in item.ativacao_mensagem)
                    {
                        msgs.Add(new
                        {

                            @id_ativacao = msg.id_ativacao,
                            @mensagem = msg.mensagem,
                            @id = msg.id

                        });

                    }

                    foreach (var foto in item.ativacao_foto)
                    {
                        fotos.Add(new
                        {
                            @foto_url = foto.url_foto,
                            @id_ativacao_foto = foto.id
                        });
                    }
                }


                return Json(new { status = 1, msgs = msgs, ativ = list, fotos = fotos }, JsonRequestBehavior.AllowGet);
            }



        }

        public ActionResult listaPecasMobile(int? id_ativacao, string id_cliente)
        {

            //traz cluster do cliente
            var id_cliente2 = id_cliente.ToString();
            var cluster_cliente = db.cliente_cluster_SAP.Where(cc => cc.ZCLIENTE == id_cliente2);

            var list = new List<dynamic>();

            foreach (var item in cluster_cliente)
            {
                var rsPeca = db.cluster_peca_SAP.Where(c => c.ZCODCLUST == item.ZCODCLUSTER);

                foreach (var item2 in rsPeca)
                {
                    var pecaativ = db.ativacao_peca.Where(ativacao_peca => ativacao_peca.id_ativacao == id_ativacao && ativacao_peca.ZCODPECA == item2.ZCODPECA).Count();
                    var peca = db.caracteristica_de_peca_SAP.FirstOrDefault(p=>p.ZCODIPECA == item2.peca_SAP.ZCODPECA);
                    //return Json(new { pecaativ = pecaativ, id_ativacao = id_ativacao, id_cliente=id_cliente, id=item2.id }, JsonRequestBehavior.AllowGet);

                    list.Add(new
                    {
                        @peca_codigo = item2.peca_SAP.ZCODPECA.ToString(),
                        @peca_descricao = item2.peca_SAP.descricao == null ? "" : item2.peca_SAP.descricao.ToString(),
                        @peca_id = item2.peca_SAP.ZCODPECA.ToString(),
                        @peca_nome = item2.peca_SAP.ZNOMPECA == null ? "" : item2.peca_SAP.ZNOMPECA.ToString(),
                        @peca_desativado = item2.peca_SAP.ZDESACTIV == null ? "" : item2.peca_SAP.ZDESACTIV.ToString(),
                        @mandt = peca.MANDT == null ? "" : peca.MANDT.ToString(),
                        @peca_codpeca = peca.ZCODPIEZA.ToString(),
                        @peca_codmarca = peca.ZCODMARCA == null ? "" : peca.ZCODMARCA.ToString(),
                        @peca_codembalagem = peca.ZCODEMBAL == null ? "" : peca.ZCODEMBAL.ToString(),
                        @peca_codalimento = peca.ZCODALIME == null ? "" : peca.ZCODALIME.ToString(),
                        @peca_codimagem = peca.ZCODIMAGEN == null ? "" : peca.ZCODIMAGEN.ToString(),
                        @peca_detalhe = peca.ZDETALLE == null ? "" : peca.ZDETALLE.ToString(),
                        @peca_kits = peca.ZCODKITS == null ? "" : peca.ZCODKITS.ToString(),
                        @peca_formacusto = peca.ZFORMCUS == null ? "" : peca.ZFORMCUS.ToString(),
                        @peca_custom2 = peca.ZCOSTOM2 == null ? "" : peca.ZCOSTOM2.ToString(),
                        @peca_ancho = peca.ZANCHO == null ? "" : peca.ZANCHO.ToString(),
                        @peca_altura = peca.ZALTURA == null ? "" : peca.ZALTURA.ToString(),
                        @peca_diametro = peca.ZDIAMETRO == null ? "" : peca.ZDIAMETRO.ToString(),
                        @peca_provedor = peca.ZPROVEEDO == null ? "" : peca.ZPROVEEDO.ToString(),
                        @peca_valorpza = peca.ZVALORPZA == null ? "" : peca.ZVALORPZA.ToString(),
                        @desativado = peca.ZDESACTIV == null ? "" : peca.ZDESACTIV.ToString(),
                        @peca_valor = peca.ZVALORPZA == null ? "" : peca.ZVALORPZA.ToString().Replace(",", "."),
                        @peca_foto = peca.foto_url == null ? "" : peca.foto_url.ToString(),


                    });
                }
            }


            var jsonResult = Json(new { status = 1, list = list.Distinct() }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            // return Json(new { status = 1, list = list.Distinct() }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult informacoesAtivacaoMobile(string id_cliente)
        {
            var list = new List<dynamic>();
            var id_cliente2 = id_cliente.ToString();
            //var id_cliente2 = id_cliente.ToString();
            var ativacao_info = db.ativacao.FirstOrDefault(a => a.ZCLIENTE == id_cliente2);

            if (ativacao_info == null)
            {
                var cliente = db.cliente_SAP.FirstOrDefault(c => c.ZCLIENTE == id_cliente2);

                list.Add(new
                {
                    id = cliente.ZCLIENTE,
                    status = cliente.status_ativacao,
                    motivotxt = cliente.motivotxt,
                    motivofoto = cliente.recusa_foto == null ? "" : cliente.recusa_foto.ToString()

                });

                return Json(new { info = list }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                list.Add(new
                {
                    id = ativacao_info.id,
                    codigo = ativacao_info.codigo == null ? "" : ativacao_info.codigo.ToString(),
                    custo_valor = ativacao_info.custo_valor == null ? "" : ativacao_info.custo_valor.ToString().Replace(",", "."),
                    descricao = ativacao_info.descricao == null ? "" : ativacao_info.descricao.ToString(),
                    id_cliente = ativacao_info.id_cliente == null ? "" : ativacao_info.id_cliente.ToString(),
                    status = ativacao_info.status == null ? "" : ativacao_info.status.ToString(),
                    id_projeto = ativacao_info.id_projeto == null ? "" : ativacao_info.id_projeto.ToString(),
                    situacao = ativacao_info.situacao == null ? "" : ativacao_info.situacao.ToString(),
                    executivo = ativacao_info.executivo,
                    motivotxt = ativacao_info.cliente_SAP.motivotxt == null ? "" : ativacao_info.cliente_SAP.motivotxt.ToString(),
                    motivofoto = ativacao_info.cliente_SAP.recusa_foto == null ? "" : ativacao_info.cliente_SAP.recusa_foto.ToString(),
                    carta_foto = ativacao_info.cliente_SAP.carta_foto == null ? "" : ativacao_info.cliente_SAP.carta_foto.ToString()
                });


                return Json(new { info = list }, JsonRequestBehavior.AllowGet);
            }
                        
        }

        public ActionResult listaContrapartidasMobile()
        {
            var rsContrapartida = db.contrapartida_SAP;

            var list = new List<dynamic>();

            foreach (var item in rsContrapartida)
            {
                list.Add(new
                {
                    @contra_id = item.ZCODCONPA,
                    @contra_nome = item.ZNOMCONPA
                });
            }

            return Json(new { status = 1, contrapartida = list }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult procuraPecaMobile(String query, int? id_ativacao, string id_cliente)
        {
            // 
            var list = new List<dynamic>();
            var cc = db.cliente_cluster_SAP.Where(c=>c.ZCLIENTE == id_cliente);


            foreach (var item in cc)
            {
                var rspeca = db.cluster_peca_SAP.Where(p=>p.ZCODCLUST == item.ZCODCLUSTER && p.peca_SAP.ZNOMPECA.Contains(query));

                foreach (var peca in rspeca)
                {
                    var pecaativ = db.ativacao_peca.Where(ativacao_peca => ativacao_peca.id_ativacao == id_ativacao && ativacao_peca.ZCODPECA == peca.peca_SAP.ZCODPECA).Count();
                    var pecainfo = db.caracteristica_de_peca_SAP.FirstOrDefault(i=>i.ZCODIPECA == peca.ZCODPECA);
                    list.Add(new
                    {
                        @peca_embalagem = pecainfo.embalagem_SAP == null ? "" : pecainfo.embalagem_SAP.ZNOMEMBAL,
                        @peca_alimento = pecainfo.alimento_SAP == null ? "" : pecainfo.alimento_SAP.ZNOMBREALIMENTO,
                        @peca_descricao = pecainfo.peca_SAP.ZNOMPECA == null ? "" : pecainfo.peca_SAP.ZNOMPECA.ToString().Trim(),
                        @peca_nome = pecainfo.peca_SAP.ZNOMPECA == null ? "" : pecainfo.peca_SAP.ZNOMPECA.ToString().Trim(),
                        @peca_id = pecainfo.ZCODIPECA == null ? "" : pecainfo.ZCODIPECA.ToString(),
                        @peca_desativado = pecainfo.ZDESACTIV == null ? "" : pecainfo.ZDESACTIV.ToString(),
                        @peca_valor = pecainfo.ZVALORPZA == null ? "" : pecainfo.ZVALORPZA.ToString().Replace(",", "."),
                        @peca_foto = pecainfo.foto_url == null ? "" : pecainfo.foto_url.ToString(),
                        @peca_altura = pecainfo.ZALTURA == null ? "" : pecainfo.ZALTURA.ToString(),
                        @peca_diametro = pecainfo.ZDIAMETRO == null ? "" : pecainfo.ZDIAMETRO.ToString(),
                        /*@peca_ocasiao = item2.peca.ocasiao,
                        @peca_atributo = item2.peca.atributo,
                        @peca_tamanho = item2.peca.tamanho,
                        @peca_largura = item2.peca.largura,
                       
                        @peca_detalhe = item2.peca.detalhe,
                        @peca_texto_inserido = item2.peca.texto_inserido,
                        @peca_lona_instalada = item2.peca.lona_instalada,
                        @peca_codigo = peca.peca_SAP.ZCODPECA,*/
                        @verificar = pecaativ
                    });
                }
            }

            return Json(new { status = 1, peca = list.Distinct() }, JsonRequestBehavior.AllowGet);



            // começo teste procura por cluster
            /*var id_cliente2 = id_cliente.ToString();
            var cluster_cliente = db.cliente_cluster_SAP.Where(cc2 => cc2.ZCLIENTE == id_cliente2);


            foreach (var item in cluster_cliente)
            {
                var rsPeca = db.cluster_peca_SAP.Where(c => c.ZCODCLUST == item.ZCODCLUSTER && c.peca_SAP.ZNOMPECA.Contains(query));

                foreach (var item2 in rsPeca)
                {
                    var pecaativ = db.ativacao_peca.Where(ativacao_peca => ativacao_peca.id_ativacao == id_ativacao && ativacao_peca.ZCODPECA == item2.peca_SAP.ZCODPECA).Count();


                    list.Add(new
                    {
                        @peca_codigo = item2.peca_SAP.ZCODPECA,
                        //CARACTERISTICA DE PECA
                    });
                }
            }

            return Json(new { status = 1, peca = list }, JsonRequestBehavior.AllowGet);*/




            //fim teste procura cluster






            // começo original
            /* var rsPecas = db.peca.Where(peca => peca.nome.Contains(query));


             var list = new List<dynamic>();

             foreach (var item in rsPecas)
             {

    
                 var pecaativ = db.ativacao_peca.Where(ativacao_peca => ativacao_peca.id_ativacao == id_ativacao && ativacao_peca.ZCODPECA == item.id).Count();




                 list.Add(new
                 {
                     @peca_codigo = item.codigo,
                     @peca_descricao = item.descricao,
                     @peca_id = item.id,
                     @peca_nome = item.nome,
                     @peca_desativado = item.desativado,
                     @peca_embalagem = item.embalagem,
                     @peca_alimento = item.alimento,
                     @peca_ocasiao = item.ocasiao,
                     @peca_atributo = item.atributo,
                     @peca_tamanho = item.tamanho,
                     @peca_largura = item.largura,
                     @peca_altura = item.altura,
                     @peca_diametro = item.diametro,
                     @peca_valor = item.valor,
                     @peca_detalhe = item.detalhe,
                     @peca_texto_inserido = item.texto_inserido,
                     @peca_lona_instalada = item.lona_instalada,
                     @peca_foto = item.foto_url,
                     @verificar = pecaativ


                 });


             }


             return Json(new { status = 1, peca = list }, JsonRequestBehavior.AllowGet);

             //fim original*/


        }

        public ActionResult valorTotalAtivacaoMobile(int? ZCLIENTE)
        {
            var ZCLIENTE2 = ZCLIENTE.ToString();
            var dadosativacao = db.ativacao.FirstOrDefault(ativacao => ativacao.ZCLIENTE == ZCLIENTE2);

            return Json(new { dadosativacao = dadosativacao.custo_valor }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult dadosClienteMobile(string id)
        {
            var projetos = new List<dynamic>();
            var clusters = new List<dynamic>();
            var list = new List<dynamic>();
            var id2 = id.ToString();
            var rsCliente = db.cliente_SAP.FirstOrDefault(cliente => cliente.ZCLIENTE == id2);

            if (rsCliente == null)
            {
                return Json(new { status = 0, msg = "Usuário não encontrado" }, JsonRequestBehavior.AllowGet);
                
            }

            if (rsCliente.status_ativacao != 2)
            {
                var rsProjetos = db.cliente_projeto.Where(p => p.ZCLIENTE == rsCliente.ZCLIENTE);

                foreach (var item in rsProjetos)
                {
                    projetos.Add(new
                    {
                        @projeto_nome = item.projeto.descricao == null ? "" : item.projeto.descricao.ToString()
                    });
                }
            }

            var rsCluster = db.cliente_cluster_SAP.Where(c=>c.ZCLIENTE == rsCliente.ZCLIENTE);
            foreach (var clu in rsCluster)
            {
                clusters.Add(new
                {
                    @cluster_nome = clu.cluster_SAP.ZNOMBRE == null ? "" : clu.cluster_SAP.ZNOMBRE.ToString()
                });
            }

            if (rsCliente != null)
            {


                return Json(new
                {
                    // dadoscliente = Json(new {
                    @id = rsCliente.ZCLIENTE.ToString(),
                    @mandante = rsCliente.MANDT == null ? "" : rsCliente.MANDT.ToString().Trim(),
                    @codigo = rsCliente.ZCLIENTE == null ? "" : rsCliente.ZCLIENTE.ToString().Trim(),
                    @razao_social = rsCliente.ZNOMFANTA == null ? "" : rsCliente.ZNOMFANTA.ToString().Trim(),
                    @cidade = rsCliente.ZCIUDAD == null ? "" : rsCliente.ZCIUDAD.ToString().Trim(),
                    @id_rota = rsCliente.ZRUTA == null ? "" : rsCliente.ZRUTA.ToString().Trim(),
                    @nome = rsCliente.ZNOMBRE == null ? "" : rsCliente.ZNOMBRE.ToString().Trim(),
                    @cnpj = rsCliente.ZCNPJCPFI == null ? "" : rsCliente.ZCNPJCPFI.ToString().Trim(),
                    @cep = rsCliente.ZCEP == null ? "" : rsCliente.ZCEP.ToString().Trim(),
                    @endereco = rsCliente.ZDIRECCIO == null ? "" : rsCliente.ZDIRECCIO.ToString().Trim(),
                    @bairro = rsCliente.ZBARRIO == null ? "" : rsCliente.ZBARRIO.ToString().Trim(),
                    @local = rsCliente.ZLOCALIZA == null ? "" : rsCliente.ZLOCALIZA.ToString().Trim(),
                    @regiao = rsCliente.ZUF == null ? "" : rsCliente.ZUF.ToString().Trim(),
                    @ponto_referencia = rsCliente.ZPUNTOREF == null ? "" : rsCliente.ZPUNTOREF.ToString().Trim(),
                    @canal = rsCliente.ZCANAL == null ? "" : rsCliente.ZCANAL.ToString().Trim(),
                    @regional = rsCliente.regional == null ? "" : rsCliente.regional.ToString().Trim(),
                    @numero = rsCliente.numero == null ? "" : rsCliente.numero.ToString().Trim(),
                    @status_nome = rsCliente.status_ativacao1 == null ? "" : rsCliente.status_ativacao1.nome.ToString().Trim(),
                    @status_id = rsCliente.status_ativacao == null ? "" : rsCliente.status_ativacao.ToString().Trim(),
                    @budget_cliente = rsCliente.custo_budget == null ? "" : rsCliente.custo_budget.ToString().Replace(",", "."),
                    @latitude = rsCliente.lattext == null ? "" : rsCliente.lattext.ToString().Trim(),
                    @longitude = rsCliente.longtext == null ? "" : rsCliente.longtext.ToString().Trim(),
                    @observacao = rsCliente.observacao == null ? "" : rsCliente.observacao.ToString().Trim(),
                    @cluster = clusters.Distinct(),
                    @primeiro_telefone = rsCliente.ZTELEFONO != null ? rsCliente.ZTELEFONO.ToString() : "",
                    @projetos = projetos.Distinct()
                    //   });
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {

                return Json(new { status = 0, msg = "Usuário não encontrado" }, JsonRequestBehavior.AllowGet);

            }


        }

        public ActionResult salvaPecaAtivacaoMobile([Bind(Include = "id,id_ativacao,ZCODPECA, quantidade")] ativacao_peca ativacao_peca)
        {
            var checa = db.ativacao_peca.FirstOrDefault(atv => (atv.id_ativacao == ativacao_peca.id_ativacao && atv.ZCODPECA == ativacao_peca.ZCODPECA));

            if (checa == null)
            {
                ativacao_peca.quantidade = 1;
                db.ativacao_peca.Add(ativacao_peca);
                db.SaveChanges();

                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { status = 0, msg = "Este item já está vinculado à esta ativacão." }, JsonRequestBehavior.AllowGet);
            }


        }

        public bool pendenteretorno(string id_cliente, string motivo)
        {
            var status = true;

            var cliente = db.cliente_SAP.FirstOrDefault(c=>c.ZCLIENTE == id_cliente);

            if (cliente == null)
            {
                return status = false;
            }
            cliente.motivo_retorno = motivo;
            cliente.pendente_retorno = 1;
            db.SaveChanges();

            return status;
        }

        public ActionResult listaPecasAtivacaoMobile(int? id_ativacao)
        {
            var pecas = db.ativacao_peca.Where(ativacao_peca => ativacao_peca.id_ativacao == id_ativacao);

            var lista = new List<dynamic>();

            foreach (var item in pecas)
            {
                var pecainfo = db.caracteristica_de_peca_SAP.FirstOrDefault(p=>p.ZCODIPECA == item.peca_SAP.ZCODPECA);
                // CARACTERISTICA DE PECA
                lista.Add(new
                {
                    @quantidade = item.quantidade,
                    @peca_id = item.peca_SAP.ZCODPECA,
                    @peca_descricao = item.peca_SAP.descricao,
                    @peca_valor = pecainfo.ZVALORPZA.ToString().Replace(",", "."),
                    @peca_diametro = pecainfo.ZDIAMETRO,
                    @peca_altura = pecainfo.ZALTURA,
                    @peca_nome = item.peca_SAP.ZNOMPECA,
                    /*
                    @peca_foto = item.peca_SAP.foto_url,
                     */
                });

            }

            return Json(new { status = 1, pecas = lista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult fotosAtivacaoMobile(int? id_ativacao, int? etapa)
        {
            var list = new List<dynamic>();

            if (id_ativacao != null)
            {
                var serv = db.ativacao_peca.Where(at => (at.id_ativacao == id_ativacao));

                if (serv != null)
                {
                    foreach (var item in serv)
                    {
                        var foto = "";
                        var serv2 = db.ativacao_foto.Where(at => (at.id_ativacao == item.ativacao.id && at.etapa == etapa && at.id_peca == item.ZCODPECA));


                        if (serv2 != null && serv2.Count() >= 1)
                        {
                            
                            foreach (var item2 in serv2)
                            {
                                foto = item2.url_foto;
                            }
                          
                        }




                        list.Add(new
                        {
                            // CARACTERISTICA DE PECA
                            @peca_codigo = item.peca_SAP.ZCODPECA,
                            @peca_nome = item.peca_SAP.ZNOMPECA,
                            @peca_id = item.peca_SAP.ZCODPECA,
                            /*@peca_valor = item.peca_SAP.valor,
                            @peca_detalhe = item.peca_SAP.detalhe,*/
                            @peca_descricao = item.peca_SAP.descricao,
                            @id = item.id,
                            @ativacao = item.ativacao.id,
                            @foto = foto,
                            @etapa = etapa/**/


                        });
                    }

                }

            }

            
          

            return Json(new { serv = list }, JsonRequestBehavior.AllowGet);
            




        }

        public ActionResult criaAtivacaoMobile([Bind(Include = "ZCLIENTE, id_projeto")] ativacao atv)
        {

            if (atv.ZCLIENTE != null)
            {
                var checa = db.ativacao.FirstOrDefault(ativacao => ativacao.ZCLIENTE == atv.ZCLIENTE);
                if (checa != null)
                {
                    db.ativacao.Remove(checa);
                    db.SaveChanges();
                    //return Json(new { status = 0, mensagem = "Este cliente já possui uma ativação." }, JsonRequestBehavior.AllowGet);
                }


                var dateAndTime = DateTime.Now;
                var date = dateAndTime.Date;

                atv.criacao = date;
                atv.situacao = "P";
                var idcliente = atv.id_cliente.ToString();
                var clienteinfo = db.cliente_SAP.FirstOrDefault(c => c.ZCLIENTE == idcliente);
                //atv.descricao = clienteinfo.ZNOMBRE;
                atv.executivo = 0;
                //atv.id_projeto = atv.id_projeto;
                db.ativacao.Add(atv);
                db.SaveChanges();


                int id = atv.id;

                return Json(new { status = 1, idativacao = id }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = 0, msg = "Cliente não especificado" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult alteraStatusAtivacaoMobile(string id_cliente, int? id_status, int? motivo_id, string motivotxt)
        {

            if (id_cliente != null)
            {
                var id2 = id_cliente.ToString();
                var clienteinfo = db.cliente_SAP.Single(cliente => cliente.ZCLIENTE == id2);
                clienteinfo.status_ativacao = id_status;
                clienteinfo.motivo_id = motivo_id;
                if (motivotxt != "")
                {
                    clienteinfo.motivotxt = motivotxt;
                }
                db.SaveChanges();
                return Json(new { status = 1, msg = "Ativação alterada com sucesso" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = 0, msg = "ID da ativação não foi informado." }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult adicionaProjetoAtivacaoMobile(string ZCLIENTE, int? id_projeto)
        {
            var idcliente2 = ZCLIENTE.ToString();
            var atv = db.ativacao.FirstOrDefault(a => a.ZCLIENTE == idcliente2);
            atv.id_projeto = id_projeto;

            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult associaProjetoAtivacaoMobile(int? id_projeto, string ZCLIENTE)
        {

            if (id_projeto != null)
            {
                var ZCLIENTE2 = ZCLIENTE.ToString();
                var projeto = db.ativacao.FirstOrDefault(a => a.ZCLIENTE == ZCLIENTE2);
                var exclui = db.ativacao.SqlQuery("DELETE FROM ativacao_projeto WHERE id_projeto = " + projeto.id_projeto);
            }


            var ativ = new cliente_projeto
            {
                id_projeto = id_projeto,
                ZCLIENTE = ZCLIENTE.ToString()
            };

            db.cliente_projeto.Add(ativ);
            db.SaveChanges();


            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult associaContrapartidaAtivacaoMobile(int? id_contrapartida, int? id_ativacao, int? etapa)
        {


            var ativ = new contrapartida_ativacoes
            {
                ZCODCONPA = id_contrapartida,
                id_ativacao = id_ativacao,
                etapa = etapa
            };

            db.contrapartida_ativacoes.Add(ativ);
            db.SaveChanges();


            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult procuraClienteMobile(string query, int? id_usuario)
        public ActionResult procuraClienteMobile(string query, int? id_usuario, string codigo, string razao_social, string cidade, string proximidade, string bairro, string status, string canal, string lat, string longi)
        {
            //var rsCliente = db.cliente_SAP.Where(cliente => cliente.nome.Contains(query) && cliente.id_colaborador == id_usuario);


            var sqlpagina = "";

            //sqlpagina = sqlpagina + "SELECT * FROM cliente WHERE id_colaborador = " + id_usuario;
            sqlpagina = sqlpagina + "SELECT * ";
            if (proximidade != "" && proximidade != null)
            {
                sqlpagina = sqlpagina + ", ( 6371*1000 * acos( cos( radians(" + lat + ") ) * cos( radians( lattext ) ) * cos( radians( longtext ) - radians(" + longi + ") ) + sin( radians(" + lat + ") ) * sin( radians( lattext ) ) ) ) AS distance ";

            }
            sqlpagina = sqlpagina + " FROM cliente_SAP WHERE id_colaborador = " + id_usuario;
            if (query != "" && query != null)
            {
                sqlpagina = sqlpagina + " AND ZNOMBRE LIKE '%" + query + "%' OR ZCLIENTE = '" + query + "' ";
            }
            if (status != "" && status != null)
            {
                if (status == "1")
                {
                    sqlpagina = sqlpagina + " AND status_ativacao = 2 ";
                }
                else if (status == "2")
                {
                    sqlpagina = sqlpagina + " AND status_ativacao = 8 ";
                }
                else if (status == "3")
                {
                    sqlpagina = sqlpagina + " AND status_ativacao >= 9 ";
                }
            }
            if (cidade != "" && cidade != null)
            {
                sqlpagina = sqlpagina + " AND ZCIUDAD LIKE '%" + cidade + "%'";
            }
            if (razao_social != "" && razao_social != null)
            {
                sqlpagina = sqlpagina + " AND ZNOMFANTA LIKE '%" + razao_social + "%'";
            }

            if (bairro != "" && bairro != null)
            {
                sqlpagina = sqlpagina + " AND ZBARRIO LIKE '%" + bairro + "%'";
            }
            if (codigo != "" && codigo != null)
            {
                sqlpagina = sqlpagina + " AND ZCLIENTE LIKE '%" + codigo + "%'";
            }
            if (canal != "" && canal != null)
            {
                sqlpagina = sqlpagina + " AND ZCANAL = '" + canal + "'";
            }
            if (proximidade != null && proximidade != "")
            {
                sqlpagina = sqlpagina + " AND ( 6371*1000 * acos( cos( radians(" + lat + ") ) * cos( radians( lattext ) ) * cos( radians( longtext ) - radians(" + longi + ") ) + sin( radians(" + lat + ") ) * sin( radians( lattext ) ) ) ) < " + proximidade + " ";
            }
            if ((query != "" && query != null) || (status != "" && status != null) || (cidade != "" && cidade != null) ||
                (razao_social != "" && razao_social != null) || (bairro != "" && bairro != null) || (codigo != "" && codigo != null) ||
                (canal != "" && canal != null) || (proximidade != null && proximidade != ""))
            {
            }
            else
            {
                sqlpagina = sqlpagina + " AND status_ativacao != 2";
            }




            var rscliente = db.cliente_SAP.SqlQuery(sqlpagina).ToList();
            //return Json(rscliente, JsonRequestBehavior.AllowGet);

            var list = new List<dynamic>();

            foreach (var item in rscliente)
            {


                list.Add(new
                {
                    @id = item.ZCLIENTE == null ? "" : item.ZCLIENTE == null ? "" : item.ZCLIENTE.ToString(),
                    @codigo = item.ZCLIENTE == null ? "" : item.ZCLIENTE.ToString(),
                    @clienteid = item.ZCLIENTE == null ? "" : item.ZCLIENTE.ToString(),
                    @cliente = item.ZNOMBRE == null ? "" : item.ZNOMBRE.ToString(),
                    @clientecodigo = item.ZCLIENTE == null ? "" : item.ZCLIENTE.ToString(),
                    @clienteendereco = item.ZDIRECCIO == null ? "" : item.ZDIRECCIO.ToString(),
                    @clientebairro = item.ZBARRIO == null ? "" : item.ZBARRIO.ToString(),
                    @clientecidade = item.ZCIUDAD == null ? "" : item.ZCIUDAD.ToString(),
                    @clientecnpj = item.ZCNPJCPFI == null ? "" : item.ZCNPJCPFI.ToString(),
                    @clienteprimeiro_telefone = item.ZTELEFONO == null ? "" : item.ZTELEFONO.ToString(),
                    @status_ativacao_nome = item.status_ativacao1.nome.ToString(),
                    @status_ativacao = item.status_ativacao1.id.ToString(),
                    @ativacao_ultima_atualizacao = item.ativacao_ultima_atualizacao == null ? "" : item.ativacao_ultima_atualizacao.ToString(),
                    @pendenteretorno = item.pendente_retorno == null ? "" : item.pendente_retorno.ToString()
                });
            }

            return Json(new { status = 1, cliente = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult checaFotoPecaMobile(int? id_peca, int? id_ativacao, int etapa)
        {

            var checa = db.ativacao_foto.FirstOrDefault(ativacao_foto => (ativacao_foto.id_ativacao == id_ativacao && ativacao_foto.id_peca == id_peca && ativacao_foto.etapa == etapa));

            if (checa == null)
            {
                var pecanome = db.peca_SAP.FirstOrDefault(peca => peca.ZCODPECA == id_peca);
                return Json(new { status = false, peca_nome = pecanome.ZNOMPECA, peca_id = pecanome.ZCODPECA }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var peca_id = checa.peca_SAP.ZCODPECA;
                return Json(new { status = true, peca_id = peca_id, peca_nome = checa.peca_SAP.ZNOMPECA}, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult salvarFotoAtivacaoMobile(ativacao_foto ativacao_foto)
        {

            var idativacao = ativacao_foto.id_ativacao;
            var dateAndTime = DateTime.Now;
            var date = dateAndTime.Date;

            ativacao_foto.criacao = date;
            //.criacao = DateTime.Now;
            //ativacao_foto.url_foto = idativacao.ToString();

            var info = db.ativacao.FirstOrDefault(a => a.id == idativacao);



            db.ativacao_foto.Add(ativacao_foto);
            db.SaveChanges();



            // var countativ = db.ativacao_foto.Where(at => at.id_ativacao == ativacao_foto.id_ativacao).Count();
            //var countpeca = db.ativacao_peca.Where(at => at.id_ativacao == ativacao_foto.id_ativacao).Count();




            var countativ = db.ativacao_foto.Where(at => at.id_ativacao == idativacao).Count();
            var countpeca = db.ativacao_peca.Where(at => at.id_ativacao == idativacao).Count();

            if (countativ == countpeca)
            {
                var ativacao = db.ativacao.FirstOrDefault(a => a.id == idativacao);

                var result = db.ativacao.SingleOrDefault(b => b.id == idativacao);
                result.status = 8;
                result.cliente_SAP.status_ativacao = 8;
                db.SaveChanges();

                //var cercaJson = db.ativacao.SqlQuery("UPDATE ativacao SET status = 8 WHERE id = " + idativacao+";");
                //var cercaJson2 = db.ativacao.SqlQuery("UPDATE cliente SET status_ativacao = 8 WHERE id = " + info.cliente_SAP.id + ";");

            }

            //return Json(new { count = countativ.Count()}, JsonRequestBehavior.AllowGet);



            return Json(new { status = idativacao }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult listaMensagensMobile(int? id_ativacao, int? id_usuario, string id_cliente)
        {

            if (id_cliente != null)
            {
                var id_cliente2 = id_cliente.ToString();
                var mensagens = db.mensagem.Where(mensagem => mensagem.ativacao.cliente_SAP.ZCLIENTE == id_cliente2).OrderBy(m => m.hora);

                var msgs = new List<dynamic>();

                foreach (var msg in mensagens)
                {
                    msgs.Add(new
                    {
                        @texto = msg.texto,
                        @administrador_nome = msg.administrador.nome,
                        @mensagem_id = msg.id,
                        @adm_id = msg.administrador.id,
                        @mensagem_titulo = msg.titulo
                    });
                }

                return Json(new { status = 1, mensagens = msgs }, JsonRequestBehavior.AllowGet);
            }

            if (id_ativacao != null)
            {
                var mensagens = db.mensagem.Where(mensagem => mensagem.ativacao.id == id_ativacao);

                var msgs = new List<dynamic>();

                foreach (var msg in mensagens)
                {
                    msgs.Add(new
                    {
                        @texto = msg.texto,
                        @administrador_nome = msg.administrador.nome,
                        @adm_id = msg.administrador.id,
                        @mensagem_id = msg.id,
                        @mensagem_titulo = msg.titulo
                    });
                }

                return Json(new { status = 1, mensagens = msgs }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                var mensagens = db.mensagem.Where(mensagem => mensagem.ZCODCOLAB == id_usuario || (mensagem.ZCODCOLAB == null));

                var msgs = new List<dynamic>();

                foreach (var msg in mensagens)
                {
                    msgs.Add(new
                    {
                        @texto = msg.texto == null ? "" : msg.texto,
                        @administrador_nome = msg.administrador.nome == null ? "" : msg.administrador.nome,
                        @adm_id = msg.administrador.id,
                        @mensagem_id = msg.id,
                        @mensagem_titulo = msg.titulo == null ? "" : msg.titulo
                    });
                }

                return Json(new { status = 1, mensagens = msgs.Distinct() }, JsonRequestBehavior.AllowGet);
            }





            // return Json(new { status = 0, msg = "Nenhuma mensagem foi encontrada."});
        }

        public ActionResult listaChatMobile(int id_mensagem)
        {

            var mensagens = db.chat_mensagem.Where(chat_mensagem => chat_mensagem.mensagem_id == id_mensagem).OrderBy(c => c.hora);
            var lista = new List<dynamic>();
            int count = mensagens.Count();
            foreach (var item in mensagens)
            {
                lista.Add(new
                {
                    @mensagem_id = item.mensagem_id,
                    @usuario_id = item.ZCODCOLAB,
                    @remetente_id = item.remetente_id,
                    @texto = item.texto,
                    @hora = item.hora,
                    @nome_administrador = item.administrador.nome,
                    @tipo = item.tipo
                });


            }

            return Json(new { count = count, mensagens = lista }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult enviaMensagemChatMobile(chat_mensagem chat_mensagem)
        {
            chat_mensagem.hora = DateTime.Now;
            db.chat_mensagem.Add(chat_mensagem);
            db.SaveChanges();
            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult listaEnquetesMobile()
        {
            var pesquisas = db.enquete;
            var list = new List<dynamic>();

            foreach (var item in pesquisas)
            {
                list.Add(new
                {
                    @id = item.id,
                    @titulo = item.titulo,
                    @vigencia = item.vigencia,
                    @data_limite = item.data_limite,
                    @status = item.status,
                    @id_cluster = item.ZCODCLUST,
                    @data_inicio = item.data_inicio
                });
            }


            return Json(new { status = 1, pesquisas = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaPerguntasEnqueteMobile(int? id_enquete, int? id)
        {
            var enquete = db.enquete.FirstOrDefault(e => e.id == id_enquete);
            var infoenquete = new List<dynamic>();

            infoenquete.Add(new
            {
                @id = enquete.id,
                @titulo = enquete.titulo,
                @vigencia = enquete.vigencia,
                @data_limite = enquete.data_limite,
                @status = enquete.status,
                @id_cluster = enquete.ZCODCLUST,
                @data_inicio = enquete.data_inicio
            });


            var perguntas = db.enquete_perguntas.Where(enquete_perguntas => enquete_perguntas.id_enquete == id_enquete);
            var listperguntas = new List<dynamic>();

            foreach (var item in perguntas)
            {
                var verifica = db.enquete_resposta.Where(en => en.id_enquete == enquete.id && en.id_pergunta == item.id && en.ZCODCOLAB == id).Count();

                listperguntas.Add(new
                {
                    @id = item.id,
                    @nome = item.nome,
                    @id_enquete = item.id_enquete,
                    @verifica = verifica
                });
            }


            return Json(new { status = 1, perguntas = listperguntas, enquete = infoenquete }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult respondePerguntaEnqueteMobile(enquete_resposta enquete_resposta)
        {
            db.enquete_resposta.Add(enquete_resposta);
            db.SaveChanges();

            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult checkoutAtivacaoMobile(int? id_ativacao, int? id_status_ativacao, decimal? custo, int? executivo, string motivotxt, string motivofoto)
        {
            //return Json(DateTime.Now, JsonRequestBehavior.AllowGet);

            if (id_ativacao != null)
            {
                var ativacaoinfo = db.ativacao.FirstOrDefault(a => a.id == id_ativacao);
                if (ativacaoinfo != null)
                {

                    if (id_status_ativacao == 6)
                    {

                        ativacaoinfo.cliente_SAP.recusa_foto = motivofoto;
                        ativacaoinfo.situacao = "C";
                        ativacaoinfo.cliente_SAP.motivotxt = "Recusado pelo Executivo de Vendas. Cliente não aceitou as contrapartidas.";

                    }
                    ativacaoinfo.cliente_SAP.status_ativacao = id_status_ativacao;
                    //ativacaoinfo.cliente_SAP.status_ativacao = id_status_ativacao;
                    ativacaoinfo.status = id_status_ativacao;

                    if (id_status_ativacao == 1)
                    {
                        var ativacao = db.ativacao.FirstOrDefault(c => c.ZCLIENTE == ativacaoinfo.ZCLIENTE);
                        ativacao.data_finalizacao = DateTime.Now;
                    }

                    ativacaoinfo.cliente_SAP.ativacao_ultima_atualizacao = DateTime.Now;
                    if (executivo != null)
                    {
                        if (executivo == 1)
                        {
                            ativacaoinfo.executivo = 1;
                        }
                        else
                        {
                            ativacaoinfo.executivo = 0;
                        }
                    }

                    if (custo != null)
                    {
                        ativacaoinfo.custo_valor = custo;
                    }
                    db.SaveChanges();

                    if (id_status_ativacao == 6)
                    {
                        var email = new MailMessage();
                        var message = new MailMessage();
                        var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";

                        message.To.Add(new MailAddress("ops.ga@hotmail.com"));  // replace with valid value 
                        message.From = new MailAddress("rgabrieel@outlook.com");  // replace with valid value
                        message.Subject = "Your email subject";
                        message.Body = string.Format(body, "model.FromName", "model.FromEmail", "model.Message");
                        message.IsBodyHtml = true;

                        using (var smtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = "rgabrieel@outlook.com",  // replace with valid value
                                Password = "L!vedalie"  // replace with valid value
                            };
                            smtp.Credentials = credential;
                            smtp.Host = "smtp-mail.outlook.com";
                            smtp.Port = 587;
                            smtp.EnableSsl = true;
                            smtp.Send(message);
                            //return RedirectToAction("Sent");
                        }
                    }


                    //var total = db.ativacao_peca.Sum();

                    return Json(new { status = 1, msg = "Ativação atualizada com sucesso" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 0, msg = "Nenhum registro encontrado." }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new { status = 0, msg = "ID ativação não foi informado." }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult checaTotalBudgetMobile(int? id_ativacao, decimal total)
        {

            var budget_cliente = db.ativacao.FirstOrDefault(ativacao => ativacao.id == id_ativacao);

            if (budget_cliente.cliente_SAP.custo_budget < total)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
                //return Json(new { status = 0, msg = "Budget total ultrapassa o valor estimado." }, JsonRequestBehavior.AllowGet

            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult pecasAtivacaoMobile(int? id_ativacao)
        {
            if (id_ativacao == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            var rsPecas = db.ativacao_peca.Where(ativacao_peca => ativacao_peca.id_ativacao == id_ativacao);
            var list = new List<dynamic>();
            decimal valor = 0;
            foreach (var item in rsPecas)
            {
                //valor = valor + (item.peca.valor * item.quantidade);
                // CARACTERISTICA DE PECA
                var valorpeca = db.caracteristica_de_peca_SAP.FirstOrDefault(p=>p.ZCODIPECA == item.ZCODPECA);
                valor = valor + Convert.ToDecimal(valorpeca.ZVALORPZA) * Convert.ToInt32(item.quantidade);
                list.Add(new
                {
                    @quantidade = item.quantidade == null ? "" : item.quantidade.ToString(),
                    @peca_codigo = item.peca_SAP.ZCODPECA.ToString(),
                    @peca_id = item.id,
                    @peca_descricao = item.peca_SAP.descricao == null ? "" : item.peca_SAP.descricao,
                    @peca_nome = item.peca_SAP.ZNOMPECA == null ? "" : item.peca_SAP.ZNOMPECA.ToString().Trim(),
                    @peca_diametro = valorpeca.ZDIAMETRO == null ? "" : valorpeca.ZDIAMETRO.ToString(),
                    @peca_altura = valorpeca.ZALTURA == null ? "" : valorpeca.ZALTURA.ToString(),
                    @peca_valor = valorpeca.ZVALORPZA == null ? "" : valorpeca.ZVALORPZA.ToString().Replace(",", "."),
                });
            }

            return Json(new { total = valor, pecas = list }, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult alteraPecaAtivacaoMobile(int? id_ativacaopeca, int quantidade)
        {
            if (id_ativacaopeca != null)
            {
                var ativacaoinfo = db.ativacao_peca.FirstOrDefault(ativacao_peca => ativacao_peca.id == id_ativacaopeca);
                if (ativacaoinfo != null)
                {
                    ativacaoinfo.quantidade = quantidade;
                    db.SaveChanges();
                    return Json(new { status = 1, msg = "Ativação alterada com sucesso" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 0, msg = "Nenhum registro encontrado." }, JsonRequestBehavior.AllowGet);
                }


            }
            else
            {
                return Json(new { status = 0, msg = "ID não foi informado." }, JsonRequestBehavior.AllowGet);
            }


        }

       
        public ActionResult removePecaAtivacaoMobile(int? id_pecaativacao)
        {
            ativacao_peca ativacao_peca = db.ativacao_peca.Find(id_pecaativacao);

            db.ativacao_peca.Remove(ativacao_peca);
            db.SaveChanges();


            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult checaPecaAtivacaoMobile(int? id_ativacao, int? id_peca)
        {
            var pecaativ = db.ativacao_peca.FirstOrDefault(ativacao_peca => ativacao_peca.id_ativacao == id_ativacao && ativacao_peca.ZCODPECA == id_peca);

            if (pecaativ != null)
            {
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }




        }

        public ActionResult mostraMensagemDoDiaMobile()
        {
            


            var msg = db.mensagem_do_dia;
            var msgs = new List<dynamic>();
            foreach (var item in msg)
            {
                msgs.Add(new
                {
                    @id = item.id,
                    @mensagem = item.@mensagem,
                    @status = item.status,
                    @timestamp = item.@timestamp

                });
            }

            return Json(new { status = 1, mensagem = msgs }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult trocaFotoProspector([Bind(Include = "ZCODCOLAB, foto")] colaborador_SAP colaborador)
        {


            colaborador_SAP col = db.colaborador_SAP.FirstOrDefault(c => c.ZCODCOLAB == colaborador.ZCODCOLAB);
            if (col != null)
            {
                col.foto = colaborador.foto;
                db.SaveChanges();
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);

            
            
        }

        public ActionResult fotoPecaMobile(int? id_peca)
        {
            if (id_peca == null)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }

            var peca_imagem = db.caracteristica_de_peca_SAP.FirstOrDefault(p => p.peca_SAP.ZCODPECA == id_peca);

            //var peca_imagem = db.peca_SAP.FirstOrDefault(p => p.ZCODPECA == id_peca);

            //String peca_foto = ">img src = "+peca_imagem+"></img>";

            // CARACTERISTICA DE PECA
            var peca = peca_imagem.foto_url;

            /*
            Response.AddHeader(
                   "Content-Disposition", "attachment; filename=\"filenamehere.png\""); 

            */


            Response.Write("<img src='" + peca + "'/>");


            return new EmptyResult();

            //return Json(new { peca_foto = peca}, JsonRequestBehavior.AllowGet);
            //return Json(peca, JsonRequestBehavior.AllowGet);
            //return Json(peca.Trim('"'), JsonRequestBehavior.AllowGet);

        }

        public ActionResult listaMotivosDeRecusa()
        {
            var recusa = db.motivo_de_recuso_SAP;
            var recusalist = new List<dynamic>();

            foreach (var item in recusa)
            {
                recusalist.Add(new
                {
                    @id = item.ZCODMOTIV,
                    @mandante = item.MANDT == null ? "" : item.MANDT.ToString().Trim(),
                    @codigo = item.ZCODMOTIV.ToString(),
                    @descricao = item.descricao == null ? "" : item.descricao.ToString().Trim(),
                    @nome = item.ZNOMMOTIV == null ? "" : item.ZNOMMOTIV.ToString().Trim(),
                    @desativado = item.ZDESACTIV == null ? "" : item.ZDESACTIV.ToString().Trim(),
                    @id_motivos_2 = item.ZCODMOTI2 == null ? "" : item.ZCODMOTI2.ToString().Trim() 
                });
            }


            return Json(new { recusa = recusalist }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult detalhesEnqueteMobile(int? id_enquete)
        {
            var enquete = db.enquete.Where(e => e.id == id_enquete);
            var enquetelist = new List<dynamic>();
            var enqueteperguntas = new List<dynamic>();

            foreach (var item in enquete)
            {
                enquetelist.Add(new
                {
                    @id = item.id,
                    @titulo = item.titulo == null ? "" : item.titulo.ToString(),
                    @vigencia = item.vigencia == null ? "" : item.vigencia.ToString(),
                    @data_limite = item.data_limite == null ? "" : item.data_limite.ToString(),
                    @status = item.status == null ? "" : item.status.ToString(),
                    @id_cluster = item.ZCODCLUST == null ? "" : item.ZCODCLUST.ToString(),
                    @data_inicio = item.data_inicio == null ? "" : item.data_inicio.ToString()
                });

            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult etapasAtivacaoMobile(int? etapa, int? id_ativacao)
        {
            var rsEtapas = db.contrapartida_ativacoes.Where(a => a.etapa == etapa);
            var etapas = new List<dynamic>();

            foreach (var item in rsEtapas)
            {
                etapas.Add(new
                {
                    @id = item.id,
                    @id_contrapartida = item.ZCODCONPA,
                    @id_ativacao = item.id_ativacao,
                    @contrapartida = item.contrapartida_SAP.ZNOMCONPA,
                    @etapa = item.etapa
                });
            }


            return Json(new { etapas = etapas }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaProjetosClienteMobile(string id_cliente)
        {
            var idcliente2 = id_cliente.ToString();
            var cluster_cliente = db.cliente_cluster_SAP.Where(c => c.ZCLIENTE == idcliente2);

            var projeto_cliente_lista = new List<dynamic>();



            foreach (var item in cluster_cliente)
            {
                //return Json(item.id_cluster, JsonRequestBehavior.AllowGet);
                var projeto_cliente = db.projeto_cluster.Where(p => p.ZCODCLUST == item.ZCODCLUSTER);

                foreach (var item2 in projeto_cliente)
                {
                    projeto_cliente_lista.Add(new
                    {
                        @id = item2.projeto.id,
                        @codigo = item2.projeto.codigo,
                        @descricao = item2.projeto.descricao,
                        @observacao = item2.projeto.observacao,
                        //@id_cluster = item2.projeto,
                    });
                }


            }

            return Json(new { projeto_cliente_lista = projeto_cliente_lista.Distinct() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaContrapartidaAtivacaoMobile(int? id_ativacao)
        {
            var max = db.contrapartida_ativacoes.Where(x => x.ativacao.id == id_ativacao).OrderByDescending(p => p.etapa).FirstOrDefault();
            //return Json(new { max = max.etapa }, JsonRequestBehavior.AllowGet);


            var contras = new List<dynamic>();
            var contra = db.contrapartida_ativacoes.Where(c => c.id_ativacao == id_ativacao && c.etapa == max.etapa);

            foreach (var item in contra)
            {
                var verifica = db.contrapartida_ativacoes.Where(cp => cp.id_ativacao == id_ativacao && cp.id_contrapartida == item.contrapartida_SAP.ZCODCONPA).Count();
                contras.Add(new
                {
                    @id = item.id,
                    @mandante = item.contrapartida_SAP.MANDT == null ? "" : item.contrapartida_SAP.MANDT.ToString(),
                    @codigo = item.contrapartida_SAP.ZCODCONPA.ToString(),
                    @descricao = item.contrapartida_SAP.descricao == null ? "" : item.contrapartida_SAP.descricao.ToString(),
                    @nome = item.contrapartida_SAP.ZNOMCONPA == null ? "" : item.contrapartida_SAP.ZNOMCONPA.ToString(),
                    @obrigatorio = item.contrapartida_SAP.obrigatorio == null ? "" : item.contrapartida_SAP.obrigatorio.ToString(),
                    @verifica = verifica
                });
            }



            return Json(new { contras = contras }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaContrapartidasClienteMobile(string id_cliente)
        {
            var id_cliente2 = id_cliente.ToString();
            // pegar as contrapartidas relacionadas ao projeto escolhido para o cliente
            var ativacao = db.ativacao.FirstOrDefault(c => c.ZCLIENTE == id_cliente2);
            // pegar o id do cliente
            // pegar os projetos desse cara

            var projetos = db.cliente_projeto.Where(cp => cp.ZCLIENTE == id_cliente2);
            var listacontrapartidas = new List<dynamic>();
            // pegar as contrapartidas relacionadas a esse projeto



            foreach (var item in projetos)
            {
                var contrapartida_projeto = db.contrapartida_projeto.Where(p => p.id_projeto == item.id_projeto);

                if (contrapartida_projeto.Count() != 0)
                {
                    foreach (var item2 in contrapartida_projeto)
                    {
                        //var verifica = db.contrapartida_ativacoes.Where(cp => cp.id_ativacao == ativacao.id && cp.id_contrapartida == item2.contrapartida_SAP.ZCODCONPA).Count();
                        listacontrapartidas.Add(new
                        {
                            @id = item2.contrapartida_SAP.ZCODCONPA,
                            @mandante = item2.contrapartida_SAP.MANDT == null ? "" : item2.contrapartida_SAP.MANDT.ToString(),
                            @codigo = item2.contrapartida_SAP.ZCODCONPA.ToString(),
                            @descricao = item2.contrapartida_SAP.descricao == null ? "" : item2.contrapartida_SAP.descricao.ToString(),
                            @nome = item2.contrapartida_SAP.ZNOMCONPA == null ? "" : item2.contrapartida_SAP.ZNOMCONPA.ToString(),
                            @obrigatorio = item2.contrapartida_SAP.obrigatorio == null ? "" : item2.contrapartida_SAP.obrigatorio.ToString(),
                            /*@verifica = verifica*/
                        });
                    }
                }
                
            }



            //var contrapartidas = db.contrapartida_projeto.Where(p=>p.id == item.id);

            return Json(new { listacontrapartidas = listacontrapartidas.Distinct() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult verificaEtapaAtivacaoMobile(int? id_ativacao)
        {
            var max = db.ativacao_foto.Where(x => x.ativacao.id == id_ativacao).OrderByDescending(p => p.id).FirstOrDefault();

            var id_cliente = max.ativacao.cliente_SAP.ZCLIENTE;

            var projetos = db.cliente_projeto.Where(cp => cp.ZCLIENTE == id_cliente);
            var listacontrapartidas = new List<dynamic>();
            // pegar as contrapartidas relacionadas a esse projeto



            foreach (var item in projetos)
            {
                var contrapartida_projeto = db.contrapartida_projeto.Where(p => p.id_projeto == item.id_projeto);

                foreach (var item2 in contrapartida_projeto)
                {
                    var verifica = db.contrapartida_projeto.Where(cp => cp.id_projeto == item.projeto.id && cp.ZCODCONPA == item2.contrapartida_SAP.ZCODCONPA).Count();
                    listacontrapartidas.Add(new
                    {
                        @id = item2.contrapartida_SAP.ZCODCONPA,
                        @mandante = item2.contrapartida_SAP.MANDT == null ? "" : item2.contrapartida_SAP.MANDT.ToString(),
                        @codigo =item2.contrapartida_SAP.ZCODCONPA.ToString(),
                        @descricao = item2.contrapartida_SAP.descricao == null ? "" : item2.contrapartida_SAP.descricao.ToString(),
                        @nome = item2.contrapartida_SAP.ZNOMCONPA == null ? "" : item2.contrapartida_SAP.ZNOMCONPA.ToString(),
                        @obrigatorio = item2.contrapartida_SAP.obrigatorio,
                        @verifica = verifica
                    });
                }
            }


            
            return Json(new { etapa = max.etapa, exec = max.executivo, contras = listacontrapartidas.Distinct() }, JsonRequestBehavior.AllowGet);/**/


            /*return Json(new { etapa = max.etapa, contras = listacontrapartidas.Distinct()}, JsonRequestBehavior.AllowGet);*/
        }

        public ActionResult alteraQuantidadePeca(string id_peca, int id_ativacao, int? quantidadepeca)
        {
            bool result;
            decimal total = 0;
            var id_peca2 = Convert.ToDecimal(id_peca);

            var peca = db.ativacao_peca.Where(a=>a.id_ativacao == id_ativacao && a.ZCODPECA != id_peca2);


            foreach (var item in peca)
            {
                var preco = db.caracteristica_de_peca_SAP.FirstOrDefault(a=>a.ZCODIPECA == item.ZCODPECA);
                var precopeca = Convert.ToDecimal(preco.ZVALORPZA);
                var quantidade = Convert.ToInt32(item.quantidade);

                total += precopeca * quantidade;
            }


            var budget = db.ativacao.FirstOrDefault(a => a.id == id_ativacao).cliente_SAP.custo_budget;
            
            var pecaadicionar = db.caracteristica_de_peca_SAP.FirstOrDefault(p=>p.ZCODIPECA == id_peca2);

            var totalpecaadicionar = Convert.ToDecimal(pecaadicionar.ZVALORPZA) * quantidadepeca;

            var totalcompra = total + totalpecaadicionar;

            if (totalcompra > budget)
            {
                result = false;
            }
            else
            {
                var ativpeca = db.ativacao_peca.FirstOrDefault(
                      a => a.id_ativacao == id_ativacao && a.ZCODPECA == id_peca2
                    );
                ativpeca.quantidade = quantidadepeca;
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
                        
        }

        public ActionResult sincronizaDadosMobile(int? id_colaborador)
        {
            var countresult = 0;


            if (id_colaborador == null)
            {
                return Json("Não foi possível sincronizar.", JsonRequestBehavior.AllowGet);
            }

            var listacliente = new List<dynamic>();
            var listaenquete = new List<dynamic>();
            var listaenquetepergunta = new List<dynamic>();
            var listaenqueteresposta = new List<dynamic>();
            var listaclienteprojeto = new List<dynamic>();
            var listaativacao = new List<dynamic>();
            var listpecaativacao = new List<dynamic>();
            var listafotoativacao = new List<dynamic>();
            var listacontrapartidasativacao = new List<dynamic>();
            var listmotivoderecuso = new List<dynamic>();
            var listaclientecluster = new List<dynamic>();
            var listaprojetos = new List<dynamic>();
            var listaprojetocluster = new List<dynamic>();
            var listapecas = new List<dynamic>();
            var listacontrapartidas = new List<dynamic>();
            var listacontrapartidasprojeto = new List<dynamic>();
            var listapecacluster = new List<dynamic>();
            var listastatusativacao = new List<dynamic>();
            var dadosColaborador = new List<dynamic>();
            var listacluster = new List<dynamic>();

            var colab = db.colaborador_SAP.FirstOrDefault(c => c.ZCODCOLAB == id_colaborador);
            var dateAndTime = DateTime.Now;

            colab.acessoffline = 1;
            colab.ultimosinc = dateAndTime;
            db.SaveChanges();



            //return Json(colab.id, JsonRequestBehavior.AllowGet);
            
            dadosColaborador.Add(new
            {
                @id = colab.ZCODCOLAB.ToString(),
                @mandante = colab.MANDT == null ? "" : colab.MANDT.ToString(),
                @codigo = colab.ZCODCOLAB.ToString(),
                @nome = colab.ZNOMBRE == null ? "" : colab.ZNOMBRE.ToString(),
                @login = colab.ZLOGIN == null ? "" : colab.ZLOGIN.ToString(),
                @email = colab.ZEMAIL == null ? "" : colab.ZEMAIL.ToString(),
                @foto = colab.foto == null ? "" : colab.foto.ToString()
            });

            var rsCliente = db.cliente_SAP.Where(c => c.id_colaborador == colab.ZCODCOLAB);
            foreach (var item in rsCliente)
            {

                
                listacliente.Add(new
                {
                    @id = item.ZCLIENTE == null ? "" : item.ZCLIENTE == null ? "" : item.ZCLIENTE.ToString(),
                    @codigo = item.ZCLIENTE == null ? "" : item.ZCLIENTE.ToString(),
                    @nome = item.ZNOMBRE == null ? "" : item.ZNOMBRE.ToString(),
                    @nomefantasia = item.ZNOMFANTA == null ? "" : item.ZNOMFANTA.ToString(),
                    @endereco = item.ZDIRECCIO == null ? "" : item.ZDIRECCIO.ToString(),
                    @bairro = item.ZBARRIO == null ? "" : item.ZBARRIO.ToString(),
                    @cidade = item.ZCIUDAD == null ? "" : item.ZCIUDAD.ToString(),
                    @cnpj = item.ZCNPJCPFI == null ? "" : item.ZCNPJCPFI.ToString(),
                    @primeiro_telefone = item.ZTELEFONO == null ? "" : item.ZTELEFONO.ToString(),
                    @status_ativacao_nome = item.status_ativacao1.nome.ToString(),
                    @status_ativacao = item.status_ativacao1.id.ToString(),
                    @ativacao_ultima_atualizacao = item.ativacao_ultima_atualizacao == null ? "" : item.ativacao_ultima_atualizacao.ToString(),
                    @motivo_id = item.motivo_id == null ? "" : item.motivo_id.ToString(),
                    @motivotxt = item.motivotxt == null ? "" : item.motivotxt.ToString(),
                    @custo_budget = item.custo_budget == null ? "" : item.custo_budget.ToString().Replace(",", "."),
                    @observacao = item.observacao == null ? "" : item.observacao.ToString(),
                    @pendente_retorno = item.pendente_retorno == null ? "" : item.pendente_retorno.ToString(),
                    @motivo_retorno = item.motivo_retorno == null ? "" : item.motivo_retorno.ToString(),
                    @recusa_foto = item.recusa_foto == null ? "" : item.recusa_foto.ToString(),
                    @carta_foto = item.carta_foto == null ? "" : item.carta_foto.ToString(),
                    @id_colaborador = colab.ZCODCOLAB.ToString(),
                });

                var projetocliente = db.cliente_projeto.Where(p => p.ZCLIENTE == item.ZCLIENTE);

                foreach (var item12 in projetocliente)
                {
                    
                    listaclienteprojeto.Add(new
                    {
                        @id = item12.id,
                        @id_cliente = item12.ZCLIENTE,
                        @id_projeto = item12.id_projeto
                    });
                }

                var clustercliente = db.cliente_cluster_SAP.Where(cl => cl.ZCLIENTE == item.ZCLIENTE);

                foreach (var item2 in clustercliente)
                {

                    
                    listaclientecluster.Add(new
                    {
                        @id = item2.id,
                        @id_cliente = item2.ZCLIENTE,
                        @id_cluster = item2.ZCODCLUSTER
                    });

                    var pecacluster = db.cluster_peca_SAP.Where(p => p.ZCODCLUST == item2.ZCODCLUSTER);

                    foreach (var item10 in pecacluster)
                    {
                        var pecainfo = db.caracteristica_de_peca_SAP.FirstOrDefault(p=>p.ZCODIPECA == item10.ZCODPECA);

                        
                        listapecas.Add(new
                        {
                            // CARACTERISTICA DE PECA
                            @id = pecainfo.peca_SAP.ZCODPECA.ToString(),
                            @nome = pecainfo.peca_SAP.ZNOMPECA == null ? "" : pecainfo.peca_SAP.ZNOMPECA.ToString().Trim(),
                            @mandt = pecainfo.peca_SAP.MANDT == null ? "" : pecainfo.peca_SAP.MANDT.ToString().Trim(),
                            @altura = pecainfo.ZALTURA == null ? "" : pecainfo.ZALTURA.ToString().Trim(),
                            @diametro = pecainfo.ZDIAMETRO == null ? "" : pecainfo.ZDIAMETRO.ToString().Trim(),
                            @valor = pecainfo.ZVALORPZA == null ? "" : pecainfo.ZVALORPZA.ToString().Replace(",", "."),
                            @peca_foto = pecainfo.foto_url == null ? "" : pecainfo.foto_url.ToString().Trim(),
                        });


                        
                        listapecacluster.Add(new
                        {
                            @id = item10.id,
                            @id_cluster = item10.ZCODCLUST,
                            @id_peca = item10.ZCODPECA,
                        });
                    }
                    var projetocluster = db.projeto_cluster.Where(p => p.ZCODCLUST == item2.ZCODCLUSTER);

                    foreach (var item4 in projetocluster)
                    {
                        
                        listaprojetocluster.Add(new
                        {
                            id = item4.id,
                            id_projeto = item4.id_projeto,
                            id_cluster = item4.ZCODCLUST
                        });

                        var contrapartidas = db.contrapartida_projeto.Where(p => p.id_projeto == item4.projeto.id);

                        foreach (var item6 in contrapartidas)
                        {
                            
                            listacontrapartidasprojeto.Add(new
                            {
                                @id = item6.id,
                                @id_projeto = item6.id_projeto,
                                @id_contrapartida = item6.ZCODCONPA,
                            });
                        }

                        foreach (var item5 in contrapartidas)
                        {
                            
                            listacontrapartidas.Add(new
                            {
                                id = item5.contrapartida_SAP.ZCODCONPA.ToString(),
                                mandante = item5.contrapartida_SAP.MANDT == null ? "" : item5.contrapartida_SAP.MANDT.ToString(),
                                codigo = item5.contrapartida_SAP == null ? "" : item5.contrapartida_SAP.ZCODCONPA.ToString(),
                                descricao = item5.contrapartida_SAP.descricao == null ? "" : item5.contrapartida_SAP.descricao.ToString(),
                                nome = item5.contrapartida_SAP.ZNOMCONPA == null ? "" : item5.contrapartida_SAP.ZNOMCONPA.ToString(),
                                obrigatorio = item5.contrapartida_SAP.obrigatorio == null ? "" : item5.contrapartida_SAP.obrigatorio.ToString()
                            });
                        }


                        
                        listaprojetos.Add(new
                        {
                            @id = item4.projeto.id.ToString(),
                            @codigo = item4.projeto.codigo == null ? "" : item4.projeto.codigo.ToString(),
                            @descricao = item4.projeto.descricao == null ? "" : item4.projeto.descricao.ToString(),
                            @observacao = item4.projeto.observacao == null ? "" : item4.projeto.observacao.ToString(),
                            //@id_cluster = item4.projeto.id_cluster
                        });
                    }

                }

            }

            var rsAtivacao = db.ativacao.Where(c => c.cliente_SAP.id_colaborador == id_colaborador);

            foreach (var item7 in rsAtivacao)
            {
                var contrapartidaativacao = db.contrapartida_ativacoes.Where(c => c.id_ativacao == item7.id);

                foreach (var item11 in contrapartidaativacao)
                {
                    
                    listacontrapartidasativacao.Add(new
                    {
                        @id = item11.id,
                        @id_contrapartida = item11.ZCODCONPA,
                        @id_ativacao = item11.id_ativacao,
                        @etapa = item11.etapa,
                    });
                }

                
                listaativacao.Add(new
                {
                    @id = item7.id.ToString(),
                    @codigo = item7.codigo == null ? "" : item7.codigo.ToString(),
                    @custo_valor = item7.custo_valor == null ? "" : item7.custo_valor.ToString(),
                    @descricao = item7.descricao == null ? "" : item7.descricao.ToString(),
                    @id_cliente = item7.ZCLIENTE == null ? "" : item7.ZCLIENTE.ToString(),
                    @status = item7.status == null ? "" : item7.status.ToString(),
                    @id_projeto = item7.id_projeto == null ? "" : item7.id_projeto.ToString(),
                    @situacao = item7.situacao == null ? "" : item7.situacao.ToString(),
                    @executivo = item7.executivo == null ? "" : item7.executivo.ToString(),
                    @criacao = item7.criacao == null ? "" : item7.criacao.ToString()
                });

                var rsFotoAtivacao = db.ativacao_foto.Where(f => f.id_ativacao == item7.id);

                foreach (var item8 in rsFotoAtivacao)
                {
                    
                    listafotoativacao.Add(new
                    {
                        @id = item8.id,
                        @id_ativacao = item8.id_ativacao,
                        @url_foto = item8.url_foto,
                        @etapa = item8.etapa,
                        @id_peca = item8.id_peca,
                        @executivo = item8.executivo,

                    });
                }

                var rsPecaAtivacao = db.ativacao_peca.Where(p => p.id_ativacao == item7.id);

                foreach (var item9 in rsPecaAtivacao)
                {
                    
                    listpecaativacao.Add(new
                    {
                        @id = item9.id,
                        @id_peca = item9.ZCODPECA,
                        @id_ativacao = item9.id_ativacao,
                        @quantidade = item9.quantidade,

                    });
                }


            }

            var rsMotivosDeRecuso = db.motivo_de_recuso_SAP;

            foreach (var item12 in rsMotivosDeRecuso)
            {
                
                listmotivoderecuso.Add(new
                {
                    @id = item12.ZCODMOTIV.ToString(),
                    @mandante = item12.MANDT == null ? "" : item12.MANDT.ToString(),
                    @codigo = item12.ZCODMOTIV.ToString(),
                    @descricao = item12.descricao == null ? "" : item12.descricao.ToString(),
                    @nome = item12.ZNOMMOTIV == null ? "" : item12.ZNOMMOTIV.ToString(),
                    @desativado = item12.ZDESACTIV == null ? "" : item12.ZDESACTIV.ToString(),
                    @id_motivos_2 = item12.ZCODMOTI2 == null ? "" : item12.ZCODMOTI2.ToString()
                });
            }

            var status = db.status_ativacao;

            foreach (var item17 in status)
            {
                
                listastatusativacao.Add(new
                {
                    @id = item17.id,
                    @nome = item17.nome
                });
            }

            var dadosSinc = new List<dynamic>();

            countresult += dadosColaborador.Count();
            countresult += listaprojetocluster.Count();
            countresult += listastatusativacao.Count();
            countresult += listmotivoderecuso.Distinct().Count();
            countresult += listaclientecluster.Distinct().Count();
            countresult += listacliente.Distinct().Count();
            countresult += listaprojetos.Distinct().Count();
            countresult += listapecas.Distinct().Count();
            countresult += listacontrapartidas.Distinct().Count();
            countresult += listaativacao.Distinct().Count();
            countresult += listacontrapartidasprojeto.Distinct().Count();
            countresult += listacontrapartidasativacao.Distinct().Count();
            countresult += listafotoativacao.Distinct().Count();
            countresult += listapecacluster.Distinct().Count();
            countresult += listpecaativacao.Distinct().Count();
            countresult += listaclienteprojeto.Distinct().Count();
            countresult += listacluster.Distinct().Count();
            

            dadosSinc.Add(new
            {
                count = countresult,
                dadosColaborador = dadosColaborador,
                listaprojetocluster = listaprojetocluster,
                listastatusativacao = listastatusativacao,
                listmotivoderecuso = listmotivoderecuso.Distinct(),
                listaclientecluster = listaclientecluster.Distinct(),
                listacliente = listacliente.Distinct(),
                listaprojetos = listaprojetos.Distinct(),
                listapecas = listapecas.Distinct(),
                listacontrapartidas = listacontrapartidas.Distinct(),
                listaativacao = listaativacao.Distinct(),
                listacontrapartidasprojeto = listacontrapartidasprojeto.Distinct(),
                listacontrapartidasativacao = listacontrapartidasativacao.Distinct(),
                listafotoativacao = listafotoativacao.Distinct(),
                listapecacluster = listapecacluster.Distinct(),
                listpecaativacao = listpecaativacao.Distinct(),
                listaclienteprojeto = listaclienteprojeto.Distinct(),
                listacluster = listacluster.Distinct()
            });

            dadosSinc.Add(new
            {
                count = countresult
            });

            return Json(dadosSinc, JsonRequestBehavior.AllowGet);
            //return Json(new { listacliente = listacliente, listaclientecluster = listaclientecluster, listapecas = listapecas.Distinct(), listaprojetos = listaprojetos.Distinct(), listacontrapartidas = listacontrapartidas.Distinct(), listacontrapartidasprojeto = listacontrapartidasprojeto }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaProjetoClienteMobile(string id_cliente)
        {
            var id_cliente2 = id_cliente.ToString();
            var projeto_cliente_lista = new List<dynamic>();

            var listaProjetos = db.cliente_projeto.Where(p => p.ZCLIENTE == id_cliente2);
            foreach (var item in listaProjetos)
            {
                projeto_cliente_lista.Add(new
                {
                    @id = item.projeto.id,
                    @nome = item.projeto.descricao
                });
            }


            return Json(new { projeto_cliente_lista = projeto_cliente_lista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaStatusAtivacoes()
        {
            var listastatus = db.status_ativacao;
            var lista = new List<dynamic>();

            foreach (var item in listastatus)
            {
                lista.Add(new
                {
                    id = item.id,
                    nome = item.nome
                });
            }

            return Json(new { listastatus = lista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult listaBuscaMobile(int id)
        {



            var busca = new List<dynamic>();
            var cidade = new List<dynamic>();
            var bairro = new List<dynamic>();
            var canal = new List<dynamic>();
            var estado = new List<dynamic>();
            var status = new List<dynamic>();

            var dados = db.cliente_SAP.Where(c => c.id_colaborador == id);

            foreach (var item in dados)
            {
                cidade.Add(new
                {
                    @cidade = item.ZCIUDAD
                });

                bairro.Add(new
                {
                    @bairro = item.ZBARRIO
                });

                status.Add(new
                {
                    @id = item.status_ativacao1.id,
                    @status_ativacao = item.status_ativacao1.nome
                });


                /*canal.Add(new
                {
                    @id = item.canal1.id,
                    @nome = item.canal1.nome
                });*/
            }


            busca.Add(new
            {
                cidade = cidade.Distinct(),
                bairro = bairro.Distinct(),
                //canal = canal.Distinct(),
                status = status.Distinct()
            });

            return Json(busca, JsonRequestBehavior.AllowGet);
        }

        public ActionResult verificaItem(int? id_ativacao)
        {
            var pecas = db.ativacao_peca.Where(p => p.id_ativacao == id_ativacao).Count();

            if (pecas >= 1)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult adicionaFotoRecusado(string cliente, string foto)
        {
            var cliente2 = cliente.ToString();
            var clienteinfo = db.cliente_SAP.FirstOrDefault(a => a.ZCLIENTE == cliente2);

            clienteinfo.recusa_foto = foto;

            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public int uploadDadosClienteProjeto2([Bind(Include = "id_cliente, id_projeto")] cliente_projeto cliente_projeto)
        {
            if (cliente_projeto != null)
            {

                var clienteprojeto = new cliente_projeto
                {
                    ZCLIENTE = cliente_projeto.ZCLIENTE,
                    id_projeto = cliente_projeto.id_projeto,
                };
                db.cliente_projeto.Add(clienteprojeto);
                db.SaveChanges();

            }

            return 0;
        }

        public int uploadDadosCliente(List<cliente_SAP> cliente, List<cliente_projeto> cliente_projeto)
        {
            if (cliente != null)
            {
                foreach (var item in cliente)
                {
                    var clientex = db.cliente_SAP.FirstOrDefault(c => c.ZCLIENTE == item.ZCLIENTE);
                    clientex.status_ativacao = item.status_ativacao;
                    clientex.observacao = item.observacao;
                    clientex.carta_foto = item.carta_foto;
                    if (item.motivo_id != null)
                    {
                        clientex.motivo_id = Convert.ToInt32(item.motivo_id);
                    }
                    //clientex.ativacao_ultima_atualizacao = item.ativacao_ultima_atualizacao;
                    clientex.motivotxt = item.motivotxt.ToString();
                    db.SaveChanges();
                }
            }


            if (cliente_projeto != null)
            {
                foreach (var item5 in cliente_projeto)
                {
                    var clienteprojeto = new cliente_projeto
                    {
                        ZCLIENTE = item5.ZCLIENTE,
                        id_projeto = item5.id_projeto,
                    };
                    db.cliente_projeto.Add(clienteprojeto);
                    db.SaveChanges();
                }

            }
            return 0;

        }

        public ActionResult etapasAtivacaoMobile2(int? etapa, int? id_ativacao)
        {
            var rsEtapas = db.contrapartida_ativacoes.Where(a => a.etapa == etapa && a.id_ativacao == id_ativacao).OrderBy(c => c.etapa);
            var etapas = new List<dynamic>();

            foreach (var item in rsEtapas)
            {
                etapas.Add(new
                {
                    @id = item.id,
                    @id_contrapartida = item.ZCODCONPA,
                    @id_ativacao = item.id_ativacao,
                    @contrapartida = item.contrapartida_SAP.ZNOMCONPA,
                    @etapa = item.etapa
                });
            }


            return Json(new { etapas = etapas }, JsonRequestBehavior.AllowGet);
        }

        public void uploadDadosAtualizaAtivacao(List<ativacao> ativacao, List<contrapartida_ativacoes> ca, List<ativacao_foto> af, List<ativacao_peca> ap)
        {
            if (ativacao != null)
            {
                foreach (var item in ativacao)
                {
                    int id = 0;
                    var ativ = db.ativacao.FirstOrDefault(a => a.id == item.id);
                    if (ativ != null)
                    {
                        ativ.codigo = item.codigo;
                        ativ.custo_valor = item.custo_valor;
                        ativ.descricao = item.descricao;
                        ativ.ZCLIENTE = item.ZCLIENTE;
                        ativ.status = item.status;
                        ativ.id_projeto = item.id_projeto;
                        ativ.situacao = item.situacao;
                        ativ.executivo = item.executivo;
                        ativ.criacao = item.criacao;
                        db.SaveChanges();
                        id = ativ.id;
                    }
                    else if (ativ == null)
                    {
                        var ati = new ativacao
                        {
                            codigo = item.codigo,
                            custo_valor = item.custo_valor,
                            descricao = item.descricao,
                            ZCLIENTE = item.ZCLIENTE,
                            status = item.status,
                            id_projeto = item.id_projeto,
                            situacao = item.situacao,
                            executivo = item.executivo,
                            criacao = item.criacao,
                        };
                        db.ativacao.Add(ati);
                        db.SaveChanges();
                        id = ati.id;
                    }



                    if (ca != null)
                    {
                        foreach (var item2 in ca)
                        {
                            var contra = new contrapartida_ativacoes
                            {
                                ZCODCONPA = item2.ZCODCONPA,
                                id_ativacao = id,
                                etapa = item2.etapa
                            };
                            db.contrapartida_ativacoes.Add(contra);
                            db.SaveChanges();
                        }

                    }

                    if (af != null)
                    {
                        foreach (var item3 in af)
                        {
                            var foto = new ativacao_foto
                            {
                                id_ativacao = id,
                                url_foto = item3.url_foto,
                                etapa = item3.etapa,
                                id_peca = item3.id_peca,
                                executivo = item3.executivo
                            };
                            db.ativacao_foto.Add(foto);
                            db.SaveChanges();
                        }
                    }
                    if (ap != null)
                    {
                        foreach (var item4 in ap)
                        {
                            var peca = new ativacao_peca
                            {
                                id_ativacao = id,
                                ZCODPECA = item4.ZCODPECA,
                                quantidade = item4.quantidade
                            };
                            db.ativacao_peca.Add(peca);
                            db.SaveChanges();
                        }
                    }
                }
            }

        }

        public ActionResult uploadDadosNovaAtivacao(List<ativacao> ativacao, List<contrapartida_ativacoes> ca, List<ativacao_foto> af, List<ativacao_peca> ap)
        {
            foreach (var item in ativacao)
            {
                //var ativ = db.ativacao.FirstOrDefault(a => a.id == item.id);
                var ativ = new ativacao
                {
                    codigo = item.codigo,
                    custo_valor = item.custo_valor,
                    descricao = item.descricao,
                    id_cliente = item.id_cliente,
                    status = item.status,
                    id_projeto = item.id_projeto,
                    situacao = item.situacao,
                    executivo = item.executivo,
                    criacao = item.criacao,
                };

                db.ativacao.Add(ativ);
                db.SaveChanges();
                int id = ativ.id;

                if (ca != null)
                {
                    foreach (var item2 in ca)
                    {
                        var contra = new contrapartida_ativacoes
                        {
                            ZCODCONPA = item2.ZCODCONPA,
                            id_ativacao = id,
                            etapa = item2.etapa
                        };
                        db.contrapartida_ativacoes.Add(contra);
                        db.SaveChanges();
                    }

                }

                if (af != null)
                {
                    foreach (var item3 in af)
                    {
                        var foto = new ativacao_foto
                        {
                            id_ativacao = id,
                            url_foto = item3.url_foto,
                            etapa = item3.etapa,
                            id_peca = item3.id_peca,
                            executivo = item3.executivo
                        };
                        db.ativacao_foto.Add(foto);
                        db.SaveChanges();
                    }
                }
                if (ap != null)
                {
                    foreach (var item4 in ap)
                    {
                        var peca = new ativacao_peca
                        {
                            id_ativacao = id,
                            ZCODPECA = item4.ZCODPECA,
                            quantidade = item4.quantidade
                        };
                        db.ativacao_peca.Add(peca);
                        db.SaveChanges();
                    }
                }
            }

            return Json(af, JsonRequestBehavior.AllowGet);
        }

        public void listaContrapartidasBackoffice(int? id_ativacao, string id_cliente)
        {
            var htmlretorno = "";
            var id_cliente2 = id_cliente.ToString();
            var projetos = db.cliente_projeto.Where(c => c.ZCLIENTE == id_cliente2);
            var listacontra2 = new List<dynamic>();

            foreach (var item in projetos)
            {
                var contra = db.contrapartida_projeto.Where(c => c.id_projeto == item.id_projeto);

                for (int countetapa = 0; countetapa < 12; countetapa++)
                {
                    int validarcontra = 0;
                    foreach (var itemcount in contra)
                    {
                        var contrapartidas = db.contrapartida_ativacoes.Where(c => c.id_ativacao == id_ativacao && c.etapa == countetapa && c.id_contrapartida == itemcount.ZCODCONPA);
                        if (contrapartidas.Count() >= 1)
                        {
                            validarcontra = 1;

                        }

                    }
                    if (validarcontra >= 1)
                    {
                        htmlretorno += "";
                        if (countetapa == 0)
                        {
                            htmlretorno += "<tr style='background-color:red; margin-top:20px'><td> Contrapartidas Anteriormente Negociadas: " + countetapa + " </td> </tr>";
                        }
                        else
                        {
                            htmlretorno += "<tr style='background-color:red; margin-top:20px'><td> Etapa: " + countetapa + " </td> </tr>";

                        }
                        foreach (var item2 in contra)
                        {
                            var contrapartidas = db.contrapartida_ativacoes.Where(c => c.id_ativacao == id_ativacao && c.etapa == countetapa && c.id_contrapartida == item2.ZCODCONPA);
                            if (contrapartidas.Count() >= 1)
                            {
                                htmlretorno += "<tr><td> <input type = 'checkbox' disabled checked> " + item2.contrapartida_SAP.ZNOMCONPA + " </td> </tr>";
                                listacontra2.Add(new
                                {

                                    @descricao = item2.contrapartida_SAP.ZNOMCONPA,
                                    @nome = item2.contrapartida_SAP.ZNOMCONPA,
                                    @id = item2.contrapartida_SAP.ZCODCONPA,
                                });
                            }
                            else
                            {
                                htmlretorno += "<tr><td> <input type = 'checkbox' disabled>" + item2.contrapartida_SAP.ZNOMCONPA + " </td></tr> ";
                                listacontra2.Add(new
                                {

                                    @descricao = item2.contrapartida_SAP.ZNOMCONPA,
                                    @nome = item2.contrapartida_SAP.ZNOMCONPA,
                                    @id = item2.contrapartida_SAP.ZCODCONPA,
                                });
                            }

                        }
                        htmlretorno += "";

                    }

                }


            }

            Response.Write(htmlretorno);

            //return Json(listacontra2, JsonRequestBehavior.AllowGet);


            /*var contrapartidas = db.contrapartida_ativacoes.Where(c => c.id_ativacao == id_ativacao).OrderBy(c => c.etapa);

            var listacontra = new List<dynamic>();

            foreach (var item in contrapartidas)
            {
                var check = db.contrapartida_projeto.Where(c => c.id_contrapartida == item.id).Count();

                listacontra.Add(new
                {
                    @descricao = item.contrapartida.descricao,
                    @nome = item.contrapartida.nome,
                    @id = item.contrapartida.id,
                    @etapa = item.etapa
                });
            }

            return Json(listacontra, JsonRequestBehavior.AllowGet);*/


        }

        public ActionResult gravaCartaCliente(string id_cliente, string foto)
        {
            var cliente = db.cliente_SAP.FirstOrDefault(c => c.ZCLIENTE == id_cliente);

            cliente.carta_foto = foto;

            db.SaveChanges();


            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GravaObservacaoCliente(string id_cliente, string observacao)
        {
            var id_cliente2 = id_cliente.ToString();
            var clientedados = db.cliente_SAP.FirstOrDefault(c => c.ZCLIENTE == id_cliente2);
            //return Json(clientedados, JsonRequestBehavior.AllowGet);
            clientedados.observacao = observacao;
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}