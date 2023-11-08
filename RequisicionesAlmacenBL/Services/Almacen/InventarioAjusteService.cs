using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Services.SAACG;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacenBL.Services
{
    public class InventarioAjusteService : BaseService<ARtblInventarioAjuste>
    {
        public override ARtblInventarioAjuste Inserta(ARtblInventarioAjuste entidad, SAACGContext context)
        {
            //Asignamos el autonumerico
            entidad.CodigoAjusteInventario = new AutonumericoService().GetSiguienteAutonumerico("Ajuste de Inventario", context);

            //Agregamos la entidad el Context
            ARtblInventarioAjuste inventarioAjuste = context.ARtblInventarioAjuste.Add(entidad);

            //Guardamos cambios
            context.SaveChanges();

            //Retornamos el objeto registrado
            return inventarioAjuste;
            
        }

        public override bool Actualiza(ARtblInventarioAjuste entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            context.ARtblInventarioAjuste.Add(entidad);

            //Marcamos la entidad como modificada
            context.Entry(entidad).State = EntityState.Modified;

            //Marcar todas las propiedades que no se pueden actualizar como FALSE
            //para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in ARtblInventarioAjuste.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            //Guardamos cambios
            context.SaveChanges();
                
            //Retornamos true o false si se realizo correctamente la operacion
            return true;
        }

        public ARtblInventarioAjusteDetalle GuardaDetalle(ARtblInventarioAjusteDetalle entidad, SAACGContext context)
        {
            //Agregamos los detalles al Context
            ARtblInventarioAjusteDetalle detalle = context.ARtblInventarioAjusteDetalle.Add(entidad);

            //Si es un registro que se va actualizar
            if (detalle.InventarioAjusteDetalleId > 0)
            {
                context.Entry(detalle).State = EntityState.Modified;

                //Marcar todas las propiedades que no se pueden actualizar como FALSE
                //para que no se actualice su informacion en Base de Datos
                foreach (string propertyName in ARtblInventarioAjusteDetalle.PropiedadesNoActualizables)
                {
                    context.Entry(detalle).Property(propertyName).IsModified = false;
                }
            }

            //Guardamos cambios
            context.SaveChanges();

            return detalle;
            
        }

        public void GuardaDetalles(int inventarioAjusteId, List<ARtblInventarioAjusteDetalle> detalles, SAACGContext context)
        {
            foreach (ARtblInventarioAjusteDetalle detalleTemp in detalles)
            {
                //Asignamos el Id de la cabecera
                detalleTemp.InventarioAjusteId = inventarioAjusteId;

                //Guardamos el detalle
                ARtblInventarioAjusteDetalle detalle = GuardaDetalle(detalleTemp, context);

                detalle.NombreArchivoTmp = detalleTemp.NombreArchivoTmp;

                if (detalle.NombreArchivoTmp != null)
                {
                    GuardaArchivo(detalle, context);
                }
            }
        }

        public int GuardaCambios(ARtblInventarioAjuste entidad, List<ARtblInventarioAjusteDetalle> detalles)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Si es un registro nuevo
                    if (entidad.InventarioAjusteId == 0)
                    {
                        entidad = Inserta(entidad, Context);
                    }
                    else
                    {
                        Actualiza(entidad, Context);
                    }

                    //Creamos el return
                    int inventarioAjusteId = entidad.InventarioAjusteId;

                    //Guardamos los detalles
                    GuardaDetalles(inventarioAjusteId, detalles, Context);

                    //Afectamos el inventario
                    AfectaInventario(inventarioAjusteId, entidad.CreadoPorId, Context);

                    //Actualizamos la póliza
                    new PolizaService().ActualizaPolizaAjusteInventario(inventarioAjusteId, entidad.FechaCreacion, Context);

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    //Retornamos la entidad que se acaba de guardar en la Base de Datos
                    return inventarioAjusteId;
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

        public void AfectaInventario(int inventarioId, int usuarioId, SAACGContext context)
        {
            context.ARspAfectaInventarioAjuste(inventarioId, usuarioId);
        }

        private void GuardaArchivo(ARtblInventarioAjusteDetalle inventarioAjusteDetalle, SAACGContext context)
        {
            ArchivoService archivoService = new ArchivoService();

            string nombreArchivoTemporal = inventarioAjusteDetalle.NombreArchivoTmp;
            string extensionArchivo = nombreArchivoTemporal.Substring(nombreArchivoTemporal.LastIndexOf("."));
            int tipoArchivo = archivoService.ObtenerTipoArchivo(extensionArchivo);            
            int id = inventarioAjusteDetalle.InventarioAjusteId;

            Guid archivoId = archivoService.GuardaArchivo(inventarioAjusteDetalle.CreadoPorId,
                                                            null,
                                                            nombreArchivoTemporal,
                                                            null,
                                                            ListadoCMOA.EVIDENCIA_AJUSTE_INVENTARIO,
                                                            new List<string>() { id.ToString() },
                                                            id,
                                                            tipoArchivo,
                                                            context);

            inventarioAjusteDetalle.ArchivoId = archivoId;

            GuardaDetalle(inventarioAjusteDetalle, context);
        }        

        public override ARtblInventarioAjuste BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.ARtblInventarioAjuste.Where(m => m.InventarioAjusteId == id).FirstOrDefault();
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public List<ARvwListadoInventarioAjuste> BuscaListado()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARvwListadoInventarioAjuste.ToList();
            }
        }

        public List<ARspConsultaExistenciaAlmacen_Result> ConsultaExistenciaAlmacen(string almacenId, string productosIds)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaExistenciaAlmacen(almacenId, productosIds).ToList();
            }
        }

        public List<ARspConsultaInventarioAjusteDetalles_Result> BuscaDetallesPorInventarioAjusteId(int inventarioId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaInventarioAjusteDetalles(inventarioId).ToList();
            }
        }

        public ARtblInventarioAjusteDetalle BuscaDetallePorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.ARtblInventarioAjusteDetalle.Find(id);
            }
        }
    }
}