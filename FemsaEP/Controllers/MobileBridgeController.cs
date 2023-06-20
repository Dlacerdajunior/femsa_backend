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
    public class MobileBridgeController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

       // string local = "http://localhost:51286/";
        string local = "http://10.137.64.116/";

        public void loginMobile(string usuario, string senha)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("usuario", usuario.ToString()),
                new KeyValuePair<string, string>("senha", senha.ToString()),
            });
                var result = client.PostAsync("/Mobile/loginMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void dadosColaboradorMobile(int? id_colaborador)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_colaborador", id_colaborador.ToString()),
                
            });
                var result = client.PostAsync("/Mobile/dadosColaboradorMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void listaClientesMobile(int? id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id", id.ToString()),
                
            });
                var result = client.PostAsync("/Mobile/listaClientesMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void ativacaoMobile(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id", id.ToString()),
                
            });
                var result = client.PostAsync("/Mobile/ativacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void listaPecasMobile(int? id_ativacao, string id_cliente)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString()),
                new KeyValuePair<string, string>("id_cliente", id_cliente.ToString()),
                
            });
                var result = client.PostAsync("/Mobile/listaPecasMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void informacoesAtivacaoMobile(string id_cliente)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_cliente", id_cliente.ToString())
            });
                var result = client.PostAsync("/Mobile/informacoesAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void gravaCartaCliente(string id_cliente, string foto)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_cliente", id_cliente.ToString()),
                new KeyValuePair<string, string>("foto", foto.ToString())
            });
                var result = client.PostAsync("/Mobile/gravaCartaCliente", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void listaContrapartidasMobile()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("empty", "empty") });
                var result = client.PostAsync("/Mobile/listaContrapartidasMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void procuraPecaMobile(String query, int? id_ativacao, string id_cliente)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("query", query.ToString()),
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString()),
                new KeyValuePair<string, string>("id_cliente", id_cliente.ToString())
            });
                var result = client.PostAsync("/Mobile/procuraPecaMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void valorTotalAtivacaoMobile(int? ZCLIENTE)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("ZCLIENTE", ZCLIENTE.ToString())
            });
                var result = client.PostAsync("/Mobile/valorTotalAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void dadosClienteMobile(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id", id.ToString())
            });
                var result = client.PostAsync("/Mobile/dadosClienteMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void salvaPecaAtivacaoMobile([Bind(Include = "id,id_ativacao,ZCODPECA, quantidade")] ativacao_peca ativacao_peca)
        {


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id", ativacao_peca.id.ToString()),
                new KeyValuePair<string, string>("id_ativacao", ativacao_peca.id_ativacao.ToString()),
                new KeyValuePair<string, string>("ZCODPECA", ativacao_peca.ZCODPECA.ToString()),
                new KeyValuePair<string, string>("quantidade", ativacao_peca.quantidade.ToString()),
            });
                var result = client.PostAsync("/Mobile/salvaPecaAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void pendenteretorno(string id_cliente, string motivo)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_cliente", id_cliente.ToString()),
                new KeyValuePair<string, string>("motivo", motivo.ToString()),
            });
                var result = client.PostAsync("/Mobile/pendenteretorno", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void listaPecasAtivacaoMobile(int? id_ativacao)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString())
            });
                var result = client.PostAsync("/Mobile/listaPecasAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void fotosAtivacaoMobile(int? id_ativacao, int? etapa)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString()),
                new KeyValuePair<string, string>("etapa", etapa.ToString())
            });
                var result = client.PostAsync("/Mobile/fotosAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }


        }

        public void criaAtivacaoMobile([Bind(Include = "ZCLIENTE, id_projeto")] ativacao atv)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("ZCLIENTE", atv.ZCLIENTE.ToString())            
            });
                var result = client.PostAsync("/Mobile/criaAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void alteraStatusAtivacaoMobile(string id_cliente, int? id_status, int? motivo_id, string motivotxt)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_cliente", id_cliente.ToString()),
                new KeyValuePair<string, string>("id_status", id_status.ToString()),
                new KeyValuePair<string, string>("motivo_id", motivo_id.ToString()),
                new KeyValuePair<string, string>("motivotxt", motivotxt.ToString())
            });
                var result = client.PostAsync("/Mobile/alteraStatusAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void adicionaProjetoAtivacaoMobile(string ZCLIENTE, int? id_projeto)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("ZCLIENTE", ZCLIENTE.ToString()),
                new KeyValuePair<string, string>("id_projeto", id_projeto.ToString())
            });
                var result = client.PostAsync("/Mobile/adicionaProjetoAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void associaProjetoAtivacaoMobile(int? id_projeto, string ZCLIENTE)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("ZCLIENTE", ZCLIENTE.ToString()),
                new KeyValuePair<string, string>("id_projeto", id_projeto.ToString())
            });
                var result = client.PostAsync("/Mobile/associaProjetoAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }


        }

        public void associaContrapartidaAtivacaoMobile(int? id_contrapartida, int? id_ativacao, int? etapa)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_contrapartida", id_contrapartida.ToString()),
                new KeyValuePair<string, string>("etapa", etapa.ToString()),
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString())
            });
                var result = client.PostAsync("/Mobile/associaContrapartidaAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void procuraClienteMobile(string query, int? id_usuario, string codigo, string razao_social, string cidade, string proximidade, string bairro, string status, string canal, string lat, string longi)
        {

            if (Request.QueryString["query"] == null)
            {
                query = "";
            }
            if (Request.QueryString["id_usuario"] == null)
            {
                id_usuario = 0;
            }
            if (Request.QueryString["codigo"] == null)
            {
                codigo = "";
            }
            if (Request.QueryString["razao_social"] == null)
            {
                razao_social = "";
            }
            if (Request.QueryString["cidade"] == null)
            {
                cidade = "";
            }
            if (Request.QueryString["proximidade"] == null)
            {
                proximidade = "";
            }
            if (Request.QueryString["bairro"] == null)
            {
                bairro = "";
            }
            if (Request.QueryString["status"] == null)
            {
                status = "";
            }
            if (Request.QueryString["canal"] == null)
            {
                canal = "";
            }
            if (Request.QueryString["lat"] == null)
            {
                lat = "";
            }
            if (Request.QueryString["longi"] == null)
            {
                longi = "";
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("query", query),
                new KeyValuePair<string, string>("id_usuario", id_usuario.ToString()),
                new KeyValuePair<string, string>("codigo", codigo),
                new KeyValuePair<string, string>("razao_social", razao_social),
                new KeyValuePair<string, string>("cidade", cidade),
                new KeyValuePair<string, string>("proximidade", proximidade),
                new KeyValuePair<string, string>("bairro", bairro),
                new KeyValuePair<string, string>("status", status),
                new KeyValuePair<string, string>("canal", canal),
                new KeyValuePair<string, string>("lat", lat),
                new KeyValuePair<string, string>("longi", longi)
            });
                var result = client.PostAsync("/Mobile/procuraClienteMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void checaFotoPecaMobile(int? id_peca, int? id_ativacao, int etapa)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_peca", id_peca.ToString()),
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString()),
                new KeyValuePair<string, string>("etapa", etapa.ToString())
            });
                var result = client.PostAsync("/Mobile/checaFotoPecaMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void salvarFotoAtivacaoMobile(ativacao_foto ativacao_foto)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_ativacao", ativacao_foto.id_ativacao.ToString()),
                new KeyValuePair<string, string>("url_foto", ativacao_foto.url_foto.ToString()),
                new KeyValuePair<string, string>("etapa", ativacao_foto.etapa.ToString()),
                new KeyValuePair<string, string>("executivo", ativacao_foto.executivo.ToString()),
                new KeyValuePair<string, string>("id_peca", ativacao_foto.id_peca.ToString())
            });
                var result = client.PostAsync("/Mobile/salvarFotoAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void listaMensagensMobile(int? id_ativacao, int? id_usuario, string id_cliente)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString()),
                new KeyValuePair<string, string>("id_usuario", id_usuario.ToString()),
                new KeyValuePair<string, string>("id_cliente", id_cliente.ToString())
            });
                var result = client.PostAsync("/Mobile/listaMensagensMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void listaChatMobile(int id_mensagem)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_mensagem", id_mensagem.ToString())
            });
                var result = client.PostAsync("/Mobile/listaChatMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }


        }

        public void enviaMensagemChatMobile(chat_mensagem chat_mensagem)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("mensagem_id", chat_mensagem.mensagem_id.ToString()),
                new KeyValuePair<string, string>("ZCLIENTE", chat_mensagem.ZCODCOLAB.ToString()),
                new KeyValuePair<string, string>("remetente_id", chat_mensagem.remetente_id.ToString()),
                new KeyValuePair<string, string>("texto", chat_mensagem.texto.ToString()),
                new KeyValuePair<string, string>("tipo", chat_mensagem.tipo.ToString())
            });
                var result = client.PostAsync("/Mobile/enviaMensagemChatMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void listaEnquetesMobile()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);

                var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("empty", "empty") });
                var result = client.PostAsync("/Mobile/listaEnquetesMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void listaPerguntasEnqueteMobile(int? id_enquete, int? id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_enquete", id_enquete.ToString()),
                new KeyValuePair<string, string>("id", id.ToString())
            });
                var result = client.PostAsync("/Mobile/listaPerguntasEnqueteMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void respondePerguntaEnqueteMobile(enquete_resposta enquete_resposta)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_enquete", enquete_resposta.id_enquete.ToString()),
                new KeyValuePair<string, string>("id", enquete_resposta.id.ToString())
            });
                var result = client.PostAsync("/Mobile/listaPerguntasEnqueteMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void checkoutAtivacaoMobile(int? id_ativacao, int? id_status_ativacao, decimal? custo, int? executivo, string motivotxt, string motivofoto)
        {
            if (Request.QueryString["id_ativacao"] == null)
            {
                id_ativacao = null;
            }
            if (Request.QueryString["id_status_ativacao"] == null)
            {
                id_status_ativacao = null;
            }
            if (Request.QueryString["custo"] == null)
            {
                custo = null;
            }
            if (Request.QueryString["executivo"] == null)
            {
                executivo = null;
            }
            if (Request.QueryString["motivotxt"] == null)
            {
                motivotxt = "";
            }
            if (Request.QueryString["motivofoto"] == null)
            {
                motivofoto = "";
            }


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString()),
                new KeyValuePair<string, string>("id_status_ativacao", id_status_ativacao.ToString()),
                new KeyValuePair<string, string>("custo", custo.ToString()),
                new KeyValuePair<string, string>("executivo", executivo.ToString()),
                new KeyValuePair<string, string>("motivotxt", motivotxt.ToString()),
                new KeyValuePair<string, string>("motivofoto", motivofoto.ToString())
            });
                var result = client.PostAsync("/Mobile/checkoutAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void checaTotalBudgetMobile(int? id_ativacao, decimal total)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString()),
                new KeyValuePair<string, string>("total", total.ToString())
            });
                var result = client.PostAsync("/Mobile/checaTotalBudgetMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void pecasAtivacaoMobile(int? id_ativacao)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString())
            });
                var result = client.PostAsync("/Mobile/pecasAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void alteraPecaAtivacaoMobile(int? id_ativacaopeca, int quantidade)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_ativacaopeca", id_ativacaopeca.ToString()),
                new KeyValuePair<string, string>("quantidade", quantidade.ToString())
            });
                var result = client.PostAsync("/Mobile/alteraPecaAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void removePecaAtivacaoMobile(int? id_pecaativacao)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_pecaativacao", id_pecaativacao.ToString())
            });
                var result = client.PostAsync("/Mobile/removePecaAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void checaPecaAtivacaoMobile(int? id_ativacao, int? id_peca)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString()),
                new KeyValuePair<string, string>("id_peca", id_peca.ToString())
            });
                var result = client.PostAsync("/Mobile/checaPecaAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void dadosPecaMobile(int? id_peca)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_peca", id_peca.ToString())
            });
                var result = client.PostAsync("/Mobile/dadosPecaMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void mostraMensagemDoDiaMobile()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);

                var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("empty", "empty") });
                var result = client.PostAsync("/Mobile/mostraMensagemDoDiaMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void trocaFotoProspector([Bind(Include = "ZCODCOLAB, foto")] colaborador_SAP colaborador)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("ZCODCOLAB", colaborador.ZCODCOLAB.ToString()),
                new KeyValuePair<string, string>("foto", colaborador.foto.ToString())
            });
                var result = client.PostAsync("/Mobile/trocaFotoProspector", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void fotoPecaMobile(int? id_peca)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_peca", id_peca.ToString())
            });
                var result = client.PostAsync("/Mobile/fotoPecaMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void listaMotivosDeRecusa()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);

                var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("empty", "empty") });
                var result = client.PostAsync("/Mobile/listaMotivosDeRecusa", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void detalhesEnqueteMobile(int? id_enquete)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_enquete", id_enquete.ToString())
            });
                var result = client.PostAsync("/Mobile/detalhesEnqueteMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void etapasAtivacaoMobile(int? etapa, int? id_ativacao)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("etapa", etapa.ToString()),
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString())
            });
                var result = client.PostAsync("/Mobile/etapasAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void listaProjetosClienteMobile(string id_cliente)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_cliente", id_cliente.ToString())
            });
                var result = client.PostAsync("/Mobile/listaProjetosClienteMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void listaContrapartidaAtivacaoMobile(int? id_ativacao)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString())
            });
                var result = client.PostAsync("/Mobile/listaContrapartidaAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void listaContrapartidasClienteMobile(string id_cliente)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_cliente", id_cliente.ToString())
            });
                var result = client.PostAsync("/Mobile/listaContrapartidasClienteMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void alteraQuantidadePeca(string id_peca, int id_ativacao, int? quantidadepeca)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_peca", id_peca.ToString()),
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString()),
                new KeyValuePair<string, string>("quantidadepeca", quantidadepeca.ToString())
            });
                var result = client.PostAsync("/Mobile/alteraQuantidadePeca", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void verificaEtapaAtivacaoMobile(int? id_ativacao)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString())
            });
                var result = client.PostAsync("/Mobile/verificaEtapaAtivacaoMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void sincronizaDadosMobile(int? id_colaborador)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_colaborador", id_colaborador.ToString())
            });
                var result = client.PostAsync("/Mobile/sincronizaDadosMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void listaProjetoClienteMobile(string id_cliente)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_cliente", id_cliente.ToString())
            });
                var result = client.PostAsync("/Mobile/listaProjetoClienteMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void listaStatusAtivacoes()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("empty", "empty") });
                var result = client.PostAsync("/Mobile/listaStatusAtivacoes", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void listaBuscaMobile(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id", id.ToString())
            });
                var result = client.PostAsync("/Mobile/listaBuscaMobile", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void verificaItem(int? id_ativacao)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString())
            });
                var result = client.PostAsync("/Mobile/verificaItem", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void adicionaFotoRecusado(string cliente, string foto)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("cliente", cliente.ToString()),
                new KeyValuePair<string, string>("foto", foto.ToString())
            });
                var result = client.PostAsync("/Mobile/adicionaFotoRecusado", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void verificaAtivacao(string id_cliente)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_cliente", id_cliente.ToString())
            });
                var result = client.PostAsync("/Mobile/verificaAtivacao", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void etapasAtivacaoMobile2(int? etapa, int? id_ativacao)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("etapa", etapa.ToString()),
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString())
            });
                var result = client.PostAsync("/Mobile/etapasAtivacaoMobile2", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void uploadDadosNovaAtivacao(List<ativacao> ativacao, List<contrapartida_ativacoes> ca, List<ativacao_foto> af, List<ativacao_peca> ap)
        {
            var ativacaoarray = ativacao.ToArray();
            var ativacaostring = ativacaoarray.ToString();

            var caarray = ativacao.ToArray();
            var castring = caarray.ToString();

            var afarray = ativacao.ToArray();
            var afstring = afarray.ToString();

            var aparray = ativacao.ToArray();
            var apstring = aparray.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
                    {
                        new KeyValuePair<string, string>("ativacao", ativacaoarray.ToList().ToString()),
                        new KeyValuePair<string, string>("af", afstring.ToList().ToString()),
                        new KeyValuePair<string, string>("ap", apstring.ToList().ToString()),
                        new KeyValuePair<string, string>("ca", castring.ToList().ToString()),
                    });
                var result = client.PostAsync("/Mobile/uploadDadosAtualizaAtivacao", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void uploadDadosAtualizaAtivacao(List<ativacao> ativacao, List<contrapartida_ativacoes> ca, List<ativacao_foto> af, List<ativacao_peca> ap)
        {
            var ativacaoarray = ativacao.ToArray();
            var ativacaostring = ativacaoarray.ToString();

            var caarray = ativacao.ToArray();
            var castring = caarray.ToString();

            var afarray = ativacao.ToArray();
            var afstring = afarray.ToString();

            var aparray = ativacao.ToArray();
            var apstring = aparray.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
                    {
                        new KeyValuePair<string, string>("ativacao", ativacaostring),
                        new KeyValuePair<string, string>("af", afstring),
                        new KeyValuePair<string, string>("ap", apstring),
                        new KeyValuePair<string, string>("ca", castring),
                    });
                var result = client.PostAsync("/Mobile/uploadDadosAtualizaAtivacao", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }
        }

        public void uploadDadosCliente(List<cliente_SAP> cliente)
        {

            foreach (var item in cliente)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(local);
                    var content = new FormUrlEncodedContent(new[] 
                        {
                            new KeyValuePair<string, string>("ZCLIENTE", item.ZCLIENTE.ToString()),
                            new KeyValuePair<string, string>("status_ativacao", item.status_ativacao.ToString()),
                            /*new KeyValuePair<string, string>("motivo_id", item.motivo_id),
                            new KeyValuePair<string, string>("ativacao_ultima_atualizacao", item.ativacao_ultima_atualizacao == null ? "" : item.ativacao_ultima_atualizacao.ToString()),*/
                            new KeyValuePair<string, string>("motivotxt", item.motivotxt == null ? "" : item.motivotxt.ToString()),
                            new KeyValuePair<string, string>("observacao", item.observacao == null ? "" : item.observacao.ToString()),
                            new KeyValuePair<string, string>("recusa_foto", item.recusa_foto == null ? "" : item.recusa_foto.ToString()),
                            new KeyValuePair<string, string>("pendente_retorno", item.pendente_retorno == null ? "" : item.pendente_retorno.ToString()),
                            new KeyValuePair<string, string>("motivo_retorno", item.motivo_retorno == null ? "" : item.motivo_retorno.ToString()),
                            new KeyValuePair<string, string>("carta_foto", item.carta_foto.ToString()),
                        });
                    var result = client.PostAsync("/Mobile/uploadDadosCliente", content).Result;
                    string resultContent = result.Content.ReadAsStringAsync().Result;
                    Response.Write(resultContent);
                }
            }

        }

        public int uploadDadosClienteProjeto2(List<cliente_projeto> cliente_projeto)
        {
            foreach (var item in cliente_projeto)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(local);
                    var content = new FormUrlEncodedContent(new[] 
                        {
                            new KeyValuePair<string, string>("ZCLIENTE", item.ZCLIENTE.ToString()),
                            new KeyValuePair<string, string>("id_projeto", item.id_projeto.ToString())
                        });
                    var result = client.PostAsync("/Mobile/uploadDadosClienteProjeto2", content).Result;
                    string resultContent = result.Content.ReadAsStringAsync().Result;
                    Response.Write(resultContent);
                }
            }

            return 0;
        }

        public void listaContrapartidasBackoffice(int? id_ativacao, string id_cliente)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_ativacao", id_ativacao.ToString()),
                new KeyValuePair<string, string>("id_cliente", id_cliente.ToString())
            });
                var result = client.PostAsync("/Mobile/listaContrapartidasBackoffice", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }

        }

        public void GravaObservacaoCliente(string id_cliente, string observacao)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(local);
                var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("id_cliente", id_cliente.ToString()),
                new KeyValuePair<string, string>("observacao", observacao.ToString())
            });
                var result = client.PostAsync("/Mobile/GravaObservacaoCliente", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                Response.Write(resultContent);
            }


        }

    }
}