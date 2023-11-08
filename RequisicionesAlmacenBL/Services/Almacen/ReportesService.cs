using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.Almacen
{
    public class ReportesService
    {
        public List<ARrptKardex> GetRptKardex(Nullable<DateTime> fechaInicio,
                                         Nullable<DateTime> fechaFin,
                                         Nullable<int> tipoMvtoId,
                                         Nullable<int> mvtoId,
                                         Nullable<int> polizaId,
                                         string almacenId,
                                         string productoId,
                                         string unidadAdministrativaId,
                                         string proyectoId,
                                         string fuenteFinanciamientoId,
                                         int usuarioId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Creamos los parámetros para la función
                SqlParameter pFechaInicio = new SqlParameter("fechaInicio", fechaInicio);
                SqlParameter pFechaFin = new SqlParameter("fechaFin", fechaFin);
                SqlParameter pTipoMvtoId = new SqlParameter("tipoMvtoId", tipoMvtoId);
                SqlParameter pMvtoId = new SqlParameter("mvtoId", mvtoId);
                SqlParameter pPolizaId = new SqlParameter("polizaId", polizaId);
                SqlParameter pAlmacenId = new SqlParameter("almacenId", almacenId);
                SqlParameter pProductoId = new SqlParameter("productoId", productoId);
                SqlParameter pUnidadAdministrativaId = new SqlParameter("unidadAdministrativaId", unidadAdministrativaId);
                SqlParameter pProyectoId = new SqlParameter("proyectoId", proyectoId);
                SqlParameter pFuenteFinanciamientoId = new SqlParameter("fuenteFinanciamientoId", fuenteFinanciamientoId);
                SqlParameter pUsuarioId = new SqlParameter("usuarioId", usuarioId);

                //Marcamos los parámetros como Nullables
                pFechaInicio.Value = fechaInicio != null ? pFechaInicio.Value : DBNull.Value;
                pFechaFin.Value = fechaFin != null ? pFechaFin.Value : DBNull.Value;
                pTipoMvtoId.Value = tipoMvtoId != null ? pTipoMvtoId.Value : DBNull.Value;
                pMvtoId.Value = mvtoId != null ? pMvtoId.Value : DBNull.Value;
                pPolizaId.Value = polizaId != null ? pPolizaId.Value : DBNull.Value;
                pAlmacenId.Value = almacenId != null ? pAlmacenId.Value : DBNull.Value;
                pProductoId.Value = productoId != null ? pProductoId.Value : DBNull.Value;
                pUnidadAdministrativaId.Value = unidadAdministrativaId != null ? pUnidadAdministrativaId.Value : DBNull.Value;
                pProyectoId.Value = proyectoId != null ? pProyectoId.Value : DBNull.Value;
                pFuenteFinanciamientoId.Value = fuenteFinanciamientoId != null ? pFuenteFinanciamientoId.Value : DBNull.Value;

                return Context.Database.SqlQuery<ARrptKardex>(
                    "SELECT * FROM ARfnRptKardex ( @fechaInicio, " +
                                                  "@fechaFin, " +
                                                  "@tipoMvtoId, " +
                                                  "@mvtoId, " +
                                                  "@polizaId, " +
                                                  "@almacenId, " +
                                                  "@productoId, " +
                                                  "@unidadAdministrativaId, " +
                                                  "@proyectoId, " +
                                                  "@fuenteFinanciamientoId, " +
                                                  "@usuarioId )",
                        pFechaInicio,
                        pFechaFin,
                        pTipoMvtoId,
                        pMvtoId,
                        pPolizaId,
                        pAlmacenId,
                        pProductoId,
                        pUnidadAdministrativaId,
                        pProyectoId,
                        pFuenteFinanciamientoId,
                        pUsuarioId
                ).ToList();
            }
        }
    }
}