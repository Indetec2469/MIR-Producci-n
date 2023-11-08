namespace RequisicionesAlmacenBL.Entities
{
    using RequisicionesAlmacen.Helpers;
    using RequisicionesAlmacenBL.Models.Mapeos;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(OrdenCompraReciboDetalleItemMetaData))]
    public partial class OrdenCompraReciboDetalleItem
    {
        //Propiedades del Item
        public string UM { get; set; }
        public string Impuesto { get; set; }
        public int OrdenCompraId { get; set; }
        public double CantidadSolicitada { get; set; }
        public double CantidadRecibida { get; set; }
        public double CantidadPorRecibir { get; set; }
        public int AlmacenProductoId { get; set; }
        public Nullable<int> RequisicionMaterialId { get; set; }
        public byte[] RequisicionTimestamp { get; set; }
        public string Solicitud { get; set; }
        public Nullable<int> RequisicionMaterialDetalleId { get; set; }
        public byte[] DetalleTimestamp { get; set; }
        public string UnidadAdministrativaId { get; set; }
        public string UnidadAdministrativa { get; set; }
        public string ProyectoId { get; set; }
        public string Proyecto { get; set; }
        public string FuenteFinanciamientoId { get; set; }
        public string FuenteFinanciamiento { get; set; }
        public string TipoGastoId { get; set; }
        public string TipoGasto { get; set; }

        //Propiedades de la Compra Detalle
        public Nullable<int> CompraDetId { get; set; }
        public Nullable<int> CompraId { get; set; }
        public string ProductoId { get; set; }
        public string TarifaImpuestoId { get; set; }
        public int RefOrdenCompraDetId { get; set; }
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

        public static OrdenCompraReciboDetalleItem ConvertTo(ARspConsultaOCPorRecibirDetalles_Result obj)
        {
            OrdenCompraReciboDetalleItem modelo = new OrdenCompraReciboDetalleItem();

            modelo.UM = obj.UM;
            modelo.Impuesto = obj.Impuesto;
            modelo.OrdenCompraId = obj.OrdenCompraId;
            modelo.CantidadSolicitada = obj.CantidadSolicitada;
            modelo.CantidadRecibida = obj.CantidadRecibida;
            modelo.CantidadPorRecibir = obj.CantidadPorRecibir;
            modelo.AlmacenProductoId = obj.AlmacenProductoId;
            modelo.RequisicionMaterialId = obj.RequisicionMaterialId;
            modelo.RequisicionTimestamp = obj.RequisicionTimestamp;
            modelo.Solicitud = obj.Solicitud;
            modelo.RequisicionMaterialDetalleId = obj.RequisicionMaterialDetalleId;
            modelo.DetalleTimestamp = obj.DetalleTimestamp;

            modelo.UnidadAdministrativaId = obj.UnidadAdministrativaId;
            modelo.UnidadAdministrativa = obj.UnidadAdministrativa;
            modelo.ProyectoId = obj.ProyectoId;
            modelo.Proyecto = obj.Proyecto;
            modelo.FuenteFinanciamientoId = obj.FuenteFinanciamientoId;
            modelo.FuenteFinanciamiento = obj.FuenteFinanciamiento;
            modelo.TipoGastoId = obj.TipoGastoId;
            modelo.TipoGasto = obj.TipoGasto;

            modelo.CompraDetId = obj.CompraDetId;
            modelo.CompraId = obj.CompraId;
            modelo.ProductoId = obj.ProductoId;
            modelo.TarifaImpuestoId = obj.TarifaImpuestoId;
            modelo.RefOrdenCompraDetId = obj.RefOrdenCompraDetId;
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

            return modelo;
        }

        public static OrdenCompraReciboDetalleItem ConvertTo(ARspConsultaOrdenCompraReciboDetalles_Result obj)
        {
            OrdenCompraReciboDetalleItem modelo = new OrdenCompraReciboDetalleItem();

            modelo.UM = obj.UM;
            modelo.Impuesto = obj.Impuesto;
            modelo.OrdenCompraId = obj.OrdenCompraId;
            modelo.CantidadSolicitada = obj.CantidadSolicitada;
            modelo.CantidadRecibida = obj.CantidadRecibida;
            modelo.CantidadPorRecibir = obj.CantidadPorRecibir;
            modelo.AlmacenProductoId = obj.AlmacenProductoId;
            modelo.RequisicionMaterialId = obj.RequisicionMaterialId;
            modelo.RequisicionTimestamp = obj.RequisicionTimestamp;
            modelo.Solicitud = obj.Solicitud;
            modelo.RequisicionMaterialDetalleId = obj.RequisicionMaterialDetalleId;
            modelo.DetalleTimestamp = obj.DetalleTimestamp;

            modelo.UnidadAdministrativaId = obj.UnidadAdministrativaId;
            modelo.UnidadAdministrativa = obj.UnidadAdministrativa;
            modelo.ProyectoId = obj.ProyectoId;
            modelo.Proyecto = obj.Proyecto;
            modelo.FuenteFinanciamientoId = obj.FuenteFinanciamientoId;
            modelo.FuenteFinanciamiento = obj.FuenteFinanciamiento;
            modelo.TipoGastoId = obj.TipoGastoId;
            modelo.TipoGasto = obj.TipoGasto;

            modelo.CompraDetId = obj.CompraDetId;
            modelo.CompraId = obj.CompraId;
            modelo.ProductoId = obj.ProductoId;
            modelo.TarifaImpuestoId = obj.TarifaImpuestoId;
            modelo.RefOrdenCompraDetId = obj.RefOrdenCompraDetId.GetValueOrDefault();
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

            return modelo;
        }

        public static tblCompraDet ConvertTo(OrdenCompraReciboDetalleItem obj)
        {
            tblCompraDet modelo = new tblCompraDet();

            modelo.AlmacenProductoId = obj.AlmacenProductoId;

            modelo.CompraDetId = obj.CompraDetId.GetValueOrDefault();
            modelo.CompraId = obj.CompraId.GetValueOrDefault();
            modelo.ProductoId = obj.ProductoId;
            modelo.TarifaImpuestoId = obj.TarifaImpuestoId;
            modelo.RefOrdenCompraDetId = obj.RefOrdenCompraDetId;
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

            return modelo;
        }
    }

    public class OrdenCompraReciboDetalleItemMetaData
    {
        [Display(Name = "UM")]
        [Required(ErrorMessage = "UM requerida")] 
        public string UM { get; set; }

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
        
        [Display(Name = "Unidad Administrativa")]
        public string UnidadAdministrativaId { get; set; }

        [Display(Name = "Unidad Administrativa")] 
        public string UnidadAdministrativa { get; set; }

        [Display(Name = "Proyecto")] 
        public string ProyectoId { get; set; }

        [Display(Name = "Proyecto")] 
        public string Proyecto { get; set; }

        [Display(Name = "Fuente de Financiamiento")] 
        public string FuenteFinanciamientoId { get; set; }

        [Display(Name = "Fuente de Financiamiento")] 
        public string FuenteFinanciamiento { get; set; }

        [Display(Name = "Tipo de Gasto")] 
        public string TipoGastoId { get; set; }

        [Display(Name = "Tipo de Gasto")] 
        public string TipoGasto { get; set; }
    }
}