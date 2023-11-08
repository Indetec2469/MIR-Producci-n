using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.MIR.MIR.Models.ViewModel
{
    public class MatrizPresupuestoDevengadoViewModel
    {
        public int MIRId { get; set; }
        public string CodigoMIR { get; set; }
        public int ConfiguracionPresupuestoId { get; set; }
        public string ProgramaPresupuestario { get; set; }
        public decimal PresupuestoDevengado { get; set; }
        public string FechaModificacion { get; set; }
        public IEnumerable<MIspConsultaMatrizPresupuestoDevengado_Result> MatrizPresupuestoDevengado { get; set; }
        public IEnumerable<MIspConsultaMatrizPresupuestoDevengadoH_Result> MatrizPresupuestoDevengadoH { get; set; }
        public IEnumerable<MItblControlMaestroControlPeriodo> ControlPeriodoList { get; set; }
        public IEnumerable<MItblMatrizConfiguracionPresupuestalDetalle> MatrizPresupuestoDevengadoRespaldo { get; set; }
    }

}