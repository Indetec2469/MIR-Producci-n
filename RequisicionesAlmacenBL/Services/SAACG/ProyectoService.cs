using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class ProyectoService : BaseService<tblProyecto>
    {
        public override bool Actualiza(tblProyecto entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblProyecto BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override tblProyecto Inserta(tblProyecto entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblProyecto> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblProyecto.AsEnumerable().Select(proyecto => new tblProyecto { 
                    ProyectoId = proyecto.ProyectoId, 
                    ClasificadorFuncionalId = proyecto.ClasificadorFuncionalId,
                    SubProgramaGobiernoId = proyecto.SubProgramaGobiernoId,
                    Nombre = proyecto.Nombre,
                    Editable = proyecto.Editable,
                    ClasificacionGeograficaId = proyecto.ClasificacionGeograficaId,
                    Capitulos = proyecto.Capitulos
                }).ToList().OrderBy(m => m.ProyectoId);
            }
        }

        public List<tblProyecto> BuscaProyectosPorRamoId(string ramoId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblProyecto.Where(m => m.tblRamo.Where(r => r.RamoId == ramoId).Any()).ToList();
            }
        }

        public List<MIspConsultaRepProyecto_Result> BuscaFechaInicioYFechaFin(DateTime fechaInicio, DateTime fechaFin)
        {
            return EjecutarRepProyecto(fechaInicio, fechaFin);
        }

        private List<MIspConsultaRepProyecto_Result> EjecutarRepProyecto(DateTime fehcaInicio, DateTime fechaFin)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MIspConsultaRepProyecto(fehcaInicio, fechaFin).ToList();
            }
        }

        public List<string> BuscaRelacionProyectoYProgramaGobierno(string programaGobiernoId)

        {

            using (var Context = SAACGContextHelper.GetContext())

            {

                string query = "SELECT p.ProyectoId FROM tblProyecto p " +

                    "INNER JOIN tblSubProgramaGobierno spg ON p.SubProgramaGobiernoId = spg.SubProgramaGobiernoId " +

                    "INNER JOIN tblProgramaGobierno pg ON spg.ProgramaGobiernoId = pg.ProgramaGobiernoId " +

                    "WHERE pg.ProgramaGobiernoId = '" + (programaGobiernoId != "" ? programaGobiernoId : "0") + "'";



                return Context.Database.SqlQuery<string>(query).ToList();

            }

        }
        public IEnumerable<tblProyecto> BuscaProyectosPorAreaId(string areaId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaProyectosPorAreaId(areaId).ToList();
            }
        }

        public IEnumerable<tblProyecto_Dependencia> BuscaProyectosDependenciasPorAreaId(string areaId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaDependenciasProyectosPorAreaId(areaId).ToList();
            }
        }

        public IEnumerable<tblProyecto_Dependencia> BuscaProyectosDependencias()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblProyecto_Dependencia.ToList();
            }
        }
    }
}
