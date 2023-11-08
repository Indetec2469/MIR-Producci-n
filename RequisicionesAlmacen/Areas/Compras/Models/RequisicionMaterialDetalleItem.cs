namespace RequisicionesAlmacenBL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(RequisicionMaterialDetalleItemMetaData))]
    public partial class RequisicionMaterialDetalleItem
    {
        public string ProductoDetalleId { get; set; }
        public string ProductoId { get; set; }
        public string Descripcion { get; set; }
        public string Producto { get; set; }
        public int UnidadDeMedidaId { get; set; }
        public string UnidadDeMedida { get; set; }
        public decimal CostoUnitario { get; set; }
        public string UnidadAdministrativaId { get; set; }
        public string UnidadAdministrativa { get; set; }
        public string ProyectoId { get; set; }
        public string Proyecto { get; set; }
        public string TipoGastoId { get; set; }
        public string TipoGasto { get; set; }

        public int RequisicionMaterialDetalleId { get; set; }
        public int RequisicionMaterialId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal TotalPartida { get; set; }
        public string Comentarios { get; set; }
        public int EstatusId { get; set; }
        public string Estatus { get; set; }
        public string MotivoRechazo { get; set; }

        public static RequisicionMaterialDetalleItem ConvertTo(ARspConsultaRequisicionMaterialDetalles_Result obj)
        {
            RequisicionMaterialDetalleItem modelo = new RequisicionMaterialDetalleItem();

            modelo.ProductoDetalleId = obj.ProductoDetalleId;
            modelo.ProductoId = obj.ProductoId;
            modelo.Descripcion = obj.Descripcion;
            modelo.Producto = obj.Producto;
            modelo.UnidadDeMedidaId = obj.UnidadDeMedidaId;
            modelo.UnidadDeMedida = obj.UnidadDeMedida;
            modelo.CostoUnitario = obj.CostoUnitario;
            modelo.UnidadAdministrativaId = obj.UnidadAdministrativaId;
            modelo.UnidadAdministrativa = obj.UnidadAdministrativa;
            modelo.ProyectoId = obj.ProyectoId;
            modelo.Proyecto = obj.Proyecto;
            modelo.TipoGastoId = obj.TipoGastoId;
            modelo.TipoGasto = obj.TipoGasto;
            modelo.RequisicionMaterialDetalleId = obj.RequisicionMaterialDetalleId;
            modelo.RequisicionMaterialId = obj.RequisicionMaterialId;
            modelo.Cantidad = obj.Cantidad;
            modelo.TotalPartida = obj.TotalPartida;
            modelo.Comentarios = obj.Comentarios;
            modelo.EstatusId = obj.EstatusId;
            modelo.Estatus = obj.Estatus;
            modelo.MotivoRechazo = obj.MotivoRechazo;

            return modelo;
        }

        public static explicit operator ARtblRequisicionMaterialDetalle(RequisicionMaterialDetalleItem obj)
        {
            ARtblRequisicionMaterialDetalle modelo = new ARtblRequisicionMaterialDetalle();

            modelo.RequisicionMaterialDetalleId = obj.RequisicionMaterialDetalleId;
            modelo.RequisicionMaterialId = obj.RequisicionMaterialId;
            modelo.UnidadAdministrativaId = obj.UnidadAdministrativaId;
            modelo.ProyectoId = obj.ProyectoId;
            modelo.TipoGastoId = obj.TipoGastoId;
            modelo.ProductoId = obj.ProductoId;
            modelo.Descripcion = obj.Descripcion;
            modelo.UnidadMedidaId = obj.UnidadDeMedidaId;
            modelo.CostoUnitario = obj.CostoUnitario;
            modelo.Cantidad = obj.Cantidad;
            modelo.TotalPartida = obj.TotalPartida;
            modelo.Comentarios = obj.Comentarios;
            modelo.EstatusId = obj.EstatusId;
            modelo.MotivoRechazo = obj.MotivoRechazo;

            return modelo;
        }
    }

    public class RequisicionMaterialDetalleItemMetaData
    {
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

        [Display(Name = "Tipo de Gasto")]
        [Required(ErrorMessage = "Tipo de Gasto requerido")]
        public string TipoGastoId { get; set; }

        [Display(Name = "Tipo de Gasto")]
        [Required(ErrorMessage = "Tipo de Gasto requerido")]
        public string TipoGasto { get; set; }

        [Display(Name = "Artículo")]
        public string Producto { get; set; }

        [Display(Name = "Artículo")]
        [Required(ErrorMessage = "Artículo requerido")]
        public string ProductoDetalleId { get; set; }

        [Display(Name = "UM")]
        [Required(ErrorMessage = "UM requerida")]
        public int UnidadDeMedidaId { get; set; }

        [Display(Name = "UM")]
        [Required(ErrorMessage = "UM requerida")]
        public int UnidadDeMedida { get; set; }

        [Display(Name = "Costo Unitario")]
        [Required(ErrorMessage = "Costo Unitario requerido")]
        public decimal CostoUnitario { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Cantidad requerida")]
        public decimal Cantidad { get; set; }

        [Display(Name = "Total")]
        [Required(ErrorMessage = "Total requerido")]
        public decimal TotalPartida { get; set; }

        [StringLength(1000)]
        [Display(Name = "Comentarios")]
        public string Comentarios { get; set; }
    }
}