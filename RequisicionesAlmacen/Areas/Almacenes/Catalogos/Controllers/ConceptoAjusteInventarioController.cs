using RequisicionesAlmacen.Areas.Almacenes.Catalogos.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Almacenes.Catalogos.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.CONCEPTOS_AJUSTE_INVENTARIO)]
    public class ConceptoAjusteInventarioController : BaseController<List<ARtblControlMaestroConceptoAjusteInventario>, ControlMaestroConceptoAjusteInventarioViewModel>
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
        public override JsonResult Guardar(List<ARtblControlMaestroConceptoAjusteInventario> listControlMaestroConceptoAjusteInventario)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            foreach (ARtblControlMaestroConceptoAjusteInventario concepto in listControlMaestroConceptoAjusteInventario)
            {
                if (concepto.ConceptoAjusteInventarioId > 0)
                {
                    concepto.ModificadoPorId = usuarioId;
                    concepto.FechaUltimaModificacion = fecha;

                    //Verificamos si es posible eliminar
                    if (concepto.EstatusId.Equals(EstatusRegistro.BORRADO) && !new SistemaService().PermiteEliminarRegistro(concepto.ConceptoAjusteInventarioId, Tablas.ARtblControlMaestroConceptoAjusteInventario))
                    {
                        throw new Exception("El Concepto [" + concepto.ConceptoAjuste + "] no puede ser eliminado ya que está siendo utilizado para otros procesos.");
                    }
                }
                else
                {
                    concepto.CreadoPorId = usuarioId;
                }
            }

            new ControlMaestroConceptoAjusteInventarioService().GuardaCambios(listControlMaestroConceptoAjusteInventario);

            return Json("Registros con Exito!");
        }

        [HttpGet]
        public override ActionResult Listar()
        {
            ControlMaestroConceptoAjusteInventarioViewModel conceptoViewModel = new ControlMaestroConceptoAjusteInventarioViewModel();
            
            conceptoViewModel.ControlMaestroConceptoAjusteInventario = new ARtblControlMaestroConceptoAjusteInventario();

            conceptoViewModel.ControlMaestroConceptoAjusteInventario.EstatusId = ControlMaestroMapeo.EstatusRegistro.ACTIVO;

            GetDatosFicha(ref conceptoViewModel);

            return View("ConceptoAjusteInventario", conceptoViewModel);
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ControlMaestroConceptoAjusteInventarioViewModel conceptoViewModel)
        {
            conceptoViewModel.ListControlMaestroConceptoAjusteInventario = new ControlMaestroConceptoAjusteInventarioService().BuscaTodos();

            conceptoViewModel.ListTipoMovimiento = new ControlMaestroService().BuscaControl("TipoMovimiento");
        }
    }
}