using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using RequisicionesAlmacen.Areas.Almacenes.Reportes.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.Almacen;
using RequisicionesAlmacenBL.Services.Compras;
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using SACG.sysSacg.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Almacenes.Reportes.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.ENTRADA_A_ALMACEN)]
    public class ReporteReciboOCController : BaseController<ReporteReciboOCViewModel, ReporteReciboOCViewModel>
    {
        public ActionResult Index()
        {
            // Creamos el objecto nuevo
            ReporteReciboOCViewModel reporteViewModel = new ReporteReciboOCViewModel();

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref reporteViewModel);

            ViewBag.WebReport = null;

            //Retornamos la vista junto con su Objeto Modelo
            return View("ReporteReciboOC", reporteViewModel);
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
        public override JsonResult Guardar(ReporteReciboOCViewModel viewModel)
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

        protected override void GetDatosFicha(ref ReporteReciboOCViewModel viewModel)
        {
            //Fechas
            viewModel.FechaInicio = new DateTime(DateTime.Now.Year, 1, 1);
            viewModel.FechaFin = new ConfiguracionEnteService().BuscaFechaOperacion();
        }

        [JsonException]
        public JsonResult BuscarReporte(ARrptReciboOC reporte)
        {
            List<ARrptReciboOC> registrosReporte = new OrdenCompraReciboService().BuscaRptReciboOC(
                                                                                    reporte.FechaInicio,
                                                                                    reporte.FechaFin,
                                                                                    reporte.OrdenCompraId,
                                                                                    reporte.CompraId);
            if (registrosReporte.Count > 0)
            {
                return Json(registrosReporte);
            }
            else
            {
                throw new Exception("El reporte no contiene registros que mostrar.");
            }
        }
    }
}