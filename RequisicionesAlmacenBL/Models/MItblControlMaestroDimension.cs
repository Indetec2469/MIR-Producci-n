using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(MItblControlMaestroDimensionMetaData))]
    public partial class MItblControlMaestroDimension
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

    }
    public class MItblControlMaestroDimensionMetaData
    {

    }
}
