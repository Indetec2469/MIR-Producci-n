using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class RamoService : BaseService<tblRamo>
    {
        public override bool Actualiza(tblRamo entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblRamo BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblRamo Inserta(tblRamo entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblRamo> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblRamo.AsEnumerable().Select(m => new tblRamo {
                    RamoId = m.RamoId,
                    Nombre = m.Nombre,
                    Editable = m.Editable,
                    FuenteFinanciamientoCONACId = m.FuenteFinanciamientoCONACId,
                    TipoGastoDestinoId = m.TipoGastoDestinoId,
                    Anio = m.Anio,
                    RamoDetalleId = m.RamoDetalleId,
                    TipoRecurso = m.TipoRecurso
                }).ToList().OrderBy(m => m.RamoId);
            }
        }
    }
}