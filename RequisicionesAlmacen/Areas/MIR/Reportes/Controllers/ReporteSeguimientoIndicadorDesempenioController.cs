using NCalc;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static RequisicionesAlmacen.Areas.MIR.MIR.Controllers.MatrizIndicadorResultadoController;

namespace RequisicionesAlmacen.Areas.MIR.Reportes.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.REPORTE_SID)]
    public class ReporteSeguimientoIndicadorDesempenioController : BaseController<ReporteSeguimientoIndicadorDesempenioViewModel, ReporteSeguimientoIndicadorDesempenioViewModel>
    {
        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Guardar(ReporteSeguimientoIndicadorDesempenioViewModel modelView)
        {
            throw new NotImplementedException();
        }

        // GET: MIR/ReporteSeguimientoIndicadorDesempenio
        public ActionResult Index()
        {
            // Creamos el objecto nuevo
            ReporteSeguimientoIndicadorDesempenioViewModel reporteSeguimientoIndicadorDesempenioViewModel = new ReporteSeguimientoIndicadorDesempenioViewModel();
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref reporteSeguimientoIndicadorDesempenioViewModel);
            // Retornamos la vista junto con su Objeto Modelo
            return View("ReporteSeguimientoIndicadorDesempenio", reporteSeguimientoIndicadorDesempenioViewModel);
        }

        public override ActionResult Listar()
        {
            throw new NotImplementedException();
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ReporteSeguimientoIndicadorDesempenioViewModel reporteSeguimientoIndicadorDesempenioViewModel)
        {
            reporteSeguimientoIndicadorDesempenioViewModel.ComboListadoMIR = new MatrizIndicadorResultadoService().BuscaComboListadoMIR();
            reporteSeguimientoIndicadorDesempenioViewModel.ComboListaPeriodo = new List<PeriodoModel>();
            MesMapeo mesMapeo = new MesMapeo();
            for(int x = 1; x <= 7; x++)
            {
                PeriodoModel periodoModel = new PeriodoModel();
                periodoModel.PeriodoId = x;
                periodoModel.NombrePeriodo = mesMapeo.BuscaNombrePeriodo(x);
                reporteSeguimientoIndicadorDesempenioViewModel.ComboListaPeriodo.Add(periodoModel);
            }
        }

        [JsonException]
        public ActionResult BuscarReporte(int mirId, string fechaReporte, bool semaforoId)
        {
            MesMapeo mesMapeo = new MesMapeo();
            DateTime fecha = DateTime.Parse(fechaReporte);
            int periodoId= ObtenerTrimestre(fecha);

            if (semaforoId == true) { BuscarReporteConSemaforo(mirId, fecha, periodoId); }
            else if (semaforoId == false) { BuscarReporteSinSemaforo(mirId, fecha, periodoId); }
            else 
{
                throw new Exception("No hay los reportes.");
            }
            return Json("Ha encontrado el reporte", JsonRequestBehavior.AllowGet);
        }

        private ActionResult BuscarReporteConSemaforo(int mirId, DateTime fechaReporte, int periodoId) {
           

            MItblMatrizIndicadorResultado matrizIndicadorResultado = new MatrizIndicadorResultadoService().BuscaPorId(mirId);
            if (matrizIndicadorResultado != null)
            {
                FileInfo logotipo = new ArchivoService().GetImagenLogotipo("saacg-net.png");
                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    // Obtener la columna fin con el ID de Periodo 
                    string columnaFin = obtenerColumnaKaW(periodoId);
                    // Reporte Lirbo Consulta Matriz Indicador Resultado 
                    MIspRptLibroConsultaMatrizIndicadorResultado_Result rptLibroConsultaMatrizIndicadorResultado = new MatrizIndicadorResultadoService().BuscaReportePorMIRId(matrizIndicadorResultado.MIRId);
                    // Reporte Lirbo Consulta Plan Desarrollo Estructura
                    List<MIspRptLibroConsultaPlanDesarrolloEstructura_Result> listRptLibroConsultaPlanDesarrolloEstructura = new PlanDesarrolloEstructuraService().BuscaReportePorPlanDesarrolloEstructuraId(matrizIndicadorResultado.PlanDesarrolloEstructuraId).ToList();
                    // Reporte Libro Consulta Lista de MIR
                    List<MIspRptLibroConsultaListaMatrizIndicadorResultado_Result> listRptLibroConsultaListaMatrizIndicadorResultado = new MatrizIndicadorResultadoService().BuscaReporteListaMIRPorMIRId(matrizIndicadorResultado.MIRId).ToList();

                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(matrizIndicadorResultado.Codigo);
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
                    excelWorksheet.Column(8).Width = Convert.ToDouble(11.73);
                    excelWorksheet.Column(9).Width = Convert.ToDouble(7.73);
                    // Ciclo para saber cual es el periodo de fin
                    double columnaDefault = 61.11;
                    double columnaAncho = Math.Round((columnaDefault / periodoId), 2);
                    double columnaAnchoFin = columnaAncho;
                    if (columnaDefault != (columnaAnchoFin * periodoId))
                    {
                        if (columnaDefault > (columnaAnchoFin * periodoId))
                        {
                            columnaAnchoFin = Math.Round(columnaAnchoFin - (columnaDefault - (columnaAnchoFin * periodoId)), 2);
                        }
                        else
                        {
                            columnaAnchoFin = Math.Round(columnaAnchoFin - ((columnaAnchoFin * periodoId) - columnaDefault), 2);
                        }
                    }

                    for (int x = 10; x <= (10 + (periodoId - 1)); x++)
                    {
                        if (x == (10 + (periodoId - 1)))
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
                    // Seguimiento a Indicadores de Desempeño
                    excelRange = excelWorksheet.Cells["A3:" + columnaFin + "3"];
                    excelRange.Value = "SEGUIMIENTO A INDICADORES DE DESEMPEÑO";
                    excelHelper.ExcelTitulo(ref excelRange);
                    // 
                    excelRange = excelWorksheet.Cells["A4:" + columnaFin + "4"];
                    excelRange.Value = "(" + new MesMapeo().BuscaNombrePeriodoTitulo(periodoId) + ")";
                    excelHelper.ExcelTitulo(ref excelRange);
                    // 1 Espacio
                    excelRange = excelWorksheet.Cells["A5:" + columnaFin + "5"];
                    excelHelper.ExcelTitulo(ref excelRange);
                    // Logotipo
                    //if (logotipo != null)
                    //{
                    //    ExcelPicture excelLogotipo = excelWorksheet.Drawings.AddPicture("SAACG.NET", logotipo);
                    //    excelLogotipo.SetSize(90, 60);
                    //    excelLogotipo.SetPosition(0, 30, 0, 20);
                    //}
                    // Border
                    excelRange = excelWorksheet.Cells["A1:" + columnaFin + "5"];
                    excelRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    /////////////
                    // Grupo 2 //
                    // Identificacion
                    excelRange = excelWorksheet.Cells["A6:" + columnaFin + "6"];
                    excelRange.Value = "IDENTIFICACIÓN DEL PROGRAMA";
                    excelHelper.ExcelTitulo(ref excelRange);
                    // Programa Presupuestario
                    excelRange = excelWorksheet.Cells["A7:" + columnaFin + "7"];
                    excelRange.Value = "Programa: " + rptLibroConsultaMatrizIndicadorResultado.ProgramaPresupuestario;
                    excelHelper.ExcelParrafo(ref excelRange);
                    // Unidad Responsable del Gasto
                    excelRange = excelWorksheet.Cells["A8:" + columnaFin + "8"];
                    excelRange.Value = "Unidad Responsable del Gasto: " + entidad.Nombre;
                    excelHelper.ExcelParrafo(ref excelRange);
                    // Población Objetivo
                    excelRange = excelWorksheet.Cells["A9:" + columnaFin + "9"];
                    excelRange.Value = "Población Objetivo: " + rptLibroConsultaMatrizIndicadorResultado.PoblacionObjetivo;
                    excelHelper.ExcelParrafo(ref excelRange);
                    // Border
                    excelRange = excelWorksheet.Cells["A5:" + columnaFin + "9"];
                    excelRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    /////////////
                    // Creamos el variable de filaSiguiente para empezar la fila en Alineación
                    int filaSiguiente = 11;
                    // Grupo 3 //
                    // Alineación
                    excelRange = excelWorksheet.Cells["A10:" + columnaFin + "10"];
                    excelRange.Value = "ALINEACIÓN GENERAL PLAN DE DESARROLLO";
                    excelHelper.ExcelTitulo(ref excelRange);
                    // Ciclo la fila contador por alineacion
                    foreach (MIspRptLibroConsultaPlanDesarrolloEstructura_Result rptLibroConsultaPlanDesarrolloEstructura in listRptLibroConsultaPlanDesarrolloEstructura)
                    {
                        // Nivel
                        excelRange = excelWorksheet.Cells["A" + filaSiguiente + ":" + columnaFin + "" + filaSiguiente + ""];
                        excelRange.Value = rptLibroConsultaPlanDesarrolloEstructura.NombreNivel;
                        excelHelper.ExcelParrafo(ref excelRange);
                        filaSiguiente++;
                    }
                    // Border
                    excelRange = excelWorksheet.Cells["A10:" + columnaFin + "" + (filaSiguiente - 1) + ""];
                    excelRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    /////////////

                    int filaIndicadorDefault = filaSiguiente;
                    filaSiguiente = filaSiguiente + 2;
                    // Grupo 4 //
                    excelWorksheet.Row(filaIndicadorDefault + 1).Height = Convert.ToDouble(22.5);
                    // Resumen Narrativo
                    excelRange = excelWorksheet.Cells["A" + filaIndicadorDefault + ":B" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "RESUMEN NARRATIVO";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 11, false, false);
                    // Indicadores
                    excelRange = excelWorksheet.Cells["C" + filaIndicadorDefault + ":G" + filaIndicadorDefault + ""];
                    excelRange.Value = "INDICADORES";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 11, true, true);
                    // Nombre del Indicador
                    excelRange = excelWorksheet.Cells["C" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "NOMBRE DEL INDICADOR";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Fórmula
                    excelRange = excelWorksheet.Cells["D" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "FÓRMULA";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Linea Base
                    excelRange = excelWorksheet.Cells["E" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "LINEA BASE";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Frecuencia
                    excelRange = excelWorksheet.Cells["F" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "FRECUENCIA";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Unidad de Medida
                    excelRange = excelWorksheet.Cells["G" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "UNIDAD DE MEDIDA";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Medios de Verificación
                    excelRange = excelWorksheet.Cells["H" + filaIndicadorDefault + ":H" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "MEDIOS DE VERIFICACIÓN";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 8, false, false);
                    // Meta
                    excelRange = excelWorksheet.Cells["I" + filaIndicadorDefault + ":I" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "META";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 8, false, false);
                    // Variables
                    excelRange = excelWorksheet.Cells["J" + filaIndicadorDefault + ":" + columnaFin + "" + filaIndicadorDefault + ""];
                    excelRange.Value = "AVANCE";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 11, true, true);
                    // Ciclo para saber cual es el periodo de fin (TRIM SEM ANUAL)
                    for (int x = 1; x <= periodoId; x++)
                    {
                        // TRIM SEM ANUAL Depende con el ID de Periodo es fin
                        excelRange = excelWorksheet.Cells[obtenerColumnaJav(x) + (filaIndicadorDefault + 1) + ""];
                        excelRange.Value = new MesMapeo().BuscaNombrePeriodo3Letras(x);
                        excelHelper.ExcelTablaTitulo(ref excelRange, false, 11, false, false);

                        // Semaforo
                        excelRange = excelWorksheet.Cells[obtenerColumnaKaW(x) + (filaIndicadorDefault + 1) + ""];
                        excelRange.Value = "SEMAFORO";
                        excelHelper.ExcelTablaTitulo(ref excelRange, false, 9, false, false);
                    }
                    /////////////
                    // Grupo 5 //
                    foreach (MIspRptLibroConsultaListaMatrizIndicadorResultado_Result rptLibroConsultaListaMatrizIndicadorResultado in listRptLibroConsultaListaMatrizIndicadorResultado)
                    {
                        List<int> listaUnidadMedidaFormulaVariableId = new List<int>();
                        // Matriz Indicador Resultado Indicador
                        MItblMatrizIndicadorResultadoIndicador matrizIndicadorResultadoIndicador = new MatrizIndicadorResultadoIndicadorService().BuscaPorId(rptLibroConsultaListaMatrizIndicadorResultado.MIRIndicadorId.Value);
                        // Lista Seguimiento Indicador Desempeño
                        IEnumerable<MIfnObtenerSeguimientoIndicadorDesempenio_Result> listaSeguimientoIndicadorDesempenio = new MatrizConfiguracionPresupuestalSeguimientoVariableService().BuscaReporteSIDPorMIRIndicadorId(matrizIndicadorResultadoIndicador.MIRIndicadorId, fechaReporte);
                        // Control Maestro Unidad Medida 
                        MItblControlMaestroUnidadMedida controlMaestroUnidadMedida = new ControlMaestroUnidadMedidaService().BuscaPorId(matrizIndicadorResultadoIndicador.UnidadMedidaId);
                        // Creamos variables
                        int mes = 0;
                        double iTrim = 0, iiTrim = 0, iSem = 0, iiiTrim = 0, ivTrim = 0, iiSem = 0, anual = 0;
                        // I, II, III y IV Trimestre -> Mensual, Trimestral, Semestral y Anual
                        if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                        {
                            //mes = 1;
                            // I Trimestre
                            //do
                            //{
                            Expression ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                            //{
                            listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            {
                                ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0);
                            });
                            try
                            {
                                iTrim += Convert.ToDouble(ncalc.Evaluate());
                            }
                            catch
                            {
                                iTrim += 0;
                            }

                            // Contador
                            //mes++;
                            // Divide 3
                            //if (mes == 4)
                            //{
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                                iTrim = iTrim;
                            else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                iTrim = iTrim;
                            else
                                iTrim = Double.NaN;
                            //}
                            //}
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                            //{
                            //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            //    {
                            //        ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0);
                            //    });
                            //    try
                            //    {
                            //        iTrim += Convert.ToDouble(ncalc.Evaluate());
                            //    }
                            //    catch
                            //    {
                            //        iTrim += 0;
                            //    }
                            //    // Cambiamos el valor
                            //    mes = 4;
                            //}
                            //} while (!(mes == 4));
                            // II Trimestre
                            //do
                            //{
                            ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                            //{
                            listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            {
                                ncalc.Parameters[sid.Variable] = (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0);
                            });
                            try
                            {
                                iiTrim += Convert.ToDouble(ncalc.Evaluate());
                            }
                            catch
                            {
                                iiTrim += 0;
                            }
                            // Contador
                            //mes++;
                            // Divide 3
                            //if (mes == 7)
                            //{
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                                iiTrim = iiTrim;
                            else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                iiTrim = iiTrim;
                            else
                                iiTrim = Double.NaN;
                            //}
                            //}
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                            //{
                            //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            //    {
                            //        ncalc.Parameters[sid.Variable] = (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0);
                            //    });
                            //    try
                            //    {
                            //        iiTrim += Convert.ToDouble(ncalc.Evaluate());
                            //    }
                            //    catch
                            //    {
                            //        iiTrim += 0;
                            //    }
                            //    // Cambiamos el valor
                            //    mes = 7;
                            //}
                            //} while (!(mes == 7));
                            // III Trimestre
                            //do
                            //{
                            ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                            //{
                            listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            {
                                ncalc.Parameters[sid.Variable] = (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0);
                            });
                            try
                            {
                                iiiTrim += Convert.ToDouble(ncalc.Evaluate());
                            }
                            catch
                            {
                                iiiTrim += 0;
                            }
                            // Contador
                            //mes++;
                            // Divide 3
                            //if (mes == 10)
                            //{
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                                iiiTrim = iiiTrim;
                            else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                iiiTrim = iiiTrim;
                            else
                                iiiTrim = Double.NaN;
                            //}
                            //}
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                            //{
                            //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            //    {
                            //        ncalc.Parameters[sid.Variable] = (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0);
                            //    });
                            //    try
                            //    {
                            //        iiiTrim += Convert.ToDouble(ncalc.Evaluate());
                            //    }
                            //    catch
                            //    {
                            //        iiiTrim += 0;
                            //    }
                            //    // Cambiamos el valor
                            //    mes = 10;
                            //}
                            //} while (!(mes == 10));
                            // IV Trimestre
                            //do
                            //{
                            ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                            //{
                            listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            {
                                ncalc.Parameters[sid.Variable] = (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                            });
                            try
                            {
                                ivTrim += Convert.ToDouble(ncalc.Evaluate());
                            }
                            catch
                            {
                                ivTrim += 0;
                            }
                            // Contador
                            //mes++;
                            // Divide 3
                            //if (mes == 13)
                            //{
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                                ivTrim = ivTrim;
                            else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                ivTrim = ivTrim;
                            else
                                ivTrim = Double.NaN;
                            //}
                            //}
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                            //{
                            //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            //    {
                            //        ncalc.Parameters[sid.Variable] = (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                            //    });
                            //    try
                            //    {
                            //        ivTrim += Convert.ToDouble(ncalc.Evaluate());
                            //    }
                            //    catch
                            //    {
                            //        ivTrim += 0;
                            //    }
                            //    // Cambiamos el valor
                            //    mes = 13;
                            //}
                            //} while (!(mes == 13));
                        }
                        // I y II Semestre -> Mensual, Trimestral, Semestral y Anual
                        if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                        {
                            //mes = 1;
                            // I Semestre
                            //do
                            //{
                            Expression ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                            // Mensual
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                            //{
                            listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            {
                                ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0) + (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0);
                            });
                            try
                            {
                                iSem += Convert.ToDouble(ncalc.Evaluate());
                            }
                            catch
                            {
                                iSem += 0;
                            }
                            // Contador
                            //mes++;
                            // Divide 6
                            //if (mes == 7)
                            //{
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                iSem = (iTrim + iiTrim) / 2;
                            else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                                iSem = iSem;
                            else
                                iSem = double.NaN;

                            //}
                            //}
                            //// Trimestral
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                            //{
                            //    iSem = ((iTrim + iiTrim) / 2);
                            //    // Cambiamos el valor
                            //    mes = 7;
                            //}
                            // Semestral
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                            //{
                            //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            //    {
                            //        ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0) + (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0);
                            //    });
                            //    try
                            //    {
                            //        iSem += Convert.ToDouble(ncalc.Evaluate());
                            //    }
                            //    catch
                            //    {
                            //        iSem += 0;
                            //    }
                            //    // Cambiamos el valor
                            //    mes = 7;
                            //}
                            //} while (!(mes == 7));
                            // II Semestre
                            //do
                            //{
                            ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                            // Mensual
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                            //{
                            listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            {
                                ncalc.Parameters[sid.Variable] = (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0) + (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                            });
                            try
                            {
                                iiSem += Convert.ToDouble(ncalc.Evaluate());
                            }
                            catch
                            {
                                iiSem += 0;
                            }
                            // Contador
                            //mes++;
                            // Divide 6
                            //if (mes == 13)
                            //{
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                iiSem = (iiiTrim + ivTrim) / 2;
                            else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                                iiSem = iiSem;
                            else
                                iiSem = double.NaN;
                            //}
                            //}
                            // Trimestral
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                            //{
                            //    iiSem = ((iiiTrim + ivTrim) / 2);
                            //    // Cambiamos el valor
                            //    mes = 13;
                            //}
                            // Semestral
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                            //{
                            //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            //    {
                            //        ncalc.Parameters[sid.Variable] = (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0) + (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                            //    });
                            //    try
                            //    {
                            //        iiSem += Convert.ToDouble(ncalc.Evaluate());
                            //    }
                            //    catch
                            //    {
                            //        iiSem += 0;
                            //    }
                            //    // Cambiamos el valor
                            //    mes = 13;
                            //}
                            //} while (!(mes == 13));
                        }
                        // Anual -> Mensual, Trimestral, Semestral y Anual
                        if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                        {
                            //mes = 1;
                            // Anual
                            //do
                            //{
                            Expression ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                            //// Mensual
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                            //{
                            listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            {
                                ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0) + (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0) + (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0) + (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                            });
                            try
                            {
                                anual += Convert.ToDouble(ncalc.Evaluate());
                            }
                            catch
                            {
                                anual += 0;
                            }
                            // Contador
                            //mes++;
                            // Divide 12
                            //if (mes == 13)
                            //{
                            // Esperado
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                                anual = (iSem + iiSem) / 2;
                            else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                                anual = anual;
                            else
                                anual = double.NaN;
                            //}
                            //}
                            // Trimestral
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                            //{
                            //    anual = iTrim + iiTrim + iiiTrim + ivTrim;
                            //    // Cambiamos el valor
                            //    mes = 13;
                            //}
                            // Semestral
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                            //{
                            //    anual = iSem + iiSem;
                            //    // Cambiamos el valor
                            //    mes = 13;
                            //}
                            // Anual
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                            //{
                            //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            //    {
                            //        ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0) + (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0) + (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0) + (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                            //    });
                            //    try
                            //    {
                            //        anual += Convert.ToDouble(ncalc.Evaluate());
                            //    }
                            //    catch
                            //    {
                            //        anual += 0;
                            //    }
                            //    // Cambiamos el valor
                            //    mes = 13;
                            //}
                            //} while (!(mes == 13));
                        }
                        // Nombre
                        excelRange = excelWorksheet.Cells["A" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.Nombre;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Resumen
                        excelRange = excelWorksheet.Cells["B" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.ResumenNarrativo;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Nombre del Indicador
                        excelRange = excelWorksheet.Cells["C" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.NombreIndicador;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Formula
                        excelRange = excelWorksheet.Cells["D" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.Formula;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Linea Base
                        excelRange = excelWorksheet.Cells["E" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.LineaBase;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Frecuencia
                        excelRange = excelWorksheet.Cells["F" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.FrecuenciaMedicion;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Unidad de Medida
                        excelRange = excelWorksheet.Cells["G" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.UnidadMedida;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Medio Verificacion
                        excelRange = excelWorksheet.Cells["H" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.MedioVerificacion;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Meta
                        excelRange = excelWorksheet.Cells["I" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.Meta;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Enero a Diciembre Depende con el ID de MES
                        for (int x = 1; x <= periodoId; x++)
                        {
                            excelRange = excelWorksheet.Cells[obtenerColumnaJav(x) + (filaSiguiente) + ""];
                            // I TRIM
                            if (x == 1)
                            {
                                excelRange.Value = Double.IsNaN(iTrim) ? null : string.Format("{0:n2}", iTrim);
                            }
                            // II TRIM
                            if (x == 2)
                            {
                                excelRange.Value = Double.IsNaN(iiTrim) ? null : string.Format("{0:n2}", iiTrim);
                            }
                            // I SEM
                            if (x == 3)
                            {
                                excelRange.Value = Double.IsNaN(iSem) ? null : string.Format("{0:n2}", iSem);
                            }
                            // III TRIM
                            if (x == 4)
                            {
                                excelRange.Value = Double.IsNaN(iiiTrim) ? null : string.Format("{0:n2}", iiiTrim);
                            }
                            // IV TRIM
                            if (x == 5)
                            {
                                excelRange.Value = Double.IsNaN(ivTrim) ? null : string.Format("{0:n2}", ivTrim);
                            }
                            // II SEM
                            if (x == 6)
                            {
                                excelRange.Value = Double.IsNaN(iiSem) ? null : string.Format("{0:n2}", iiSem);
                            }
                            // ANUAL
                            if (x == 7)
                            {
                                excelRange.Value = Double.IsNaN(anual) ? null : string.Format("{0:n2}", anual);
                            }

                            excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            excelRange = excelWorksheet.Cells[obtenerColumnaKaW(x) + (filaSiguiente) + ""];
                            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            if (x == 1)
                            {
                                
                                excelRange.Style.Fill.BackgroundColor.SetColor(GetCellBackgroundColor(Double.IsNaN(iTrim)?0: iTrim, matrizIndicadorResultadoIndicador.AceptableDesde, matrizIndicadorResultadoIndicador.AceptableHasta, matrizIndicadorResultadoIndicador.ConRiesgoDesde, matrizIndicadorResultadoIndicador.ConRiesgoHasta, matrizIndicadorResultadoIndicador.CriticoPorDebajo, matrizIndicadorResultadoIndicador.CriticoPorEncima));
                            }
                            if (x == 2)
                            {
                                excelRange.Style.Fill.BackgroundColor.SetColor(GetCellBackgroundColor(Double.IsNaN(iiTrim) ? 0 : iiTrim, matrizIndicadorResultadoIndicador.AceptableDesde, matrizIndicadorResultadoIndicador.AceptableHasta, matrizIndicadorResultadoIndicador.ConRiesgoDesde, matrizIndicadorResultadoIndicador.ConRiesgoHasta, matrizIndicadorResultadoIndicador.CriticoPorDebajo, matrizIndicadorResultadoIndicador.CriticoPorEncima));
                            }
                            if (x == 3)
                            {
                                excelRange.Style.Fill.BackgroundColor.SetColor(GetCellBackgroundColor(Double.IsNaN(iSem) ? 0 : iSem, matrizIndicadorResultadoIndicador.AceptableDesde, matrizIndicadorResultadoIndicador.AceptableHasta, matrizIndicadorResultadoIndicador.ConRiesgoDesde, matrizIndicadorResultadoIndicador.ConRiesgoHasta, matrizIndicadorResultadoIndicador.CriticoPorDebajo, matrizIndicadorResultadoIndicador.CriticoPorEncima));
                            }
                            if (x == 4)
                            {
                                excelRange.Style.Fill.BackgroundColor.SetColor(GetCellBackgroundColor(Double.IsNaN(iiiTrim) ? 0 : iiiTrim, matrizIndicadorResultadoIndicador.AceptableDesde, matrizIndicadorResultadoIndicador.AceptableHasta, matrizIndicadorResultadoIndicador.ConRiesgoDesde, matrizIndicadorResultadoIndicador.ConRiesgoHasta, matrizIndicadorResultadoIndicador.CriticoPorDebajo, matrizIndicadorResultadoIndicador.CriticoPorEncima));
                            }
                            if (x == 5)
                            {
                                excelRange.Style.Fill.BackgroundColor.SetColor(GetCellBackgroundColor(Double.IsNaN(ivTrim) ? 0 : ivTrim, matrizIndicadorResultadoIndicador.AceptableDesde, matrizIndicadorResultadoIndicador.AceptableHasta, matrizIndicadorResultadoIndicador.ConRiesgoDesde, matrizIndicadorResultadoIndicador.ConRiesgoHasta, matrizIndicadorResultadoIndicador.CriticoPorDebajo, matrizIndicadorResultadoIndicador.CriticoPorEncima));
                            }
                            if (x == 6)
                            {
                                excelRange.Style.Fill.BackgroundColor.SetColor(GetCellBackgroundColor(Double.IsNaN(iiSem) ? 0 : iiSem, matrizIndicadorResultadoIndicador.AceptableDesde, matrizIndicadorResultadoIndicador.AceptableHasta, matrizIndicadorResultadoIndicador.ConRiesgoDesde, matrizIndicadorResultadoIndicador.ConRiesgoHasta, matrizIndicadorResultadoIndicador.CriticoPorDebajo, matrizIndicadorResultadoIndicador.CriticoPorEncima));
                            }
                            if (x == 7)
                            {
                                excelRange.Style.Fill.BackgroundColor.SetColor(GetCellBackgroundColor(Double.IsNaN(anual) ? 0 : anual, matrizIndicadorResultadoIndicador.AceptableDesde, matrizIndicadorResultadoIndicador.AceptableHasta, matrizIndicadorResultadoIndicador.ConRiesgoDesde, matrizIndicadorResultadoIndicador.ConRiesgoHasta, matrizIndicadorResultadoIndicador.CriticoPorDebajo, matrizIndicadorResultadoIndicador.CriticoPorEncima));
                            }


                            excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);

                        }
                        // Contador
                        filaSiguiente++;
                    }



                    Session["DescargarExcel_ReporteSID"] = excelPackage.GetAsByteArray();
                }

                return Json("Ha encontrado el reporte", JsonRequestBehavior.AllowGet);
            }
            else
            {
                throw new Exception("No hay los reportes.");
            }
        }


        [JsonException]
        public ActionResult BuscarReporteSinSemaforo(int mirId, DateTime fechaReporte, int periodoId)
        {
            MItblMatrizIndicadorResultado matrizIndicadorResultado = new MatrizIndicadorResultadoService().BuscaPorId(mirId);
            if (matrizIndicadorResultado != null)
            {
                FileInfo logotipo = new ArchivoService().GetImagenLogotipo("saacg-net.png");
                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    // Obtener la columna fin con el ID de Periodo 
                    string columnaFin = obtenerColumnaJaP(periodoId);
                    // Reporte Lirbo Consulta Matriz Indicador Resultado 
                    MIspRptLibroConsultaMatrizIndicadorResultado_Result rptLibroConsultaMatrizIndicadorResultado = new MatrizIndicadorResultadoService().BuscaReportePorMIRId(matrizIndicadorResultado.MIRId);
                    // Reporte Lirbo Consulta Plan Desarrollo Estructura
                    List<MIspRptLibroConsultaPlanDesarrolloEstructura_Result> listRptLibroConsultaPlanDesarrolloEstructura = new PlanDesarrolloEstructuraService().BuscaReportePorPlanDesarrolloEstructuraId(matrizIndicadorResultado.PlanDesarrolloEstructuraId).ToList();
                    // Reporte Libro Consulta Lista de MIR
                    List<MIspRptLibroConsultaListaMatrizIndicadorResultado_Result> listRptLibroConsultaListaMatrizIndicadorResultado = new MatrizIndicadorResultadoService().BuscaReporteListaMIRPorMIRId(matrizIndicadorResultado.MIRId).ToList();

                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(matrizIndicadorResultado.Codigo);
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
                    excelWorksheet.Column(8).Width = Convert.ToDouble(11.73);
                    excelWorksheet.Column(9).Width = Convert.ToDouble(7.73);
                    // Ciclo para saber cual es el periodo de fin
                    double columnaDefault = 61.11;
                    double columnaAncho = Math.Round((columnaDefault / periodoId), 2);
                    double columnaAnchoFin = columnaAncho;
                    if (columnaDefault != (columnaAnchoFin * periodoId))
                    {
                        if (columnaDefault > (columnaAnchoFin * periodoId))
                        {
                            columnaAnchoFin = Math.Round(columnaAnchoFin - (columnaDefault - (columnaAnchoFin * periodoId)), 2);
                        }
                        else
                        {
                            columnaAnchoFin = Math.Round(columnaAnchoFin - ((columnaAnchoFin * periodoId) - columnaDefault), 2);
                        }
                    }

                    for (int x = 10; x <= (10 + (periodoId - 1)); x++)
                    {
                        if (x == (10 + (periodoId - 1)))
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
                    // Seguimiento a Indicadores de Desempeño
                    excelRange = excelWorksheet.Cells["A3:" + columnaFin + "3"];
                    excelRange.Value = "SEGUIMIENTO A INDICADORES DE DESEMPEÑO";
                    excelHelper.ExcelTitulo(ref excelRange);
                    // 
                    excelRange = excelWorksheet.Cells["A4:" + columnaFin + "4"];
                    excelRange.Value = "(" + new MesMapeo().BuscaNombrePeriodoTitulo(periodoId) + ")";
                    excelHelper.ExcelTitulo(ref excelRange);
                    // 1 Espacio
                    excelRange = excelWorksheet.Cells["A5:" + columnaFin + "5"];
                    excelHelper.ExcelTitulo(ref excelRange);
                    // Logotipo
                    //if (logotipo != null)
                    //{
                    //    ExcelPicture excelLogotipo = excelWorksheet.Drawings.AddPicture("SAACG.NET", logotipo);
                    //    excelLogotipo.SetSize(90, 60);
                    //    excelLogotipo.SetPosition(0, 30, 0, 20);
                    //}
                    // Border
                    excelRange = excelWorksheet.Cells["A1:" + columnaFin + "5"];
                    excelRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    /////////////
                    // Grupo 2 //
                    // Identificacion
                    excelRange = excelWorksheet.Cells["A6:" + columnaFin + "6"];
                    excelRange.Value = "IDENTIFICACIÓN DEL PROGRAMA";
                    excelHelper.ExcelTitulo(ref excelRange);
                    // Programa Presupuestario
                    excelRange = excelWorksheet.Cells["A7:" + columnaFin + "7"];
                    excelRange.Value = "Programa: " + rptLibroConsultaMatrizIndicadorResultado.ProgramaPresupuestario;
                    excelHelper.ExcelParrafo(ref excelRange);
                    // Unidad Responsable del Gasto
                    excelRange = excelWorksheet.Cells["A8:" + columnaFin + "8"];
                    excelRange.Value = "Unidad Responsable del Gasto: " + entidad.Nombre;
                    excelHelper.ExcelParrafo(ref excelRange);
                    // Población Objetivo
                    excelRange = excelWorksheet.Cells["A9:" + columnaFin + "9"];
                    excelRange.Value = "Población Objetivo: " + rptLibroConsultaMatrizIndicadorResultado.PoblacionObjetivo;
                    excelHelper.ExcelParrafo(ref excelRange);
                    // Border
                    excelRange = excelWorksheet.Cells["A5:" + columnaFin + "9"];
                    excelRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    /////////////
                    // Creamos el variable de filaSiguiente para empezar la fila en Alineación
                    int filaSiguiente = 11;
                    // Grupo 3 //
                    // Alineación
                    excelRange = excelWorksheet.Cells["A10:" + columnaFin + "10"];
                    excelRange.Value = "ALINEACIÓN GENERAL PLAN DE DESARROLLO";
                    excelHelper.ExcelTitulo(ref excelRange);
                    // Ciclo la fila contador por alineacion
                    foreach (MIspRptLibroConsultaPlanDesarrolloEstructura_Result rptLibroConsultaPlanDesarrolloEstructura in listRptLibroConsultaPlanDesarrolloEstructura)
                    {
                        // Nivel
                        excelRange = excelWorksheet.Cells["A" + filaSiguiente + ":" + columnaFin + "" + filaSiguiente + ""];
                        excelRange.Value = rptLibroConsultaPlanDesarrolloEstructura.NombreNivel;
                        excelHelper.ExcelParrafo(ref excelRange);
                        filaSiguiente++;
                    }
                    // Border
                    excelRange = excelWorksheet.Cells["A10:" + columnaFin + "" + (filaSiguiente - 1) + ""];
                    excelRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    /////////////

                    int filaIndicadorDefault = filaSiguiente;
                    filaSiguiente = filaSiguiente + 2;
                    // Grupo 4 //
                    excelWorksheet.Row(filaIndicadorDefault + 1).Height = Convert.ToDouble(22.5);
                    // Resumen Narrativo
                    excelRange = excelWorksheet.Cells["A" + filaIndicadorDefault + ":B" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "RESUMEN NARRATIVO";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 11, false, false);
                    // Indicadores
                    excelRange = excelWorksheet.Cells["C" + filaIndicadorDefault + ":G" + filaIndicadorDefault + ""];
                    excelRange.Value = "INDICADORES";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 11, true, true);
                    // Nombre del Indicador
                    excelRange = excelWorksheet.Cells["C" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "NOMBRE DEL INDICADOR";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Fórmula
                    excelRange = excelWorksheet.Cells["D" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "FÓRMULA";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Linea Base
                    excelRange = excelWorksheet.Cells["E" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "LINEA BASE";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Frecuencia
                    excelRange = excelWorksheet.Cells["F" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "FRECUENCIA";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Unidad de Medida
                    excelRange = excelWorksheet.Cells["G" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "UNIDAD DE MEDIDA";
                    excelHelper.ExcelTablaTitulo(ref excelRange, false, 8, false, false);
                    // Medios de Verificación
                    excelRange = excelWorksheet.Cells["H" + filaIndicadorDefault + ":H" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "MEDIOS DE VERIFICACIÓN";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 8, false, false);
                    // Meta
                    excelRange = excelWorksheet.Cells["I" + filaIndicadorDefault + ":I" + (filaIndicadorDefault + 1) + ""];
                    excelRange.Value = "META";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 8, false, false);
                    // Variables
                    excelRange = excelWorksheet.Cells["J" + filaIndicadorDefault + ":" + columnaFin + "" + filaIndicadorDefault + ""];
                    excelRange.Value = "AVANCE";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 11, true, true);
                    // Ciclo para saber cual es el periodo de fin (TRIM SEM ANUAL)
                    for (int x = 1; x <= periodoId; x++)
                    {
                        // TRIM SEM ANUAL Depende con el ID de Periodo es fin
                        excelRange = excelWorksheet.Cells[obtenerColumnaJaP(x) + (filaIndicadorDefault + 1) + ""];
                        excelRange.Value = new MesMapeo().BuscaNombrePeriodo3Letras(x);
                        excelHelper.ExcelTablaTitulo(ref excelRange, false, 11, false, false);
                    }
                    /////////////
                    // Grupo 5 //
                    foreach (MIspRptLibroConsultaListaMatrizIndicadorResultado_Result rptLibroConsultaListaMatrizIndicadorResultado in listRptLibroConsultaListaMatrizIndicadorResultado)
                    {
                        List<int> listaUnidadMedidaFormulaVariableId = new List<int>();
                        // Matriz Indicador Resultado Indicador
                        MItblMatrizIndicadorResultadoIndicador matrizIndicadorResultadoIndicador = new MatrizIndicadorResultadoIndicadorService().BuscaPorId(rptLibroConsultaListaMatrizIndicadorResultado.MIRIndicadorId.Value);
                        // Lista Seguimiento Indicador Desempeño
                        IEnumerable<MIfnObtenerSeguimientoIndicadorDesempenio_Result> listaSeguimientoIndicadorDesempenio = new MatrizConfiguracionPresupuestalSeguimientoVariableService().BuscaReporteSIDPorMIRIndicadorId(matrizIndicadorResultadoIndicador.MIRIndicadorId,fechaReporte);
                        // Control Maestro Unidad Medida 
                        MItblControlMaestroUnidadMedida controlMaestroUnidadMedida = new ControlMaestroUnidadMedidaService().BuscaPorId(matrizIndicadorResultadoIndicador.UnidadMedidaId);
                        // Creamos variables
                        int mes = 0;
                        double iTrim = 0, iiTrim = 0, iSem = 0, iiiTrim = 0, ivTrim = 0, iiSem = 0, anual = 0;
                        // I, II, III y IV Trimestre -> Mensual, Trimestral, Semestral y Anual
                        if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                        {
                            //mes = 1;
                            // I Trimestre
                            //do
                            //{
                            Expression ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                            //{
                            listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            {
                                ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0);
                            });
                            try
                            {
                                iTrim += Convert.ToDouble(ncalc.Evaluate());
                            }
                            catch
                            {
                                iTrim += 0;
                            }

                            // Contador
                            //mes++;
                            // Divide 3
                            //if (mes == 4)
                            //{
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                                iTrim = iTrim;
                            else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                iTrim = iTrim;
                            else
                                iTrim = Double.NaN;
                            //}
                            //}
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                            //{
                            //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            //    {
                            //        ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0);
                            //    });
                            //    try
                            //    {
                            //        iTrim += Convert.ToDouble(ncalc.Evaluate());
                            //    }
                            //    catch
                            //    {
                            //        iTrim += 0;
                            //    }
                            //    // Cambiamos el valor
                            //    mes = 4;
                            //}
                            //} while (!(mes == 4));
                            // II Trimestre
                            //do
                            //{
                            ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                            //{
                            listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            {
                                ncalc.Parameters[sid.Variable] = (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0);
                            });
                            try
                            {
                                iiTrim += Convert.ToDouble(ncalc.Evaluate());
                            }
                            catch
                            {
                                iiTrim += 0;
                            }
                            // Contador
                            //mes++;
                            // Divide 3
                            //if (mes == 7)
                            //{
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                                iiTrim = iiTrim;
                            else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                iiTrim = iiTrim;
                            else
                                iiTrim = Double.NaN;
                            //}
                            //}
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                            //{
                            //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            //    {
                            //        ncalc.Parameters[sid.Variable] = (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0);
                            //    });
                            //    try
                            //    {
                            //        iiTrim += Convert.ToDouble(ncalc.Evaluate());
                            //    }
                            //    catch
                            //    {
                            //        iiTrim += 0;
                            //    }
                            //    // Cambiamos el valor
                            //    mes = 7;
                            //}
                            //} while (!(mes == 7));
                            // III Trimestre
                            //do
                            //{
                            ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                            //{
                            listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            {
                                ncalc.Parameters[sid.Variable] = (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0);
                            });
                            try
                            {
                                iiiTrim += Convert.ToDouble(ncalc.Evaluate());
                            }
                            catch
                            {
                                iiiTrim += 0;
                            }
                            // Contador
                            //mes++;
                            // Divide 3
                            //if (mes == 10)
                            //{
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                                iiiTrim = iiiTrim;
                            else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                iiiTrim = iiiTrim;
                            else
                                iiiTrim = Double.NaN;
                            //}
                            //}
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                            //{
                            //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            //    {
                            //        ncalc.Parameters[sid.Variable] = (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0);
                            //    });
                            //    try
                            //    {
                            //        iiiTrim += Convert.ToDouble(ncalc.Evaluate());
                            //    }
                            //    catch
                            //    {
                            //        iiiTrim += 0;
                            //    }
                            //    // Cambiamos el valor
                            //    mes = 10;
                            //}
                            //} while (!(mes == 10));
                            // IV Trimestre
                            //do
                            //{
                            ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                            //{
                            listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            {
                                ncalc.Parameters[sid.Variable] = (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                            });
                            try
                            {
                                ivTrim += Convert.ToDouble(ncalc.Evaluate());
                            }
                            catch
                            {
                                ivTrim += 0;
                            }
                            // Contador
                            //mes++;
                            // Divide 3
                            //if (mes == 13)
                            //{
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                                ivTrim = ivTrim;
                            else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                ivTrim = ivTrim;
                            else
                                ivTrim = Double.NaN;
                            //}
                            //}
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                            //{
                            //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            //    {
                            //        ncalc.Parameters[sid.Variable] = (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                            //    });
                            //    try
                            //    {
                            //        ivTrim += Convert.ToDouble(ncalc.Evaluate());
                            //    }
                            //    catch
                            //    {
                            //        ivTrim += 0;
                            //    }
                            //    // Cambiamos el valor
                            //    mes = 13;
                            //}
                            //} while (!(mes == 13));
                        }
                        // I y II Semestre -> Mensual, Trimestral, Semestral y Anual
                        if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                        {
                            //mes = 1;
                            // I Semestre
                            //do
                            //{
                            Expression ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                            // Mensual
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                            //{
                            listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            {
                                ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0) + (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0);
                            });
                            try
                            {
                                iSem += Convert.ToDouble(ncalc.Evaluate());
                            }
                            catch
                            {
                                iSem += 0;
                            }
                            // Contador
                            //mes++;
                            // Divide 6
                            //if (mes == 7)
                            //{
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                iSem = (iTrim + iiTrim) / 2;
                            else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                                iSem = iSem;
                            else
                                iSem = double.NaN;

                            //}
                            //}
                            //// Trimestral
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                            //{
                            //    iSem = ((iTrim + iiTrim) / 2);
                            //    // Cambiamos el valor
                            //    mes = 7;
                            //}
                            // Semestral
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                            //{
                            //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            //    {
                            //        ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0) + (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0);
                            //    });
                            //    try
                            //    {
                            //        iSem += Convert.ToDouble(ncalc.Evaluate());
                            //    }
                            //    catch
                            //    {
                            //        iSem += 0;
                            //    }
                            //    // Cambiamos el valor
                            //    mes = 7;
                            //}
                            //} while (!(mes == 7));
                            // II Semestre
                            //do
                            //{
                            ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                            // Mensual
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                            //{
                            listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            {
                                ncalc.Parameters[sid.Variable] = (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0) + (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                            });
                            try
                            {
                                iiSem += Convert.ToDouble(ncalc.Evaluate());
                            }
                            catch
                            {
                                iiSem += 0;
                            }
                            // Contador
                            //mes++;
                            // Divide 6
                            //if (mes == 13)
                            //{
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                iiSem = (iiiTrim + ivTrim) / 2;
                            else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                                iiSem = iiSem;
                            else
                                iiSem = double.NaN;
                            //}
                            //}
                            // Trimestral
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                            //{
                            //    iiSem = ((iiiTrim + ivTrim) / 2);
                            //    // Cambiamos el valor
                            //    mes = 13;
                            //}
                            // Semestral
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                            //{
                            //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            //    {
                            //        ncalc.Parameters[sid.Variable] = (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0) + (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                            //    });
                            //    try
                            //    {
                            //        iiSem += Convert.ToDouble(ncalc.Evaluate());
                            //    }
                            //    catch
                            //    {
                            //        iiSem += 0;
                            //    }
                            //    // Cambiamos el valor
                            //    mes = 13;
                            //}
                            //} while (!(mes == 13));
                        }
                        // Anual -> Mensual, Trimestral, Semestral y Anual
                        if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                        {
                            //mes = 1;
                            // Anual
                            //do
                            //{
                            Expression ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                            //// Mensual
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                            //{
                            listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            {
                                ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0) + (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0) + (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0) + (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                            });
                            try
                            {
                                anual += Convert.ToDouble(ncalc.Evaluate());
                            }
                            catch
                            {
                                anual += 0;
                            }
                            // Contador
                            //mes++;
                            // Divide 12
                            //if (mes == 13)
                            //{
                            // Esperado
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                                anual = (iSem + iiSem) / 2;
                            else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                                anual = anual;
                            else
                                anual = double.NaN;
                            //}
                            //}
                            // Trimestral
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                            //{
                            //    anual = iTrim + iiTrim + iiiTrim + ivTrim;
                            //    // Cambiamos el valor
                            //    mes = 13;
                            //}
                            // Semestral
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                            //{
                            //    anual = iSem + iiSem;
                            //    // Cambiamos el valor
                            //    mes = 13;
                            //}
                            // Anual
                            //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                            //{
                            //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                            //    {
                            //        ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0) + (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0) + (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0) + (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                            //    });
                            //    try
                            //    {
                            //        anual += Convert.ToDouble(ncalc.Evaluate());
                            //    }
                            //    catch
                            //    {
                            //        anual += 0;
                            //    }
                            //    // Cambiamos el valor
                            //    mes = 13;
                            //}
                            //} while (!(mes == 13));
                        }
                        // Nombre
                        excelRange = excelWorksheet.Cells["A" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.Nombre;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Resumen
                        excelRange = excelWorksheet.Cells["B" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.ResumenNarrativo;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Nombre del Indicador
                        excelRange = excelWorksheet.Cells["C" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.NombreIndicador;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Formula
                        excelRange = excelWorksheet.Cells["D" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.Formula;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Linea Base
                        excelRange = excelWorksheet.Cells["E" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.LineaBase;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Frecuencia
                        excelRange = excelWorksheet.Cells["F" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.FrecuenciaMedicion;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Unidad de Medida
                        excelRange = excelWorksheet.Cells["G" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.UnidadMedida;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Medio Verificacion
                        excelRange = excelWorksheet.Cells["H" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.MedioVerificacion;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Meta
                        excelRange = excelWorksheet.Cells["I" + (filaSiguiente) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.Meta;
                        excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        // Enero a Diciembre Depende con el ID de MES
                        for (int x = 1; x <= periodoId; x++)
                        {
                            excelRange = excelWorksheet.Cells[obtenerColumnaJaP(x) + (filaSiguiente) + ""];
                            // I TRIM
                            if (x == 1)
                            {
                                excelRange.Value = Double.IsNaN(iTrim) ? null : string.Format("{0:n2}", iTrim);
                            }
                            // II TRIM
                            if (x == 2)
                            {
                                excelRange.Value = Double.IsNaN(iiTrim) ? null : string.Format("{0:n2}", iiTrim);
                            }
                            // I SEM
                            if (x == 3)
                            {
                                excelRange.Value = Double.IsNaN(iSem) ? null : string.Format("{0:n2}", iSem);
                            }
                            // III TRIM
                            if (x == 4)
                            {
                                excelRange.Value = Double.IsNaN(iiiTrim) ? null : string.Format("{0:n2}", iiiTrim);
                            }
                            // IV TRIM
                            if (x == 5)
                            {
                                excelRange.Value = Double.IsNaN(ivTrim) ? null : string.Format("{0:n2}", ivTrim);
                            }
                            // II SEM
                            if (x == 6)
                            {
                                excelRange.Value = Double.IsNaN(iiSem) ? null : string.Format("{0:n2}", iiSem);
                            }
                            // ANUAL
                            if (x == 7)
                            {
                                excelRange.Value = Double.IsNaN(anual) ? null : string.Format("{0:n2}", anual);
                            }


                            excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        }
                        // Contador
                        filaSiguiente++;
                    }

                    Session["DescargarExcel_ReporteSID"] = excelPackage.GetAsByteArray();
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

            if (Session["DescargarExcel_ReporteSID"] != null)
            {
                byte[] data = Session["DescargarExcel_ReporteSID"] as byte[];
                return File(data, "application/octet-stream", "ReporteSID.xlsx");
            }
            else
            {
                return new EmptyResult();
            }
        }

        private string obtenerColumnaJaP(int periodoId)
        {
            switch (periodoId)
            {
                case 1:
                    return "J";
                case 2:
                    return "K";
                case 3:
                    return "L";
                case 4:
                    return "M";
                case 5:
                    return "N";
                case 6:
                    return "O";
                case 7:
                    return "P";
                default:
                    return "J";
            }
        }

        private string obtenerColumnaJav(int periodoId)
        {
            switch (periodoId)
            {
                case 1:
                    return "J"; 
                case 2:
                    return "L";
                case 3:
                    return "N";
                case 4:
                    return "P";
                case 5:
                    return "R";
                case 6:
                    return "T";
                case 7:
                    return "V";
                default:
                    return "J";
            }
        }
        private string obtenerColumnaKaW(int periodoId)
        {
            switch (periodoId)
            {
                case 1:
                    return "K"; 
                case 2:
                    return "M";
                case 3:
                    return "O";
                case 4:
                    return "Q";
                case 5:
                    return "S";
                case 6:
                    return "U";
                case 7:
                    return "W";
                default:
                    return "K";
            }
        }

        private int ObtenerTrimestre(DateTime fechaReporte) {
            int tipoFecha = 0;
            DateTime fechaLimite = DateTime.Now;
            // Obtener el año actual y el trimestre actual
            int añoActual = DateTime.Now.Year;
           int trimestreActual = (fechaReporte.Month - 1) / 3 + 1;             
           // Obtener la fecha límite para el trimestre actual     
            if (trimestreActual == 4)
            {
                 fechaLimite = new DateTime(añoActual, 3 * trimestreActual, 31);
            }
            else
            {
                 fechaLimite = new DateTime(añoActual, 3 * trimestreActual + 1, 1).AddDays(-1);
            }

            if (trimestreActual == 1) { tipoFecha= 1; }
            if (trimestreActual == 2 && fechaReporte<fechaLimite) { tipoFecha = 2; }
            if (trimestreActual == 2 && fechaReporte==fechaLimite) { tipoFecha = 3; }
            if (trimestreActual == 3) { tipoFecha = 4; }
            if (trimestreActual == 4 && fechaReporte < fechaLimite) { tipoFecha = 5; }
            if (trimestreActual == 4 && fechaReporte == fechaLimite) { tipoFecha = 7; }
            

            return tipoFecha;
           
         }
        
        public Color GetCellBackgroundColor(double numero, decimal aceptableDesde, decimal aceptableHasta, decimal riesgoDesde, decimal riesgoHasta, decimal criticoAbajo, decimal criticoEncima)
        {
            decimal valor = Convert.ToDecimal(numero);

            if (valor >= aceptableDesde && valor <= aceptableHasta)
            {
                return Color.Green;
            }
            else if (valor >= riesgoDesde && valor <= riesgoHasta)
            {
                return Color.Orange;
            }
            else if (valor < criticoAbajo || valor > criticoEncima)
            {
                return Color.Red;
            }
            else
            {
                return Color.Transparent; // Color por defecto si no se cumple ninguna condición
            }
        }
        
        
    }
}