using System;
using System.Collections.Generic;
using System.Linq;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Models.Mapeos;

namespace RequisicionesAlmacenBL.Services
{
    public class MenuPrincipalService : BaseService<GRtblMenuPrincipal>
    {
        public override bool Actualiza(GRtblMenuPrincipal entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override GRtblMenuPrincipal BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override GRtblMenuPrincipal Inserta(GRtblMenuPrincipal entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IList<GRtblMenuPrincipal> GetArbolMenuPrincipalUsuario(int usuarioId)
        {
            IList<GRtblMenuPrincipal> arbolMenuPrincipal = null;
            
            using (var Context = SAACGContextHelper.GetContext())
            {
                //arbolMenuPrincipal = Context.Fn_getArbolMenuPrincipal().ToList<MenuPrincipal>();
            }

            return arbolMenuPrincipal;
        }

        public IEnumerable<GRtblMenuPrincipal> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.GRtblMenuPrincipal.Where(mp => mp.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).ToList();
            }
        }

        public IEnumerable<GRspConsultaMenuPrincipalPermiso_Result> BuscaTodosPorRolId(int rolId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.GRspConsultaMenuPrincipalPermiso(rolId).ToList();
            }
        }

        public IEnumerable<GRspConsultaMenuPrincipalPermiso_Result> BuscaTodosPorRolLoginId(int rolId, string enteId)
        {
            using (var Context = SAACGContextHelper.GetContext(enteId))
            {
                return Context.GRspConsultaMenuPrincipalPermiso(rolId).ToList();
            }
        }
    }
}
