using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Data.Entity.Infrastructure;

namespace RequisicionesAlmacenBL.Services
{
    public class MatrizConfiguracionPresupuestalService : BaseService<MItblMatrizConfiguracionPresupuestal>
    {
        public override bool Actualiza(MItblMatrizConfiguracionPresupuestal entidad, SAACGContext context)
        {
            // Agregamos la entidad que vamos a actualizar al Context
            context.MItblMatrizConfiguracionPresupuestal.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in MItblMatrizConfiguracionPresupuestal.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos true o false si se realizo correctamente la operacion
            return true;
        }

        public override MItblMatrizConfiguracionPresupuestal BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.MItblMatrizConfiguracionPresupuestal.Find(id);
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override MItblMatrizConfiguracionPresupuestal Inserta(MItblMatrizConfiguracionPresupuestal entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            MItblMatrizConfiguracionPresupuestal matrizConfiguracionPresupuestal = context.MItblMatrizConfiguracionPresupuestal.Add(entidad);

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return matrizConfiguracionPresupuestal;
        }

        public void GuardaCambios(MItblMatrizConfiguracionPresupuestal matrizConfiguracionPresupuestal, IEnumerable<MItblMatrizConfiguracionPresupuestalDetalle> listaMatrizConfiguracionPresupuestalDetalle)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    // Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    // Creamos el return
                    MItblMatrizConfiguracionPresupuestal _matrizConfiguracionPresupuestal = matrizConfiguracionPresupuestal;

                    if (_matrizConfiguracionPresupuestal != null)
                    {
                        // Si es un registro nuevo
                        if (_matrizConfiguracionPresupuestal.ConfiguracionPresupuestoId > 0)
                        {
                            Actualiza(_matrizConfiguracionPresupuestal, Context);
                        }
                        else
                        {
                            _matrizConfiguracionPresupuestal = Inserta(_matrizConfiguracionPresupuestal, Context);
                            
                        }
                    }

                    // Existe la lista para guardar o actualizar
                    if (listaMatrizConfiguracionPresupuestalDetalle != null)
                    {
                        GuardaListaMatrizConfiguracionPresupuestalDetalle(_matrizConfiguracionPresupuestal, listaMatrizConfiguracionPresupuestalDetalle, Context);   
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

        public void GuardaListaMatrizConfiguracionPresupuestalDetalle(MItblMatrizConfiguracionPresupuestal matrizConfiguracionPresupuestal, IEnumerable<MItblMatrizConfiguracionPresupuestalDetalle> listaMatrizConfiguracionPresupuestalDetalle, SAACGContext context)
        {
            
            // Service
            MatrizConfiguracionPresupuestalDetalleService matrizConfiguracionPresupuestalDetalleService = new MatrizConfiguracionPresupuestalDetalleService();
            MatrizConfiguracionPresupuestalDetalleHService matrizConfiguracionPresupuestalDetalleServiceH = new MatrizConfiguracionPresupuestalDetalleHService();

            foreach (MItblMatrizConfiguracionPresupuestalDetalle matrizConfiguracionPresupuestalDetalle in listaMatrizConfiguracionPresupuestalDetalle.ToList())
            {
                // Creamos el return
                MItblMatrizConfiguracionPresupuestalDetalle _matrizConfiguracionPresupuestalDetalle = matrizConfiguracionPresupuestalDetalle;
                MItblMatrizConfiguracionPresupuestalDetalleH _matrizConfiguracionPresupuestalDetalleH = new MItblMatrizConfiguracionPresupuestalDetalleH
                {
                    ConfiguracionPresupuestoId = matrizConfiguracionPresupuestalDetalle.ConfiguracionPresupuestoId,
                    MIRIndicadorId = matrizConfiguracionPresupuestalDetalle.MIRIndicadorId,
                    Enero = matrizConfiguracionPresupuestalDetalle.Enero,
                    Febrero = matrizConfiguracionPresupuestalDetalle.Febrero,
                    Marzo = matrizConfiguracionPresupuestalDetalle.Marzo,
                    Abril = matrizConfiguracionPresupuestalDetalle.Abril,
                    Mayo = matrizConfiguracionPresupuestalDetalle.Mayo,
                    Junio = matrizConfiguracionPresupuestalDetalle.Junio,
                    Julio = matrizConfiguracionPresupuestalDetalle.Julio,
                    Agosto = matrizConfiguracionPresupuestalDetalle.Agosto,
                    Septiembre = matrizConfiguracionPresupuestalDetalle.Septiembre,
                    Octubre = matrizConfiguracionPresupuestalDetalle.Octubre,
                    Noviembre = matrizConfiguracionPresupuestalDetalle.Noviembre,
                    Diciembre = matrizConfiguracionPresupuestalDetalle.Diciembre,
                    Anual = matrizConfiguracionPresupuestalDetalle.Anual,
                    Porcentaje = matrizConfiguracionPresupuestalDetalle.Porcentaje,
                    EstatusId = matrizConfiguracionPresupuestalDetalle.EstatusId,
                    FechaCreacion = matrizConfiguracionPresupuestalDetalle.FechaCreacion,
                    CreadoPorId =matrizConfiguracionPresupuestalDetalle.CreadoPorId,
                    FechaUltimaModificacion = matrizConfiguracionPresupuestalDetalle.FechaUltimaModificacion,
                    ModificadoPorId = matrizConfiguracionPresupuestalDetalle.ModificadoPorId,
                    Timestamp = matrizConfiguracionPresupuestalDetalle.Timestamp
                };


                if (_matrizConfiguracionPresupuestalDetalle.ConfiguracionPresupuestoDetalleId > 0)
                {
                    // Actualizamos
                    matrizConfiguracionPresupuestalDetalleService.Actualiza(_matrizConfiguracionPresupuestalDetalle, context);
                }
                else
                {
                    if (matrizConfiguracionPresupuestal != null)
                    {
                        if(_matrizConfiguracionPresupuestalDetalle.ConfiguracionPresupuestoId <= 0)
                        {
                            // Asignamos el Id de la cabecera
                            _matrizConfiguracionPresupuestalDetalle.ConfiguracionPresupuestoId = matrizConfiguracionPresupuestal.ConfiguracionPresupuestoId;
                        }
                    }
                    // Guardamos
                    _matrizConfiguracionPresupuestalDetalle = matrizConfiguracionPresupuestalDetalleService.Inserta(_matrizConfiguracionPresupuestalDetalle, context);
                }
                _matrizConfiguracionPresupuestalDetalleH = matrizConfiguracionPresupuestalDetalleServiceH.Inserta(_matrizConfiguracionPresupuestalDetalleH, context);

            }

        }

        public IEnumerable<MIvwListadoMatrizConfiguracionPresupuestal> BuscaListado()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MIvwListadoMatrizConfiguracionPresupuestal.ToList();
            }
        }

        public IEnumerable<MIvwListadoMatrizPresupuestoVigente> BuscaListadoPresupuestoVigente()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MIvwListadoMatrizPresupuestoVigente.ToList();
            }
        }

        public IEnumerable<MIvwListadoMatrizPresupuestoDevengado> BuscaListadoPresupuestoDevengado()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MIvwListadoMatrizPresupuestoDevengado.ToList();
            }
        }

        public MItblMatrizConfiguracionPresupuestal BuscaPorMIRId(int mirId, int tipoPresupuestalId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblMatrizConfiguracionPresupuestal.Where(mcp => mcp.MIRId == mirId && mcp.ClasificadorId == tipoPresupuestalId && mcp.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).FirstOrDefault();
            }
        }

        public Boolean ExistePorMIR(int mirId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblMatrizConfiguracionPresupuestal.Any(mcp => mcp.MIRId == mirId && mcp.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO);
            }
        }

    }
}
