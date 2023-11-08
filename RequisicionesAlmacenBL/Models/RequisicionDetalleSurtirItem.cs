namespace RequisicionesAlmacenBL.Entities
{
    using System;

    public partial class RequisicionDetalleSurtirItem
    {
        public int AlmacenProductoId { get; set; }
        public int RequisicionMaterialDetalleId { get; set; }
        public decimal CantidadSurtir { get; set; }
        public decimal CantidadPorSurtir { get; set; }
    }
}