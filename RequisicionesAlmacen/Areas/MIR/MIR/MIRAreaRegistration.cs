using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.MIR.MIR
{
    public class MIRAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "mir/mir";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MIR_MIR",
                "mir/mir/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "RequisicionesAlmacen.Areas.MIR.MIR.Controllers" }
            );
        }
    }
}