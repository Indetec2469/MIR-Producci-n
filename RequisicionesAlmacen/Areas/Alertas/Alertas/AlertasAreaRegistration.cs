using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Alertas.Alertas
{
    public class AlertasAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "alertas/alertas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Alertas",
                "alertas/alertas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "RequisicionesAlmacen.Areas.Alertas.Alertas.Controllers" }
            );
        }
    }
}