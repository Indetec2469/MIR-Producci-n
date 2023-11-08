namespace RequisicionesAlmacenBL.Entities
{
    using RequisicionesAlmacen.Helpers;
    using RequisicionesAlmacenBL.Models.Mapeos;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(InvitacionCompraDetallePrecioProveedorItemMetaData))]
    public partial class InvitacionCompraDetallePrecioProveedorItem
    {
        //Propiedades del modelo
        public int InvitacionCompraDetallePrecioProveedorId { get; set; }
        public int InvitacionCompraDetalleId { get; set; }
        public int ProveedorId { get; set; }
        public decimal PrecioArticulo { get; set; }
        public Nullable<bool> Ganador { get; set; }
        public string ComentarioGanador { get; set; }
        public string ComentarioCotizacion { get; set; }
        public int EstatusId { get; set; }

        //Propiedades del Item
        public int ProductoId { get; set; }
        public string Descripcion { get; set; }
        public string Producto { get; set; }
        public string RazonSocial { get; set; }
        public string Proveedor { get; set; }

        public static explicit operator ArtblInvitacionCompraDetallePrecioProveedor(InvitacionCompraDetallePrecioProveedorItem obj)
        {
            ArtblInvitacionCompraDetallePrecioProveedor modelo = new ArtblInvitacionCompraDetallePrecioProveedor();

            modelo.InvitacionCompraDetallePrecioProveedorId = obj.InvitacionCompraDetallePrecioProveedorId;
            modelo.InvitacionCompraDetalleId = obj.InvitacionCompraDetalleId;
            modelo.ProveedorId = obj.ProveedorId;
            modelo.PrecioArticulo = obj.PrecioArticulo;
            modelo.Ganador = obj.Ganador;
            modelo.ComentarioGanador = obj.ComentarioGanador;
            modelo.ComentarioCotizacion = obj.ComentarioCotizacion;
            modelo.EstatusId = obj.EstatusId;

            return modelo;
        }
    }

    public class InvitacionCompraDetallePrecioProveedorItemMetaData
    {
    }
}