using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Services.SAACG;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacenBL.Services.Compras
{
    public class OrdenCompraReciboService : BaseService<tblCompra>
    {
        public override tblCompra BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblCompra.Where(m => m.CompraId == id).FirstOrDefault();
            }
        }

        public List<tblCompra> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblCompra.ToList();
            }
        }

        public List<ARvwListadoOrdenCompraRecibo> BuscaListado()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARvwListadoOrdenCompraRecibo.OrderByDescending(m => m.CompraId).ToList();
            }
        }

        public override tblCompra Inserta(tblCompra entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            tblCompra compra = context.tblCompra.Add(entidad);

            //Guardamos cambios
            context.SaveChanges();

            //Retornamos si se guardó correctamente
            return compra;
        }

        public override bool Actualiza(tblCompra entidad, SAACGContext context)
        {
            return Actualiza(entidad, null, context);
        }

        public bool Actualiza(tblCompra entidad, Nullable<DateTime> fechaCancelacion, SAACGContext context)
        {
            //Agregamos la entidad el Context
            tblCompra compra = context.tblCompra.Add(entidad);

            //Marcamos el modelo como modificado
            context.Entry(entidad).State = EntityState.Modified;

            //Marcar todas las propiedades que no se pueden actualizar como FALSE
            //para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in tblCompra.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            //Guardamos cambios
            context.SaveChanges();

            //Actualizamos la póliza
            new PolizaService().ActualizaPolizaGastoDevengado(compra.CompraId, 
                                                              entidad.Status.Equals(EstatusOrdenCompraRecibo.CANCELADO) ? "C" : "M", 
                                                              fechaCancelacion != null ? fechaCancelacion : compra.Fecha,
                                                              context);

            return true;
        }

        public int GuardaCambios(int ordenCompraId,
                                 string almacenId, 
                                 int usuarioId,
                                 tblCompra entidad,
                                 List<tblCompraDet> detalles,
                                 List<int> requisicionesIds,
                                 List<ARtblRequisicionMaterialDetalle> detallesRecibidos)
        {
            return GuardaCambios(ordenCompraId,
                                 almacenId,
                                 usuarioId,
                                 entidad,
                                 detalles,
                                 requisicionesIds,
                                 detallesRecibidos,
                                 false,
                                 null);
        }

        public int GuardaCambios(int ordenCompraId,
                                 string almacenId,
                                 int usuarioId, 
                                 tblCompra entidad, 
                                 List<tblCompraDet> detalles,
                                 List<int> requisicionesIds,
                                 List<ARtblRequisicionMaterialDetalle> detallesRecibidos,
                                 bool cancelacion,
                                 ARtblCompraCancelInfo cancelInfo)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    //Creamos el Id de la cabecera
                    int compraId = entidad.CompraId;
                    bool insertarPoliza = false;

                    //Validamos si es registro nuevo
                    if (entidad.CompraId == 0)
                    {
                        compraId = Inserta(entidad, Context).CompraId;
                        insertarPoliza = true;
                    }
                    else
                    {
                        if (cancelacion && cancelInfo != null)
                        {
                            Actualiza(entidad, cancelInfo.FechaCancelacion, Context);
                        }
                        else
                        {
                            Actualiza(entidad, Context);
                        }
                    }

                    //Guardamos cambios
                    Context.SaveChanges();

                    if (detalles != null)
                    {
                        int tipoMovimientoId = !cancelacion ? TipoInventarioMovimiento.ORDEN_COMPRA_RECIBO : TipoInventarioMovimiento.CANCELACION_RECIBO_OC;
                        string motivoMovto = (cancelacion ? "Cancelación de " : "") + "Recibo de OC: " + ordenCompraId;
                        int creadoPorId = usuarioId;
                        int referenciaMovtoId = compraId;

                        List<ARudtInventarioMovimiento> movimientosUDT = new List<ARudtInventarioMovimiento>();

                        //Registramos los movimientos en el inventario
                        foreach (tblCompraDet detalle in detalles)
                        {
                            tblCompraDet detalleTemp;

                            if (!cancelacion)
                            {
                                //Agregamos el id de la cabecera
                                detalle.CompraId = compraId;

                                //Agregamos los detalles al Context
                                detalleTemp = Context.tblCompraDet.Add(detalle);

                                if (detalle.CompraDetId > 0)
                                {
                                    Context.Entry(detalle).State = EntityState.Modified;

                                    //Marcar todas las propiedades que no se pueden actualizar como FALSE
                                    //para que no se actualice su informacion en Base de Datos
                                    foreach (string propertyName in tblCompraDet.PropiedadesNoActualizables)
                                    {
                                        Context.Entry(detalle).Property(propertyName).IsModified = false;
                                    }
                                }

                                //Guardamos cambios
                                Context.SaveChanges();

                                //Actualizamos los costos
                                tblProducto producto = new ProductoService().BuscaArticuloPorId(detalle.ProductoId);
                                producto.CostoPromedio = Decimal.Round((producto.CostoPromedio + detalle.Costo) / 2, 2);
                                producto.CostoUltimo = Decimal.Round(detalle.Costo, 2);
                                new ProductoService().Actualiza(producto, Context);

                                //Creamos la relación de Almacén/Producto si no existe
                                if (detalle.AlmacenProductoId < 0)
                                {
                                    ARtblAlmacenProducto almacenProducto = new ARtblAlmacenProducto();

                                    detalle.AlmacenProductoId = new AlmacenProductoService().Inserta(
                                        detalle.ProductoId,
                                        almacenId,
                                        detalle.CuentaPresupuestalEgrId,
                                        creadoPorId,
                                        Context).AlmacenProductoId;
                                }
                            } 
                            else
                            {
                                detalle.Cantidad = detalle.Cantidad * -1;
                                detalleTemp = detalle;
                            }

                            ARudtInventarioMovimiento udt = new ARudtInventarioMovimiento();

                            udt.AlmacenProductoId = detalle.AlmacenProductoId;
                            udt.CantidadMovimiento = Convert.ToDecimal(detalle.Cantidad);
                            udt.CostoUnitario = detalle.Costo;
                            udt.ReferenciaMovtoId = detalleTemp.CompraDetId;

                            movimientosUDT.Add(udt);
                        }

                        new ProcesadorInventariosService().Execute(tipoMovimientoId,
                                                                   motivoMovto,
                                                                   creadoPorId,
                                                                   true,
                                                                   referenciaMovtoId,
                                                                   null,
                                                                   null,
                                                                   null,
                                                                   movimientosUDT,
                                                                   Context);
                    }

                    //Insertar la póliza si es un recibo
                    if (insertarPoliza)
                    {
                        //Actualizamos la póliza
                        new PolizaService().ActualizaPolizaGastoDevengado(compraId, "A", entidad.Fecha, Context);
                    }

                    //Actualizamos el estatus de la OC
                    Context.ARspActualizaEstatusOrdenCompra(ordenCompraId);

                    //Actualizamos los detalles recibidos
                    if (detallesRecibidos != null)
                    {
                        new RequisicionMaterialService().GuardaDetalles(detallesRecibidos, Context);
                    }

                    if (requisicionesIds != null)
                    {
                        foreach (int requisicionId in requisicionesIds)
                        {
                            //Actualizamos el estatus de la Requisición
                            Context.ARspActualizaEstatusRequisicionMaterial(requisicionId);
                        }
                    }

                    if (cancelacion && cancelInfo != null)
                    {
                        //Agregamos la entidad el Context
                        ARtblCompraCancelInfo cancelInfoTemp = Context.ARtblCompraCancelInfo.Add(cancelInfo);

                        //Guardamos cambios
                        Context.SaveChanges();
                    }

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    //Retornamos la entidad que se acaba de guardar en la Base de Datos
                    return compraId;
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

        public List<ARspConsultaOCPorRecibir_Result> BuscaComboOrdenesCompra()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaOCPorRecibir().OrderByDescending(m => m.OrdenCompraId).ToList();
            }
        }
        
        public List<ARspConsultaOCPorRecibirDetalles_Result> BuscaOrdenesCompraDetalles()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaOCPorRecibirDetalles().ToList();
            }
        }

        public List<ARspConsultaOrdenCompraReciboDetalles_Result> BuscaDetallesPorCompraId(int compraId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaOrdenCompraReciboDetalles(compraId).ToList();
            }
        }

        public ARspConsultaDatosOCReciboPorId_Result BuscaDatosOCReciboPorId(int ordenCompraId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaDatosOCReciboPorId(ordenCompraId).FirstOrDefault();
            }
        }

        public List<ARrptReciboOC> BuscaRptReciboOC(Nullable<DateTime> fechaInicioP, Nullable<DateTime> fechaFinP, Nullable<int> ordenCompraIdP, Nullable<int> compraIdP)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                DateTime fechaInicio = fechaInicioP.GetValueOrDefault();
                DateTime fechaFin = (fechaFinP != null ? fechaFinP.GetValueOrDefault() : new DateTime(9999, 12, 31));
                int ordenCompraId = ordenCompraIdP.GetValueOrDefault();
                int compraId = compraIdP.GetValueOrDefault();

                return Context.ARvwListadoOrdenCompraRecibo
                    .Where(m => DateTime.Compare(m.Fecha, fechaInicio) >= 0
                             && DateTime.Compare(m.Fecha, fechaFin) <= 0
                             && (m.CodigoOC == ordenCompraId || ordenCompraId == 0)
                             && (m.CompraId == compraId || compraId == 0))
                    .AsEnumerable().Select(m => new ARrptReciboOC
                    {
                        CompraId = m.CompraId,
                        FechaRecibo = m.FechaRecibo,
                        OrdenCompraId = m.CodigoOC,
                        FechaOC = m.FechaOC,
                    }).OrderByDescending(m => m.CompraId).ToList();
            }
        }
    }
}