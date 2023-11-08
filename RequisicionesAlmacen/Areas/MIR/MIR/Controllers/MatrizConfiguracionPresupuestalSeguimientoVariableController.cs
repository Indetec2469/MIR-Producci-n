using RequisicionesAlmacen.Areas.MIR.MIR.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.MIR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.MIR.MIR.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.SEGUIMIENTO_A_VARIABLES)]
    public class MatrizConfiguracionPresupuestalSeguimientoVariableController : BaseController<MatrizConfiguracionPresupuestalSeguimientoVariableViewModel, MatrizConfiguracionPresupuestalSeguimientoVariableViewModel>
    {
        public override ActionResult Editar(int id)
        {
            // Services
            MatrizIndicadorResultadoService matrizIndicadorResultadoService = new MatrizIndicadorResultadoService();
            MatrizIndicadorResultadoIndicadorService matrizIndicadorResultadoIndicadorService = new MatrizIndicadorResultadoIndicadorService();
            MatrizIndicadorResultadoIndicadorFormulaVariableService matrizIndicadorResultadoIndicadorFormulaVariableService = new MatrizIndicadorResultadoIndicadorFormulaVariableService();
            // Crear los objetos nuevos
            MatrizConfiguracionPresupuestalSeguimientoVariableViewModel matrizConfiguracionPresupuestalSeguimientoVariableViewModel = new MatrizConfiguracionPresupuestalSeguimientoVariableViewModel();
            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.MatrizConfiguracionPresupuestalSeguimientoVariableModel = new MItblMatrizConfiguracionPresupuestalSeguimientoVariable();
            // Buscamos los objetos por el ID de MIR que se envio como parametro
            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ConsultaMatrizIndicadorResultado = matrizIndicadorResultadoService.ConsultaMatrizIndicadorResultado(id);
            if (matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ConsultaMatrizIndicadorResultado == null)
            {
                return new HttpNotFoundResult("La MIR no existe la solicitud.");
            }
            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListMatrizIndicadorResultadoIndicador = matrizIndicadorResultadoIndicadorService.BuscaPorMIRId(id);
            List<MItblMatrizIndicadorResultadoIndicadorFormulaVariable> listMatrizIndicadorResultadoIndicadorFormulaVariable = new List<MItblMatrizIndicadorResultadoIndicadorFormulaVariable>();
            List<MItblMatrizConfiguracionPresupuestalSeguimientoVariable> listMatrizConfiguracionPresupuestalSeguimientoVariable = new List<MItblMatrizConfiguracionPresupuestalSeguimientoVariable>();
            foreach (MItblMatrizIndicadorResultadoIndicador matrizIndicadorResultadoIndicador in matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListMatrizIndicadorResultadoIndicador)
            {
                IEnumerable<MItblMatrizIndicadorResultadoIndicadorFormulaVariable> _listMatrizIndicadorResultadoIndicadorFormulaVariable = matrizIndicadorResultadoIndicadorFormulaVariableService.BuscaPorMIRIndicadorId(matrizIndicadorResultadoIndicador.MIRIndicadorId);
                _listMatrizIndicadorResultadoIndicadorFormulaVariable.ToList().ForEach(fv =>
                {
                    listMatrizIndicadorResultadoIndicadorFormulaVariable.Add(fv);
                    // Buscamos los guardados de Matriz Configuracion Presupuestal Seguimiento Variable
                    MItblMatrizConfiguracionPresupuestalSeguimientoVariableH matrizConfiguracionPresupuestalSeguimientoVariable = new MatrizConfiguracionPresupuestalSeguimientoVariableHService().BuscaPorMIRIndicadorFormulaVariableId(fv.MIRIndicadorFormulaVariableId);
                    if (matrizConfiguracionPresupuestalSeguimientoVariable != null)
                    {
                        MItblMatrizConfiguracionPresupuestalSeguimientoVariable MapeoMItblMatrizConfiguracionPresupuestalSeguimientoVariable = new MItblMatrizConfiguracionPresupuestalSeguimientoVariable()
                        {
                            Abril = matrizConfiguracionPresupuestalSeguimientoVariable.Abril,
                            Agosto = matrizConfiguracionPresupuestalSeguimientoVariable.Agosto,
                            CreadoPorId = matrizConfiguracionPresupuestalSeguimientoVariable.CreadoPorId,
                            Diciembre = matrizConfiguracionPresupuestalSeguimientoVariable.Diciembre,
                            Enero = matrizConfiguracionPresupuestalSeguimientoVariable.Enero,
                            EstatusId = matrizConfiguracionPresupuestalSeguimientoVariable.EstatusId,
                            Febrero = matrizConfiguracionPresupuestalSeguimientoVariable.Febrero,
                            FechaCreacion = matrizConfiguracionPresupuestalSeguimientoVariable.FechaCreacion,
                            FechaUltimaModificacion = matrizConfiguracionPresupuestalSeguimientoVariable.FechaUltimaModificacion,
                            Julio = matrizConfiguracionPresupuestalSeguimientoVariable.Julio,
                            Junio = matrizConfiguracionPresupuestalSeguimientoVariable.Junio,
                            Marzo = matrizConfiguracionPresupuestalSeguimientoVariable.Marzo,
                            Mayo = matrizConfiguracionPresupuestalSeguimientoVariable.Mayo,
                            MIRIndicadorFormulaVariableId = matrizConfiguracionPresupuestalSeguimientoVariable.MIRIndicadorFormulaVariableId,
                            MIRSeguimientoVariableId = matrizConfiguracionPresupuestalSeguimientoVariable.MIRSeguimientoVariableHId,
                            ModificadoPorId = matrizConfiguracionPresupuestalSeguimientoVariable.ModificadoPorId,
                            Noviembre = matrizConfiguracionPresupuestalSeguimientoVariable.Noviembre,
                            Octubre = matrizConfiguracionPresupuestalSeguimientoVariable.Octubre,
                            Septiembre = matrizConfiguracionPresupuestalSeguimientoVariable.Septiembre,
                            Timestamp = matrizConfiguracionPresupuestalSeguimientoVariable.Timestamp


                        }; ;

                        listMatrizConfiguracionPresupuestalSeguimientoVariable.Add(MapeoMItblMatrizConfiguracionPresupuestalSeguimientoVariable);
                    }
                });
            }
            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListMatrizIndicadorResultadoIndicadorFormulaVariable = listMatrizIndicadorResultadoIndicadorFormulaVariable;
            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListMatrizConfiguracionPresupuestalSeguimientoVariable = listMatrizConfiguracionPresupuestalSeguimientoVariable;
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref matrizConfiguracionPresupuestalSeguimientoVariableViewModel);
            // Crear un menu para los indicadores
            GetMenuSeguimientoVariable(ref matrizConfiguracionPresupuestalSeguimientoVariableViewModel);
            //Retornamos la vista junto con su Objeto Modelo
            return View("MatrizConfiguracionPresupuestalSeguimientoVariable", matrizConfiguracionPresupuestalSeguimientoVariableViewModel);
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public override JsonResult Guardar(MatrizConfiguracionPresupuestalSeguimientoVariableViewModel matrizConfiguracionPresupuestalSeguimientoVariableViewModel)
        {
            DateTime FechaModificacion = DateTime.Parse(matrizConfiguracionPresupuestalSeguimientoVariableViewModel.FechaModificacion);

            // Usuario
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;

            // Matriz Configuracion Presupuestal Seguimiento Variable
            if (matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListMatrizConfiguracionPresupuestalSeguimientoVariable != null)
            {
                // Service
                MatrizConfiguracionPresupuestalSeguimientoVariableService matrizConfiguracionPresupuestalSeguimientoVariableService = new MatrizConfiguracionPresupuestalSeguimientoVariableService();
                foreach (MItblMatrizConfiguracionPresupuestalSeguimientoVariable matrizConfiguracionPresupuestalSeguimientoVariable in matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListMatrizConfiguracionPresupuestalSeguimientoVariable)
                {
                    // Si el ID es nuevo para registrar o actualizar
                    if (matrizConfiguracionPresupuestalSeguimientoVariable.MIRSeguimientoVariableId > 0)
                    {
                        matrizConfiguracionPresupuestalSeguimientoVariable.ModificadoPorId = usuarioId;
                        matrizConfiguracionPresupuestalSeguimientoVariable.FechaUltimaModificacion = DateTime.Now;
                    }
                    else
                    {

                        matrizConfiguracionPresupuestalSeguimientoVariable.CreadoPorId = usuarioId;
                    }
                }
            }

            new MatrizConfiguracionPresupuestalSeguimientoVariableService().GuardaCambios(matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListMatrizConfiguracionPresupuestalSeguimientoVariable, FechaModificacion);
            
            return Json("Registro guardado con Exito!");
        }

        // GET: MIR/MatrizConfiguracionPresupuestalSeguimientoVariable
        public ActionResult Index()
        {
            return View();
        }

        public override ActionResult Listar()
        {
            MatrizConfiguracionPresupuestalSeguimientoVariableViewModel matrizConfiguracionPresupuestalSeguimientoVariableViewModel = new MatrizConfiguracionPresupuestalSeguimientoVariableViewModel();

            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListadoMatrizConfiguracionPresupuestalSeguimientoVariable = new MatrizConfiguracionPresupuestalSeguimientoVariableService().BuscaListado();

            return View("ListadoMatrizConfiguracionPresupuestalSeguimientoVariable", matrizConfiguracionPresupuestalSeguimientoVariableViewModel);
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref MatrizConfiguracionPresupuestalSeguimientoVariableViewModel matrizConfiguracionPresupuestalSeguimientoVariableViewModel)
        {
            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListUnidadMedida = new ControlMaestroUnidadMedidaService().BuscaTodos2();
            // Control Maestro
            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListFrecuenciaMedicion = new ControlMaestroFrecuenciaMedicionService().BuscaTodos();
            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListSentido = new ControlMaestroService().BuscaControl("Sentido");
            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListaControlMaestroControlPeriodo = new ControlMaestroControlPeriodoService().BuscaTodos();
        }

        protected void GetMenuSeguimientoVariable(ref MatrizConfiguracionPresupuestalSeguimientoVariableViewModel matrizConfiguracionPresupuestalSeguimientoVariableViewModel)
        {
            // Variables
            int countIndicador = 1;
            List<MenuSeguimientoVariableModel> listMenuSeguimientoVariable = new List<MenuSeguimientoVariableModel>();
            // Nivel Fin
            MenuSeguimientoVariableModel menuSeguimientoVariableModel = new MenuSeguimientoVariableModel();
            List<MenuSeguimientoVariableModel> listMenuItemsSeguimientoVariable = new List<MenuSeguimientoVariableModel>();
            menuSeguimientoVariableModel.Id = -1;
            menuSeguimientoVariableModel.Text = "Fin";
            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListMatrizIndicadorResultadoIndicador.Where(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.FIN).ToList().ForEach(miri =>
            {
                MenuSeguimientoVariableModel menuItemsSeguimientoVariableModel = new MenuSeguimientoVariableModel();
                menuItemsSeguimientoVariableModel.Id = miri.MIRIndicadorId;
                menuItemsSeguimientoVariableModel.Text = "Fin " + countIndicador;
                listMenuItemsSeguimientoVariable.Add(menuItemsSeguimientoVariableModel);
                countIndicador++;
            });
            menuSeguimientoVariableModel.Items = listMenuItemsSeguimientoVariable;
            listMenuSeguimientoVariable.Add(menuSeguimientoVariableModel);
            // Nivel Proposito
            countIndicador = 1;
            menuSeguimientoVariableModel = new MenuSeguimientoVariableModel();
            listMenuItemsSeguimientoVariable = new List<MenuSeguimientoVariableModel>();
            menuSeguimientoVariableModel.Id = -2;
            menuSeguimientoVariableModel.Text = "Propósito";
            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListMatrizIndicadorResultadoIndicador.Where(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.PROPOSITO).ToList().ForEach(miri =>
            {
                MenuSeguimientoVariableModel menuItemsSeguimientoVariableModel = new MenuSeguimientoVariableModel();
                menuItemsSeguimientoVariableModel.Id = miri.MIRIndicadorId;
                menuItemsSeguimientoVariableModel.Text = "Propósito " + countIndicador;
                listMenuItemsSeguimientoVariable.Add(menuItemsSeguimientoVariableModel);
                countIndicador++;
            });
            menuSeguimientoVariableModel.Items = listMenuItemsSeguimientoVariable;
            listMenuSeguimientoVariable.Add(menuSeguimientoVariableModel);
            // Nivel Componente
            countIndicador = 1;
            menuSeguimientoVariableModel = new MenuSeguimientoVariableModel();
            listMenuItemsSeguimientoVariable = new List<MenuSeguimientoVariableModel>();
            menuSeguimientoVariableModel.Id = -3;
            menuSeguimientoVariableModel.Text = "Componente";
            List<MenuItemsSeguimientoVariableModel> menuItemsSeguimientoVariableModel2 = new List<MenuItemsSeguimientoVariableModel>();
            //// Nivel Actividad
            int? componente = 0;
            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListMatrizIndicadorResultadoIndicador.Where(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.ACTIVIDAD).OrderBy(x => x.MIRIndicadorComponenteId).ToList().ForEach(miri =>
            {
                if (componente != miri.MIRIndicadorComponenteId)
                {
                    countIndicador = 1;
                    componente = miri.MIRIndicadorComponenteId;
                }
                

                MenuItemsSeguimientoVariableModel menuItemsSeguimientoVariableModel = new MenuItemsSeguimientoVariableModel();
                menuItemsSeguimientoVariableModel.Id = miri.MIRIndicadorId;
                menuItemsSeguimientoVariableModel.Text = "Actividad " + countIndicador;
                menuItemsSeguimientoVariableModel.Id_Componente = miri.MIRIndicadorComponenteId;
                menuItemsSeguimientoVariableModel2.Add(menuItemsSeguimientoVariableModel);
                countIndicador++;
            });
            countIndicador = 1;
            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListMatrizIndicadorResultadoIndicador.Where(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE).ToList().ForEach(miri =>
            {

                MenuSeguimientoVariableModel menuItemsSeguimientoVariableModel = new MenuSeguimientoVariableModel();
                menuItemsSeguimientoVariableModel.Id = miri.MIRIndicadorId;
                menuItemsSeguimientoVariableModel.Text = "Componente " + countIndicador;
                menuItemsSeguimientoVariableModel.Items = menuItemsSeguimientoVariableModel2.Where(x => x.Id_Componente == miri.MIRIndicadorId).Select(x=>new MenuSeguimientoVariableModel {Id=x.Id,Text=x.Text }).ToList();
                listMenuItemsSeguimientoVariable.Add(menuItemsSeguimientoVariableModel);
                countIndicador++;
            });
            menuSeguimientoVariableModel.Items = listMenuItemsSeguimientoVariable;
            listMenuSeguimientoVariable.Add(menuSeguimientoVariableModel);
            matrizConfiguracionPresupuestalSeguimientoVariableViewModel.ListMenuSeguimientoVariable = listMenuSeguimientoVariable;
        }
    }
}