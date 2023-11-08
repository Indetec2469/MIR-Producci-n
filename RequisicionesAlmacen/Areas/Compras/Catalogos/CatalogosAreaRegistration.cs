using System.Web.Http;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Compras.Catalogos
{
    public class CatalogosAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "compras/catalogos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                name: "CatalogosAPI",
                routeTemplate: "compras/catalogos/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
               );
            context.MapRoute(
                "Compras_Catalogos",
                "compras/catalogos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "RequisicionesAlmacen.Areas.Compras.Catalogos.Controllers" }
            );
        }
    }
}