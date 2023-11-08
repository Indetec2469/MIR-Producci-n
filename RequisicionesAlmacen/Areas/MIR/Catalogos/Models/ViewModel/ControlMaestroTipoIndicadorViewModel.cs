using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.MIR.Catalogos.Models.ViewModel
{
    public class ControlMaestroTipoIndicadorViewModel
    {
        public MItblControlMaestroTipoIndicador ControlMaestroTipoIndicador { get; set; }
        public MItblControlMaestroTipoIndicadorNivel ControlMaestroTipoIndicadorNivel { get; set; }
        public IEnumerable<MItblControlMaestroTipoIndicador> ListControlMaestroTipoIndicador { get; set; }
        public IEnumerable<MItblControlMaestroTipoIndicadorNivel> ListControlMaestroTipoIndicadorNivel { get; set; }
        public IEnumerable<GRtblControlMaestro> ListControlMaestroNivel { get; set; }
    }
}