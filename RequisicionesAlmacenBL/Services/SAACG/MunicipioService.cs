using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class MunicipioService : BaseService<tblMunicipio>
    {
        public override bool Actualiza(tblMunicipio entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblMunicipio BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext contextv)
        {
            throw new NotImplementedException();
        }

        public override tblMunicipio Inserta(tblMunicipio entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblMunicipio> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblMunicipio.ToList().OrderBy(m => m.Clave);
            }
        }
    }
}