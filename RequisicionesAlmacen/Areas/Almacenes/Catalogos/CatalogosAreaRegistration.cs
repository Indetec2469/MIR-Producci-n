using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Almacenes.Catalogos
{
    public class CatalogosAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "almacenes/catalogos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Catalogos",
                "almacenes/catalogos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "RequisicionesAlmacen.Areas.Almacenes.Catalogos.Controllers" }
            );
        }
    }
}