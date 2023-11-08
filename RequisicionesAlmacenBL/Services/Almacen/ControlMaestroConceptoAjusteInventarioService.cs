using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Models.Mapeos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace RequisicionesAlmacenBL.Services
{
    public class ControlMaestroConceptoAjusteInventarioService : BaseService<ARtblControlMaestroConceptoAjusteInventario>
    {
        public override ARtblControlMaestroConceptoAjusteInventario Inserta(ARtblControlMaestroConceptoAjusteInventario entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            ARtblControlMaestroConceptoAjusteInventario controlMaestroConceptoAjusteInventario = context.ARtblControlMaestroConceptoAjusteInventario.Add(entidad);

            //Guardamos cambios
            context.SaveChanges();

            //Retornamos la entidad que se acaba de guardar en la Base de Datos
            return controlMaestroConceptoAjusteInventario;
            
        }

        public override bool Actualiza(ARtblControlMaestroConceptoAjusteInventario entidad, SAACGContext context)
        {

            //Agregamos la entidad que vamos a actualizar al Context
            context.ARtblControlMaestroConceptoAjusteInventario.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            //Marcar todas las propiedades que no se pueden actualizar como FALSE
            //para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in ARtblControlMaestroConceptoAjusteInventario.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            //Retornamos true o false si se realizo correctamente la operacion
            return context.SaveChanges() > 0;
        
        }

        public override ARtblControlMaestroConceptoAjusteInventario BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.ARtblControlMaestroConceptoAjusteInventario.Find(id);
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ARtblControlMaestroConceptoAjusteInventario> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                // Obtenemos todos los registros con activo
                return Context.ARtblControlMaestroConceptoAjusteInventario.Where(cai => cai.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).ToList();
            }
        }

        public List<ARtblControlMaestroConceptoAjusteInventario> BuscaConceptosAjustePorTipoMovimientoId(int tipoMovimientoId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblControlMaestroConceptoAjusteInventario.Where(m => m.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO && m.TipoMovimientoId == tipoMovimientoId).ToList();
            }
        }

        public void GuardaCambios(List<ARtblControlMaestroConceptoAjusteInventario> listConceptos)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    foreach (ARtblControlMaestroConceptoAjusteInventario concepto in listConceptos)
                    {
                        if (concepto.ConceptoAjusteInventarioId > 0)
                        {
                            Actualiza(concepto, Context);
                        }
                        else
                        {
                            Inserta(concepto, Context);
                        }
                    }

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();
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
    }
}