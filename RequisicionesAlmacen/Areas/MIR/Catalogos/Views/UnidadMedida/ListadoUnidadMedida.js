// VARIABLES GLOBALES //
var modalUnidadMedida,
    modalConfirmaEliminar,
    modalConfirmaDeshacer,
    dxFormUnidadMedida,
    contadorRegistrosNuevos,
    dxDataGridListado,
    tablaVariables,
    dxButtonDeshacer,
    dxButtonGuardar,
    rowEliminar,
    API_FICHA = "/mir/catalogos/unidadmedida/";;

let listVariables = [],
    responseFormula = {};

const listadoModel = {
    UnidadMedidaId: null,
    Nombre: '',
    Dimension: [],
    Formula: '',
    ListVariables: [],
    Sistema: false,
    Borrado: false
};
//////////////////////


// Function Default //
$(() => {
    // Inicializamos las variables para la ficha
    inicializaVariables();
    // Inhabilitamos los botones de acciones
    habilitaComponentes(false);
});
//////////////////////

inicializaVariables = () => {
    contadorRegistrosNuevos = -1;
    modalUnidadMedida = $("#modalUnidadMedida");
    modalConfirmaEliminar = $('#modalConfirmaEliminar');
    modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    dxFormUnidadMedida = $("#dxFormUnidadMedida").dxForm("instance");
    dxDataGridListado = $("#dxDataGridListado").dxDataGrid("instance");
    tablaVariables = $("#tablaVariables");
    dxButtonDeshacer = $("#dxButtonDeshacer").dxButton("instance");
    dxButtonGuardar = $("#dxButtonGuardar").dxButton("instance");
    // Cargar los datos a listado
    cargarListado();
}

// Cargar //
cargarListado = () => {
    let listadoDataSource = [];

    _listControlMaestroUnidadMedida.map(um => {
        // Crear los objetos
        let crearLista = $.extend(true, {}, listadoModel);
        crearLista.UnidadMedidaId = um.UnidadMedidaId;
        crearLista.Nombre = um.Nombre;
        // Formula Dimension
        _listControlMaestroUnidadMedidaDimension.filter(und => und.UnidadMedidaId == um.UnidadMedidaId).map(und => {
            crearLista.Dimension.push(und.DimensionId);
        });
        crearLista.Formula = um.Formula;
        crearLista.Sistema = um.Sistema
        crearLista.Borrado = um.Borrado;
        // Variables
        _listControlMaestroUnidadMedidaFormulaVariable.filter(fv => fv.UnidadMedidaId == um.UnidadMedidaId).map(fv => {
            crearLista.ListVariables.push({ UnidadMedidaFormulaVariableId: fv.UnidadMedidaFormulaVariableId, Variable: fv.Variable });
        });
        // Agregar una lista a listado
        listadoDataSource.push(crearLista);
    });
    // Establecer los datos a DataGrid
    var dataSource = new DevExpress.data.DataSource({
        store: {
            type: 'array',
            key: 'UnidadMedidaId',
            data: listadoDataSource
        },
        filter: [['Borrado', '=', false]]
    });
    dxDataGridListado.option("dataSource", dataSource);
}
////////////

// MODAL //
nuevoModal = () => {

    dxFormUnidadMedida.resetValues();
    dxFormUnidadMedida.updateData("UnidadMedidaId", contadorRegistrosNuevos);

    contadorRegistrosNuevos -= 1;

    // Limpiar la tabla de variables
    limpiarVariables();

    modalUnidadMedida.find(".modal-title").text("Nuevo registro");
    modalUnidadMedida.attr("isEdit", false);
    modalUnidadMedida.modal('show');
}

editaModal = (event) => {
    //Obtenemos una copia del objeto a modificar
    var listado = $.extend(true, {}, event.row.data);
    // Reniciar o limpiar el fomrulario
    dxFormUnidadMedida.resetValues();
    //Le pasamos el objeto al Form para que cargue sus valores
    dxFormUnidadMedida.option("formData", listado);
    // Cambiamos el titulo en el modal
    modalUnidadMedida.find(".modal-title").text("Editar Formula");
    // Cambiamos el modo editar es verdad en el modal
    modalUnidadMedida.attr("isEdit", true);
    //Mostramos el modal
    modalUnidadMedida.modal('show');
}

confirmaEliminarModal = (event) => {
    // Obtenemos una copia del objeto a eliminar
    rowEliminar = $.extend(true, {}, event.row.data);
    // Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}

guardaCambiosModal = () => {
    // Validamos que la informacion requerida del Formulario este completa
    if (!dxFormUnidadMedida.validate().isValid)
        return;
    // Obtenemos el Objeto que se esta creando/editando en el Form
    var cmum = dxFormUnidadMedida.option("formData");
    // La validación
    if (!esValidacion(cmum)) {
        return;
    }

    var store = dxDataGridListado.getDataSource().store();
    //// Si el modo editar es falso (Nuevo) o si es verdad (Editar)
    if (modalUnidadMedida.attr("isEdit") == "false") {
        // Nuevo
        let crearLista = $.extend(true, {}, listadoModel);
        crearLista.UnidadMedidaId = cmum.UnidadMedidaId;
        crearLista.Nombre = cmum.Nombre;
        crearLista.Dimension = cmum.Dimension;
        crearLista.Formula = cmum.Formula;
        listVariables.map(variable => {
            crearLista.ListVariables.push({ UnidadMedidaFormulaVariableId: null, Variable: variable });
        });

        store.insert(crearLista)
            .done(function () {
                dxDataGridListado.getDataSource().reload();
                // Habilitamos los botones de acciones
                habilitaComponentes(true);
                // Ocultamos el modal
                modalUnidadMedida.modal('hide');
                // Ir a navigate por Row Nuevo en el DataGrid
                dxDataGridListado.navigateToRow(crearLista.FormulaId);
            })
            .fail(function () {
                toast("No se pudo agregar el nuevo registro a la tabla.", "error");
            });
    } else {
        //Editar
        let agregarListVariables = [];
        listVariables.map(variable => {
            const formulaVariable = cmum.ListVariables.find(fv => fv.Variable == variable);
            if (formulaVariable)
                agregarListVariables.push({ UnidadMedidaFormulaVariableId: formulaVariable.UnidadMedidaFormulaVariableId, Variable: variable });
            else
                agregarListVariables.push({ UnidadMedidaFormulaVariableId: null, Variable: variable });
        });
        cmum.ListVariables = agregarListVariables;

        store.update(cmum.UnidadMedidaId, cmum)
            .done(function () {
                // Recargamos la informacion de la tabla
                dxDataGridListado.getDataSource().reload();
                // Habilitamos los botones de acciones
                habilitaComponentes(true);
                // Ocultamos el modal
                modalUnidadMedida.modal('hide');
            })
            .fail(function () {
                toast("No se pudo actualizar el registro en la tabla.", "error");
            });
    }
}

///////////

eliminaRow = () => {
    // Ocultamos el modal confirma eliminar
    modalConfirmaEliminar.modal('hide');
    //Obtenemos la instancia del DataSource
    var dataGridListado = dxDataGridListado.getDataSource();
    //Eliminamos el registro de la tabla
    if (rowEliminar != null) {
        // Si el registro viene de la base de datos
        // para posteriormente eliminarlo en la base de datos
        if (rowEliminar.UnidadMedidaId > 0) {
            // Actualizamos el borrado del registro a "true"
            rowEliminar.Borrado = true;

            dataGridListado.store().update(rowEliminar.UnidadMedidaId, rowEliminar)
                .done(function () {
                    //Recargamos la informacion de la tabla
                    dataGridListado.reload();

                    // Habilitamos los botones de acciones
                    habilitaComponentes(true);

                })
                .fail(function () {
                    toast("No se pudo actualizar el artículo en la tabla.", "error");
                });
        } else {
            dataGridListado.store().remove(rowEliminar.UnidadMedidaId)
                .done(function () {
                    // Recargamos la informacion de la tabla
                    dataGridListado.reload();
                    // Habilitamos los botones de acciones
                    habilitaComponentes(true);
                })
                .fail(function () {
                    toast("No se pudo eliminar el artículo de la tabla.", "error");
                });
        }
        // Limpiar Row Eliminar
        rowEliminar = null;
    }

}

// Guardar //
guardaCambios = () => {
    // Mostramos Loader
    dxLoaderPanel.show();
    // Obtener los datos para guardar
    const data = obtenerData();
    // Verificar hay nuevas la filas para guardar o no
    if (data.listControlMaestroUnidadMedida.length == 0 && data.listControlMaestroUnidadMedidaDimension.length == 0 && data.listControlMaestroUnidadMedidaFormulaVariable.length == 0) {
        toast("No se puede guardar ya que no hubo cambios en la información", "error");
        // Ocultamos Loader
        dxLoaderPanel.hide();
        return;
    }

    $.ajax({
        type: "POST",
        url: API_FICHA + "guardar",
        data: data,
        success: function (response) {
            //Mostramos mensaje de Exito
            toast(response, 'success');
            // Actualizar la pagina
            window.location.href = API_FICHA + 'listar';
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();
            //Mostramos mensaje de error
            toast(response.responseText, "error");
        }
    });
}

obtenerData = () => {
    // Crear los objetos
    let data = {
        listControlMaestroUnidadMedida: [],
        listControlMaestroUnidadMedidaDimension: [],
        listControlMaestroUnidadMedidaFormulaVariable: []
    },
        listado = [];
    // Obtenemos todos los registros que hay en el DataGrid
    dxDataGridListado.getDataSource().store().load().done(response => listado = response);

    listado.map(l => {
        // Verificar hay los datos de Control Maestro Unidad Medida
        const obtenerUnidadMedida = _listControlMaestroUnidadMedida.find(um => um.UnidadMedidaId == l.UnidadMedidaId);
        if (obtenerUnidadMedida) {
            // Control Maestro Unidad Medida
            if (l.Nombre != obtenerUnidadMedida.Nombre || l.Formula != obtenerUnidadMedida.Formula || l.Borrado != obtenerUnidadMedida.Borrado) {
                let actualizarFormula = $.extend(true, {}, obtenerUnidadMedida);
                actualizarFormula.Nombre = l.Nombre;
                actualizarFormula.Formula = l.Formula;
                actualizarFormula.Sistema = l.Sistema;
                actualizarFormula.Borrado = l.Borrado;
                actualizarFormula.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(actualizarFormula.Timestamp)));
                data.listControlMaestroUnidadMedida.push(actualizarFormula)
            }
            // Control Maestro Unidad Medida Dimension
            if (!esIgualDimension(l) || l.Borrado == true) {
                const obtenerUnidadMedidaDimension = _listControlMaestroUnidadMedidaDimension.filter(umd => umd.UnidadMedidaId == obtenerUnidadMedida.UnidadMedidaId);
                if (obtenerUnidadMedidaDimension) {
                    obtenerUnidadMedidaDimension.map(fd => {
                        if (!l.Dimension.some(dimensionId => dimensionId == fd.DimensionId) || l.Borrado == true) {
                            let actualizarFormulaDimension = $.extend(true, {}, fd);
                            actualizarFormulaDimension.Borrado = true;
                            actualizarFormulaDimension.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(actualizarFormulaDimension.Timestamp)));
                            data.listControlMaestroUnidadMedidaDimension.push(actualizarFormulaDimension);
                        }
                    });
                }
                l.Dimension.map(dimensionId => {
                    if (!_listControlMaestroUnidadMedidaDimension.some(dimension => dimension.DimensionId == dimensionId && dimension.UnidadMedidaId == l.UnidadMedidaId)) {
                        let crearFormulaDimension = $.extend(true, {}, _controlMaestroUnidadMedidaDimensionModel);
                        crearFormulaDimension.UnidadMedidaDimensionId = contadorRegistrosNuevos;
                        crearFormulaDimension.UnidadMedidaId = l.UnidadMedidaId;
                        crearFormulaDimension.DimensionId = dimensionId;
                        crearFormulaDimension.Borrado = l.Borrado;
                        data.listControlMaestroUnidadMedidaDimension.push(crearFormulaDimension);
                        contadorRegistrosNuevos--;
                    }
                });
            }
            // Control Maestro Unidad Medida Formula Variable
            if (!esIgualVariable(l) || l.Borrado == true) {
                const obtenerFormulaVariable = _listControlMaestroUnidadMedidaFormulaVariable.filter(fv => fv.UnidadMedidaId == obtenerUnidadMedida.UnidadMedidaId);
                if (obtenerFormulaVariable) {
                    obtenerFormulaVariable.map(fv => {
                        if (!l.ListVariables.some(formulaVariable => formulaVariable.UnidadMedidaFormulaVariableId == fv.UnidadMedidaFormulaVariableId && formulaVariable.Variable == fv.Variable) || l.Borrado == true) {
                            let actualizarFormulaVariable = $.extend(true, {}, fv);
                            actualizarFormulaVariable.Borrado = true;
                            actualizarFormulaVariable.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(actualizarFormulaVariable.Timestamp)));
                            data.listControlMaestroUnidadMedidaFormulaVariable.push(actualizarFormulaVariable);
                        }
                    });
                }
                l.ListVariables.map(formulaVariable => {
                    if (!_listControlMaestroUnidadMedidaFormulaVariable.some(_formulaVariable => _formulaVariable.UnidadMedidaFormulaVariableId == formulaVariable.UnidadMedidaFormulaVariableId && _formulaVariable.Variable == formulaVariable.Variable && _formulaVariable.UnidadMedidaId == l.UnidadMedidaId)) {
                        let crearFormulaVariable = $.extend(true, {}, _controlMaestroUnidadMedidaFormulaVariableModel);
                        crearFormulaVariable.UnidadMedidaFormulaVariableId = contadorRegistrosNuevos;
                        crearFormulaVariable.UnidadMedidaId = l.UnidadMedidaId;
                        crearFormulaVariable.Variable = formulaVariable.Variable;
                        crearFormulaVariable.Borrado = l.Borrado;
                        data.listControlMaestroUnidadMedidaFormulaVariable.push(crearFormulaVariable);
                        contadorRegistrosNuevos--;
                    }
                });
            }
        } else {
            // Control Maestro Unidad Medida
            let crearUnidadMedida = $.extend(true, {}, _controlMaestroUnidadMedidaModel);
            crearUnidadMedida.UnidadMedidaId = l.UnidadMedidaId;
            crearUnidadMedida.Nombre = l.Nombre;
            crearUnidadMedida.Formula = l.Formula;
            crearUnidadMedida.Sistema = l.Sistema;
            crearUnidadMedida.Borrado = l.Borrado;
            data.listControlMaestroUnidadMedida.push(crearUnidadMedida);
            // Control Maestro Unidad Medida Dimension
            l.Dimension.map(dimensionId => {
                let crearUnidadMedidaDimension = $.extend(true, {}, _controlMaestroUnidadMedidaDimensionModel);
                crearUnidadMedidaDimension.UnidadMedidaDimensionId = contadorRegistrosNuevos;
                crearUnidadMedidaDimension.UnidadMedidaId = l.UnidadMedidaId;
                crearUnidadMedidaDimension.DimensionId = dimensionId;
                crearUnidadMedidaDimension.Borrado = false;
                data.listControlMaestroUnidadMedidaDimension.push(crearUnidadMedidaDimension);
                contadorRegistrosNuevos--;
            });
            // Control Maestro Unidad Medida Formula Variable
            l.ListVariables.map(fv => {
                let crearFormulaVariable = $.extend(true, {}, _controlMaestroUnidadMedidaFormulaVariableModel);
                crearFormulaVariable.UnidadMedidaFormulaVariableId = contadorRegistrosNuevos;
                crearFormulaVariable.UnidadMedidaId = l.UnidadMedidaId;
                crearFormulaVariable.Variable = fv.Variable;
                crearFormulaVariable.Borrado = l.Borrado;
                data.listControlMaestroUnidadMedidaFormulaVariable.push(crearFormulaVariable);
                contadorRegistrosNuevos--;

            });
        }
    });

    return data;
}
/////////////

esIgualVariable = (unidadMedida) => {
    let _listVariables = [{ UnidadMedidaFormulaVariableId: null, Variable: '' }];
    _listControlMaestroUnidadMedidaFormulaVariable.filter(fv => fv.UnidadMedidaId == unidadMedida.UnidadMedidaId && fv.Borrado == false).map(fv => _listVariables.push({ UnidadMedidaFormulaVariableId: fv.UnidadMedidaFormulaVariableId, Variable: fv.Variable }));
    if (JSON.stringify(unidadMedida.ListVariables) === JSON.stringify(_listVariables)) {
        return true;
    } else {
        return false;
    }
}

esIgualDimension = (unidadMedida) => {
    let dimension = [];
    _listControlMaestroUnidadMedidaDimension.filter(fd => fd.UnidadMedidaId == unidadMedida.UnidadMedidaId && fd.Borrado == false).map(fd => dimension.push(fd.DimensionId));
    if (JSON.stringify(unidadMedida.Dimension) === JSON.stringify(dimension)) {
        return true;
    } else {
        return false;
    }
}

// Las validaciones //
esValidacion = (unidadMedida) => {
    var listUnidadMedida = [];
    //Obtenemos todos los registros que hay en el Data Grid
    dxDataGridListado.getDataSource().store().load().done(response => listUnidadMedida = response.filter(filter => filter.Borrado == false));
    // No se puede repetir el campo Nombre
    if (listUnidadMedida.some(_unidadMedida => _unidadMedida.FormulaId != unidadMedida.FormulaId && _unidadMedida.Nombre.toUpperCase() == unidadMedida.Nombre.toUpperCase() && _unidadMedida.Borrado == false)) {
        var dxTextBoxNombre = dxFormUnidadMedida.getEditor('Nombre');
        dxTextBoxNombre.option({
            validationStatus: 'invalid',
            validationError: {
                type: 'custom',
                message: 'El nombre ya esta registrado, intentes otra.'
            }
        });
        return false;
    }
    // No se puede repetir el nombre de la formula
    if (listUnidadMedida.some(_unidadMedida => _unidadMedida.FormulaId != unidadMedida.FormulaId && _unidadMedida.Formula == unidadMedida.Formula)) {
        var dxTextBoxFormula = dxFormUnidadMedida.getEditor('Formula');
        dxTextBoxFormula.option({
            validationStatus: 'invalid',
            validationError: {
                type: 'custom',
                message: 'La formula ya esta registrado, intentes otra.'
            }
        });
        return false;
    }
    // Verificar el campo Formula validar (desde el campo Formula hacer api para validar)
    if (!!responseFormula.Estatus == 0) {
        var dxTextBoxFormula = dxFormUnidadMedida.getEditor("Formula");
        dxTextBoxFormula.option({
            validationStatus: 'invalid',
            validationError: {
                type: "custom", message: responseFormula.Mensaje
            }
        });
        return false;
    }

    return true;
}
//////////////////////

cellTemplate = (container, options) => {
    var noBreakSpace = "\u00A0",
        text = (options.value || []).map(element => {
            return options.column.lookup.calculateCellValue(element);
        }).join(", ");
    container.text(text || noBreakSpace).attr("title", text);
}

asyncTreeViewSeleccion = (treeView, value) => {
    if (!value) {
        treeView.unselectAll();
        return;
    }

    value.forEach(function (key) {
        treeView.selectItem(key);
    });
}

// Listado //
calculateCellValueUnidadMedidaId = (event) => {
    if (event.UnidadMedidaId > 0) {
        return event.UnidadMedidaId;
    }
    return 0;
}

calculateCellValueDimension = (event) => {
    return event.Dimension.map(dimension => { return _listControlMaestroDimension.find(cmd => cmd.DimensionId == dimension).Descripcion; }).join(", ");
}

calculateCellValueVariables = (event) => {
    return event.ListVariables.length;
}

habilitaAccionSistema = (event) => {
    return !event.row.data.Sistema;
}
/////////////

// Otro //
regresarListado = () => {
    window.location.href = API_FICHA + "listar";
}

recargarFicha = () => {
    window.location.href = API_FICHA + 'listar';
}

habilitaComponentes = (enabled) => {
    dxButtonDeshacer.option("disabled", !enabled);
    dxButtonGuardar.option("disabled", !enabled);
}

limpiarVariables = () => {
    // Limpiar la tabla de variables
    tablaVariables.find("tbody").empty().append("<tr><th>Ingresar Definición Fórmula</th></tr>");
}

validaHayCambios = () => {
    // Obtener los datos para guardar
    const data = obtenerData();
    // Verificar hay nuevas la filas para guardar o no
    if (data.listControlMaestroUnidadMedida.length == 0 && data.listControlMaestroUnidadMedidaDimension.length == 0 && data.listControlMaestroUnidadMedidaFormulaVariable.length == 0) {
        regresarListado();
    } else {
        modalConfirmaDeshacer.modal('show')
    }
}
//////////

// OnValueChanged //
onValueChangedFormula = (params) => {
    if (params.value) {
        // Mostramos Loader
        dxLoaderPanel.show();

        responseFormula = {};
        // Enviar a controller para validar las variables
        $.ajax({
            type: "POST",
            url: API_FICHA + "esvalidarvariables",
            data: { value: params.value },
            success: function (response) {

                // Ocultamos Loader
                dxLoaderPanel.hide();

                responseFormula = response;

                if (response.Estatus == 0) {
                    tablaVariables.find("tbody").empty();
                    var dxTextBoxFormula = dxFormUnidadMedida.getEditor("Formula");
                    dxTextBoxFormula.option({
                        validationStatus: 'invalid',
                        validationError: {
                            type: "custom", message: response.Mensaje
                        }
                    });
                } else {
                    listVariables = [];
                    tablaVariables.find("tbody").empty();
                    response.ListVariables.map((variable, index) => {
                        tablaVariables.find("tbody").append("<tr><th>Variable " + (index + 1) + "</th><td>" + variable + "</td></tr>");
                        // agregar la variable a lista de variables
                        listVariables.push(variable);
                    });
                }
            },
            error: function (response, status, error) {
                // Ocultamos Loader
                dxLoaderPanel.hide();
                //Mostramos mensaje de error
                toast(response.responseText, "error");
            }
        });
    }
}

onValueChangedDropDownBox = (e) => {
    if (e.component.content()) {
        var $treeView = e.component.content().find(".dx-treeview");
        if ($treeView.length) {
            asyncTreeViewSeleccion($treeView.dxTreeView("instance"), e.value);
        }
    }
}
////////////////////

// Toast //
toast = (message, type) => {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 3500);
}
///////////