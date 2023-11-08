using RequisicionesAlmacen.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RequisicionesAlmacen {

    public class MvcApplication : HttpApplication {
        protected void Application_Start() {

            //Remove All View Engine including Webform and Razor
            ViewEngines.Engines.Clear();
            //Register C# Razor View Engine
            ViewEngines.Engines.Add(new CustomeRazorViewEngine());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DevExtremeBundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
