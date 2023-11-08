using RequisicionesAlmacen.Areas.Compras.Catalogos.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Compras.Catalogos.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.CONFIGURACION_AREAS)]
    public class ConfiguracionAreasController : BaseController<List<ConfiguracionAreaItem>, ConfiguracionAreaViewModel>
    {
        // Variables Estaticas
        int ESTATUS_CARGADO = 0;
        int ESTATUS_NUEVO = 1;        
        int ESTATUS_EDITADO = 2;
        int ESTATUS_ELIMINADO = 3;

        public ActionResult Index()
        {
            ConfiguracionAreaViewModel viewModel = new ConfiguracionAreaViewModel();            

            GetDatosFicha(ref viewModel);

            return View("ConfiguracionArea", viewModel);
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
        public override JsonResult Guardar(List<ConfiguracionAreaItem> configuracionAreas)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;

            ConfiguracionAreaService service = new ConfiguracionAreaService();

            List<ARtblControlMaestroConfiguracionArea> areas = new List<ARtblControlMaestroConfiguracionArea>();

            foreach (ConfiguracionAreaItem configuracionArea in configuracionAreas)
            {
                if (configuracionArea.EstatusId != ESTATUS_CARGADO) // Editados, nuevos, eliminados
                {
                    if (!configuracionAreas.Exists(m => m.ConfiguracionAreaId != configuracionArea.ConfiguracionAreaId
                        && m.AreaId == configuracionArea.AreaId
                        && m.Borrado))
                    {
                        if (configuracionArea.EstatusId != ESTATUS_ELIMINADO && service.ExisteArea(configuracionArea.ConfiguracionAreaId, configuracionArea.AreaId))
                        {
                            throw new Exception("Ya existe una registro con la misma Área: " + configuracionArea.Area + ". Favor de verificar.");
                        }
                    }

                    // Si es un registro nuevo
                    if (configuracionArea.ConfiguracionAreaId <= 0)
                    {
                        configuracionArea.CreadoPorId = usuarioId;
                    }
                    // Si un registro que se editó o eliminó
                    else
                    {
                        ARtblControlMaestroConfiguracionArea temp = service.BuscaPorId(configuracionArea.ConfiguracionAreaId);

                        if (!StructuralComparisons.StructuralEqualityComparer.Equals(configuracionArea.Timestamp, temp.Timestamp))
                        {
                            throw new Exception("La Configuración con el Área  [" + configuracionArea.Area + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                        }

                        configuracionArea.FechaUltimaModificacion = DateTime.Now;
                        configuracionArea.ModificadoPorId = usuarioId;
                    }

                    areas.Add((ARtblControlMaestroConfiguracionArea)configuracionArea);
                }
            }
            
            return Json(service.Guarda(areas));
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Listar()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ConfiguracionAreaViewModel viewModel)
        {
            ConfiguracionAreaService configuracionAreaService = new ConfiguracionAreaService();
            
            viewModel.ListConfiguracionArea = configuracionAreaService.BuscaListado();
            viewModel.ListAreas = new DependenciaService().BuscaTodos();
            viewModel.ListDependenciasProyectos = configuracionAreaService.BuscaDependenciasProyectosPorConfiguracionAreaId(0);
            viewModel.ListAlmacenes = configuracionAreaService.BuscaAlmacenesPorConfiguracionAreaId(0);
        }

        [JsonException]
        public JsonResult GetDatosProyectos(int configuracionAreaId)
        {
            ConfiguracionAreaService configuracionAreaService = new ConfiguracionAreaService();

            ConfiguracionAreaViewModel viewModel = new ConfiguracionAreaViewModel();

            viewModel.ListDependenciasProyectos = configuracionAreaService.BuscaDependenciasProyectosPorConfiguracionAreaId(configuracionAreaId);
            viewModel.ListConfiguracionAreaProyectos = configuracionAreaService.BuscaDetallesPorConfiguracionAreaId(configuracionAreaId);

            return Json(viewModel);
        }

        [JsonException]
        public JsonResult GetDatosAlmacenes(int configuracionAreaId)
        {
            ConfiguracionAreaService configuracionAreaService = new ConfiguracionAreaService();

            ConfiguracionAreaViewModel viewModel = new ConfiguracionAreaViewModel();

            viewModel.ListAlmacenes = configuracionAreaService.BuscaAlmacenesPorConfiguracionAreaId(configuracionAreaId);
            viewModel.ListConfiguracionAreaAlmacenes = configuracionAreaService.BuscaConfiguracionAlmacenesPorConfiguracionAreaId(configuracionAreaId);

            return Json(viewModel);
        }
    }
}