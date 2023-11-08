namespace RequisicionesAlmacenBL.Entities
{
    using RequisicionesAlmacen.Helpers;
    using RequisicionesAlmacenBL.Models.Mapeos;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(InvitacionCompraDetalleItemMetaData))]
    public partial class InvitacionCompraDetalleItem
    {
        //Propiedades de la Invitacion de Compra Detalle
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
        public int RequisicionMaterialId { get; set; }
        public int RequisicionMaterialDetalleId { get; set; }
        public string CodigoRequisicion { get; set; }
        public string FechaRequisicion { get; set; }
        public byte[] Timestamp { get; set; }

        public static explicit operator ARtblInvitacionCompraDetalle(InvitacionCompraDetalleItem obj)
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
            modelo.EstatusId = obj.EstatusId;
            modelo.InvitacionArticuloDetalleId = obj.InvitacionArticuloDetalleId;

            return modelo;
        }
    }

    public class InvitacionCompraDetalleItemMetaData
    {
        [Display(Name = "Requisición")] 
        public string CodigoRequisicion { get; set; }

        [Display(Name = "Fecha")] 
        public string FechaRequisicion { get; set; }

        [Display(Name = "Producto")]
        public string Descripcion { get; set; }

        [Display(Name = "Costo Unitario")] 
        public decimal Costo { get; set; }

        [Display(Name = "Subtotal")] 
        public decimal Importe { get; set; }
    }
}