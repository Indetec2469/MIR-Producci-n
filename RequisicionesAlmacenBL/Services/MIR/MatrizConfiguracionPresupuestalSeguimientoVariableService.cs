using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.MIR
{
    public class MatrizConfiguracionPresupuestalSeguimientoVariableService : BaseService<MItblMatrizConfiguracionPresupuestalSeguimientoVariable>
    {
        public override bool Actualiza(MItblMatrizConfiguracionPresupuestalSeguimientoVariable entidad, SAACGContext context)
        {
            // Agregamos la entidad que vamos a actualizar al Context
            context.MItblMatrizConfiguracionPresupuestalSeguimientoVariable.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in MItblMatrizConfiguracionPresupuestalSeguimientoVariable.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos true o false si se realizo correctamente la operacion
            return true;
        }

        public override MItblMatrizConfiguracionPresupuestalSeguimientoVariable BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.MItblMatrizConfiguracionPresupuestalSeguimientoVariable.Find(id);
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override MItblMatrizConfiguracionPresupuestalSeguimientoVariable Inserta(MItblMatrizConfiguracionPresupuestalSeguimientoVariable entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            MItblMatrizConfiguracionPresupuestalSeguimientoVariable matrizConfiguracionPresupuestalSeguimientoVariable = context.MItblMatrizConfiguracionPresupuestalSeguimientoVariable.Add(entidad);

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return matrizConfiguracionPresupuestalSeguimientoVariable;
        }

        public void GuardaCambios(IEnumerable<MItblMatrizConfiguracionPresupuestalSeguimientoVariable> listaMatrizConfiguracionPresupuestalSeguimientoVariable,DateTime FechaModificacion)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    // Iniciamos la transacción
                    Context.Database.BeginTransaction();
                    MatrizConfiguracionPresupuestalSeguimientoVariableHService MatrizConfiguracionPresupuestalSeguimientoVariableServiceH = new MatrizConfiguracionPresupuestalSeguimientoVariableHService();

                    // Existe la lista para guardar o actualizar
                    if (listaMatrizConfiguracionPresupuestalSeguimientoVariable != null)
                    {
                        listaMatrizConfiguracionPresupuestalSeguimientoVariable.ToList().ForEach(sv =>
                        {
                            // Creamos el return
                            MItblMatrizConfiguracionPresupuestalSeguimientoVariable matrizConfiguracionPresupuestalSeguimientoVariable = sv;
                            MItblMatrizConfiguracionPresupuestalSeguimientoVariableH matrizConfiguracionPresupuestalSeguimientoVariableH = new MItblMatrizConfiguracionPresupuestalSeguimientoVariableH
                            {
                                MIRIndicadorFormulaVariableId=sv.MIRIndicadorFormulaVariableId,
                                Enero = sv.Enero,
                                Febrero = sv.Febrero,
                                Marzo = sv.Marzo,
                                Abril = sv.Abril,
                                Mayo = sv.Mayo,
                                Junio = sv.Junio,
                                Julio = sv.Julio,
                                Agosto = sv.Agosto,
                                Septiembre = sv.Septiembre,
                                Octubre = sv.Octubre,
                                Noviembre = sv.Noviembre,
                                Diciembre = sv.Diciembre,
                                EstatusId = sv.EstatusId,
                                FechaCreacion = DateTime.Now,
                                CreadoPorId = sv.CreadoPorId,
                                FechaUltimaModificacion = FechaModificacion,
                                ModificadoPorId = sv.ModificadoPorId,
                                Timestamp = sv.Timestamp

                            };

                            // Si es un registro nuevo
                            if (matrizConfiguracionPresupuestalSeguimientoVariable.MIRSeguimientoVariableId > 0)
                            {
                                Actualiza(matrizConfiguracionPresupuestalSeguimientoVariable, Context);
                            }
                            else
                            {
                                matrizConfiguracionPresupuestalSeguimientoVariable = Inserta(matrizConfiguracionPresupuestalSeguimientoVariable, Context);

                            }

                            matrizConfiguracionPresupuestalSeguimientoVariableH = MatrizConfiguracionPresupuestalSeguimientoVariableServiceH.Inserta(matrizConfiguracionPresupuestalSeguimientoVariableH, Context);

                        });
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

        public IEnumerable<MIvwListadoMatrizConfiguracionPresupuestalSeguimientoVariable> BuscaListado()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MIvwListadoMatrizConfiguracionPresupuestalSeguimientoVariable.ToList();
            }
        }

        public MItblMatrizConfiguracionPresupuestalSeguimientoVariable BuscaPorMIRIndicadorFormulaVariableId(int MIRIndicadorFormulaVariableId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblMatrizConfiguracionPresupuestalSeguimientoVariable.Where(sv => sv.MIRIndicadorFormulaVariableId == MIRIndicadorFormulaVariableId).FirstOrDefault();
            }
        }

        public IEnumerable<MIspConsultaListaVariableIndicador_Result> BuscaReportePorMIRId(int mirId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MIspConsultaListaVariableIndicador(mirId).ToList();
            }
        }

        public IEnumerable<MIfnObtenerSeguimientoIndicadorDesempenio_Result> BuscaReporteSIDPorMIRIndicadorId(int mirIndicadorId,DateTime fechaReporte)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MIfnObtenerSeguimientoIndicadorDesempenio(mirIndicadorId,fechaReporte).ToList();
            }
        }
    }
}
