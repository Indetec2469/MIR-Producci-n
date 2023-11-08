using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.Compras
{
    public class CortesiaService : BaseService<ARtblCortesia>
    {
        public override ARtblCortesia BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblCortesia.Where(m => m.CortesiaId == id).FirstOrDefault();
            }
        }

        public List<ARtblCortesia> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblCortesia.ToList();
            }
        }

        public List<ARvwListadoReciboCortesia> BuscaListado()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARvwListadoReciboCortesia.ToList();
            }
        }

        public override ARtblCortesia Inserta(ARtblCortesia entidad, SAACGContext context)
        {
            //Asignamos el autonumerico
            entidad.Codigo = new AutonumericoService().GetSiguienteAutonumerico("Recibo de Cortesía", context);

            //Agregamos la entidad el Context
            ARtblCortesia modelo = context.ARtblCortesia.Add(entidad);

            //Guardamos cambios
            context.SaveChanges();

            //Retornamos si guardó correctamente
            return modelo;
        }

        public override bool Actualiza(ARtblCortesia entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            ARtblCortesia modelo = context.ARtblCortesia.Add(entidad);

            //Marcamos el modelo como modificado
            context.Entry(entidad).State = EntityState.Modified;

            //Marcar todas las propiedades que no se pueden actualizar como FALSE
            //para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in ARtblCortesia.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            //Guardamos cambios
            return context.SaveChanges() > 0;
        }

        public int GuardaCambios(ARtblCortesia entidad)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Guardamos el modelo
                    ARtblCortesia cortesia = Inserta(entidad, Context);

                    //Guardamos cambios
                    Context.SaveChanges();

                    //Registramos los movimientos en el inventario
                    Context.ARspAfectaReciboCortesia(cortesia.CortesiaId, cortesia.CreadoPorId);

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    //Retornamos la entidad que se acaba de guardar en la Base de Datos
                    return cortesia.CortesiaId;
                }
                catch (DbEntityValidationException ex)
                {
                    //Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
                catch (Exception ex)
                {
                    //Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public List<ARspConsultaCortesiaDetalles_Result> BuscaDetallesPorCortesiaId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaCortesiaDetalles(id).ToList();
            }
        }
    }
}