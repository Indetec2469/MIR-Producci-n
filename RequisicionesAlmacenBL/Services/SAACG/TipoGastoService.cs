using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class TipoGastoService : BaseService<tblTipoGasto>
    {
        public override bool Actualiza(tblTipoGasto entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblTipoGasto BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblTipoGasto Inserta(tblTipoGasto entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblTipoGasto> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblTipoGasto.Where(m => m.TipoGastoId.Equals("1") || m.TipoGastoId.Equals("2"))
                    .AsEnumerable().Select(m => new tblTipoGasto
                    {
                        TipoGastoId = m.TipoGastoId,
                        Nombre = m.Nombre,
                        NombreCorto = m.NombreCorto
                    }).ToList().OrderBy(m => m.TipoGastoId);
            }
        }

        public List<tblTipoGasto> BuscaPorIds(List<string> ids)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblTipoGasto.Where(tg => ids.Contains(tg.TipoGastoId)).ToList();
            }
        }
    }
}