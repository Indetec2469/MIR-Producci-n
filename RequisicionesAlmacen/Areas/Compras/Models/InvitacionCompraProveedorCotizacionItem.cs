namespace RequisicionesAlmacenBL.Entities
{
    using RequisicionesAlmacen.Helpers;
    using RequisicionesAlmacenBL.Models.Mapeos;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    [MetadataType(typeof(InvitacionCompraProveedorCotizacionItemMetaData))]
    public partial class InvitacionCompraProveedorCotizacionItem
    {
        //Propiedades de la Invitacion de Compra Proveedor Cotización
        public int InvitacionCompraProveedorCotizacionId { get; set; }
        public int InvitacionCompraProveedorId { get; set; }
        public Nullable<Guid> CotizacionId { get; set; }
        public DateTime FechaCotizacion { get; set; }
        public string Comentario { get; set; }
        public int EstatusId { get; set; }
        public byte[] Timestamp { get; set; }

        //Propiedades del Item
        public int ProveedorId { get; set; }
        public string Proveedor { get; set; }
        public string NombreArchivo { get; set; }
        public string CreadoPorUsuario { get; set; }
        public string Tipo { get; set; }
        public string NombreArchivoTmp { get; set; }

        public static explicit operator ArtblInvitacionCompraProveedorCotizacion(InvitacionCompraProveedorCotizacionItem obj)
        {
            ArtblInvitacionCompraProveedorCotizacion modelo = new ArtblInvitacionCompraProveedorCotizacion();

            modelo.InvitacionCompraProveedorCotizacionId = obj.InvitacionCompraProveedorCotizacionId;
            modelo.InvitacionCompraProveedorId = obj.InvitacionCompraProveedorId;
            modelo.CotizacionId = obj.CotizacionId;
            modelo.FechaCotizacion = obj.FechaCotizacion;
            modelo.Comentario = obj.Comentario;
            modelo.EstatusId = obj.EstatusId;
            modelo.NombreArchivoTmp = obj.NombreArchivoTmp;

            return modelo;
        }
    }

    public class InvitacionCompraProveedorCotizacionItemMetaData
    {
        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "El Proveedor es requerido")]
        public int InvitacionCompraProveedorId { get; set; }

        [Display(Name = "Nombre archivo")]
        public string NombreArchivo { get; set; }

        [Display(Name = "Fecha")] 
        public DateTime FechaCotizacion { get; set; }

        [Display(Name = "Creado por")] 
        public string CreadoPorUsuario { get; set; }
    }
}