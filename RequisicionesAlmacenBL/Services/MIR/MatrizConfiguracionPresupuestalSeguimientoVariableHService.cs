using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.MIR
{
    public class MatrizConfiguracionPresupuestalSeguimientoVariableHService : BaseService<MItblMatrizConfiguracionPresupuestalSeguimientoVariableH>
    {
        public override bool Actualiza(MItblMatrizConfiguracionPresupuestalSeguimientoVariableH entidad, SAACGContext context)
        {
            // Agregamos la entidad que vamos a actualizar al Context
            context.MItblMatrizConfiguracionPresupuestalSeguimientoVariableH.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in MItblMatrizConfiguracionPresupuestalSeguimientoVariableH.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos true o false si se realizo correctamente la operacion
            return true;
        }

        public override MItblMatrizConfiguracionPresupuestalSeguimientoVariableH BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.MItblMatrizConfiguracionPresupuestalSeguimientoVariableH.Find(id);
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override MItblMatrizConfiguracionPresupuestalSeguimientoVariableH Inserta(MItblMatrizConfiguracionPresupuestalSeguimientoVariableH entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            MItblMatrizConfiguracionPresupuestalSeguimientoVariableH matrizConfiguracionPresupuestalSeguimientoVariable = context.MItblMatrizConfiguracionPresupuestalSeguimientoVariableH.Add(entidad);

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return matrizConfiguracionPresupuestalSeguimientoVariable;
        }

        public void GuardaCambios(IEnumerable<MItblMatrizConfiguracionPresupuestalSeguimientoVariableH> listaMatrizConfiguracionPresupuestalSeguimientoVariable)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    // Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    // Existe la lista para guardar o actualizar
                    if (listaMatrizConfiguracionPresupuestalSeguimientoVariable != null)
                    {
                        listaMatrizConfiguracionPresupuestalSeguimientoVariable.ToList().ForEach(sv =>
                        {
                            // Creamos el return
                            MItblMatrizConfiguracionPresupuestalSeguimientoVariableH matrizConfiguracionPresupuestalSeguimientoVariable = sv;

                            // Si es un registro nuevo
                            if (matrizConfiguracionPresupuestalSeguimientoVariable.MIRSeguimientoVariableHId > 0)
                            {
                                Actualiza(matrizConfiguracionPresupuestalSeguimientoVariable, Context);
                            }
                            else
                            {
                                matrizConfiguracionPresupuestalSeguimientoVariable = Inserta(matrizConfiguracionPresupuestalSeguimientoVariable, Context);

                            }
                            
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

        public MItblMatrizConfiguracionPresupuestalSeguimientoVariableH BuscaPorMIRIndicadorFormulaVariableId(int MIRIndicadorFormulaVariableId)
        {
            MItblMatrizConfiguracionPresupuestalSeguimientoVariableH matrizConfiguracionPresupuestalSeguimientoVariableH= new MItblMatrizConfiguracionPresupuestalSeguimientoVariableH();

            using (var Context = SAACGContextHelper.GetContext())
            {
                var tblMatrizH = Context.MItblMatrizConfiguracionPresupuestalSeguimientoVariableH
                 .Join(Context.MItblMatrizConfiguracionPresupuestalSeguimientoVariable,
                       p => p.MIRIndicadorFormulaVariableId,
                       e => e.MIRIndicadorFormulaVariableId,
                       (p, e) => new 
                       {
                           MIRSeguimientoVariableHId = e.MIRSeguimientoVariableId,
                           MIRIndicadorFormulaVariableId = p.MIRIndicadorFormulaVariableId,
                           Enero = p.Enero,
                           Febrero = p.Febrero,
                           Marzo = p.Marzo,
                           Abril = p.Abril,
                           Mayo = p.Mayo,
                           Junio = p.Junio,
                           Julio = p.Julio,
                           Agosto = p.Agosto,
                           Septiembre = p.Septiembre,
                           Octubre = p.Octubre,
                           Noviembre = p.Noviembre,
                           Diciembre = p.Diciembre,
                           EstatusId = p.EstatusId,
                           FechaCreacion = p.FechaCreacion,
                           CreadoPorId = p.CreadoPorId,
                           FechaUltimaModificacion = p.FechaUltimaModificacion,
                           ModificadoPorId = p.ModificadoPorId,
                           Timestamp = p.Timestamp

                       }).Where(sv => sv.MIRIndicadorFormulaVariableId == MIRIndicadorFormulaVariableId).OrderByDescending(x => x.FechaUltimaModificacion).FirstOrDefault();
                if (tblMatrizH != null)
                {

                   matrizConfiguracionPresupuestalSeguimientoVariableH = new MItblMatrizConfiguracionPresupuestalSeguimientoVariableH
                    {
                        MIRSeguimientoVariableHId = tblMatrizH.MIRSeguimientoVariableHId,
                        MIRIndicadorFormulaVariableId = tblMatrizH.MIRIndicadorFormulaVariableId,
                        Enero = tblMatrizH.Enero,
                        Febrero = tblMatrizH.Febrero,
                        Marzo = tblMatrizH.Marzo,
                        Abril = tblMatrizH.Abril,
                        Mayo = tblMatrizH.Mayo,
                        Junio = tblMatrizH.Junio,
                        Julio = tblMatrizH.Julio,
                        Agosto = tblMatrizH.Agosto,
                        Septiembre = tblMatrizH.Septiembre,
                        Octubre = tblMatrizH.Octubre,
                        Noviembre = tblMatrizH.Noviembre,
                        Diciembre = tblMatrizH.Diciembre,
                        EstatusId = tblMatrizH.EstatusId,
                        FechaCreacion = DateTime.Now,
                        CreadoPorId = tblMatrizH.CreadoPorId,
                        FechaUltimaModificacion = tblMatrizH.FechaUltimaModificacion,
                        ModificadoPorId = tblMatrizH.ModificadoPorId,
                        Timestamp = tblMatrizH.Timestamp

                    };
                }
                return matrizConfiguracionPresupuestalSeguimientoVariableH;//Context.MItblMatrizConfiguracionPresupuestalSeguimientoVariableH.Where(sv => sv.MIRIndicadorFormulaVariableId == MIRIndicadorFormulaVariableId).OrderByDescending(x => x.FechaUltimaModificacion).FirstOrDefault();
            }
        }

        public IEnumerable<MIspConsultaListaVariableIndicador_Result> BuscaReportePorMIRId(int mirId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MIspConsultaListaVariableIndicador(mirId).ToList();
            }
        }

        public IEnumerable<MIfnObtenerSeguimientoIndicadorDesempenio_Result> BuscaReporteSIDPorMIRIndicadorId(int mirIndicadorId, DateTime fechaReporte)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MIfnObtenerSeguimientoIndicadorDesempenio(mirIndicadorId, fechaReporte).ToList();
            }
        }
    }
}
