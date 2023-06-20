using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FemsaEP.Models;
using System.DirectoryServices;

namespace FemsaEP.Controllers
{
    public class LoginController : Controller
    {
        private MC1_DB_FEMSAEntities db = new MC1_DB_FEMSAEntities();
        //
        // GET: /Login/
        public ActionResult Login()
        {   
            return View();
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["id"] = null;
            return RedirectToAction("Login", "Login");
        }


        [HttpPost]
        public ActionResult Login(administrador administrador)
        {

            string path = @"LDAP://10.137.64.31";       //CAMBIAR POR VUESTRO PATH (URL DEL SERVICIO DE DIRECTORIO LDAP)
                                                          //Por ejemplo: 'LDAP://ejemplo.lan.es'
            string dominio = @"10.137.64.116";             //CAMBIAR POR VUESTRO DOMINIO
            string usu = administrador.login.Trim();      //USUARIO DEL DOMINIO
            string pass = administrador.senha.Trim();     //PASSWORD DEL USUARIO
            string domUsu = dominio + @"\" + usu;         //CADENA DE DOMINIO + USUARIO A COMPROBAR


            bool permiso = AutenticaUsuario(path, domUsu, pass);

            if (permiso)
            {
                var verifica = db.administrador.Where(a => a.login == usu).FirstOrDefault();
                if (verifica != null)
                {
                    Session["user"] = verifica.login;
                    Session["id"] = verifica.id;
                }
                else
                {
                    administrador.login = usu;
                    administrador.senha = null;
                    db.administrador.Add(administrador);
                    db.SaveChanges();
                    Session["user"] = administrador.login;
                    Session["id"] = administrador.id;     
                }

                 return RedirectToAction("Index", "Home");
            }
            else
            {
               Response.Write("<script>alert('Login ou Senha incorretos.')</script>");
            }

            return View();
        }


        private bool AutenticaUsuario(String path, String user, String pass)
        {
            //Los datos que hemos pasado los 'convertimos' en una entrada de Active Directory para hacer la consulta
            DirectoryEntry de = new DirectoryEntry(path, user, pass, AuthenticationTypes.Secure);
            try
            {
                //Inicia el chequeo con las credenciales que le hemos pasado
                //Si devuelve algo significa que ha autenticado las credenciales
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.FindOne();
                return true;
            }
            catch
            {
                //Si no devuelve nada es que no ha podido autenticar las credenciales
                //ya sea porque no existe el usuario o por que no son correctas
                return false;
            }
        }

	}
}