using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.MIR.Catalogos.Models.ViewModel
{
    public class ControlMaestroUnidadMedidaViewModel
    {
        public MItblControlMaestroUnidadMedida ControlMaestroUnidadMedidaModel { get; set; }
        public MItblControlMaestroUnidadMedidaDimension ControlMaestroUnidadMedidaDimensionModel { get; set; }
        public MItblControlMaestroUnidadMedidaFormulaVariable ControlMaestroUnidadMedidaFormulaVariableModel { get; set; }
        public IEnumerable<MItblControlMaestroUnidadMedida> ListControlMaestroUnidadMedida { get; set; }
        public IEnumerable<MItblControlMaestroUnidadMedidaDimension> ListControlMaestroUnidadMedidaDimension { get; set; }
        public IEnumerable<MItblControlMaestroUnidadMedidaFormulaVariable> ListControlMaestroUnidadMedidaFormulaVariable { get; set; }
        public IEnumerable<MItblControlMaestroDimension> ListControlMaestroDimension { get; set; }
    }
}