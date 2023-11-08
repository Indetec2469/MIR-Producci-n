using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RequisicionesAlmacenBL.Services
{
    public class ControlMaestroTipoIndicadorNivelService : BaseService<MItblControlMaestroTipoIndicadorNivel>
    {
        public override bool Actualiza(MItblControlMaestroTipoIndicadorNivel entidad, SAACGContext context)
        {
            // Agregamos la entidad que vamos a actualizar al Context
            context.MItblControlMaestroTipoIndicadorNivel.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in MItblControlMaestroTipoIndicadorNivel.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos true o false si se realizo correctamente la operacion
            return true;                    
        }

        public override MItblControlMaestroTipoIndicadorNivel BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.MItblControlMaestroTipoIndicadorNivel.Find(id);
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override MItblControlMaestroTipoIndicadorNivel Inserta(MItblControlMaestroTipoIndicadorNivel entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            MItblControlMaestroTipoIndicadorNivel controlMaestroTipoIndicadorNivel = context.MItblControlMaestroTipoIndicadorNivel.Add(entidad);

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return controlMaestroTipoIndicadorNivel;
                    
        }

        public IEnumerable<MItblControlMaestroTipoIndicadorNivel> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblControlMaestroTipoIndicadorNivel.Where(tin => tin.Borrado == false).ToList();
            }
        }

        public Boolean EsNivelExiste(MItblControlMaestroTipoIndicadorNivel controlMaestroTipoIndicadorNivel)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblControlMaestroTipoIndicadorNivel.Any(tin => tin.NivelId == controlMaestroTipoIndicadorNivel.NivelId && tin.TipoIndicadorId == controlMaestroTipoIndicadorNivel.TipoIndicadorId && tin.TipoIndicadorNivelId != controlMaestroTipoIndicadorNivel.TipoIndicadorNivelId && tin.Borrado == false);
            }
        }
    }
}
