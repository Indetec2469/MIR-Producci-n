using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Sistemas.Sistema
{
    public class SistemaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Sistemas/Sistema";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sistema",
                "sistemas/sistema/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "RequisicionesAlmacen.Areas.Sistemas.Sistema.Controllers" }
            );
        }
    }
}