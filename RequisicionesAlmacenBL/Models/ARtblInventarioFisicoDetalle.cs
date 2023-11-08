using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblInventarioFisicoDetalleMetaData))]
    public partial class ARtblInventarioFisicoDetalle
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() { 
            "CreadoPorId",
            "FechaCreacion"
        };

    }

    public class ARtblInventarioFisicoDetalleMetaData
    {
        
    }
}
