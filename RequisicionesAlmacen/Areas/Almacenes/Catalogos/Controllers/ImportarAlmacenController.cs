using LinqToExcel;
using Newtonsoft.Json;
using RequisicionesAlmacen.Areas.Almacenes.Catalogos.Models;
using RequisicionesAlmacen.Areas.Almacenes.Catalogos.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.Almacen;
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Almacenes.Catalogos.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.IMPORTAR_ALMACEN_PRODUCTOS)]
    public class ImportarAlmacenController : BaseController<ImportarAlmacenProductoItem, ImportarAlmacenViewModel>
    {
        public ActionResult Index()
        {
            //Crear un objeto nuevo
            ImportarAlmacenViewModel viewModel = new ImportarAlmacenViewModel();
            viewModel.FechaOperacion = new ConfiguracionEnteService().BuscaFechaOperacion();

            //Retornamos la vista junto con su Objeto Modelo
            return View("ImportarAlmacen", viewModel);
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Guardar(ImportarAlmacenProductoItem modelo)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult GuardaCambios(List<ImportarAlmacenProductoItem> productos)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;

            List<ARudtImportarAlmacenProducto> movimientos = new List<ARudtImportarAlmacenProducto>();

            //Agregamos los detalles
            foreach (ImportarAlmacenProductoItem producto in productos)
            {
                movimientos.Add((ARudtImportarAlmacenProducto)producto);
            }

            //Guardamos los cambios
            new InventarioFisicoService().CargaInventarioInicial(movimientos, usuarioId);

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Registro guardado con Exito!");
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Listar()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ImportarAlmacenViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        [HttpPost]
        public JsonResult LeerArchivo(HttpPostedFileBase file)
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

            ArchivoService archivoService = new ArchivoService();

            string nombreArchivoTmp = archivoService.GuardaArchivoTemporal(fileName, fileInputStream);
            string ruta = archivoService.GetRutaTmpCMM() + archivoService.GetFileSeparator() + nombreArchivoTmp;

            var excelData = new ExcelQueryFactory(ruta);
            var query = excelData.Worksheet("Importación Material Suministro");
            int filaInicial = 0;

            List<ImportarAlmacenProductoItem> productos = new List<ImportarAlmacenProductoItem>();

            try
            {
                foreach (var line in query)
                {
                    if (filaInicial >= 5 && line[0].ToString().CompareTo("") != 0)
                    {
                        ImportarAlmacenProductoItem item = new ImportarAlmacenProductoItem(line);

                        item.Errores = new InventarioFisicoService().InventarioInicialValidarFila(item.ProductoId,
                                                                                                  item.AlmacenId,
                                                                                                  item.FuenteFinanciamientoId,
                                                                                                  item.ProyectoId,
                                                                                                  item.UnidadAdministrativaId,
                                                                                                  item.TipoGastoId,
                                                                                                  item.Cantidad.ToString(),
                                                                                                  item.CostoUnitario.ToString());

                        productos.Add(item);
                    }

                    filaInicial++;
                }
                
                archivoService.DeleteArchivo(ruta);
            }
            catch (Exception ex)
            {
                archivoService.DeleteArchivo(ruta);
                
                throw new Exception("Error al leer el archivo.");
            }

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json(productos);
        }
    }
}