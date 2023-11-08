using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Models.Mapeos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacenBL.Services.Compras
{
    public class InvitacionArticuloService : BaseService<ARtblInvitacionArticulo>
    {
        public override ARtblInvitacionArticulo BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblInvitacionArticulo.Where(m => m.InvitacionArticuloId == id).FirstOrDefault();
            }
        }

        public ARtblInvitacionArticuloDetalle BuscaDetallePorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblInvitacionArticuloDetalle.Where(m => m.InvitacionArticuloDetalleId == id).FirstOrDefault();
            }
        }

        public ARtblInvitacionArticuloDetalle BuscaDetalleInvitadoPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblInvitacionArticuloDetalle.Where(m => m.InvitacionArticuloDetalleId == id
                                                                    && m.EstatusId == AREstatusInvitacionArticuloDetalle.INVITADO).FirstOrDefault();
            }
        }

        public List<ARtblInvitacionArticulo> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblInvitacionArticulo.ToList();
            }
        }

        public override ARtblInvitacionArticulo Inserta(ARtblInvitacionArticulo entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            ARtblInvitacionArticulo invitacionArticulo = context.ARtblInvitacionArticulo.Add(entidad);

            //Guardamos cambios
            context.SaveChanges();

            //Retornamos si se guardó correctamente
            return invitacionArticulo;
        }

        public override bool Actualiza(ARtblInvitacionArticulo entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            ARtblInvitacionArticulo InvitacionArticulo = context.ARtblInvitacionArticulo.Add(entidad);

            //Marcamos el modelo como modificado
            context.Entry(entidad).State = EntityState.Modified;

            //Marcar todas las propiedades que no se pueden actualizar como FALSE
            //para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in ARtblInvitacionArticulo.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            //Guardamos cambios
            return context.SaveChanges() > 0;
        }

        public void GuardaDetalles(int invitacionArticuloId, List<ARtblInvitacionArticuloDetalle> detalles, SAACGContext context)
        {
            foreach (ARtblInvitacionArticuloDetalle detalle in detalles)
            {
                //Asignamos el Id de la cabecera
                detalle.InvitacionArticuloId = invitacionArticuloId;

                //Agregamos los detalles al Context
                context.ARtblInvitacionArticuloDetalle.Add(detalle);

                if (detalle.InvitacionArticuloDetalleId > 0)
                {
                    context.Entry(detalle).State = EntityState.Modified;

                    //Marcar todas las propiedades que no se pueden actualizar como FALSE
                    //para que no se actualice su informacion en Base de Datos
                    foreach (string propertyName in ARtblInvitacionArticuloDetalle.PropiedadesNoActualizables)
                    {
                        context.Entry(detalle).Property(propertyName).IsModified = false;
                    }
                }
            }

            //Guardamos cambios
            context.SaveChanges();            
        }

        public int GuardaCambios(ARtblInvitacionArticulo entidad, 
                                 List<ARtblInvitacionArticuloDetalle> detalles,
                                 List<ARtblRequisicionMaterialDetalle> requisicionDetalles)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Creamos el Id de la cabecera
                    int invitacionArticuloId = -1;

                    if (entidad != null)
                    {
                        invitacionArticuloId = entidad.InvitacionArticuloId;

                        //Validamos si es un registro nuevo
                        if (entidad.InvitacionArticuloId == 0)
                        {
                            invitacionArticuloId = Inserta(entidad, Context).InvitacionArticuloId;
                        }
                        else
                        {
                            Actualiza(entidad, Context);
                        }

                        //Guardamos cambios
                        Context.SaveChanges();
                    }

                    //Guardamos los detalles
                    if (detalles != null)
                    {
                        GuardaDetalles(invitacionArticuloId, detalles, Context);
                    }

                    //Actualizamos los detalles de la requisición en caso de Cancelación
                    if (requisicionDetalles != null)
                    {
                        new RequisicionMaterialService().GuardaDetalles(requisicionDetalles, Context);

                        List<int> requisicionesIds = new List<int>();

                        foreach (ARtblRequisicionMaterialDetalle requisicionDetalle in requisicionDetalles)
                        {
                            if (!requisicionesIds.Contains(requisicionDetalle.RequisicionMaterialId))
                            {
                                requisicionesIds.Add(requisicionDetalle.RequisicionMaterialId);
                            }
                        }

                        if (requisicionesIds != null)
                        {
                            foreach (int requisicionId in requisicionesIds)
                            {
                                //Actualizamos el estatus de la Requisición
                                Context.ARspActualizaEstatusRequisicionMaterial(requisicionId);
                            }
                        }
                    }

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    //Retornamos la entidad que se acaba de guardar en la Base de Datos
                    return invitacionArticuloId;
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

        public List<ARtblInvitacionArticuloDetalle> BuscaDetallesNoCancelados(int invitacionArticuloId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblInvitacionArticuloDetalle.Where(m => m.InvitacionArticuloId == invitacionArticuloId
                    && m.EstatusId != AREstatusInvitacionArticuloDetalle.CANCELADO).ToList();
            }
        }

        public List<ARspConsultaInvitacionArticulosPorConvertir_Result> BuscaInvitacionArticulosPorConvertir()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaInvitacionArticulosPorConvertir().ToList();
            }
        }
    }
}