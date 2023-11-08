using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Services
{
    class InventarioMovimientoAgrupadorService : BaseService<ARtblInventarioMovimientoAgrupador>
    {
        public override bool Actualiza(ARtblInventarioMovimientoAgrupador entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override ARtblInventarioMovimientoAgrupador BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblInventarioMovimientoAgrupador.Where(m => m.InventarioMovtoAgrupadorId == id).FirstOrDefault();
            }
        }

        public ARtblInventarioMovimientoAgrupador BuscaPorId(int id, SAACGContext context)
        {
            return context.ARtblInventarioMovimientoAgrupador.Where(m => m.InventarioMovtoAgrupadorId == id).FirstOrDefault();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override ARtblInventarioMovimientoAgrupador Inserta(ARtblInventarioMovimientoAgrupador entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }
    }
}
