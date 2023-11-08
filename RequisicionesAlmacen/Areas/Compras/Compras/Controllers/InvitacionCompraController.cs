using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using RequisicionesAlmacen.Areas.Compras.Compras.Models;
using RequisicionesAlmacen.Areas.Compras.Compras.Models.ViewModel;
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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Compras.Compras.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.INVITACION_DE_COMPRA)]
    public class InvitacionCompraController : BaseController<ARtblInvitacionCompra, InvitacionCompraViewModel>
    {
        private string API_FICHA = "/compras/compras/invitacioncompra/";

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        public override ActionResult Editar(int id)
        {
            return ViewModel(id, null);
        }

        public ActionResult Ver(int id)
        {
            return ViewModel(id, true);
        }

        public ActionResult ViewModel(int id, Nullable<bool> soloLectura)
        {
            InvitacionCompraService service = new InvitacionCompraService();

            //Creamos el objeto
            InvitacionCompraViewModel viewModel = new InvitacionCompraViewModel();

            //Buscamos el Objeto por el Id que se envio como parametro
            ARtblInvitacionCompra invitacionCompra = service.BuscaPorId(id);

            //Asignamos el Objeto al ViewModel
            viewModel.InvitacionCompra = invitacionCompra != null ? invitacionCompra : new ARtblInvitacionCompra();

            if (invitacionCompra == null)
            {
                //Asignamos el error
                SetViewBagError("La Invitación de Compra no existe. Favor de revisar.", API_FICHA + "listar");
            }
            else
            {
                //Asignamos el string de la fecha
                invitacionCompra.FechaInvitacion = new SistemaService().GetFechaConFormato(invitacionCompra.Fecha);
                invitacionCompra.FechaDesiertaS = invitacionCompra.FechaDesierta != null ? new SistemaService().GetFechaConFormato(invitacionCompra.FechaDesierta.GetValueOrDefault())  : null;

                //Asignamos los detalles
                viewModel.ListInvitacionCompraDetalles = service.BuscaDetallesPorInvitacionCompraId(invitacionCompra.InvitacionCompraId);

                //Agregamos todos los datos necesarios para el funcionamiento de la ficha
                //como son los Listados para combos, tablas, arboles.
                GetDatosFicha(ref viewModel);

                //Asignamos el string del numero de proveedores
                int contadorProveedores = 0;

                foreach (var proveedor in viewModel.ListProveedores)
                {
                    if (proveedor.Seleccionado.GetValueOrDefault())
                    {
                        contadorProveedores++;
                    }
                }

                invitacionCompra.Proveedores = contadorProveedores;

                //Validamos si alún registro permite editar
                bool permiteEditar = false;

                if (soloLectura == null
                    && !invitacionCompra.EstatusId.Equals(AREstatusInvitacionCompra.CANCELADA)
                    && !invitacionCompra.EstatusId.Equals(AREstatusInvitacionCompra.FINALIZADA))
                {
                    foreach (ARspConsultaInvitacionCompraDetalles_Result detalle in viewModel.ListInvitacionCompraDetalles)
                    {
                        if (detalle.PermiteEditar.GetValueOrDefault())
                        {
                            permiteEditar = true;
                        }
                    }
                }

                //Modo Solo Lectura
                viewModel.SoloLectura = soloLectura == null ? !permiteEditar : soloLectura.GetValueOrDefault();

                //Validamos si se podrán editar los proveedores
                foreach(ARspConsultaInvitacionCompraListadoProveedores_Result proveedor in viewModel.ListProveedores.ToList())
                {
                    viewModel.DeshabilitarProveedores = proveedor.InvitacionCompraProveedorId > 0 || viewModel.DeshabilitarProveedores;
                }
            }

            //Retornamos la vista junto con su Objeto Modelo
            return View("InvitacionCompra", viewModel);
        }

        [JsonException]
        public override JsonResult Guardar(ARtblInvitacionCompra invitacionCompra)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult GuardaCambios(ARtblInvitacionCompra invitacionCompra,
                                        List<InvitacionCompraDetalleItem> detallesEliminados,
                                        List<InvitacionCompraProveedorItem> proveedoresInvitados,
                                        List<ArtblInvitacionCompraDetallePrecioProveedor> preciosProveedor,
                                        List<InvitacionCompraProveedorCotizacionItem> cotizacionesEliminadas)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            InvitacionCompraService service = new InvitacionCompraService();

            //Verificamos que no se haya modidicado
            ARtblInvitacionCompra temp = service.BuscaPorId(invitacionCompra.InvitacionCompraId);

            if (!StructuralComparisons.StructuralEqualityComparer.Equals(invitacionCompra.Timestamp, temp.Timestamp))
            {
                throw new Exception("La Invitación con el código [" + temp.CodigoInvitacion + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            //Actualizamos la cabecera
            invitacionCompra.ModificadoPorId = usuarioId;
            invitacionCompra.FechaUltimaModificacion = fecha;

            List<ARtblInvitacionCompraDetalle> detallesCancelados = new List<ARtblInvitacionCompraDetalle>();

            if (detallesEliminados != null)
            {
                foreach (InvitacionCompraDetalleItem registro in detallesEliminados)
                {
                    ARtblInvitacionCompraDetalle detalle = (ARtblInvitacionCompraDetalle)registro;

                    //Actualizamos los detalles cancelados
                    detalle.EstatusId = AREstatusInvitacionCompraDetalle.CANCELADO;
                    detalle.ModificadoPorId = usuarioId;
                    detalle.FechaUltimaModificacion = fecha;

                    detallesCancelados.Add(detalle);
                }
            }

            List<ARtblInvitacionCompraProveedor> proveedoresInvitadosTemp = new List<ARtblInvitacionCompraProveedor>();

            if (proveedoresInvitados != null)
            {
                foreach (InvitacionCompraProveedorItem registro in proveedoresInvitados)
                {
                    ARtblInvitacionCompraProveedor proveedor = (ARtblInvitacionCompraProveedor)registro;

                    //Si es un registro nuevo
                    if (proveedor.InvitacionCompraProveedorId < 0)
                    {
                        proveedor.EstatusId = EstatusRegistro.ACTIVO;
                        proveedor.CreadoPorId = usuarioId;
                    }

                    //Actualizamos los detalles cancelados
                    else
                    {
                        proveedor.ModificadoPorId = usuarioId;
                        proveedor.FechaUltimaModificacion = fecha;
                    }

                    if (proveedor.Cotizaciones != null)
                    {
                        foreach (ArtblInvitacionCompraProveedorCotizacion cotizacion in proveedor.Cotizaciones)
                        {
                            cotizacion.EstatusId = EstatusRegistro.ACTIVO;

                            //Si es un registro nuevo
                            if (cotizacion.CotizacionId == null)
                            {
                                cotizacion.CreadoPorId = usuarioId;
                            }
                            else
                            {
                                cotizacion.ModificadoPorId = usuarioId;
                                cotizacion.FechaUltimaModificacion = fecha;
                            }
                        }
                    }

                    proveedoresInvitadosTemp.Add(proveedor);
                }
            }

            List<int> prospectosProveedorGanadores = new List<int>();

            if (preciosProveedor != null)
            {
                foreach (ArtblInvitacionCompraDetallePrecioProveedor precioProveedor in preciosProveedor)
                {
                    //Verificamos si es registro nuevo
                    if (precioProveedor.InvitacionCompraDetallePrecioProveedorId < 0)
                    {
                        precioProveedor.CreadoPorId = usuarioId;
                    }

                    //De lo contrario actualizamos el registro
                    else
                    {
                        precioProveedor.ModificadoPorId = usuarioId;
                        precioProveedor.FechaUltimaModificacion = fecha;
                    }

                    //Agregamos el Id de los Prospectos a convertir en Proveedor
                    if (precioProveedor.ProveedorId > 2000000 
                        && precioProveedor.Ganador.GetValueOrDefault()
                        && !prospectosProveedorGanadores.Contains(precioProveedor.ProveedorId))
                    {
                        prospectosProveedorGanadores.Add(precioProveedor.ProveedorId);
                    }
                }
            }

            List<ArtblInvitacionCompraProveedorCotizacion> cotizaciones = new List<ArtblInvitacionCompraProveedorCotizacion>();

            if (cotizacionesEliminadas != null)
            {
                foreach (InvitacionCompraProveedorCotizacionItem cotizacion in cotizacionesEliminadas)
                {
                    ArtblInvitacionCompraProveedorCotizacion cotizacionTemp = (ArtblInvitacionCompraProveedorCotizacion)cotizacion;

                    cotizacionTemp.EstatusId = EstatusRegistro.BORRADO;
                    cotizacionTemp.ModificadoPorId = usuarioId;
                    cotizacionTemp.FechaUltimaModificacion = fecha;

                    cotizaciones.Add(cotizacionTemp);
                }
            }

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json(service.GuardaCambios(invitacionCompra,
                                              detallesCancelados,
                                              null,
                                              proveedoresInvitadosTemp,
                                              preciosProveedor,
                                              cotizaciones,
                                              null,
                                              prospectosProveedorGanadores), "Registro guardado con Exito!");
        }

        [JsonException]
        public JsonResult ConvertirOC(ARtblInvitacionCompra invitacionCompra, InvitacionOrdenCompraItem invitacionOC)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            InvitacionCompraService service = new InvitacionCompraService();

            //Verificamos que no se haya modidicado
            ARtblInvitacionCompra temp = service.BuscaPorId(invitacionCompra.InvitacionCompraId);

            if (!StructuralComparisons.StructuralEqualityComparer.Equals(invitacionCompra.Timestamp, temp.Timestamp))
            {
                throw new Exception("La Invitación con el código [" + temp.CodigoInvitacion + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            int ordenCompraId = service.ConvertirOC((tblOrdenCompra)invitacionOC, usuarioId, fecha);

            string poliza = new PolizaService().BuscaPolizaPorReferenciaTipoMovimientoIdYTipo(ordenCompraId, tblTipoMovimiento.ORDEN_COMPRA, "A").Poliza;

            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "ordenCompraId", ordenCompraId },
                { "poliza", poliza }
            };

            //Retornamos el data si todo salio correctamente
            return Json(data);
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult EnviarDesierta(ARtblInvitacionCompra invitacionCompra)
        {
            InvitacionCompraService service = new InvitacionCompraService();

            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            ARtblInvitacionCompra temp = new ARtblInvitacionCompra().GetModelo(service.BuscaPorId(invitacionCompra.InvitacionCompraId));

            if (!StructuralComparisons.StructuralEqualityComparer.Equals(invitacionCompra.Timestamp, temp.Timestamp))
            {
                throw new Exception("La Invitación con el código [" + temp.CodigoInvitacion + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            temp.EstatusId = AREstatusInvitacionCompra.DESIERTA;
            temp.FechaDesierta = invitacionCompra.FechaDesierta;
            temp.Observacion = invitacionCompra.Observacion;
            temp.ModificadoPorId = usuarioId;
            temp.FechaUltimaModificacion = fecha;

            List<ARtblInvitacionCompraDetalle> detallesTemp = service.BuscaDetallesNoCancelados(temp.InvitacionCompraId);
            List<ARtblInvitacionCompraDetalle> detalles = new List<ARtblInvitacionCompraDetalle>();
            List<ARtblRequisicionMaterialDetalle> requisicionDetalles = new List<ARtblRequisicionMaterialDetalle>();

            foreach (ARtblInvitacionCompraDetalle detalle in detallesTemp)
            {
                detalle.EstatusId = AREstatusInvitacionCompraDetalle.DESIERTO;
                detalle.ModificadoPorId = usuarioId;
                detalle.FechaUltimaModificacion = fecha;

                detalles.Add(new ARtblInvitacionCompraDetalle().GetModelo(detalle));

                ARtblInvitacionArticuloDetalle invitacionArticuloDetalle = new InvitacionArticuloService().BuscaDetalleInvitadoPorId(detalle.InvitacionArticuloDetalleId);

                if (invitacionArticuloDetalle != null)
                {
                    ARtblRequisicionMaterialDetalle requisicionMaterialDetalle = new ARtblRequisicionMaterialDetalle().GetModelo(new RequisicionMaterialService().BuscaDetallePorId(invitacionArticuloDetalle.RequisicionMaterialDetalleId));

                    requisicionMaterialDetalle.EstatusId = ControlMaestroMapeo.AREstatusRequisicionDetalle.POR_COMPRAR;
                    requisicionMaterialDetalle.FechaUltimaModificacion = fecha;
                    requisicionMaterialDetalle.ModificadoPorId = usuarioId;

                    requisicionDetalles.Add(requisicionMaterialDetalle);
                }
            }
            
            service.GuardaCambios(temp, detalles, requisicionDetalles);

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Registro eliminado con Exito!");
        }

        [JsonException]
        public JsonResult Rechazar(ARtblInvitacionCompra invitacionCompra)
        {
            InvitacionCompraService service = new InvitacionCompraService();

            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            ARtblInvitacionCompra temp = new ARtblInvitacionCompra().GetModelo(service.BuscaPorId(invitacionCompra.InvitacionCompraId));

            if (!StructuralComparisons.StructuralEqualityComparer.Equals(invitacionCompra.Timestamp, temp.Timestamp))
            {
                throw new Exception("La Invitación con el código [" + temp.CodigoInvitacion + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            temp.EstatusId = AREstatusInvitacionCompra.RECHAZADA;
            temp.Observacion = invitacionCompra.Observacion;
            temp.ModificadoPorId = usuarioId;
            temp.FechaUltimaModificacion = fecha;

            List<ARtblInvitacionCompraDetalle> detallesTemp = service.BuscaDetallesNoCancelados(temp.InvitacionCompraId);
            List<ARtblInvitacionCompraDetalle> detalles = new List<ARtblInvitacionCompraDetalle>();
            List<ARtblRequisicionMaterialDetalle> requisicionDetalles = new List<ARtblRequisicionMaterialDetalle>();

            foreach (ARtblInvitacionCompraDetalle detalle in detallesTemp)
            {
                detalle.EstatusId = AREstatusInvitacionCompraDetalle.RECHAZADO;
                detalle.ModificadoPorId = usuarioId;
                detalle.FechaUltimaModificacion = fecha;

                detalles.Add(new ARtblInvitacionCompraDetalle().GetModelo(detalle));

                ARtblInvitacionArticuloDetalle invitacionArticuloDetalle = new InvitacionArticuloService().BuscaDetalleInvitadoPorId(detalle.InvitacionArticuloDetalleId);

                if (invitacionArticuloDetalle != null)
                {
                    ARtblRequisicionMaterialDetalle requisicionMaterialDetalle = new ARtblRequisicionMaterialDetalle().GetModelo(new RequisicionMaterialService().BuscaDetallePorId(invitacionArticuloDetalle.RequisicionMaterialDetalleId));

                    requisicionMaterialDetalle.EstatusId = ControlMaestroMapeo.AREstatusRequisicionDetalle.RECHAZADO;
                    requisicionMaterialDetalle.FechaUltimaModificacion = fecha;
                    requisicionMaterialDetalle.ModificadoPorId = usuarioId;

                    requisicionDetalles.Add(requisicionMaterialDetalle);
                }
            }

            service.GuardaCambios(temp, detalles, requisicionDetalles);

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Registro eliminado con Exito!");
        }

        [JsonException]
        public JsonResult Cancelar(ARtblInvitacionCompra invitacionCompra)
        {
            InvitacionCompraService service = new InvitacionCompraService();

            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            ARtblInvitacionCompra temp = new ARtblInvitacionCompra().GetModelo(service.BuscaPorId(invitacionCompra.InvitacionCompraId));

            if (!StructuralComparisons.StructuralEqualityComparer.Equals(invitacionCompra.Timestamp, temp.Timestamp))
            {
                throw new Exception("La Invitación con el código [" + temp.CodigoInvitacion + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            temp.EstatusId = AREstatusInvitacionCompra.CANCELADA;
            temp.ModificadoPorId = usuarioId;
            temp.FechaUltimaModificacion = fecha;

            List<ARtblInvitacionCompraDetalle> detallesTemp = service.BuscaDetallesNoCancelados(temp.InvitacionCompraId);
            List<ARtblInvitacionCompraDetalle> detalles = new List<ARtblInvitacionCompraDetalle>();
            List<ARtblInvitacionArticuloDetalle> invitacionArticuloDetalles = new List<ARtblInvitacionArticuloDetalle>();

            foreach (ARtblInvitacionCompraDetalle detalle in detallesTemp)
            {
                //detalle.EstatusId = AREstatusInvitacionCompraDetalle.CANCELADO;
                detalle.ModificadoPorId = usuarioId;
                detalle.FechaUltimaModificacion = fecha;

                detalles.Add(new ARtblInvitacionCompraDetalle().GetModelo(detalle));

                ARtblInvitacionArticuloDetalle invitacionArticuloDetalle = new InvitacionArticuloService().BuscaDetalleInvitadoPorId(detalle.InvitacionArticuloDetalleId);

                if (invitacionArticuloDetalle != null)
                {
                    invitacionArticuloDetalle.EstatusId = AREstatusInvitacionArticuloDetalle.POR_INVITAR;
                    invitacionArticuloDetalle.ModificadoPorId = usuarioId;
                    invitacionArticuloDetalle.FechaUltimaModificacion = fecha;

                    invitacionArticuloDetalles.Add(new ARtblInvitacionArticuloDetalle().GetModelo(invitacionArticuloDetalle));
                }
            }

            service.GuardaCambios(temp, detalles, invitacionArticuloDetalles);

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Registro eliminado con Exito!");
        }

        public override ActionResult Listar()
        {
            InvitacionCompraViewModel viewModel = new InvitacionCompraViewModel();

            viewModel.ListInvitacionCompra = new InvitacionCompraService().BuscaListado();
            
            return View("ListadoInvitacionCompra", viewModel);
        }

        protected override void GetDatosFicha(ref InvitacionCompraViewModel viewModel)
        {
            InvitacionCompraService invitacionCompraService = new InvitacionCompraService();
            int invitacionCompraId = viewModel.InvitacionCompra.InvitacionCompraId;

            //Datos de Invitación de OC
            viewModel.InvitacionCompra.Estatus = AREstatusInvitacionCompra.Nombre[viewModel.InvitacionCompra.EstatusId];
            viewModel.ListProveedores = invitacionCompraService.BuscaInvitacionCompraListadoProveedores(invitacionCompraId);
            viewModel.ListAlmacenes = new AlmacenService().BuscaTodos();
            viewModel.ListPreciosProveedores = new InvitacionCompraDetallePrecioProveedorService().BuscaInvitacionCompraListadoPreciosProveedores(invitacionCompraId);
            viewModel.ListProveedoresCotizaciones = new InvitacionCompraProveedorCotizacionService().BuscaInvitacionCompraListadoCotizaciones(invitacionCompraId);
            viewModel.ListOrdenesCompra = invitacionCompraService.BuscaInvitacionCompraOrdenesCompra(invitacionCompraId);
            viewModel.ListTarifasImpuesto = new TarifaImpuestoService().BuscaTodos();
            viewModel.ListMontosCompra = new ControlMaestroConfiguracionMontoCompraService().BuscaTodos();
            viewModel.FechaOperacion = new ConfiguracionEnteService().BuscaFechaOperacion();

            //Ejercicio para los campos de Fecha
            viewModel.EjercicioUsuario = SessionHelper.GetUsuario().Ejercicio;

            if (viewModel.ListInvitacionCompraDetalles != null && viewModel.ListInvitacionCompraDetalles.Count() > 0)
            {
                viewModel.TipoCompraId = new InvitacionArticuloService().BuscaPorId(new InvitacionArticuloService().BuscaDetallePorId(viewModel.ListInvitacionCompraDetalles.ElementAt(0).InvitacionArticuloDetalleId).InvitacionArticuloId).TipoCompraId;
            }
        }

        [JsonException]
        [HttpPost]
        public JsonResult GuardaArchivoTemporal(HttpPostedFileBase file)
        {
            string fileName = null;
            Stream fileInputStream = null;

            if (file != null)
            {
                // Checking for Internet Explorer  
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fileName = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fileName = file.FileName;
                }

                fileInputStream = file.InputStream;
            }

            string nombreArchivoTemp = new ArchivoService().GuardaArchivoTemporal(fileName, fileInputStream);

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json(nombreArchivoTemp);
        }

        [JsonException]
        public ActionResult BuscarArchivo(Nullable<Guid> archivoId, string nombreArchivoTmp, string nombreOriginal)
        {
            Dictionary<string, byte[]> diccionarioArchivo = null;

            if (nombreArchivoTmp != null && !nombreArchivoTmp.Equals(""))
            {
                diccionarioArchivo = new ArchivoService().Download(nombreArchivoTmp, nombreOriginal);
            }
            else if (archivoId != null)
            {
                diccionarioArchivo = new ArchivoService().Download(archivoId);                
            }

            if (diccionarioArchivo != null)
            {
                foreach (KeyValuePair<string, byte[]> archivo in diccionarioArchivo)
                {
                    Session["InvitacionCompra_NombreArchivo"] = archivo.Key;
                    Session["InvitacionCompra_DescargarArchivo"] = archivo.Value;
                }
            }
            else 
            {
                throw new Exception("No se pudo descargar el archivo.");
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [JsonException]
        public ActionResult DescargarArchivo()
        {
            Object archivo = Session["InvitacionCompra_DescargarArchivo"];

            if (archivo != null)
            {
                return File(archivo as byte[], "application/octet-stream", Session["InvitacionCompra_NombreArchivo"].ToString());
            }
            else
            {
                throw new Exception("No se pudo descargar el archivo.");
            }
        }

        [JsonException]
        public ActionResult BuscarReportePreciosProveedores(int invitacionCompraId)
        {
            //Obtenemos los datos de la Entidad
            Entidad entidad = new ReportHelper().GetDatosEntidad();

            //Obtenemos todos los proveedores
            List<tblProveedor> proveedores = new InvitacionCompraProveedorService().BuscaProveedores(invitacionCompraId);
            int noProveedores = proveedores.Count;

            if (noProveedores > 0)
            {
                string columnaFinalTitulo = GetLetraPorPosicion(noProveedores);
                string penultimaColumna = GetLetraPorPosicion(noProveedores + 1);
                string ultimaColumna = GetLetraPorPosicion(noProveedores + 2);

                //Obtenemos los detalles de la Invitación de Compra
                List<ARtblInvitacionCompraDetalle> detalles = new InvitacionCompraService().BuscaDetallesNoCancelados(invitacionCompraId);

                //Obtenemos los Precios de los Proveedores
                List<ARspConsultaInvitacionCompraDetallePreciosProveedores_Result> preciosProveedores = new InvitacionCompraDetallePrecioProveedorService().BuscaInvitacionCompraListadoPreciosProveedores(invitacionCompraId);

                FileInfo logotipo = new ArchivoService().GetImagenLogotipo("saacg-net.png");
                string fechaYhora = new SistemaService().GetFechaConFormato(DateTime.Now, true);
                string fecha = new SistemaService().GetFechaConFormato(DateTime.Now);

                using (var excelPackage = new ExcelPackage(new MemoryStream()))
                {
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Kárdex");
                    ExcelHelper excelHelper = new ExcelHelper();
                    ExcelRange excelRange = null;

                    excelWorksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                    excelWorksheet.PrinterSettings.PaperSize = ePaperSize.Legal;

                    //Nombre del Ente Público
                    excelRange = excelWorksheet.Cells["C1:" + columnaFinalTitulo + "1"];
                    excelRange.Value = entidad.Nombre;
                    excelHelper.EncabezadoEnteStyle(ref excelRange);

                    //Estado
                    excelRange = excelWorksheet.Cells["C2:" + columnaFinalTitulo + "2"];
                    excelRange.Value = entidad.Estado;
                    excelHelper.EncabezadoEstadoStyle(ref excelRange);

                    //Título del Reporte
                    excelRange = excelWorksheet.Cells["C3:" + columnaFinalTitulo + "3"];
                    excelRange.Value = "PRECIOS PROVEEDORES";
                    excelHelper.EncabezadoTituloReporteStyle(ref excelRange);

                    //Espacio
                    excelRange = excelWorksheet.Cells["A4:" + ultimaColumna + "4"];
                    excelHelper.ExcelTitulo(ref excelRange);

                    //Usuario
                    excelRange = excelWorksheet.Cells["A5"];
                    excelRange.Value = "Usuario:";
                    excelHelper.EncabezadoDatosTituloStyle(ref excelRange);
                    excelRange = excelWorksheet.Cells["B5:" + columnaFinalTitulo + "5"];
                    excelRange.Value = SessionHelper.GetUsuario().NombreUsuario;
                    excelHelper.EncabezadoDatosStyle(ref excelRange);

                    //Nombre Reporte
                    excelRange = excelWorksheet.Cells["A6"];
                    excelRange.Value = "Reporte:";
                    excelHelper.EncabezadoDatosTituloStyle(ref excelRange);
                    excelRange = excelWorksheet.Cells["B6:" + columnaFinalTitulo + "6"];
                    excelRange.Value = "rptPreciosProveedores";
                    excelHelper.EncabezadoDatosStyle(ref excelRange);

                    //Invitación de Compra
                    excelRange = excelWorksheet.Cells["A7"];
                    excelRange.Value = "Código Invitación:";
                    excelHelper.EncabezadoDatosTituloStyle(ref excelRange);
                    excelRange = excelWorksheet.Cells["B7:" + columnaFinalTitulo + "7"];
                    excelRange.Value = new InvitacionCompraService().BuscaPorId(invitacionCompraId).CodigoInvitacion;
                    excelHelper.EncabezadoDatosStyle(ref excelRange);

                    //Fecha Impresión
                    excelRange = excelWorksheet.Cells[penultimaColumna + "5"];
                    excelRange.Value = "Fecha y";
                    excelHelper.EncabezadoDatosTituloStyle(ref excelRange);
                    excelRange = excelWorksheet.Cells[ultimaColumna + "5"];
                    excelRange.Value = fecha;
                    excelHelper.EncabezadoDatosStyle(ref excelRange);

                    //Hora Impresión
                    excelRange = excelWorksheet.Cells[penultimaColumna + "6"];
                    excelRange.Value = "hora de Impresión";
                    excelHelper.EncabezadoDatosTituloStyle(ref excelRange);
                    excelRange = excelWorksheet.Cells[ultimaColumna + "6"];
                    excelRange.Value = fechaYhora.Replace(fecha + " ", "");
                    excelHelper.EncabezadoDatosStyle(ref excelRange);

                    //Logotipo
                    if (logotipo != null)
                    {
                        ExcelPicture excelLogotipo = excelWorksheet.Drawings.AddPicture("SAACG.NET", logotipo);

                        excelLogotipo.SetSize(150, 90);
                        excelLogotipo.SetPosition(0, 10, 0, 30);
                    }

                    //Espacio
                    excelRange = excelWorksheet.Cells["A8:" + ultimaColumna + "8"];
                    excelHelper.ExcelTitulo(ref excelRange);

                    //Agregar Titulos de Columnas
                    excelWorksheet.Row(1).Height = 25;
                    excelWorksheet.Row(2).Height = 25;
                    excelWorksheet.Row(3).Height = 25;
                    excelWorksheet.Row(9).Height = 30;

                    excelRange = excelWorksheet.Cells["A9"];
                    excelRange.Value = "Producto ID";
                    excelHelper.TablaColumnaStyle(ref excelRange, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center);

                    excelRange = excelWorksheet.Cells["B9"];
                    excelRange.Value = "Descripción";
                    excelHelper.TablaColumnaStyle(ref excelRange, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center);

                    for (int i = 1; i <= noProveedores; i++)
                    {
                        excelRange = excelWorksheet.Cells[GetLetraPorPosicion(i + 2) + 9];
                        excelRange.Value = proveedores[i - 1].ProveedorId + " - " + proveedores[i - 1].RazonSocial;
                        excelHelper.TablaColumnaStyle(ref excelRange, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center);
                    }

                    //Border
                    excelRange = excelWorksheet.Cells["A9:" + ultimaColumna + "9"];
                    excelRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //Variable contador iniciar rows
                    int iniciarEn = 10;

                    //Agrgamos cada fila del reporte
                    for (int i = 0; i < detalles.Count; i++)
                    {
                        excelRange = excelWorksheet.Cells["A" + iniciarEn];
                        excelRange.Value = detalles[i].ProductoId;
                        excelHelper.TablaDatoStyle(ref excelRange, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center);

                        excelRange = excelWorksheet.Cells["B" + iniciarEn];
                        excelRange.Value = detalles[i].Descripcion;
                        excelHelper.TablaDatoStyle(ref excelRange, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center);

                        for (int j = 1; j <= noProveedores; j++)
                        {
                            ARspConsultaInvitacionCompraDetallePreciosProveedores_Result precioProveedor
                                = preciosProveedores.Find(x => x.InvitacionCompraDetalleId == detalles[i].InvitacionCompraDetalleId
                                                            && x.ProveedorId == proveedores[j - 1].ProveedorId);

                            if (precioProveedor != null)
                            {
                                excelRange = excelWorksheet.Cells[GetLetraPorPosicion(j + 2) + iniciarEn];
                                excelRange.Value = precioProveedor.PrecioArticulo;
                                excelRange.Style.Numberformat.Format = "$ #,##0.00##";
                                excelHelper.TablaDatoStyle(ref excelRange, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center);
                            }
                        }

                        //Border
                        excelRange = excelWorksheet.Cells["A" + iniciarEn + ":" + ultimaColumna + iniciarEn];
                        excelRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                        iniciarEn++;
                    }

                    //Ajustar columnas
                    for (int i = 1; i <= (noProveedores + 2); i++)
                    {
                        excelWorksheet.Column(i).Width = (i == 1 ? 15 : 30);
                    }

                    //Añadimos el color de las celdas
                    excelRange = excelWorksheet.Cells["A1:" + ultimaColumna + iniciarEn];
                    excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    excelRange.Style.Fill.BackgroundColor.SetColor(Color.White);

                    Session["DescargarExcel_ReportePreciosProveedores"] = excelPackage.GetAsByteArray();
                }

                return Json(proveedores.Count, JsonRequestBehavior.AllowGet);
            }
            else
            {
                throw new Exception("El reporte no contiene registros que mostrar.");
            }
        }

        [JsonException]
        public ActionResult DescargarReportePreciosProveedores()
        {
            Object excel = Session["DescargarExcel_ReportePreciosProveedores"];

            if (excel != null)
            {
                return File(excel as byte[], "application/octet-stream", "ReportePreciosProveedor.xlsx");
            }
            else
            {
                throw new Exception("No se pudo generar el Reporte.");
            }
        }

        private string GetLetraPorPosicion(int pos)
        {
            string letra1 = "";
            string letra2 = "";

            if (pos <= 26)
            {
                letra1 = ((char)(pos + 64)).ToString();
            }
            else
            {
                letra1 = ((char)((pos / 26) + 64)).ToString();
                letra2 = ((char)((pos % 26) + 65)).ToString();
            }

            return letra1.ToUpper() + letra2.ToUpper();
        }
    }
}