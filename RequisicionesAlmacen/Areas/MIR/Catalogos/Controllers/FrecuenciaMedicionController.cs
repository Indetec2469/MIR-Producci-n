using RequisicionesAlmacen.Areas.MIR.Catalogos.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.MIR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.MIR.Catalogos.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.FRECUENCIA_DE_MEDICION)]
    public class FrecuenciaMedicionController : BaseController<ControlMaestroFrecuenciaMedicionViewModel, ControlMaestroFrecuenciaMedicionViewModel>
    {
        public override ActionResult Editar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public override JsonResult Guardar(ControlMaestroFrecuenciaMedicionViewModel modelView)
        {
            throw new NotImplementedException();
        }

        // GET: MIR/FrecuenciaMedicion
        public ActionResult Index()
        {
            return View();
        }

        public override ActionResult Listar()
        {
            // Crear el objeto
            ControlMaestroFrecuenciaMedicionViewModel controlMaestroFrecuenciaMedicionViewModel = new ControlMaestroFrecuenciaMedicionViewModel();
            GetDatosFicha(ref controlMaestroFrecuenciaMedicionViewModel);

            return View("ListadoFrecuenciaMedicion", controlMaestroFrecuenciaMedicionViewModel);
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ControlMaestroFrecuenciaMedicionViewModel controlMaestroFrecuenciaMedicionViewModel)
        {
            controlMaestroFrecuenciaMedicionViewModel.ListControlMaestroFrecuenciaMedicion = new ControlMaestroFrecuenciaMedicionService().BuscaTodos();
            controlMaestroFrecuenciaMedicionViewModel.ListControlMaestroFrecuenciaMedicionNivel = new ControlMaestroFrecuenciaMedicionNivelService().BuscaTodos();
            controlMaestroFrecuenciaMedicionViewModel.ListControlMaestroNivel = new ControlMaestroService().BuscaControl("Nivel");
        }
    }
}