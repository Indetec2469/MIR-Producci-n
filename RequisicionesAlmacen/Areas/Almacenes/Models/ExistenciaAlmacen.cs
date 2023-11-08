namespace RequisicionesAlmacenBL.Entities
{
    using System;

    public partial class ExistenciaAlmacen
    {
        public int InventarioFisicoDetalleId { get; set; }
        public int AlmacenProductoId { get; set; }
        public string ProductoId { get; set; }
        public string Producto { get; set; }
        public int UnidadDeMedidaId { get; set; }
        public string UnidadDeMedida { get; set; }
        public int CuentaPresupuestalId { get; set; }
        public string ProyectoId { get; set; }
        public string Proyecto { get; set; }
        public string FuenteFinanciamientoId { get; set; }
        public string FuenteFinanciamiento { get; set; }
        public string UnidadAdministrativaId { get; set; }
        public string UnidadAdministrativa { get; set; }
        public string TipoGastoId { get; set; }
        public string TipoGasto { get; set; }
        public decimal Existencia { get; set; }
        public Nullable<decimal> Conteo { get; set; }
        public decimal CostoPromedio { get; set; }
        public string MotivoAjuste { get; set; }
        public Nullable<decimal> CantidadAjuste { get; set; }
        public Nullable<decimal> CostoMovimiento { get; set; }
        public Nullable<bool> Borrado { get; set; }

        public static ExistenciaAlmacen ConvertTo(ARspConsultaExistenciaAlmacen_Result obj)
        {
            ExistenciaAlmacen detalle = new ExistenciaAlmacen();

            detalle.InventarioFisicoDetalleId = obj.InventarioFisicoDetalleId;
            detalle.AlmacenProductoId = obj.AlmacenProductoId;
            detalle.ProductoId = obj.ProductoId;
            detalle.Producto = obj.Producto;
            detalle.UnidadDeMedidaId = obj.UnidadDeMedidaId;
            detalle.UnidadDeMedida = obj.UnidadDeMedida;
            detalle.CuentaPresupuestalId = obj.CuentaPresupuestalId;
            detalle.ProyectoId = obj.ProyectoId;
            detalle.Proyecto = obj.Proyecto;
            detalle.FuenteFinanciamientoId = obj.FuenteFinanciamientoId;
            detalle.FuenteFinanciamiento = obj.FuenteFinanciamiento;
            detalle.UnidadAdministrativaId = obj.UnidadAdministrativaId;
            detalle.UnidadAdministrativa = obj.UnidadAdministrativa;
            detalle.TipoGastoId = obj.TipoGastoId;
            detalle.TipoGasto = obj.TipoGasto;
            detalle.Existencia = obj.Existencia;
            detalle.Conteo = obj.Conteo;
            detalle.CostoPromedio = obj.CostoPromedio;
            detalle.MotivoAjuste = obj.MotivoAjuste;

            return detalle;
        }

        public static ExistenciaAlmacen ConvertTo(ARspConsultaInventarioFisicoDetalles_Result obj)
        {
            ExistenciaAlmacen detalle = new ExistenciaAlmacen();

            detalle.InventarioFisicoDetalleId = obj.InventarioFisicoDetalleId;
            detalle.AlmacenProductoId = obj.AlmacenProductoId;
            detalle.ProductoId = obj.ProductoId;
            detalle.Producto = obj.Producto;
            detalle.UnidadDeMedidaId = obj.UnidadDeMedidaId;
            detalle.UnidadDeMedida = obj.UnidadDeMedida;
            detalle.CuentaPresupuestalId = obj.CuentaPresupuestalId;
            detalle.ProyectoId = obj.ProyectoId;
            detalle.Proyecto = obj.Proyecto;
            detalle.FuenteFinanciamientoId = obj.FuenteFinanciamientoId;
            detalle.FuenteFinanciamiento = obj.FuenteFinanciamiento;
            detalle.UnidadAdministrativaId = obj.UnidadAdministrativaId;
            detalle.UnidadAdministrativa = obj.UnidadAdministrativa;
            detalle.TipoGastoId = obj.TipoGastoId;
            detalle.TipoGasto = obj.TipoGasto;
            detalle.Existencia = obj.Existencia;
            detalle.Conteo = obj.Conteo;
            detalle.CantidadAjuste = obj.CantidadAjuste;
            detalle.CostoMovimiento = obj.CostoMovimiento;
            detalle.CostoPromedio = obj.CostoPromedio;
            detalle.MotivoAjuste = obj.MotivoAjuste;

            return detalle;
        }

        public static explicit operator ARtblInventarioFisicoDetalle(ExistenciaAlmacen obj)
        {
            ARtblInventarioFisicoDetalle detalle = new ARtblInventarioFisicoDetalle();

            detalle.InventarioFisicoDetalleId = obj.InventarioFisicoDetalleId;
            detalle.AlmacenProductoId = obj.AlmacenProductoId;
            detalle.ProductoId = obj.ProductoId;
            detalle.Producto = obj.Producto;
            detalle.UnidadDeMedidaId = obj.UnidadDeMedidaId;
            detalle.UnidadDeMedida = obj.UnidadDeMedida;
            detalle.CostoPromedio = obj.CostoPromedio;
            detalle.Existencia = obj.Existencia;
            detalle.Conteo = obj.Conteo;
            detalle.MotivoAjuste = obj.MotivoAjuste;
            detalle.ProyectoId = obj.ProyectoId;
            detalle.Proyecto = obj.Proyecto;
            detalle.FuenteFinanciamientoId = obj.FuenteFinanciamientoId;
            detalle.FuenteFinanciamiento = obj.FuenteFinanciamiento;
            detalle.UnidadAdministrativaId = obj.UnidadAdministrativaId;
            detalle.UnidadAdministrativa = obj.UnidadAdministrativa;
            detalle.TipoGastoId = obj.TipoGastoId;
            detalle.TipoGasto = obj.TipoGasto;
            detalle.Borrado = obj.Borrado.GetValueOrDefault();

            return detalle;
        }
    }
}