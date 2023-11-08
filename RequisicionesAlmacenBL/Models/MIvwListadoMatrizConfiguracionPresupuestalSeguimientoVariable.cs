using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(MIvwListadoMatrizConfiguracionPresupuestalSeguimientoVariableMetaData))]
    public partial class MIvwListadoMatrizConfiguracionPresupuestalSeguimientoVariable
    {
    }

    public class MIvwListadoMatrizConfiguracionPresupuestalSeguimientoVariableMetaData
    {

        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Tipo de Plan")]
        public string TipoPlanDesarrollo { get; set; }

        [Display(Name = "Plan de Desarrollo")]
        public string NombrePlanPeriodo { get; set; }

        [Display(Name = "Programa Presupuestario")]
        public string ProgramaPresupuestario { get; set; }

        [Display(Name = "# Indicadores")]
        public Nullable<int> Indicadores { get; set; }
    }
}
