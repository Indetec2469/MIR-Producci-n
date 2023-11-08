using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class CuentaPresupuestalEgrService : BaseService<tblCuentaPresupuestalEgr>
    {
        public override bool Actualiza(tblCuentaPresupuestalEgr entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblCuentaPresupuestalEgr BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblCuentaPresupuestalEgr Inserta(tblCuentaPresupuestalEgr entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblCuentaPresupuestalEgr> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblCuentaPresupuestalEgr.AsEnumerable().Select(cpe => new tblCuentaPresupuestalEgr {
                    CuentaPresupuestalEgrId = cpe.CuentaPresupuestalEgrId,
                    RamoId = cpe.RamoId,
                    ProyectoId = cpe.ProyectoId,
                    DependenciaId = cpe.DependenciaId,
                    ObjetoGastoId = cpe.ObjetoGastoId,
                    TipoGastoId = cpe.TipoGastoId
                }).ToList();
            }
        }

        public Nullable<int> BuscaCuentaPresupuestalIdPoRamoIdProyectoIdDependenciaIdTipoGastoIdObjetoGastoId(string ramoId, 
                                                                                          string proyectoId, 
                                                                                          string dependenciaId, 
                                                                                          string tipoGastoId,
                                                                                          string objetoGastoId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                var cuentaPresupuestal = Context.tblCuentaPresupuestalEgr.Where(m => m.RamoId == ramoId
                                                           && m.ProyectoId == proyectoId
                                                           && m.DependenciaId == dependenciaId
                                                           && m.TipoGastoId == tipoGastoId
                                                           && m.ObjetoGastoId == objetoGastoId
                    ).FirstOrDefault();

                return cuentaPresupuestal != null ? (Nullable<int>) cuentaPresupuestal.CuentaPresupuestalEgrId : null;
            }
        }
    }
}
