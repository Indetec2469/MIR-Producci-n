using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacenBL.Services.Compras
{
    public class InvitacionCompraService : BaseService<ARtblInvitacionCompra>
    {
        public override ARtblInvitacionCompra BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblInvitacionCompra.Where(m => m.InvitacionCompraId == id).FirstOrDefault();
            }
        }

        public List<ARtblInvitacionCompra> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblInvitacionCompra.ToList();
            }
        }

        public List<ARvwListadoInvitacionCompra> BuscaListado()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARvwListadoInvitacionCompra.OrderByDescending(m => m.InvitacionCompraId).ToList();
            }
        }

        public override ARtblInvitacionCompra Inserta(ARtblInvitacionCompra entidad, SAACGContext context)
        {
            //Asignamos el autonumerico
            entidad.CodigoInvitacion = new AutonumericoService().GetSiguienteAutonumerico("Invitación de Compra", entidad.Ejercicio, context);

            //Agregamos la entidad el Context
            ARtblInvitacionCompra invitacionCompra = context.ARtblInvitacionCompra.Add(entidad);

            //Guardamos cambios
            context.SaveChanges();

            //Retornamos si guardó correctamente
            return invitacionCompra;
        }

        public override bool Actualiza(ARtblInvitacionCompra entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            ARtblInvitacionCompra invitacionCompra = context.ARtblInvitacionCompra.Add(entidad);

            //Marcamos el modelo como modificado
            context.Entry(entidad).State = EntityState.Modified;

            //Marcar todas las propiedades que no se pueden actualizar como FALSE
            //para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in ARtblInvitacionCompra.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            //Guardamos cambios
            return context.SaveChanges() > 0;
        }

        public void GuardaDetalles(int invitacionCompraId, List<ARtblInvitacionCompraDetalle> detalles, SAACGContext context)
        {
            foreach (ARtblInvitacionCompraDetalle detalle in detalles)
            {
                //Asignamos el Id de la cebecera
                detalle.InvitacionCompraId = invitacionCompraId;

                //Agregamos los detalles al Context
                context.ARtblInvitacionCompraDetalle.Add(detalle);

                if (detalle.InvitacionCompraDetalleId > 0)
                {
                    context.Entry(detalle).State = EntityState.Modified;

                    //Marcar todas las propiedades que no se pueden actualizar como FALSE
                    //para que no se actualice su informacion en Base de Datos
                    foreach (string propertyName in ARtblInvitacionCompraDetalle.PropiedadesNoActualizables)
                    {
                        context.Entry(detalle).Property(propertyName).IsModified = false;
                    }
                }
            }

            //Guardamos cambios
            context.SaveChanges();
        }

        public int GuardaCambios(ARtblInvitacionCompra entidad,
                                 List<ARtblInvitacionCompraDetalle> detalles,
                                 List<ARtblInvitacionArticuloDetalle> articulosConvertidos)
        {
            return GuardaCambios(entidad, detalles, articulosConvertidos, null, null, null, null, null);
        }

        public int GuardaCambios(ARtblInvitacionCompra entidad, 
                                 List<ARtblInvitacionCompraDetalle> detalles, 
                                 List<ARtblRequisicionMaterialDetalle> requisicionDetalles)
        {
            return GuardaCambios(entidad, detalles, null, null, null, null, requisicionDetalles, null);
        }

        public int GuardaCambios(ARtblInvitacionCompra entidad, 
                                 List<ARtblInvitacionCompraDetalle> detalles,
                                 List<ARtblInvitacionArticuloDetalle> articulosConvertidos,
                                 List<ARtblInvitacionCompraProveedor> proveedoresInvitados,
                                 List<ArtblInvitacionCompraDetallePrecioProveedor> preciosProveedor,
                                 List<ArtblInvitacionCompraProveedorCotizacion> cotizaciones,
                                 List<ARtblRequisicionMaterialDetalle> requisicionDetalles,
                                 List<int> prospectosProveedorGanadores)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Creamos el Id de la cabecera
                    int invitacionCompraId = entidad.InvitacionCompraId;

                    //Validamos si es un registro nuevo
                    if (entidad.InvitacionCompraId == 0)
                    {
                        invitacionCompraId = Inserta(entidad, Context).InvitacionCompraId;
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
                        GuardaDetalles(invitacionCompraId, detalles, Context);
                    }

                    //Actualizamos los artículos convertidos
                    if (articulosConvertidos != null && articulosConvertidos.Count > 0)
                    {
                        new InvitacionArticuloService().GuardaDetalles(articulosConvertidos[0].InvitacionArticuloId, articulosConvertidos, Context);
                    }

                    //Guardamos los Proveedores Invitados
                    if (proveedoresInvitados != null)
                    {
                        foreach (ARtblInvitacionCompraProveedor proveedor in proveedoresInvitados)
                        {
                            if (proveedor.InvitacionCompraProveedorId < 0)
                            {
                                new InvitacionCompraProveedorService().Inserta(proveedor, Context);
                            }
                            else
                            {
                                new InvitacionCompraProveedorService().Actualiza(proveedor, Context);
                            }
                        }
                    }

                    //Guardamos los Precios por Proveedor
                    if (preciosProveedor != null)
                    {
                        foreach (ArtblInvitacionCompraDetallePrecioProveedor precioProveedor in preciosProveedor)
                        {
                            if (precioProveedor.InvitacionCompraDetallePrecioProveedorId < 0)
                            {
                                new InvitacionCompraDetallePrecioProveedorService().Inserta(precioProveedor, Context);
                            }
                            else
                            {
                                new InvitacionCompraDetallePrecioProveedorService().Actualiza(precioProveedor, Context);
                            }
                        }
                    }

                    //Guardamos las cotizaciones de los proveedores
                    if (cotizaciones != null)
                    {
                        new InvitacionCompraProveedorCotizacionService().GuardaCambios(null, cotizaciones, Context);
                    }

                    //Actualizamos los detalles de la requisición en caso de Desierta
                    if (requisicionDetalles != null)
                    {
                        new RequisicionMaterialService().GuardaDetalles(requisicionDetalles, Context);

                        List<int> requisicionesIds = new List<int>();

                        foreach (ARtblRequisicionMaterialDetalle requisicionDetalle in requisicionDetalles)
                        {
                            if (!requisicionesIds.Contains(requisicionDetalle.RequisicionMaterialId))
                            {
                                requisicionesIds.Add(requisicionDetalle.RequisicionMaterialId);
                            }
                        }

                        if (requisicionesIds != null)
                        {
                            foreach (int requisicionId in requisicionesIds)
                            {
                                //Actualizamos el estatus de la Requisición
                                Context.ARspActualizaEstatusRequisicionMaterial(requisicionId);
                            }
                        }
                    }

                    //Convertimos los Prospectos en Proveedores
                    if (prospectosProveedorGanadores != null)
                    {
                        foreach (int prospectoId in prospectosProveedorGanadores)
                        {
                            Context.ARspConvertirProspectoProveedor(prospectoId);
                        }
                    }

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    //Retornamos la entidad que se acaba de guardar en la Base de Datos
                    return invitacionCompraId;
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

        public int ConvertirOC(tblOrdenCompra entidad, int usuarioId, DateTime fecha)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Creamos el Id de la cabecera
                    tblOrdenCompra ordenCompra = new OrdenCompraService().Inserta(entidad, Context);

                    if (ordenCompra.tblOrdenCompraDet != null)
                    {
                        int invitacionCompraId = 0;
                        List<ARtblInvitacionCompraDetalle> invitacionCompraDetalles = new List<ARtblInvitacionCompraDetalle>();

                        foreach (tblOrdenCompraDet detalle in ordenCompra.tblOrdenCompraDet)
                        {
                            //Agregamos los detalles convertidos
                            ARtblInvitacionCompraDetalle invitacionCompraDetalle = BuscaDetallePorId(detalle.InvitacionCompraDetalleId);
                            invitacionCompraDetalle.EstatusId = AREstatusInvitacionCompraDetalle.CONVERTIDO_OC;
                            invitacionCompraDetalle.ModificadoPorId = usuarioId;
                            invitacionCompraDetalle.FechaUltimaModificacion = fecha;
                            invitacionCompraDetalles.Add(invitacionCompraDetalle);
                            invitacionCompraId = invitacionCompraDetalle.InvitacionCompraId;

                            ARtblOrdenCompraInvitacionDet ocInvitacionDet = new ARtblOrdenCompraInvitacionDet();
                            ocInvitacionDet.OrdenCompraDetId = detalle.OrdenCompraDetId;
                            ocInvitacionDet.InvitacionCompraDetalleId = detalle.InvitacionCompraDetalleId;
                            ocInvitacionDet.CreadoPorId = usuarioId;

                            //Agregamos los detalles al Context
                            Context.ARtblOrdenCompraInvitacionDet.Add(ocInvitacionDet);

                            ARtblOrdenCompraRequisicionDet ocRequisicionDetalle = new ARtblOrdenCompraRequisicionDet();
                            ocRequisicionDetalle.OrdenCompraDetId = detalle.OrdenCompraDetId;
                            ocRequisicionDetalle.RequisicionMaterialDetalleId = detalle.RequisicionMaterialDetalleId;
                            ocRequisicionDetalle.Cantidad = Convert.ToDecimal(detalle.Cantidad);
                            ocRequisicionDetalle.CreadoPorId = usuarioId;

                            //Agregamos los detalles al Context
                            Context.ARtblOrdenCompraRequisicionDet.Add(ocRequisicionDetalle);
                        }

                        //Guardamos cambios
                        Context.SaveChanges();

                        //Guardamos los detalles convertidos
                        GuardaDetalles(invitacionCompraId, invitacionCompraDetalles, Context);

                        //Actualizamos el estatus de la Invitación de Compra
                        Context.ARspActualizaEstatusInvitacionCompra(invitacionCompraId);
                    }

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    //Retornamos la entidad que se acaba de guardar en la Base de Datos
                    return ordenCompra.OrdenCompraId;
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

        public List<ARtblInvitacionCompraDetalle> BuscaDetallesNoCancelados(int invitacionCompraId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblInvitacionCompraDetalle.Where(m => m.InvitacionCompraId == invitacionCompraId
                    && m.EstatusId != AREstatusInvitacionCompraDetalle.CANCELADO).ToList();
            }
        }

        public List<ARspConsultaInvitacionCompraDetalles_Result> BuscaDetallesPorInvitacionCompraId(int invitacionCompraId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaInvitacionCompraDetalles(invitacionCompraId).ToList();
            }
        }

        public ARtblInvitacionCompraDetalle BuscaDetallePorId(int invitacionCompraDetalleId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblInvitacionCompraDetalle.Where(m => m.InvitacionCompraDetalleId == invitacionCompraDetalleId).FirstOrDefault();
            }
        }

        public List<ARspConsultaInvitacionCompraListadoProveedores_Result> BuscaInvitacionCompraListadoProveedores(int invitacionCompraId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaInvitacionCompraListadoProveedores(invitacionCompraId).ToList();
            }
        }

        public List<ARspConsultaInvitacionCompraOrdenesCompra_Result> BuscaInvitacionCompraOrdenesCompra(int invitacionCompraId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaInvitacionCompraOrdenesCompra(invitacionCompraId).ToList();
            }
        }
    }
}