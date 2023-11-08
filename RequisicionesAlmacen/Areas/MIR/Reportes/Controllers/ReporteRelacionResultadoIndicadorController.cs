using NCalc;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static RequisicionesAlmacen.Areas.MIR.MIR.Controllers.MatrizIndicadorResultadoController;

namespace RequisicionesAlmacen.Areas.MIR.Reportes.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.REPORTE_RRI)]
    public class ReporteRelacionResultadoIndicadorController : BaseController<ReporteRelacionResultadoIndicadorViewModel, ReporteRelacionResultadoIndicadorViewModel>
    {
        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Guardar(ReporteRelacionResultadoIndicadorViewModel modelView)
        {
            throw new NotImplementedException();
        }

        // GET: MIR/ReporteRelacionResultadoIndicador
        public ActionResult Index()
        {
            // Creamos el objecto nuevo
            ReporteRelacionResultadoIndicadorViewModel reporteRelacionResultadoIndicadorViewModel = new ReporteRelacionResultadoIndicadorViewModel();
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref reporteRelacionResultadoIndicadorViewModel);
            // Retornamos la vista junto con su Objeto Modelo
            return View("ReporteRelacionResultadoIndicador", reporteRelacionResultadoIndicadorViewModel);
        }

        public override ActionResult Listar()
        {
            throw new NotImplementedException();
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ReporteRelacionResultadoIndicadorViewModel reporteRelacionResultadoIndicadorViewModel)
        {
            reporteRelacionResultadoIndicadorViewModel.ComboListadoMIR = new MatrizIndicadorResultadoService().BuscaComboListadoMIR();
            reporteRelacionResultadoIndicadorViewModel.ComboListaPeriodo = new List<PeriodoModel>();
            MesMapeo mesMapeo = new MesMapeo();
            for (int x = 1; x <= 7; x++)
            {
                PeriodoModel periodoModel = new PeriodoModel();
                periodoModel.PeriodoId = x;
                periodoModel.NombrePeriodo = mesMapeo.BuscaNombrePeriodo(x);
                reporteRelacionResultadoIndicadorViewModel.ComboListaPeriodo.Add(periodoModel);
            }
        }

        [JsonException]
        public ActionResult BuscarReporte(int mirId, int periodoId)
        {
            int filaImportes;
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
                    excelRange.Value = "RELACIÓN DE RESULTADOS DE INDICADORES";
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
                    // Grupo 4 //
                    // Importe Presupuestado
                    excelRange = excelWorksheet.Cells["A" + filaSiguiente + ":B" + filaSiguiente + ""];
                    excelRange.Value = "IMPORTE PRESUPUESTADO:";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 8, false, false);
                    // Value de Importe Presupuestado
                    excelRange = excelWorksheet.Cells["C" + filaSiguiente + ":G" + filaSiguiente + ""];
                    excelRange.Value = "---";
                    excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                    // Importe Ejercido
                    excelRange = excelWorksheet.Cells["H" + filaSiguiente + ":I" + filaSiguiente + ""];
                    excelRange.Value = "IMPORTE EJERCIDO:";
                    excelHelper.ExcelTablaTitulo(ref excelRange, true, 8, false, false);
                    // Value de Importe Ejercido
                    excelRange = excelWorksheet.Cells["J" + filaSiguiente + ":" + columnaFin + "" + filaSiguiente + ""];
                    excelRange.Value = "---";
                    excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                    filaImportes = filaSiguiente;
                    // Border
                    excelRange = excelWorksheet.Cells["A" + filaSiguiente + ":" + columnaFin + "" + filaSiguiente + ""];
                    excelRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    // Contador Fila Siguiente
                    filaSiguiente++;
                    /////////////

                    int filaIndicadorDefault = filaSiguiente;
                    filaSiguiente = filaSiguiente + 2;
                    // Grupo 5 //
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
                    excelRange.Value = "PRESUPUESTO / META";
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
                    // Grupo 6 //
                    foreach (MIspRptLibroConsultaListaMatrizIndicadorResultado_Result rptLibroConsultaListaMatrizIndicadorResultado in listRptLibroConsultaListaMatrizIndicadorResultado)
                    {
                        // Variables
                        int contadorAvance = 4;

                        List<int> listaUnidadMedidaFormulaVariableId = new List<int>();
                        // Matriz Indicador Resultado Indicador
                        MItblMatrizIndicadorResultadoIndicador matrizIndicadorResultadoIndicador = new MatrizIndicadorResultadoIndicadorService().BuscaPorId(rptLibroConsultaListaMatrizIndicadorResultado.MIRIndicadorId.Value);

                        // Si es Fin o Proposito para 2 restas el contador
                        if (matrizIndicadorResultadoIndicador.NivelIndicadorId == ControlMaestroMapeo.Nivel.FIN || matrizIndicadorResultadoIndicador.NivelIndicadorId == ControlMaestroMapeo.Nivel.PROPOSITO)
                        {
                            contadorAvance = contadorAvance - 2;
                        }
                        // Nombre
                        excelRange = excelWorksheet.Cells["A" + filaSiguiente + ":A" + (filaSiguiente + (contadorAvance - 1)) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.Nombre;
                        excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                        // Resumen
                        excelRange = excelWorksheet.Cells["B" + filaSiguiente + ":B" + (filaSiguiente + (contadorAvance - 1)) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.ResumenNarrativo;
                        excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                        // Nombre del Indicador
                        excelRange = excelWorksheet.Cells["C" + filaSiguiente + ":C" + (filaSiguiente + (contadorAvance - 1)) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.NombreIndicador;
                        excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                        // Formula
                        excelRange = excelWorksheet.Cells["D" + filaSiguiente + ":D" + (filaSiguiente + (contadorAvance - 1)) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.Formula;
                        excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                        // Linea Base
                        excelRange = excelWorksheet.Cells["E" + filaSiguiente + ":E" + (filaSiguiente + (contadorAvance - 1)) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.LineaBase;
                        excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                        // Frecuencia
                        excelRange = excelWorksheet.Cells["F" + filaSiguiente + ":F" + (filaSiguiente + (contadorAvance - 1)) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.FrecuenciaMedicion;
                        excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                        // Unidad de Medida
                        excelRange = excelWorksheet.Cells["G" + filaSiguiente + ":G" + (filaSiguiente + (contadorAvance - 1)) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.UnidadMedida;
                        excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                        // Medio Verificacion
                        excelRange = excelWorksheet.Cells["H" + filaSiguiente + ":H" + (filaSiguiente + (contadorAvance - 1)) + ""];
                        excelRange.Value = rptLibroConsultaListaMatrizIndicadorResultado.MedioVerificacion;
                        excelHelper.ExcelTablaParrafo(ref excelRange, true, 8);
                        // Si es Componente y Actividad para Presupuestado y Ejercido
                        if (matrizIndicadorResultadoIndicador.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE || matrizIndicadorResultadoIndicador.NivelIndicadorId == ControlMaestroMapeo.Nivel.ACTIVIDAD)
                        {
                            // Lista Matriz Configuracion Presupuestal Detalle
                            IEnumerable<MItblMatrizConfiguracionPresupuestalDetalle> listaMatrizConfiguracionPresupuestalDetalle = new MatrizConfiguracionPresupuestalDetalleService().BuscaPorMIRIndicadorId(matrizIndicadorResultadoIndicador.MIRIndicadorId);

                            //Asignamos los montos de Importe presupuestado e Importe Devengado
                            MItblMatrizConfiguracionPresupuestalDetalle mcpdPorEjercer = listaMatrizConfiguracionPresupuestalDetalle.Where(cp => cp.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.POR_EJERCER).FirstOrDefault();
                            MItblMatrizConfiguracionPresupuestalDetalle mcpdDevengado = listaMatrizConfiguracionPresupuestalDetalle.Where(cp => cp.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.DEVENGADO).FirstOrDefault();
                            //Presupuestado
                            excelRange = excelWorksheet.Cells["C" + filaImportes + ":G" + filaImportes + ""];
                            excelRange.Value = mcpdPorEjercer == null ? "---" : string.Format("{0:n2}", mcpdPorEjercer.MItblMatrizConfiguracionPresupuestal.PresupuestoPorEjercer);

                            //Devengado
                            excelRange = excelWorksheet.Cells["J" + filaImportes + ":" + columnaFin + "" + filaImportes + ""];
                            excelRange.Value = mcpdDevengado == null ? "---" : string.Format("{0:n2}", mcpdDevengado.MItblMatrizConfiguracionPresupuestal.PresupuestoDevengado);

                            // Creamos variables
                            double iTrimPresupuestado = 0, iTrimEjercido = 0, iiTrimPresupuestado = 0, iiTrimEjercido = 0, iSemPresupuestado = 0, iSemEjercido = 0, iiiTrimPresupuestado = 0, iiiTrimEjercido = 0, ivTrimPresupuestado = 0, ivTrimEjercido = 0, iiSemPresupuestado = 0, iiSemEjercido = 0, anualPresupuestado = 0, anualEjercido = 0;
                            // I, II, III y IV Trimestre -> Mensual, Trimestral, Semestral y Anual
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                            {
                                listaMatrizConfiguracionPresupuestalDetalle.ToList().ForEach(mcpd =>
                                {
                                        // Enero, Febrero y Marzo
                                        // Presupuestado -> I Trimestre
                                        if (mcpd.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.POR_EJERCER)
                                        iTrimPresupuestado = Convert.ToDouble(mcpd.Enero + mcpd.Febrero + mcpd.Marzo);
                                        // Ejercido -  I Trimestre
                                        if (mcpd.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.DEVENGADO)
                                        iTrimEjercido = Convert.ToDouble(mcpd.Enero + mcpd.Febrero + mcpd.Marzo);
                                        // Abril, Mayo y Junio
                                        // Presupuestado -> II Trimestre
                                        if (mcpd.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.POR_EJERCER)
                                        iiTrimPresupuestado = Convert.ToDouble(mcpd.Abril + mcpd.Mayo + mcpd.Junio);
                                        // Ejercido -  II Trimestre
                                        if (mcpd.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.DEVENGADO)
                                        iiTrimEjercido = Convert.ToDouble(mcpd.Abril + mcpd.Mayo + mcpd.Junio);
                                        // Julio, Agosto y Septiembre
                                        // Presupuestado -> III Trimestre
                                        if (mcpd.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.POR_EJERCER)
                                        iiiTrimPresupuestado = Convert.ToDouble(mcpd.Julio + mcpd.Agosto + mcpd.Septiembre);
                                        // Ejercido -  III Trimestre
                                        if (mcpd.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.DEVENGADO)
                                        iiiTrimEjercido = Convert.ToDouble(mcpd.Julio + mcpd.Agosto + mcpd.Septiembre);
                                        // Octubre, Noviembre y Diciembre
                                        // Presupuestado -> IV Trimestre
                                        if (mcpd.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.POR_EJERCER)
                                        ivTrimPresupuestado = Convert.ToDouble(mcpd.Octubre + mcpd.Noviembre + mcpd.Diciembre);
                                        // Ejercido -  IV Trimestre
                                        if (mcpd.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.DEVENGADO)
                                        ivTrimEjercido = Convert.ToDouble(mcpd.Octubre + mcpd.Noviembre + mcpd.Diciembre);
                                });
                            }
                            // I Semestre -> Mensual, Trimestral, Semestral y Anual
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                            {
                                //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                                //{
                                    listaMatrizConfiguracionPresupuestalDetalle.ToList().ForEach(mcpd =>
                                    {
                                            // Enero, Febrero, Marzo, Abril, Mayo y Junio
                                            // Presupuestado -> I Semestre
                                            if (mcpd.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.POR_EJERCER)
                                            iSemPresupuestado = Convert.ToDouble(mcpd.Enero + mcpd.Febrero + mcpd.Marzo + mcpd.Abril + mcpd.Mayo + mcpd.Junio);
                                            // Ejercido -  I Semestre
                                            if (mcpd.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.DEVENGADO)
                                            iSemEjercido = Convert.ToDouble(mcpd.Enero + mcpd.Febrero + mcpd.Marzo + mcpd.Abril + mcpd.Mayo + mcpd.Junio);
                                            // Julio, Agosto, Septiembre, Octubre, Noviembre y Diciembre
                                            // Presupuestado -> II Semestre
                                            if (mcpd.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.POR_EJERCER)
                                            iiSemPresupuestado = Convert.ToDouble(mcpd.Julio + mcpd.Agosto + mcpd.Septiembre + mcpd.Octubre + mcpd.Noviembre + mcpd.Diciembre);
                                            // Ejercido -  II Semestre
                                            if (mcpd.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.DEVENGADO)
                                            iiSemEjercido = Convert.ToDouble(mcpd.Julio + mcpd.Agosto + mcpd.Septiembre + mcpd.Octubre + mcpd.Noviembre + mcpd.Diciembre);
                                    });
                                //}
                                //// Trimestral
                                //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                //{
                                //    // Presupuestado -> I Semestre
                                //    iSemPresupuestado = iTrimPresupuestado + iiTrimPresupuestado;
                                //    // Ejercido -> I Semestre
                                //    iSemEjercido = iTrimEjercido + iiTrimEjercido;
                                //    // Presupuestado -> II Semestre
                                //    iiSemPresupuestado = iiiTrimPresupuestado + ivTrimPresupuestado;
                                //    // Ejercido -> II Semestre
                                //    iiSemEjercido = iiiTrimEjercido + ivTrimEjercido;
                                //}
                            }
                            // Anual -> Mensual, Trimestral, Semestral y Anual
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                            {
                                //// Mensual y Anual
                                //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                                //{

                                    listaMatrizConfiguracionPresupuestalDetalle.ToList().ForEach(mcpd =>
                                    {
                                            // Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre y Diciembre
                                            // Presupuestado
                                            if (mcpd.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.POR_EJERCER)
                                            anualPresupuestado = Convert.ToDouble(mcpd.Enero + mcpd.Febrero + mcpd.Marzo + mcpd.Abril + mcpd.Mayo + mcpd.Junio + mcpd.Julio + mcpd.Agosto + mcpd.Septiembre + mcpd.Octubre + mcpd.Noviembre + mcpd.Diciembre);
                                            // Ejercido
                                            if (mcpd.MItblMatrizConfiguracionPresupuestal.ClasificadorId == ControlMaestroMapeo.TipoPresupuesto.DEVENGADO)
                                            anualEjercido = Convert.ToDouble(mcpd.Enero + mcpd.Febrero + mcpd.Marzo + mcpd.Abril + mcpd.Mayo + mcpd.Junio + mcpd.Julio + mcpd.Agosto + mcpd.Septiembre + mcpd.Octubre + mcpd.Noviembre + mcpd.Diciembre);
                                    });
                                //}
                                // Trimestral
                                //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                //{
                                //    // Presupuestado
                                //    anualPresupuestado = iTrimPresupuestado + iiTrimPresupuestado + iiiTrimPresupuestado + ivTrimPresupuestado;
                                //    // Ejercido
                                //    anualEjercido = iTrimEjercido + iiTrimEjercido + iiiTrimEjercido + ivTrimEjercido;
                                //}
                                // Semestral
                                //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                                //{
                                //    // Presupuestado
                                //    anualPresupuestado = iSemPresupuestado + iiSemPresupuestado;
                                //    // Ejercido
                                //    anualEjercido = iSemEjercido + iiSemEjercido;
                                //}
                            }
                            // Presupuestado
                            // Meta
                            excelRange = excelWorksheet.Cells["I" + filaSiguiente + ""];
                            excelRange.Value = "Presupuestado";
                            excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            // Enero a Diciembre Depende con el ID de MES
                            for (int x = 1; x <= periodoId; x++)
                            {
                                excelRange = excelWorksheet.Cells[obtenerColumnaJaP(x) + (filaSiguiente) + ""];
                                // I TRIM
                                if (x == 1)
                                {
                                    excelRange.Value = string.Format("{0:n2}", iTrimPresupuestado);
                                }
                                // II TRIM
                                if (x == 2)
                                {
                                    excelRange.Value = string.Format("{0:n2}", iiTrimPresupuestado);
                                }
                                // I SEM
                                if (x == 3)
                                {
                                    excelRange.Value = string.Format("{0:n2}", iSemPresupuestado);
                                }
                                // III TRIM
                                if (x == 4)
                                {
                                    excelRange.Value = string.Format("{0:n2}", iiiTrimPresupuestado);
                                }
                                // IV TRIM
                                if (x == 5)
                                {
                                    excelRange.Value = string.Format("{0:n2}", ivTrimPresupuestado);
                                }
                                // II SEM
                                if (x == 6)
                                {
                                    excelRange.Value = string.Format("{0:n2}", iiSemPresupuestado);
                                }
                                // ANUAL
                                if (x == 7)
                                {
                                    excelRange.Value = string.Format("{0:n2}", anualPresupuestado);
                                }

                                excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            }
                            //// I Trimestre
                            //excelRange = excelWorksheet.Cells["J" + filaSiguiente + ""];
                            //excelRange.Value = string.Format("{0:n2}", iTrimPresupuestado);
                            //excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            //// II Trimestre
                            //excelRange = excelWorksheet.Cells["K" + filaSiguiente + ""];
                            //excelRange.Value = string.Format("{0:n2}", iiTrimPresupuestado);
                            //excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            //// I Semestre
                            //excelRange = excelWorksheet.Cells["L" + filaSiguiente + ""];
                            //excelRange.Value = string.Format("{0:n2}", iSemPresupuestado);
                            //excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            //// III Trimestre
                            //excelRange = excelWorksheet.Cells["M" + filaSiguiente + ""];
                            //excelRange.Value = string.Format("{0:n2}", iiiTrimPresupuestado);
                            //excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            //// IV Trimestre
                            //excelRange = excelWorksheet.Cells["N" + filaSiguiente + ""];
                            //excelRange.Value = string.Format("{0:n2}", ivTrimPresupuestado);
                            //excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            //// II Semestre
                            //excelRange = excelWorksheet.Cells["O" + filaSiguiente + ""];
                            //excelRange.Value = string.Format("{0:n2}", iiSemPresupuestado);
                            //excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            //// Anual
                            //excelRange = excelWorksheet.Cells["P" + filaSiguiente + ""];
                            //excelRange.Value = string.Format("{0:n2}", anualPresupuestado);
                            //excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            // Ejercido
                            // Meta
                            excelRange = excelWorksheet.Cells["I" + (filaSiguiente + 1) + ""];
                            excelRange.Value = "Ejercido";
                            excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            // Enero a Diciembre Depende con el ID de MES
                            for (int x = 1; x <= periodoId; x++)
                            {
                                excelRange = excelWorksheet.Cells[obtenerColumnaJaP(x) + (filaSiguiente + 1) + ""];
                                // I TRIM
                                if (x == 1)
                                {
                                    excelRange.Value = string.Format("{0:n2}", iTrimEjercido);
                                }
                                // II TRIM
                                if (x == 2)
                                {
                                    excelRange.Value = string.Format("{0:n2}", iiTrimEjercido);
                                }
                                // I SEM
                                if (x == 3)
                                {
                                    excelRange.Value = string.Format("{0:n2}", iSemEjercido);
                                }
                                // III TRIM
                                if (x == 4)
                                {
                                    excelRange.Value = string.Format("{0:n2}", iiiTrimEjercido);
                                }
                                // IV TRIM
                                if (x == 5)
                                {
                                    excelRange.Value = string.Format("{0:n2}", ivTrimEjercido);
                                }
                                // II SEM
                                if (x == 6)
                                {
                                    excelRange.Value = string.Format("{0:n2}", iiSemEjercido);
                                }
                                // ANUAL
                                if (x == 7)
                                {
                                    excelRange.Value = string.Format("{0:n2}", anualEjercido);
                                }

                                excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            }
                            //// I Trimestre
                            //excelRange = excelWorksheet.Cells["J" + (filaSiguiente + 1) + ""];
                            //excelRange.Value = string.Format("{0:n2}", iTrimEjercido);
                            //excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            //// II Trimestre
                            //excelRange = excelWorksheet.Cells["K" + (filaSiguiente + 1) + ""];
                            //excelRange.Value = string.Format("{0:n2}", iiTrimEjercido);
                            //excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            //// I Semestre
                            //excelRange = excelWorksheet.Cells["L" + (filaSiguiente + 1) + ""];
                            //excelRange.Value = string.Format("{0:n2}", iSemEjercido);
                            //excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            //// III Trimestre
                            //excelRange = excelWorksheet.Cells["M" + (filaSiguiente + 1) + ""];
                            //excelRange.Value = string.Format("{0:n2}", iiiTrimEjercido);
                            //excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            //// IV Trimestre
                            //excelRange = excelWorksheet.Cells["N" + (filaSiguiente + 1) + ""];
                            //excelRange.Value = string.Format("{0:n2}", ivTrimEjercido);
                            //excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            //// II Semestre
                            //excelRange = excelWorksheet.Cells["O" + (filaSiguiente + 1) + ""];
                            //excelRange.Value = string.Format("{0:n2}", iiSemEjercido);
                            //excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            //// Anual
                            //excelRange = excelWorksheet.Cells["P" + (filaSiguiente + 1) + ""];
                            //excelRange.Value = string.Format("{0:n2}", anualEjercido);
                            //excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                        }
                        // Si es Fin, Proposito, Componente y Actividad para Esperado y Real
                        if (matrizIndicadorResultadoIndicador.NivelIndicadorId == ControlMaestroMapeo.Nivel.FIN || matrizIndicadorResultadoIndicador.NivelIndicadorId == ControlMaestroMapeo.Nivel.PROPOSITO || matrizIndicadorResultadoIndicador.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE || matrizIndicadorResultadoIndicador.NivelIndicadorId == ControlMaestroMapeo.Nivel.ACTIVIDAD)
                        {
                            // Lista Meta (Esperando)
                            IEnumerable<MItblMatrizIndicadorResultadoIndicadorMeta> listaMatrizIndicadorResultadoIndicadorMeta = new MatrizIndicadorResultadoIndicadorMetaService().BuscaPorMIRIndicadorId(matrizIndicadorResultadoIndicador.MIRIndicadorId);
                            // Lista Seguimiento Indicador Desempeño (Real)
                            IEnumerable<MIfnObtenerSeguimientoIndicadorDesempenio_Result> listaSeguimientoIndicadorDesempenio = new MatrizConfiguracionPresupuestalSeguimientoVariableService().BuscaReporteSIDPorMIRIndicadorId(matrizIndicadorResultadoIndicador.MIRIndicadorId, DateTime.Now);
                            // Control Maestro Unidad Medida 
                            MItblControlMaestroUnidadMedida controlMaestroUnidadMedida = new ControlMaestroUnidadMedidaService().BuscaPorId(matrizIndicadorResultadoIndicador.UnidadMedidaId);
                            // Creamos variables
                            int mes = 0;
                            Double iTrimEsperado = 0, iTrimReal = 0, iiTrimEsperado = 0, iiTrimReal = 0, iSemEsperado = 0, iSemReal = 0, iiiTrimEsperado = 0, iiiTrimReal = 0, ivTrimEsperado = 0, ivTrimReal = 0, iiSemEsperado = 0, iiSemReal = 0, anualEsperado = 0, anualReal = 0;
                            // I, II, III y IV Trimestre -> Mensual, Trimestral, Semestral y Anual
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                            {
                                //mes = 1;
                                //// I Trimestre
                                //do
                                //{
                                    Expression ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                                    //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                                    //{
                                        listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                                        {
                                            ncalc.Parameters[sid.Variable] =  (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0);
                                        });
                                        try
                                        {
                                            iTrimReal += Convert.ToDouble(ncalc.Evaluate());
                                        }
                                        catch
                                        {
                                            iTrimReal += 0;
                                        }
                                        //// Contador
                                        //mes++;
                                        //// Divide 3
                                        //if (mes == 4)
                                        //{
                                        // Esperado
                                        if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                                        {
                                            iTrimEsperado = Convert.ToDouble(listaMatrizIndicadorResultadoIndicadorMeta.ToList()[0].Valor +
                                                                             listaMatrizIndicadorResultadoIndicadorMeta.ToList()[1].Valor +
                                                                             listaMatrizIndicadorResultadoIndicadorMeta.ToList()[2].Valor) / 3;
                                            // Real
                                            iTrimReal = iTrimReal;
                                        }
                                        else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                            iTrimEsperado = Convert.ToDouble(listaMatrizIndicadorResultadoIndicadorMeta.ToList()[0].Valor);
                                        else
                                        {
                                            iTrimEsperado = Double.NaN;
                                            iTrimReal = Double.NaN;
                                        }

                                        //}
                                    //}
                                    //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                    //{
                                    //    // Esperado
                                    //    listaMatrizIndicadorResultadoIndicadorMeta.ToList().ForEach(meta =>
                                    //    {
                                    //        // Enero, Febrero y Marzo
                                    //        if (meta.Orden <= 2)
                                    //        {
                                    //            iTrimEsperado += Convert.ToDouble(meta.Valor);
                                    //        }
                                    //    });
                                    //    // Real
                                    //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                                    //    {
                                    //        ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0);
                                    //    });
                                    //    try
                                    //    {
                                    //        iTrimReal += Convert.ToDouble(ncalc.Evaluate());
                                    //    }
                                    //    catch
                                    //    {
                                    //        iTrimReal += 0;
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
                                            iiTrimReal += Convert.ToDouble(ncalc.Evaluate());
                                        }
                                        catch
                                        {
                                            iiTrimReal += 0;
                                        }
                                        //// Contador
                                        //mes++;
                                        //// Divide 3
                                        //if (mes == 7)
                                        //{
                                        // Esperado
                                        if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                                        {
                                            iiTrimEsperado = Convert.ToDouble(listaMatrizIndicadorResultadoIndicadorMeta.ToList()[3].Valor +
                                                                             listaMatrizIndicadorResultadoIndicadorMeta.ToList()[4].Valor +
                                                                             listaMatrizIndicadorResultadoIndicadorMeta.ToList()[5].Valor) / 3;
                                            // Real
                                            iiTrimReal = iiTrimReal;
                                        }
                                        else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                            iiTrimEsperado = Convert.ToDouble(listaMatrizIndicadorResultadoIndicadorMeta.ToList()[1].Valor);
                                        else
                                        {
                                            iiTrimEsperado = Double.NaN;
                                            iiTrimReal = Double.NaN;
                                        }

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
                                    //        iiTrimReal += Convert.ToDouble(ncalc.Evaluate());
                                    //    }
                                    //    catch
                                    //    {
                                    //        iiTrimReal += 0;
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
                                            iiiTrimReal += Convert.ToDouble(ncalc.Evaluate());
                                        }
                                        catch
                                        {
                                            iiiTrimReal += 0;
                                        }
                                        //// Contador
                                        //mes++;
                                        //// Divide 3
                                        //if (mes == 10)
                                        //{
                                        // Esperado
                                        if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                                        {
                                            iiiTrimEsperado = Convert.ToDouble(listaMatrizIndicadorResultadoIndicadorMeta.ToList()[6].Valor +
                                                                             listaMatrizIndicadorResultadoIndicadorMeta.ToList()[7].Valor +
                                                                             listaMatrizIndicadorResultadoIndicadorMeta.ToList()[8].Valor) / 3;
                                            // Real
                                            iiiTrimReal = iiiTrimReal;
                                        }
                                        else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                            iiiTrimEsperado = Convert.ToDouble(listaMatrizIndicadorResultadoIndicadorMeta.ToList()[2].Valor);
                                        else
                                        {
                                            iiiTrimEsperado = Double.NaN;
                                            iiiTrimReal = Double.NaN;
                                        }

                                        //}
                                    //}
                                    //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                    //{
                                    //    // Esperado
                                    //    listaMatrizIndicadorResultadoIndicadorMeta.ToList().ForEach(meta =>
                                    //    {
                                    //            // Julio, Agosto y Septiembre
                                    //            if (meta.Orden >= 6 && meta.Orden <= 8)
                                    //        {
                                    //            iiiTrimEsperado += Convert.ToDouble(meta.Valor);
                                    //        }
                                    //    });
                                    //    // Real
                                    //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                                    //    {
                                    //        ncalc.Parameters[sid.Variable] = (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0);
                                    //    });
                                    //    try
                                    //    {
                                    //        iiiTrimReal += Convert.ToDouble(ncalc.Evaluate());
                                    //    }
                                    //    catch
                                    //    {
                                    //        iiiTrimReal += 0;
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
                                            ivTrimReal += Convert.ToDouble(ncalc.Evaluate());
                                        }
                                        catch
                                        {
                                            ivTrimReal += 0;
                                        }
                                        //// Contador
                                        //mes++;
                                        //// Divide 3
                                        //if (mes == 13)
                                        //{
                                        // Esperado
                                        if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                                        {
                                            ivTrimEsperado = Convert.ToDouble(listaMatrizIndicadorResultadoIndicadorMeta.ToList()[9].Valor +
                                                                             listaMatrizIndicadorResultadoIndicadorMeta.ToList()[10].Valor +
                                                                             listaMatrizIndicadorResultadoIndicadorMeta.ToList()[11].Valor) / 3;
                                            // Real
                                            ivTrimReal = ivTrimReal;
                                        }
                                        else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                            ivTrimEsperado = Convert.ToDouble(listaMatrizIndicadorResultadoIndicadorMeta.ToList()[3].Valor);
                                        else
                                        {
                                            ivTrimEsperado = Double.NaN;
                                            ivTrimReal = Double.NaN;
                                        }

                                        //}
                                    //}
                                    //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                    //{
                                    //    // Esperado
                                    //    listaMatrizIndicadorResultadoIndicadorMeta.ToList().ForEach(meta =>
                                    //    {
                                    //        // Octubre, Noviembre y Diciembre
                                    //        if (meta.Orden >= 9 && meta.Orden <= 11)
                                    //        {
                                    //            ivTrimEsperado += Convert.ToDouble(meta.Valor);
                                    //        }
                                    //    });
                                    //    // Real
                                    //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                                    //    {
                                    //        ncalc.Parameters[sid.Variable] = (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                                    //    });
                                    //    try
                                    //    {
                                    //        ivTrimReal += Convert.ToDouble(ncalc.Evaluate());
                                    //    }
                                    //    catch
                                    //    {
                                    //        ivTrimReal += 0;
                                    //    }
                                    //    // Cambiamos el valor
                                    //    mes = 13;
                                    //}
                                //} while (!(mes == 13));
                            }
                            // I y II Semestre -> Mensual, Trimestral , Semestral y Anual
                            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                            {
                                //mes = 1;
                                //// I Semestre
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
                                            iSemReal += Convert.ToDouble(ncalc.Evaluate());
                                        }
                                        catch
                                        {
                                            iSemReal += 0;
                                        }
                                        //// Contador
                                        //mes++;
                                        //// Divide 6
                                        //if (mes == 7)
                                        //{
                                        // Esperado
                                        if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                        {
                                            iSemEsperado = (iTrimEsperado + iiTrimEsperado) / 2;
                                            // Real
                                            iSemReal = (iTrimReal + iiTrimReal) / 2;
                                        }
                                        else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                                        {
                                            iSemEsperado = Convert.ToDouble(listaMatrizIndicadorResultadoIndicadorMeta.ToList()[0].Valor);
                                        }
                                        else
                                        {
                                            iSemEsperado = double.NaN;
                                            iSemReal = double.NaN;
                                        }
                                        //}
                                    //}
                                    // Trimestral
                                    //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                    //{
                                    //    // Esperado
                                    //    iSemEsperado = (iTrimEsperado + iiTrimEsperado) / 2;
                                    //    // Real
                                    //    iSemReal = (iTrimReal + iiTrimReal) / 2;
                                    //    // Cambiamos el valor
                                    //    mes = 7;
                                    //}
                                    // Semestral
                                    //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                                    //{
                                    //    // Esperado
                                    //    listaMatrizIndicadorResultadoIndicadorMeta.ToList().ForEach(meta =>
                                    //    {
                                    //            // Enero, Febrero, Marzo, Abril, Mayo y Junio
                                    //            if (meta.Orden <= 5)
                                    //        {
                                    //            iSemEsperado += Convert.ToDouble(meta.Valor);
                                    //        }
                                    //    });
                                    //    // Real
                                    //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                                    //    {
                                    //        ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0) + (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0);
                                    //    });
                                    //    try
                                    //    {
                                    //        iSemReal += Convert.ToDouble(ncalc.Evaluate());
                                    //    }
                                    //    catch
                                    //    {
                                    //        iSemReal += 0;
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
                                            iiSemReal += Convert.ToDouble(ncalc.Evaluate());
                                        }
                                        catch
                                        {
                                            iiSemReal += 0;
                                        }
                                        //// Contador
                                        //mes++;
                                        //// Divide 6
                                        //if (mes == 13)
                                        //{
                                        // Esperado
                                        if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                        {
                                            iiSemEsperado = (iiiTrimEsperado + ivTrimEsperado) / 2;
                                            // Real
                                            iiSemReal = (iiiTrimReal + ivTrimReal) / 2;
                                        }
                                        else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                                            iiSemEsperado = Convert.ToDouble(listaMatrizIndicadorResultadoIndicadorMeta.ToList()[1].Valor);
                                        else
                                        {
                                            iiSemEsperado = double.NaN;
                                            iiSemReal = double.NaN;
                                        }

                                    //}
                                    //}
                                    // Trimestral
                                    //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                    //{
                                    //    // Esperado
                                    //    iiSemEsperado = (iiiTrimEsperado + ivTrimEsperado) / 2;
                                    //    // Real
                                    //    iiSemReal = ((iiiTrimReal + ivTrimReal) / 2);
                                    //    // Cambiamos el valor
                                    //    mes = 13;
                                    //}
                                    // Semestral
                                    //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                                    //{
                                    //    // Esperado
                                    //    listaMatrizIndicadorResultadoIndicadorMeta.ToList().ForEach(meta =>
                                    //    {
                                    //            // Julio, Agosto, Septiembre, Octubre, Noviembre y Diciembre
                                    //            if (meta.Orden >= 6 && meta.Orden <= 11)
                                    //        {
                                    //            iiSemEsperado += Convert.ToDouble(meta.Valor);
                                    //        }
                                    //    });
                                    //    // Real
                                    //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                                    //    {
                                    //        ncalc.Parameters[sid.Variable] = (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0) + (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                                    //    });
                                    //    try
                                    //    {
                                    //        iiSemReal += Convert.ToDouble(ncalc.Evaluate());
                                    //    }
                                    //    catch
                                    //    {
                                    //        iiSemReal += 0;
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
                                //// Anual
                                //do
                                //{
                                    Expression ncalc = new Expression(controlMaestroUnidadMedida.Formula);
                                    // Mensual
                                    //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL)
                                    //{
                                        listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                                        {
                                            ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0) + (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0) + (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0) + (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                                        });
                                        // Real
                                        try
                                        {
                                            anualReal += Convert.ToDouble(ncalc.Evaluate());
                                        }
                                        catch
                                        {
                                            anualReal += 0;
                                        }
                                        //// Contador
                                        //mes++;

                                        //if (mes == 13)
                                        //{

                                        // Esperado
                                        if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.MENSUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                                        {
                                            anualEsperado = (iSemEsperado + iiSemEsperado) / 2;
                                            // Real
                                            anualReal = (iSemReal + iiSemReal) / 2;
                                        }
                                        else if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                                            anualEsperado = Convert.ToDouble(listaMatrizIndicadorResultadoIndicadorMeta.ToList()[0].Valor);
                                        else
                                            anualEsperado = double.NaN;


                                    //}
                                    //}
                                    // Trimestral
                                    //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.TRIMESTRAL)
                                    //{
                                    //    // Esperado
                                    //    anualEsperado = iTrimEsperado + iiTrimEsperado + iiiTrimEsperado + ivTrimEsperado;
                                    //    // Real
                                    //    anualReal = iTrimReal + iiTrimReal + iiiTrimReal + ivTrimReal;
                                    //    // Cambiamos el valor
                                    //    mes = 13;
                                    //}
                                    // Semestral
                                    //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.SEMESTRAL)
                                    //{
                                    //    // Esperado
                                    //    anualEsperado = iSemEsperado + iiSemEsperado;
                                    //    // Real
                                    //    anualReal = iSemReal + iiSemReal;
                                    //    // Cambiamos el valor
                                    //    mes = 13;
                                    //}
                                    // Anual
                                    //if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == FrecuenciaMedicionMapeo.ID.ANUAL)
                                    //{
                                    //    // Esperado
                                    //    listaMatrizIndicadorResultadoIndicadorMeta.ToList().ForEach(meta =>
                                    //    {
                                    //        anualEsperado += Convert.ToDouble(meta.Valor);
                                    //    });
                                    //    // Real
                                    //    listaSeguimientoIndicadorDesempenio.ToList().ForEach(sid =>
                                    //    {
                                    //        ncalc.Parameters[sid.Variable] = (sid.Enero != null ? sid.Enero : 0) + (sid.Febrero != null ? sid.Febrero : 0) + (sid.Marzo != null ? sid.Marzo : 0) + (sid.Abril != null ? sid.Abril : 0) + (sid.Mayo != null ? sid.Mayo : 0) + (sid.Junio != null ? sid.Junio : 0) + (sid.Julio != null ? sid.Julio : 0) + (sid.Agosto != null ? sid.Agosto : 0) + (sid.Septiembre != null ? sid.Septiembre : 0) + (sid.Octubre != null ? sid.Octubre : 0) + (sid.Noviembre != null ? sid.Noviembre : 0) + (sid.Diciembre != null ? sid.Diciembre : 0);
                                    //    });
                                    //    try
                                    //    {
                                    //        anualReal += Convert.ToDouble(ncalc.Evaluate());
                                    //    }
                                    //    catch
                                    //    {
                                    //        anualReal += 0;
                                    //    }
                                    //    // Cambiamos el valor
                                    //    mes = 13;
                                    //}
                                //} while (!(mes == 13));
                            }
                            // Esperado
                            // Meta
                            excelRange = excelWorksheet.Cells["I" + (contadorAvance == 4 ? filaSiguiente + 2 : filaSiguiente) + ""];
                            excelRange.Value = "Esperado";
                            excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            // Enero a Diciembre Depende con el ID de MES
                            for (int x = 1; x <= periodoId; x++)
                            {
                                excelRange = excelWorksheet.Cells[obtenerColumnaJaP(x) + (contadorAvance == 4 ? filaSiguiente + 2 : filaSiguiente) + ""];
                                // I TRIM
                                if (x == 1)
                                {
                                    excelRange.Value = Double.IsNaN(iTrimEsperado) ? null : string.Format("{0:n2}", iTrimEsperado);
                                }
                                // II TRIM
                                if (x == 2)
                                {
                                    excelRange.Value = Double.IsNaN(iiTrimEsperado) ? null : string.Format("{0:n2}", iiTrimEsperado);
                                }
                                // I SEM
                                if (x == 3)
                                {
                                    excelRange.Value = Double.IsNaN(iSemEsperado) ? null : string.Format("{0:n2}", iSemEsperado);
                                }
                                // III TRIM
                                if (x == 4)
                                {
                                    excelRange.Value = Double.IsNaN(iiiTrimEsperado) ? null : string.Format("{0:n2}", iiiTrimEsperado);
                                }
                                // IV TRIM
                                if (x == 5)
                                {
                                    excelRange.Value = Double.IsNaN(ivTrimEsperado) ? null : string.Format("{0:n2}", ivTrimEsperado);
                                }
                                // II SEM
                                if (x == 6)
                                {
                                    excelRange.Value = Double.IsNaN(iiSemEsperado) ? null : string.Format("{0:n2}", iiSemEsperado);
                                }
                                // ANUAL
                                if (x == 7)
                                {
                                    excelRange.Value = Double.IsNaN(anualEsperado) ? null : string.Format("{0:n2}", anualEsperado);
                                }

                                excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            }
                            // Real
                            // Meta
                            excelRange = excelWorksheet.Cells["I" + (contadorAvance == 4 ? filaSiguiente + 3 : filaSiguiente + 1) + ""];
                            excelRange.Value = "Real";
                            excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            // Enero a Diciembre Depende con el ID de MES
                            for (int x = 1; x <= periodoId; x++)
                            {
                                excelRange = excelWorksheet.Cells[obtenerColumnaJaP(x) + (contadorAvance == 4 ? filaSiguiente + 3 : filaSiguiente + 1) + ""];
                                // I TRIM
                                if (x == 1)
                                {
                                    excelRange.Value = Double.IsNaN(iTrimReal) ? null : string.Format("{0:n2}", iTrimReal);
                                }
                                // II TRIM
                                if (x == 2)
                                {
                                    excelRange.Value = Double.IsNaN(iiTrimReal) ? null : string.Format("{0:n2}", iiTrimReal);
                                }
                                // I SEM
                                if (x == 3)
                                {
                                    excelRange.Value = Double.IsNaN(iSemReal) ? null : string.Format("{0:n2}", iSemReal);
                                }
                                // III TRIM
                                if (x == 4)
                                {
                                    excelRange.Value = Double.IsNaN(iiiTrimReal) ? null : string.Format("{0:n2}", iiiTrimReal);
                                }
                                // IV TRIM
                                if (x == 5)
                                {
                                    excelRange.Value = Double.IsNaN(ivTrimReal) ? null : string.Format("{0:n2}", ivTrimReal);
                                }
                                // II SEM
                                if (x == 6)
                                {
                                    excelRange.Value = Double.IsNaN(iiSemReal) ? null : string.Format("{0:n2}", iiSemReal);
                                }
                                // ANUAL
                                if (x == 7)
                                {
                                    excelRange.Value = Double.IsNaN(anualReal) ? null : string.Format("{0:n2}", anualReal);
                                }

                                excelHelper.ExcelTablaParrafo(ref excelRange, false, 8);
                            }
                        }

                        filaSiguiente = filaSiguiente + 1 + (contadorAvance - 1);
                    }

                    Session["DescargarExcel_ReporteRRI"] = excelPackage.GetAsByteArray();
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

            if (Session["DescargarExcel_ReporteRRI"] != null)
            {
                byte[] data = Session["DescargarExcel_ReporteRRI"] as byte[];
                return File(data, "application/octet-stream", "ReporteRRI.xlsx");
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
    }
}