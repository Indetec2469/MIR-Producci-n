using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static RequisicionesAlmacen.Areas.MIR.MIR.Controllers.MatrizIndicadorResultadoController;

namespace RequisicionesAlmacen.Areas.MIR.Reportes.Models.ViewModel
{
    public class ReporteMatrizIndicadorResultadoViewModel
    {
        public IEnumerable<MIvwComboListadoMIR> ComboListadoMIR { get; set; }  
    }
}