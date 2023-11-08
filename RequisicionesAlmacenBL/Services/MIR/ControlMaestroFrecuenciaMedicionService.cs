using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.MIR
{
    public class ControlMaestroFrecuenciaMedicionService : BaseService<MItblControlMaestroFrecuenciaMedicion>
    {
        public override bool Actualiza(MItblControlMaestroFrecuenciaMedicion entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override MItblControlMaestroFrecuenciaMedicion BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext contextv)
        {
            throw new NotImplementedException();
        }

        public override MItblControlMaestroFrecuenciaMedicion Inserta(MItblControlMaestroFrecuenciaMedicion entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MItblControlMaestroFrecuenciaMedicion> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblControlMaestroFrecuenciaMedicion.Where(fm => fm.Borrado == false).ToList();
            }
        }

        public IEnumerable<MIspConsultaFrecuenciaMedicionConNivel_Result> BuscaTodosConNivel()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MIspConsultaFrecuenciaMedicionConNivel().ToList();
            }
        }
    }
}
