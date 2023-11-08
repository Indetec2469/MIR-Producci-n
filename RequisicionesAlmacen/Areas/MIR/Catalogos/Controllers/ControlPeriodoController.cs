using RequisicionesAlmacen.Areas.MIR.Catalogos.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.MIR.Catalogos.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.CONTROL_PERIODO)]
    public class ControlPeriodoController : BaseController<ControlMaestroControlPeriodoViewModel, ControlMaestroControlPeriodoViewModel>
    {
        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public override JsonResult Guardar(ControlMaestroControlPeriodoViewModel controlMaestroControlPeriodoViewModel)
        {
            // Usuario
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            // Services
            ControlMaestroControlPeriodoService controlMaestroControlPeriodoService = new ControlMaestroControlPeriodoService();

            List<MItblControlMaestroControlPeriodo> listControlPeriodos = new List<MItblControlMaestroControlPeriodo>();

            if (controlMaestroControlPeriodoViewModel.ListControlMaestroControlPeriodo != null)
            {
                foreach (MItblControlMaestroControlPeriodo controlMaestroControlPeriodo in controlMaestroControlPeriodoViewModel.ListControlMaestroControlPeriodo)
                {
                    // Verificar si el usuario ha sido modificado en misma la ficha y regresa mensaje de error
                    MItblControlMaestroControlPeriodo _controlMaestroControlPeriodo = controlMaestroControlPeriodoService.BuscaPorId(controlMaestroControlPeriodo.ControlPeriodoId);
                    if (!StructuralComparisons.StructuralEqualityComparer.Equals(controlMaestroControlPeriodo.Timestamp, _controlMaestroControlPeriodo.Timestamp))
                    {
                        throw new Exception("El Control Periodo con el código [" + controlMaestroControlPeriodo.Codigo + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                    }

                    controlMaestroControlPeriodo.ModificadoPorId = usuarioId;
                    controlMaestroControlPeriodo.FechaUltimaModificacion = DateTime.Now;
                    listControlPeriodos.Add(controlMaestroControlPeriodo);
                }

                controlMaestroControlPeriodoService.GuardaCambios(listControlPeriodos);

            }

            if(controlMaestroControlPeriodoViewModel.ListControlMaestroControlPeriodo.Count() == 1)
            {
                return Json("Registro con Exito!");
            }
            else
            {
                return Json("Registros con Exito!");
            }
        }

        // GET: MIR/Catalogos/ControlPeriodo
        public override ActionResult Listar()
        {
            ControlMaestroControlPeriodoViewModel controlMaestroControlPeriodoViewModel = new ControlMaestroControlPeriodoViewModel();
            controlMaestroControlPeriodoViewModel.ListControlMaestroControlPeriodo = new ControlMaestroControlPeriodoService().BuscaTodos();
            if(controlMaestroControlPeriodoViewModel.ListControlMaestroControlPeriodo.Count() == 0)
            {
                controlMaestroControlPeriodoViewModel.ListControlMaestroControlPeriodo = NuevoControlPeriodo();
            }
            controlMaestroControlPeriodoViewModel.ListControlMaestroMIEstatusPeriodo = new ControlMaestroService().BuscaControl("MIEstatusPeriodo");
            return View("ListadoControlPeriodo", controlMaestroControlPeriodoViewModel);
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ControlMaestroControlPeriodoViewModel modelView)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<MItblControlMaestroControlPeriodo> NuevoControlPeriodo()
        {
            // Service
            ControlMaestroControlPeriodoService controlMaestroControlPeriodoService = new ControlMaestroControlPeriodoService();
            List<MItblControlMaestroControlPeriodo> listControlMaestroControlPeriodo = new List<MItblControlMaestroControlPeriodo>();
            for(int i = 1; i <= 12; i++)
            {
                MItblControlMaestroControlPeriodo controlMaestroControlPeriodo = new MItblControlMaestroControlPeriodo();
                controlMaestroControlPeriodo.EstatusPeriodoId = ControlMaestroMapeo.MIEstatusPeriodo.CERRADO;
                controlMaestroControlPeriodo.CreadoPorId = SessionHelper.GetUsuario().UsuarioId;
                switch (i)
                {
                    case 1:
                        controlMaestroControlPeriodo.Codigo = "ENE";
                        controlMaestroControlPeriodo.Periodo = "Enero";
                        break;
                    case 2:
                        controlMaestroControlPeriodo.Codigo = "FEB";
                        controlMaestroControlPeriodo.Periodo = "Febrero";
                        break;
                    case 3:
                        controlMaestroControlPeriodo.Codigo = "MAR";
                        controlMaestroControlPeriodo.Periodo = "Marzo";
                        break;
                    case 4:
                        controlMaestroControlPeriodo.Codigo = "ABR";
                        controlMaestroControlPeriodo.Periodo = "Abril";
                        break;
                    case 5:
                        controlMaestroControlPeriodo.Codigo = "MAY";
                        controlMaestroControlPeriodo.Periodo = "Mayo";
                        break;
                    case 6:
                        controlMaestroControlPeriodo.Codigo = "JUN";
                        controlMaestroControlPeriodo.Periodo = "Junio";
                        break;
                    case 7:
                        controlMaestroControlPeriodo.Codigo = "JUL";
                        controlMaestroControlPeriodo.Periodo = "Julio";
                        break;
                    case 8:
                        controlMaestroControlPeriodo.Codigo = "AGO";
                        controlMaestroControlPeriodo.Periodo = "Agosto";
                        break;
                    case 9:
                        controlMaestroControlPeriodo.Codigo = "SEP";
                        controlMaestroControlPeriodo.Periodo = "Septiembre";
                        break;
                    case 10:
                        controlMaestroControlPeriodo.Codigo = "OCT";
                        controlMaestroControlPeriodo.Periodo = "Octubre";
                        break;
                    case 11:
                        controlMaestroControlPeriodo.Codigo = "NOV";
                        controlMaestroControlPeriodo.Periodo = "Noviembre";
                        break;
                    case 12:
                        controlMaestroControlPeriodo.Codigo = "DIC";
                        controlMaestroControlPeriodo.Periodo = "Diciembre";
                        break;
                }
                listControlMaestroControlPeriodo.Add(controlMaestroControlPeriodo);
            }

            controlMaestroControlPeriodoService.GuardaCambios(listControlMaestroControlPeriodo);

            listControlMaestroControlPeriodo = (List<MItblControlMaestroControlPeriodo>) controlMaestroControlPeriodoService.BuscaTodos();

            return listControlMaestroControlPeriodo;
        }
    }
}