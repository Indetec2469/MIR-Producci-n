using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Models.Mapeos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RequisicionesAlmacenBL.Services
{
    public class MatrizIndicadorResultadoIndicadorMetaService : BaseService<MItblMatrizIndicadorResultadoIndicadorMeta>
    {
        public override bool Actualiza(MItblMatrizIndicadorResultadoIndicadorMeta entidad, SAACGContext context)
        {
            // Agregamos la entidad que vamos a actualizar al Context
            context.MItblMatrizIndicadorResultadoIndicadorMeta.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in MItblMatrizIndicadorResultadoIndicadorMeta.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos true o false si se realizo correctamente la operacion
            return true;
        }

        public override MItblMatrizIndicadorResultadoIndicadorMeta BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                // Retornamos la entidad con el ID que se envio como parametro
                return Context.MItblMatrizIndicadorResultadoIndicadorMeta.Where(meta => meta.MIRIndicadorMetaId == id).FirstOrDefault();
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override MItblMatrizIndicadorResultadoIndicadorMeta Inserta(MItblMatrizIndicadorResultadoIndicadorMeta entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            MItblMatrizIndicadorResultadoIndicadorMeta matrizIndicadorResultadoIndicadorMeta = context.MItblMatrizIndicadorResultadoIndicadorMeta.Add(entidad);

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return matrizIndicadorResultadoIndicadorMeta;
        }

        public IEnumerable<MItblMatrizIndicadorResultadoIndicadorMeta> BuscaPorMIRIndicadorId(int mirIndicadorId)
        {
            using(var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblMatrizIndicadorResultadoIndicadorMeta.Where(mirim => mirim.MIRIndicadorId == mirIndicadorId && mirim.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).ToList();
            }
        }
    }
}
