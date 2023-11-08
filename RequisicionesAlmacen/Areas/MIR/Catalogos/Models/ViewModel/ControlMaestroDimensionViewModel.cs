using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.MIR.Catalogos.Models.ViewModel
{
    public class ControlMaestroDimensionViewModel
    {
        public MItblControlMaestroDimension ControlMaestroDimension { get; set; }
        public MItblControlMaestroDimensionNivel ControlMaestroDimensionNivel { get; set; }
        public IEnumerable<MItblControlMaestroDimension> ListControlMaestroDimension { get; set; }
        public IEnumerable<MItblControlMaestroDimensionNivel> ListControlMaestroDimensionNivel { get; set; }
        public IEnumerable<GRtblControlMaestro> ListControlMaestroNivel { get; set; }
    }
}