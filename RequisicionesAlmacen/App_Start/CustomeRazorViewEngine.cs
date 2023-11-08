using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.App_Start
{
    public class CustomeRazorViewEngine : RazorViewEngine
    {
        public CustomeRazorViewEngine()
        {
            AreaViewLocationFormats = new[]
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml"
            };
            AreaMasterLocationFormats = new[]
            {
                 "~/Areas/{2}/Views/{1}/{0}.cshtml",
                 "~/Areas/{2}/Views/Shared/{0}.cshtml"
            };
            AreaPartialViewLocationFormats = new[]
            {
                 "~/Areas/{2}/Views/{1}/{0}.cshtml",
                 "~/Areas/{2}/Views/Shared/{0}.cshtml"
             };
            ViewLocationFormats = new[]
            {
                 "~/Views/{1}/{0}.cshtml",
                 "~/Views/Shared/{0}.cshtml"
             };
            MasterLocationFormats = new[]
            {
                 "~/Views/{1}/{0}.cshtml",
                 "~/Views/Shared/{0}.cshtml"
             };
            PartialViewLocationFormats = new[]
            {
                 "~/Views/{1}/{0}.cshtml",
                 "~/Views/Shared/{0}.cshtml"
            };
        }
    }
}