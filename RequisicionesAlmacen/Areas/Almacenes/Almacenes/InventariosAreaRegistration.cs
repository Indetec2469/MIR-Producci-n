using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Almacenes.Almacenes
{
    public class AlmacenesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "almacenes/almacenes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Almacenes",
                "almacenes/almacenes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "RequisicionesAlmacen.Areas.Almacenes.Almacenes.Controllers" }
            );
        }
    }
}