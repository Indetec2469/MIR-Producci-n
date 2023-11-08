using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacenBL.Services
{
    public class PlanDesarrolloService : BaseService<MItblPlanDesarrollo>
    {
        public override MItblPlanDesarrollo Inserta(MItblPlanDesarrollo entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            MItblPlanDesarrollo planNacionalDesarrollo = context.MItblPlanDesarrollo.Add(entidad);

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return planNacionalDesarrollo;
        }

        public override bool Actualiza(MItblPlanDesarrollo entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            context.MItblPlanDesarrollo.Add(entidad);

            // Marcamos la entidad como modificada
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in MItblPlanDesarrollo.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos true o false si se realizo correctamente la operacion
            return true;
        }

        public void GuardaCambios(MItblPlanDesarrollo planDesarrollo, IEnumerable<MItblPlanDesarrolloEstructura> listaPlanDesarrolloEstructura)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    // Iniciamos la transacción
                    Context.Database.BeginTransaction();
                   
                    // Creamos el return
                    MItblPlanDesarrollo _planDesarrollo = planDesarrollo;

                    // Existe la entidad para guardar o actualizar
                    if (_planDesarrollo != null)
                    {
                        // Si es un registro nuevo
                        if (_planDesarrollo.PlanDesarrolloId > 0)
                        {
                            Actualiza(_planDesarrollo, Context);
                        }
                        else
                        {
                            Inserta(_planDesarrollo, Context);
                        }
                    }
                    
                    // Existe la lista para guardar o actualizar
                    if(listaPlanDesarrolloEstructura != null)
                    {
                        GuardaListaPlanDesarrolloEstructura(_planDesarrollo, listaPlanDesarrolloEstructura, Context);
                    }

                    // Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    // Retornamos la entidad que se acaba de guardar en la Base de Datos
                    // return planDesarrollo;
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

        public void GuardaListaPlanDesarrolloEstructura(MItblPlanDesarrollo planDesarrollo, IEnumerable<MItblPlanDesarrolloEstructura> listaPlanDesarrolloEstructura, SAACGContext context)
        {
            // Service
            PlanDesarrolloEstructuraService planDesarrolloEstructuraService = new PlanDesarrolloEstructuraService();

            foreach (MItblPlanDesarrolloEstructura planDesarrolloEstructura in listaPlanDesarrolloEstructura.ToList())
            {
                // el ID Padre
                int idPadre = planDesarrolloEstructura.PlanDesarrolloEstructuraId;

                // Creamos el return
                MItblPlanDesarrolloEstructura _planDesarrolloEstructura = planDesarrolloEstructura;

                // Si es un registro
                if (_planDesarrolloEstructura.PlanDesarrolloEstructuraId > 0)
                {
                    // Actualizamos
                    planDesarrolloEstructuraService.Actualiza(_planDesarrolloEstructura, context);
                }
                else
                {
                    if (planDesarrollo != null)
                    {
                        if (_planDesarrolloEstructura.PlanDesarrolloId <= 0)
                        {
                            // Asignamos el Id de la cabecera
                            _planDesarrolloEstructura.PlanDesarrolloId = planDesarrollo.PlanDesarrolloId;
                        }
                    }
                    // Guardamos
                    _planDesarrolloEstructura = planDesarrolloEstructuraService.Inserta(_planDesarrolloEstructura, context);
                }

                // Cambiamos el ID del padre para estructuras
                if(idPadre < 0)
                {
                    listaPlanDesarrolloEstructura.Where(pde => pde.EstructuraPadreId == idPadre).ToList().ForEach(pde =>
                    {
                        pde.EstructuraPadreId = _planDesarrolloEstructura.PlanDesarrolloEstructuraId;
                    });
                }
            }
            
        }

        //public MItblPlanDesarrolloEstructura GuardaPlanDesarrolloEstructura(MItblPlanDesarrolloEstructura entidad)
        //{
        //    using (var Context = SAACGContextHelper.GetContext())
        //    {
        //        // Agregamos los detalles al Context
        //        MItblPlanDesarrolloEstructura __planDesarrolloEstructura = Context.MItblPlanDesarrolloEstructura.Add(entidad);

        //        // Si es un registro que se va actualizar
        //        if (__planDesarrolloEstructura.PlanDesarrolloEstructuraId > 0)
        //        {
        //            Context.Entry(__planDesarrolloEstructura).State = EntityState.Modified;

        //            // Marcar todas las propiedades que no se pueden actualizar como FALSE
        //            // para que no se actualice su informacion en Base de Datos
        //            foreach (string propertyName in MItblPlanDesarrolloEstructura.PropiedadesNoActualizables)
        //            {
        //                Context.Entry(__planDesarrolloEstructura).Property(propertyName).IsModified = false;
        //            }
        //        }

        //        // Guardamos cambios
        //        Context.SaveChanges();

        //        return __planDesarrolloEstructura;
        //    }
        //}

        public override MItblPlanDesarrollo BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.MItblPlanDesarrollo.Where(m => m.PlanDesarrolloId == id).FirstOrDefault();
            }
        }

        public bool ExistePorNombre(int id, string nombrePlan)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.MItblPlanDesarrollo.Where(m => m.PlanDesarrolloId != id 
                                                           && m.NombrePlan.Equals(nombrePlan)
                                                           && m.EstatusId == EstatusRegistro.ACTIVO).FirstOrDefault() != null;
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MIvwListadoPlanNacionalDesarrollo> BuscaListado()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MIvwListadoPlanNacionalDesarrollo.ToList();
            }
        }

        public List<MItblPlanDesarrollo> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblPlanDesarrollo.Where(pnd => pnd.EstatusId == EstatusRegistro.ACTIVO).ToList();
            }
        }

        public List<MItblPlanDesarrollo> BuscaActivos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblPlanDesarrollo.Where(pnd => pnd.EstatusId == EstatusRegistro.ACTIVO).ToList();
            }
        }

        public List<MItblPlanDesarrollo> BuscaActivosPorEjercicio(int ejercicio)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblPlanDesarrollo.Where(m => 
                       m.EstatusId == EstatusRegistro.ACTIVO
                    && ejercicio >= m.FechaInicio.Year
                    && ejercicio <=  m.FechaFin.Year
                ).ToList();
            }
        }

        //public List<spConsultaExistenciaAlmacen_Result> ConsultaExistenciaAlmacen(string almacenId, string productosIds)
        //{
        //    using (var Context = new EntityContext())
        //    {
        //        return Context.spConsultaExistenciaAlmacen(almacenId, productosIds).ToList();
        //    }
        //}

        /*public List<PlanNacionalDesarrolloEstructura> BuscaDetallesPorPlanNacionalDesarrolloId(int planId)
        {
            using (var Context = new RequisicionesAlmacenContext())
            {
                return Context.spConsultaPlanNacionalDesarrolloEstructuras(planId).ToList();
            }
        }*/
    }
}