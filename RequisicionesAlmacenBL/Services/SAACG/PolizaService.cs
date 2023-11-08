using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class PolizaService : BaseService<tblPoliza>
    {
        public override bool Actualiza(tblPoliza entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblPoliza BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblPoliza Inserta(tblPoliza entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblPoliza> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblPoliza.AsEnumerable().Select(m => new tblPoliza {
                    PolizaId = m.PolizaId,
                    Ejercicio = m.Ejercicio,
                    Poliza = m.Poliza
                }).ToList().OrderBy(m => m.Poliza);
            }
        }

        public tblPoliza BuscaPolizaPorReferenciaYTipoMovimientoId(int referencia, int tipoMovimientoId)
        {
            return BuscaPolizaPorReferenciaTipoMovimientoIdYTipo(referencia, tipoMovimientoId, null);
        }

        public tblPoliza BuscaPolizaPorReferenciaTipoMovimientoIdYTipo(int referencia, int tipoMovimientoId, string tipo)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblPoliza
                    .Join(Context.tblPolizaRef, d => d.PolizaId, e => e.PolizaId, (d, e) => new { e = e, d = d })
                    .Where(m => m.e.Referencia == referencia 
                             && m.e.TipoMovimientoId == tipoMovimientoId
                             && m.e.Tipo.Equals((tipo != null ? tipo : m.e.Tipo)))
                    .AsEnumerable().Select(m => new tblPoliza
                    {
                        PolizaId = m.d.PolizaId,
                        Ejercicio = m.d.Ejercicio,
                        Poliza = m.d.Poliza
                    }).FirstOrDefault();
            }
        }

        public void ActualizaPolizaAjusteInventario(Nullable<int> ajusteInventarioId, Nullable<DateTime> fechaPoliza, SAACGContext context)
        {
            context.ARspActualizaPolizaAjusteInventario(ajusteInventarioId, fechaPoliza);
        }

        public void ActualizaPolizaTransferencia(Nullable<int> transferenciaId, Nullable<DateTime> fechaPoliza, SAACGContext context)
        {
            context.ARspActualizaPolizaTransferencia(transferenciaId, fechaPoliza);
        }

        public void ActualizaPolizaSurtimiento(Nullable<int> inventarioMovimientoAgrupadorId, Nullable<System.DateTime> fechaPoliza, SAACGContext context)
        {
            context.ARspActualizaPolizaSurtimiento(inventarioMovimientoAgrupadorId, fechaPoliza);
        }

        public void ActualizaPolizaGastoComprometido(Nullable<int> ordenCompraId, string tipoOrdenCompra, Nullable<DateTime> fecha, SAACGContext context)
        {
            context.spActualizaPolizaGastoComprometido(ordenCompraId, tipoOrdenCompra, fecha);
        }

        public void ActualizaPolizaGastoDevengado(Nullable<int> compraId, string tipoCompra, Nullable<DateTime> fecha, SAACGContext context)
        {
            context.spActualizaPolizaGastoDevengado(compraId, tipoCompra, fecha);
        }
    }
}