using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(GRtblArchivoMetaData))]
    public partial class GRtblArchivo
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();
    }

    public class GRtblArchivoMetaData
    {
        
    }
}
