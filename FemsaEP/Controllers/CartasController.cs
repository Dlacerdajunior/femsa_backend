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


namespace FemsaEP.Controllers
{
    public class CartasController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();

        // GET: /Cartas/
        public ActionResult Index(int? pagina)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var carta = db.carta.Include(c => c.uf);
            int tamanhoPagina = 10;
            int numeroPagina = pagina ?? 1;
                    
            return View(carta.OrderBy(p => p.id).ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: /Cartas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carta carta = db.carta.Find(id);
            if (carta == null)
            {
                return HttpNotFound();
            }
            return View(carta);
        }

        // GET: /Cartas/Create
        public ActionResult Create()
        {
            ViewBag.zatzat = new SelectList(db.zat_SAP, "ZCODZATS", "ZNOMBRE");
            ViewBag.uf_id = new SelectList(db.uf, "id", "uf_sigla");
            return View();
        }

        // POST: /Cartas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "id,codigo,razao,cidade,uf_id,bairro,endereco,contato,telefone,rota_vendedor,cluster,unidade,desenvolvedor,prospector,zat,nome_zat,ano_cliente,ano_contrapartida")] carta carta, HttpPostedFileBase file)
        {
            var fileName = "";

            if (ModelState.IsValid)
            {
                var finalString = GeraCodigo();

                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(finalString + file.FileName).ToString().Replace(" ", "");
                    fileName = removerAcentos(fileName);
                    fileName = removerSpecial(fileName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/CartaPDF"), fileName);
                    file.SaveAs(path);
                }
                if (fileName == "")
                {
                    carta.pdf = null;
                }
                else
                {
                    carta.pdf = "http://10.137.64.116/Content/assets/CartaPDF/" + fileName;
                }


                db.carta.Add(carta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.zatzat = new SelectList(db.zat_SAP, "ZCODZATS", "ZNOMBRE", carta.zat);
            ViewBag.uf_id = new SelectList(db.uf, "id", "uf_sigla", carta.uf_id);
            return View(carta);
        }

        public static string removerAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }

        public static string removerSpecial(string texto)
        {
            string comAcentos = "[!;\\/:*?\"<>|&']";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), "");
            }
            return texto;
        }

        //maquina de codigos para lojas
        public String GeraCodigo()
        {
            var chars = "0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            var confere = db.carta.Where(a => a.pdf == finalString);
            if (confere.Count() != 0)
            {
                var finalString2 = GeraCodigo();
                return finalString2;
            }
            else
            {
                return finalString;
            }
        }

        // GET: /Cartas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carta carta = db.carta.Find(id);
            if (carta == null)
            {
                return HttpNotFound();
            }
            ViewBag.zatzat = new SelectList(db.zat_SAP, "ZCODZATS", "ZNOMBRE", carta.zat);
            ViewBag.uf_id = new SelectList(db.uf, "id", "uf_sigla", carta.uf_id);
            return View(carta);
        }

        // POST: /Cartas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigo,razao,cidade,uf_id,bairro,endereco,contato,telefone,rota_vendedor,cluster,unidade,desenvolvedor,prospector,zat,nome_zat,ano_cliente,ano_contrapartida")] carta carta, HttpPostedFileBase file, string zatzat)
        {
            var fileName = "";

            if (ModelState.IsValid)
            {
                var finalString = GeraCodigo();

                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(finalString + file.FileName).ToString().Replace(" ", "");
                    fileName = removerAcentos(fileName);
                    fileName = removerSpecial(fileName);
                    var path = Path.Combine(Server.MapPath("~/Content/assets/CartaPDF"), fileName);
                    file.SaveAs(path);
                }
                if (fileName == "")
                {
                    carta.pdf = null;
                }
                else
                {
                    carta.pdf = "http://10.137.64.116/Content/assets/CartaPDF/" + fileName;
                }
                if (zatzat != "")
                {
                    carta.zat = Convert.ToInt32(zatzat);
                }
                db.Entry(carta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.zatzat = new SelectList(db.zat_SAP, "ZCODZATS", "ZNOMBRE", carta.zat);
            ViewBag.uf_id = new SelectList(db.uf, "id", "uf_sigla", carta.uf_id);
            return View(carta);
        }

        // GET: /Cartas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carta carta = db.carta.Find(id);
            if (carta == null)
            {
                return HttpNotFound();
            }
            return View(carta);
        }

        // POST: /Cartas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            carta carta = db.carta.Find(id);
            db.carta.Remove(carta);
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
