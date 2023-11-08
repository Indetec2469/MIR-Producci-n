using EntityFrameworkExtras.EF6;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacenBL.Services
{
    public class InventarioFisicoService : BaseService<ARtblInventarioFisico>
    {
        public override ARtblInventarioFisico Inserta(ARtblInventarioFisico entidad, SAACGContext context)
        {
            //Asignamos el autonumerico
            entidad.Codigo = new AutonumericoService().GetSiguienteAutonumerico("Inventarios Físicos", context);

            //Agregamos la entidad el Context
            ARtblInventarioFisico inventarioFisico = context.ARtblInventarioFisico.Add(entidad);

            //Guardamos cambios
            context.SaveChanges();

            //Retornamos el objeto registrado
            return inventarioFisico;
        }

        public override bool Actualiza(ARtblInventarioFisico entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            ARtblInventarioFisico inventarioFisico = context.ARtblInventarioFisico.Add(entidad);

            //Marcamos la entidad como modificada
            context.Entry(entidad).State = EntityState.Modified;

            //Marcar todas las propiedades que no se pueden actualizar como FALSE
            //para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in ARtblInventarioFisico.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            //Guardamos cambios
            context.SaveChanges();

            //Retornamos true o false si se realizo correctamente la operacion
            return true;            
        }

        public void GuardaDetalles(int inventarioFisicoId, List<ARtblInventarioFisicoDetalle> detalles, SAACGContext context)
        {

            foreach (ARtblInventarioFisicoDetalle detalle in detalles)
            {
                //Asignamos el Id de la cabecera
                detalle.InventarioFisicoId = inventarioFisicoId;

                //Agregamos los detalles al Context
                context.ARtblInventarioFisicoDetalle.Add(detalle);

                //Si es un registro que se va actualizar
                if (detalle.InventarioFisicoDetalleId > 0)
                {
                    context.Entry(detalle).State = EntityState.Modified;

                    //Marcar todas las propiedades que no se pueden actualizar como FALSE
                    //para que no se actualice su informacion en Base de Datos
                    foreach (string propertyName in ARtblInventarioFisicoDetalle.PropiedadesNoActualizables)
                    {
                        context.Entry(detalle).Property(propertyName).IsModified = false;
                    }
                }
            }

            //Guardamos cambios
            context.SaveChanges();
            
        }

        public int GuardaCambios(ARtblInventarioFisico entidad, List<ARtblInventarioFisicoDetalle> detalles)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Creamos el return
                    int inventarioFisicoId = entidad.InventarioFisicoId;

                    //Si es un registro nuevo
                    if (entidad.InventarioFisicoId == 0)
                    {
                        inventarioFisicoId = Inserta(entidad, Context).InventarioFisicoId;
                    }
                    else
                    {
                        Actualiza(entidad, Context);
                    }

                    //Guardamos los detalles
                    GuardaDetalles(inventarioFisicoId, detalles, Context);

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    //Retornamos la entidad que se acaba de guardar en la Base de Datos
                    return inventarioFisicoId;
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

        public override ARtblInventarioFisico BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.ARtblInventarioFisico.Where(m => 
                    m.InventarioFisicoId == id && 
                    (m.EstatusId == EstatusInventarioFisico.EN_PROCESO 
                        || m.EstatusId == EstatusInventarioFisico.TERMINADO)
                    ).FirstOrDefault();
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public void Elimina(ARtblInventarioFisico entidad)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    Actualiza(entidad, Context);

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

        public List<ARvwListadoInventarioFisico> BuscaListado()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARvwListadoInventarioFisico.ToList();
            }
        }

        public List<ARspConsultaExistenciaAlmacen_Result> ConsultaExistenciaAlmacen(string almacenId, string productosIds)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaExistenciaAlmacen(almacenId, productosIds).ToList();
            }
        }

        public List<ARspConsultaInventarioFisicoDetalles_Result> BuscaDetallesPorInventarioFisicoId(int inventarioId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaInventarioFisicoDetalles(inventarioId).ToList();
            }
        }

        public ARtblInventarioFisicoDetalle BuscaDetallePorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.ARtblInventarioFisicoDetalle.Find(id);
            }
        }

        public bool ExisteInventarioIniciado(string almacenId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblInventarioFisico.FirstOrDefault(m => 
                    m.AlmacenId == almacenId && 
                    m.EstatusId == EstatusInventarioFisico.EN_PROCESO) != null;
            }
        }

        public void AfectaInventario(int inventarioId, int usuarioId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    Context.ARspAfectaInventarioFisico(inventarioId, usuarioId);

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

        public void CargaInventarioInicial(List<ARudtImportarAlmacenProducto> importarAlmacenProducto, int usuarioId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Cargamos el inventario
                    var procedure = new ARspCargaInventarioInicial()
                    {
                        ImportarAlmacenProducto = importarAlmacenProducto,
                        UsuarioId = usuarioId
                    };

                    //Ejecutamos el procedure
                    Context.Database.ExecuteStoredProcedure(procedure);

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

        public string InventarioInicialValidarFila(string productoId, 
                                                   string almacenId, 
                                                   string fuenteFinanciamientoId, 
                                                   string proyectoId, 
                                                   string unidadAdministrativaId, 
                                                   string tipoGastoId, 
                                                   string cantidad, 
                                                   string costoUnitario)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspCargaInventarioInicialValidarFila(productoId,
                                                                     almacenId,
                                                                     fuenteFinanciamientoId,
                                                                     proyectoId,
                                                                     unidadAdministrativaId,
                                                                     tipoGastoId,
                                                                     cantidad,
                                                                     costoUnitario).FirstOrDefault();
            }
        }
    }
}