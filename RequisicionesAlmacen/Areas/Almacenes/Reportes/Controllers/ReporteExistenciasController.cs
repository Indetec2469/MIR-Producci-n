using FastReport.Web;
using RequisicionesAlmacen.Areas.Almacenes.Reportes.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services.SAACG;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using static RequisicionesAlmacen.Areas.Almacenes.Reportes.Models.ViewModel.ReporteExistenciasViewModel;

namespace RequisicionesAlmacen.Areas.Almacenes.Reportes.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.EXISTENCIAS)]
    public class ReporteExistenciasController : BaseController<ReporteExistenciasViewModel, ReporteExistenciasViewModel>
    {
        //Tipos de Reporte
        public static int TipoRptExistenciasTotales = 0;
        public static int TipoRptExistenciasPresupuestales = 1;
        public static int TipoRptExistenciasPartidas = 2;

        public ActionResult Index()
        {
            // Creamos el objecto nuevo
            ReporteExistenciasViewModel reporteViewModel = new ReporteExistenciasViewModel();

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref reporteViewModel);

            ViewBag.WebReport = null;

            //Retornamos la vista junto con su Objeto Modelo
            return View("ReporteExistencias", reporteViewModel);
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
        public override JsonResult Guardar(ReporteExistenciasViewModel viewModel)
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
        public ActionResult BuscaReporte(ARrptExistencias reporte)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();            

            string path = "Almacen/Existencias/";
            string tituloReporte = "";
            string nombreReporte = "";

            if (reporte.Id == TipoRptExistenciasTotales)
            {
                path = path + "ARrptExistenciasTotales.frx";
                tituloReporte = "Reporte de Existencias Totales";
                nombreReporte = "rptExistenciasTotales";
            }
            else if (reporte.Id == TipoRptExistenciasPresupuestales)
            {
                path = path + "ARrptExistenciasPresupuestales.frx";
                tituloReporte = "Reporte de Existencias por Presupuesto";
                nombreReporte = "rptExistenciasPresupuetales";

                parametros.Add("@pUnidadAdministrativaId", reporte.UnidadAdministrativaId != null ? new SqlParameter("", reporte.UnidadAdministrativaId).Value : DBNull.Value);
                parametros.Add("@pProyectoId", reporte.ProyectoId != null ? new SqlParameter("", reporte.ProyectoId).Value : DBNull.Value);
                parametros.Add("@pFuenteFinanciamientoId", reporte.FuenteFinanciamientoId != null ? new SqlParameter("", reporte.FuenteFinanciamientoId).Value : DBNull.Value);
                parametros.Add("@pTipoGastoId", reporte.TipoGastoId != null ? new SqlParameter("", reporte.TipoGastoId).Value : DBNull.Value);
            }
            else if (reporte.Id == TipoRptExistenciasPartidas)
            {
                path = path + "ARrptExistenciasPartidas.frx";
                tituloReporte = "Reporte de Existencias por Partida";
                nombreReporte = "rptExistenciasPartidas";

                parametros.Add("@pObjetoGastoId", reporte.ObjetoGastoId != null ? new SqlParameter("", reporte.ObjetoGastoId).Value : DBNull.Value);
            }
            
            parametros.Add("@pTituloReporte", tituloReporte);
            parametros.Add("@pNombreReporte", nombreReporte);

            WebReport webReport = new ReportHelper().GetWebReport(path, parametros);
            webReport.ShowRefreshButton = false;
            webReport.ReportDone = true;

            ViewBag.WebReport = webReport;

            return PartialView("ReporteExistenciasPartialView");
        }

        protected override void GetDatosFicha(ref ReporteExistenciasViewModel viewModel)
        {
            //Datos Financiamiento
            viewModel.ListUnidadesAdministrativas = new DependenciaService().BuscaTodos();
            viewModel.ListProyectos = new ProyectoService().BuscaTodos();
            viewModel.ListFuentesFinanciamiento = new RamoService().BuscaTodos();
            viewModel.ListTiposGasto = new TipoGastoService().BuscaTodos();
            viewModel.ListObjetosGasto = new ObjetoGastoService().BuscaPartidasEspecificas();

            //Tipo de Reporte
            List<TipoReporteSelect> tiposResporte = new List<TipoReporteSelect>();
            tiposResporte.Add(new TipoReporteSelect { Id = TipoRptExistenciasTotales, Value = "Existencias Totales" });
            tiposResporte.Add(new TipoReporteSelect { Id = TipoRptExistenciasPresupuestales, Value = "Existencias Presupuestales" });
            tiposResporte.Add(new TipoReporteSelect { Id = TipoRptExistenciasPartidas, Value = "Existencias Partidas" });
            viewModel.ListTiposReporte = tiposResporte;
        }
    }
}