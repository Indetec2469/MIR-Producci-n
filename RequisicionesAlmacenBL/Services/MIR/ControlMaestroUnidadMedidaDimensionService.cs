using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RequisicionesAlmacenBL.Services
{
    public class ControlMaestroUnidadMedidaDimensionService : BaseService<MItblControlMaestroUnidadMedidaDimension>
    {
        public override bool Actualiza(MItblControlMaestroUnidadMedidaDimension entidad, SAACGContext context)
        {
            // Agregamos la entidad que vamos a actualizar al Context
            context.MItblControlMaestroUnidadMedidaDimension.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in MItblControlMaestroUnidadMedidaDimension.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos true o false si se realizo correctamente la operacion
            return true;
        }

        public override MItblControlMaestroUnidadMedidaDimension BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.MItblControlMaestroUnidadMedidaDimension.Find(id);
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override MItblControlMaestroUnidadMedidaDimension Inserta(MItblControlMaestroUnidadMedidaDimension entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            MItblControlMaestroUnidadMedidaDimension controlMaestroUnidadMedidaDimension = context.MItblControlMaestroUnidadMedidaDimension.Add(entidad);

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return controlMaestroUnidadMedidaDimension;
        }

        public IEnumerable<MItblControlMaestroUnidadMedidaDimension> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblControlMaestroUnidadMedidaDimension.Where(umd => umd.Borrado == false).ToList();
            }
        }

        public Boolean EsDimensionExiste(MItblControlMaestroUnidadMedidaDimension controlMaestroUnidadMedidaDimension)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblControlMaestroUnidadMedidaDimension.Any(umd => umd.DimensionId == controlMaestroUnidadMedidaDimension.DimensionId && umd.UnidadMedidaId == controlMaestroUnidadMedidaDimension.UnidadMedidaId && umd.UnidadMedidaDimensionId != controlMaestroUnidadMedidaDimension.UnidadMedidaDimensionId && umd.Borrado == false);
            }
        }
    }
}
