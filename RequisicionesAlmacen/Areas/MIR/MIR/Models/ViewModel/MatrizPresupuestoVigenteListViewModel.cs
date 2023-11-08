using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.MIR.MIR.Models.ViewModel
{
    public class MatrizPresupuestoVigenteListViewModel
    {

        public IEnumerable<MIvwListadoMatrizPresupuestoVigente> ListadoMatrizConfiguracionPresupuestal { get; set; }
    }
}