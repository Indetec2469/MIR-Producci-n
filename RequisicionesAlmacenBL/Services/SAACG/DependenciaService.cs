using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class DependenciaService : BaseService<tblDependencia>
    {
        public override bool Actualiza(tblDependencia entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblDependencia BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public tblDependencia BuscaDependenciaPorId(string id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblDependencia.Where(m => m.DependenciaId == id).AsEnumerable().Select(m => new tblDependencia
                {
                    DependenciaId = m.DependenciaId,
                    Nombre = m.Nombre,
                    CuentaDeRegistro = m.CuentaDeRegistro
                }).FirstOrDefault();
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblDependencia Inserta(tblDependencia entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblDependencia> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblDependencia.Where(dependencia => dependencia.CuentaDeRegistro == true).AsEnumerable().Select(dependencia => new tblDependencia
                { 
                    DependenciaId = dependencia.DependenciaId, 
                    Nombre = dependencia.Nombre, 
                    CuentaDeRegistro = dependencia.CuentaDeRegistro 
                }).ToList().OrderBy(m => m.DependenciaId);
            }
        }

        public IEnumerable<tblDependencia> BuscaAreasPorEmpleadoId(int empleadoId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblDependencia.Where(m => m.CuentaDeRegistro == true)
                    .Join(Context.RHtblEmpleado, d => d.DependenciaId, e => e.AreaAdscripcionId, (d, e) => new { e = e, d = d })
                    .Where(m => m.e.EmpleadoId == empleadoId)
                    .AsEnumerable().Select(m => new tblDependencia
                    {
                        DependenciaId = m.d.DependenciaId,
                        Nombre = m.d.Nombre,
                        CuentaDeRegistro = m.d.CuentaDeRegistro
                    }).ToList().OrderBy(m => m.DependenciaId);
            }
        }
        
        public IEnumerable<tblDependencia> BuscaUnidadesAdministrativasPorAreaId(string areaId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaDependenciasPorAreaId(areaId).ToList();
            }
        }

        public List<tblDependencia> BuscaDependenciasPorProyectoId(string proyectoId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblDependencia.Where(m => m.CuentaDeRegistro == true)
                    .Join(Context.tblProyecto_Dependencia, d => d.DependenciaId, pd => pd.DependenciaId, (d, pd) => new tblDependencia { }).OrderBy(d=> d.DependenciaId).ToList();
            }
        }
    }
}
