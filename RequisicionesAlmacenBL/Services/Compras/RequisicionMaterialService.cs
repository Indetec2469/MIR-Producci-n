using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.Compras
{
    public class RequisicionMaterialService : BaseService<ARtblRequisicionMaterial>
    {
        public override ARtblRequisicionMaterial BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblRequisicionMaterial.Where(m => m.RequisicionMaterialId == id).FirstOrDefault();
            }
        }

        public List<ARtblRequisicionMaterial> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblRequisicionMaterial.ToList();
            }
        }

        public List<ARvwListadoRequisicionMaterial> BuscaListado(int usuarioId, bool isPermisoTerceros)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARvwListadoRequisicionMaterial.Where(m => m.UsuarioId == usuarioId || (isPermisoTerceros && m.CreadoPorId == usuarioId)).OrderByDescending(m => m.CodigoRequisicion).ToList();
            }
        }

        public override ARtblRequisicionMaterial Inserta(ARtblRequisicionMaterial entidad, SAACGContext context)
        {
            //Asignamos el autonumerico
            entidad.CodigoRequisicion = new AutonumericoService().GetSiguienteAutonumerico("Solicitud de Materiales y Consumibles", entidad.FechaRequisicion.Year, context);

            //Agregamos la entidad el Context
            ARtblRequisicionMaterial requisicionMaterial = context.ARtblRequisicionMaterial.Add(entidad);

            //Validamos que exista un periodo abierto
            if (!new SistemaService().RevisarPeriodoAbierto(entidad.FechaRequisicion.Year, entidad.FechaRequisicion.Month, "P"))
            {
                throw new Exception("No existe un periodo abierto para la Requisición. Favor de revisar.");
            }

            //Guardamos cambios
            context.SaveChanges();

            //Retornamos si guardó correctamente
            return requisicionMaterial;
        }

        public override bool Actualiza(ARtblRequisicionMaterial entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            ARtblRequisicionMaterial requisicionMaterial = context.ARtblRequisicionMaterial.Add(entidad);

            //Validamos que exista un periodo abierto
            if (!new SistemaService().RevisarPeriodoAbierto(entidad.FechaRequisicion.Year, entidad.FechaRequisicion.Month, "P"))
            {
                throw new Exception("No existe un periodo abierto para la Requisición. Favor de revisar.");
            }

            //Marcamos el modelo como modificado
            context.Entry(entidad).State = EntityState.Modified;

            //Marcar todas las propiedades que no se pueden actualizar como FALSE
            //para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in ARtblRequisicionMaterial.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }
            
            //Guardamos cambios
            return context.SaveChanges() > 0;
        }

        public void GuardaDetalles(List<ARtblRequisicionMaterialDetalle> detalles, SAACGContext context)
        {
            GuardaDetalles(null, detalles, context);
        }

        public void GuardaDetalles(Nullable<int> requisicionId, List<ARtblRequisicionMaterialDetalle> detalles, SAACGContext context)
        {
            foreach (ARtblRequisicionMaterialDetalle detalle in detalles)
            {
                //Asignamos el Id de la cebecera
                if (requisicionId != null)
                {
                    detalle.RequisicionMaterialId = requisicionId.GetValueOrDefault();
                }

                //Agregamos los detalles al Context
                context.ARtblRequisicionMaterialDetalle.Add(detalle);

                if (detalle.RequisicionMaterialDetalleId > 0)
                {
                    context.Entry(detalle).State = EntityState.Modified;

                    //Marcar todas las propiedades que no se pueden actualizar como FALSE
                    //para que no se actualice su informacion en Base de Datos
                    foreach (string propertyName in ARtblRequisicionMaterialDetalle.PropiedadesNoActualizables)
                    {
                        context.Entry(detalle).Property(propertyName).IsModified = false;
                    }
                }
            }

            //Guardamos cambios
            context.SaveChanges();
        }

        public int GuardaCambios(ARtblRequisicionMaterial entidad, List<ARtblRequisicionMaterialDetalle> detalles)
        {
            return GuardaCambios(entidad, detalles, false);
        }

        public int GuardaCambios(ARtblRequisicionMaterial entidad, List<ARtblRequisicionMaterialDetalle> detalles, bool enviarAutorizar)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Creamos el Id de la cabecera
                    int requisicionId = entidad.RequisicionMaterialId;

                    //Validamos si es un registro nuevo
                    if (entidad.RequisicionMaterialId == 0)
                    {
                        requisicionId = Inserta(entidad, Context).RequisicionMaterialId;
                    }
                    else
                    {
                        Actualiza(entidad, Context);
                    }

                    //Guardamos los detalles
                    if (detalles != null)
                    {
                        GuardaDetalles(requisicionId, detalles, Context);
                    }

                    if (enviarAutorizar)
                    {
                        int alertaId = new AlertaService().IniciarAlerta(
                            AlertaDefinicion.REQUISICION_MATERIA_AUTORIZACION,
                            requisicionId,
                            entidad.CodigoRequisicion,
                            "Requisición: " + entidad.CodigoRequisicion,
                            entidad.ModificadoPorId.GetValueOrDefault(),
                            Context);
                    }

                    //Actualizamos el estatus de la Requisición
                    Context.ARspActualizaEstatusRequisicionMaterial(requisicionId);

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    //Retornamos la entidad que se acaba de guardar en la Base de Datos
                    return requisicionId;
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

        public List<ARspConsultaRequisicionMaterialProductos_Result> BuscaComboProductos(string areaId, string unidadAdministrativaId, string proyectoId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaRequisicionMaterialProductos(areaId, unidadAdministrativaId, proyectoId).ToList();
            }
        }

        public ARtblRequisicionMaterialDetalle BuscaDetallePorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblRequisicionMaterialDetalle.Where(m => m.RequisicionMaterialDetalleId == id).FirstOrDefault();
            }
        }

        public List<ARspConsultaRequisicionMaterialDetalles_Result> BuscaDetallesPorRequisicionMaterialId(int requisicionId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaRequisicionMaterialDetalles(requisicionId).ToList();
            }
        }

        public string GetNombreCompletoEmpleado(int empleadoId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.RHspGetNombreCompletoEmpleado(empleadoId).FirstOrDefault();
            }
        }

        public void Autorizar(int requisicionMaterialId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    Context.ARspRequisicionMaterialAutorizar(requisicionMaterialId);

                    //Guardamos cambios
                    Context.SaveChanges();

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