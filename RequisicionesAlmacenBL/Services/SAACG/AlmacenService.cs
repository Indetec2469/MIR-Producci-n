using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class AlmacenService : BaseService<tblAlmacen>
    {
        public override bool Actualiza(tblAlmacen entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblAlmacen BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblAlmacen.Where(m => m.AlmacenId.Equals(id.ToString())).FirstOrDefault();
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblAlmacen Inserta(tblAlmacen entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblAlmacen> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblAlmacen.OrderBy(m => m.Nombre).ToList();
            }
        }
    }
}
