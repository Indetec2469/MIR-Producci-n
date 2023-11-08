using FastReport;
using FastReport.Web;
using RequisicionesAlmacen.Areas.MIR.Reportes.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.SAACG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using static RequisicionesAlmacen.Areas.MIR.MIR.Controllers.MatrizIndicadorResultadoController;

namespace RequisicionesAlmacen.Areas.MIR.Reportes.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.REPORTE_MIR)]
    public class ReporteMatrizIndicadorResultadoController : BaseController<ReporteMatrizIndicadorResultadoViewModel, ReporteMatrizIndicadorResultadoViewModel>
    {
        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Guardar(ReporteMatrizIndicadorResultadoViewModel reporteMatrizIndicadorResultadoViewModel)
        {
            throw new NotImplementedException();
        }

        // GET: MIR/ReporteMatrizIndicadorResultado
        public ActionResult Index()
        {
            // Creamos el objecto nuevo
            ReporteMatrizIndicadorResultadoViewModel reporteMatrizIndicadorResultadoViewModel = new ReporteMatrizIndicadorResultadoViewModel();
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref reporteMatrizIndicadorResultadoViewModel);

            ViewBag.WebReport = null;
            //Retornamos la vista junto con su Objeto Modelo
            return View("ReporteMatrizIndicadorResultado", reporteMatrizIndicadorResultadoViewModel);
        }

        public override ActionResult Listar()
        {
            throw new NotImplementedException();
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ReporteMatrizIndicadorResultadoViewModel reporteMatrizIndicadorResultadoViewModel)
        {
            reporteMatrizIndicadorResultadoViewModel.ComboListadoMIR = new MatrizIndicadorResultadoService().BuscaComboListadoMIR();
        }

        [JsonException]
        public ActionResult BuscarReporte(int mirId)
        {

            MItblMatrizIndicadorResultado matrizIndicadorResultado = new MatrizIndicadorResultadoService().BuscaPorId(mirId);

            if(matrizIndicadorResultado != null)
            {

                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("@pTituloReporte", "MATRIZ INDICADORES PARA RESULTADOS");
                parametros.Add("@pNombreReporte", "rptMatrizIndicadorResultado");
                parametros.Add("@pMIRId", matrizIndicadorResultado.MIRId);
                parametros.Add("@pPlanDesarrolloEstructuraId", matrizIndicadorResultado.PlanDesarrolloEstructuraId);
                // Obtener el reporte con los parametros para agregar
                WebReport webReport = new ReportHelper().GetWebReport("MIR/MatrizIndicadorResultado/MIrptMatrizIndicadorResultado.frx", parametros);
                webReport.ShowRefreshButton = false;
                webReport.ReportDone = true;

                ViewBag.WebReport = webReport;
                // Retornamos la vista junto con su Objeto Modelo
                return PartialView("ReporteMatrizIndicadorResultadoPartialView");
            }
            else
            {
                return Json(new { esNoReporte = true });
            }
        }
    }
}