using Newtonsoft.Json;
using RequisicionesAlmacen.Areas.Almacenes.Almacenes.Models;
using RequisicionesAlmacen.Areas.Almacenes.Almacenes.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.Compras;
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using SACG.sysSacg.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Almacenes.Almacenes.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.REQUISICION_POR_SURTIR)]
    public class SolicitudPorSurtirController : BaseController<ARtblRequisicionMaterial, RequisicionPorSurtirViewModel>
    {
        private string API_FICHA = "/almacenes/almacenes/solicitudporsurtir/";

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        public override ActionResult Editar(int id)
        {
            // Creamos el objeto
            RequisicionPorSurtirViewModel viewModel = new RequisicionPorSurtirViewModel();

            //Buscamos el Objeto por el Id que se envio como parametro
            ARtblRequisicionMaterial requisicionMaterial = new RequisicionMaterialService().BuscaPorId(id);

            //Asignamos el modelo
            viewModel.RequisicionMaterial = requisicionMaterial != null ? requisicionMaterial : new ARtblRequisicionMaterial();

            if (requisicionMaterial == null)
            {
                //Asignamos el error
                SetViewBagError("La Solicitúd no existe o está Cancelada. Favor de revisar.", API_FICHA + "listar");
            }
            else if (requisicionMaterial.EstatusId != AREstatusRequisicion.AUTORIZADA
                && requisicionMaterial.EstatusId != AREstatusRequisicion.EN_PROCESO
                && requisicionMaterial.EstatusId != AREstatusRequisicion.EN_ALMACEN)
            {
                //Asignamos el error
                SetViewBagError("La Solicitúd no está Autorizada, En Proceso o En Almacén. Favor de revisar.", API_FICHA + "listar");
            }
            else
            {
                //Asignamos los detalles
                viewModel.ListRequisicionMaterialDetalles = new RequisicionPorSurtirService().BuscaDetallesPorRequisicionMaterialId(id);

                //Asignamos las existencias
                viewModel.ListExistencias = new RequisicionPorSurtirService().BuscaExistenciaProducto(id);

                //Agregamos todos los datos necesarios para el funcionamiento de la ficha
                //como son los Listados para combos, tablas, arboles.
                GetDatosFicha(ref viewModel);
            }

            //Retornamos la vista junto con su Objeto Modelo
            return View("RequisicionPorSurtir", viewModel);
        }

        [JsonException]
        public override JsonResult Guardar(ARtblRequisicionMaterial requisicion)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult GuardaCambios(ARtblRequisicionMaterial requisicion, 
                                        List<RequisicionDetalleSurtirItem> movimientos,
                                        List<ARspConsultaRequisicionPorSurtirDetalles_Result> detallesRevision,
                                        string numeroOficio,
                                        string observaciones)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now; 
            
            RequisicionMaterialService requisicionService = new RequisicionMaterialService();

            ARtblRequisicionMaterial temp = requisicionService.BuscaPorId(requisicion.RequisicionMaterialId);

            if (!StructuralComparisons.StructuralEqualityComparer.Equals(requisicion.Timestamp, temp.Timestamp))
            {
                throw new Exception("La Requisición con el código [" + requisicion.CodigoRequisicion + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            List<ARtblRequisicionMaterialDetalle> detalles = new List<ARtblRequisicionMaterialDetalle>();

            if (movimientos != null)
            {
                foreach (RequisicionDetalleSurtirItem movimiento in movimientos)
                {
                    if (detalles.Find(m => m.RequisicionMaterialDetalleId == movimiento.RequisicionMaterialDetalleId) == null)
                    {
                        Decimal cantidadPorSurtir = movimiento.CantidadPorSurtir;
                        Decimal cantidadSurtida = 0;

                        List<RequisicionDetalleSurtirItem> movimientosTemp = movimientos.FindAll(m => m.RequisicionMaterialDetalleId == movimiento.RequisicionMaterialDetalleId);

                        foreach (RequisicionDetalleSurtirItem movimientoTemp in movimientosTemp)
                        {
                            cantidadSurtida += movimiento.CantidadSurtir;
                        }
                        
                        ARtblRequisicionMaterialDetalle detalleTemp = new ARtblRequisicionMaterialDetalle().GetModelo(requisicionService.BuscaDetallePorId(movimiento.RequisicionMaterialDetalleId));

                        detalleTemp.EstatusId = cantidadPorSurtir == cantidadSurtida ? AREstatusRequisicionDetalle.SURTIDO : AREstatusRequisicionDetalle.SURTIDO_PARCIAL;
                        detalleTemp.ModificadoPorId = usuarioId;
                        detalleTemp.FechaUltimaModificacion = fecha;

                        detalles.Add(detalleTemp);
                    }
                }
            }

            if (detallesRevision != null)
            {
                foreach (ARspConsultaRequisicionPorSurtirDetalles_Result detalle in detallesRevision)
                {
                    ARtblRequisicionMaterialDetalle detalleTemp = new ARtblRequisicionMaterialDetalle().GetModelo(requisicionService.BuscaDetallePorId(detalle.RequisicionMaterialDetalleId));

                    detalleTemp.EstatusId = detalle.Revision.GetValueOrDefault() ? AREstatusRequisicionDetalle.REVISION : AREstatusRequisicionDetalle.POR_COMPRAR;
                    detalleTemp.ModificadoPorId = usuarioId;
                    detalleTemp.FechaUltimaModificacion = fecha;

                    detalles.Add(detalleTemp); 
                }
            }

            //Regresamos el Id del agrupador que se insertó
            return Json(new RequisicionPorSurtirService().GuardaCambios(temp.RequisicionMaterialId, temp.CodigoRequisicion, numeroOficio, observaciones, movimientos, detalles, usuarioId));
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Listar()
        {
            RequisicionPorSurtirViewModel viewModel = new RequisicionPorSurtirViewModel();

            viewModel.ListRequisicionPorSurtir = new RequisicionPorSurtirService().BuscaListado();
            
            return View("ListadoRequisicionPorSurtir", viewModel);
        }

        protected override void GetDatosFicha(ref RequisicionPorSurtirViewModel viewModel)
        {
            ARtblRequisicionMaterial requisicion = viewModel.RequisicionMaterial;

            GRtblUsuario usuario = new UsuarioService().BuscaPorId(requisicion.CreadoPorId);
            tblDependencia area = new DependenciaService().BuscaDependenciaPorId(requisicion.AreaId);

            viewModel.Solicitante = new RequisicionMaterialService().GetNombreCompletoEmpleado(usuario.EmpleadoId.GetValueOrDefault());
            viewModel.Area = area.DependenciaId + " - " + area.Nombre;
            viewModel.Fecha = new SistemaService().GetFechaConFormato(requisicion.FechaRequisicion);
            viewModel.Estatus = AREstatusRequisicion.Nombre[requisicion.EstatusId];
            viewModel.FechaOperacion = new ConfiguracionEnteService().BuscaFechaOperacion();
        }

        public ActionResult RptSurtidoSolicitud(int requisicionId, int agrupadorId)
        {
            ReportHelper reportHelper = new ReportHelper();

            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@pTituloReporte", "Salida de Almacén");
            parametros.Add("@pFechaCorte", new RequisicionMaterialService().BuscaPorId(requisicionId).FechaRequisicion.Year);
            parametros.Add("@pNombreReporte", "rptSalidaAlmacen");
            parametros.Add("@pRequisicionId", requisicionId);
            parametros.Add("@pAgrupadorId", agrupadorId);

            ViewBag.WebReport = reportHelper.GetWebReport("Almacen/RequisicionMaterial/ARrptSurtidoSolicitud.frx", parametros);

            return View("RptSurtidoSolicitud");
        }
    }
}