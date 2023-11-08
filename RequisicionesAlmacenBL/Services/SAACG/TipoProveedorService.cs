using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class TipoProveedorService : BaseService<tblTipoProveedor>
    {
        public override bool Actualiza(tblTipoProveedor entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblTipoProveedor BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblTipoProveedor Inserta(tblTipoProveedor entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblTipoProveedor> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblTipoProveedor.ToList().OrderBy(m => m.TipoProveedorId);
            }
        }
    }
}
