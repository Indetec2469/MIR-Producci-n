using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using RequisicionesAlmacen.Areas.Sistemas.Sistema.Models;
using RequisicionesAlmacen.Areas.Sistemas.Sistema.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.Compras;
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using SACG.sysSacg.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Sistemas.Sistema.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.SISTEMA_CONFIGURACION)]
    public class ConfiguracionController : BaseController<GRtblConfiguracionEnte, ConfiguracionViewModel>
    {
        private string API_FICHA = "/sistemas/sistema/configuracion/";

        public ActionResult Index()
        {
            ConfiguracionViewModel viewModel = new ConfiguracionViewModel();

            GetDatosFicha(ref viewModel);

            return View("Configuracion", viewModel);
        }

        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public override JsonResult Guardar(GRtblConfiguracionEnte configuracion)
        {
            ConfiguracionEnteService service = new ConfiguracionEnteService();

            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            //Si es un nuevo registro llenamos el campo de creadoPor
            if (configuracion.ConfiguracionEnteId == 0)
            {
                configuracion.EstatusId = EstatusRegistro.ACTIVO;
                configuracion.CreadoPorId = usuarioId;
            }

            //De lo contrario llenamos el campo de ModificadoPor y Fecha de Ultima Modificacion
            else
            {
                GRtblConfiguracionEnte temp = service.BuscaPorId(configuracion.ConfiguracionEnteId);

                if (!StructuralComparisons.StructuralEqualityComparer.Equals(configuracion.Timestamp, temp.Timestamp))
                {
                    throw new Exception("La Configuración ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
                }

                configuracion.ModificadoPorId = usuarioId;
                configuracion.FechaUltimaModificacion = fecha;
            }

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json(service.GuardaCambios(configuracion), "Registro guardado con Exito!");
        }

        public override ActionResult Listar()
        {
            throw new NotImplementedException();
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ConfiguracionViewModel viewModel)
        {
            viewModel.ConfiguracionEnte = new ConfiguracionEnteService().BuscaConfigutacionEnte();
            viewModel.ListTipoConfiguracionFecha = new ControlMaestroService().BuscaControl("GRTipoConfiguracionFecha");

            //Ejercicio para los campos de Fecha
            viewModel.EjercicioUsuario = SessionHelper.GetUsuario().Ejercicio;
        }
    }
}