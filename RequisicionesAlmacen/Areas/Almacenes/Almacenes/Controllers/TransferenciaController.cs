using Newtonsoft.Json;
using RequisicionesAlmacen.Areas.Almacenes.Almacenes.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.Almacen;
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Almacenes.Almacenes.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.TRANSFERENCIAS)]
    public class TransferenciaController : BaseController<ARtblTransferencia, TransferenciaViewModel>
    {
        private string API_FICHA = "/almacenes/almacenes/transferencia/";

        public override ActionResult Nuevo()
        {
            //Crear un objeto nuevo
            TransferenciaViewModel viewModel = new TransferenciaViewModel();

            //Asignamos el modelo al modelView
            ARtblTransferencia modelo = new ARtblTransferencia();

            modelo.TransferenciaId = 0;
            modelo.Codigo = "TRA000000";

            viewModel.Transferencia = modelo;
            viewModel.FechaOperacion = new ConfiguracionEnteService().BuscaFechaOperacion();

            if (viewModel.FechaOperacion != null)
            {
                viewModel.Transferencia.Fecha = viewModel.FechaOperacion.GetValueOrDefault();
            }

            //Modo Solo Lectura
            viewModel.SoloLectura = false;

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref viewModel);

            //Retornamos la vista junto con su Objeto Modelo
            return View("Transferencia", viewModel);
        }

        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult Ver(int id)
        {
            TransferenciaService service = new TransferenciaService();

            //Creamos el objeto
            TransferenciaViewModel viewModel = new TransferenciaViewModel();

            //Buscamos el Objeto por el Id que se envio como parametro
            ARtblTransferencia modelo = service.BuscaPorId(id);

            //Asignamos el Objeto al ViewModel
            viewModel.Transferencia = modelo != null ? modelo : new ARtblTransferencia();

            if (modelo == null || !modelo.EstatusId.Equals(EstatusRegistro.ACTIVO))
            {
                //Asignamos el error
                SetViewBagError("La Transferencia no existe o está Cancelada. Favor de revisar.", API_FICHA + "listar");
            }
            else
            {
                //Asignamos la fecha
                modelo.FechaTransferencia = new SistemaService().GetFechaConFormato(modelo.Fecha);

                //Asignamos los detalles
                viewModel.ListTransferenciaMovtos = service.BuscaMovtosPorTransferenciaId(modelo.TransferenciaId);

                //Modo Solo Lectura
                viewModel.SoloLectura = true;

                //Agregamos todos los datos necesarios para el funcionamiento de la ficha
                //como son los Listados para combos, tablas, arboles.
                GetDatosFicha(ref viewModel);
            }

            //Retornamos la vista junto con su Objeto Modelo
            return View("Transferencia", viewModel);
        }

        public override JsonResult Guardar(ARtblTransferencia transferencia)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult GuardaCambios(ARtblTransferencia transferencia, List<TransferenciaMovtoItem> movimientos)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            TransferenciaService service = new TransferenciaService();

            //Actualizamos la cabecera
            transferencia.EstatusId = EstatusRegistro.ACTIVO;
            transferencia.CreadoPorId = usuarioId;

            int transferenciaId = service.GuardaCambios(transferencia, movimientos);
            tblPoliza poliza = new PolizaService().BuscaPolizaPorReferenciaYTipoMovimientoId(transferenciaId, tblTipoMovimiento.TRANSFERENCIA_PRODUCTO);

            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "transferenciaId", transferenciaId },
                { "poliza", poliza == null ? "SIN POLIZA" : poliza.Poliza }
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
            TransferenciaViewModel viewModel = new TransferenciaViewModel();

            viewModel.ListTransferecias = new TransferenciaService().BuscaListado();
            
            return View("ListadoTransferencia", viewModel);
        }

        protected override void GetDatosFicha(ref TransferenciaViewModel viewModel)
        {
            //Datos de la Transferencia
            RHtblEmpleado empleado = new EmpleadoService().BuscaPorUsuarioId(SessionHelper.GetUsuario().UsuarioId);
            ARtblControlMaestroConfiguracionArea configuracionArea = new ConfiguracionAreaService().BuscaPorAreaId(empleado.AreaAdscripcionId);
            viewModel.ListAlmacenes = new ConfiguracionAreaService().BuscaAlmacenesPorConfiguracionAreaId(configuracionArea.ConfiguracionAreaId);

            if (!viewModel.SoloLectura)
            {
                viewModel.ListProductos = new TransferenciaService().BuscaProductos();
            }

            //Ejercicio para los campos de Fecha
            viewModel.EjercicioUsuario = SessionHelper.GetUsuario().Ejercicio;
        }

        [JsonException]
        public ContentResult GetProductosDestino(string almacenId, string productoId)
        {
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };

            var result = new ContentResult
            {
                Content = serializer.Serialize(new TransferenciaService().BuscaComboProductosDestino(almacenId, productoId)),
                ContentType = "application/json"
            };

            return result;
        }
    }
}