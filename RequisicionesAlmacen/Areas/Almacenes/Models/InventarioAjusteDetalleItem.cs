namespace RequisicionesAlmacenBL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    [MetadataType(typeof(InventarioAjusteDetalleItemMetaData))]
    public partial class InventarioAjusteDetalleItem
    {
        public int InventarioAjusteDetalleId { get; set; }
        public int InventarioAjusteId { get; set; }
        public string AlmacenProductoId { get; set; }
        public string AlmacenId { get; set; }
        public string Almacen { get; set; }        
        public string FuenteFinanciamientoId { get; set; }
        public string FuenteFinanciamiento { get; set; }
        public string ProyectoId { get; set; }
        public string Proyecto { get; set; }
        public string UnidadAdministrativaId { get; set; }
        public string UnidadAdministrativa { get; set; }
        public string TipoGastoId { get; set; }
        public string TipoGasto { get; set; }
        public string ProductoId { get; set; }
        public string Descripcion { get; set; }
        public string Producto { get; set; }
        public int UnidadDeMedidaId { get; set; }
        public string UnidadDeMedida { get; set; }
        public int CuentaPresupuestalId { get; set; }
        public Nullable<decimal> CostoPromedio { get; set; }
        public int TipoMovimientoId { get; set; }
        public string TipoMovimiento { get; set; }       
        public int ConceptoAjusteId { get; set; }
        public string ConceptoAjuste { get; set; }
        public decimal Cantidad { get; set; }
        public Nullable<decimal> ExistenciaActual { get; set; }
        public Nullable<decimal> ExistenciaFinal { get; set; }
        public decimal CostoMovimiento { get; set; }
        public string Comentarios { get; set; }
        public string ArchivoId { get; set; }
        public string NombreArchivoTmp { get; set; }

        public static InventarioAjusteDetalleItem ConvertTo(ARspConsultaInventarioAjusteDetalles_Result obj)
        {
            InventarioAjusteDetalleItem detalle = new InventarioAjusteDetalleItem();

            detalle.InventarioAjusteDetalleId = obj.InventarioAjusteDetalleId;
            detalle.InventarioAjusteId = obj.InventarioAjusteId;
            detalle.AlmacenId = obj.AlmacenId;
            detalle.Almacen = obj.Almacen;
            detalle.FuenteFinanciamientoId = obj.FuenteFinanciamientoId;
            detalle.FuenteFinanciamiento = obj.FuenteFinanciamiento;
            detalle.ProyectoId = obj.ProyectoId;
            detalle.Proyecto = obj.Proyecto;
            detalle.UnidadAdministrativaId = obj.UnidadAdministrativaId;
            detalle.UnidadAdministrativa = obj.UnidadAdministrativa;
            detalle.TipoGastoId = obj.TipoGastoId;
            detalle.TipoGasto = obj.TipoGasto;
            detalle.ProductoId = obj.ProductoId;
            detalle.Descripcion = obj.Descripcion;
            detalle.Producto = obj.Producto;
            detalle.UnidadDeMedidaId = obj.UnidadDeMedidaId;
            detalle.UnidadDeMedida = obj.UnidadDeMedida;
            detalle.CuentaPresupuestalId = obj.CuentaPresupuestalId;
            detalle.CostoPromedio = obj.CostoPromedio;
            detalle.TipoMovimientoId = obj.TipoMovimientoId;
            detalle.TipoMovimiento = obj.TipoMovimiento;
            detalle.ConceptoAjusteId = obj.ConceptoAjusteId;
            detalle.ConceptoAjuste = obj.ConceptoAjuste;
            detalle.Cantidad = obj.Cantidad;
            detalle.ExistenciaActual = obj.ExistenciaActual;
            detalle.ExistenciaFinal = obj.ExistenciaFinal;
            detalle.CostoMovimiento = obj.CostoMovimiento.GetValueOrDefault();
            detalle.Comentarios = obj.Comentarios;
            detalle.ArchivoId = obj.ArchivoId.ToString();

            return detalle;
        }

        public static explicit operator ARtblInventarioAjusteDetalle(InventarioAjusteDetalleItem obj)
        {
            ARtblInventarioAjusteDetalle detalle = new ARtblInventarioAjusteDetalle();

            detalle.InventarioAjusteDetalleId = obj.InventarioAjusteDetalleId;
            detalle.InventarioAjusteId = obj.InventarioAjusteId;
            detalle.AlmacenId = obj.AlmacenId;
            detalle.FuenteFinanciamientoId = obj.FuenteFinanciamientoId;
            detalle.ProyectoId = obj.ProyectoId;
            detalle.UnidadAdministrativaId = obj.UnidadAdministrativaId;
            detalle.TipoGastoId = obj.TipoGastoId;
            detalle.ProductoId = obj.ProductoId;
            detalle.Descripcion = obj.Descripcion;
            detalle.UnidadMedidaId = obj.UnidadDeMedidaId;
            detalle.CuentaPresupuestalId = obj.CuentaPresupuestalId;
            detalle.CostoUnitario = obj.CostoPromedio.GetValueOrDefault();
            detalle.TipoMovimientoId = obj.TipoMovimientoId;
            detalle.ConceptoAjusteId = obj.ConceptoAjusteId;
            detalle.Cantidad = obj.Cantidad;
            detalle.ExistenciaActual = obj.ExistenciaActual.GetValueOrDefault();
            detalle.ExistenciaFinal = obj.ExistenciaFinal.GetValueOrDefault();
            detalle.Comentarios = obj.Comentarios;
            detalle.NombreArchivoTmp = obj.NombreArchivoTmp;

            return detalle;
        }
    }

    public class InventarioAjusteDetalleItemMetaData
    {
        [Display(Name = "Almacén")]
        [Required(ErrorMessage = "Almacén requerido")]
        public string AlmacenId { get; set; }

        [Display(Name = "Fuente de Financiamiento")]
        [Required(ErrorMessage = "Fuente de Financiamiento requerida")]
        public string FuenteFinanciamientoId { get; set; }

        [Display(Name = "Proyecto")]
        [Required(ErrorMessage = "ProyectoId requerido")]
        public string ProyectoId { get; set; }

        [Display(Name = "UnidadAdministrativaId")]
        [Required(ErrorMessage = "Unidad Administrativa requerida")]
        public string UnidadAdministrativaId { get; set; }

        [Display(Name = "Tipo de Gasto")]
        [Required(ErrorMessage = "Tipo de Gasto requerido")]
        public string TipoGastoId { get; set; }

        [Display(Name = "Tipo de Movimiento")]
        [Required(ErrorMessage = "Tipo de Movimiento requerido")]
        public int TipoMovimientoId { get; set; }

        [Display(Name = "Artículo")]
        [Required(ErrorMessage = "Artículo requerido")]
        public string AlmacenProductoId { get; set; }

        [Display(Name = "Artículo")]
        [Required(ErrorMessage = "Artículo requerido")]
        public string ProductoId { get; set; }

        [Display(Name = "Concepto de Ajuste")]
        [Required(ErrorMessage = "Concepto de Ajuste requerido")]
        public int ConceptoAjusteId { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Cantidad requerida")]
        public float Cantidad { get; set; }

        [Display(Name = "Documento")]
        public string ArchivoId { get; set; }

        [Display(Name = "Comentarios")]
        public string Comentarios { get; set; }
    }
}