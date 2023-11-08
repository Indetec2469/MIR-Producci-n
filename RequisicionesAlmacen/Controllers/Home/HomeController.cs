using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Controllers.Home
{
    [Authenticated(nodoMenuId = 0)]
    public class HomeController : Controller 
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuPrincipal()
        {
            try
            {
                return View("_MenuPrincipal");
            }
            catch (Exception ex)
            {
                return Content("Error");
            }
        }
    }
}