using EntityFrameworkExtras.EF6;
using System.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [StoredProcedure("ARspProcesadorInventarios")]
    public class ARspProcesadorInventarios
    {
        [StoredProcedureParameter(SqlDbType.Int, ParameterName = "TipoMovimientoId")]
        public int TipoMovimientoId { get; set; }

        [StoredProcedureParameter(SqlDbType.NVarChar, ParameterName = "MotivoMovto")]
        public string MotivoMovto { get; set; }

        [StoredProcedureParameter(SqlDbType.Int, ParameterName = "CreadoPorId")]
        public int CreadoPorId { get; set; }

        [StoredProcedureParameter(SqlDbType.Bit, ParameterName = "InsertarAgrupador")]
        public bool InsertarAgrupador { get; set; }

        [StoredProcedureParameter(SqlDbType.Int, ParameterName = "ReferenciaMovtoId")]
        public int ReferenciaMovtoId { get; set; }

        [StoredProcedureParameter(SqlDbType.Int, ParameterName = "PolizaId")]
        public Nullable<int> PolizaId { get; set; }

        [StoredProcedureParameter(SqlDbType.NVarChar, ParameterName = "NumeroOficio")]
        public string NumeroOficio { get; set; }

        [StoredProcedureParameter(SqlDbType.NVarChar, ParameterName = "Observaciones")]
        public string Observaciones { get; set; }

        [StoredProcedureParameter(SqlDbType.Udt, ParameterName = "Movimientos")]
        public List<ARudtInventarioMovimiento> Movimientos { get; set; }
    }
}