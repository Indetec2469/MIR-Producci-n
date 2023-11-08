namespace RequisicionesAlmacenBL.Entities
{
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(CortesiaDetalleItemMetaData))]
    public partial class CortesiaDetalleItem
    {
        //Propiedades de la Cortesía Detalle
        public int CortesiaDetalleId { get; set; }
        public int CortesiaId { get; set; }
        public int NumeroPartida { get; set; }
        public string ProductoId { get; set; }
        public string Descripcion { get; set; }
        public int CuentaPresupuestalEgrId { get; set; }
        public int UnidadMedidaId { get; set; }
        public double Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        public double TotalPartida { get; set; }

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

        public static explicit operator ARtblCortesiaDetalle(CortesiaDetalleItem obj)
        {
            ARtblCortesiaDetalle modelo = new ARtblCortesiaDetalle();

            modelo.CortesiaDetalleId = obj.CortesiaDetalleId;
            modelo.CortesiaId = obj.CortesiaId;
            modelo.NumeroPartida = obj.NumeroPartida;
            modelo.ProductoId = obj.ProductoId;
            modelo.Descripcion = obj.Descripcion;
            modelo.CuentaPresupuestalEgrId = obj.CuentaPresupuestalEgrId;
            modelo.UnidadMedidaId = obj.UnidadDeMedidaId;
            modelo.Cantidad = obj.Cantidad;
            modelo.PrecioUnitario = obj.PrecioUnitario;
            modelo.TotalPartida = obj.TotalPartida;

            return modelo;
        }
    }

    public class CortesiaDetalleItemMetaData
    {
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "Producto requerido")]
        public string ProductoId { get; set; }

        [Display(Name = "Cuenta Presupuestal")]
        [Required(ErrorMessage = "Cuenta Presupuestal requerida")]
        public string AlmacenProductoId { get; set; }

        [Display(Name = "UM")]
        [Required(ErrorMessage = "UM requerida")]
        public int UnidadDeMedidaId { get; set; }
        
        [Display(Name = "UM")]
        [Required(ErrorMessage = "UM requerida")] 
        public string UnidadDeMedida { get; set; }

        [Display(Name = "Unidad Administrativa")]
        [Required(ErrorMessage = "Unidad Administrativa requerida")]
        public string UnidadAdministrativaId { get; set; }

        [Display(Name = "Unidad Administrativa")]
        [Required(ErrorMessage = "Unidad Administrativa requerida")]
        public string UnidadAdministrativa { get; set; }

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
        
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "Producto requerido")]
        public string Descripcion { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Cantidad requerida")]
        public double Cantidad { get; set; }

        [Display(Name = "Costo Unitario")]
        [Required(ErrorMessage = "Costo Unitario requerido")]
        public double PrecioUnitario { get; set; }
    }
}