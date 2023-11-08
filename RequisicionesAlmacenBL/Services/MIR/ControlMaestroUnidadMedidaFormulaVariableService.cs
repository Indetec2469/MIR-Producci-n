using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RequisicionesAlmacenBL.Services
{
    public class ControlMaestroUnidadMedidaFormulaVariableService : BaseService<MItblControlMaestroUnidadMedidaFormulaVariable>
    {
        public override bool Actualiza(MItblControlMaestroUnidadMedidaFormulaVariable entidad, SAACGContext context)
        {
            // Agregamos la entidad que vamos a actualizar al Context
            context.MItblControlMaestroUnidadMedidaFormulaVariable.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in MItblControlMaestroUnidadMedidaFormulaVariable.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos true o false si se realizo correctamente la operacion
            return true;
        }

        public override MItblControlMaestroUnidadMedidaFormulaVariable BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.MItblControlMaestroUnidadMedidaFormulaVariable.Find(id);
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override MItblControlMaestroUnidadMedidaFormulaVariable Inserta(MItblControlMaestroUnidadMedidaFormulaVariable entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            MItblControlMaestroUnidadMedidaFormulaVariable controlMaestroUnidadMedidaFormulaVariable = context.MItblControlMaestroUnidadMedidaFormulaVariable.Add(entidad);

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return controlMaestroUnidadMedidaFormulaVariable;
        }

        public IEnumerable<MItblControlMaestroUnidadMedidaFormulaVariable> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblControlMaestroUnidadMedidaFormulaVariable.AsEnumerable().Select(fv => new MItblControlMaestroUnidadMedidaFormulaVariable
                {
                    UnidadMedidaFormulaVariableId = fv.UnidadMedidaFormulaVariableId,
                    UnidadMedidaId = fv.UnidadMedidaId,
                    Variable = fv.Variable,
                    Borrado = fv.Borrado,
                    FechaCreacion = fv.FechaCreacion,
                    CreadoPorId = fv.CreadoPorId,
                    FechaUltimaModificacion = fv.FechaUltimaModificacion,
                    ModificadoPorId = fv.ModificadoPorId,
                    Timestamp = fv.Timestamp
                }).Where(fv => fv.Borrado == false).ToList();
            }
        }

        public Boolean EsVariableExiste(MItblControlMaestroUnidadMedidaFormulaVariable formulaVariable)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblControlMaestroUnidadMedidaFormulaVariable.Any(fv => fv.Variable == formulaVariable.Variable && fv.UnidadMedidaId == formulaVariable.UnidadMedidaId && fv.UnidadMedidaFormulaVariableId != formulaVariable.UnidadMedidaFormulaVariableId && fv.Borrado == false);
            }
        }

        public IEnumerable<MItblControlMaestroUnidadMedidaFormulaVariable> BuscaPorUnidadMedidaFormulaVariableId(List<int> listaUnidadMedidaFormulaVariableId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblControlMaestroUnidadMedidaFormulaVariable.Where(fv => listaUnidadMedidaFormulaVariableId.Contains(fv.UnidadMedidaFormulaVariableId)).ToList();
            }
        }
    }
}
