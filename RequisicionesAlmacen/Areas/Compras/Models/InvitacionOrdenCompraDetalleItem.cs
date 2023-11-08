namespace RequisicionesAlmacenBL.Entities
{
    using RequisicionesAlmacen.Helpers;
    using RequisicionesAlmacenBL.Models.Mapeos;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class InvitacionOrdenCompraDetalleItem
    {
        //Propiedades de la Orden de Compra Detalle
        public int OrdenCompraDetId { get; set; }
        public int OrdenCompraId { get; set; }
        public string TarifaImpuestoId { get; set; }
        public string ProductoId { get; set; }
        public int CuentaPresupuestalEgrId { get; set; }
        public string Descripcion { get; set; }
        public string Status { get; set; }
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

        //Propiedades del Item
        public int RequisicionMaterialDetalleId { get; set; }
        public int InvitacionCompraDetalleId { get; set; }

        public static explicit operator tblOrdenCompraDet(InvitacionOrdenCompraDetalleItem obj)
        {
            tblOrdenCompraDet modelo = new tblOrdenCompraDet();

            modelo.OrdenCompraDetId = obj.OrdenCompraDetId;
            modelo.OrdenCompraId = obj.OrdenCompraId;
            modelo.TarifaImpuestoId = obj.TarifaImpuestoId;
            modelo.ProductoId = obj.ProductoId;
            modelo.CuentaPresupuestalEgrId = obj.CuentaPresupuestalEgrId;
            modelo.Descripcion = obj.Descripcion;
            modelo.Status = obj.Status;
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
            modelo.RequisicionMaterialDetalleId = obj.RequisicionMaterialDetalleId;
            modelo.InvitacionCompraDetalleId = obj.InvitacionCompraDetalleId;

            return modelo;
        }
    }
}