using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacenBL.Services.Compras
{
    public class OrdenCompraService : BaseService<tblOrdenCompra>
    {
        public override tblOrdenCompra BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblOrdenCompra.Where(m => m.OrdenCompraId == id).FirstOrDefault();
            }
        }

        public List<tblOrdenCompra> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblOrdenCompra.OrderByDescending(m => m.OrdenCompraId).ToList();
            }
        }

        public List<ARvwListadoOrdenCompra> BuscaListado()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARvwListadoOrdenCompra.OrderByDescending(m => m.OrdenCompraId).ToList();
            }
        }

        public override tblOrdenCompra Inserta(tblOrdenCompra entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            tblOrdenCompra ordenCompra = context.tblOrdenCompra.Add(entidad);

            //Validamos que exista un periodo abierto para la OC
            if (!new SistemaService().RevisarPeriodoAbierto(entidad.Fecha.Year, entidad.Fecha.Month, "P"))
            {
                throw new Exception("No existe un periodo abierto para la Orden de Compra. Favor de revisar.");
            }

            //Guardamos cambios
            context.SaveChanges();

            //Actualizamos la póliza
            new PolizaService().ActualizaPolizaGastoComprometido(ordenCompra.OrdenCompraId, "A", entidad.Fecha, context);

            //Retornamos si se guardó correctamente
            return ordenCompra;
        }

        public override bool Actualiza(tblOrdenCompra entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            tblOrdenCompra ordenCompra = context.tblOrdenCompra.Add(entidad);

            //Validamos si es cancelación
            bool esCancelación = ordenCompra.Status.Equals(EstatusOrdenCompra.CANCELADA);

            //Validamos que exista un periodo abierto para la OC
            if (!new SistemaService().RevisarPeriodoAbierto(esCancelación ? entidad.FechaCancelacion.GetValueOrDefault().Year : entidad.Fecha.Year,
                                                            esCancelación ? entidad.FechaCancelacion.GetValueOrDefault().Month : entidad.Fecha.Month,
                                                            "P"))
            {
                throw new Exception("No existe un periodo abierto para la " + (esCancelación ? "Cancelación de " : "") + "OC. Favor de revisar.");
            }

            //Marcamos el modelo como modificado
            context.Entry(entidad).State = EntityState.Modified;

            //Marcar todas las propiedades que no se pueden actualizar como FALSE
            //para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in tblOrdenCompra.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            //Guardamos cambios
            context.SaveChanges();

            //Actualizamos la póliza
            new PolizaService().ActualizaPolizaGastoComprometido(ordenCompra.OrdenCompraId,
                                                                    esCancelación ? "C" : "M",
                                                                    esCancelación && entidad.FechaCancelacion != null ? entidad.FechaCancelacion : ordenCompra.Fecha, context);

            return true;
        }

        public void GuardaDetalles(int ordenCompraId, List<tblOrdenCompraDet> detalles, SAACGContext context)
        {
            foreach (tblOrdenCompraDet detalle in detalles)
            {
                //Asignamos el Id de la cabecera
                detalle.OrdenCompraId = ordenCompraId;

                //Agregamos los detalles al Context
                context.tblOrdenCompraDet.Add(detalle);

                if (detalle.OrdenCompraDetId > 0)
                {
                    context.Entry(detalle).State = EntityState.Modified;

                    //Marcar todas las propiedades que no se pueden actualizar como FALSE
                    //para que no se actualice su informacion en Base de Datos
                    foreach (string propertyName in tblOrdenCompraDet.PropiedadesNoActualizables)
                    {
                        context.Entry(detalle).Property(propertyName).IsModified = false;
                    }
                }
            }

            //Guardamos cambios
            context.SaveChanges();
        }

        public int GuardaCambios(tblOrdenCompra entidad, List<tblOrdenCompraDet> detalles)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Creamos el Id de la cabecera
                    int ordenCompraId = entidad.OrdenCompraId;

                    //Validamos si es un registro nuevo
                    if (entidad.OrdenCompraId == 0)
                    {
                        ordenCompraId = Inserta(entidad, Context).OrdenCompraId;
                    }
                    else
                    {
                        Actualiza(entidad, Context);
                    }

                    //Guardamos cambios
                    Context.SaveChanges();

                    //Guardamos los detalles
                    if (detalles != null)
                    {
                        GuardaDetalles(ordenCompraId, detalles, Context);
                    }

                    //Actualizamos el estatus de la OC
                    Context.ARspActualizaEstatusOrdenCompra(ordenCompraId);

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    //Retornamos la entidad que se acaba de guardar en la Base de Datos
                    return ordenCompraId;
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

        public List<ARspConsultaOrdenCompraProductos_Result> BuscaComboProductos(string almacenId, string dependenciaId, string proyectoId, string ramoId)
        {
            return BuscaComboProductos(almacenId, dependenciaId, proyectoId, ramoId, null);
        }

        public List<ARspConsultaOrdenCompraProductos_Result> BuscaComboProductos(string almacenId, string productoId)
        {
            return BuscaComboProductos(almacenId, null, null, null, productoId);
        }

        public List<ARspConsultaOrdenCompraProductos_Result> BuscaComboProductos(string almacenId, string dependenciaId, string proyectoId, string ramoId, string productoId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaOrdenCompraProductos(almacenId, dependenciaId, proyectoId, ramoId, productoId).ToList();
            }
        }

        public List<ARspConsultaOrdenCompraDetalles_Result> BuscaDetallesPorOrdenCompraId(int ordenCompraId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaOrdenCompraDetalles(ordenCompraId).ToList();
            }
        }

        public List<tblOrdenCompraDet> BuscaDetallesNoCancelados(int ordenCompraId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblOrdenCompraDet.Where(m => m.OrdenCompraId == ordenCompraId).ToList();
            }
        }

        public string GetNombreCompletoEmpleado(int empleadoId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.RHspGetNombreCompletoEmpleado(empleadoId).FirstOrDefault();
            }
        }

        public ARspConsultaDatosFinanciamientoOrdenCompra_Result GetDatosFinanciamiento(int ordenCompraId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaDatosFinanciamientoOrdenCompra(ordenCompraId).FirstOrDefault();
            }
        }

        public bool ARspValidarRequisicionOC(int ordenCompraId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspValidarRequisicionOC(ordenCompraId).FirstOrDefault() != null;
            }
        }
    }
}