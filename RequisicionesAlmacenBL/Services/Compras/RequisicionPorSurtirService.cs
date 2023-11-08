using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services.SAACG;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacenBL.Services.Compras
{
    public class RequisicionPorSurtirService : BaseService<ARtblRequisicionMaterial>
    {
        public override ARtblRequisicionMaterial BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public List<ARtblRequisicionMaterial> BuscaTodos()
        {
            throw new NotImplementedException();
        }

        public List<ARvwListadoRequisicionPorSurtir> BuscaListado()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARvwListadoRequisicionPorSurtir.OrderByDescending(m => m.RequisicionMaterialId).ToList();
            }
        }

        public override ARtblRequisicionMaterial Inserta(ARtblRequisicionMaterial entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override bool Actualiza(ARtblRequisicionMaterial entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }
        
        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public List<ARspConsultaRequisicionPorSurtirDetalles_Result> BuscaDetallesPorRequisicionMaterialId(int requisicionId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaRequisicionPorSurtirDetalles(requisicionId).ToList();
            }
        }

        public List<ARspConsultaRequisicionPorSurtirExistencias_Result> BuscaExistenciaProducto(int requisicionId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaRequisicionPorSurtirExistencias(requisicionId).ToList();
            }
        }

        public Nullable<int> GuardaCambios(int requisicionId,
                                  string codigo,
                                  string numeroOficio,
                                  string observaciones,
                                  List<RequisicionDetalleSurtirItem> movimientos,
                                  List<ARtblRequisicionMaterialDetalle> detalles,
                                  int usuarioId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    int inventarioMovtoAgrupadorId = -1;

                    if (movimientos != null)
                    {
                        int tipoMovimientoId = TipoInventarioMovimiento.REQUISICION_MATERIAL_SURTIMIENTO;
                        string motivoMovto = "Surtimiento de Solicitud: " + codigo;
                        int creadoPorId = usuarioId;
                        int referenciaMovtoId = requisicionId;

                        List<ARudtInventarioMovimiento> movimientosUDT = new List<ARudtInventarioMovimiento>();

                        //Registramos los movimientos en el inventario
                        foreach (RequisicionDetalleSurtirItem movimiento in movimientos)
                        {
                            ARudtInventarioMovimiento udt = new ARudtInventarioMovimiento();
                            
                            udt.AlmacenProductoId = movimiento.AlmacenProductoId;
                            udt.CantidadMovimiento = -1 * movimiento.CantidadSurtir;
                            udt.ReferenciaMovtoId = movimiento.RequisicionMaterialDetalleId;

                            movimientosUDT.Add(udt);
                        }

                        inventarioMovtoAgrupadorId = new ProcesadorInventariosService().Execute(tipoMovimientoId,
                                                                   motivoMovto,
                                                                   creadoPorId,
                                                                   true,
                                                                   referenciaMovtoId,
                                                                   null,
                                                                   numeroOficio,
                                                                   observaciones,
                                                                   movimientosUDT,
                                                                   Context);

                        ARtblInventarioMovimientoAgrupador agrupador = new InventarioMovimientoAgrupadorService().BuscaPorId(inventarioMovtoAgrupadorId, Context);

                        //Actualizamos la póliza
                        new PolizaService().ActualizaPolizaSurtimiento(inventarioMovtoAgrupadorId, agrupador.FechaCreacion, Context);
                    }

                    //Actualizamos los detalles en Revisión o Por Comprar
                    if (detalles != null)
                    {
                        new RequisicionMaterialService().GuardaDetalles(requisicionId, detalles, Context);
                    }

                    //Actualizamos el estatus de la Requisición
                    Context.ARspActualizaEstatusRequisicionMaterial(requisicionId);

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    //Regresamos el Id del agrupador que se insertó
                    return inventarioMovtoAgrupadorId;
                }
                catch (DbEntityValidationException ex)
                {
                    //Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
                catch (Exception ex)
                {
                    //Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
            }
        }

        public List<ARrptSurtidoMateriales> BuscaRptSurtidoMateriales(Nullable<DateTime> fechaInicioP, Nullable<DateTime> fechaFinP, string solicitud)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                DateTime fechaInicio = fechaInicioP.GetValueOrDefault();
                DateTime fechaFin = (fechaFinP != null ? fechaFinP.GetValueOrDefault() : new DateTime(9999, 12, 31));

                return Context.ARtblInventarioMovimientoAgrupador
                    .Join(Context.ARtblRequisicionMaterial, im => im.ReferenciaMovtoId, rm => rm.RequisicionMaterialId, (im, rm) => new { rm = rm, im = im })
                    .Join(Context.tblPolizaRef, im => im.im.InventarioMovtoAgrupadorId, pr => pr.Referencia, (im, pr) => new { pr = pr, im = im })
                    .Join(Context.tblPoliza, pr => pr.pr.PolizaId, p => p.PolizaId, (pr, p) => new { p = p, pr = pr })
                    .Where(m => m.pr.im.im.TipoMovimientoId == TipoInventarioMovimiento.REQUISICION_MATERIAL_SURTIMIENTO
                             && DateTime.Compare(m.pr.im.im.FechaCreacion, fechaInicio) >= 0
                             && DateTime.Compare(m.pr.im.im.FechaCreacion, fechaFin) <= 0
                             && m.pr.im.rm.CodigoRequisicion.Equals((solicitud != null && !solicitud.Trim().Equals("") ? solicitud : m.pr.im.rm.CodigoRequisicion))
                             && m.pr.pr.Tipo.Equals("A")
                             && m.pr.pr.TipoMovimientoId == tblTipoMovimiento.SURTIMIENTO_REQUISICION)
                    .AsEnumerable().Select(m => new ARrptSurtidoMateriales
                    {
                        InventarioMovtoAgrupadorId = m.pr.im.im.InventarioMovtoAgrupadorId,
                        RequisicionMaterialId = m.pr.im.rm.RequisicionMaterialId,
                        Solicitud = m.pr.im.rm.CodigoRequisicion,
                        Fecha = m.pr.im.im.FechaCreacion,
                        Poliza = m.p.Poliza
                    }).OrderBy(m => m.InventarioMovtoAgrupadorId).ToList();
            }
        }
    }
}