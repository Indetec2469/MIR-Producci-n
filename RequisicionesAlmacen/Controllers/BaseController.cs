using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Controllers
{
    public abstract class BaseController<T, T2> : Controller
    {
        /// <summary>Método para Iniciar un Nuevo Formulario </summary>
        /// <returns>Una <c>Nueva Vista</c> para el tipo de Modelo a crear</returns>
        public abstract ActionResult Nuevo();

        /// <summary>Método para Editar un Formulario </summary>
        /// <returns>Una <c>Vista de Edición</c> para el tipo de Modelo a modificar</returns>
        [HttpGet]
        public abstract ActionResult Editar(int id);

        /// <summary>Método para Guardar la información enviada desde el Formulario </summary>
        /// <returns>Objeto <c>JSON</c> con la respuesta de la operación</returns>
        [HttpPost]
        public abstract JsonResult Guardar(T modelView);

        /// <summary>Método para Eliminar un registro enviado desde un Formulario </summary>
        /// <returns>Objeto <c>JSON</c> con la respuesta de la operación</returns>
        [HttpDelete]
        public abstract JsonResult Eliminar(int id);

        /// <summary>Método para Listar registros en Formulario </summary>
        /// <returns>>Una <c>Nueva Vista</c> con todos los registros de un tipo de Modelo</returns>
        public abstract ActionResult Listar();

        /// <summary>Método para agregar al Modelo datos necesarios para el funcionamiento de un Formulario </summary>
        protected abstract void GetDatosFicha(ref T2 modelView);

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Verificamos si existe un error que mostrar
            ViewBag.Error = GetViewBagError();

            //Verificamos si existe un warning que mostrar
            ViewBag.Warning = GetViewBagWarning();
        }

        /// <summary>Método para asignar un Error que mostrar </summary>
        public void SetViewBagError(string mensaje)
        {
            SetViewBagError(mensaje, null);
        }

        /// <summary>Método para asignar un Error que mostrar y el Path para redireccionar </summary>
        public void SetViewBagError(string mensaje, string rutaRedireccionarA)
        {
            Session["ViewBagError"] = mensaje;
            ViewBag.RedireccionarA = rutaRedireccionarA;
        }

        /// <summary>Método para verificar si existe un Error que mostrar </summary>
        private string GetViewBagError()
        {
            string ViewBagError = Session["ViewBagError"] != null ? Session["ViewBagError"].ToString() : "";
            Session["ViewBagError"] = "";

            return ViewBagError;
        }

        /// <summary>Método para asignar un Warning que mostrar </summary>
        public void SetViewBagWarning(string mensaje)
        {
            Session["ViewBagWarning"] = mensaje;
        }

        /// <summary>Método para verificar si existe un Warning que mostrar </summary>
        private string GetViewBagWarning()
        {
            string ViewBagError = Session["ViewBagWarning"] != null ? Session["ViewBagWarning"].ToString() : "";
            Session["ViewBagWarning"] = "";

            return ViewBagError;
        }
    }
}