using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Compras.Compras
{
    public class ComprasAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Compras/Compras";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Compras",
                "compras/compras/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "RequisicionesAlmacen.Areas.Compras.Compras.Controllers" }
            );
        }
    }
}