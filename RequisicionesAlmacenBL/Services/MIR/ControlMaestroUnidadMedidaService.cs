using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace RequisicionesAlmacenBL.Services
{
    public class ControlMaestroUnidadMedidaService : BaseService<MItblControlMaestroUnidadMedida>
    {
        //private static string INCLUDE_CMUMFV = "ControlMaestroUnidadMedidaFormulaVariable";
        public override bool Actualiza(MItblControlMaestroUnidadMedida entidad, SAACGContext context)
        {
            // Agregamos la entidad que vamos a actualizar al Context
            context.MItblControlMaestroUnidadMedida.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in MItblControlMaestroUnidadMedida.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos true o false si se realizo correctamente la operacion
            return true;
        }

        public override MItblControlMaestroUnidadMedida BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.MItblControlMaestroUnidadMedida.Find(id);
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override MItblControlMaestroUnidadMedida Inserta(MItblControlMaestroUnidadMedida entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            MItblControlMaestroUnidadMedida controlMaestroUnidadMedida = context.MItblControlMaestroUnidadMedida.Add(entidad);

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return controlMaestroUnidadMedida;           
        }

        public void GuardaCambios(IEnumerable<MItblControlMaestroUnidadMedida> listaUnidadMedida, IEnumerable<MItblControlMaestroUnidadMedidaDimension> listaUnidadMedidaDimension, IEnumerable<MItblControlMaestroUnidadMedidaFormulaVariable> listaUnidadMedidaFormulaVariable)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    // Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    if(listaUnidadMedida != null)
                    {
                        listaUnidadMedida.ToList().ForEach(unidadMedida =>
                        {
                            // Id Padre (pre el ID de Padre)
                            int idPadre = unidadMedida.UnidadMedidaId;

                            // Creamos el return
                            MItblControlMaestroUnidadMedida _unidadMedida = unidadMedida;

                            // Si es un registro nuevo
                            if (_unidadMedida.UnidadMedidaId > 0)
                            {
                                Actualiza(_unidadMedida, Context);
                            }
                            else
                            {
                                _unidadMedida = Inserta(_unidadMedida, Context);
                            }

                            // Existe la lista para guardar o actualizar
                            if(listaUnidadMedidaDimension != null)
                            {
                                if (listaUnidadMedidaDimension.Where(umd => umd.UnidadMedidaId == idPadre).ToList().Count > 0)
                                {
                                    GuardaListaUnidadMedidaDimension(_unidadMedida, listaUnidadMedidaDimension.Where(umd => umd.UnidadMedidaId == idPadre), Context);
                                }
                            }

                            // Existe la lista para guardar o actualizar
                            if(listaUnidadMedidaFormulaVariable != null)
                            {
                                if (listaUnidadMedidaFormulaVariable.Where(umfv => umfv.UnidadMedidaId == idPadre).ToList().Count > 0)
                                {
                                    GuardaListaUnidadMedidaFormulaVariable(_unidadMedida, listaUnidadMedidaFormulaVariable.Where(umfv => umfv.UnidadMedidaId == idPadre), Context);
                                }
                            }
                        });
                    }
                    else
                    {
                        // Existe la lista para guardar o actualizar
                        if (listaUnidadMedidaDimension != null)
                        {
                            GuardaListaUnidadMedidaDimension(null, listaUnidadMedidaDimension, Context);
                        }

                        // Existe la lista para guardar o actualizar
                        if (listaUnidadMedidaFormulaVariable != null)
                        {
                            GuardaListaUnidadMedidaFormulaVariable(null, listaUnidadMedidaFormulaVariable, Context);
                        }
                    }

                    // Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();
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

        public void GuardaListaUnidadMedidaDimension(MItblControlMaestroUnidadMedida unidadMedida, IEnumerable<MItblControlMaestroUnidadMedidaDimension> listaUnidadMedidaDimension, SAACGContext context)
        {
            // Service
            ControlMaestroUnidadMedidaDimensionService unidadMedidaDimensionService = new ControlMaestroUnidadMedidaDimensionService();

            foreach (MItblControlMaestroUnidadMedidaDimension unidadMedidaDimension in listaUnidadMedidaDimension.ToList())
            {
                // Creamos el return
                MItblControlMaestroUnidadMedidaDimension _unidadMedidaDimension = unidadMedidaDimension;

                // Si es un registro
                if (_unidadMedidaDimension.UnidadMedidaDimensionId > 0)
                {
                    // Actualizamos
                    unidadMedidaDimensionService.Actualiza(_unidadMedidaDimension, context);
                }
                else
                {
                    if (unidadMedida != null)
                    {
                        if(_unidadMedidaDimension.UnidadMedidaId <= 0)
                        {
                            // Asignamos el Id de la cabecera
                            _unidadMedidaDimension.UnidadMedidaId = unidadMedida.UnidadMedidaId;
                        }
                    }
                    // Guardamos
                    _unidadMedidaDimension = unidadMedidaDimensionService.Inserta(_unidadMedidaDimension, context);
                }
            }
        }

        public void GuardaListaUnidadMedidaFormulaVariable(MItblControlMaestroUnidadMedida unidadMedida, IEnumerable<MItblControlMaestroUnidadMedidaFormulaVariable> listaUnidadMedidaFormulaVariable, SAACGContext context)
        {
            // Service
            ControlMaestroUnidadMedidaFormulaVariableService unidadMedidaFormulaVariableService = new ControlMaestroUnidadMedidaFormulaVariableService();
           
            foreach (MItblControlMaestroUnidadMedidaFormulaVariable unidadMedidaFormulaVariable in listaUnidadMedidaFormulaVariable.ToList())
            {
                // Creamos el return
                MItblControlMaestroUnidadMedidaFormulaVariable _unidadMedidaFormulaVariable = unidadMedidaFormulaVariable;

                // Si es un registro
                if (_unidadMedidaFormulaVariable.UnidadMedidaFormulaVariableId > 0)
                {
                    // Actualizamos
                    unidadMedidaFormulaVariableService.Actualiza(_unidadMedidaFormulaVariable, context);
                }
                else
                {
                    if (unidadMedida != null)
                    {
                        if(_unidadMedidaFormulaVariable.UnidadMedidaId <= 0)
                        {
                            // Asignamos el Id de la cabecera
                            _unidadMedidaFormulaVariable.UnidadMedidaId = unidadMedida.UnidadMedidaId;
                        }
                    }
                    // Guardamos
                    _unidadMedidaFormulaVariable = unidadMedidaFormulaVariableService.Inserta(_unidadMedidaFormulaVariable, context);
                }
            }
        }

        public IEnumerable<MItblControlMaestroUnidadMedida> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblControlMaestroUnidadMedida.AsEnumerable().Select(um => new MItblControlMaestroUnidadMedida
                { 
                    UnidadMedidaId = um.UnidadMedidaId,
                    Nombre = um.Nombre,
                    Borrado = um.Borrado,
                    FechaCreacion = um.FechaCreacion,
                    CreadoPorId = um.CreadoPorId,
                    FechaUltimaModificacion = um.FechaUltimaModificacion,
                    ModificadoPorId = um.ModificadoPorId,
                    Formula = um.Formula,
                    Sistema = um.Sistema,
                    Timestamp = um.Timestamp
                }).Where(um => um.Borrado == false).ToList();
            }
        }

        public IEnumerable<MItblControlMaestroUnidadMedida> BuscaTodos2()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblControlMaestroUnidadMedida.ToList();
            }
        }

        public Boolean EsNombreExiste(MItblControlMaestroUnidadMedida controlMaestroUnidadMedida)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblControlMaestroUnidadMedida.Any(um => um.Nombre == controlMaestroUnidadMedida.Nombre && um.UnidadMedidaId != controlMaestroUnidadMedida.UnidadMedidaId && um.Borrado == false);
            }
        }

        public IEnumerable<MIspConsultaUnidadMedidaConDimension_Result> BuscaTodosConDimension()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MIspConsultaUnidadMedidaConDimension().ToList();
            }
        }
    }
}
