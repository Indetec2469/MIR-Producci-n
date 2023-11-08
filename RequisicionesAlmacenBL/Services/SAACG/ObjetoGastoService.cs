using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class ObjetoGastoService : BaseService<tblObjetoGasto>
    {
        private static string INCLUDE_PRODUCTOS = "Productos";

        public override bool Actualiza(tblObjetoGasto entidad, SAACGContext contextv)
        {
            throw new NotImplementedException();
        }

        public override tblObjetoGasto BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext contextv)
        {
            throw new NotImplementedException();
        }

        public override tblObjetoGasto Inserta(tblObjetoGasto entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public List<tblObjetoGasto> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblObjetoGasto.ToList();
            }
        }

        public List<tblObjetoGasto> BuscaObjetoGastoPorNivel(string inicial)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                Context.Configuration.LazyLoadingEnabled = false;

                return Context.spObjetoGasto(inicial).ToList();
            }
        }

        public List<tblObjetoGasto> BuscaPartidasEspecificas(string inicial)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                Context.Configuration.LazyLoadingEnabled = false;

                return Context.tblObjetoGasto.Where(m => m.ObjetoGastoId.StartsWith(inicial) && m.CuentaDeRigistro == true).ToList();
            }
        }

        public List<tblObjetoGasto> BuscaPartidasEspecificas()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                Context.Configuration.LazyLoadingEnabled = false;

                return Context.tblObjetoGasto.Where(m => m.CuentaDeRigistro == true).ToList();
            }
        }

        public List<CGvwClasificadorObjetoGasto> BuscaClasificadorObjetoGasto()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.CGvwClasificadorObjetoGasto.ToList();
            }
        }
    }
}
