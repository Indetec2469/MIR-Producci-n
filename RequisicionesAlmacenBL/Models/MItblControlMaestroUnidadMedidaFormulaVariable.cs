using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(MItblControlMaestroUnidadMedidaFormulaVariableMetaData))]
    public partial class MItblControlMaestroUnidadMedidaFormulaVariable
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

    }

    public class MItblControlMaestroUnidadMedidaFormulaVariableMetaData
    {
    }
}
