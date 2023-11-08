using RequisicionesAlmacenBL.Entities;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Text.Json;

namespace RequisicionesAlmacenBL.Helpers
{
    public static class SessionHelper
    {

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