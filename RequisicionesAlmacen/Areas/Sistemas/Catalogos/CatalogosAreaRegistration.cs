using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Sistemas.Catalogos
{
    public class CatalogosAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "sistemas/catalogos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sistemas_Catalogos",
                "sistemas/catalogos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "RequisicionesAlmacen.Areas.Sistemas.Catalogos.Controllers" }
            );
        }
    }
}