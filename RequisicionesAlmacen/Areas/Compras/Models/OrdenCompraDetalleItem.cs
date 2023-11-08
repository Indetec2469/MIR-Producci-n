namespace RequisicionesAlmacenBL.Entities
{
    using RequisicionesAlmacen.Helpers;
    using RequisicionesAlmacenBL.Models.Mapeos;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(OrdenCompraDetalleItemMetaData))]
    public partial class OrdenCompraDetalleItem
    {
        //Propiedades del Item
        public string AlmacenProductoId { get; set; }
        public int UnidadDeMedidaId { get; set; }
        public string UnidadDeMedida { get; set; }
        public string UnidadAdministrativaId { get; set; }
        public string UnidadAdministrativa { get; set; }
        public string ProyectoId { get; set; }
        public string Proyecto { get; set; }
        public string FuenteFinanciamientoId { get; set; }
        public string FuenteFinanciamiento { get; set; }
        public string TipoGastoId { get; set; }
        public string TipoGasto { get; set; }
        public Nullable<double> CantidadRecibida { get; set; }
        public Nullable<double> CantidadPorRecibir { get; set; }
        public string Impuesto { get; set; }
        public Nullable<bool> PermiteEditar { get; set; }
        public string Estatus { get; set; }

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
        
        public static explicit operator tblOrdenCompraDet(OrdenCompraDetalleItem obj)
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

            return modelo;
        }
    }

    public class OrdenCompraDetalleItemMetaData
    {
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "Producto requerido")]
        public string AlmacenProductoId { get; set; }

        [Display(Name = "UM")]
        [Required(ErrorMessage = "UM requerida")]
        public int UnidadDeMedidaId { get; set; }
        
        [Display(Name = "UM")]
        [Required(ErrorMessage = "UM requerida")] 
        public string UnidadDeMedida { get; set; }

        [Display(Name = "Proyecto")]
        [Required(ErrorMessage = "Proyecto requerido")]
        public string ProyectoId { get; set; }

        [Display(Name = "Proyecto")]
        [Required(ErrorMessage = "Proyecto requerido")]
        public string Proyecto { get; set; }

        [Display(Name = "Fuente de Financiamiento")]
        [Required(ErrorMessage = "Fuente de Financiamiento requerida")]
        public string FuenteFinanciamientoId { get; set; }

        [Display(Name = "Fuente de Financiamiento")]
        [Required(ErrorMessage = "Fuente de Financiamiento requerida")]
        public string FuenteFinanciamiento { get; set; }

        [Display(Name = "Tipo de Gasto")]
        [Required(ErrorMessage = "Tipo de Gasto requerido")]
        public string TipoGastoId { get; set; }

        [Display(Name = "Tipo de Gasto")]
        [Required(ErrorMessage = "Tipo de Gasto requerido")]
        public string TipoGasto { get; set; }

        [Display(Name = "Impuesto")]
        [Required(ErrorMessage = "Impuesto requerido")]
        public string TarifaImpuestoId { get; set; }
        
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "Producto requerido")]
        public string Descripcion { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Cantidad requerida")]
        public double Cantidad { get; set; }

        [Display(Name = "Costo Unitario")]
        [Required(ErrorMessage = "Costo Unitario requerido")]
        public decimal Costo { get; set; }

        [Display(Name = "IEPS")]
        [Required(ErrorMessage = "IEPS requerido")]
        public double IEPS { get; set; }
    }
}