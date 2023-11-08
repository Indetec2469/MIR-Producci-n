using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ArtblInvitacionCompraDetallePrecioProveedorMetaData))]
    public partial class ArtblInvitacionCompraDetallePrecioProveedor
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() {
            "InvitacionCompraDetalleId",
            "ProveedorId",
            "CreadoPorId",
            "FechaCreacion"
        };
    }

    public class ArtblInvitacionCompraDetallePrecioProveedorMetaData
    {
    }
}