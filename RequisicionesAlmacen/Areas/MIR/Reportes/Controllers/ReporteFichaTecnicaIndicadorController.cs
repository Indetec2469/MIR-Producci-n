using FastReport;
using FastReport.Web;
using RequisicionesAlmacen.Areas.MIR.Reportes.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;

using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.MIR.Reportes.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.REPORTE_TECNICA_INDICADOR)]
    public class ReporteFichaTecnicaIndicadorController : BaseController<ReporteFichaTecnicaIndicadorViewModel, ReporteFichaTecnicaIndicadorViewModel>
    {

        // GET: MIR/ReporteVariableIndicador
        public ActionResult Index()
        {
            // Creamos el objecto nuevo
            ReporteFichaTecnicaIndicadorViewModel reporteFichaTecnicaIndicadorViewModel = new ReporteFichaTecnicaIndicadorViewModel();
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref reporteFichaTecnicaIndicadorViewModel);
            // Retornamos la vista junto con su Objeto Modelo
            return View("ReporteFichaTecnicaIndicador", reporteFichaTecnicaIndicadorViewModel);
        }

        [JsonException]
        public ActionResult BuscarReporte(int MIRId)
        {
              if (MIRId > 0)
            {
                MItblMatrizIndicadorResultado mir = new MatrizIndicadorResultadoService().BuscaPorId(MIRId);
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("@pTituloReporte", "FICHA TÉCNICA DE INDICADORES");
                parametros.Add("@pMIRId", MIRId);
                parametros.Add("@pFechaCorte", mir.Ejercicio);
                parametros.Add("@pNombreReporte", "rptFichaTecnicaIndicador");

                // Obtener el reporte con los parametros para agregar
                //Report report = reportHelper.GetReport("MIR/FichaTecnicaIndicador/FichaTecnicaIndicador.frx", parametros);                
                WebReport webReport = new ReportHelper().GetWebReport("MIR/FichaTecnicaIndicador/FichaTecnicaIndicador.frx", parametros);
                webReport.ShowRefreshButton = false;
                webReport.ReportDone = true;
                ViewBag.WebReport = webReport;
                // Retornamos la vista junto con su Objeto Modelo
                return PartialView("ReporteFichaTecnicaIndicadorPartialView");
            }
            else
            {
                return Json(new { esNoReporte = true });
            }
        }

        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Guardar(ReporteFichaTecnicaIndicadorViewModel modelView)
        {
            throw new NotImplementedException();
        }
               
        public override ActionResult Listar()
        {
            throw new NotImplementedException();
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ReporteFichaTecnicaIndicadorViewModel reporteFichaTecnicaIndicadorViewModel)
        {
            reporteFichaTecnicaIndicadorViewModel.ListMIR = new MatrizIndicadorResultadoService().BuscaComboListadoMIR();
        }
    }
}