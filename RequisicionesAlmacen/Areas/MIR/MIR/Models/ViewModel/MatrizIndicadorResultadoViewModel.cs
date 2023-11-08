using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static RequisicionesAlmacen.Areas.MIR.MIR.Controllers.MatrizIndicadorResultadoController;

namespace RequisicionesAlmacen.Areas.MIR.MIR.Models.ViewModel
{
    public class MatrizIndicadorResultadoViewModel
    {
        public IEnumerable<MIvwListadoMIR> ListMatrizIndicadorResultado { get; set; }
        public MItblMatrizIndicadorResultado MatrizIndicadorResultado { get; set; }
        public MItblMatrizIndicadorResultado MatrizIndicadorResultadoModel { get; set; }
        public MItblMatrizIndicadorResultadoIndicador MatrizIndicadorResultadoIndicadorModel { get; set; }
        public MItblMatrizIndicadorResultadoIndicadorMeta MatrizIndicadorResultadoIndicadorMetaModel { get; set; }
        public MItblMatrizIndicadorResultadoIndicadorFormulaVariable MatrizIndicadorResultadoIndicadorFormulaVariableModel { get; set; }
        public IEnumerable<MItblMatrizIndicadorResultadoIndicador> ListMatrizIndicadorResultadoIndicador { get; set; }
        public IEnumerable<MItblMatrizIndicadorResultadoIndicadorMeta> ListMatrizIndicadorResultadoIndicadorMeta { get; set; }
        public IEnumerable<MItblMatrizIndicadorResultadoIndicadorFormulaVariable> ListMatrizIndicadorResultadoIndicadorFormulaVariable { get; set; }
        public IEnumerable<EjercicioModel> ListEjercicio { get; set; }
        public IEnumerable<MItblPlanDesarrollo> ListPlanDesarrollo { get; set; }
        public IEnumerable<MItblPlanDesarrolloEstructura> ListPlanDesarrolloEstructura { get; set; }
        public IEnumerable<spComboProgramaGobierno_Result> ListProgramaGobierno { get; set; }
        public IEnumerable<MIspConsultaTipoIndicadorConNivel_Result> ListControlMaestroTipoIndicadorConNivel { get; set; }
        public IEnumerable<MIspConsultaDimensionConNivel_Result> ListControlMaestroDimensionConNivel { get; set; }
        public IEnumerable<MIspConsultaUnidadMedidaConDimension_Result> ListControlMaestroUnidadMedidaConDimension { get; set; }
        public IEnumerable<MItblControlMaestroUnidadMedidaFormulaVariable> ListControlMaestroUnidadMedidaFormulaVariable { get; set; }
        public IEnumerable<GRtblControlMaestro> ListNivel { get; set; }
        public IEnumerable<MIspConsultaFrecuenciaMedicionConNivel_Result> ListControlMaestroFrecuenciaMedicionConNivel { get; set; }
        public IEnumerable<GRtblControlMaestro> ListSentido { get; set; }
        public IEnumerable<GRtblControlMaestro> ListTipoComponente { get; set; }
        public IEnumerable<MIspConsultaRepProyecto_Result> ListProyecto { get; set; }
        public Boolean EsPermisoProyecto { get; set; }
    }
}