using Newtonsoft.Json;
using RequisicionesAlmacen.Areas.Compras.Compras.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services.Compras;
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Compras.Compras.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.ORDEN_DE_COMPRA)]
    public class OrdenCompraController : BaseController<tblOrdenCompra, OrdenCompraViewModel>
    {
        private string API_FICHA = "/compras/compras/ordencompra/";

        private string ESTATUS_NUEVO = "N";
        private string ESTATUS_MODIFICADO = "M";

        //Esto es temporal para no poder generar nuevas OC
        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        //public override ActionResult Nuevo()
        //{
        //    //Crear un objeto nuevo
        //    OrdenCompraViewModel viewModel = new OrdenCompraViewModel();

        //    //Asignamos el modelo al modelView
        //    tblOrdenCompra ordenCompra = new tblOrdenCompra();
            
        //    ordenCompra.OrdenCompraId = 0;
        //    ordenCompra.Status = EstatusOrdenCompra.ACTIVA;
        //    ordenCompra.Fecha = DateTime.Now;
        //    ordenCompra.FechaRecepcion = DateTime.Now;
        //    ordenCompra.Ajuste = true;
            
        //    viewModel.OrdenCompra = ordenCompra;

        //    //Modo Solo Lectura
        //    viewModel.SoloLectura = false;

        //    //Agregamos todos los datos necesarios para el funcionamiento de la ficha
        //    //como son los Listados para combos, tablas, arboles.
        //    GetDatosFicha(ref viewModel);

        //    //Retornamos la vista junto con su Objeto Modelo
        //    return View("OrdenCompra", viewModel);
        //}

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
            OrdenCompraService service = new OrdenCompraService();

            //Creamos el objeto
            OrdenCompraViewModel viewModel = new OrdenCompraViewModel();

            //Buscamos el Objeto por el Id que se envio como parametro
            tblOrdenCompra ordenCompra = service.BuscaPorId(id);

            //Asignamos el Objeto al ViewModel
            viewModel.OrdenCompra = ordenCompra != null ? ordenCompra : new tblOrdenCompra();

            if (ordenCompra == null)
            {
                //Asignamos el error
                SetViewBagError("La Orden de Compra no existe. Favor de revisar.", API_FICHA + "listar");
            }

            //Validamos que la OC venga de una Requisición
            else if (!service.ARspValidarRequisicionOC(id))
            {
                //Asignamos el error
                SetViewBagError("La Orden de Compra no fué generada por una Requisición ni por Invitación. Favor de revisar.", API_FICHA + "listar");
            }
            else
            {
                //Buscamos los Datos de Financiamiento
                ARspConsultaDatosFinanciamientoOrdenCompra_Result datosFinanciamiento = service.GetDatosFinanciamiento(ordenCompra.OrdenCompraId);

                if (datosFinanciamiento != null)
                {
                    ordenCompra.UnidadAdministrativaId = datosFinanciamiento.DependenciaId;
                    ordenCompra.ProyectoId = datosFinanciamiento.ProyectoId;
                    ordenCompra.FuenteFinanciamientoId = datosFinanciamiento.RamoId;
                    ordenCompra.Ajuste = datosFinanciamiento.Ajuste.GetValueOrDefault();
                }

                //Asignamos los detalles
                viewModel.ListOrdenCompraDetalles = service.BuscaDetallesPorOrdenCompraId(ordenCompra.OrdenCompraId);

                //Agregamos todos los datos necesarios para el funcionamiento de la ficha
                //como son los Listados para combos, tablas, arboles.
                GetDatosFicha(ref viewModel);

                //Asigmanos la Fecha de Operación
                viewModel.FechaOperacion = new ConfiguracionEnteService().BuscaFechaOperacion();

                //Agregamos el código de la Póliza
                tblPoliza poliza = new PolizaService().BuscaPolizaPorReferenciaTipoMovimientoIdYTipo(viewModel.OrdenCompra.OrdenCompraId, tblTipoMovimiento.ORDEN_COMPRA, "A");

                if (poliza == null)
                {
                    poliza = new PolizaService().BuscaPolizaPorReferenciaTipoMovimientoIdYTipo(viewModel.OrdenCompra.OrdenCompraId, tblTipoMovimiento.ORDEN_COMPRA, "M");
                }

                if (poliza != null)
                {
                    viewModel.Poliza = poliza.Poliza;
                }

                //Agregamos el código de la Póliza de Cancelación
                tblPoliza polizaCancelacion = new PolizaService().BuscaPolizaPorReferenciaTipoMovimientoIdYTipo(viewModel.OrdenCompra.OrdenCompraId, tblTipoMovimiento.ORDEN_COMPRA, "C");

                if (polizaCancelacion != null)
                {
                    viewModel.PolizaCancelacion = polizaCancelacion.Poliza;
                }
            }

            //Validamos si alún registro permite editar
            bool permiteEditar = false;

            if (soloLectura == null && !ordenCompra.Status.Equals(EstatusOrdenCompra.RECIBIDA) && viewModel.ListOrdenCompraDetalles != null)
            {
                foreach (ARspConsultaOrdenCompraDetalles_Result detalle in viewModel.ListOrdenCompraDetalles)
                {
                    if (detalle.PermiteEditar.GetValueOrDefault())
                    {
                        permiteEditar = true;
                    }
                }
            }

            //Modo Solo Lectura
            viewModel.SoloLectura = ordenCompra.Status.Equals(EstatusOrdenCompra.CANCELADA) || (soloLectura == null ? !permiteEditar : soloLectura.GetValueOrDefault());

            //Retornamos la vista junto con su Objeto Modelo
            return View("OrdenCompra", viewModel);
        }

        [JsonException]
        public override JsonResult Guardar(tblOrdenCompra ordenCompra)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult GuardaCambios(tblOrdenCompra ordenCompra, List<OrdenCompraDetalleItem> detalles)
        {
            OrdenCompraService service = new OrdenCompraService();

            //Si es un nuevo registro
            if (ordenCompra.OrdenCompraId == 0)
            {
                ordenCompra.Status = EstatusOrdenCompra.ACTIVA;
            }

            //De lo contrario verificamos que no se haya modidicado
            else
            {
                tblOrdenCompra temp = service.BuscaPorId(ordenCompra.OrdenCompraId);

                if (!ordenCompra.Status.Equals(temp.Status))
                {
                    throw new Exception("La Orden de Compra con el código [" + temp.OrdenCompraId + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
                }
            }

            List<tblOrdenCompraDet> detallesTemp = new List<tblOrdenCompraDet>();

            foreach (OrdenCompraDetalleItem registro in detalles)
            {
                if (registro.Status.Equals(ESTATUS_NUEVO) 
                    || registro.Status.Equals(ESTATUS_MODIFICADO)
                    || registro.Status.Equals(EstatusOrdenCompra.CANCELADA))
                {
                    tblOrdenCompraDet detalle = (tblOrdenCompraDet)registro;

                    // Si es un registro nuevo o editado
                    if (registro.Status.Equals(ESTATUS_NUEVO) || registro.Status.Equals(ESTATUS_MODIFICADO))
                    {
                        detalle.Status = EstatusOrdenCompra.ACTIVA;
                    }

                    detallesTemp.Add(new tblOrdenCompraDet().GetModelo(detalle));
                }
            }

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json(service.GuardaCambios(ordenCompra, detallesTemp), "Registro guardado con Exito!");
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult EliminarPorId(int ordenCompraId, string status, string fechaCancelacion)
        {
            tblOrdenCompra ordenCompra = new tblOrdenCompra();

            ordenCompra.OrdenCompraId = ordenCompraId;
            ordenCompra.Status = status;
            ordenCompra.FechaCancelacion = Convert.ToDateTime(fechaCancelacion);

            return EliminarPorModelo(ordenCompra);
        }

        [JsonException]
        public JsonResult EliminarPorModelo(tblOrdenCompra ordenCompra)
        {
            OrdenCompraService service = new OrdenCompraService();

            tblOrdenCompra temp = service.BuscaPorId(ordenCompra.OrdenCompraId);

            if (!ordenCompra.Status.Equals(temp.Status))
            {
                throw new Exception("La Orden de Compra con el código [" + temp.OrdenCompraId + "] ha sido modificada por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            temp.Status = EstatusOrdenCompra.CANCELADA;

            List<tblOrdenCompraDet> detallesTemp = new List<tblOrdenCompraDet>();
            List<tblOrdenCompraDet> detalles = service.BuscaDetallesNoCancelados(temp.OrdenCompraId);

            foreach (tblOrdenCompraDet detalle in detalles)
            {
                detalle.Status = EstatusOrdenCompra.CANCELADA;

                detallesTemp.Add(new tblOrdenCompraDet().GetModelo(detalle));
            }

            temp = new tblOrdenCompra().GetModelo(temp);
            temp.FechaCancelacion = ordenCompra.FechaCancelacion;

            service.GuardaCambios(temp, detallesTemp);

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Registro eliminado con Exito!");
        }

        public override ActionResult Listar()
        {
            OrdenCompraViewModel viewModel = new OrdenCompraViewModel();

            viewModel.ListOrdenCompra = new OrdenCompraService().BuscaListado();
            
            return View("ListadoOrdenCompra", viewModel);
        }

        protected override void GetDatosFicha(ref OrdenCompraViewModel viewModel)
        {
            //Datos de OC
            viewModel.OrdenCompra.Estatus = EstatusOrdenCompra.Nombre[viewModel.OrdenCompra.Status];
            viewModel.ListProveedores = new ProveedorService().BuscaTodos();
            viewModel.ListAlmacenes = new AlmacenService().BuscaTodos();
            viewModel.ListTiposOperacion = new TipoOperacionService().BuscaTodos();
            viewModel.ListTiposComprobanteFiscal = new TipoComprobanteFiscalService().BuscaTodos();

            //Datos Financiamiento
            viewModel.ListUnidadesAdministrativas = new DependenciaService().BuscaTodos();
            viewModel.ListProyectos = new ProyectoService().BuscaTodos();
            viewModel.ListFuentesFinanciamiento = new RamoService().BuscaTodos();

            //Datos Detalles
            viewModel.ListTiposGasto = new TipoGastoService().BuscaTodos();
            viewModel.ListTarifasImpuesto = new TarifaImpuestoService().BuscaTodos();

            //Ejercicio para los campos de Fecha
            viewModel.EjercicioUsuario = SessionHelper.GetUsuario().Ejercicio;
        }

        [JsonException]
        public ContentResult GetProductos(string almacenId, string dependenciaId, string proyectoId, string ramoId)
        {
            dependenciaId = dependenciaId == "" ? null : dependenciaId;
            proyectoId = proyectoId == "" ? null : proyectoId;
            ramoId = ramoId == "" ? null : ramoId;

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };

            var result = new ContentResult
            {
                Content = serializer.Serialize(new OrdenCompraService().BuscaComboProductos(almacenId, dependenciaId, proyectoId, ramoId)),
                ContentType = "application/json"
            };

            return result;
        }
    }
}