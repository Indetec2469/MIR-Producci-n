using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RequisicionesAlmacen.Helpers
{
    public class AuthenticatedAttribute : ActionFilterAttribute
    {
        public int nodoMenuId;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!SessionHelper.ExisteSesion())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Login",
                    action = "Login",
                    area = ""
                }));
            }
            else
            {
                List<int> listaNodoMenuId = SessionHelper.GetNodoMenuId();
                if(listaNodoMenuId != null && listaNodoMenuId.Count > 0)
                {
                    if (!listaNodoMenuId.Any(_nodoMenuId => _nodoMenuId == nodoMenuId))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            controller = "Home",
                            action = "Index",
                            area = ""
                        }));
                    }
                }
                else
                {
                    SessionHelper.CierraSesion();
                }
            }
        }
    }

    // Si estamos logeado ya no podemos acceder a la página de Login
    public class NoneAuthenticatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (SessionHelper.ExisteSesion())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "Index"
                }));
            }
        }
    }

    public class JsonExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public virtual void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.Exception != null)
            {
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                filterContext.Result = new JsonResult() { Data = filterContext.Exception.Message };
            }
        }
    }
}