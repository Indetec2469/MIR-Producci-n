namespace RequisicionesAlmacenBL.Entities
{
    using RequisicionesAlmacen.Helpers;
    using RequisicionesAlmacenBL.Models.Mapeos;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(InvitacionCompraProveedorItemMetaData))]
    public partial class InvitacionCompraProveedorItem
    {
        //Propiedades del modelo
        public int InvitacionCompraProveedorId { get; set; }
        public int InvitacionCompraId { get; set; }
        public int ProveedorId { get; set; }
        public int EstatusId { get; set; }

        //Propiedades del Item
        public string Invitaciones { get; set; }
        public string RFC { get; set; }
        public string RazonSocial { get; set; }
        public bool Seleccionado { get; set; }
        public List<InvitacionCompraProveedorCotizacionItem> Cotizaciones { get; set; }

        public static explicit operator ARtblInvitacionCompraProveedor(InvitacionCompraProveedorItem obj)
        {
            ARtblInvitacionCompraProveedor modelo = new ARtblInvitacionCompraProveedor();

            modelo.InvitacionCompraProveedorId = obj.InvitacionCompraProveedorId;
            modelo.InvitacionCompraId = obj.InvitacionCompraId;
            modelo.ProveedorId = obj.ProveedorId;
            modelo.EstatusId = obj.EstatusId;

            if (obj.Cotizaciones != null)
            {
                modelo.Cotizaciones = new List<ArtblInvitacionCompraProveedorCotizacion>();

                foreach (InvitacionCompraProveedorCotizacionItem cotizacion in obj.Cotizaciones)
                {
                    modelo.Cotizaciones.Add((ArtblInvitacionCompraProveedorCotizacion) cotizacion);
                }
            }

            return modelo;
        }
    }

    public class InvitacionCompraProveedorItemMetaData
    {
    }
}