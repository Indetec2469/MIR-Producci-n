using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Text.Json;

namespace RequisicionesAlmacen.Helpers
{
    public static class SessionHelper
    {

        public static void CreaSesion(UsuarioSesion usuario, List<int> listaNodoMenuId)
        {

            Sesion sesion = new Sesion(usuario, listaNodoMenuId);

            string jsonSesion = JsonSerializer.Serialize(sesion);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, usuario.NombreUsuario, DateTime.Now, DateTime.MaxValue, true, jsonSesion);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
            cookie.Name = FormsAuthentication.FormsCookieName;
            cookie.Expires = DateTime.MaxValue;
            cookie.Value = FormsAuthentication.Encrypt(ticket);


            //FormsAuthenticationTicket ticketMenu = new FormsAuthenticationTicket(1, usuario.NombreUsuario, DateTime.Now, DateTime.MaxValue, true, "1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41");

            //HttpCookie cookieMenu = new HttpCookie("QKIEMenu");
            //cookieMenu.Name = "QKIEMenu";
            //cookieMenu.Expires = DateTime.MaxValue;
            //cookieMenu.Value =  FormsAuthentication.Encrypt(ticketMenu);


            HttpContext.Current.Response.Cookies.Add(cookie);
            //HttpContext.Current.Response.Cookies.Add(cookieMenu);

            //string valor = JsonSerializer.Serialize(listaMenuPrincipal);
            //HttpCookie cookie1 = new HttpCookie("menu", valor);
            //HttpContext.Current.Response.Cookies.Add(cookie1);
        }


        public static bool ExisteSesion()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }


        public static void CierraSesion()
        {
            FormsAuthentication.SignOut();
        }


        public static UsuarioSesion GetUsuario()
        {
            UsuarioSesion usuario = null;

            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is FormsIdentity)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                if (ticket != null)
                {
                    usuario = JsonSerializer.Deserialize<Sesion>(ticket.UserData).Usuario;
                }
            }

            return usuario;
        }

        public static List<int> GetNodoMenuId()
        {
            List<int> listaMenuPrincipal = null;

            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is FormsIdentity)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                if (ticket != null)
                {
                    listaMenuPrincipal = JsonSerializer.Deserialize<Sesion>(ticket.UserData).ListaNodoMenuId;
                }
            }

            return listaMenuPrincipal;
        }
    }

    class Sesion {

        public UsuarioSesion Usuario { get; set; }
        public List<int> ListaNodoMenuId{ get; set; }

        public Sesion() { }

        public Sesion(UsuarioSesion usuario, List<int> listaNodoMenuId)
        {
            Usuario = usuario;
            ListaNodoMenuId = listaNodoMenuId;
        }
    }
}