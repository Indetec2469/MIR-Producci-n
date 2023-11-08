using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.MIR.MIR.Models.ViewModel
{
    public class MatrizPresupuestoVigenteViewModel
    {
        public int MIRId { get; set; }
        public string CodigoMIR { get; set; }
        public int ConfiguracionPresupuestoId { get; set; }
        public string ProgramaPresupuestario { get; set; }
        public decimal PresupuestoVigente { get; set; }
        public string FechaModificacion { get; set; }
        public IEnumerable<MIspConsultaMatrizPresupuestoVigente_Result> MatrizPresupuestoVigente { get; set; }
        public IEnumerable<MIspConsultaMatrizPresupuestoVigenteH_Result> MatrizPresupuestoVigenteH { get; set; }
        public IEnumerable<MItblControlMaestroControlPeriodo> ControlPeriodoList { get; set; }
    }
}