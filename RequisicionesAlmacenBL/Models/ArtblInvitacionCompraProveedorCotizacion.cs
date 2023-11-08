using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ArtblInvitacionCompraProveedorCotizacionMetaData))]
    public partial class ArtblInvitacionCompraProveedorCotizacion
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() {
            "InvitacionCompraProveedorId",
            "FechaCotizacion",
            "CreadoPorId",
            "FechaCreacion"
        };

        public string NombreArchivoTmp { get; set; }
    }

    public class ArtblInvitacionCompraProveedorCotizacionMetaData
    {
    }
}