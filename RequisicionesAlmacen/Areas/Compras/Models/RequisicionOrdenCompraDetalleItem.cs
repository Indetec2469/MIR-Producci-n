namespace RequisicionesAlmacenBL.Entities
{
    using RequisicionesAlmacen.Helpers;
    using RequisicionesAlmacenBL.Models.Mapeos;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class RequisicionOrdenCompraDetalleItem
    {
        //Propiedades del Item
        public int RequisicionMaterialDetalleId { get; set; }
        public string Solicitud { get; set; }
        public string Fecha { get; set; }
        public string Usuario { get; set; }
        public string Area { get; set; }
        public string AlmacenId { get; set; }
        public string Almacen { get; set; }
        public string UnidadAdministrativaId { get; set; }
        public string UnidadAdministrativa { get; set; }
        public string ProyectoId { get; set; }
        public string Proyecto { get; set; }
        public string TipoGastoId { get; set; }
        public string TipoGasto { get; set; }
        public string UM { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal CantidadSolicitada { get; set; }
        public decimal CantidadSurtida { get; set; }
        public Nullable<decimal> Existencia { get; set; }
        public Nullable<decimal> ExistenciaMinima { get; set; }
        public Nullable<double> CantidadComprar { get; set; }
        public string Comentarios { get; set; }
        public string FuenteFinanciamientoId { get; set; }
        public Nullable<int> ProveedorId { get; set; }
        public int EstatusId { get; set; }
        public string Estatus { get; set; }
        public Nullable<bool> Revision { get; set; }
        public Nullable<bool> Comprar { get; set; }
        public Nullable<bool> Rechazar { get; set; }
        public string Motivo { get; set; }
        public int RequisicionMaterialId { get; set; }
        public byte[] RequisicionTimestamp { get; set; }
        public byte[] DetalleTimestamp { get; set; }

        //Propiedades de la Orden de Compra Detalle
        public int OrdenCompraDetId { get; set; }
        public int OrdenCompraId { get; set; }
        public string TarifaImpuestoId { get; set; }
        public string ProductoId { get; set; }
        public int CuentaPresupuestalEgrId { get; set; }
        public string Descripcion { get; set; }
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

        //Propiedades de la Invitación de Compra Detalle
        public int InvitacionArticuloDetalleId { get; set; }
        public int InvitacionArticuloId { get; set; }
        
        public static explicit operator tblOrdenCompraDet(RequisicionOrdenCompraDetalleItem obj)
        {
            tblOrdenCompraDet modelo = new tblOrdenCompraDet();

            modelo.OrdenCompraDetId = obj.OrdenCompraDetId;
            modelo.OrdenCompraId = obj.OrdenCompraId;
            modelo.TarifaImpuestoId = obj.TarifaImpuestoId;
            modelo.ProductoId = obj.ProductoId;
            modelo.CuentaPresupuestalEgrId = obj.CuentaPresupuestalEgrId;
            modelo.Descripcion = obj.Descripcion;
            modelo.Status = "A";
            modelo.Cantidad = obj.CantidadComprar.GetValueOrDefault();
            modelo.Costo = obj.CostoUnitario;
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

            return modelo;
        }

        public static explicit operator ARtblInvitacionArticuloDetalle(RequisicionOrdenCompraDetalleItem obj)
        {
            ARtblInvitacionArticuloDetalle modelo = new ARtblInvitacionArticuloDetalle();

            modelo.InvitacionArticuloDetalleId = obj.InvitacionArticuloDetalleId;
            modelo.InvitacionArticuloId = obj.InvitacionArticuloId;
            modelo.RequisicionMaterialDetalleId = obj.RequisicionMaterialDetalleId;
            modelo.ProductoId = obj.ProductoId;
            modelo.CuentaPresupuestalEgrId = obj.CuentaPresupuestalEgrId;
            modelo.TarifaImpuestoId = obj.TarifaImpuestoId;
            modelo.Descripcion = obj.Descripcion;
            modelo.Cantidad = obj.CantidadComprar.GetValueOrDefault();
            modelo.Costo = obj.CostoUnitario;
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
            modelo.EstatusId = ControlMaestroMapeo.AREstatusInvitacionArticuloDetalle.POR_INVITAR;
            modelo.CreadoPorId = SessionHelper.GetUsuario().UsuarioId;

            return modelo;
        }
    }
}