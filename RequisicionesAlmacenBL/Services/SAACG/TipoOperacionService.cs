using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class TipoOperacionService : BaseService<tblTipoOperacion>
    {
        public override bool Actualiza(tblTipoOperacion entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblTipoOperacion BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblTipoOperacion Inserta(tblTipoOperacion entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblTipoOperacion> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblTipoOperacion.ToList().OrderBy(m => m.TipoOperacionId);
            }
        }
    }
}
