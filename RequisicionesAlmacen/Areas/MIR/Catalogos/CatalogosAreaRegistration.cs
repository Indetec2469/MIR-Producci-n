using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.MIR.Catalogos
{
    public class CatalogosAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "mir/catalogos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MIR_Catalogos",
                "mir/catalogos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "RequisicionesAlmacen.Areas.MIR.Catalogos.Controllers" }
            );
        }
    }
}