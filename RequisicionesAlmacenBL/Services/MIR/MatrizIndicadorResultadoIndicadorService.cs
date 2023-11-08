using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Models.Mapeos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RequisicionesAlmacenBL.Services
{
    public class MatrizIndicadorResultadoIndicadorService : BaseService<MItblMatrizIndicadorResultadoIndicador>
    {
        public override bool Actualiza(MItblMatrizIndicadorResultadoIndicador entidad, SAACGContext context)
        {
            // Agregamos la entidad que vamos a actualizar al Context
            context.MItblMatrizIndicadorResultadoIndicador.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in MItblMatrizIndicadorResultadoIndicador.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos true o false si se realizo correctamente la operacion
            return true;
        }

        public override MItblMatrizIndicadorResultadoIndicador BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                // Retornamos la entidad con el ID que se envio como parametro
                return Context.MItblMatrizIndicadorResultadoIndicador.Where(miri => miri.MIRIndicadorId == id).FirstOrDefault();
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override MItblMatrizIndicadorResultadoIndicador Inserta(MItblMatrizIndicadorResultadoIndicador entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            MItblMatrizIndicadorResultadoIndicador matrizIndicadorResultadoIndicador = context.MItblMatrizIndicadorResultadoIndicador.Add(entidad);

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return matrizIndicadorResultadoIndicador;
        }

        public IEnumerable<MItblMatrizIndicadorResultadoIndicador> BuscaPorMIRId(int mirId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblMatrizIndicadorResultadoIndicador.Where(miri => miri.MIRId == mirId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).ToList();
            }
        }

        public IEnumerable<MItblMatrizIndicadorResultadoIndicador> BuscaPorMIRIdYNivelActividad(int mirId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblMatrizIndicadorResultadoIndicador.Where(miri => miri.MIRId == mirId && miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.ACTIVIDAD && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).ToList();
            }
        }

        public IEnumerable<MItblMatrizIndicadorResultadoIndicador> BuscaPorMIRIdYNivelActividadPorComponente(int mirId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                List<MItblMatrizIndicadorResultadoIndicador> ListaMatrizIndicadorResultadoIndicador = new List<MItblMatrizIndicadorResultadoIndicador>();
                Context.MItblMatrizIndicadorResultadoIndicador.Where(miri => miri.MIRId == mirId && miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).OrderBy(miri => miri.Codigo).ToList().ForEach(miri =>
                {
                    Context.MItblMatrizIndicadorResultadoIndicador.Where(_miri => _miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.ACTIVIDAD && _miri.MIRIndicadorComponenteId == miri.MIRIndicadorId && _miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).OrderBy(_miri => _miri.Codigo).ToList().ForEach(_miri => {
                        ListaMatrizIndicadorResultadoIndicador.Add(_miri);
                    });

                });
                return ListaMatrizIndicadorResultadoIndicador;
            }
        }

        public IEnumerable<MItblMatrizIndicadorResultadoIndicador> BuscaListaPorMIRIndicadorComponenteId(int mirId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblMatrizIndicadorResultadoIndicador.Where(miri => miri.MIRId == mirId && miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).OrderBy(miri => miri.Codigo).ToList();
            }
        }

        public MItblMatrizIndicadorResultadoIndicador BuscaPorMIRIndicadorComponenteId(int? mirIndicadorComponenteId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblMatrizIndicadorResultadoIndicador.Where(miri => miri.MIRIndicadorId == mirIndicadorComponenteId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).FirstOrDefault();
            }
        }

        public string BuscaPorProyecto(List<int> listaMIRIndicadorComponenteId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return string.Join(",", Context.MItblMatrizIndicadorResultadoIndicador.Where(miri => listaMIRIndicadorComponenteId.Contains(miri.MIRIndicadorId) && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).GroupBy(miri => miri.ProyectoId).Select(miri => miri.Key).ToList());
            }
        }
    }
}
