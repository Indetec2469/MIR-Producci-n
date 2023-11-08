using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Models.Mapeos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacenBL.Services.Sistema
{
    public class AlertaConfiguracionService : BaseService<GRtblAlertaConfiguracion>
    {
        public override GRtblAlertaConfiguracion Inserta(GRtblAlertaConfiguracion entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            GRtblAlertaConfiguracion rolMenu = context.GRtblAlertaConfiguracion.Add(entidad);

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return rolMenu;
        }

        public override bool Actualiza(GRtblAlertaConfiguracion entidad, SAACGContext context)
        {
            // Agregamos la entidad que vamos a actualizar al Context
            context.GRtblAlertaConfiguracion.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in GRtblAlertaConfiguracion.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

                // Retornamos true o false si se realizo correctamente la operacion
                return true;
        }

        public bool GuardaCambios(List<GRtblAlertaConfiguracion> detalles)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    foreach (GRtblAlertaConfiguracion detalle in detalles)
                    {
                        //Agregamos los detalles al Context
                        Context.GRtblAlertaConfiguracion.Add(detalle);

                        if (detalle.AlertaConfiguracionId > 0)
                        {
                            Context.Entry(detalle).State = EntityState.Modified;

                            //Marcar todas las propiedades que no se pueden actualizar como FALSE
                            //para que no se actualice su informacion en Base de Datos
                            foreach (string propertyName in ARtblRequisicionMaterialDetalle.PropiedadesNoActualizables)
                            {
                                Context.Entry(detalle).Property(propertyName).IsModified = false;
                            }
                        }
                    }

                    //Guardamos cambios
                    Context.SaveChanges();

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    //Retornamos la entidad que se acaba de guardar en la Base de Datos
                    return true;
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

        public override GRtblAlertaConfiguracion BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.GRtblAlertaConfiguracion.Where(m => m.AlertaConfiguracionId == id).FirstOrDefault();
            }
        }

        public GRtblAlertaConfiguracion ExisteConfiguracion(int id, int alertaEtapaAccionId, Nullable<int> empleadoId, Nullable<int> figuraId, int tipoNotificacionId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.GRtblAlertaConfiguracion.Where(x => x.AlertaConfiguracionId != id
                                                           && x.AlertaEtapaAccionId == alertaEtapaAccionId
                                                           && x.EmpleadoId == empleadoId
                                                           && x.FiguraId == figuraId
                                                           && x.TipoNotificacionId == tipoNotificacionId
                                                           && x.EstatusId == EstatusRegistro.ACTIVO).FirstOrDefault();
            }
        }

        public IEnumerable<GRtblAlertaConfiguracion> BuscaListadoAlertaConfiguracion()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos el listado
                return Context.GRtblAlertaConfiguracion.Where(m => m.EstatusId == EstatusRegistro.ACTIVO).ToList();
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GRspAlertasConfiguracionMenuPrincipal_Result> BuscaListadoMenuPrincipal()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.GRspAlertasConfiguracionMenuPrincipal().ToList();
            }
        }

        public IEnumerable<GRspAlertasConfiguracionEmpleados_Result> BuscaListadoEmpleados()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.GRspAlertasConfiguracionEmpleados().ToList();
            }
        }

        public string GetNombreEtapaAccion(int etapaAccionId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspGetNombreEtapaAccion(etapaAccionId).FirstOrDefault();
            }
        }
    }
}
