namespace RequisicionesAlmacenBL.Entities
{
    using RequisicionesAlmacen.Helpers;
    using RequisicionesAlmacenBL.Models.Mapeos;
    using System;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(InvitacionArticuloDetalleItemMetaData))]
    public partial class InvitacionArticuloDetalleItem
    {
        //Propiedades de la Invitación de Compra Detalle
        public int InvitacionCompraDetalleId { get; set; }
        public int InvitacionCompraId { get; set; }
        public string TarifaImpuestoId { get; set; }
        public string ProductoId { get; set; }
        public int CuentaPresupuestalEgrId { get; set; }
        public string Descripcion { get; set; }
        public double Cantidad { get; set; }
        public decimal Costo { get; set; }
        public decimal Importe { get; set; }
        public double IEPS { get; set; }
        public double Ajuste { get; set; }
        public double IVA { get; set; }
        public double ISH { get; set; }
        public double RetencionISR { get; set; }
        public double RetencionCedular { get; set; }
        public double RetencionIVA { get; set; }
        public double TotalPresupuesto { get; set; }
        public double Total { get; set; }
        public int EstatusId { get; set; }
        public int InvitacionArticuloDetalleId { get; set; }

        //Propiedades del Item
        public string Requisicion { get; set; }
        public string FechaRegistro { get; set; }
        public int ProveedorId { get; set; }
        public string Proveedor { get; set; }
        public string AlmacenId { get; set; }
        public string Almacen { get; set; }
        public int TipoCompraId { get; set; }
        public string TipoCompra { get; set; }
        public string Producto { get; set; }
        public int InvitacionArticuloId { get; set; }
        public byte[] InvitacionTimestamp { get; set; }
        public byte[] DetalleTimestamp { get; set; }
        public bool Seleccionado { get; set; }

        public static explicit operator ARtblInvitacionCompraDetalle(InvitacionArticuloDetalleItem obj)
        {
            ARtblInvitacionCompraDetalle modelo = new ARtblInvitacionCompraDetalle();

            modelo.InvitacionCompraDetalleId = obj.InvitacionCompraDetalleId;
            modelo.InvitacionCompraId = obj.InvitacionCompraId;
            modelo.TarifaImpuestoId = obj.TarifaImpuestoId;
            modelo.ProductoId = obj.ProductoId;
            modelo.CuentaPresupuestalEgrId = obj.CuentaPresupuestalEgrId;
            modelo.Descripcion = obj.Descripcion;
            modelo.Cantidad = obj.Cantidad;
            modelo.Costo = obj.Costo;
            modelo.Importe = obj.Importe;
            modelo.IEPS = obj.IEPS;
            modelo.Ajuste = obj.Ajuste;
            modelo.IVA = obj.IVA;
            modelo.ISH = obj.ISH;
            modelo.RetencionISR = obj.RetencionISR;
            modelo.RetencionCedular = obj.RetencionCedular;
            modelo.RetencionIVA = obj.RetencionIVA;
            modelo.TotalPresupuesto = obj.TotalPresupuesto;
            modelo.Total = obj.Total;
            modelo.InvitacionArticuloDetalleId = obj.InvitacionArticuloDetalleId;

            return modelo;
        }
    }

    public class InvitacionArticuloDetalleItemMetaData
    {
        [Display(Name = "Proveedor")]
        public string Proveedor { get; set; }

        [Display(Name = "Almacén")]
        public string Almacen { get; set; }

        [Display(Name = "Requisición")]
        public string Requisicion { get; set; }

        [Display(Name = "Fecha Requisición")]
        public string FechaRegistro { get; set; }

        [Display(Name = "Artículo")]
        public string Producto { get; set; }

        [Display(Name = "Costo Unitario")] 
        public decimal Costo { get; set; }

        [Display(Name = "Subtotal")] 
        public decimal Importe { get; set; }
    }
}