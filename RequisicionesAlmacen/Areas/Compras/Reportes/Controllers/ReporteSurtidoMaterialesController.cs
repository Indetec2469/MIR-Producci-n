using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using RequisicionesAlmacen.Areas.Compras.Reportes.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.Almacen;
using RequisicionesAlmacenBL.Services.Compras;
using RequisicionesAlmacenBL.Services.SAACG;
using SACG.sysSacg.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Compras.Reportes.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.SURTIMIENTO_DE_MATERIALES)]
    public class ReporteSurtidoMaterialesController : BaseController<ReporteSurtidoMaterialesViewModel, ReporteSurtidoMaterialesViewModel>
    {
        public ActionResult Index()
        {
            // Creamos el objecto nuevo
            ReporteSurtidoMaterialesViewModel reporteViewModel = new ReporteSurtidoMaterialesViewModel();

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref reporteViewModel);

            ViewBag.WebReport = null;

            //Retornamos la vista junto con su Objeto Modelo
            return View("ReporteSurtidoMateriales", reporteViewModel);
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
        public override JsonResult Guardar(ReporteSurtidoMaterialesViewModel viewModel)
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

        protected override void GetDatosFicha(ref ReporteSurtidoMaterialesViewModel viewModel)
        {
            
        }

        [JsonException]
        public JsonResult BuscarReporte(ARrptSurtidoMateriales reporte)
        {
            List<ARrptSurtidoMateriales> registrosReporte = new RequisicionPorSurtirService().BuscaRptSurtidoMateriales(
                                                                                    reporte.FechaInicio,
                                                                                    reporte.FechaFin,
                                                                                    reporte.Solicitud);
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