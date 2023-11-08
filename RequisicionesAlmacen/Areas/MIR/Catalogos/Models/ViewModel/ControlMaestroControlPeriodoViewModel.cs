using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.MIR.Catalogos.Models.ViewModel
{
    public class ControlMaestroControlPeriodoViewModel
    {
        public IEnumerable<MItblControlMaestroControlPeriodo> ListControlMaestroControlPeriodo { get; set; }
        public IEnumerable<GRtblControlMaestro> ListControlMaestroMIEstatusPeriodo { get; set; }
    }
}