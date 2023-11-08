using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(MItblMatrizConfiguracionPresupuestalMetaData))]
    public partial class MItblMatrizConfiguracionPresupuestal
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

    }

    public class MItblMatrizConfiguracionPresupuestalMetaData
    {
        [Display(Name = "Presupuesto Vigente")]
        public decimal PresupuestoPorEjercer { get; set; }
    }
}
