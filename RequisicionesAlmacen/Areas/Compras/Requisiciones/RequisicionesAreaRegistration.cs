using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Compras.Requisiciones
{
    public class RequisicionesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Compras/Requisiciones";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Requisiciones",
                "compras/requisiciones/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "RequisicionesAlmacen.Areas.Compras.Requisiciones.Controllers" }
            );
        }
    }
}