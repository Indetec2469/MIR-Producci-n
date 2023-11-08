using Newtonsoft.Json;
using RequisicionesAlmacen.Areas.Alertas.Alertas.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Alertas.Alertas.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.CONFIGURACION)]
    public class ConfiguracionController : BaseController<GRtblAlertaConfiguracion, ConfiguracionViewModel>
    {
        public ActionResult Index()
        {
            ConfiguracionViewModel viewModel = new ConfiguracionViewModel();

            GetDatosFicha(ref viewModel);

            return View("Configuracion", viewModel);
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
        public override JsonResult Guardar(GRtblAlertaConfiguracion modelo)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult GuardaCambios(List<GRtblAlertaConfiguracion> detalles)
        {
            AlertaConfiguracionService service = new AlertaConfiguracionService();

            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            List<GRtblAlertaConfiguracion> detallesTemp = new List<GRtblAlertaConfiguracion>();

            foreach (GRtblAlertaConfiguracion registro in detalles)
            {
                //Guardamos solo los Nuevos o Eliminados
                if (registro.AlertaConfiguracionId < 0 || registro.EstatusId != EstatusRegistro.ACTIVO)
                {
                    //Asignamos el Estaus del registro
                    registro.EstatusId = registro.EstatusId != EstatusRegistro.BORRADO ? EstatusRegistro.ACTIVO : registro.EstatusId;

                    //Si es un nuevo registro llenamos el campo de creadoPor
                    if (registro.AlertaConfiguracionId < 0)
                    {
                        //Validamos la configuración
                        GRtblAlertaConfiguracion temp = service.ExisteConfiguracion(registro.AlertaConfiguracionId,
                                                                                    registro.AlertaEtapaAccionId,
                                                                                    registro.EmpleadoId,
                                                                                    registro.FiguraId,
                                                                                    registro.TipoNotificacionId);

                        if (temp != null)
                        {
                            throw new Exception("Ya existe un registro con la misma configuración. Favor de verificar.");
                        }

                        registro.CreadoPorId = usuarioId;
                    }

                    //De lo contrario llenamos el campo de ModificadoPor y Fecha de Ultima Modificacion
                    else
                    {
                        GRtblAlertaConfiguracion temp = service.BuscaPorId(registro.AlertaConfiguracionId);

                        if (!StructuralComparisons.StructuralEqualityComparer.Equals(registro.Timestamp, temp.Timestamp))
                        {
                            throw new Exception("La Configuración [" + service.GetNombreEtapaAccion(registro.AlertaEtapaAccionId) + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
                        }

                        registro.ModificadoPorId = usuarioId;
                        registro.FechaUltimaModificacion = fecha;

                        //Verificamos si es posible eliminar
                        if (registro.EstatusId.Equals(EstatusRegistro.BORRADO) && !new SistemaService().PermiteEliminarRegistro(registro.AlertaConfiguracionId, Tablas.GRtblAlertaConfiguracion))
                        {
                            throw new Exception("El Configuración [" + service.GetNombreEtapaAccion(registro.AlertaEtapaAccionId) + "] no puede ser eliminado ya que está siendo utilizado para otros procesos.");
                        }
                    }

                    detallesTemp.Add(registro);
                }
            }

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json(service.GuardaCambios(detallesTemp), "Registros guardados con Exito!");
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Listar()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ConfiguracionViewModel viewModel)
        {
            viewModel.ListMenuPrincipal = new AlertaConfiguracionService().BuscaListadoMenuPrincipal();
            viewModel.ListTipoNotificacion = new ControlMaestroService().BuscaControl("TipoNotificacionAlerta");
            viewModel.ListEmpleado = new AlertaConfiguracionService().BuscaListadoEmpleados();
            viewModel.ListAlertaConfiguracion = new AlertaConfiguracionService().BuscaListadoAlertaConfiguracion();
        }
    }
}