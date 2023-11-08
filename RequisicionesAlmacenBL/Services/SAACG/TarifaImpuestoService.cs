using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class TarifaImpuestoService : BaseService<tblTarifaImpuesto>
    {
        public override bool Actualiza(tblTarifaImpuesto entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblTarifaImpuesto BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblTarifaImpuesto Inserta(tblTarifaImpuesto entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblTarifaImpuesto> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblTarifaImpuesto.ToList().OrderBy(m => m.Nombre);
            }
        }
    }
}
