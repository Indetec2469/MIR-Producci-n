using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(MItblControlMaestroUnidadMedidaMetaData))]
    public partial class MItblControlMaestroUnidadMedida
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

    }

    public class MItblControlMaestroUnidadMedidaMetaData
    {
    }
}
