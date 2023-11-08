using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RequisicionesAlmacenBL.Services
{
    public class ControlMaestroDimensionNivelService : BaseService<MItblControlMaestroDimensionNivel>
    {
        public override bool Actualiza(MItblControlMaestroDimensionNivel entidad, SAACGContext context)
        {
            // Agregamos la entidad que vamos a actualizar al Context
            context.MItblControlMaestroDimensionNivel.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in MItblControlMaestroDimensionNivel.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos true o false si se realizo correctamente la operacion
            return true;

        }

        public override MItblControlMaestroDimensionNivel BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.MItblControlMaestroDimensionNivel.Find(id);
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override MItblControlMaestroDimensionNivel Inserta(MItblControlMaestroDimensionNivel entidad, SAACGContext context)
        {

            // Agregamos la entidad el Context
            MItblControlMaestroDimensionNivel controlMaestroDimensionNivel = context.MItblControlMaestroDimensionNivel.Add(entidad);

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return controlMaestroDimensionNivel;
        }

        public IEnumerable<MItblControlMaestroDimensionNivel> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblControlMaestroDimensionNivel.Where(dn => dn.Borrado == false).ToList();
            }
        }

        public Boolean EsNivelExiste(MItblControlMaestroDimensionNivel controlMaestroDimensionNivel)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblControlMaestroDimensionNivel.Any(dn => dn.NivelId == controlMaestroDimensionNivel.NivelId && dn.DimensionId == controlMaestroDimensionNivel.DimensionId && dn.DimensionNivelId != controlMaestroDimensionNivel.DimensionNivelId && dn.Borrado == false);
            }
        }
    }
}
