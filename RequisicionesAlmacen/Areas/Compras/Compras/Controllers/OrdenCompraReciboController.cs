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
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.RECIBO_DE_OC)]
    public class OrdenCompraReciboController : BaseController<tblCompra, OrdenCompraReciboViewModel>
    {
        private string API_FICHA = "/compras/compras/ordencomprarecibo/";

        public override ActionResult Nuevo()
        {
            //Crear un objeto nuevo
            OrdenCompraReciboViewModel viewModel = new OrdenCompraReciboViewModel();

            //Asignamos el modelo al modelView
            tblCompra compra = new tblCompra();

            compra.CompraId = 0;
            compra.Fecha = DateTime.Now;
            compra.FechaVencimiento = DateTime.Now;
            compra.FechaContrarecibo = DateTime.Now;
            compra.FechaPagoProgramado = DateTime.Now;
            compra.Status = EstatusOrdenCompraRecibo.ACTIVO;
            compra.FechaCancelacion = DateTime.Now;
            compra.MotivoCancelacion = "NA";

            viewModel.Compra = compra;

            //Modo Solo Lectura
            viewModel.SoloLectura = false;

            //Buscamos los detalles de las OC
            viewModel.ListOrdenesCompraDetalles = new OrdenCompraReciboService().BuscaOrdenesCompraDetalles();

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref viewModel);

            //Retornamos la vista junto con su Objeto Modelo
            return View("OrdenCompraRecibo", viewModel);
        }

        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult Ver(int id)
        {
            OrdenCompraReciboService service = new OrdenCompraReciboService();

            //Creamos el objeto
            OrdenCompraReciboViewModel viewModel = new OrdenCompraReciboViewModel();

            //Buscamos el Objeto por el Id que se envio como parametro
            tblCompra compra = service.BuscaPorId(id);

            //Asignamos el Objeto al ViewModel
            viewModel.Compra = compra != null ? compra : new tblCompra();

            if (compra == null)
            {
                //Asignamos el error
                SetViewBagError("El recibo no existe. Favor de revisar.", API_FICHA + "listar");
            }
            else
            {
                //Asignamos los detalles
                List<ARspConsultaOrdenCompraReciboDetalles_Result> detalles = service.BuscaDetallesPorCompraId(compra.CompraId);
                viewModel.ListOrdenCompraReciboDetalles = detalles.ConvertAll(new Converter<ARspConsultaOrdenCompraReciboDetalles_Result, OrdenCompraReciboDetalleItem>(OrdenCompraReciboDetalleItem.ConvertTo));

                //Agregamos todos los datos necesarios para el funcionamiento de la ficha
                //como son los Listados para combos, tablas, arboles.
                GetDatosFicha(ref viewModel);

                //Validamos que el recibo tenga detalles relacionados
                if (viewModel.ListOrdenCompraReciboDetalles.Count == 0)
                {
                    //Asignamos el error
                    SetViewBagError("El recibo no tiene detalles. Favor de revisar.", API_FICHA + "listar");
                }
                else if (viewModel.ListOrdenCompraReciboDetalles[0].OrdenCompraId != 0)
                {
                    //Buscamos los datos de la Orden de Compra
                    ARspConsultaDatosOCReciboPorId_Result ordenCompra = service.BuscaDatosOCReciboPorId(viewModel.ListOrdenCompraReciboDetalles[0].OrdenCompraId);

                    if (ordenCompra != null)
                    {
                        viewModel.Compra.OrdenCompraId = ordenCompra.OrdenCompraId;
                        viewModel.Compra.FechaOC = ordenCompra.FechaOC;
                        viewModel.Compra.EstatusOC = ordenCompra.EstatusOC;
                    }
                }
            }

            //Modo Solo Lectura
            viewModel.SoloLectura = true;

            //Retornamos la vista junto con su Objeto Modelo
            return View("OrdenCompraRecibo", viewModel);
        }

        [JsonException]
        public override JsonResult Guardar(tblCompra ordenCompra)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult GuardaCambios(int ordenCompraId, 
                                        string statusOC, 
                                        tblCompra compra, 
                                        List<OrdenCompraReciboDetalleItem> detalles)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            OrdenCompraReciboService service = new OrdenCompraReciboService();

            //Como es un nuevo registro
            compra.Status = EstatusOrdenCompraRecibo.ACTIVO;

            //Verificamos que no se haya modidicado la OC
            tblOrdenCompra temp = new OrdenCompraService().BuscaPorId(ordenCompraId);

            if (!statusOC.Equals(temp.Status))
            {
                throw new Exception("La OC con el código [" + temp.OrdenCompraId + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            //Verificamos que no se haya modificado la Requisición
            RequisicionMaterialService requisicionService = new RequisicionMaterialService();

            List<int> requisicionesIds = new List<int>();
            List<ARtblRequisicionMaterialDetalle> detallesRecibidos = new List<ARtblRequisicionMaterialDetalle>();

            foreach (OrdenCompraReciboDetalleItem detalle in detalles)
            {
                if (detalle.RequisicionMaterialId != null)
                {
                    ARtblRequisicionMaterial requisicionTemp = requisicionService.BuscaPorId(detalle.RequisicionMaterialId.GetValueOrDefault());
                    ARtblRequisicionMaterialDetalle detalleTemp = requisicionService.BuscaDetallePorId(detalle.RequisicionMaterialDetalleId.GetValueOrDefault());

                    if (!StructuralComparisons.StructuralEqualityComparer.Equals(detalle.RequisicionTimestamp, requisicionTemp.Timestamp)
                            || !StructuralComparisons.StructuralEqualityComparer.Equals(detalle.DetalleTimestamp, detalleTemp.Timestamp))
                    {
                        throw new Exception("La Requisición con el código [" + detalle.Solicitud + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
                    }

                    if (!requisicionesIds.Contains(requisicionTemp.RequisicionMaterialId))
                    {
                        requisicionesIds.Add(requisicionTemp.RequisicionMaterialId);
                    }

                    detalleTemp.EstatusId = AREstatusRequisicionDetalle.EN_ALMACEN;
                    detalleTemp.FechaUltimaModificacion = fecha;
                    detalleTemp.ModificadoPorId = usuarioId;

                    detallesRecibidos.Add(new ARtblRequisicionMaterialDetalle().GetModelo(detalleTemp));
                }
            }

            List<tblCompraDet> detallesTemp = detalles.ConvertAll(new Converter<OrdenCompraReciboDetalleItem, tblCompraDet>(OrdenCompraReciboDetalleItem.ConvertTo));

            int compraId = service.GuardaCambios(ordenCompraId,
                                                 usuarioId,
                                                 compra,
                                                 detallesTemp,
                                                 requisicionesIds,
                                                 detallesRecibidos);

            string poliza = new PolizaService().BuscaPolizaPorReferenciaTipoMovimientoIdYTipo(compraId, tblTipoMovimiento.COMPRA, "A").Poliza;

            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "compraId", compraId },
                { "poliza", poliza }
            };

            //Retornamos el data si todo salio correctamente
            return Json(data);
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Listar()
        {
            OrdenCompraReciboViewModel viewModel = new OrdenCompraReciboViewModel();

            viewModel.ListOrdenCompraRecibo = new OrdenCompraReciboService().BuscaListado();
            
            return View("ListadoOrdenCompraRecibo", viewModel);
        }

        protected override void GetDatosFicha(ref OrdenCompraReciboViewModel viewModel)
        {
            //Datos de OC            
            viewModel.ListOrdenesCompra = new OrdenCompraReciboService().BuscaComboOrdenesCompra();
            viewModel.ListProveedores = new ProveedorService().BuscaTodos();
            viewModel.ListAlmacenes = new AlmacenService().BuscaTodos();

            //Datos de Recibo
            viewModel.Compra.Estatus = EstatusOrdenCompraRecibo.Nombre[viewModel.Compra.Status];

            //Datos Detalles
            viewModel.ListTarifasImpuesto = new TarifaImpuestoService().BuscaTodos();

            //Ejercicio para los campos de Fecha
            viewModel.EjercicioUsuario = SessionHelper.GetUsuario().Ejercicio;
        }

        public ActionResult RptSurtimientoOC(int id)
        {
            ReportHelper reportHelper = new ReportHelper();

            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@pTituloReporte", "Surtimiento de OC");
            parametros.Add("@pFechaCorte", new OrdenCompraReciboService().BuscaPorId(id).Fecha.Year);
            parametros.Add("@pNombreReporte", "rptSurtimientoOC");
            parametros.Add("@pReciboId", id);

            ViewBag.WebReport = reportHelper.GetWebReport("Almacen/Compras/ARrptSurtimientoOC.frx", parametros);

            return View("RptSurtimientoOC");
        }
    }
}