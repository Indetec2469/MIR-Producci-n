using EntityFrameworkExtras.EF6;
using System.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [StoredProcedure("ARspCargaInventarioInicial")]
    public class ARspCargaInventarioInicial
    {
        [StoredProcedureParameter(SqlDbType.Udt, ParameterName = "ImportarAlmacenProducto")]
        public List<ARudtImportarAlmacenProducto> ImportarAlmacenProducto { get; set; }

        [StoredProcedureParameter(SqlDbType.Int, ParameterName = "UsuarioId")]
        public int UsuarioId { get; set; }
    }
}