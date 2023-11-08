using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class ProgramaGobiernoService : BaseService<tblProgramaGobierno>
    {
        public override bool Actualiza(tblProgramaGobierno entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblProgramaGobierno BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public tblProgramaGobierno BuscaPorId(string id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblProgramaGobierno.Where(pr => pr.ProgramaGobiernoId == id).FirstOrDefault();
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblProgramaGobierno Inserta(tblProgramaGobierno entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblProgramaGobierno> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblProgramaGobierno.ToList();
            }
        }

        public IEnumerable<spComboProgramaGobierno_Result> BuscaComboProgramaGobierno(string programaPresupuestarioId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.spComboProgramaGobierno(programaPresupuestarioId).ToList();
            }
        }
    }
}
