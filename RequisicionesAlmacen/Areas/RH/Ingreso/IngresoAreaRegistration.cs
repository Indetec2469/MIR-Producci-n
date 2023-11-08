using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.RH.Ingreso
{

    public class IngresoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RH/Ingreso";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //context.MapRoute(
            //    "RH_default",
            //    "RH/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "PROMOCION_default",
            //    "RH/PROMOCION/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional },
            //    new [] { "RequisicionesAlmacen.Areas.RH.PROMOCION.Controllers" }
            //);

            //context.MapRoute(
            //    "INGRESO_default",
            //    "RH/INGRESO/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional },
            //    new[] { "RequisicionesAlmacen.Areas.RH.INGRESO.Controllers" }
            //);

            context.MapRoute(
                "Ingreso",
                "rh/ingreso/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "RequisicionesAlmacen.Areas.RH.Ingreso.Controllers" }
            );
        }
    }
}