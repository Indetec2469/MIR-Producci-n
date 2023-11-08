using FastReport.Web;
using RequisicionesAlmacen.Areas.Almacenes.Reportes.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Almacenes.Reportes.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.LIBRO_ALMACEN)]
    public class ReporteLibroAlmacenController : BaseController<ReporteLibroAlmacenViewModel, ReporteLibroAlmacenViewModel>
    {
        public ActionResult Index()
        {
            // Creamos el objecto nuevo
            ReporteLibroAlmacenViewModel reporteViewModel = new ReporteLibroAlmacenViewModel();

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref reporteViewModel);

            ViewBag.WebReport = null;

            //Retornamos la vista junto con su Objeto Modelo
            return View("ReporteLibroAlmacen", reporteViewModel);
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
        public override JsonResult Guardar(ReporteLibroAlmacenViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public override ActionResult Listar()
        {
            throw new NotImplementedException();
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public ActionResult BuscaReporte(List<string> objetosGastoId, string fecha)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@pTituloReporte", "Libro de Almacén de Materiales y Suministros de Consumo");
            parametros.Add("@pNombreReporte", "rptLibroAlmacen");
            parametros.Add("@pObjetosGastoId", String.Join("|", objetosGastoId.ToArray()));
            parametros.Add("@pFecha", fecha);

            WebReport webReport = new ReportHelper().GetWebReport("Almacen/InventarioFisico/ARrptLibroAlmacen.frx", parametros);
            webReport.ShowRefreshButton = false;
            webReport.ReportDone = true;

            ViewBag.WebReport = webReport;

            return PartialView("ReporteLibroAlmacenPartialView");
        }

        protected override void GetDatosFicha(ref ReporteLibroAlmacenViewModel viewModel)
        {
            //Fecha
            viewModel.FechaOperacion = new ConfiguracionEnteService().BuscaFechaOperacion();

            //Datos Financiamiento
            viewModel.ListClasificadorObjetoGasto = new ObjetoGastoService().BuscaClasificadorObjetoGasto();
        }
    }
}