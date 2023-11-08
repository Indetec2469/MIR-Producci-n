using Newtonsoft.Json;
using RequisicionesAlmacen.Areas.Almacenes.Almacenes.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Almacenes.Almacenes.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.AJUSTE_DE_INVENTARIO)]
    public class InventariosAjustesController : BaseController<ARtblInventarioAjuste, InventarioAjusteViewModel>
    {
        private string API_FICHA = "/almacenes/almacenes/inventariosajustes/";

        public override ActionResult Nuevo()
        {
            //Crear un objeto nuevo
            ARtblInventarioAjuste inventarioAjuste = new ARtblInventarioAjuste();
            InventarioAjusteViewModel inventarioAjusteViewModel = new InventarioAjusteViewModel();

            //Inicializamos el ID en 0 para indicar que es un Registro Nuevo
            inventarioAjuste.InventarioAjusteId = 0;

            //Asignamos el modelo al modelView
            inventarioAjusteViewModel.InventarioAjuste = inventarioAjuste;

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref inventarioAjusteViewModel);

            //Retornamos la vista junto con su Objeto Modelo
            return View("InventarioAjuste", inventarioAjusteViewModel);
        }

        public override ActionResult Editar(int id)
        {
            InventarioAjusteViewModel inventarioAjusteViewModel = new InventarioAjusteViewModel();

            //Buscamos el Objeto por el Id que se envio como parametro
            InventarioAjusteService inventarioAjusteService = new InventarioAjusteService();
            ARtblInventarioAjuste inventarioAjuste = inventarioAjusteService.BuscaPorId(id);

            //Asignamos el modelo al viewModel
            inventarioAjusteViewModel.InventarioAjuste = inventarioAjuste != null ? inventarioAjuste : new ARtblInventarioAjuste();

            if (inventarioAjuste == null)
            {
                //Asignamos el error
                SetViewBagError("El Ajuste de Inventario no existe. Favor de revisar.", API_FICHA + "listar");
            }
            else
            {
                //Asignamos los detalles
                List<ARspConsultaInventarioAjusteDetalles_Result> listDetalles = inventarioAjusteService.BuscaDetallesPorInventarioAjusteId(inventarioAjuste.InventarioAjusteId);
                inventarioAjusteViewModel.ListInventarioAjusteDetallesItems = listDetalles.ConvertAll(new Converter<ARspConsultaInventarioAjusteDetalles_Result, InventarioAjusteDetalleItem>(InventarioAjusteDetalleItem.ConvertTo));
            }

            //Retornamos la vista junto con su Objeto Modelo
            return View("InventarioAjuste", inventarioAjusteViewModel);
        }

        [JsonException]
        public override JsonResult Guardar(ARtblInventarioAjuste InventarioAjuste)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult GuardaCambios(List<InventarioAjusteDetalleItem> existencias)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;

            InventarioAjusteService service = new InventarioAjusteService();

            ARtblInventarioAjuste inventarioAjuste = new ARtblInventarioAjuste();

            inventarioAjuste.CreadoPorId = usuarioId;
            inventarioAjuste.CantidadArticulosAfectados = existencias.Count;
            inventarioAjuste.MontoAjuste = 0;

            Dictionary<string, string> almacenes = new Dictionary<string, string>();

            List<ARtblInventarioAjusteDetalle> detalles = new List<ARtblInventarioAjusteDetalle>();

            foreach (InventarioAjusteDetalleItem registro in existencias)
            {
                ARtblInventarioAjusteDetalle detalle = (ARtblInventarioAjusteDetalle)registro;

                if (!almacenes.ContainsKey(registro.AlmacenId))
                {
                    almacenes.Add(registro.AlmacenId, registro.Almacen);
                }

                detalle.CreadoPorId = usuarioId;

                inventarioAjuste.MontoAjuste += detalle.Cantidad * detalle.CostoUnitario * (registro.TipoMovimientoId == TipoMovimiento.DISMINUYE ? -1 : 1);

                detalles.Add(detalle);
            }

            foreach (KeyValuePair<string, string> almacen in almacenes)
            {
                if (new InventarioFisicoService().ExisteInventarioIniciado(almacen.Key))
                {
                    throw new Exception("Existe un inventario iniciado para el almacén [" + almacen.Value + "].");
                }
            }

            int inventarioAjusteId = service.GuardaCambios(inventarioAjuste, detalles);
            tblPoliza poliza = new PolizaService().BuscaPolizaPorReferenciaYTipoMovimientoId(inventarioAjusteId, tblTipoMovimiento.AJUSTE_INVENTARIO);


            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "inventarioAjusteId", inventarioAjusteId },
                { "poliza", poliza == null ? "SIN POLIZA" : poliza.Poliza }
            };

            //Retornamos el data si todo salio correctamente
            return Json(data);
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

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Listar()
        {
            InventarioAjusteViewModel inventarioAjusteViewModel = new InventarioAjusteViewModel();

            inventarioAjusteViewModel.ListInventarioAjuste = new InventarioAjusteService().BuscaListado();

            return View("ListadoInventariosAjustes", inventarioAjusteViewModel);
        }

        protected override void GetDatosFicha(ref InventarioAjusteViewModel viewModel)
        {
            viewModel.ListAlmacenes = new AlmacenService().BuscaTodos();
            viewModel.ListTiposMovimiento = new ControlMaestroService().BuscaControl("TipoMovimiento");
            viewModel.FechaOperacion = new ConfiguracionEnteService().BuscaFechaOperacion();
        }

        [JsonException]
        public ContentResult BuscaProductosPorAlmacenId(string almacenId)
        {
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };

            var result = new ContentResult
            {
                Content = serializer.Serialize(new ProductoService().BuscaProductosPorAlmacenId(almacenId)),
                ContentType = "application/json"
            };

            return result;
        }

        [JsonException]
        public JsonResult BuscaArticuloPorId(string productoId)
        {
            return Json(new ProductoService().BuscaArticuloPorId(productoId));
        }

        [JsonException]
        public JsonResult BuscaConceptosAjustePorTipoMovimientoId(int tipoMovimientoId)
        {
            return Json(new ControlMaestroConceptoAjusteInventarioService().BuscaConceptosAjustePorTipoMovimientoId(tipoMovimientoId));
        }
    }
}