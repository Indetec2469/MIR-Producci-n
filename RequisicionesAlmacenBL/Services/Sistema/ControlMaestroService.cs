using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services
{
    public class ControlMaestroService : BaseService<GRtblControlMaestro>
    {
        public override bool Actualiza(GRtblControlMaestro entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override GRtblControlMaestro BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.GRtblControlMaestro.Where(m => m.ControlId == id).AsEnumerable().Select(m => new GRtblControlMaestro
                {
                    ControlId = m.ControlId,
                    Control = m.Control,
                    Valor = m.Valor
                }).FirstOrDefault();
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override GRtblControlMaestro Inserta(GRtblControlMaestro entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GRtblControlMaestro> BuscaControl(String control)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Context.Configuration.LazyLoadingEnabled = false;
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.GRtblControlMaestro.Where(cm => cm.Control == control && cm.Activo).ToList();
            }
        }
    }
}