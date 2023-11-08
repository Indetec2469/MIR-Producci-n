using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class EstadoService : BaseService<tblEstado>
    {
        public override bool Actualiza(tblEstado entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblEstado BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext contextv)
        {
            throw new NotImplementedException();
        }

        public override tblEstado Inserta(tblEstado entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }
        
        public IEnumerable<tblEstado> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblEstado.ToList().OrderBy(m => m.EstadoId);
            }
        }
    }
}