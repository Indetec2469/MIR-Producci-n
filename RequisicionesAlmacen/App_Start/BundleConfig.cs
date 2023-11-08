using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace RequisicionesAlmacen {

    public class BundleConfig {

        public static void RegisterBundles(BundleCollection bundles) {

            var scriptBundle = new ScriptBundle("~/Scripts/bundle");
            var styleBundle = new StyleBundle("~/Content/bundle");

            // jQuery
            scriptBundle.Include("~/Content/lib/jquery/jquery.min.js");

            // Bootstrap
            scriptBundle.Include("~/Content/lib/bootstrap/js/bootstrap.bundle.min.js");

            // Bootstrap
            styleBundle.Include("~/Content/bootstrap.css");

            // Custom site styles
            styleBundle.Include("~/Content/Site.css");

            bundles.Add(scriptBundle);
            bundles.Add(styleBundle);

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}