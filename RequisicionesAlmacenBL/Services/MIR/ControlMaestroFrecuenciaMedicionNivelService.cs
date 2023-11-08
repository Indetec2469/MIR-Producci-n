using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.MIR
{
    public class ControlMaestroFrecuenciaMedicionNivelService : BaseService<MItblControlMaestroFrecuenciaMedicionNivel>
    {
        public override bool Actualiza(MItblControlMaestroFrecuenciaMedicionNivel entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override MItblControlMaestroFrecuenciaMedicionNivel BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override MItblControlMaestroFrecuenciaMedicionNivel Inserta(MItblControlMaestroFrecuenciaMedicionNivel entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MItblControlMaestroFrecuenciaMedicionNivel> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblControlMaestroFrecuenciaMedicionNivel.Where(fm => fm.Borrado == false).ToList();
            }
        }
    }
}
