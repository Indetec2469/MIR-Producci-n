using Newtonsoft.Json;
using RequisicionesAlmacen.Areas.Compras.Requisiciones.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.Compras;
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Compras.Requisiciones.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.REQUISICION_POR_COMPRAR)]
    public class RequisicionPorComprarController : BaseController<ARtblRequisicionMaterial, RequisicionPorComprarViewModel>
    {
        public ActionResult Index()
        {
            // Creamos el objeto
            RequisicionPorComprarViewModel viewModel = new RequisicionPorComprarViewModel();

            //Asignamos los detalles
            viewModel.ListRequisicionMaterialDetalles = new RequisicionPorComprarService().BuscaRequisicionMaterialPorComprarDetalles();

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref viewModel);

            //Retornamos la vista junto con su Objeto Modelo
            return View("RequisicionPorComprar", viewModel);
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public override JsonResult Guardar(ARtblRequisicionMaterial requisicion)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult GuardaCambios(List<RequisicionOrdenCompraItem> ordenesCompra,
                                        List<RequisicionOrdenCompraItem> invitacionesCompra,
                                        List<RequisicionOrdenCompraDetalleItem> detallesRevision)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            RequisicionMaterialService requisicionService = new RequisicionMaterialService();

            List<tblOrdenCompra> ordenesCompraTemp = new List<tblOrdenCompra>();
            List<ARtblOrdenCompraRequisicionDet> ocRequisicionDetalles = new List<ARtblOrdenCompraRequisicionDet>();
            List<ARtblInvitacionArticulo> invitacionesArticuloTemp = new List<ARtblInvitacionArticulo>();
            List<int> requisicionesIds = new List<int>();
            List<ARtblRequisicionMaterialDetalle> detallesConvertidos = new List<ARtblRequisicionMaterialDetalle>();
            List<ARtblRequisicionMaterialDetalle> detallesRevisionTemp = new List<ARtblRequisicionMaterialDetalle>();

            if (ordenesCompra != null)
            {
                foreach (RequisicionOrdenCompraItem ordenCompra in ordenesCompra)
                {
                    foreach (RequisicionOrdenCompraDetalleItem detalle in ordenCompra.Detalles)
                    {
                        ARtblRequisicionMaterial requisicionTemp = requisicionService.BuscaPorId(detalle.RequisicionMaterialId);
                        ARtblRequisicionMaterialDetalle detalleTemp = new ARtblRequisicionMaterialDetalle().GetModelo(requisicionService.BuscaDetallePorId(detalle.RequisicionMaterialDetalleId));

                        if (!StructuralComparisons.StructuralEqualityComparer.Equals(detalle.RequisicionTimestamp, requisicionTemp.Timestamp)
                            || !StructuralComparisons.StructuralEqualityComparer.Equals(detalle.DetalleTimestamp, detalleTemp.Timestamp))
                        {
                            throw new Exception("La Requisición con el código [" + detalle.Solicitud + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
                        }

                        if (!requisicionesIds.Contains(requisicionTemp.RequisicionMaterialId))
                        {
                            requisicionesIds.Add(requisicionTemp.RequisicionMaterialId);
                        }

                        detalleTemp.EstatusId = AREstatusRequisicionDetalle.RELACIONADO_OC;
                        detalleTemp.FechaUltimaModificacion = fecha;
                        detalleTemp.ModificadoPorId = usuarioId;

                        detallesConvertidos.Add(detalleTemp);

                        ARtblOrdenCompraRequisicionDet ocRequisicionDetalle = new ARtblOrdenCompraRequisicionDet();
                        ocRequisicionDetalle.OrdenCompraDetId = detalle.OrdenCompraDetId;
                        ocRequisicionDetalle.RequisicionMaterialDetalleId = detalle.RequisicionMaterialDetalleId;
                        ocRequisicionDetalle.Cantidad = Convert.ToDecimal(detalle.CantidadComprar.GetValueOrDefault());
                        ocRequisicionDetalle.CreadoPorId = usuarioId;

                        ocRequisicionDetalles.Add(ocRequisicionDetalle);
                    }

                    ordenesCompraTemp.Add((tblOrdenCompra)ordenCompra);
                }
            }

            if (invitacionesCompra != null)
            {
                foreach (RequisicionOrdenCompraItem invitacionCompra in invitacionesCompra)
                {
                    foreach (RequisicionOrdenCompraDetalleItem detalle in invitacionCompra.Detalles)
                    {
                        ARtblRequisicionMaterial requisicionTemp = requisicionService.BuscaPorId(detalle.RequisicionMaterialId);
                        ARtblRequisicionMaterialDetalle detalleTemp = new ARtblRequisicionMaterialDetalle().GetModelo(requisicionService.BuscaDetallePorId(detalle.RequisicionMaterialDetalleId));

                        if (!StructuralComparisons.StructuralEqualityComparer.Equals(detalle.RequisicionTimestamp, requisicionTemp.Timestamp)
                            || !StructuralComparisons.StructuralEqualityComparer.Equals(detalle.DetalleTimestamp, detalleTemp.Timestamp))
                        {
                            throw new Exception("La Requisición con el código [" + detalle.Solicitud + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
                        }

                        if (!requisicionesIds.Contains(requisicionTemp.RequisicionMaterialId))
                        {
                            requisicionesIds.Add(requisicionTemp.RequisicionMaterialId);
                        }

                        detalleTemp.EstatusId = AREstatusRequisicionDetalle.POR_INVITAR;
                        detalleTemp.FechaUltimaModificacion = fecha;
                        detalleTemp.ModificadoPorId = usuarioId;

                        detallesConvertidos.Add(detalleTemp);
                    }

                    invitacionesArticuloTemp.Add((ARtblInvitacionArticulo)invitacionCompra);
                }
            }

            if (detallesRevision != null)
            {
                foreach (RequisicionOrdenCompraDetalleItem detalle in detallesRevision)
                {
                    ARtblRequisicionMaterial requisicionTemp = requisicionService.BuscaPorId(detalle.RequisicionMaterialId);
                    ARtblRequisicionMaterialDetalle detalleTemp = new ARtblRequisicionMaterialDetalle().GetModelo(requisicionService.BuscaDetallePorId(detalle.RequisicionMaterialDetalleId));

                    if (!StructuralComparisons.StructuralEqualityComparer.Equals(detalle.RequisicionTimestamp, requisicionTemp.Timestamp)
                        || !StructuralComparisons.StructuralEqualityComparer.Equals(detalle.DetalleTimestamp, detalleTemp.Timestamp))
                    {
                        throw new Exception("La Requisición con el código [" + detalle.Solicitud + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
                    }

                    if (!requisicionesIds.Contains(requisicionTemp.RequisicionMaterialId))
                    {
                        requisicionesIds.Add(requisicionTemp.RequisicionMaterialId);
                    }

                    detalleTemp.EstatusId = detalle.Revision.GetValueOrDefault() ? AREstatusRequisicionDetalle.REVISION : AREstatusRequisicionDetalle.RECHAZADO;
                    detalleTemp.FechaUltimaModificacion = fecha;
                    detalleTemp.ModificadoPorId = usuarioId;
                    detalleTemp.MotivoRechazo = detalle.Motivo;

                    detallesRevisionTemp.Add(detalleTemp);
                }
            }

            new RequisicionPorComprarService().GuardaCambios(ordenesCompraTemp,
                                                             invitacionesArticuloTemp,
                                                             requisicionesIds,
                                                             detallesConvertidos,
                                                             detallesRevisionTemp,
                                                             usuarioId);

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Registros guardados con Exito!");
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Listar()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref RequisicionPorComprarViewModel viewModel)
        {
            viewModel.ListTarifasImpuesto = new TarifaImpuestoService().BuscaTodos();
            viewModel.ListAlmacenes = new RequisicionPorComprarService().BuscaAlmacenes();
            viewModel.ListFuentesFinanciamiento = new RequisicionPorComprarService().BuscaFuentesFinanciamientoProducto();
            viewModel.ListProveedores = new ProveedorService().BuscaTodos();
            viewModel.ListMontosCompra = new ControlMaestroConfiguracionMontoCompraService().BuscaTodos();
            viewModel.FechaOperacion = new ConfiguracionEnteService().BuscaFechaOperacion();

            //Ejercicio para los campos de Fecha
            viewModel.EjercicioUsuario = SessionHelper.GetUsuario().Ejercicio;
        }
    }
}