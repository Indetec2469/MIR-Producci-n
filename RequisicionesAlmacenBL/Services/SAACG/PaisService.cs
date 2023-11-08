using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class PaisService : BaseService<tblPais>
    {
        public override bool Actualiza(tblPais entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblPais BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext contextv)
        {
            throw new NotImplementedException();
        }

        public override tblPais Inserta(tblPais entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblPais> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblPais.ToList().OrderBy(m => m.PaisId);
            }
        }
    }
}