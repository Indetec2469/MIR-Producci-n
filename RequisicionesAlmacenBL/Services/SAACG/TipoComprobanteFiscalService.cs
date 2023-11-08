using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class TipoComprobanteFiscalService : BaseService<tblTipoComprobanteFiscal>
    {
        public override bool Actualiza(tblTipoComprobanteFiscal entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblTipoComprobanteFiscal BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblTipoComprobanteFiscal Inserta(tblTipoComprobanteFiscal entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblTipoComprobanteFiscal> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblTipoComprobanteFiscal.ToList().OrderBy(m => m.Nombre);
            }
        }
    }
}
