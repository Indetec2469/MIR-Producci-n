using RequisicionesAlmacen.Areas.Compras.Catalogos.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Compras.Catalogos.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.CONFIGURACION_MONTOS_COMPRAS)]
    public class ConfiguracionMontoCompraController : BaseController<List<ARtblControlMaestroConfiguracionMontoCompra>, ControlMaestroConfiguracionMontoCompraViewModel>
    {
        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public override JsonResult Guardar(List<ARtblControlMaestroConfiguracionMontoCompra> listControlMaestroConfiguracionMontoCompra)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;

            ControlMaestroConfiguracionMontoCompraService service = new ControlMaestroConfiguracionMontoCompraService();
            
            foreach (ARtblControlMaestroConfiguracionMontoCompra configuracionMontoCompra in listControlMaestroConfiguracionMontoCompra)
            {
                if (configuracionMontoCompra.ConfiguracionMontoId > 0)
                {
                    configuracionMontoCompra.ModificadoPorId = usuarioId;
                    configuracionMontoCompra.FechaUltimaModificacion = fecha;

                    //Verificamos si es posible eliminar
                    if (configuracionMontoCompra.EstatusId.Equals(EstatusRegistro.BORRADO) && !new SistemaService().PermiteEliminarRegistro(configuracionMontoCompra.ConfiguracionMontoId, Tablas.ARtblControlMaestroConfiguracionMontoCompra))
                    {
                        throw new Exception("La Configuración [" + service.BuscaTipoCompra(configuracionMontoCompra.TipoCompraId).Valor + "] no puede ser eliminada ya que está siendo utilizada para otros procesos.");
                    }
                }
                else
                {
                    configuracionMontoCompra.CreadoPorId = usuarioId;
                }
            }

            new ControlMaestroConfiguracionMontoCompraService().GuardaCambios(listControlMaestroConfiguracionMontoCompra);

            return Json("Registros con Exito!");
        }

        // GET: Compras/ConfiguracionMontoCompra/Listar
        public override ActionResult Listar()
        {
            ControlMaestroConfiguracionMontoCompraViewModel viewModel = new ControlMaestroConfiguracionMontoCompraViewModel();
            
            viewModel.ControlMaestroConfiguracionMontoCompra = new ARtblControlMaestroConfiguracionMontoCompra();
            viewModel.ControlMaestroConfiguracionMontoCompra.EstatusId = ControlMaestroMapeo.EstatusRegistro.ACTIVO;
            
            GetDatosFicha(ref viewModel);
            
            return View("ConfiguracionMontoCompra", viewModel);
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ControlMaestroConfiguracionMontoCompraViewModel viewModel)
        {
            viewModel.ListConfiguracionMontoCompra = new ControlMaestroConfiguracionMontoCompraService().BuscaTodos();

            viewModel.ListTipoCompra = new ControlMaestroService().BuscaControl("TipoCompra");
        }
    }
}