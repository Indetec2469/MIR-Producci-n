using EntityFrameworkExtras.EF6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{   
    [UserDefinedTableType("ARudtInventarioMovimiento")]
    public class ARudtInventarioMovimiento
    {
        [UserDefinedTableTypeColumn(1)]
        public int AlmacenProductoId { get; set; }

        [UserDefinedTableTypeColumn(2)]
        public decimal CantidadMovimiento { get; set; }

        [UserDefinedTableTypeColumn(3)]
        public Nullable<decimal> CostoUnitario { get; set; }

        [UserDefinedTableTypeColumn(4)]
        public int ReferenciaMovtoId { get; set; }
    }
}