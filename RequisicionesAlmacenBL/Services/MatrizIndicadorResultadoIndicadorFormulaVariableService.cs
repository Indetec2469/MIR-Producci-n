using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Services
{
    public class MatrizIndicadorResultadoIndicadorFormulaVariableService : BaseService<MatrizIndicadorResultadoIndicadorFormulaVariable>
    {
        public override bool Actualiza(MatrizIndicadorResultadoIndicadorFormulaVariable entidad)
        {
            using (var Context = new RequisicionesAlmacenContext())
            {
                using (var ContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        // Agregamos la entidad que vamos a actualizar al Context
                        Context.MatrizIndicadorResultadoIndicadorFormulaVariable.Add(entidad);
                        Context.Entry(entidad).State = EntityState.Modified;

                        // Marcar todas las propiedades que no se pueden actualizar como FALSE
                        // para que no se actualice su informacion en Base de Datos
                        foreach (string propertyName in MatrizIndicadorResultadoIndicadorFormulaVariable.PropiedadesNoActualizables)
                        {
                            Context.Entry(entidad).Property(propertyName).IsModified = false;
                        }

                        // Guardamos cambios
                        Context.SaveChanges();

                        // Hacemos el Commit
                        ContextTransaction.Commit();

                        // Retornamos true o false si se realizo correctamente la operacion
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // Hacemos el Rollback
                        ContextTransaction.Rollback();

                        throw new Exception("Error al guardar: " + ex);
                    }
                }
            }
        }

        public override MatrizIndicadorResultadoIndicadorFormulaVariable BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId)
        {
            throw new NotImplementedException();
        }

        public override MatrizIndicadorResultadoIndicadorFormulaVariable Inserta(MatrizIndicadorResultadoIndicadorFormulaVariable entidad)
        {
            using (var Context = new RequisicionesAlmacenContext())
            {
                using (var ContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        // Agregamos la entidad el Context
                        MatrizIndicadorResultadoIndicadorFormulaVariable matrizIndicadorResultadoIndicadorFormulaVariable = Context.MatrizIndicadorResultadoIndicadorFormulaVariable.Add(entidad);

                        // Guardamos cambios
                        Context.SaveChanges();

                        // Hacemos el Commit
                        ContextTransaction.Commit();

                        // Retornamos la entidad que se acaba de guardar en la Base de Datos
                        return matrizIndicadorResultadoIndicadorFormulaVariable;
                    }
                    catch (Exception ex)
                    {
                        // Hacemos el Rollback
                        ContextTransaction.Rollback();

                        throw new Exception("Error al guardar: " + ex);
                    }
                }
            }
        }

        public IEnumerable<MatrizIndicadorResultadoIndicadorFormulaVariable> BuscaPorMIRIndicadorId(int mirIndicadorId)
        {
            using (var Context = new RequisicionesAlmacenContext())
            {
                return Context.MatrizIndicadorResultadoIndicadorFormulaVariable.Where(mirim => mirim.MIRIndicadorId == mirIndicadorId && mirim.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).ToList();
            }
        }
    }
}
