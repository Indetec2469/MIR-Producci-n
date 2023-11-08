using EntityFrameworkExtras.EF6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{   
    [UserDefinedTableType("ARudtImportarAlmacenProducto")]
    public class ARudtImportarAlmacenProducto
    {
        [UserDefinedTableTypeColumn(1)] 
        public decimal Cantidad { get; set; }

        [UserDefinedTableTypeColumn(2)] 
        public string ProductoId { get; set; }

        [UserDefinedTableTypeColumn(3)] 
        public decimal CostoUnitario { get; set; }

        [UserDefinedTableTypeColumn(4)] 
        public string AlmacenId { get; set; }

        [UserDefinedTableTypeColumn(5)] 
        public string FuenteFinanciamientoId { get; set; }

        [UserDefinedTableTypeColumn(6)] 
        public string ProyectoId { get; set; }

        [UserDefinedTableTypeColumn(7)] 
        public string UnidadAdministrativaId { get; set; }

        [UserDefinedTableTypeColumn(8)] 
        public string TipoGastoId { get; set; }
    }
}