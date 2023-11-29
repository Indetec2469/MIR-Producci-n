using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using RequisicionesAlmacen.Areas.MIR.MIR.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.MIR;
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.MIR.MIR.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.MIR)]
    public class MatrizIndicadorResultadoController : BaseController<MatrizIndicadorResultadoViewModel, MatrizIndicadorResultadoViewModel>
    {
        public override ActionResult Nuevo()
        {
            // Ejercicio desde sesion (iniciar sesion)
            string ejercicio = SessionHelper.GetUsuario().Ejercicio;
            // Crear los objetos nuevos
            MatrizIndicadorResultadoViewModel matrizIndicadorResultadoViewModel = new MatrizIndicadorResultadoViewModel();

            // Asignamos los modelos nuevos al ViewModel
            matrizIndicadorResultadoViewModel.MatrizIndicadorResultado = new MItblMatrizIndicadorResultado();
            matrizIndicadorResultadoViewModel.MatrizIndicadorResultadoModel = new MItblMatrizIndicadorResultado();
            matrizIndicadorResultadoViewModel.MatrizIndicadorResultadoIndicadorModel = new MItblMatrizIndicadorResultadoIndicador();
            matrizIndicadorResultadoViewModel.MatrizIndicadorResultadoIndicadorMetaModel = new MItblMatrizIndicadorResultadoIndicadorMeta();
            matrizIndicadorResultadoViewModel.MatrizIndicadorResultadoIndicadorFormulaVariableModel = new MItblMatrizIndicadorResultadoIndicadorFormulaVariable();

            // Inicializamos el ID en 0 y EstatusId para indicar que es un Registro Nuevo, tambien alunos.
            matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.MIRId = 0;
            matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.EstatusId = EstatusRegistro.ACTIVO;
            matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.Ejercicio = ejercicio;
            matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.FechaFinConfiguracion = new DateTime(Convert.ToInt32(matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.Ejercicio), 2, 15);

            // Agregamos todos los datos necesarios para el funcionamiento de la ficha
            // como son los listados para combos, tablas, arboles.
            matrizIndicadorResultadoViewModel.ListProgramaGobierno = new ProgramaGobiernoService().BuscaComboProgramaGobierno(matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.ProgramaPresupuestarioId);
            GetDatosFicha(ref matrizIndicadorResultadoViewModel);

            //Retornamos la vista junto con su Objeto Modelo
            return View("MatrizIndicadorResultado", matrizIndicadorResultadoViewModel);
        }

        public override ActionResult Editar(int id)
        {
            // Crear los objetos nuevos
            MatrizIndicadorResultadoViewModel matrizIndicadorResultadoViewModel = new MatrizIndicadorResultadoViewModel();
            //Buscamos los Objetos por el Id que se envio como parametro
            matrizIndicadorResultadoViewModel.MatrizIndicadorResultado = new MatrizIndicadorResultadoService().BuscaPorId(id);
            if (matrizIndicadorResultadoViewModel.MatrizIndicadorResultado == null)
            {
                return new HttpNotFoundResult("La MIR no existe la solicitud.");
            }
            // Asignamos los modelos nuevos al ViewModel
            matrizIndicadorResultadoViewModel.MatrizIndicadorResultadoModel = new MItblMatrizIndicadorResultado();
            matrizIndicadorResultadoViewModel.MatrizIndicadorResultadoIndicadorModel = new MItblMatrizIndicadorResultadoIndicador();
            matrizIndicadorResultadoViewModel.MatrizIndicadorResultadoIndicadorMetaModel = new MItblMatrizIndicadorResultadoIndicadorMeta();
            matrizIndicadorResultadoViewModel.MatrizIndicadorResultadoIndicadorFormulaVariableModel = new MItblMatrizIndicadorResultadoIndicadorFormulaVariable();

            matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicador = new MatrizIndicadorResultadoIndicadorService().BuscaPorMIRId(id);
            List<MItblMatrizIndicadorResultadoIndicadorMeta> listMatrizIndicadorResultadoIndicadorMeta = new List<MItblMatrizIndicadorResultadoIndicadorMeta>();
            List<MItblMatrizIndicadorResultadoIndicadorFormulaVariable> listMatrizIndicadorResultadoIndicadorFormulaVariable = new List<MItblMatrizIndicadorResultadoIndicadorFormulaVariable>();
            foreach (MItblMatrizIndicadorResultadoIndicador matrizIndicadorResultadoIndicador in matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicador)
            {
                // Matriz Indicador Resultado Indicador Meta
                IEnumerable<MItblMatrizIndicadorResultadoIndicadorMeta> _listMatrizIndicadorResultadoIndicadorMeta = new MatrizIndicadorResultadoIndicadorMetaService().BuscaPorMIRIndicadorId(matrizIndicadorResultadoIndicador.MIRIndicadorId);
                _listMatrizIndicadorResultadoIndicadorMeta.ToList().ForEach(mirim =>
                {
                    listMatrizIndicadorResultadoIndicadorMeta.Add(mirim); 
                });
                // Matriz Indicador Resultado Indicador Formula Variable
                IEnumerable<MItblMatrizIndicadorResultadoIndicadorFormulaVariable> _listMatrizIndicadorResultadoIndicadorFormulaVariable = new MatrizIndicadorResultadoIndicadorFormulaVariableService().BuscaPorMIRIndicadorId(matrizIndicadorResultadoIndicador.MIRIndicadorId);
                _listMatrizIndicadorResultadoIndicadorFormulaVariable.ToList().ForEach(mirim =>
                {
                    listMatrizIndicadorResultadoIndicadorFormulaVariable.Add(mirim);
                });
            }
            matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicadorMeta = listMatrizIndicadorResultadoIndicadorMeta;
            matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicadorFormulaVariable = listMatrizIndicadorResultadoIndicadorFormulaVariable;

            //Asignamos el lisatado de Estructuras
            //matrizIndicadorResultadoViewModel.ListPlanDesarrolloEstructura = new PlanDesarrolloService().BuscaDetallesPorPlanDesarrolloId(mir.PlanDesarrolloId);

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            matrizIndicadorResultadoViewModel.ListProgramaGobierno = new ProgramaGobiernoService().BuscaComboProgramaGobierno(matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.ProgramaPresupuestarioId);
            GetDatosFicha(ref matrizIndicadorResultadoViewModel);

            //Retornamos la vista junto con su Objeto Modelo
            return View("MatrizIndicadorResultado", matrizIndicadorResultadoViewModel);
        }

        [JsonException]
        public override JsonResult Guardar(MatrizIndicadorResultadoViewModel matrizIndicadorResultadoViewModel)
        {
            // Obtenemos el sesion usuario
            var sessionUsuario = SessionHelper.GetUsuario();

            //Validar que si se quiere borrar la MIR, o este siendo utilizada ya en otros procesos
            if (matrizIndicadorResultadoViewModel.MatrizIndicadorResultado != null && matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.MIRId > 0 && matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.EstatusId == EstatusRegistro.BORRADO)
            {
                //Validar si se puede borrar la MIR
                bool permiteEliminar = new SistemaService().PermiteEliminarRegistro(matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.MIRId, Tablas.MItblMatrizIndicadorResultado);

                if (!permiteEliminar)
                {
                    throw new Exception("La MIR con el código [" + matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.Codigo + "] no puede ser eliminada ya que está siendo utilizada para otros procesos.");
                }
            }

            //Validar los registros por borrar, si hubiera
            if (matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicador != null)
            {
                foreach (MItblMatrizIndicadorResultadoIndicador _tmpIndicador in matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicador)
                {
                    if(_tmpIndicador.MIRIndicadorId > 0 && _tmpIndicador.EstatusId == EstatusRegistro.BORRADO)
                    {
                        bool permiteEliminar = new SistemaService().PermiteEliminarRegistro(_tmpIndicador.MIRIndicadorId, Tablas.MItblMatrizIndicadorResultadoIndicador);

                        if (!permiteEliminar)
                        {
                            if (_tmpIndicador.NivelIndicadorId == ControlMaestroMapeo.Nivel.FIN)                              
                                throw new Exception("El FIN con el código [" + _tmpIndicador.Codigo + "] no puede ser eliminado ya que tiene un indicador de propósito relacionados.");
                            else if (_tmpIndicador.NivelIndicadorId == ControlMaestroMapeo.Nivel.PROPOSITO)                                
                                throw new Exception("El PROPOSITO con el código [" + _tmpIndicador.Codigo + "] no puede ser eliminado ya que tiene un indicador de componente relacionado.");
                            else if (_tmpIndicador.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE) 
                                throw new Exception("El COMPONENTE con el código [" + _tmpIndicador.Codigo + "] no puede ser eliminado ya que tiene un indicador de actividad relacionado.");
                            else if(_tmpIndicador.NivelIndicadorId == ControlMaestroMapeo.Nivel.ACTIVIDAD)
                                throw new Exception("La ACTIVIDAD con el código [" + _tmpIndicador.Codigo + "] no puede ser eliminada ya que tiene una relación presupuestal");
                        }
                    }
                }
            }

            // Matriz Indicador Resultado
            if (matrizIndicadorResultadoViewModel.MatrizIndicadorResultado != null)
            {
                // Service
                MatrizIndicadorResultadoService matrizIndicadorResultadoService = new MatrizIndicadorResultadoService();

                // Verificar si el usuario ha sido modificado en mismo y regresa mensaje de error
                Boolean esExisteProgramaPresupuestario = matrizIndicadorResultadoService.BuscaExisteProgramaPresupuestario(matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.MIRId, matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.ProgramaPresupuestarioId);
                if (esExisteProgramaPresupuestario)
                {
                    throw new Exception("La seleccion de Programa Presupuestario ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                }

                // Si el ID es nuevo para registrar o actualizar
                if (matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.MIRId > 0)
                {
                    // Verificar si el usuario ha sido modificado en mismo y regresa mensaje de error
                    MItblMatrizIndicadorResultado matrizIndicadorResultado = matrizIndicadorResultadoService.BuscaPorId(matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.MIRId);
                    if (!StructuralComparisons.StructuralEqualityComparer.Equals(matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.Timestamp, matrizIndicadorResultado.Timestamp))
                    {
                        throw new Exception("La Matriz Indicador Resultado con el código [" + matrizIndicadorResultado.Codigo + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                    }

                    // Validamos existe los datos de Configuracion de Presupuestal
                    if (ExisteConfiguracionPresupuestal(matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.MIRId, null))
                    {
                        throw new Exception("No se puede eliminar la MIR con el código [" + matrizIndicadorResultado.Codigo + "] porque ya esta guardados los datos en la matriz de Configuración de Presupuestal.");
                    }

                    matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.ModificadoPorId = sessionUsuario.UsuarioId;
                    matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.FechaUltimaModificacion = DateTime.Now;
                }
                else
                {
                    //// Ejercicio
                    //int ejercicio = Convert.ToInt32(sessionUsuario.Ejercicio);

                    //matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.Codigo = new AutonumericoService().GetSiguienteAutonumerico("MIR", ejercicio);

                    //// Si Codigo MIR no esta aplica y regresa un mensaje de error.
                    //if (matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.Codigo == null)
                    //{
                    //    throw new Exception("Error al generar el Código de la MIR");
                    //}

                    matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.CreadoPorId = sessionUsuario.UsuarioId;
                }
            }

            // Matriz Indicador Resultado Indicador
            if (matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicador != null)
            {
                // Service
                MatrizIndicadorResultadoIndicadorService matrizIndicadorResultadoIndicadorService = new MatrizIndicadorResultadoIndicadorService();

                IEnumerable<MItblMatrizIndicadorResultadoIndicador> ListMatrizIndicadorResultadoIndicadorSinActividad = matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicador.Where(miri => miri.NivelIndicadorId != Nivel.ACTIVIDAD);
                IEnumerable<MItblMatrizIndicadorResultadoIndicador> ListMatrizIndicadorResultadoIndicadorConActividad = matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicador.Where(miri => miri.NivelIndicadorId == Nivel.ACTIVIDAD);

                // Sin Actividad es para cuando guardar MIRI para poner el MIRIndicadorId a MIRIndicadorComponenteId
                foreach (MItblMatrizIndicadorResultadoIndicador matrizIndicadorResultadoIndicador in ListMatrizIndicadorResultadoIndicadorSinActividad)
                {
                    // Si el ID es nuevo para registrar o actualizar
                    if (matrizIndicadorResultadoIndicador.MIRIndicadorId > 0)
                    {
                        // Verificar si el usuario ha sido modificado en mismo y regresa mensaje de error
                        MItblMatrizIndicadorResultadoIndicador _matrizIndicadorResultadoIndicador = matrizIndicadorResultadoIndicadorService.BuscaPorId(matrizIndicadorResultadoIndicador.MIRIndicadorId);
                        if (!StructuralComparisons.StructuralEqualityComparer.Equals(matrizIndicadorResultadoIndicador.Timestamp, _matrizIndicadorResultadoIndicador.Timestamp))
                        {
                            throw new Exception("La Matriz Indicador Resultado Indicador con el código [" + _matrizIndicadorResultadoIndicador.Codigo + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                        }

                        // Validamos existe los datos de Configuracion de Presupuestal
                        if (ExisteConfiguracionPresupuestal(null, matrizIndicadorResultadoIndicador.MIRIndicadorId))
                        {
                            throw new Exception("No se puede eliminar la MIRI con el código [" + matrizIndicadorResultadoIndicador.Codigo + "] porque ya esta guardados los datos en la matriz de Configuración de Presupuestal.");
                        }

                        matrizIndicadorResultadoIndicador.ModificadoPorId = sessionUsuario.UsuarioId;
                        matrizIndicadorResultadoIndicador.FechaUltimaModificacion = DateTime.Now;
                    }
                    else
                    {
                        //switch (matrizIndicadorResultadoIndicador.NivelIndicadorId)
                        //{
                        //    // Nivel Fin
                        //    case 40:
                        //        matrizIndicadorResultadoIndicador.Codigo = new AutonumericoService().GetSiguienteAutonumerico("Nivel Fin");
                        //        break;

                        //    case 41:
                        //        matrizIndicadorResultadoIndicador.Codigo = new AutonumericoService().GetSiguienteAutonumerico("Nivel Proposito");
                        //        break;
                        //    case 42:
                        //        matrizIndicadorResultadoIndicador.Codigo = new AutonumericoService().GetSiguienteAutonumerico("Nivel Componente");
                        //        break;
                        //    case 43:
                        //        matrizIndicadorResultadoIndicador.Codigo = new AutonumericoService().GetSiguienteAutonumerico("Nivel Actividad");
                        //        break;

                        //    default:
                        //        throw new Exception("El Codigo de Nivel no esta aplica.");
                        //}

                        //// Si Codigo Nivel Fin no esta aplica y regresa un mensaje de error.
                        //if (matrizIndicadorResultadoIndicador.Codigo == null)
                        //{
                        //    throw new Exception("El Codigo de Nivel no esta aplica.");
                        //}

                        matrizIndicadorResultadoIndicador.CreadoPorId = sessionUsuario.UsuarioId;
                    }
                }

                // Con Actividad
                foreach (MItblMatrizIndicadorResultadoIndicador matrizIndicadorResultadoIndicador in ListMatrizIndicadorResultadoIndicadorConActividad)
                {
                    // Si el ID es nuevo para registrar o actualizar
                    if (matrizIndicadorResultadoIndicador.MIRIndicadorId > 0)
                    {
                        // Verificar si el usuario ha sido modificado en mismo y regresa mensaje de error
                        MItblMatrizIndicadorResultadoIndicador _matrizIndicadorResultadoIndicador = matrizIndicadorResultadoIndicadorService.BuscaPorId(matrizIndicadorResultadoIndicador.MIRIndicadorId);
                        if (!StructuralComparisons.StructuralEqualityComparer.Equals(matrizIndicadorResultadoIndicador.Timestamp, _matrizIndicadorResultadoIndicador.Timestamp))
                        {
                            throw new Exception("La Matriz Indicador Resultado Indicador con el código [" + _matrizIndicadorResultadoIndicador.Codigo + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                        }

                        // Validamos existe los datos de Configuracion de Presupuestal
                        if (ExisteConfiguracionPresupuestal(null, matrizIndicadorResultadoIndicador.MIRIndicadorId))
                        {
                            throw new Exception("No se puede eliminar la MIRI con el código [" + matrizIndicadorResultadoIndicador.Codigo + "] porque ya esta guardandos los datos en la matriz de Configuración de Presupuestal.");
                        }

                        matrizIndicadorResultadoIndicador.ModificadoPorId = sessionUsuario.UsuarioId;
                        matrizIndicadorResultadoIndicador.FechaUltimaModificacion = DateTime.Now;
                    }
                    else
                    {
                        //switch (matrizIndicadorResultadoIndicador.NivelIndicadorId)
                        //{
                        //    // Nivel Fin
                        //    case 40:
                        //        matrizIndicadorResultadoIndicador.Codigo = new AutonumericoService().GetSiguienteAutonumerico("Nivel Fin");
                        //        break;

                        //    case 41:
                        //        matrizIndicadorResultadoIndicador.Codigo = new AutonumericoService().GetSiguienteAutonumerico("Nivel Proposito");
                        //        break;
                        //    case 42:
                        //        matrizIndicadorResultadoIndicador.Codigo = new AutonumericoService().GetSiguienteAutonumerico("Nivel Componente");
                        //        break;
                        //    case 43:
                        //        matrizIndicadorResultadoIndicador.Codigo = new AutonumericoService().GetSiguienteAutonumerico("Nivel Actividad");
                        //        break;

                        //    default:
                        //        throw new Exception("No se pudo generar un código para el componente.");
                        //}

                        //// Si Codigo Nivel Fin no esta aplica y regresa un mensaje de error.
                        //if (matrizIndicadorResultadoIndicador.Codigo == null)
                        //{
                        //    throw new Exception("No se pudo generar un código para el componente");
                        //}

                        matrizIndicadorResultadoIndicador.CreadoPorId = sessionUsuario.UsuarioId;
                    }
                }

            }

            // Matriz Indicador Resultado Indicador Meta
            if (matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicadorMeta != null)
            {
                // Service
                MatrizIndicadorResultadoIndicadorMetaService matrizIndicadorResultadoIndicadorMetaService = new MatrizIndicadorResultadoIndicadorMetaService();

                foreach (MItblMatrizIndicadorResultadoIndicadorMeta matrizIndicadorResultadoIndicadorMeta in matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicadorMeta)
                {
                    // Si el ID es nuevo para registrar o actualizar
                    if (matrizIndicadorResultadoIndicadorMeta.MIRIndicadorMetaId > 0)
                    {
                        // Verificar si el usuario ha sido modificado en mismo y regresa mensaje de error
                        MItblMatrizIndicadorResultadoIndicadorMeta _matrizIndicadorResultadoIndicadorMeta = matrizIndicadorResultadoIndicadorMetaService.BuscaPorId(matrizIndicadorResultadoIndicadorMeta.MIRIndicadorMetaId);
                        if (!StructuralComparisons.StructuralEqualityComparer.Equals(matrizIndicadorResultadoIndicadorMeta.Timestamp, _matrizIndicadorResultadoIndicadorMeta.Timestamp))
                        {
                            throw new Exception("La Matriz Indicador Resultado Indicador Meta con el código [" + _matrizIndicadorResultadoIndicadorMeta.MIRIndicadorMetaId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                        }

                        matrizIndicadorResultadoIndicadorMeta.ModificadoPorId = sessionUsuario.UsuarioId;
                        matrizIndicadorResultadoIndicadorMeta.FechaUltimaModificacion = DateTime.Now;
                    }
                    else
                    {
                        matrizIndicadorResultadoIndicadorMeta.CreadoPorId = sessionUsuario.UsuarioId;
                    }
                }

            }

            // Matriz Indicador Resultado Indicador Formula Variable
            if (matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicadorFormulaVariable != null)
            {
                // Service
                MatrizIndicadorResultadoIndicadorFormulaVariableService matrizIndicadorResultadoIndicadorFormulaVariableService = new MatrizIndicadorResultadoIndicadorFormulaVariableService();

                foreach (MItblMatrizIndicadorResultadoIndicadorFormulaVariable matrizIndicadorResultadoIndicadorFormulaVariable in matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicadorFormulaVariable)
                {
                    // Si el ID es nuevo para registrar o actualizar
                    if (matrizIndicadorResultadoIndicadorFormulaVariable.MIRIndicadorFormulaVariableId > 0)
                    {
                        // Verificar si el usuario ha sido modificado en mismo y regresa mensaje de error
                        MItblMatrizIndicadorResultadoIndicadorFormulaVariable _matrizIndicadorResultadoIndicadorFormulaVariable = matrizIndicadorResultadoIndicadorFormulaVariableService.BuscaPorId(matrizIndicadorResultadoIndicadorFormulaVariable.MIRIndicadorFormulaVariableId);
                        if (!StructuralComparisons.StructuralEqualityComparer.Equals(matrizIndicadorResultadoIndicadorFormulaVariable.Timestamp, _matrizIndicadorResultadoIndicadorFormulaVariable.Timestamp))
                        {
                            throw new Exception("La Matriz Indicador Resultado Indicador Formula Variable con el código [" + _matrizIndicadorResultadoIndicadorFormulaVariable.MIRIndicadorFormulaVariableId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                        }

                        matrizIndicadorResultadoIndicadorFormulaVariable.ModificadoPorId = sessionUsuario.UsuarioId;
                        matrizIndicadorResultadoIndicadorFormulaVariable.FechaUltimaModificacion = DateTime.Now;
                    }
                    else
                    {
                        matrizIndicadorResultadoIndicadorFormulaVariable.CreadoPorId = sessionUsuario.UsuarioId;
                    }
                }
            }

            new MatrizIndicadorResultadoService().GuardaCambios(Convert.ToInt32(sessionUsuario.Ejercicio), matrizIndicadorResultadoViewModel.MatrizIndicadorResultado, matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicador, matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicadorMeta, matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultadoIndicadorFormulaVariable);
            
            return Json("Registro guardado con Exito!");
        }

        [JsonException]
        public override JsonResult Eliminar(int id)
        {
            // Usuario
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;

            // Service
            MatrizIndicadorResultadoService matrizIndicadorResultadoService = new MatrizIndicadorResultadoService();

            MItblMatrizIndicadorResultado matrizIndicadorResultado = matrizIndicadorResultadoService.BuscaPorId(id);
            if (matrizIndicadorResultado.EstatusId == EstatusRegistro.BORRADO)
            {
                throw new Exception("La MIR con el código [" + matrizIndicadorResultado.Codigo + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
            }
            
            //Validar si se puede borrar la MIR
            bool permiteEliminar = new SistemaService().PermiteEliminarRegistro(id, Tablas.MItblMatrizIndicadorResultado);

            if (!permiteEliminar)
            {
                throw new Exception("La MIR no puede ser eliminada ya que está siendo utilizada para otros procesos.");
            }

            // Eliminamos Matriz Indicador Resultado
            matrizIndicadorResultado.EstatusId = EstatusRegistro.BORRADO;
            matrizIndicadorResultado.ModificadoPorId = usuarioId;
            matrizIndicadorResultado.FechaUltimaModificacion = DateTime.Now;

            // Eliminamos Matriz Indicador Resultado Indicador
            IEnumerable<MItblMatrizIndicadorResultadoIndicador> listMatrizIndicadorResultadoIndicador = new MatrizIndicadorResultadoIndicadorService().BuscaPorMIRId(id);
            foreach (MItblMatrizIndicadorResultadoIndicador matrizIndicadorResultadoIndicador in listMatrizIndicadorResultadoIndicador)
            {
                matrizIndicadorResultadoIndicador.EstatusId = EstatusRegistro.BORRADO;
                matrizIndicadorResultadoIndicador.ModificadoPorId = usuarioId;
                matrizIndicadorResultadoIndicador.FechaUltimaModificacion = DateTime.Now;
            }

            // Eliminamos Matriz Indicador Resultado Indicador Meta
            List<MItblMatrizIndicadorResultadoIndicadorMeta> listMatrizIndicadorResultadoIndicadorMeta = new List<MItblMatrizIndicadorResultadoIndicadorMeta>();
            foreach (MItblMatrizIndicadorResultadoIndicador matrizIndicadorResultadoIndicador in listMatrizIndicadorResultadoIndicador)
            {
                IEnumerable<MItblMatrizIndicadorResultadoIndicadorMeta> _listMatrizIndicadorResultadoIndicadorMeta = new MatrizIndicadorResultadoIndicadorMetaService().BuscaPorMIRIndicadorId(matrizIndicadorResultadoIndicador.MIRIndicadorId);
                _listMatrizIndicadorResultadoIndicadorMeta.ToList().ForEach(mirim =>
                {
                    listMatrizIndicadorResultadoIndicadorMeta.Add(mirim);
                });
            }
            foreach (MItblMatrizIndicadorResultadoIndicadorMeta matrizIndicadorResultadoIndicadorMeta in listMatrizIndicadorResultadoIndicadorMeta)
            {
                matrizIndicadorResultadoIndicadorMeta.EstatusId = EstatusRegistro.BORRADO;
                matrizIndicadorResultadoIndicadorMeta.ModificadoPorId = usuarioId;
                matrizIndicadorResultadoIndicadorMeta.FechaUltimaModificacion = DateTime.Now;
            }

            // Eliminamos Matriz Indicador Resultado Indicador Formula Variable
            List<MItblMatrizIndicadorResultadoIndicadorFormulaVariable> listMatrizIndicadorResultadoIndicadorFormulaVariable = new List<MItblMatrizIndicadorResultadoIndicadorFormulaVariable>();
            foreach (MItblMatrizIndicadorResultadoIndicador matrizIndicadorResultadoIndicador in listMatrizIndicadorResultadoIndicador)
            {
                IEnumerable<MItblMatrizIndicadorResultadoIndicadorFormulaVariable> _listMatrizIndicadorResultadoIndicadorFormulaVariable = new MatrizIndicadorResultadoIndicadorFormulaVariableService().BuscaPorMIRIndicadorId(matrizIndicadorResultadoIndicador.MIRIndicadorId);
                _listMatrizIndicadorResultadoIndicadorFormulaVariable.ToList().ForEach(fv =>
                {
                    listMatrizIndicadorResultadoIndicadorFormulaVariable.Add(fv);
                });
            }
            foreach (MItblMatrizIndicadorResultadoIndicadorFormulaVariable matrizIndicadorResultadoIndicadorFormulaVariable in listMatrizIndicadorResultadoIndicadorFormulaVariable)
            {
                matrizIndicadorResultadoIndicadorFormulaVariable.EstatusId = EstatusRegistro.BORRADO;
                matrizIndicadorResultadoIndicadorFormulaVariable.ModificadoPorId = usuarioId;
                matrizIndicadorResultadoIndicadorFormulaVariable.FechaUltimaModificacion = DateTime.Now;
            }

            matrizIndicadorResultadoService.GuardaCambios(Convert.ToInt32(SessionHelper.GetUsuario().Ejercicio), matrizIndicadorResultado, listMatrizIndicadorResultadoIndicador, listMatrizIndicadorResultadoIndicadorMeta, listMatrizIndicadorResultadoIndicadorFormulaVariable);

            return Json("Registro eliminado con Exito!");
        }

        public override ActionResult Listar()
        {
            MatrizIndicadorResultadoViewModel matrizIndicadorResultadoViewModel = new MatrizIndicadorResultadoViewModel();

            matrizIndicadorResultadoViewModel.ListMatrizIndicadorResultado = new MatrizIndicadorResultadoService().BuscaListado();

            return View("ListadoMatrizIndicadorResultado", matrizIndicadorResultadoViewModel);
        }

        protected override void GetDatosFicha(ref MatrizIndicadorResultadoViewModel matrizIndicadorResultadoViewModel)
        {
            // Usuario
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            // Busca el usuario tiene permiso para el proyecto
            matrizIndicadorResultadoViewModel.EsPermisoProyecto = new UsuarioPermisoService().BuscaUsuarioPermiso(usuarioId, PermisoFichaMapeo.ID.PERMISO_PROYECTO_MIR);

            matrizIndicadorResultadoViewModel.ListPlanDesarrollo = new PlanDesarrolloService().BuscaTodos();
            matrizIndicadorResultadoViewModel.ListPlanDesarrolloEstructura = new PlanDesarrolloEstructuraService().BuscaTodos();
            //matrizIndicadorResultadoViewModel.ListProgramaGobierno = new ProgramaGobiernoService().BuscaTodos();
            matrizIndicadorResultadoViewModel.ListControlMaestroTipoIndicadorConNivel = new ControlMaestroTipoIndicadorService().BuscaTodosConNivel();
            matrizIndicadorResultadoViewModel.ListControlMaestroDimensionConNivel = new ControlMaestroDimensionService().BuscaTodosConNivel();
            matrizIndicadorResultadoViewModel.ListControlMaestroUnidadMedidaConDimension = new ControlMaestroUnidadMedidaService().BuscaTodosConDimension();
            matrizIndicadorResultadoViewModel.ListControlMaestroUnidadMedidaFormulaVariable = new ControlMaestroUnidadMedidaFormulaVariableService().BuscaTodos();
            matrizIndicadorResultadoViewModel.ListControlMaestroFrecuenciaMedicionConNivel = new ControlMaestroFrecuenciaMedicionService().BuscaTodosConNivel();
            // Control Maestro
            matrizIndicadorResultadoViewModel.ListNivel = new ControlMaestroService().BuscaControl("Nivel");
            matrizIndicadorResultadoViewModel.ListSentido = new ControlMaestroService().BuscaControl("Sentido");
            matrizIndicadorResultadoViewModel.ListTipoComponente = new ControlMaestroService().BuscaControl("TipoComponente");
            // SAACG
            if (matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.MIRId > 0)
            {
                // Obtener lista de ID de Proyecto
                List<string> listProyectoId = new ProyectoService().BuscaRelacionProyectoYProgramaGobierno(matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.ProgramaPresupuestarioId);
                
                int _ejercicio = Int32.Parse(matrizIndicadorResultadoViewModel.MatrizIndicadorResultado.Ejercicio);
                DateTime fechaInicio = new DateTime(_ejercicio, 1, 1);
                DateTime fechaFin = new DateTime(_ejercicio, 12, 31);
                matrizIndicadorResultadoViewModel.ListProyecto = new ProyectoService().BuscaFechaInicioYFechaFin(fechaInicio, fechaFin).Where(p => listProyectoId.Contains(p.ProyectoId));
            }
            // Ejercicio
            List<EjercicioModel> listEjercicio = new List<EjercicioModel>();
            EjercicioModel ejercicioModel = new EjercicioModel();
            ejercicioModel.Ejercicio = "2020";
            listEjercicio.Add(ejercicioModel);
            ejercicioModel = new EjercicioModel();
            ejercicioModel.Ejercicio = "2021";
            listEjercicio.Add(ejercicioModel);
            ejercicioModel = new EjercicioModel();
            ejercicioModel.Ejercicio = "2022";
            listEjercicio.Add(ejercicioModel);
            ejercicioModel = new EjercicioModel();
            ejercicioModel.Ejercicio = "2023";
            listEjercicio.Add(ejercicioModel);
            ejercicioModel = new EjercicioModel();
            ejercicioModel.Ejercicio = "2024";
            listEjercicio.Add(ejercicioModel);
            ejercicioModel = new EjercicioModel();
            ejercicioModel.Ejercicio = "2025";
            listEjercicio.Add(ejercicioModel);
            ejercicioModel = new EjercicioModel();
            ejercicioModel.Ejercicio = "2026";
            listEjercicio.Add(ejercicioModel);
            matrizIndicadorResultadoViewModel.ListEjercicio = listEjercicio;
        }

        public class EjercicioModel
        {
            public string Ejercicio { get; set; }
        }

        public ActionResult ObtenerProyectos(string ejercicio, string programaPresupuestarioId)
        {
            // Obtener lista de ID de Proyecto
            List<string> listProyectoId = new ProyectoService().BuscaRelacionProyectoYProgramaGobierno(programaPresupuestarioId);

            int _ejercicio = Int32.Parse(ejercicio);
            DateTime fechaInicio = new DateTime(_ejercicio, 1, 1);
            DateTime fechaFin = new DateTime(_ejercicio, 12, 31);
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-MX");
            var resultJson = JsonConvert.SerializeObject(new ProyectoService().BuscaFechaInicioYFechaFin(fechaInicio, fechaFin).Where(p => listProyectoId.Contains(p.ProyectoId)));
            return Content(resultJson, "application/json");
        }

        public Boolean ExisteConfiguracionPresupuestal(int? mirId, int? mirIndicadorId)
        {
            if (mirId.HasValue)
            {
                return new MatrizConfiguracionPresupuestalService().ExistePorMIR(mirId.Value);
            }

            if (mirIndicadorId.HasValue)
            {
                return new MatrizConfiguracionPresupuestalDetalleService().ExistePorMIRI(mirIndicadorId.Value);
            }

            return false;
        }

        public ActionResult FechaDelServidor()
        {
            spObtenerFechaDelServidor_Result fechaDelServidor = new MatrizIndicadorResultadoService().BuscaFechaDelServidor();
            return Json(fechaDelServidor.FechaDelServidor);
        }

        // API MVC para la seleccion de proyecto
        //public ActionResult GetProyecto(string ejercicio, DataSourceLoadOptions loadOptions)
        //{
        //    int _ejercicio = Int32.Parse(ejercicio);
        //    DateTime fechaInicio = new DateTime(_ejercicio, 1,1);
        //    DateTime fechaFin = new DateTime(_ejercicio, 12, 31);
        //    var result = DataSourceLoader.Load(new ProyectoService().BuscaFechaInicioYFechaFin(fechaInicio, fechaFin), loadOptions);
        //    var resultJson = JsonConvert.SerializeObject(result);
        //    return Content(resultJson, "application/json");
        //}
    }
}