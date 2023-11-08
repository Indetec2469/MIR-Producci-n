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
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using SACG.sysSacg.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Almacenes.Reportes.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.KARDEX)]
    public class ReporteKardexController : BaseController<ReporteKardexViewModel, ReporteKardexViewModel>
    {
        public ActionResult Index()
        {
            // Creamos el objecto nuevo
            ReporteKardexViewModel reporteViewModel = new ReporteKardexViewModel();

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref reporteViewModel);

            ViewBag.WebReport = null;

            //Retornamos la vista junto con su Objeto Modelo
            return View("ReporteKardex", reporteViewModel);
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
        public override JsonResult Guardar(ReporteKardexViewModel viewModel)
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

        protected override void GetDatosFicha(ref ReporteKardexViewModel viewModel)
        {
            //Fechas
            viewModel.FechaInicio = new DateTime(DateTime.Now.Year, 1, 1);
            viewModel.FechaFin = new ConfiguracionEnteService().BuscaFechaOperacion();

            //Datos de Almacén
            viewModel.ListAlmacenes = new AlmacenService().BuscaTodos();
            viewModel.ListProductos = new ProductoService().BuscaActivos();
            viewModel.ListTiposMovimiento = new ControlMaestroService().BuscaControl("TipoInventarioMovimiento");

            //Datos Financiamiento
            viewModel.ListUnidadesAdministrativas = new DependenciaService().BuscaTodos();
            viewModel.ListProyectos = new ProyectoService().BuscaTodos();
            viewModel.ListFuentesFinanciamiento = new RamoService().BuscaTodos();
            viewModel.ListPolizas = new PolizaService().BuscaTodos();
        }

        [JsonException]
        public ActionResult BuscarReporte(ARrptKardex reporte)
        {
            //Obtenemos los datos de la Entidad
            Entidad entidad = new ReportHelper().GetDatosEntidad();

            string nombreEnte = entidad.Nombre;
            string estado = entidad.Estado;
            string tituloReporte = "KÁRDEX";

            List<ARrptKardex> registrosReporte = new ReportesService().GetRptKardex(reporte.FechaInicio,
                                                                                    reporte.FechaFin,
                                                                                    reporte.TipoMvtoId,
                                                                                    reporte.MvtoId,
                                                                                    reporte.PolizaId,
                                                                                    reporte.AlmacenId,
                                                                                    reporte.ProductoId,
                                                                                    reporte.UnidadAdministrativaId,
                                                                                    reporte.ProyectoId,
                                                                                    reporte.FuenteFinanciamientoId,
                                                                                    SessionHelper.GetUsuario().UsuarioId);

            if (registrosReporte.Count > 0)
            {
                List<ColumnaModel> columnas = new List<ColumnaModel>();

                columnas.Add(new ColumnaModel("A", "Almacén ID", "AlmacenId", 7.5));
                columnas.Add(new ColumnaModel("B", "Nombre Almacén", "Almacen", 16.5));
                columnas.Add(new ColumnaModel("C", "Fecha Mov.", "Fecha", 20));
                columnas.Add(new ColumnaModel("D", "Tipo Mov.", "TipoMovimiento", 25));
                columnas.Add(new ColumnaModel("E", "Mov.ID", "ReferenciaMovtoId", 6.5));
                columnas.Add(new ColumnaModel("F", "Descripción Movimiento", "MotivoMovto", 35));
                columnas.Add(new ColumnaModel("G", "Póliza", "Poliza", 10));
                columnas.Add(new ColumnaModel("H", "Producto ID", "ProductoId", 10));
                columnas.Add(new ColumnaModel("I", "Descripción", "Producto", 40));
                columnas.Add(new ColumnaModel("J", "UA", "UA", 7));
                columnas.Add(new ColumnaModel("K", "Proyecto", "Proyecto", 8));
                columnas.Add(new ColumnaModel("L", "FF", "FF", 7));
                columnas.Add(new ColumnaModel("M", "Unidad Medida", "UM", 10));
                columnas.Add(new ColumnaModel("N", "Entrada", "Entrada", 9));
                columnas.Add(new ColumnaModel("O", "Salida", "Salida", 9));
                columnas.Add(new ColumnaModel("P", "Existencia", "Existencia", 9));
                columnas.Add(new ColumnaModel("Q", "Costo Unitario", "CostoUnitario", 10));
                columnas.Add(new ColumnaModel("R", "Total", "Total", 15));
                columnas.Add(new ColumnaModel("S", "Costo Promedio", "CostoPromedio", 14));

                FileInfo logotipo = new ArchivoService().GetImagenLogotipo("saacg-net.png");

                using (var excelPackage = new ExcelPackage(new MemoryStream()))
                {
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Kárdex");
                    ExcelHelper excelHelper = new ExcelHelper();
                    ExcelRange excelRange = null;

                    excelWorksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                    excelWorksheet.PrinterSettings.PaperSize = ePaperSize.Legal;

                    //Nombre del Ente Público
                    excelRange = excelWorksheet.Cells["C1:Q1"];
                    excelRange.Value = nombreEnte;
                    excelHelper.EncabezadoEnteStyle(ref excelRange);

                    //Estado
                    excelRange = excelWorksheet.Cells["C2:Q2"];
                    excelRange.Value = estado;
                    excelHelper.EncabezadoEstadoStyle(ref excelRange);

                    //Título del Reporte
                    excelRange = excelWorksheet.Cells["C3:Q3"];
                    excelRange.Value = tituloReporte;
                    excelHelper.EncabezadoTituloReporteStyle(ref excelRange);

                    //Espacio
                    excelRange = excelWorksheet.Cells["A4:S4"];
                    excelHelper.ExcelTitulo(ref excelRange);

                    //Usuario
                    excelRange = excelWorksheet.Cells["A5"];
                    excelRange.Value = "Usuario:";
                    excelHelper.EncabezadoDatosTituloStyle(ref excelRange);
                    excelRange = excelWorksheet.Cells["B5:Q5"];
                    excelRange.Value = registrosReporte[0].Usuario;
                    excelHelper.EncabezadoDatosStyle(ref excelRange);

                    //Nombre Reporte
                    excelRange = excelWorksheet.Cells["A6"];
                    excelRange.Value = "Reporte:";
                    excelHelper.EncabezadoDatosTituloStyle(ref excelRange);
                    excelRange = excelWorksheet.Cells["B6:Q6"];
                    excelRange.Value = registrosReporte[0].Reporte;
                    excelHelper.EncabezadoDatosStyle(ref excelRange);

                    //Fecha Impresión
                    excelRange = excelWorksheet.Cells["R5"];
                    excelRange.Value = "Fecha y";
                    excelHelper.EncabezadoDatosTituloStyle(ref excelRange);
                    excelRange = excelWorksheet.Cells["S5"];
                    excelRange.Value = registrosReporte[0].FechaImpresion;
                    excelHelper.EncabezadoDatosStyle(ref excelRange);

                    //Hora Impresión
                    excelRange = excelWorksheet.Cells["R6"];
                    excelRange.Value = "hora de Impresión";
                    excelHelper.EncabezadoDatosTituloStyle(ref excelRange);
                    excelRange = excelWorksheet.Cells["S6"];
                    excelRange.Value = registrosReporte[0].HoraImpresion;
                    excelHelper.EncabezadoDatosStyle(ref excelRange);

                    //Logotipo
                    if (logotipo != null)
                    {
                        ExcelPicture excelLogotipo = excelWorksheet.Drawings.AddPicture("SAACG.NET", logotipo);

                        excelLogotipo.SetSize(90, 60);
                        excelLogotipo.SetPosition(0, 10, 0, 30);
                    }

                    //Espacio
                    excelRange = excelWorksheet.Cells["A7:S7"];
                    excelHelper.ExcelTitulo(ref excelRange);

                    //Agregar Titulos de Columnas
                    excelWorksheet.Row(8).Height = 30;

                    for (int i = 0; i < columnas.Count; i++ )
                    {
                        excelRange = excelWorksheet.Cells[columnas[i].Columna + 8];
                        excelRange.Value = columnas[i].Titulo;
                        excelHelper.TablaColumnaStyle(ref excelRange, (i > 12 ? ExcelHorizontalAlignment.Right : ExcelHorizontalAlignment.Left), ExcelVerticalAlignment.Center);
                    }

                    //Border
                    excelRange = excelWorksheet.Cells["A8:S8"];
                    excelRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //Variable contador iniciar rows
                    int iniciarEn = 9;

                    //Agrgamos cada fila del reporte
                    foreach (ARrptKardex registro in registrosReporte)
                    {
                        for (int i = 0; i < columnas.Count; i++)
                        {
                            excelRange = excelWorksheet.Cells[columnas[i].Columna + iniciarEn];
                            excelRange.Value = registro.GetType().GetProperty(columnas[i].Propiedad).GetValue(registro, null);
                            excelHelper.TablaDatoStyle(ref excelRange, (i > 12 ? ExcelHorizontalAlignment.Right : ExcelHorizontalAlignment.Left), ExcelVerticalAlignment.Center);
                        }

                        //Border
                        excelRange = excelWorksheet.Cells["A" + iniciarEn + ":S" + iniciarEn];
                        excelRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                        iniciarEn++;
                    }

                    //Ajustar columnas
                    for (int i = 0; i < columnas.Count; i++)
                    {
                        excelWorksheet.Column(i+1).Width = columnas[i].Ancho;
                    }
                    
                    //Añadimos el color de las celdas
                    excelRange = excelWorksheet.Cells["A1:S" + iniciarEn];
                    excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    excelRange.Style.Fill.BackgroundColor.SetColor(Color.White);

                    //Añadimos el formato moneda
                    excelRange = excelWorksheet.Cells["Q9:S" + iniciarEn];
                    excelRange.Style.Numberformat.Format = "$ #,##0.00##";

                    Session["DescargarExcel_ReporteKardex"] = excelPackage.GetAsByteArray();
                }

                return Json(registrosReporte.Count, JsonRequestBehavior.AllowGet);
            }
            else
            {
                throw new Exception("El reporte no contiene registros que mostrar.");
            }
        }

        [JsonException]
        public ActionResult DescargarExcel()
        {
            Object excel = Session["DescargarExcel_ReporteKardex"];

            if (excel != null)
            {
                return File(excel as byte[], "application/octet-stream", "ReporteKardex.xlsx");
            }
            else
            {
                throw new Exception("No se pudo generar el Reporte.");
            }
        }

        private class ColumnaModel
        {
            public string Columna { get; set; }
            public string Titulo { get; set; }
            public string Propiedad { get; set; }
            public double Ancho { get; set; }

            public ColumnaModel(string Columna, string Titulo, string Propiedad, double Ancho)
            {
                this.Columna = Columna;
                this.Titulo = Titulo;
                this.Propiedad = Propiedad;
                this.Ancho = Ancho;
            }
        }
    }
}