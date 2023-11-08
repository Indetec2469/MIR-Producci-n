using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacenBL.Services.Sistema
{
    public class ConfiguracionEnteService : BaseService<GRtblConfiguracionEnte>
    {
        public override GRtblConfiguracionEnte Inserta(GRtblConfiguracionEnte entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            GRtblConfiguracionEnte configuracion = context.GRtblConfiguracionEnte.Add(entidad);

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return configuracion;
        }

        public override bool Actualiza(GRtblConfiguracionEnte entidad, SAACGContext context)
        {
            // Agregamos la entidad que vamos a actualizar al Context
            context.GRtblConfiguracionEnte.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in GRtblConfiguracionEnte.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos true o false si se realizo correctamente la operacion
            return true;
        }

        public override GRtblConfiguracionEnte BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.GRtblConfiguracionEnte.Where(m => m.ConfiguracionEnteId == id && m.EstatusId == EstatusRegistro.ACTIVO).FirstOrDefault();
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public int GuardaCambios(GRtblConfiguracionEnte entidad)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    // Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Creamos el Id de la cabecera
                    int configuracionId = entidad.ConfiguracionEnteId;

                    //Validamos si es un registro nuevo
                    if (entidad.ConfiguracionEnteId == 0)
                    {
                        configuracionId = Inserta(entidad, Context).ConfiguracionEnteId;
                    }
                    else
                    {
                        Actualiza(entidad, Context);
                    }

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    //Retornamos la entidad que se acaba de guardar en la Base de Datos
                    return configuracionId;
                }
                catch (DbEntityValidationException ex)
                {
                    // Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
                catch (Exception ex)
                {
                    // Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
            }
        }

        public GRtblConfiguracionEnte BuscaConfigutacionEnte()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.GRtblConfiguracionEnte.Where(m => m.EstatusId == EstatusRegistro.ACTIVO).FirstOrDefault();
            }
        }

        public Nullable<DateTime> BuscaFechaOperacion()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.GRspGetFechaOperacion().FirstOrDefault();
            }
        }
    }
}
