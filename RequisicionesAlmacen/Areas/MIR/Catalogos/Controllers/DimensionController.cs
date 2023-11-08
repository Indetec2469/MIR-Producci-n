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
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.DIMENSION)]
    public class DimensionController : BaseController<ControlMaestroDimensionViewModel, ControlMaestroDimensionViewModel>
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
        public override JsonResult Guardar(ControlMaestroDimensionViewModel controlMaestroDimensionViewModel)
        {

            throw new NotImplementedException();
            //// Services
            //ControlMaestroDimensionService controlMaestroDimensionService = new ControlMaestroDimensionService();
            //ControlMaestroDimensionNivelService controlMaestroDimensionNivelService = new ControlMaestroDimensionNivelService();

            //if (controlMaestroDimensionViewModel.ListControlMaestroDimension != null)
            //{
            //    foreach (MItblControlMaestroDimension controlMaestroDimension in controlMaestroDimensionViewModel.ListControlMaestroDimension)
            //    {
            //        // Verificar si el usuario ha sido modificado en mismo Descripción y regresa mensaje de error
            //        if (controlMaestroDimensionService.EsDescripcionExiste(controlMaestroDimension))
            //            throw new Exception("El Dimensión con el descripción [" + controlMaestroDimension.Descripcion + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");

            //        // Control Maestro Dimension
            //        MItblControlMaestroDimension _controlMaestroDimension = controlMaestroDimension;
            //        int dimensionIdPadre = controlMaestroDimension.DimensionId;
            //        if (controlMaestroDimension.DimensionId> 0)
            //        {
            //            // Verificar si el usuario ha sido modificado en mismo Dimensión y regresa mensaje de error
            //            MItblControlMaestroDimension __controlMaestroDimension = controlMaestroDimensionService.BuscaPorId(controlMaestroDimension.DimensionId);
            //            if (!StructuralComparisons.StructuralEqualityComparer.Equals(controlMaestroDimension.Timestamp, __controlMaestroDimension.Timestamp))
            //            {
            //                throw new Exception("El Dimensión con el código [" + controlMaestroDimension.DimensionId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
            //            }

            //            controlMaestroDimension.ModificadoPorId = SessionHelper.GetUsuario().UsuarioId;
            //            controlMaestroDimension.FechaUltimaModificacion = DateTime.Now;
            //            controlMaestroDimensionService.Actualiza(controlMaestroDimension);
            //        }
            //        else
            //        {
            //            controlMaestroDimension.CreadoPorId = SessionHelper.GetUsuario().UsuarioId;
            //            _controlMaestroDimension = controlMaestroDimensionService.Inserta(controlMaestroDimension);
            //        }

            //        // Control Maestro Dimension Nivel
            //        if (controlMaestroDimensionViewModel.ListControlMaestroDimensionNivel != null)
            //        {
            //            List<MItblControlMaestroDimensionNivel> listControlMaestroDimensionNivel = controlMaestroDimensionViewModel.ListControlMaestroDimensionNivel.Where(nivel => nivel.DimensionId == dimensionIdPadre).ToList();
            //            if (listControlMaestroDimensionNivel != null)
            //            {
            //                foreach (MItblControlMaestroDimensionNivel controlMaestroDimensionNivel in listControlMaestroDimensionNivel)
            //                {
            //                    if (controlMaestroDimensionNivel.DimensionNivelId > 0)
            //                    {
            //                        // Verificar si el usuario ha sido modificado en mismo Nivel y regresa mensaje de error
            //                        if (controlMaestroDimensionNivelService.EsNivelExiste(controlMaestroDimensionNivel))
            //                            throw new Exception("El Dimensión Nivel ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");

            //                        // Verificar si el usuario ha sido modificado en mismo Dimensión Nivel y regresa mensaje de error
            //                        MItblControlMaestroDimensionNivel _controlMaestroDimensionNivel = controlMaestroDimensionNivelService.BuscaPorId(controlMaestroDimensionNivel.DimensionNivelId);
            //                        if (!StructuralComparisons.StructuralEqualityComparer.Equals(controlMaestroDimensionNivel.Timestamp, _controlMaestroDimensionNivel.Timestamp))
            //                        {
            //                            throw new Exception("El Dimensión Nivel ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
            //                        }

            //                        controlMaestroDimensionNivel.ModificadoPorId = SessionHelper.GetUsuario().UsuarioId;
            //                        controlMaestroDimensionNivel.FechaUltimaModificacion = DateTime.Now;
            //                        controlMaestroDimensionNivelService.Actualiza(controlMaestroDimensionNivel);
            //                    }
            //                    else
            //                    {
            //                        controlMaestroDimensionNivel.DimensionId = _controlMaestroDimension.DimensionId;
            //                        // Verificar si el usuario ha sido modificado en mismo Nivel y regresa mensaje de error
            //                        if (controlMaestroDimensionNivelService.EsNivelExiste(controlMaestroDimensionNivel))
            //                            throw new Exception("El Dimensión Nivel ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");

            //                        controlMaestroDimensionNivel.CreadoPorId = SessionHelper.GetUsuario().UsuarioId;
            //                        controlMaestroDimensionNivelService.Inserta(controlMaestroDimensionNivel);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    // Control Maestro Dimension Nivel
            //    if (controlMaestroDimensionViewModel.ListControlMaestroDimensionNivel != null)
            //    {
            //        foreach (MItblControlMaestroDimensionNivel controlMaestroDimensionNivel in controlMaestroDimensionViewModel.ListControlMaestroDimensionNivel)
            //        {
            //            // Verificar si el usuario ha sido modificado en mismo Nivel y regresa mensaje de error
            //            if (controlMaestroDimensionNivelService.EsNivelExiste(controlMaestroDimensionNivel))
            //                throw new Exception("El Dimensión Nivel ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");

            //            if (controlMaestroDimensionNivel.DimensionNivelId > 0)
            //            {
            //                // Verificar si el usuario ha sido modificado en mismo Dimensión Nivel y regresa mensaje de error
            //                MItblControlMaestroDimensionNivel _controlMaestroDimensionNivel = controlMaestroDimensionNivelService.BuscaPorId(controlMaestroDimensionNivel.DimensionNivelId);
            //                if (!StructuralComparisons.StructuralEqualityComparer.Equals(controlMaestroDimensionNivel.Timestamp, _controlMaestroDimensionNivel.Timestamp))
            //                {
            //                    throw new Exception("El Dimensión Nivel ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
            //                }

            //                controlMaestroDimensionNivel.ModificadoPorId = SessionHelper.GetUsuario().UsuarioId;
            //                controlMaestroDimensionNivel.FechaUltimaModificacion = DateTime.Now;
            //                controlMaestroDimensionNivelService.Actualiza(controlMaestroDimensionNivel);
            //            }
            //            else
            //            {
            //                controlMaestroDimensionNivel.CreadoPorId = SessionHelper.GetUsuario().UsuarioId;
            //                controlMaestroDimensionNivelService.Inserta(controlMaestroDimensionNivel);
            //            }
            //        }
            //    }
            //}
            //return Json("Registros con Exito!");
        }

        // GET: MIR/Dimension/Listar
        public override ActionResult Listar()
        {
            // Crear los objetos
            ControlMaestroDimensionViewModel controlMaestroDimensionViewModel = new ControlMaestroDimensionViewModel();
            GetDatosFicha(ref controlMaestroDimensionViewModel);

            return View("ListadoDimension", controlMaestroDimensionViewModel);
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ControlMaestroDimensionViewModel controlMaestroDimensionViewModel)
        {
            controlMaestroDimensionViewModel.ControlMaestroDimension = new MItblControlMaestroDimension();
            controlMaestroDimensionViewModel.ControlMaestroDimensionNivel = new MItblControlMaestroDimensionNivel();
            controlMaestroDimensionViewModel.ListControlMaestroDimension = new ControlMaestroDimensionService().BuscaTodos();
            controlMaestroDimensionViewModel.ListControlMaestroDimensionNivel = new ControlMaestroDimensionNivelService().BuscaTodos();
            controlMaestroDimensionViewModel.ListControlMaestroNivel = new ControlMaestroService().BuscaControl("Nivel");
        }
    }
}