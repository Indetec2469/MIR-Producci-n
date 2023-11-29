using RequisicionesAlmacen.Areas.MIR.MIR.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.SAACG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.MIR.MIR.Controllers
{

    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.MATRIZ_PRESUPUESTO_VIGENTE)]
    public class MatrizPresupuestoVigenteController : BaseController<MatrizPresupuestoVigenteViewModel, MatrizPresupuestoVigenteViewModel>
    {
        string API_FICHA = "/mir/mir/matrizpresupuestodevengado/";

        public override ActionResult Editar(int id)
        {
            bool mirValida = false;

            // Crear los objetos nuevos
            MatrizPresupuestoVigenteViewModel matrizPresupuestoVigenteViewModel = new MatrizPresupuestoVigenteViewModel();

            //Buscamos la MIR con el Id que viene como parametro
            MItblMatrizIndicadorResultado mir = new MatrizIndicadorResultadoService().BuscaPorId(id);
            List<MItblMatrizIndicadorResultadoIndicador> mirIndicadores = new MatrizIndicadorResultadoIndicadorService().BuscaPorMIRId(mir.MIRId).ToList();

            //Buscamos el programa presupuestario ligado a la MIR
            tblProgramaGobierno programaPresupuestario = new ProgramaGobiernoService().BuscaPorId(mir.ProgramaPresupuestarioId);

            int numeroComponentes = mirIndicadores.Where(ind => ind.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE).Count();
            int numeroActividades = mirIndicadores.Where(ind => ind.NivelIndicadorId == ControlMaestroMapeo.Nivel.ACTIVIDAD).Count();
            int proyectosVacios = mirIndicadores.Where(ind => (ind.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE && ind.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_COMPONENTE && ind.ProyectoId == null) ||
                                                              (ind.NivelIndicadorId == ControlMaestroMapeo.Nivel.ACTIVIDAD && ind.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_ACTIVIDAD && ind.ProyectoId == null)).Count();

            if (mir == null)
                SetViewBagError("No se encontró la MIR", API_FICHA + "listar");
            else if (programaPresupuestario == null)
                SetViewBagError("No se encontró el Programa Presupuestario relacionado a la MIR", API_FICHA + "listar");
            else if (numeroComponentes == 0)
                SetViewBagError("La MIR que intenta editar no tiene COMPONENTES configurados. Favor de agregarlos desde el formulario de la MIR.", API_FICHA + "listar");
            else if (numeroActividades == 0)
                SetViewBagError("La MIR que intenta editar no tiene ACTIVIDADES configuradas. Favor de agregarlas desde el formulario de la MIR.", API_FICHA + "listar");
            else if (proyectosVacios != 0)
                SetViewBagError("Falta configurar proyectos en la MIR", API_FICHA + "listar");
            else
                mirValida = true;

            //Si paso todas las validacion, cargamos la demas informacion y despues la vista
            if (mirValida)
            {
                matrizPresupuestoVigenteViewModel.MIRId = mir.MIRId;
                matrizPresupuestoVigenteViewModel.CodigoMIR = mir.Codigo;
                matrizPresupuestoVigenteViewModel.ProgramaPresupuestario = programaPresupuestario.Nombre;

                //Agregamos todos los datos necesarios para el funcionamiento de la ficha
                //como son los Listados para combos, tablas, arboles.
                GetDatosFicha(ref matrizPresupuestoVigenteViewModel);

                //Calculamos el Presupuesto Vigente
                matrizPresupuestoVigenteViewModel.PresupuestoVigente = matrizPresupuestoVigenteViewModel.MatrizPresupuestoVigenteH.Where(c => c.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE).Sum(c => c.Anual).Value;
            }
            else
            {
                matrizPresupuestoVigenteViewModel = null;
            }

            //Retornamos la vista junto con su Objeto Modelo
            return View("MatrizPresupuestoVigente", matrizPresupuestoVigenteViewModel);
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public override JsonResult Guardar(MatrizPresupuestoVigenteViewModel matrizPresupuestoVigenteViewModel)
        {
            DateTime FechaModificacion= DateTime.Parse(matrizPresupuestoVigenteViewModel.FechaModificacion);

            // Usuario
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
           

            //Verificamos de nuevo que los meses a guardar se encuentren abiertos
            if (!ValidaPeriodosAGuardar(matrizPresupuestoVigenteViewModel.MatrizPresupuestoVigenteH.FirstOrDefault()))
                throw new Exception("Existen periodos que no se encuentran abiertos y no se puede guardar la información"); 

            //Creamos la cabecera
            MItblMatrizConfiguracionPresupuestal matrizConfiguracionPresupuestal = new MItblMatrizConfiguracionPresupuestal();

            //Creamos los detalles
            List<MItblMatrizConfiguracionPresupuestalDetalle> matrizConfiguracionPresupuestalDetalleList = new List<MItblMatrizConfiguracionPresupuestalDetalle>();

            //Si es un registro nuevo
            if (matrizPresupuestoVigenteViewModel.ConfiguracionPresupuestoId == 0)
            {
                //Verificamos que realmente no se haya guardado ya un registro cabecera
                MItblMatrizConfiguracionPresupuestal _matrizConfiguracionPresupuestal = new MatrizConfiguracionPresupuestalService().BuscaPorMIRId(matrizPresupuestoVigenteViewModel.MIRId, ControlMaestroMapeo.TipoPresupuesto.POR_EJERCER);
                if (_matrizConfiguracionPresupuestal != null)
                    throw new Exception("Ya existe otro registro de Matriz Presupuestal guardada, favor de recargar el formulario.Se perderán todos los datos modificados.");

                //Creamos la cabecera
                matrizConfiguracionPresupuestal.ConfiguracionPresupuestoId = matrizPresupuestoVigenteViewModel.ConfiguracionPresupuestoId;
                matrizConfiguracionPresupuestal.MIRId = matrizPresupuestoVigenteViewModel.MIRId;
                matrizConfiguracionPresupuestal.PresupuestoPorEjercer = matrizPresupuestoVigenteViewModel.PresupuestoVigente;
                matrizConfiguracionPresupuestal.PresupuestoDevengado = 0;
                matrizConfiguracionPresupuestal.ClasificadorId = ControlMaestroMapeo.TipoPresupuesto.POR_EJERCER;
                matrizConfiguracionPresupuestal.EstatusId = ControlMaestroMapeo.EstatusRegistro.ACTIVO;
                matrizConfiguracionPresupuestal.CreadoPorId = usuarioId;

                //Creamos los detalles
                MItblMatrizConfiguracionPresupuestalDetalle matrizConfiguracionPresupuestalDetalle;
                foreach (MIspConsultaMatrizPresupuestoVigente_Result matrizConfiguracionPresupuestalDetalle_ in matrizPresupuestoVigenteViewModel.MatrizPresupuestoVigente)
                {
                    matrizConfiguracionPresupuestalDetalle = new MItblMatrizConfiguracionPresupuestalDetalle();
                    matrizConfiguracionPresupuestalDetalle.ConfiguracionPresupuestoId = matrizConfiguracionPresupuestal.ConfiguracionPresupuestoId;
                    matrizConfiguracionPresupuestalDetalle.MIRIndicadorId = matrizConfiguracionPresupuestalDetalle_.MIRIndicadorId.Value;
                    matrizConfiguracionPresupuestalDetalle.Enero = matrizConfiguracionPresupuestalDetalle_.Enero;
                    matrizConfiguracionPresupuestalDetalle.Febrero = matrizConfiguracionPresupuestalDetalle_.Febrero;
                    matrizConfiguracionPresupuestalDetalle.Marzo = matrizConfiguracionPresupuestalDetalle_.Marzo;
                    matrizConfiguracionPresupuestalDetalle.Abril =  matrizConfiguracionPresupuestalDetalle_.Abril;
                    matrizConfiguracionPresupuestalDetalle.Mayo =  matrizConfiguracionPresupuestalDetalle_.Mayo;
                    matrizConfiguracionPresupuestalDetalle.Junio = matrizConfiguracionPresupuestalDetalle_.Junio;
                    matrizConfiguracionPresupuestalDetalle.Julio = matrizConfiguracionPresupuestalDetalle_.Julio;
                    matrizConfiguracionPresupuestalDetalle.Agosto = matrizConfiguracionPresupuestalDetalle_.Agosto;
                    matrizConfiguracionPresupuestalDetalle.Septiembre = matrizConfiguracionPresupuestalDetalle_.Septiembre;
                    matrizConfiguracionPresupuestalDetalle.Octubre = matrizConfiguracionPresupuestalDetalle_.Octubre;
                    matrizConfiguracionPresupuestalDetalle.Noviembre = matrizConfiguracionPresupuestalDetalle_.Noviembre;
                    matrizConfiguracionPresupuestalDetalle.Diciembre = matrizConfiguracionPresupuestalDetalle_.Diciembre;
                    matrizConfiguracionPresupuestalDetalle.Anual = matrizConfiguracionPresupuestalDetalle_.Anual;
                    matrizConfiguracionPresupuestalDetalle.Porcentaje = matrizConfiguracionPresupuestalDetalle_.Porcentaje == null ? 0 : matrizConfiguracionPresupuestalDetalle_.Porcentaje.Value;
                    matrizConfiguracionPresupuestalDetalle.EstatusId = ControlMaestroMapeo.EstatusRegistro.ACTIVO;
                    matrizConfiguracionPresupuestalDetalle.CreadoPorId = usuarioId;
                    matrizConfiguracionPresupuestalDetalle.FechaUltimaModificacion = FechaModificacion;



                    //Agregamos el objeto al list
                    matrizConfiguracionPresupuestalDetalleList.Add(matrizConfiguracionPresupuestalDetalle);
                }

            }
            else
            {
                //Buscamos la cabecera
                MItblMatrizConfiguracionPresupuestal matrizConfiguracionPresupuestalTemp = new MatrizConfiguracionPresupuestalService().BuscaPorId(matrizPresupuestoVigenteViewModel.ConfiguracionPresupuestoId);
                matrizConfiguracionPresupuestal.ConfiguracionPresupuestoId = matrizConfiguracionPresupuestalTemp.ConfiguracionPresupuestoId;
                matrizConfiguracionPresupuestal.MIRId = matrizConfiguracionPresupuestalTemp.MIRId;
                matrizConfiguracionPresupuestal.PresupuestoPorEjercer = matrizPresupuestoVigenteViewModel.PresupuestoVigente;
                matrizConfiguracionPresupuestal.PresupuestoDevengado = 0;
                matrizConfiguracionPresupuestal.ClasificadorId = ControlMaestroMapeo.TipoPresupuesto.POR_EJERCER;
                matrizConfiguracionPresupuestal.EstatusId = ControlMaestroMapeo.EstatusRegistro.ACTIVO;
                matrizConfiguracionPresupuestal.CreadoPorId = matrizConfiguracionPresupuestalTemp.CreadoPorId;
                matrizConfiguracionPresupuestal.FechaCreacion = matrizConfiguracionPresupuestalTemp.FechaCreacion;
                matrizConfiguracionPresupuestal.ModificadoPorId = usuarioId;
                matrizConfiguracionPresupuestal.FechaUltimaModificacion = DateTime.Now;

                //Buscamos todos los detalles
                MItblMatrizConfiguracionPresupuestalDetalle matrizConfiguracionPresupuestalDetalleTemp;
                List<MItblMatrizConfiguracionPresupuestalDetalle>  matrizConfiguracionPresupuestalDetalleTempList = new MatrizConfiguracionPresupuestalDetalleService().BuscaPorConfiguracionPresupuestoId(matrizConfiguracionPresupuestalTemp.ConfiguracionPresupuestoId).ToList();

                //Iteramos por cada registro para actualizar la informacion
                MItblMatrizConfiguracionPresupuestalDetalle matrizConfiguracionPresupuestalDetalle;
                foreach (MIspConsultaMatrizPresupuestoVigenteH_Result matrizConfiguracionPresupuestalDetalleResult in matrizPresupuestoVigenteViewModel.MatrizPresupuestoVigenteH)
                {
                    //Se cambia el campo para la igualdad en la expresión lambda: ConfiguracionPresupuestoDetalleId  por MIRIndicadorId
                    matrizConfiguracionPresupuestalDetalleTemp = matrizConfiguracionPresupuestalDetalleTempList.Find(f => f.MIRIndicadorId == matrizConfiguracionPresupuestalDetalleResult.MIRIndicadorId);
                    matrizConfiguracionPresupuestalDetalle = new MItblMatrizConfiguracionPresupuestalDetalle();
                    matrizConfiguracionPresupuestalDetalle.ConfiguracionPresupuestoId = matrizConfiguracionPresupuestalDetalleTemp.ConfiguracionPresupuestoId;
                    matrizConfiguracionPresupuestalDetalle.ConfiguracionPresupuestoDetalleId = matrizConfiguracionPresupuestalDetalleTemp.ConfiguracionPresupuestoDetalleId;
                    matrizConfiguracionPresupuestalDetalle.MIRIndicadorId = matrizConfiguracionPresupuestalDetalleTemp.MIRIndicadorId;
                    matrizConfiguracionPresupuestalDetalle.Enero =  matrizConfiguracionPresupuestalDetalleResult.Enero;
                    matrizConfiguracionPresupuestalDetalle.Febrero =  matrizConfiguracionPresupuestalDetalleResult.Febrero;
                    matrizConfiguracionPresupuestalDetalle.Marzo = matrizConfiguracionPresupuestalDetalleResult.Marzo;
                    matrizConfiguracionPresupuestalDetalle.Abril =  matrizConfiguracionPresupuestalDetalleResult.Abril;
                    matrizConfiguracionPresupuestalDetalle.Mayo =  matrizConfiguracionPresupuestalDetalleResult.Mayo;
                    matrizConfiguracionPresupuestalDetalle.Junio =  matrizConfiguracionPresupuestalDetalleResult.Junio;
                    matrizConfiguracionPresupuestalDetalle.Julio =  matrizConfiguracionPresupuestalDetalleResult.Julio;
                    matrizConfiguracionPresupuestalDetalle.Agosto =  matrizConfiguracionPresupuestalDetalleResult.Agosto;
                    matrizConfiguracionPresupuestalDetalle.Septiembre =  matrizConfiguracionPresupuestalDetalleResult.Septiembre;
                    matrizConfiguracionPresupuestalDetalle.Octubre = matrizConfiguracionPresupuestalDetalleResult.Octubre;
                    matrizConfiguracionPresupuestalDetalle.Noviembre = matrizConfiguracionPresupuestalDetalleResult.Noviembre;
                    matrizConfiguracionPresupuestalDetalle.Diciembre =  matrizConfiguracionPresupuestalDetalleResult.Diciembre;
                    matrizConfiguracionPresupuestalDetalle.Anual = matrizConfiguracionPresupuestalDetalleResult.Anual;
                    matrizConfiguracionPresupuestalDetalle.Porcentaje = matrizConfiguracionPresupuestalDetalleResult.Porcentaje == null ? 0 : matrizConfiguracionPresupuestalDetalleResult.Porcentaje.Value;
                    matrizConfiguracionPresupuestalDetalle.EstatusId = ControlMaestroMapeo.EstatusRegistro.ACTIVO;
                    matrizConfiguracionPresupuestalDetalle.CreadoPorId = matrizConfiguracionPresupuestalDetalleTemp.CreadoPorId;
                    matrizConfiguracionPresupuestalDetalle.FechaCreacion = matrizConfiguracionPresupuestalDetalleTemp.FechaCreacion;
                    matrizConfiguracionPresupuestalDetalle.ModificadoPorId = usuarioId;
                    matrizConfiguracionPresupuestalDetalle.FechaCreacion = matrizConfiguracionPresupuestalDetalleTemp.FechaCreacion;
                    matrizConfiguracionPresupuestalDetalle.FechaUltimaModificacion = FechaModificacion; 
                    //Agregamos el objeto al list
                    matrizConfiguracionPresupuestalDetalleList.Add(matrizConfiguracionPresupuestalDetalle);
                }
            }

            new MatrizConfiguracionPresupuestalService().GuardaCambios(matrizConfiguracionPresupuestal, matrizConfiguracionPresupuestalDetalleList);

            return Json("Registros guardados con éxito!");
        }

        // GET: MIR/MatrizPresupuestoVigente
        public ActionResult Index()
        {
            return View();
        }

        public override ActionResult Listar()
        {
            MatrizPresupuestoVigenteListViewModel lvm = new MatrizPresupuestoVigenteListViewModel();
            lvm.ListadoMatrizConfiguracionPresupuestal =  new MatrizConfiguracionPresupuestalService().BuscaListadoPresupuestoVigente().ToList();
            return View("ListadoMatrizPresupuestoVigente", lvm);
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref MatrizPresupuestoVigenteViewModel matrizPresupuestoVigenteViewModel)
        { 
            //Buscamos la matriz de presupuesto vigente
            MItblMatrizConfiguracionPresupuestal matrizConfiguracionPresupuestal = new MatrizConfiguracionPresupuestalService().BuscaPorMIRId(matrizPresupuestoVigenteViewModel.MIRId, ControlMaestroMapeo.TipoPresupuesto.POR_EJERCER);
            matrizPresupuestoVigenteViewModel.ConfiguracionPresupuestoId = matrizConfiguracionPresupuestal == null ? 0 : matrizConfiguracionPresupuestal.ConfiguracionPresupuestoId;

            //Buscamos los detalles de la matriz  presupuesto vigente
            matrizPresupuestoVigenteViewModel.MatrizPresupuestoVigenteH = new MatrizConfiguracionPresupuestalDetalleHService().BuscaMatrizPresupuestoVigente(matrizPresupuestoVigenteViewModel.MIRId);

            //Buscamos el listado del Control de Periodos
            matrizPresupuestoVigenteViewModel.ControlPeriodoList = new ControlMaestroControlPeriodoService().BuscaTodos();

        }

        protected bool ValidaPeriodosAGuardar(MIspConsultaMatrizPresupuestoVigenteH_Result registro)
        {

            //Buscamos el listado del Control de Periodos
            List<MItblControlMaestroControlPeriodo> controlPeriodoList = new ControlMaestroControlPeriodoService().BuscaTodos().ToList();

            if (registro.Enero != null && controlPeriodoList.Find(f => f.Periodo == "Enero").EstatusPeriodoId != ControlMaestroMapeo.MIEstatusPeriodo.ABIERTO)
                return false;
            else if (registro.Febrero != null && controlPeriodoList.Find(f => f.Periodo == "Febrero").EstatusPeriodoId != ControlMaestroMapeo.MIEstatusPeriodo.ABIERTO)
                return false;
            else if (registro.Marzo != null && controlPeriodoList.Find(f => f.Periodo == "Marzo").EstatusPeriodoId != ControlMaestroMapeo.MIEstatusPeriodo.ABIERTO)
                return false;
            else if (registro.Abril != null && controlPeriodoList.Find(f => f.Periodo == "Abril").EstatusPeriodoId != ControlMaestroMapeo.MIEstatusPeriodo.ABIERTO)
                return false;
            else if (registro.Mayo != null && controlPeriodoList.Find(f => f.Periodo == "Mayo").EstatusPeriodoId != ControlMaestroMapeo.MIEstatusPeriodo.ABIERTO)
                return false;
            else if (registro.Junio != null && controlPeriodoList.Find(f => f.Periodo == "Junio").EstatusPeriodoId != ControlMaestroMapeo.MIEstatusPeriodo.ABIERTO)
                return false;
            else if (registro.Julio != null && controlPeriodoList.Find(f => f.Periodo == "Julio").EstatusPeriodoId != ControlMaestroMapeo.MIEstatusPeriodo.ABIERTO)
                return false;
            else if (registro.Agosto != null && controlPeriodoList.Find(f => f.Periodo == "Agosto").EstatusPeriodoId != ControlMaestroMapeo.MIEstatusPeriodo.ABIERTO)
                return false;
            else if (registro.Septiembre != null && controlPeriodoList.Find(f => f.Periodo == "Septiembre").EstatusPeriodoId != ControlMaestroMapeo.MIEstatusPeriodo.ABIERTO)
                return false;
            else if (registro.Octubre != null && controlPeriodoList.Find(f => f.Periodo == "Octubre").EstatusPeriodoId != ControlMaestroMapeo.MIEstatusPeriodo.ABIERTO)
                return false;
            else if (registro.Noviembre != null && controlPeriodoList.Find(f => f.Periodo == "Noviembre").EstatusPeriodoId != ControlMaestroMapeo.MIEstatusPeriodo.ABIERTO)
                return false;
            else if (registro.Diciembre != null && controlPeriodoList.Find(f => f.Periodo == "Diciembre").EstatusPeriodoId != ControlMaestroMapeo.MIEstatusPeriodo.ABIERTO)
                return false;
            return true;
        }
    }
}