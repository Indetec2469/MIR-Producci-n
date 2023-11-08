using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Almacenes.Reportes
{
    public class ReportesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "almacenes/reportes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Almacenes_Reportes",
                "almacenes/reportes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "RequisicionesAlmacen.Areas.Almacenes.Reportes.Controllers" }
            );
        }
    }
}