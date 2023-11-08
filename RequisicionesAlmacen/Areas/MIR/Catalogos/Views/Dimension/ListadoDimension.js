// VARIABLES GLOBALES //
var contadorRegistrosNuevos,
    dxDataGridListado,
    dxButtonDeshacer,
    dxButtonGuardar,
    modalConfirmaEliminar,
    modalConfirmaDeshacer,
    rowEliminar;

const listadoModel = {
    DimensionId: null,
    Descripcion: '',
    Nivel: [],
    Borrado: false
};

let seleccionFilaIndex = 0;
//////////////////////

// Function Default //
$(() => {
    // Inicializamos las variables para la ficha
    inicializaVariables();

    // Inhabilitamos los botones de acciones
    //habilitaComponentes(false);

});
//////////////////////

inicializaVariables = () => {
    //contadorRegistrosNuevos = -1;
    dxDataGridListado = $("#dxDataGridListado").dxDataGrid("instance");
    //modalConfirmaEliminar = $('#modalConfirmaEliminar');
    //modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    //dxButtonDeshacer = $("#dxButtonDeshacer").dxButton("instance");
    //dxButtonGuardar = $("#dxButtonGuardar").dxButton("instance");

    // Cargar los datos a listado
    cargarListado();
}

// Cargar //
cargarListado = () => {
    let listadoDataSource = [];
    _listControlMaestroDimension.map(d => {
        // Crear los objetos
        let listaCrear = $.extend(true, {}, listadoModel);
        listaCrear.DimensionId = d.DimensionId;
        listaCrear.Descripcion = d.Descripcion;
        // Dimension Nivel
        _listControlMaestroDimensionNivel.filter(dn => dn.DimensionId == d.DimensionId).map(dn => {
            listaCrear.Nivel.push(dn.NivelId);
        });
        listaCrear.Borrado = d.Borrado;
        // Agregar una lista a listado
        listadoDataSource.push(listaCrear);
    });
    // Establecer los datos a DataGrid
    var dataSource = new DevExpress.data.DataSource({
        store: {
            type: 'array',
            key: 'DimensionId',
            data: listadoDataSource
        },
        filter: [['Borrado', '=', false]]
    });
    dxDataGridListado.option("dataSource", dataSource);
}
////////////

// DataGrid //
onInitNewRowListado = (event) => {
    // Asignamos el modelo de listado
    event.data = $.extend(true, {}, listadoModel);
    event.data.DimensionId = contadorRegistrosNuevos;
    event.data.Borrado = false;
    contadorRegistrosNuevos--;
}

onEditorPreparingListado = (event) => {
    if (event.parentType == 'dataRow' && event.dataField == 'Descripcion') {
        seleccionFilaIndex = event.row.rowIndex;
        event.editorOptions.onFocusOut = (args) => {
            let listControlMaestroDimension = [];
            dxDataGridListado.getDataSource().store().load().done(response => listControlMaestroDimension = response);
            if (listControlMaestroDimension.length > 0) {
                if (listControlMaestroDimension.some(d => (d.Descripcion ? d.Descripcion.toUpperCase() == event.row.data.Descripcion.toUpperCase() : null) && d.DimensionId != event.row.data.DimensionId && d.Borrado == false)) {
                    // Mostramos toast
                    toast("No se puede repetir: " + event.row.data.Descripcion, "error");
                    // Cambiamos el valor a ''
                    event.editorElement.dxTextBox('instance').option('value', '');
                    // Enfocado al celda
                    enfocadoCelda("Descripcion");
                }
            }
        }
    }
}

onRowInsertedListado = (event) => {
    event.component.navigateToRow(event.key);
    // Habilitamos los botones de acciones
    habilitaComponentes(true);
}

onRowUpdateListado = (event) => {
    // Habilitamos los botones de acciones
    habilitaComponentes(true);
}

// Asignamos el valor a index fila que viene event
onFocusedCellChangedListado = (event) => {
    seleccionFilaIndex = event.rowIndex;
}

// TagBox //
onValueChangedTagBoxListado = (params) => {
    //// Verificar existe los arreglos de nivel entonces cambiamos el valor a true y sino a false
    //if (params.value.length > 0) {
    //    const fila = dxDataGridListado.getVisibleRows()[seleccionFilaIndex].data;
    //    // Descripcion
    //    if (fila.Descripcion != '') {
    //        esSiguienteFila = true;
    //    } else {
    //        esSiguienteFila = false;
    //    }
    //} else {
    //    esSiguienteFila = false;
    //}
}

cellTemplate = (container, options) => {
    var noBreakSpace = "\u00A0",
        text = (options.value || []).map(element => {
            return options.column.lookup.calculateCellValue(element);
        }).join(", ");
    container.text(text || noBreakSpace).attr("title", text);
}

calculateCellValueDimensionId = (e) => {
    if (e.DimensionId > 0) {
        return e.DimensionId;
    }
    return 0;
}
////////////
//////////////////////


// MODAL //
confirmaEliminarModal = (event) => {
    // Obtenemos una copia del objeto a eliminar
    rowEliminar = $.extend(true, {}, event.row.data);

    // Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}
///////////

// Guardar //
guardaCambios = () => {
    // Mostramos Loader
    dxLoaderPanel.show();

    // Crear los objectos
    let listado = [];

    // Obtenemos todos los registros que hay en el DataGrid
    dxDataGridListado.getDataSource().store().load().done(response => listado = response);

    // Validaciones
    if (!esValidacion(listado)) {
        // Ocultamos Loader
        dxLoaderPanel.hide();
        return;
    }

    // Obtener los datos para guardar
    const _listado = obtenerListado(listado);
    // Verificar hay nuevas la filas para guardar o no
    if (_listado.listControlMaestroDimension.length == 0 && _listado.listControlMaestroDimensionNivel.length == 0) {
        toast("No se puede guardar ya que no hubo cambios en la información", "error");
        // Ocultamos Loader
        dxLoaderPanel.hide();
        return;
    }


    $.ajax({
        type: "POST",
        url: "/mir/catalogos/dimension/guardar",
        data: _listado,
        success: function (response) {
            //Mostramos mensaje de Exito
            toast(response, 'success');

            // Actualizar la pagina
            window.location.href = '/mir/catalogos/dimension/listar';
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast(response.responseText, "error");
        }
    });
}

// Las Validaciones
esValidacion = (listado) => {
    if (listado.length > 0) {

        // Validacion 1: Si los datos estan vacio no se puede guardar
        if (listado.some(_list => _list.Descripcion == '' || _list.Nivel.length == 0)) {
            toast("Alguno de los datos estan vacio no se puede guardar", "error");
            return false;
        }

        let esValidar = true;
        // Validacion 2: Si los datos existe los dos mismos de descripcion no se puede repetir
        for (let i = 0; i < listado.length; i++) {
            if (listado.some(_lis => _lis.Descripcion == listado[i].Descripcion && _lis.DimensionId != listado[i].DimensionId && _lis.Borrado == false && listado[i].Borrado == false)) {
                esValidar = false;
                toast("El campo dimensión no se puede repetir: " + listado[i].Descripcion, "error");
                break;
            }
        }
        return esValidar;
    } else {
        toast("No se pudo guardar porque no hay dato o no se ha modificado", "error");
        return false;
    }
}

obtenerListado = (listado) => {
    let _listado = {
        listControlMaestroDimension: [],
        listControlMaestroDimensionNivel: []
    };

    listado.map(l => {
        // Verificar hay los datos de control maestro dimension
        const obtenerControlMaestroDimension = _listControlMaestroDimension.find(d => d.DimensionId == l.DimensionId);
        if (obtenerControlMaestroDimension) {
            // Control Maestro Dimension
            if (l.Descripcion != obtenerControlMaestroDimension.Descripcion || l.Borrado != obtenerControlMaestroDimension.Borrado) {
                let controlMaestroDimensionCrear = $.extend(true, {}, obtenerControlMaestroDimension);
                controlMaestroDimensionCrear.Descripcion = l.Descripcion;
                controlMaestroDimensionCrear.Borrado = l.Borrado;
                controlMaestroDimensionCrear.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(controlMaestroDimensionCrear.Timestamp)));
                _listado.listControlMaestroDimension.push(controlMaestroDimensionCrear)
            }

            // Control Maestro Dimension Nivel
            if (l.Nivel != obtenerControlMaestroDimension.Nivel || l.Borrado == true) {
                const obtenerControlMaestroDimensionNivel = _listControlMaestroDimensionNivel.filter(_n => _n.DimensionId == obtenerControlMaestroDimension.DimensionId);
                if (obtenerControlMaestroDimensionNivel) {
                    obtenerControlMaestroDimensionNivel.map(niv => {
                        if (!l.Nivel.some(_niv => _niv == niv.NivelId) || l.Borrado == true) {
                            let controlMaestroDimensionNivelActualizar = $.extend(true, {}, niv);
                            controlMaestroDimensionNivelActualizar.Borrado = true;
                            controlMaestroDimensionNivelActualizar.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(controlMaestroDimensionNivelActualizar.Timestamp)));
                            _listado.listControlMaestroDimensionNivel.push(controlMaestroDimensionNivelActualizar);
                        }
                    });
                }
                l.Nivel.map(niv => {
                    if (!_listControlMaestroDimensionNivel.some(_niv => _niv.NivelId == niv && _niv.DimensionId == l.DimensionId)) {
                        let controlMaestroDimensionNivelCrear = $.extend(true, {}, _controlMaestroDimensionNivel);
                        controlMaestroDimensionNivelCrear.DimensionNivelId = contadorRegistrosNuevos;
                        controlMaestroDimensionNivelCrear.DimensionId = l.DimensionId;
                        controlMaestroDimensionNivelCrear.NivelId = niv;
                        controlMaestroDimensionNivelCrear.Borrado = l.Borrado;
                        controlMaestroDimensionNivelCrear.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(controlMaestroDimensionNivelCrear.Timestamp)));
                        _listado.listControlMaestroDimensionNivel.push(controlMaestroDimensionNivelCrear);
                        contadorRegistrosNuevos--;
                    }
                });
            }
        } else {
            // Control Maestro Dimension
            let controlMaestroDimensionCrear = $.extend(true, {}, _controlMaestroDimension);
            controlMaestroDimensionCrear.DimensionId = l.DimensionId;
            controlMaestroDimensionCrear.Descripcion = l.Descripcion;
            controlMaestroDimensionCrear.Borrado = l.Borrado;
            _listado.listControlMaestroDimension.push(controlMaestroDimensionCrear);

            // Control Maestro Dimension Nivel
            l.Nivel.map(niv => {
                let controlMaestroDimensionNivelCrear = $.extend(true, {}, _controlMaestroDimensionNivel);
                controlMaestroDimensionNivelCrear.DimensionNivelId = contadorRegistrosNuevos;
                controlMaestroDimensionNivelCrear.DimensionId = l.DimensionId;
                controlMaestroDimensionNivelCrear.NivelId = niv;
                controlMaestroDimensionNivelCrear.Borrado = false;
                _listado.listControlMaestroDimensionNivel.push(controlMaestroDimensionNivelCrear);
                contadorRegistrosNuevos--;
            });
        }
    });

    return _listado;
}
/////////////

// OnClick //
onClickAgregarNuevaFila = () => {
    dxDataGridListado.addRow();
}

eliminaRow = () => {
    // Ocultamos el modal confirma eliminar
    modalConfirmaEliminar.modal('hide');

    //Obtenemos la instancia del DataSource
    var dataGridListado = dxDataGridListado.getDataSource();

    //Eliminamos el registro de la tabla
    if (rowEliminar != null) {
        // Si el registro viene de la base de datos
        // para posteriormente eliminarlo en la base de datos
        if (rowEliminar.DimensionId > 0) {
            // Actualizamos el borrado del registro a "true"
            rowEliminar.Borrado = true;

            dataGridListado.store().update(rowEliminar.DimensionId, rowEliminar)
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
            dataGridListado.store().remove(rowEliminar.DimensionId)
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

recargarFicha = () => {
    modalConfirmaDeshacer.modal('hide');
    window.location.href = '/mir/catalogos/dimension/listar';
}
/////////////

// Otro //
habilitaComponentes = (enabled) => {
    dxButtonDeshacer.option("disabled", !enabled);
    dxButtonGuardar.option("disabled", !enabled);
}

enfocadoCelda = (campo) => {
    setTimeout(() => {
        dxDataGridListado.editCell(seleccionFilaIndex, campo);
    }, 0);
}
//////////

// Toast //
toast = (message, type) => {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 3000);
}
///////////