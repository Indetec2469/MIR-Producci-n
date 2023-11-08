using RequisicionesAlmacen.Areas.MIR.Catalogos.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.MIR.Catalogos.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.SENTIDO_DEL_INDICADOR)]
    public class SentidoIndicadorController : BaseController<GRtblControlMaestro, SentidoIndicadorViewModel>
    {
        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Guardar(GRtblControlMaestro modelView)
        {
            throw new NotImplementedException();
        }

        // GET: MIR/Catalogo/SentidoIndicador/Listar
        public override ActionResult Listar()
        {
            // Crear los objetos
            SentidoIndicadorViewModel sentidoIndicadorViewModel = new SentidoIndicadorViewModel();
            GetDatosFicha(ref sentidoIndicadorViewModel);
            return View("ListadoSentidoIndicador", sentidoIndicadorViewModel);
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref SentidoIndicadorViewModel sentidoIndicadorViewModel)
        {
            sentidoIndicadorViewModel.ListControlMaestroSentido = new ControlMaestroService().BuscaControl("Sentido");
        }
    }
}