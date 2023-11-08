using EntityFrameworkExtras.EF6;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;

namespace RequisicionesAlmacenBL.Services
{
    public class ProcesadorInventariosService
    {
        public int Execute(int tipoMovimientoId,
                            string motivoMovto,
                            int creadoPorId,
                            bool insertarAgrupador,
                            int referenciaMovtoId,
                            Nullable<int> polizaId,
                            string numeroOficio,
                            string observaciones,
                            List<ARudtInventarioMovimiento> movimientos, 
                            SAACGContext context)
        {
                var procedure = new ARspProcesadorInventarios()
                {
                    TipoMovimientoId = tipoMovimientoId,
                    MotivoMovto = motivoMovto,
                    CreadoPorId = creadoPorId,
                    InsertarAgrupador = insertarAgrupador,
                    ReferenciaMovtoId = referenciaMovtoId,
                    PolizaId = polizaId,
                    NumeroOficio = numeroOficio,
                    Observaciones = observaciones,
                    Movimientos = movimientos
                };

                return context.Database.ExecuteStoredProcedureFirstOrDefault<int>(procedure);
        }
    }
}