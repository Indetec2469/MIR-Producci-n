using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Table;
using RequisicionesAlmacen.Areas.MIR.Reportes.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.MIR;
using RequisicionesAlmacenBL.Services.SAACG;
using SACG.sysSacg.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using static RequisicionesAlmacen.Areas.MIR.MIR.Controllers.MatrizIndicadorResultadoController;

namespace RequisicionesAlmacen.Areas.MIR.Reportes.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.REPORTE_IDV)]
    public class ReporteVariableIndicadorController : BaseController<ReporteVariableIndicadorViewModel, ReporteVariableIndicadorViewModel>
    {
        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Guardar(ReporteVariableIndicadorViewModel modelView)
        {
            throw new NotImplementedException();
        }

        // GET: MIR/ReporteVariableIndicador
        public ActionResult Index()
        {
            // Creamos el objecto nuevo
            ReporteVariableIndicadorViewModel reporteVariableIndicadorViewModel = new ReporteVariableIndicadorViewModel();
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref reporteVariableIndicadorViewModel);
            // Retornamos la vista junto con su Objeto Modelo
            return View("ReporteVariableIndicador", reporteVariableIndicadorViewModel);
        }

        public override ActionResult Listar()
        {
            throw new NotImplementedException();
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ReporteVariableIndicadorViewModel reporteVariableIndicadorViewModel)
        {
            reporteVariableIndicadorViewModel.ComboListadoMIR = new MatrizIndicadorResultadoService().BuscaComboListadoMIR();
            reporteVariableIndicadorViewModel.ComboListaMes = new List<MesModel>();
            MesMapeo mesMapeo = new MesMapeo();
            for(int x = 1; x <= 12; x++)
            {
                MesModel mesModel = new MesModel();
                mesModel.MesId = x;
                mesModel.NombreMes = mesMapeo.BuscaNombreMes(x);
                reporteVariableIndicadorViewModel.ComboListaMes.Add(mesModel);
            }
        }

        [JsonException]
        public ActionResult BuscarReporte(int mirId, int mesId)
        {

            MItblMatrizIndicadorResultado matrizIndicadorResultado = new MatrizIndicadorResultadoService().BuscaPorId(mirId);
            if (matrizIndicadorResultado != null)
            {
                FileInfo logotipo = new ArchivoService().GetImagenLogotipo("saacg-net.png");
                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    // Obtener la columna fin con el ID de Mes 
                    string columnaFin = obtenerColumnaHaS(mesId);
                    // Reporte Lirbo Consulta Matriz Indicador Resultado 
                    MIspRptLibroConsultaMatrizIndicadorResultado_Result rptLibroConsultaMatrizIndicadorResultado = new MatrizIndicadorResultadoService().BuscaReportePorMIRId(matrizIndicadorResultado.MIRId);
                    // Reporte Lirbo Consulta Plan Desarrollo Estructura
                    List<MIspRptLibroConsultaPlanDesarrolloEstructura_Result> listRptLibroConsultaPlanDesarrolloEstructura = new PlanDesarrolloEstructuraService().BuscaReportePorPlanDesarrolloEstructuraId(matrizIndicadorResultado.PlanDesarrolloEstructuraId).ToList();
                    // Reporte Libro Consulta Lista de MIR
                    List<MIspRptLibroConsultaListaMatrizIndicadorResultado_Result> listRptLibroConsultaListaMatrizIndicadorResultado = new MatrizIndicadorResultadoService().BuscaReporteListaMIRPorMIRId(matrizIndicadorResultado.MIRId).ToList();
                    // Consulta Lista de Variable Indicador
                    List<MIspConsultaListaVariableIndicador_Result> listaConsultaVariableIndicador = new MatrizConfiguracionPresupuestalSeguimientoVariableService().BuscaReportePorMIRId(rptLibroConsultaMatrizIndicadorResultado.MIRId).ToList();

                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(rptLibroConsultaMatrizIndicadorResultado.Codigo);
                    excelWorksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                    excelWorksheet.PrinterSettings.PaperSize = ePaperSize.Legal;
                    // Modificar el tamaño de una celda por una celda (para sirve el papel oficio)
                    excelWorksheet.Column(1).Width = Convert.ToDouble(14.41);
                    excelWorksheet.Column(2).Width = Convert.ToDouble(12.73);
                    excelWorksheet.Column(3).Width = Convert.ToDouble(11.30);
                    excelWorksheet.Column(4).Width = Convert.ToDouble(12.30);
                    excelWorksheet.Column(5).Width = Convert.ToDouble(9.16);
                    excelWorksheet.Column(6).Width = Convert.ToDouble(9.02);
                    excelWorksheet.Column(7).Width = Convert.ToDouble(10.73);
                    // Ciclo para saber cual es el mes de fin
                    double columnaDefault = 80.76;
                    double columnaAncho = Math.Round((columnaDefault / mesId), 2);
                    double columnaAnchoFin = columnaAncho;
                    if(columnaDefault != (columnaAnchoFin * mesId))
                    {
                        if(columnaDefault > (columnaAnchoFin * mesId))
                        {
                            columnaAnchoFin = Math.Round(columnaAnchoFin - (columnaDefault - (columnaAnchoFin * mesId)), 2);
                        }
                        else
                        {
                            columnaAnchoFin = Math.Round(columnaAnchoFin - ((columnaAnchoFin * mesId) - columnaDefault), 2);
                            // El mes Octubre ajustar el tamanio es un bug
                            if(mesId == 10)
                            {
                                columnaAnchoFin = Math.Round((columnaAnchoFin - .30), 2);
                            }
                        }
                    }

                    for(int x = 8; x <= (8 + (mesId - 1)); x++)
                    {
                        if(x == (8 + (mesId - 1)))
                        {
                            excelWorksheet.Column(x).Width = columnaAnchoFin;
                        }
                        else
                        {
                            excelWorksheet.Column(x).Width = columnaAncho;
                        }
                        
                    }

                    ExcelHelper excelHelper = new ExcelHelper();

                    // Grupo 1 //
                    Entidad entidad = new ReportHelper().GetDatosEntidad();

                    // Nombre del Ente Público
                    ExcelRange excelRange = excelWorksheet.Cells["A1:" + columnaFin + "1"];
                    excelRange.Value = entidad.Nombre;
                    excelHelper.ExcelTitulo(ref excelRange);
                    // Estado
                    excelRange = excelWorksheet.Cells["A2:" + columnaFin + "2"];
                    excelRange.Value = entidad.Estado;
                    excelHelper.ExcelTitulo(ref excelRange);
                    // Variables de los Indicadores
                    excelRange = excelWorksheet.Cells["A3:" + columnaFin + "3"];
                    excelRange.Value = "VARIABLES DE LOS INDICADORES";
                    excelHelper.ExcelTitulo(ref excelRange);
                    // Fecha de Corte
                    int ultimoDia = Int32.Parse(new DateTime(Convert.ToInt32(rptLibroConsultaMatrizIndicadorResultado.Ejercicio), mesId, 1).AddMonths(1).AddDays(-1).ToString("dd"));
                    DateTime fechaCorte = new DateTime(Int32.Parse(rptLibroConsultaMatrizIndicadorResultado.Ejercicio), mesId, ultimoDia);
                    excelRange = excelWorksheet.Cells["A4:" + columnaFin + "4"];
                    excelRange.Value = "AL " + fechaCorte.ToString("dd/MMM/yyyy");
                    excelHelper.ExcelTitulo(ref excelRange);
                    // Logotipo
                    //if(logotipo != null)
                    //{
                    //    ExcelPicture excelLogotipo = excelWorksheet.Drawings.AddPicture("SAACG.NET", logotipo);
                    //    excelLogotipo.SetSize(90, 60);
                    //    excelLogotipo.SetPosition(0, 30, 0, 20);
                    //}
                    // Border
                    excelRange = excelWorksheet.Cells["A1:" + columnaFin + "4"];
                    excelRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    /////////////
                    // Grupo 2 //
                    // Identificacion
                    excelRange = excelWorksheet.Cells["A5:" + columnaFin + "5"];
                    excelRange.Value = "IDENTIFICACIÓN DEL PROGRAMA";
                    excelHelper.ExcelTitulo(ref excelRange);
                    // Programa Presupuestario
                    excelRange = excelWorksheet.Cells["A6:" + columnaFin + "6"];
                    excelRange.Value = "Programa: " + rptLibroConsultaMatrizIndicadorResultado.ProgramaPresupuestario;
                    excelHelper.ExcelParrafo(ref excelRange);
                    // Unidad Responsable del Gasto
                    excelRange = excelWorksheet.Cells["A7:" + columnaFin + "7"];
                    excelRange.Value = "Unidad Responsable del Gasto: " + entidad.Nombre;
                    excelHelper.ExcelParrafo(ref excelRange);
                    // Población Objetivo
                    excelRange = excelWorksheet.Cells["A8:" + columnaFin + "8"];
                    excelRange.Value = "Población Objetivo: " + rptLibroConsultaMatrizIndicadorResultado.PoblacionObjetivo;
                    excelHelper.ExcelParrafo(ref excelRange);
                    // Border
                    excelRange = excelWorksheet.Cells["A5:" + columnaFin + "8"];
                    excelRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    /////////////
                    // Creamos el variable de filaSiguiente para empezar la fila en Alineación
                    int filaSiguiente = 10;
                    // Grupo 3 //
                    // Alineación
                    excelRange = excelWorksheet.Cells["A9:" + columnaFin + "9"];
                    excelRange.Value = "ALINEACIÓN GENERAL PLAN DE DESARROLLO";
                    excelHelper.ExcelTitulo(ref excelRange);
                    // Ciclo la fila contador por alineacion
                    foreach(MIspRptLibroConsultaPlanDesarrolloEstructura_Result rptLibroConsultaPlanDesarrolloEstructura in listRptLibroConsultaPlanDesarrolloEstructura)
                    {
                        // Nivel
                        excelRange = excelWorksheet.Cells["A" + filaSiguiente + ":" + columnaFin + "" + filaSiguiente + ""];
                        excelRange.Value = rptLibroConsultaPlanDesarrolloEstructura.NombreNivel;
                        excelHelper.ExcelParrafo(ref excelRange);
                        filaSiguiente++;
                    }
                    // Border
                    excelRange = excelWorksheet.Cells["A9:" + columnaFin + "" + (filaSiguiente - 1) + ""];
                    excelRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    /////////////
                        
                    int filaIndicadorDefault = filaSiguiente;
                    filaSiguiente = filaSiguiente + 2;
                    // Grupo 4 //
                    //excelWorksheet.Row(filaIndicadorDefault + 1).Height = Convert.ToDouble(22.5);
                    // Resumen Narrativo
                    excelRange = excelWorksheet.Cells["A" + filaIndicadorDefault + ":B" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "RESUMEN NARRATIVO";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 12, false, false);
                    // Indicadores
                    excelRange = excelWorksheet.Cells["C" + filaIndicadorDefault + ":F" + filaIndicadorDefault + ""];
                    excelRange.Value = "INDICADORES";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 12, true, true);
                    // Nombre del Indicador
                    excelRange = excelWorksheet.Cells["C" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "NOMBRE DEL INDICADOR";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Fórmula
                    excelRange = excelWorksheet.Cells["D" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "FÓRMULA";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Frecuencia
                    excelRange = excelWorksheet.Cells["E" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "FRECUENCIA";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Unidad de Medida
                    excelRange = excelWorksheet.Cells["F" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "UNIDAD DE MEDIDA";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Variables
                    excelRange = excelWorksheet.Cells["G" + filaIndicadorDefault + ":" + columnaFin + "" + filaIndicadorDefault + ""];
                    excelRange.Value = "VARIABLES";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 12, true, true);
                    // Dscripcion
                    excelRange = excelWorksheet.Cells["G" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Ciclo para saber cual es el mes de fin (Enero a Diciembre)
                    for (int x = 1; x <= mesId; x++)
                    {
                        // Enero a Diciembre Depende con el MES es fin
                        excelRange = excelWorksheet.Cells[obtenerColumnaHaS(x) + (filaIndicadorDefault + 1) + ""];
                        excelRange.Value = new MesMapeo().BuscaNombreMes3Letras(x);
                        excelHelper.ExcelTablaTitulo(ref excelRange, false, 12, false, false);
                    }
                    // Grupo 5 //
                    foreach (MIspRptLibroConsultaListaMatrizIndicadorResultado_Result rptLibroConsultaListaMatrizIndicadorResultado in listRptLibroConsultaListaMatrizIndicadorResultado)
                    {
                        List<MIspConsultaListaVariableIndicador_Result> _listaConsultaVariableIndicador = listaConsultaVariableIndicador.Where(vi => vi.MIRIndicadorId == rptLibroConsultaListaMatrizIndicadorResultado.MIRIndicadorId).ToList();
                        int filaContadorVariables = _listaConsultaVariableIndicador.Count - 1,
                            filaSpanVariables = filaSiguiente + filaContadorVariables,
                            filaSiguienteVariable = 0;
                        // Nombre
                        excelRange = excelWorksheet.Cells["A" + (filaSiguiente) + ":A" + filaSpanVariables + ""];
                        excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.Nombre;
                        // Resumen
                        excelRange = excelWorksheet.Cells["B" + (filaSiguiente) + ":B" + filaSpanVariables + ""];
                        excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.ResumenNarrativo;
                        // Nombre del Indicador
                        excelRange = excelWorksheet.Cells["C" + (filaSiguiente) + ":C" + filaSpanVariables + ""];
                        excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.NombreIndicador;
                        // Formula
                        excelRange = excelWorksheet.Cells["D" + (filaSiguiente) + ":D" + filaSpanVariables + ""];
                        excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.Formula;
                        // Frecuencia
                        excelRange = excelWorksheet.Cells["E" + (filaSiguiente) + ":E" + filaSpanVariables + ""];
                        excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.FrecuenciaMedicion;
                        // Unidad de Medida
                        excelRange = excelWorksheet.Cells["F" + (filaSiguiente) + ":F" + filaSpanVariables + ""];
                        excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.UnidadMedida;

                        foreach (MIspConsultaListaVariableIndicador_Result consultaVariableIndicador in _listaConsultaVariableIndicador)
                        {

                            // Descripcion
                            excelRange = excelWorksheet.Cells["G" + (filaSiguiente + filaSiguienteVariable) + ""];
                            excelRange.Value = consultaVariableIndicador.DescripcionVariable;
                            excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            // Enero a Diciembre Depende con el ID de MES
                            for(int x = 1; x <= mesId; x++)
                            {
                                excelRange = excelWorksheet.Cells[obtenerColumnaHaS(x) + (filaSiguiente + filaSiguienteVariable) + ""];
                                // Enero
                                if(x == 1)
                                {
                                    excelRange.Value = string.Format("{0:n2}", consultaVariableIndicador.Enero);
                                }
                                // Febrero
                                if (x == 2)
                                {
                                    excelRange.Value = string.Format("{0:n2}", consultaVariableIndicador.Febrero);
                                }
                                // Marzo
                                if (x == 3)
                                {
                                    excelRange.Value = string.Format("{0:n2}", consultaVariableIndicador.Marzo);
                                }
                                // Abril
                                if (x == 4)
                                {
                                    excelRange.Value = string.Format("{0:n2}", consultaVariableIndicador.Abril);
                                }
                                // Mayo
                                if (x == 5)
                                {
                                    excelRange.Value = string.Format("{0:n2}", consultaVariableIndicador.Mayo);
                                }
                                // Junio
                                if (x == 6)
                                {
                                    excelRange.Value = string.Format("{0:n2}", consultaVariableIndicador.Junio);
                                }
                                // Julio
                                if (x == 7)
                                {
                                    excelRange.Value = string.Format("{0:n2}", consultaVariableIndicador.Julio);
                                }
                                // Agosto
                                if (x == 8)
                                {
                                    excelRange.Value = string.Format("{0:n2}", consultaVariableIndicador.Agosto);
                                }
                                // Septiembre
                                if (x == 9)
                                {
                                    excelRange.Value = string.Format("{0:n2}", consultaVariableIndicador.Septiembre);
                                }
                                // Octubre
                                if (x == 10)
                                {
                                    excelRange.Value = string.Format("{0:n2}", consultaVariableIndicador.Octubre);
                                }
                                // Noviembre
                                if (x == 11)
                                {
                                    excelRange.Value = string.Format("{0:n2}", consultaVariableIndicador.Noviembre);
                                }
                                // Diciembre
                                if (x == 12)
                                {
                                    excelRange.Value = string.Format("{0:n2}", consultaVariableIndicador.Diciembre);
                                }

                                excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            }

                            filaSiguienteVariable++;
                        }

                        filaSiguiente = (filaSiguiente + 1) + filaContadorVariables;
                    }
                    /////////////

                    Session["DescargarExcel_ReporteVI"] = excelPackage.GetAsByteArray();
                }

                return Json("Ha encontrado el reporte", JsonRequestBehavior.AllowGet);
            }
            else
            {
                throw new Exception("No hay los reportes.");
            }
        }

        public ActionResult DescargarExcel()
        {

            if (Session["DescargarExcel_ReporteVI"] != null)
            {
                byte[] data = Session["DescargarExcel_ReporteVI"] as byte[];
                return File(data, "application/octet-stream", "ReporteVI.xlsx");
            }
            else
            {
                return new EmptyResult();
            }
        }

        private string obtenerColumnaHaS(int mesId)
        {
            switch (mesId)
            {
                case 1:
                    return "H";
                case 2:
                    return "I";
                case 3:
                    return "J";
                case 4:
                    return "K";
                case 5:
                    return "L";
                case 6:
                    return "M";
                case 7:
                    return "N";
                case 8:
                    return "O";
                case 9:
                    return "P";
                case 10:
                    return "Q";
                case 11:
                    return "R";
                case 12:
                    return "S";
                default:
                    return "H";
            }
        }
    }
}