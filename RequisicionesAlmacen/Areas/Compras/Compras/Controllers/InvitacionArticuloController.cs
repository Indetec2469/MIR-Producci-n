using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using RequisicionesAlmacen.Areas.Compras.Compras.Models;
using RequisicionesAlmacen.Areas.Compras.Compras.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.Compras;
using RequisicionesAlmacenBL.Services.SAACG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Compras.Compras.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.ARTICULOS_INVITACION)]
    public class InvitacionArticuloController : BaseController<InvitacionArticuloDetalleItem, InvitacionArticuloViewModel>
    {
        public ActionResult Index()
        {
            // Creamos el objeto
            InvitacionArticuloViewModel viewModel = new InvitacionArticuloViewModel();

            //Asignamos los detalles
            viewModel.ListArticulosInvitacion = new InvitacionArticuloService().BuscaInvitacionArticulosPorConvertir();

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref viewModel);

            //Retornamos la vista junto con su Objeto Modelo
            return View("InvitacionArticulos", viewModel);
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
        public override JsonResult Guardar(InvitacionArticuloDetalleItem invitacionArticulo)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult GuardaCambios(List<InvitacionArticuloDetalleItem> articulosInvitacion)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            InvitacionArticuloService invitacionArticuloService = new InvitacionArticuloService();

            ARtblInvitacionCompra invitacionCompra = null;
            List<ARtblInvitacionCompraDetalle> invitacionCompraDetalles = new List<ARtblInvitacionCompraDetalle>();
            List<ARtblInvitacionArticuloDetalle> invitacionArticuloDetallesConvertidos = new List<ARtblInvitacionArticuloDetalle>();

            foreach (InvitacionArticuloDetalleItem invitacionArticulo in articulosInvitacion)
            {
                ARtblInvitacionArticulo invitacionTemp = invitacionArticuloService.BuscaPorId(invitacionArticulo.InvitacionArticuloId);

                if (!StructuralComparisons.StructuralEqualityComparer.Equals(invitacionArticulo.InvitacionTimestamp, invitacionTemp.Timestamp))
                {
                    throw new Exception("La Invitación con el código [" + invitacionTemp.InvitacionArticuloId + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
                }

                ARtblInvitacionArticuloDetalle invitacionArticuloDetalleTemp = new ARtblInvitacionArticuloDetalle().GetModelo(invitacionArticuloService.BuscaDetallePorId(invitacionArticulo.InvitacionArticuloDetalleId));

                if (!StructuralComparisons.StructuralEqualityComparer.Equals(invitacionArticulo.DetalleTimestamp, invitacionArticuloDetalleTemp.Timestamp))
                {
                    throw new Exception("El detalle ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                }

                //Creamos la cabecera
                if (invitacionCompra == null)
                {
                    invitacionCompra = new ARtblInvitacionCompra();

                    invitacionCompra.ProveedorId = invitacionArticulo.ProveedorId;
                    invitacionCompra.AlmacenId = invitacionArticulo.AlmacenId;
                    invitacionCompra.Ejercicio = fecha.Year;
                    invitacionCompra.Fecha = fecha;
                    invitacionCompra.MontoInvitacion = 0;
                    invitacionCompra.EstatusId = ControlMaestroMapeo.AREstatusInvitacionCompra.GUARDADA;
                    invitacionCompra.CreadoPorId = usuarioId;
                }

                //Agregamos los registros a guardar
                ARtblInvitacionCompraDetalle invitacionCompraDetalle = (ARtblInvitacionCompraDetalle)invitacionArticulo;
                invitacionCompraDetalle.EstatusId = ControlMaestroMapeo.AREstatusInvitacionCompraDetalle.ACTIVO;
                invitacionCompraDetalle.CreadoPorId = usuarioId;
                
                invitacionCompraDetalles.Add(invitacionCompraDetalle);

                //Sumamos el total del detalle a la cabecera
                invitacionCompra.MontoInvitacion += Convert.ToDecimal(invitacionCompraDetalle.Total);

                //Agregamos los detalles convertidos
                invitacionArticuloDetalleTemp.EstatusId = ControlMaestroMapeo.AREstatusInvitacionArticuloDetalle.INVITADO;
                invitacionArticuloDetalleTemp.FechaUltimaModificacion = fecha;
                invitacionArticuloDetalleTemp.ModificadoPorId = usuarioId;

                invitacionArticuloDetallesConvertidos.Add(invitacionArticuloDetalleTemp);
            }

            new InvitacionCompraService().GuardaCambios(invitacionCompra,
                                                        invitacionCompraDetalles,
                                                        invitacionArticuloDetallesConvertidos);

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Registros guardados con Exito!");
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult EliminarPorModelo(InvitacionArticuloDetalleItem invitacionArticulo)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            InvitacionArticuloService service = new InvitacionArticuloService();

            ARtblInvitacionArticulo invitacionTemp = service.BuscaPorId(invitacionArticulo.InvitacionArticuloId);

            if (!StructuralComparisons.StructuralEqualityComparer.Equals(invitacionArticulo.InvitacionTimestamp, invitacionTemp.Timestamp))
            {
                throw new Exception("La Invitación con el código [" + invitacionTemp.InvitacionArticuloId + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            ARtblInvitacionArticuloDetalle detalleTemp = new ARtblInvitacionArticuloDetalle().GetModelo(service.BuscaDetallePorId(invitacionArticulo.InvitacionArticuloDetalleId));

            if (!StructuralComparisons.StructuralEqualityComparer.Equals(invitacionArticulo.DetalleTimestamp, detalleTemp.Timestamp))
            {
                throw new Exception("El detalle ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            //Validamos si todos los detalles se cancelarán para cancelar la cabecera
            if (service.BuscaDetallesNoCancelados(invitacionArticulo.InvitacionArticuloId).Count - 1 <= 0)
            {
                invitacionTemp = new ARtblInvitacionArticulo().GetModelo(invitacionTemp);
                
                invitacionTemp.EstatusId = ControlMaestroMapeo.AREstatusInvitacionArticulo.CANCELADA;
                invitacionTemp.FechaUltimaModificacion = fecha;
                invitacionTemp.ModificadoPorId = usuarioId;
            }
            else
            {
                invitacionTemp = null;
            }

            detalleTemp.EstatusId = ControlMaestroMapeo.AREstatusInvitacionArticuloDetalle.CANCELADO;
            detalleTemp.FechaUltimaModificacion = fecha;
            detalleTemp.ModificadoPorId = usuarioId;

            List<ARtblInvitacionArticuloDetalle> detalles = new List<ARtblInvitacionArticuloDetalle>();
            detalles.Add(detalleTemp);

            List<ARtblRequisicionMaterialDetalle> requisicionDetalles = new List<ARtblRequisicionMaterialDetalle>();
            ARtblRequisicionMaterialDetalle requisicionMaterialDetalle = new ARtblRequisicionMaterialDetalle().GetModelo(new RequisicionMaterialService().BuscaDetallePorId(detalleTemp.RequisicionMaterialDetalleId));
            requisicionMaterialDetalle.EstatusId = ControlMaestroMapeo.AREstatusRequisicionDetalle.POR_COMPRAR;
            requisicionMaterialDetalle.FechaUltimaModificacion = fecha;
            requisicionMaterialDetalle.ModificadoPorId = usuarioId;
            requisicionDetalles.Add(requisicionMaterialDetalle);

            service.GuardaCambios(invitacionTemp, detalles, requisicionDetalles);

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Registro eliminado con Exito!");
        }

        public override ActionResult Listar()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref InvitacionArticuloViewModel viewModel)
        {
        }
    }
}