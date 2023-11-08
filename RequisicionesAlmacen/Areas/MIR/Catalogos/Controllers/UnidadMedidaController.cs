using NCalc;
using RequisicionesAlmacen.Areas.MIR.Catalogos.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.MIR.Catalogos.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.UNIDADES_DE_MEDIDA)]
    public class UnidadMedidaController : BaseController<ControlMaestroUnidadMedidaViewModel, ControlMaestroUnidadMedidaViewModel>
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
        public override JsonResult Guardar(ControlMaestroUnidadMedidaViewModel controlMaestroUnidadMedidaViewModel)
        {
            // Usuario
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            // Services
            ControlMaestroUnidadMedidaService controlMaestroUnidadMedidaService = new ControlMaestroUnidadMedidaService();
            ControlMaestroUnidadMedidaDimensionService controlMaestroUnidadMedidaDimensionService = new ControlMaestroUnidadMedidaDimensionService();
            ControlMaestroUnidadMedidaFormulaVariableService controlMaestroUnidadMedidaFormulaVariableService = new ControlMaestroUnidadMedidaFormulaVariableService();

            if (controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedida != null)
            {
                foreach (MItblControlMaestroUnidadMedida controlMaestroUnidadMedida in controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedida)
                {
                    // Verificar si el usuario ha sido modificado en mismo y regresa mensaje de error
                    if (controlMaestroUnidadMedida.UnidadMedidaId <= 0 && controlMaestroUnidadMedidaService.EsNombreExiste(controlMaestroUnidadMedida))
                        throw new Exception("La Fórmulas a Unidades de Medida con el nombre [" + controlMaestroUnidadMedida.Nombre + "] ya existe. Favor de cambiarlo.");
                    // Control Maestro Unidad Medida
                    MItblControlMaestroUnidadMedida _controlMaestroUnidadMedida = controlMaestroUnidadMedida;
                    int idPadre = controlMaestroUnidadMedida.UnidadMedidaId;
                    if (controlMaestroUnidadMedida.UnidadMedidaId > 0)
                    {
                        // Verificar si el usuario ha sido modificado en mismo y regresa mensaje de error
                        MItblControlMaestroUnidadMedida __controlMaestroUnidadMedida = controlMaestroUnidadMedidaService.BuscaPorId(controlMaestroUnidadMedida.UnidadMedidaId);
                        if (!StructuralComparisons.StructuralEqualityComparer.Equals(controlMaestroUnidadMedida.Timestamp, __controlMaestroUnidadMedida.Timestamp))
                        {
                            throw new Exception("La Fórmulas a Unidades de Medida con el código [" + controlMaestroUnidadMedida.UnidadMedidaId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                        }

                        if (controlMaestroUnidadMedida.Borrado)
                        {
                            //Verificar que no este siendo utilizado
                            bool permiteEliminar = new SistemaService().PermiteEliminarRegistro(controlMaestroUnidadMedida.UnidadMedidaId, "MItblControlMaestroUnidadMedida");
                            if (!permiteEliminar)
                            {
                                throw new Exception("La Fórmulas a Unidades de Medida con el código [" + controlMaestroUnidadMedida.UnidadMedidaId + "] no puede ser eliminado ya que está siendo utilizado para otros procesos.");
                            }
                        }

                        controlMaestroUnidadMedida.ModificadoPorId = usuarioId;
                        controlMaestroUnidadMedida.FechaUltimaModificacion = DateTime.Now;
                    }
                    else
                    {
                        controlMaestroUnidadMedida.CreadoPorId = usuarioId;
                    }
                    // Control Maestro Unidad Medida Dimension
                    if (controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedidaDimension != null)
                    {
                        List<MItblControlMaestroUnidadMedidaDimension> listControlMaestroUnidadMedidaDimension = controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedidaDimension.Where(dimension => dimension.UnidadMedidaId == idPadre).ToList();
                        if (listControlMaestroUnidadMedidaDimension != null)
                        {
                            foreach (MItblControlMaestroUnidadMedidaDimension controlMaestroUnidadMedidaDimension in listControlMaestroUnidadMedidaDimension)
                            {
                                if (controlMaestroUnidadMedidaDimension.UnidadMedidaDimensionId > 0)
                                {
                                    // Verificar si el usuario ha sido modificado en mismo y regresa mensaje de error
                                    //if (controlMaestroUnidadMedidaDimensionService.EsDimensionExiste(controlMaestroUnidadMedidaDimension))
                                        //throw new Exception("La Unidad de Medida Dimensión ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                                    // Verificar si el usuario ha sido modificado en mismo Fórmula Dimensión y regresa mensaje de error
                                    MItblControlMaestroUnidadMedidaDimension _controlMaestroUnidadMedidaDimension = controlMaestroUnidadMedidaDimensionService.BuscaPorId(controlMaestroUnidadMedidaDimension.UnidadMedidaDimensionId);
                                    if (!StructuralComparisons.StructuralEqualityComparer.Equals(controlMaestroUnidadMedidaDimension.Timestamp, _controlMaestroUnidadMedidaDimension.Timestamp))
                                        throw new Exception("La Dimensión ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");

                                    controlMaestroUnidadMedidaDimension.ModificadoPorId = usuarioId;
                                    controlMaestroUnidadMedidaDimension.FechaUltimaModificacion = DateTime.Now;
                                }
                                else
                                {
                                    // Verificar si el usuario ha sido modificado en mismo Dimensión y regresa mensaje de error
                                    if (controlMaestroUnidadMedidaDimensionService.EsDimensionExiste(controlMaestroUnidadMedidaDimension))
                                        throw new Exception("La Dimensión ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");

                                    controlMaestroUnidadMedidaDimension.CreadoPorId = usuarioId;
                                }
                            }
                        }
                    }
                    // Control Maestro Unidad Medida Formula Variable
                    if (controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedidaFormulaVariable != null)
                    {
                        List<MItblControlMaestroUnidadMedidaFormulaVariable> listControlMaestroUnidadMedidaFormulaVariable = controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedidaFormulaVariable.Where(formulaVariable => formulaVariable.UnidadMedidaId == idPadre).ToList();
                        if (listControlMaestroUnidadMedidaFormulaVariable != null)
                        {
                            foreach (MItblControlMaestroUnidadMedidaFormulaVariable controlMaestroUnidadMedidaFormulaVariable in listControlMaestroUnidadMedidaFormulaVariable)
                            {
                                if (controlMaestroUnidadMedidaFormulaVariable.UnidadMedidaFormulaVariableId > 0)
                                {
                                    // Verificar si el usuario ha sido modificado en mismo Variabla y regresa mensaje de error
                                    if (controlMaestroUnidadMedidaFormulaVariableService.EsVariableExiste(controlMaestroUnidadMedidaFormulaVariable))
                                        throw new Exception("La Fórmula Variable con el código [" + controlMaestroUnidadMedidaFormulaVariable.UnidadMedidaFormulaVariableId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                                    // Verificar si el usuario ha sido modificado en mismo Fórmula y regresa mensaje de error
                                    MItblControlMaestroUnidadMedidaFormulaVariable _controlMaestroUnidadMedidaFormulaVariable = controlMaestroUnidadMedidaFormulaVariableService.BuscaPorId(controlMaestroUnidadMedidaFormulaVariable.UnidadMedidaFormulaVariableId);
                                    if (!StructuralComparisons.StructuralEqualityComparer.Equals(controlMaestroUnidadMedidaFormulaVariable.Timestamp, _controlMaestroUnidadMedidaFormulaVariable.Timestamp))
                                    {
                                        throw new Exception("La Fórmula Variable ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                                    }

                                    controlMaestroUnidadMedidaFormulaVariable.ModificadoPorId = usuarioId;
                                    controlMaestroUnidadMedidaFormulaVariable.FechaUltimaModificacion = DateTime.Now;
                                    //controlMaestroUnidadMedidaFormulaVariableService.Actualiza(controlMaestroUnidadMedidaFormulaVariable);
                                }
                                else
                                {
                                    // Verificar si el usuario ha sido modificado en mismo Fórmula y regresa mensaje de error
                                    if (controlMaestroUnidadMedidaFormulaVariableService.EsVariableExiste(controlMaestroUnidadMedidaFormulaVariable))
                                        throw new Exception("La Fórmula Variable con el código [" + controlMaestroUnidadMedidaFormulaVariable.UnidadMedidaFormulaVariableId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");

                                    controlMaestroUnidadMedidaFormulaVariable.CreadoPorId = usuarioId;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // Control Maestro Unidad Medida Dimension
                if (controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedidaDimension != null)
                {
                    foreach (MItblControlMaestroUnidadMedidaDimension controlMaestroUnidadMedidaDimension in controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedidaDimension)
                    {
                        // Verificar si el usuario ha sido modificado en mismo Dimensión y regresa mensaje de error
                        //if (controlMaestroUnidadMedidaDimension.UnidadMedidaDimensionId <= 0 && controlMaestroUnidadMedidaDimensionService.EsDimensionExiste(controlMaestroUnidadMedidaDimension))
                            //throw new Exception("La Dimensión ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");

                        if (controlMaestroUnidadMedidaDimension.UnidadMedidaDimensionId > 0)
                        {
                            // Verificar si el usuario ha sido modificado en mismo Fórmula Dimensión y regresa mensaje de error
                            MItblControlMaestroUnidadMedidaDimension _controlMaestroUnidadMedidaDimension = controlMaestroUnidadMedidaDimensionService.BuscaPorId(controlMaestroUnidadMedidaDimension.UnidadMedidaDimensionId);
                            if (!StructuralComparisons.StructuralEqualityComparer.Equals(controlMaestroUnidadMedidaDimension.Timestamp, _controlMaestroUnidadMedidaDimension.Timestamp))
                            {
                                throw new Exception("La Dimensión ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                            }

                            controlMaestroUnidadMedidaDimension.ModificadoPorId = usuarioId;
                            controlMaestroUnidadMedidaDimension.FechaUltimaModificacion = DateTime.Now;
                        }
                        else
                        {
                            controlMaestroUnidadMedidaDimension.CreadoPorId = usuarioId;
                        }
                    }
                }
                // Control Maestro Unidad Medida Formula Variable
                if (controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedidaFormulaVariable != null)
                {
                    foreach (MItblControlMaestroUnidadMedidaFormulaVariable controlMaestroUnidadMedidaFormulaVariable in controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedidaFormulaVariable)
                    {
                        // Verificar si el usuario ha sido modificado en mismo Variabla y regresa mensaje de error
                        if (controlMaestroUnidadMedidaFormulaVariableService.EsVariableExiste(controlMaestroUnidadMedidaFormulaVariable))
                            throw new Exception("La Fórmula Variable con el código [" + controlMaestroUnidadMedidaFormulaVariable.UnidadMedidaFormulaVariableId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");

                        if (controlMaestroUnidadMedidaFormulaVariable.UnidadMedidaFormulaVariableId > 0)
                        {
                            // Verificar si el usuario ha sido modificado en mismo Fórmula y regresa mensaje de error
                            MItblControlMaestroUnidadMedidaFormulaVariable _controlMaestroUnidadMedidaFormulaVariable = controlMaestroUnidadMedidaFormulaVariableService.BuscaPorId(controlMaestroUnidadMedidaFormulaVariable.UnidadMedidaFormulaVariableId);
                            if (!StructuralComparisons.StructuralEqualityComparer.Equals(controlMaestroUnidadMedidaFormulaVariable.Timestamp, _controlMaestroUnidadMedidaFormulaVariable.Timestamp))
                            {
                                throw new Exception("La Fórmula Variable ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                            }

                            controlMaestroUnidadMedidaFormulaVariable.ModificadoPorId = usuarioId;
                            controlMaestroUnidadMedidaFormulaVariable.FechaUltimaModificacion = DateTime.Now;
                        }
                        else
                        {
                            controlMaestroUnidadMedidaFormulaVariable.CreadoPorId = usuarioId;
                        }
                    }
                }
            }
            // Guardamos los cambios
            controlMaestroUnidadMedidaService.GuardaCambios(controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedida, controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedidaDimension, controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedidaFormulaVariable);

            return Json("Registros con Exito!");

        }

        // GET: MIR/UnidadMedida/Listar
        public override ActionResult Listar()
        {
            // Crear los objetos
            ControlMaestroUnidadMedidaViewModel controlMaestroUnidadMedidaViewModel = new ControlMaestroUnidadMedidaViewModel();

            GetDatosFicha(ref controlMaestroUnidadMedidaViewModel);

            return View("ListadoUnidadMedida", controlMaestroUnidadMedidaViewModel);
        }

        public override ActionResult Nuevo()
        {
            throw new NotImplementedException();
        }

        protected override void GetDatosFicha(ref ControlMaestroUnidadMedidaViewModel controlMaestroUnidadMedidaViewModel)
        {
            // Asignamos los modelos nuevos al ViewModel
            controlMaestroUnidadMedidaViewModel.ControlMaestroUnidadMedidaModel = new MItblControlMaestroUnidadMedida();
            controlMaestroUnidadMedidaViewModel.ControlMaestroUnidadMedidaDimensionModel = new MItblControlMaestroUnidadMedidaDimension();
            controlMaestroUnidadMedidaViewModel.ControlMaestroUnidadMedidaFormulaVariableModel = new MItblControlMaestroUnidadMedidaFormulaVariable();

            controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedida = new ControlMaestroUnidadMedidaService().BuscaTodos();
            controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedidaDimension = new ControlMaestroUnidadMedidaDimensionService().BuscaTodos();
            controlMaestroUnidadMedidaViewModel.ListControlMaestroUnidadMedidaFormulaVariable = new ControlMaestroUnidadMedidaFormulaVariableService().BuscaTodos();
            controlMaestroUnidadMedidaViewModel.ListControlMaestroDimension = new ControlMaestroDimensionService().BuscaTodos();
        }

        [HttpPost]
        [JsonException]
        public JsonResult EsValidarVariables(string value)
        {
            JsonVariables jsonVariables = new JsonVariables();
            jsonVariables.ListVariables = new List<string>();
            try
            {
                List<int> count = new List<int>();
                List<int> list = new List<int>();
                Expression ncalc = new Expression(value);

                ncalc.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    if (name is string)
                    {
                        args.Result = 1;
                        if(jsonVariables.ListVariables.Where(variable => variable == name).Count() == 0)
                            jsonVariables.ListVariables.Add(name);
                    }
                };
                ncalc.Evaluate();
                // El minímo 1 o mas variables
                if (jsonVariables.ListVariables.Count() == 0)
                {
                    jsonVariables.Estatus = 0;
                    jsonVariables.Mensaje = "el mínimo 1 o mas variables";
                    return Json(jsonVariables);
                }
                // Maximo 4 variables
                if (jsonVariables.ListVariables.Count() > 4)
                {
                    jsonVariables.Estatus = 0;
                    jsonVariables.Mensaje = "El máximo 4 variables";
                    return Json(jsonVariables);
                }
                // Verificar se repite alguna variable, esta la identifica como si fuera otra variable y no deberia.
                //var igualVariables = jsonVariables.ListVariables.GroupBy(x => x).Where(g => g.Count() > 1).Select(s => s.Key).ToList();
                //if (igualVariables.Count() > 0)
                //{
                //    jsonVariables.Estatus = 0;
                //    if (igualVariables.Count() == 1)
                //    {
                //        jsonVariables.Mensaje = "No se puede repetir la variable: " + igualVariables[0];
                //    }
                //    else
                //    {
                //        jsonVariables.Mensaje = "No se pueden repetir las variables: " + String.Join(", ", igualVariables);
                //    }

                //    return Json(jsonVariables);
                //}

                jsonVariables.Estatus = 1;
                jsonVariables.Mensaje = "Formula correcta";
                //DataTable tem = new DataTable();
                //tem.Compute(value, null);
                return Json(jsonVariables);
            }
            catch (Exception)
            {
                //throw new Exception("Incorrecto");
                jsonVariables.Estatus = 0;
                jsonVariables.Mensaje = "Formula incorrecta";
                return Json(jsonVariables);
            }
        }

        private class JsonVariables
        {
            public int Estatus { get; set; }
            public string Mensaje { get; set; }
            public List<string> ListVariables { get; set; }
        }
    }
}