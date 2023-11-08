using Newtonsoft.Json;
using RequisicionesAlmacen.Areas.Alertas.Alertas.Models.ViewModel;
using RequisicionesAlmacen.Areas.Compras.Requisiciones.Models;
using RequisicionesAlmacen.Areas.Compras.Requisiciones.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Alertas.Alertas.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.NOTIFICACIONES)]
    public class NotificacionesController : BaseController<GRtblAlerta, NotificacionesViewModel>
    {
        public ActionResult Index()
        {
            NotificacionesViewModel viewModel = new NotificacionesViewModel();

            GetDatosFicha(ref viewModel);

            return View("Notificaciones", viewModel);
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }
        
        [JsonException]
        public override JsonResult Guardar(GRtblAlerta modelo)
        {
            throw new NotImplementedException();
        }
        
        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Listar()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref NotificacionesViewModel viewModel)
        {
            viewModel.ListAlertas = new AlertaService().GetListadoAlertas(SessionHelper.GetUsuario().UsuarioId);
        }

        [HttpGet]
        [JsonException]
        public JsonResult GetListadoAlertas()
        {
            return Json(new AlertaService().GetListadoAlertas(SessionHelper.GetUsuario().UsuarioId), JsonRequestBehavior.AllowGet);
        }

        [JsonException]
        public JsonResult AutorizarAlerta(int alertaId)
        {
            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json(new AlertaService().AutorizarAlerta(alertaId, SessionHelper.GetUsuario().UsuarioId), "Trámite Autorizado.");
        }

        [JsonException]
        public JsonResult RevisionAlerta(int alertaId, string motivo)
        {
            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json(new AlertaService().RevisionAlerta(alertaId, motivo, SessionHelper.GetUsuario().UsuarioId), "Trámite en Revisión.");
        }

        [JsonException]
        public JsonResult RechazarAlerta(int alertaId, string motivo)
        {
            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json(new AlertaService().RechazarAlerta(alertaId, motivo, SessionHelper.GetUsuario().UsuarioId), "Trámite Rechazado.");
        }

        [JsonException]
        public JsonResult OcultarAlertas(string alertasId)
        {
            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json(new AlertaService().OcultarAlertas(alertasId, SessionHelper.GetUsuario().UsuarioId), "Notificaciones Ocultas.");
        }
    }
}