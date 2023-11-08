using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.MIR.Reportes
{
    public class ReportesMIRAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "mir/reportes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MIR_Reportes",
                "mir/reportes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "RequisicionesAlmacen.Areas.MIR.Reportes.Controllers" }
            );
        }
    }
}