using FastReport.Web;
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Almacenes.Almacenes.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.INVENTARIO_FISICO)]
    public class InventariosFisicosController : BaseController<ARtblInventarioFisico, InventarioFisicoViewModel>
    {
        private string API_FICHA = "/almacenes/almacenes/inventariosfisicos/";

        public override ActionResult Nuevo()
        {
            //Crear un objeto nuevo
            ARtblInventarioFisico inventarioFisico = new ARtblInventarioFisico();
            InventarioFisicoViewModel inventarioFisicoViewModel = new InventarioFisicoViewModel();

            //Inicializamos el ID en 0 para indicar que es un Registro Nuevo
            inventarioFisico.InventarioFisicoId = 0;

            //Asignamos el modelo al modelView
            inventarioFisicoViewModel.InventarioFisico = inventarioFisico;

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref inventarioFisicoViewModel);

            //Retornamos la vista junto con su Objeto Modelo
            return View("InventarioFisico", inventarioFisicoViewModel);
        }

        public override ActionResult Editar(int id)
        {
            InventarioFisicoViewModel inventarioFisicoViewModel = new InventarioFisicoViewModel();

            //Buscamos el Objeto por el Id que se envio como parametro
            InventarioFisicoService inventarioFisicoService = new InventarioFisicoService();
            ARtblInventarioFisico inventarioFisico = inventarioFisicoService.BuscaPorId(id);

            //Asignamos el modelo al viewModel
            inventarioFisicoViewModel.InventarioFisico = inventarioFisico != null ? inventarioFisico : new ARtblInventarioFisico();

            if (inventarioFisico == null)
            {
                //Asignamos el error
                SetViewBagError("El Inventario Físico no existe o está Cancelado. Favor de revisar.", API_FICHA + "listar");
            }
            else
            {
                //Asignamos los detalles
                List<ARspConsultaInventarioFisicoDetalles_Result> listDetalles = inventarioFisicoService.BuscaDetallesPorInventarioFisicoId(inventarioFisico.InventarioFisicoId);
                inventarioFisicoViewModel.ListExistenciasAlmacen = listDetalles.ConvertAll(new Converter<ARspConsultaInventarioFisicoDetalles_Result, ExistenciaAlmacen>(ExistenciaAlmacen.ConvertTo));

                //Agregamos todos los datos necesarios para el funcionamiento de la ficha
                //como son los Listados para combos, tablas, arboles.
                GetDatosFicha(ref inventarioFisicoViewModel);
            }

            //Retornamos la vista junto con su Objeto Modelo
            return View("InventarioFisico", inventarioFisicoViewModel);
        }

        [JsonException]
        public override JsonResult Guardar(ARtblInventarioFisico InventarioFisico)
        {
            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Registro guardado con Exito!");
        }

        [JsonException]
        public JsonResult GuardaCambios(ARtblInventarioFisico inventarioFisico, List<ExistenciaAlmacen> existencias)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            InventarioFisicoService service = new InventarioFisicoService();

            //Si es un nuevo registro llenamos el campo de creadoPor
            if (inventarioFisico.InventarioFisicoId == 0)
            {
                if (service.ExisteInventarioIniciado(inventarioFisico.AlmacenId))
                {
                    throw new Exception("Ya se ha iniciado un inventario para el almacén [" + inventarioFisico.Almacen + "].");
                }

                inventarioFisico.EstatusId = EstatusInventarioFisico.EN_PROCESO;
                inventarioFisico.CreadoPorId = usuarioId;
            }

            //De lo contrario llenamos el campo de ModificadoPor y Fecha de Ultima Modificacion
            else
            {
                ARtblInventarioFisico temp = service.BuscaPorId(inventarioFisico.InventarioFisicoId);

                if (!StructuralComparisons.StructuralEqualityComparer.Equals(inventarioFisico.Timestamp, temp.Timestamp))
                {
                    throw new Exception("El inventario con el código [" + inventarioFisico.Codigo + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                }

                inventarioFisico.ModificadoPorId = usuarioId;
                inventarioFisico.FechaUltimaModificacion = fecha;
            }
            
            inventarioFisico.MontoAjuste = 0;

            List<ARtblInventarioFisicoDetalle> detalles = new List<ARtblInventarioFisicoDetalle>();

            foreach (ExistenciaAlmacen registro in existencias)
            {
                ARtblInventarioFisicoDetalle detalle = (ARtblInventarioFisicoDetalle)registro;

                if (registro.InventarioFisicoDetalleId == 0)
                {
                    detalle.CreadoPorId = usuarioId;
                }
                else
                {
                    detalle.ModificadoPorId = SessionHelper.GetUsuario().UsuarioId;
                    detalle.FechaUltimaModificacion = fecha;

                    if (detalle.Conteo != null)
                    {
                        inventarioFisico.MontoAjuste += (detalle.Conteo - detalle.Existencia) * detalle.CostoPromedio;
                    }
                }                

                detalles.Add(detalle);
            }

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json(service.GuardaCambios(inventarioFisico, detalles), "Registro guardado con Exito!");
        }

        [JsonException]
        public JsonResult Afectar(ARtblInventarioFisico inventarioFisico)
        {
            new InventarioFisicoService().AfectaInventario(inventarioFisico.InventarioFisicoId, SessionHelper.GetUsuario().UsuarioId);

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Inventario afectado con Exito!");
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult Eliminar(ARtblInventarioFisico inventarioFisico)
        {
            InventarioFisicoService service = new InventarioFisicoService();

            ARtblInventarioFisico temp = service.BuscaPorId(inventarioFisico.InventarioFisicoId);

            if (!StructuralComparisons.StructuralEqualityComparer.Equals(inventarioFisico.Timestamp, temp.Timestamp))
            {
                throw new Exception("El inventario con el código [" + inventarioFisico.Codigo + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            inventarioFisico.EstatusId = EstatusInventarioFisico.CANCELADO;
            inventarioFisico.ModificadoPorId = SessionHelper.GetUsuario().UsuarioId;
            inventarioFisico.FechaUltimaModificacion = DateTime.Now;
            
            service.Elimina(inventarioFisico);

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Registro eliminado con Exito!");
        }

        public override ActionResult Listar()
        {
            InventarioFisicoViewModel inventarioFisicoViewModel = new InventarioFisicoViewModel();

            inventarioFisicoViewModel.ListInventarioFisico = new InventarioFisicoService().BuscaListado();

            return View("ListadoInventariosFisicos", inventarioFisicoViewModel);
        }

        protected override void GetDatosFicha(ref InventarioFisicoViewModel viewModel)
        {
            viewModel.ListAlmacenes = new AlmacenService().BuscaTodos();
            viewModel.ListGrupos = new ObjetoGastoService().BuscaObjetoGastoPorNivel("").ConvertAll(new Converter<tblObjetoGasto, ObjetoGastoItem>(ObjetoGastoItem.ConvertTo));
            viewModel.FechaOperacion = new ConfiguracionEnteService().BuscaFechaOperacion();
        }

        [JsonException]
        public JsonResult BuscaObjetoGastoPorNivel(List<string> iniciales, int nivel)
        {
            List<tblObjetoGasto> listsObjetosGasto = new List<tblObjetoGasto>();

            foreach (string item in iniciales)
            {
                string inicial = item.Substring(0, nivel);

                List<tblObjetoGasto> listsObjetosGastoTemp = nivel == 3 ? new ObjetoGastoService().BuscaPartidasEspecificas(inicial) : new ObjetoGastoService().BuscaObjetoGastoPorNivel(inicial);

                if (listsObjetosGastoTemp.Count > 0)
                {
                    listsObjetosGasto.AddRange(listsObjetosGastoTemp);
                }
            }

            return Json(listsObjetosGasto.ConvertAll(new Converter<tblObjetoGasto, ObjetoGastoItem>(ObjetoGastoItem.ConvertTo)));
        }

        [JsonException]
        public JsonResult BuscaProductosPorAlmacenObjetoGasto(string almacenId, List<string> objetosGastoId)
        {
            string objetoGastoIds = string.Join(",", objetosGastoId.ToArray());

            List<tblProducto> listProductos = new ProductoService().BuscaProductosPorAlmacenObjetoGasto(almacenId, objetoGastoIds);

            return Json(listProductos.ConvertAll(new Converter<tblProducto, ProductoItem>(ProductoItem.ConvertTo)));
        }

        [JsonException]
        public JsonResult ConsultaExistenciaAlmacen(string almacenId, string productosIds)
        {
            List<ARspConsultaExistenciaAlmacen_Result> listDetalles = new InventarioFisicoService().ConsultaExistenciaAlmacen(almacenId, productosIds);

            return Json(listDetalles.ConvertAll(new Converter<ARspConsultaExistenciaAlmacen_Result, ExistenciaAlmacen>(ExistenciaAlmacen.ConvertTo)));
        }

        public ActionResult RptLibroAlmacenPorProducto(string id)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@pTituloReporte", "Libro de Almacén por Producto");
            parametros.Add("@pNombreReporte", "rptLibroAlmacen");
            parametros.Add("@pInventarioId", id);

            WebReport webReport = new ReportHelper().GetWebReport("Almacen/InventarioFisico/ARrptInventarioFisico_por_articulo.frx", parametros);
            ViewBag.WebReport = webReport;

            return View("rptLibroAlmacen");
        }
        
        public ActionResult RptLibroAlmacenPorClave(string id)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@pTituloReporte", "Libro de Almacén por por Clave");
            parametros.Add("@pNombreReporte", "rptLibroAlmacen");
            parametros.Add("@pInventarioId", id);

            WebReport webReport = new ReportHelper().GetWebReport("Almacen/InventarioFisico/ARrptInventarioFisico_por_clave.frx", parametros);
            ViewBag.WebReport = webReport;

            return View("rptLibroAlmacen");
        }
    }
}