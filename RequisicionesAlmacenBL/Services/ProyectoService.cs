using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class ProyectoService : BaseService<Proyecto>
    {
        public override bool Actualiza(Proyecto entidad)
        {
            throw new NotImplementedException();
        }

        public override Proyecto BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId)
        {
            throw new NotImplementedException();
        }

        public override Proyecto Inserta(Proyecto entidad)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Proyecto> BuscaTodos()
        {
            using (var Context = new SAACGContext())
            {
                return Context.Proyecto.AsEnumerable().Select(proyecto => new Proyecto { 
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

        public List<Proyecto> BuscaProyectosPorRamoId(string ramoId)
        {
            using (var Context = new SAACGContext())
            {
                return Context.Proyecto.Where(m => m.Ramo.Where(r => r.RamoId == ramoId).Any()).ToList();
            }
        }

        public List<Rep_Proyecto_Result> BuscaFechaInicioYFechaFin(DateTime fechaInicio, DateTime fechaFin)
        {
            return EjecutarRepProyecto(fechaInicio, fechaFin, "");
        }

        private List<Rep_Proyecto_Result> EjecutarRepProyecto(DateTime fehcaInicio, DateTime fechaFin, string where)
        {
            using (var Context = new SAACGContext())
            {
                return Context.Rep_Proyecto(fehcaInicio, fechaFin, where).ToList();
            }
        }
    }
}
