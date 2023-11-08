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
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.TIPO_DE_INDICADOR)]
    public class TipoIndicadorController : BaseController<ControlMaestroTipoIndicadorViewModel, ControlMaestroTipoIndicadorViewModel>
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
        public override JsonResult Guardar(ControlMaestroTipoIndicadorViewModel controlMaestroTipoIndicadorViewModel)
        {
            throw new NotImplementedException();
            //  // Services
            //ControlMaestroTipoIndicadorService controlMaestroTipoIndicadorService = new ControlMaestroTipoIndicadorService();
            //ControlMaestroTipoIndicadorNivelService controlMaestroTipoIndicadorNivelService = new ControlMaestroTipoIndicadorNivelService();

            //if (controlMaestroTipoIndicadorViewModel.ListControlMaestroTipoIndicador != null)
            //{
            //    foreach (MItblControlMaestroTipoIndicador controlMaestroTipoIndicador in controlMaestroTipoIndicadorViewModel.ListControlMaestroTipoIndicador)
            //    {
            //        // Verificar si el usuario ha sido modificado en mismo Descripción y regresa mensaje de error
            //        if(controlMaestroTipoIndicadorService.EsDescripcionExiste(controlMaestroTipoIndicador))
            //            throw new Exception("El Tipo de Indicador con el descripción [" + controlMaestroTipoIndicador.Descripcion + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");

            //        // Control Maestro Tipo Indicador
            //        MItblControlMaestroTipoIndicador _controlMaestroTipoIndicador = controlMaestroTipoIndicador;
            //        int tipoIndicadorIdPadre = controlMaestroTipoIndicador.TipoIndicadorId;
            //        if (controlMaestroTipoIndicador.TipoIndicadorId > 0)
            //        {
            //            // Verificar si el usuario ha sido modificado en mismo Tipo Indicador y regresa mensaje de error
            //            MItblControlMaestroTipoIndicador __controlMaestroTipoIndicador = controlMaestroTipoIndicadorService.BuscaPorId(controlMaestroTipoIndicador.TipoIndicadorId);
            //            if (!StructuralComparisons.StructuralEqualityComparer.Equals(controlMaestroTipoIndicador.Timestamp, __controlMaestroTipoIndicador.Timestamp))
            //            {
            //                throw new Exception("El Tipo de Indicador con el código [" + controlMaestroTipoIndicador.TipoIndicadorId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
            //            }

            //            controlMaestroTipoIndicador.ModificadoPorId = SessionHelper.GetUsuario().UsuarioId;
            //            controlMaestroTipoIndicador.FechaUltimaModificacion = DateTime.Now;
            //            controlMaestroTipoIndicadorService.Actualiza(controlMaestroTipoIndicador);
            //        }
            //        else
            //        {
            //            controlMaestroTipoIndicador.CreadoPorId = SessionHelper.GetUsuario().UsuarioId;
            //            _controlMaestroTipoIndicador = controlMaestroTipoIndicadorService.Inserta(controlMaestroTipoIndicador);
            //        }

            //        // Control Maestro Tipo Indicador Nivel
            //        if (controlMaestroTipoIndicadorViewModel.ListControlMaestroTipoIndicadorNivel != null)
            //        {
            //            List<MItblControlMaestroTipoIndicadorNivel> listControlMaestroTipoIndicadorNivel = controlMaestroTipoIndicadorViewModel.ListControlMaestroTipoIndicadorNivel.Where(nivel => nivel.TipoIndicadorId == tipoIndicadorIdPadre).ToList();
            //            if (listControlMaestroTipoIndicadorNivel != null)
            //            {
            //                foreach (MItblControlMaestroTipoIndicadorNivel controlMaestroTipoIndicadorNivel in listControlMaestroTipoIndicadorNivel)
            //                {
            //                    if (controlMaestroTipoIndicadorNivel.TipoIndicadorNivelId > 0)
            //                    {
            //                        // Verificar si el usuario ha sido modificado en mismo Tipo Indicador Nivel y regresa mensaje de error
            //                        if (controlMaestroTipoIndicadorNivelService.EsNivelExiste(controlMaestroTipoIndicadorNivel))
            //                            throw new Exception("El Tipo de Indicador Nivel ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");

            //                        // Verificar si el usuario ha sido modificado en mismo Tipo Indicador Nivel y regresa mensaje de error
            //                        MItblControlMaestroTipoIndicadorNivel _controlMaestroTipoIndicadorNivel = controlMaestroTipoIndicadorNivelService.BuscaPorId(controlMaestroTipoIndicadorNivel.TipoIndicadorNivelId);
            //                        if (!StructuralComparisons.StructuralEqualityComparer.Equals(controlMaestroTipoIndicadorNivel.Timestamp, _controlMaestroTipoIndicadorNivel.Timestamp))
            //                        {
            //                            throw new Exception("El Tipo de Indicador Nivel ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
            //                        }

            //                        controlMaestroTipoIndicadorNivel.ModificadoPorId = SessionHelper.GetUsuario().UsuarioId;
            //                        controlMaestroTipoIndicadorNivel.FechaUltimaModificacion = DateTime.Now;
            //                        controlMaestroTipoIndicadorNivelService.Actualiza(controlMaestroTipoIndicadorNivel);
            //                    }
            //                    else
            //                    {
            //                        controlMaestroTipoIndicadorNivel.TipoIndicadorId = _controlMaestroTipoIndicador.TipoIndicadorId;
            //                        // Verificar si el usuario ha sido modificado en mismo Tipo Indicador Nivel y regresa mensaje de error
            //                        if(controlMaestroTipoIndicadorNivelService.EsNivelExiste(controlMaestroTipoIndicadorNivel))
            //                            throw new Exception("El Tipo de Indicador Nivel ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");

            //                        controlMaestroTipoIndicadorNivel.CreadoPorId = SessionHelper.GetUsuario().UsuarioId;
            //                        controlMaestroTipoIndicadorNivelService.Inserta(controlMaestroTipoIndicadorNivel);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    // Control Maestro Tipo Indicador Nivel
            //    if (controlMaestroTipoIndicadorViewModel.ListControlMaestroTipoIndicadorNivel != null)
            //    {
            //        foreach (MItblControlMaestroTipoIndicadorNivel controlMaestroTipoIndicadorNivel in controlMaestroTipoIndicadorViewModel.ListControlMaestroTipoIndicadorNivel)
            //        {
            //            // Verificar si el usuario ha sido modificado en mismo Tipo Indicador Nivel y regresa mensaje de error
            //            if (controlMaestroTipoIndicadorNivelService.EsNivelExiste(controlMaestroTipoIndicadorNivel))
            //                throw new Exception("El Tipo de Indicador Nivel ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");

            //            if (controlMaestroTipoIndicadorNivel.TipoIndicadorNivelId > 0)
            //            {
            //                // Verificar si el usuario ha sido modificado en mismo Tipo Indicador Nivel y regresa mensaje de error
            //                MItblControlMaestroTipoIndicadorNivel _controlMaestroTipoIndicadorNivel = controlMaestroTipoIndicadorNivelService.BuscaPorId(controlMaestroTipoIndicadorNivel.TipoIndicadorNivelId);
            //                if (!StructuralComparisons.StructuralEqualityComparer.Equals(controlMaestroTipoIndicadorNivel.Timestamp, _controlMaestroTipoIndicadorNivel.Timestamp))
            //                {
            //                    throw new Exception("El Tipo de Indicador Nivel ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
            //                }

            //                controlMaestroTipoIndicadorNivel.ModificadoPorId = SessionHelper.GetUsuario().UsuarioId;
            //                controlMaestroTipoIndicadorNivel.FechaUltimaModificacion = DateTime.Now;
            //                controlMaestroTipoIndicadorNivelService.Actualiza(controlMaestroTipoIndicadorNivel);
            //            }
            //            else
            //            {
            //                controlMaestroTipoIndicadorNivel.CreadoPorId = SessionHelper.GetUsuario().UsuarioId;
            //                controlMaestroTipoIndicadorNivelService.Inserta(controlMaestroTipoIndicadorNivel);
            //            }
            //        }
            //    }
            //}
            //return Json("Registros con Exito!");

        }

        // GET: MIR/TipoIndicador/Listar
        public override ActionResult Listar()
        {
            // Crear los objetos
            ControlMaestroTipoIndicadorViewModel controlMaestroTipoIndicadorViewModel = new ControlMaestroTipoIndicadorViewModel();
            GetDatosFicha(ref controlMaestroTipoIndicadorViewModel);

            return View("ListadoTipoIndicador", controlMaestroTipoIndicadorViewModel);
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ControlMaestroTipoIndicadorViewModel controlMaestroTipoIndicadorViewModel)
        {
            controlMaestroTipoIndicadorViewModel.ControlMaestroTipoIndicador = new MItblControlMaestroTipoIndicador();
            controlMaestroTipoIndicadorViewModel.ControlMaestroTipoIndicadorNivel = new MItblControlMaestroTipoIndicadorNivel();
            controlMaestroTipoIndicadorViewModel.ListControlMaestroTipoIndicador = new ControlMaestroTipoIndicadorService().BuscaTodos();
            controlMaestroTipoIndicadorViewModel.ListControlMaestroTipoIndicadorNivel = new ControlMaestroTipoIndicadorNivelService().BuscaTodos();
            controlMaestroTipoIndicadorViewModel.ListControlMaestroNivel = new ControlMaestroService().BuscaControl("Nivel");
        }
    }
}