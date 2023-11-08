using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static RequisicionesAlmacen.Areas.MIR.MIR.Controllers.MatrizIndicadorResultadoController;

namespace RequisicionesAlmacen.Areas.MIR.Reportes.Models.ViewModel
{
    public class ReporteVariableIndicadorViewModel
    {
        public IEnumerable<MIvwComboListadoMIR> ComboListadoMIR { get; set; }
        public List<MesModel> ComboListaMes { get; set; }
    }
}