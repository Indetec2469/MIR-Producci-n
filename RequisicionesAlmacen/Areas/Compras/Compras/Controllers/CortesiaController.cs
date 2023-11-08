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
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Compras.Compras.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.RECIBO_DE_CORTESIAS)]
    public class CortesiaController : BaseController<ARtblCortesia, CortesiaViewModel>
    {
        private string API_FICHA = "/compras/compras/cortesia/";

        public override ActionResult Nuevo()
        {
            //Crear un objeto nuevo
            CortesiaViewModel viewModel = new CortesiaViewModel();

            //Asignamos el modelo al modelView
            ARtblCortesia modelo = new ARtblCortesia();

            modelo.CortesiaId = 0;
            modelo.Codigo = "RC000000";
            modelo.Fecha = DateTime.Now;
            modelo.AfectaCostoPromedio = false;

            viewModel.Cortesia = modelo;

            //Modo Solo Lectura
            viewModel.SoloLectura = false;

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref viewModel);

            //Retornamos la vista junto con su Objeto Modelo
            return View("Cortesia", viewModel);
        }

        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult Ver(int id)
        {
            CortesiaService service = new CortesiaService();

            //Creamos el objeto
            CortesiaViewModel viewModel = new CortesiaViewModel();

            //Buscamos el Objeto por el Id que se envio como parametro
            ARtblCortesia modelo = service.BuscaPorId(id);

            //Asignamos el Objeto al ViewModel
            viewModel.Cortesia = modelo != null ? modelo : new ARtblCortesia();

            if (modelo == null || !modelo.EstatusId.Equals(EstatusRegistro.ACTIVO))
            {
                //Asignamos el error
                SetViewBagError("La Cortesía no existe o está Cancelada. Favor de revisar.", API_FICHA + "listar");
            }
            else
            {
                //Asignamos la fecha
                modelo.FechaCortesia = service.GetFechaConFormato(modelo.Fecha);

                //Asignamos los detalles
                viewModel.ListCortesiaDetalles = service.BuscaDetallesPorCortesiaId(modelo.CortesiaId);

                //Agregamos todos los datos necesarios para el funcionamiento de la ficha
                //como son los Listados para combos, tablas, arboles.
                GetDatosFicha(ref viewModel);
            }

            //Modo Solo Lectura
            viewModel.SoloLectura = true;

            //Retornamos la vista junto con su Objeto Modelo
            return View("Cortesia", viewModel);
        }

        public override JsonResult Guardar(ARtblCortesia cortesia)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult GuardaCambios(ARtblCortesia cortesia, List<CortesiaDetalleItem> detalles)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            CortesiaService service = new CortesiaService();

            //Actualizamos la cabecera
            cortesia.EstatusId = EstatusRegistro.ACTIVO;
            cortesia.CreadoPorId = usuarioId;

            //Asignamos el numero de partida y el total
            int numeroPartida = 1;
            cortesia.TotalCortesia = 0;

            //Agregamos los detalles
            foreach (CortesiaDetalleItem detalleTemp in detalles)
            {
                ARtblCortesiaDetalle detalle = (ARtblCortesiaDetalle)detalleTemp;

                detalle.NumeroPartida = numeroPartida;
                detalle.EstatusId = EstatusRegistro.ACTIVO;
                detalle.CreadoPorId = usuarioId;

                cortesia.ARtblCortesiaDetalle.Add(detalle);
                cortesia.TotalCortesia = cortesia.TotalCortesia + detalle.TotalPartida;

                numeroPartida++;
            }

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json(service.GuardaCambios(cortesia), "Registro guardado con Exito!");
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Listar()
        {
            CortesiaViewModel viewModel = new CortesiaViewModel();

            viewModel.ListReciboCortesia = new CortesiaService().BuscaListado();
            
            return View("ListadoCortesia", viewModel);
        }

        protected override void GetDatosFicha(ref CortesiaViewModel viewModel)
        {
            CortesiaService CortesiaService = new CortesiaService();
            
            int cortesiaId = viewModel.Cortesia.CortesiaId;

            //Datos de Invitación de OC
            viewModel.ListOrdenesCompra = new OrdenCompraService().BuscaTodos();
            viewModel.ListProveedores = new ProveedorService().BuscaTodos();
            viewModel.ListAlmacenes = new AlmacenService().BuscaTodos();

            //Ejercicio para los campos de Fecha
            viewModel.EjercicioUsuario = SessionHelper.GetUsuario().Ejercicio;
        }

        [JsonException]
        public ContentResult GetProductos(string almacenId)
        {
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };

            var result = new ContentResult
            {
                Content = serializer.Serialize(new OrdenCompraService().BuscaComboProductos(almacenId, null, null, null)),
                ContentType = "application/json"
            };

            return result;
        }
    }
}