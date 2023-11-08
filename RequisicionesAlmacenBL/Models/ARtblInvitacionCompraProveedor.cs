using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblInvitacionCompraProveedorMetaData))]
    public partial class ARtblInvitacionCompraProveedor
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() {
            "CreadoPorId",
            "FechaCreacion"
        };

        public List<ArtblInvitacionCompraProveedorCotizacion> Cotizaciones { get; set; }
    }

    public class ARtblInvitacionCompraProveedorMetaData
    {
        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "El Proveedor es requerido")]
        public int ProveedorId { get; set; }
    }
}