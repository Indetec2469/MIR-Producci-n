using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using RequisicionesAlmacen.Areas.MIR.Catalogos.Models;
using RequisicionesAlmacen.Areas.MIR.Catalogos.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.MIR.Catalogos.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.PLAN_DE_DESARROLLO)]
    public class PlanDesarrolloController : BaseController<PlanDesarrolloViewModel, PlanDesarrolloViewModel>
    {
        public override ActionResult Nuevo()
        {
            //Crear un objeto nuevo
            PlanDesarrolloViewModel planDesarrolloViewModel = new PlanDesarrolloViewModel();
            // Asignamos el modelo al modelView
            planDesarrolloViewModel.PlanDesarrollo = new MItblPlanDesarrollo();
            //Inicializamos el ID en 0 para indicar que es un Registro Nuevo
            planDesarrolloViewModel.PlanDesarrollo.PlanDesarrolloId = 0;
            planDesarrolloViewModel.PlanDesarrollo.FechaInicio = DateTime.Now.Date;
            planDesarrolloViewModel.PlanDesarrollo.FechaFin = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
            planDesarrolloViewModel.PlanDesarrollo.EstatusId = ControlMaestroMapeo.EstatusRegistro.ACTIVO;
            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref planDesarrolloViewModel);
            //Retornamos la vista junto con su Objeto Modelo
            return View("PlanDesarrollo", planDesarrolloViewModel);
        }

        public override ActionResult Editar(int id)
        {
            //Crear un objeto nuevo
            PlanDesarrolloViewModel planDesarrolloViewModel = new PlanDesarrolloViewModel();
            //Buscamos el Objeto por el Id que se envio como parametro
            planDesarrolloViewModel.PlanDesarrollo = new PlanDesarrolloService().BuscaPorId(id);           
            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref planDesarrolloViewModel);
            //Retornamos la vista junto con su Objeto Modelo
            return View("PlanDesarrollo", planDesarrolloViewModel);
        }

        [JsonException]
        public override JsonResult Guardar(PlanDesarrolloViewModel planDesarrolloViewModel)
        {
            // Usuario
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            // Plan Desarrollo
            if (planDesarrolloViewModel.PlanDesarrollo != null)
            {
                // Service
                PlanDesarrolloService planDesarrolloService = new PlanDesarrolloService();
                // Si el ID es nuevo para registrar o actualizar
                if (planDesarrolloViewModel.PlanDesarrollo.PlanDesarrolloId > 0)
                {
                    // Verificar si el usuario ha sido modificado y regresa mensaje de error
                    MItblPlanDesarrollo planDesarrollo = planDesarrolloService.BuscaPorId(planDesarrolloViewModel.PlanDesarrollo.PlanDesarrolloId);
                    if (!StructuralComparisons.StructuralEqualityComparer.Equals(planDesarrolloViewModel.PlanDesarrollo.Timestamp, planDesarrollo.Timestamp))
                    {
                        throw new Exception("Plan Desarrollo con el código [" + planDesarrollo.PlanDesarrolloId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                    }

                    planDesarrolloViewModel.PlanDesarrollo.ModificadoPorId = usuarioId;
                    planDesarrolloViewModel.PlanDesarrollo.FechaUltimaModificacion = DateTime.Now;
                }
                else
                {
                    planDesarrolloViewModel.PlanDesarrollo.CreadoPorId = usuarioId;
                }
            }
            // Plan Desarrollo Estructura
            if (planDesarrolloViewModel.ListPlanDesarrolloEstructura != null)
            {
                // Services
                PlanDesarrolloEstructuraService planDesarrolloEstructuraService = new PlanDesarrolloEstructuraService();
                // Sin Actividad es para cuando guardar MIRI para poner el MIRIndicadorId a MIRIndicadorComponenteId
                foreach (MItblPlanDesarrolloEstructura planDesarrolloEstructura in planDesarrolloViewModel.ListPlanDesarrolloEstructura)
                {
                    // Si el ID es nuevo para registrar o actualizar
                    if (planDesarrolloEstructura.PlanDesarrolloEstructuraId > 0)
                    {
                        // Verificar si el usuario ha sido modificado y regresa mensaje de error
                        MItblPlanDesarrolloEstructura _planDesarrolloEstructura = planDesarrolloEstructuraService.BuscaPorId(planDesarrolloEstructura.PlanDesarrolloEstructuraId);
                        if (!StructuralComparisons.StructuralEqualityComparer.Equals(planDesarrolloEstructura.Timestamp, _planDesarrolloEstructura.Timestamp))
                        {
                            throw new Exception("Plan Desarrollo Estructura con el código [" + _planDesarrolloEstructura.PlanDesarrolloEstructuraId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                        }

                        if(planDesarrolloEstructura.EstatusId == EstatusRegistro.BORRADO)
                        {
                            //Verificar que no este siendo utilizado
                            bool permiteEliminar = new SistemaService().PermiteEliminarRegistro(planDesarrolloEstructura.PlanDesarrolloEstructuraId, "MItblPlanDesarrolloEstructura");
                            if (!permiteEliminar)
                            {
                                throw new Exception("Plan Desarrollo Estructura con el código [" + planDesarrolloEstructura.PlanDesarrolloEstructuraId + "] no puede ser eliminado ya que está siendo utilizado para otros procesos.");
                            }
                        }

                        planDesarrolloEstructura.ModificadoPorId = usuarioId;
                        planDesarrolloEstructura.FechaUltimaModificacion = DateTime.Now;
                    }
                    else
                    {
                        planDesarrolloEstructura.CreadoPorId = usuarioId;
                    }
                }
            }

            new PlanDesarrolloService().GuardaCambios(planDesarrolloViewModel.PlanDesarrollo, planDesarrolloViewModel.ListPlanDesarrolloEstructura);

            return Json("Registro guardado con Exito!");
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult Eliminar(MItblPlanDesarrollo planDesarrollo)
        {
            // Services
            PlanDesarrolloService planDesarrolloService = new PlanDesarrolloService();
            PlanDesarrolloEstructuraService planDesarrolloEstructuraService = new PlanDesarrolloEstructuraService();
            // Plan Nacional Desarrollo
            // Verificar si el usuario ha sido modificado y regresa mensaje de error
            MItblPlanDesarrollo _planDesarrollo = planDesarrolloService.BuscaPorId(planDesarrollo.PlanDesarrolloId);
            if (!StructuralComparisons.StructuralEqualityComparer.Equals(planDesarrollo.Timestamp, _planDesarrollo.Timestamp))
            {
                throw new Exception("El Plan de Desarrollo con el código [" + planDesarrollo.PlanDesarrolloId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            //Verificar que no este siendo utilizado
            bool permiteEliminar = new SistemaService().PermiteEliminarRegistro(planDesarrollo.PlanDesarrolloId, "MItblPlanDesarrollo");
            if (!permiteEliminar)
            {
                throw new Exception("El Plan de Desarollo [" + planDesarrollo.PlanDesarrolloId + "] no puede ser eliminado ya que está siendo utilizado para otros procesos.");
            }

            // Ususario
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;

            planDesarrollo.EstatusId = EstatusRegistro.BORRADO;
            planDesarrollo.ModificadoPorId = usuarioId;
            planDesarrollo.FechaUltimaModificacion = DateTime.Now;

            //planNacionalDesarrolloService.GuardaCambios(planDesarrollo);
            // Plan Nacional Desarrollo Estructura
            IEnumerable<MItblPlanDesarrolloEstructura> listaPlanDesarrolloEstructura = planDesarrolloEstructuraService.BuscaPorPlanDesarrolloId(planDesarrollo.PlanDesarrolloId);
            listaPlanDesarrolloEstructura.ToList().ForEach(pnde =>
            {
                pnde.EstatusId = EstatusRegistro.BORRADO;
                pnde.ModificadoPorId = usuarioId;
                pnde.FechaUltimaModificacion = DateTime.Now;

                //planDesarrolloEstructuraService.GuardaCambios(pnde);
            });

            planDesarrolloService.GuardaCambios(planDesarrollo, listaPlanDesarrolloEstructura);
            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Registro eliminado con Exito!");
        }

        public override ActionResult Listar()
        {
            PlanDesarrolloViewModel planDesarrolloViewModel = new PlanDesarrolloViewModel();

            planDesarrolloViewModel.ListPlanDesarrollo = new PlanDesarrolloService().BuscaListado();

            return View("ListadoPlanDesarrollo", planDesarrolloViewModel);
        }

        protected override void GetDatosFicha(ref PlanDesarrolloViewModel planDesarrolloViewModel)
        {
            //Asignamos los detalles al viewModel
            planDesarrolloViewModel.ListPlanDesarrolloEstructura = new PlanDesarrolloEstructuraService().BuscaPorPlanDesarrolloId(planDesarrolloViewModel.PlanDesarrollo.PlanDesarrolloId);
            planDesarrolloViewModel.ListMITipoPlanDesarrollo = new ControlMaestroService().BuscaControl("MITipoPlanDesarrollo");

            //Ejercicio para los campos de Fecha
            planDesarrolloViewModel.EjercicioUsuario = SessionHelper.GetUsuario().Ejercicio;
        }
    }
}