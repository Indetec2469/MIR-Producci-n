using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Compras.Reportes
{
    public class ReportesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "compras/reportes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Compras_Reportes",
                "compras/reportes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "RequisicionesAlmacen.Areas.Compras.Reportes.Controllers" }
            );
        }
    }
}