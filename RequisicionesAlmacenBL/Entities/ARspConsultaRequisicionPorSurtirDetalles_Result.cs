//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RequisicionesAlmacenBL.Entities
{
    using System;
    
    public partial class ARspConsultaRequisicionPorSurtirDetalles_Result
    {
        public int RequisicionMaterialDetalleId { get; set; }
        public int RequisicionMaterialId { get; set; }
        public string ProductoId { get; set; }
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
        public decimal Cantidad { get; set; }
        public Nullable<decimal> Existencia { get; set; }
        public Nullable<decimal> Surtida { get; set; }
        public Nullable<decimal> PorSurtir { get; set; }
        public Nullable<decimal> CantidadSurtir { get; set; }
        public string Comentarios { get; set; }
        public int EstatusId { get; set; }
        public string Estatus { get; set; }
        public Nullable<bool> Revision { get; set; }
        public Nullable<bool> PorComprar { get; set; }
        public string Motivo { get; set; }
    }
}
