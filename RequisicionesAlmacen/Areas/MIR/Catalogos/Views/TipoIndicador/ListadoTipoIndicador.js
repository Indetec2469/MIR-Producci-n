// VARIABLES GLOBALES //
var contadorRegistrosNuevos,
    dxDataGridListado,
    dxButtonDeshacer,
    dxButtonGuardar,
    modalConfirmaEliminar,
    modalConfirmaDeshacer,
    rowEliminar;

const listadoModel = {
    TipoIndicadorId: null,
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
    cargaListado();
}

// Carga //
cargaListado = () => {
    let listadoDataSource = [];
    _listControlMaestroTipoIndicador.map(ti => {
        // Crear los objetos
        let listaCrear = $.extend(true, {}, listadoModel);
        listaCrear.TipoIndicadorId = ti.TipoIndicadorId;
        listaCrear.Descripcion = ti.Descripcion;
        // Tipo Indicador Nivel
        _listControlMaestroTipoIndicadorNivel.filter(tin => tin.TipoIndicadorId == ti.TipoIndicadorId).map(tin => {
            listaCrear.Nivel.push(tin.NivelId);
        });
        listaCrear.Borrado = ti.Borrado;
        // Agregar una lista a listado
        listadoDataSource.push(listaCrear);
    });
    // Establecer los datos a DataGrid
    var dataSource = new DevExpress.data.DataSource({
        store: {
            type: 'array',
            key: 'TipoIndicadorId',
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
    event.data.TipoIndicadorId = contadorRegistrosNuevos;
    event.data.Borrado = false;
    contadorRegistrosNuevos--;
}

onEditorPreparingListado = (event) => {
    if (event.parentType == 'dataRow' && event.dataField == 'Descripcion') {
        seleccionFilaIndex = event.row.rowIndex;
        event.editorOptions.onFocusOut = (args) => {
            let listControlMaestroTipoIndicador = [];
            dxDataGridListado.getDataSource().store().load().done(response => listControlMaestroTipoIndicador = response);
            if (listControlMaestroTipoIndicador.length > 0) {
                if (listControlMaestroTipoIndicador.some(ti => (ti.Descripcion ? ti.Descripcion.toUpperCase() == event.row.data.Descripcion.toUpperCase() : null) && ti.TipoIndicadorId != event.row.data.TipoIndicadorId && ti.Borrado == false)) {
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
onValueChangedTagBoxListado = (event) => {
    // Verificar existe los arreglos de nivel entonces cambiamos el valor a true y sino a false
    //if (event.value.length > 0) {
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

calculateCellValueTipoIndicadorId = (e) => {
    if (e.TipoIndicadorId > 0) {
        return e.TipoIndicadorId;
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
    if (_listado.listControlMaestroTipoIndicador.length == 0 && _listado.listControlMaestroTipoIndicadorNivel.length == 0) {
        toast("No se puede guardar ya que no hubo cambios en la información", "error");
        // Ocultamos Loader
        dxLoaderPanel.hide();
        return;
    }

    $.ajax({
        type: "POST",
        url: "/mir/catalogos/tipoindicador/guardar",
        data: _listado,
        //contentType: "application/json; charset=utf-8",
        //data: JSON.stringify(data),
        //dataType: 'json',
        success: function (response) {
            //Mostramos mensaje de Exito
            toast(response, 'success');

            // Actualizar la pagina
            window.location.href = '/mir/catalogos/tipoindicador/listar';
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
            if (listado.some(_lis => _lis.Descripcion == listado[i].Descripcion && _lis.TipoIndicadorId != listado[i].TipoIndicadorId && _lis.Borrado == false && listado[i].Borrado == false)) {
                esValidar = false;
                toast("El campo indicador no se puede repetir: " + listado[i].Descripcion, "error");
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
        listControlMaestroTipoIndicador: [],
        listControlMaestroTipoIndicadorNivel: []
    };

    listado.map(l => {
        // Verificar hay los datos de control maestro tipo indicador
        const obtenerControlMaestroTipoIndicador = _listControlMaestroTipoIndicador.find(ti => ti.TipoIndicadorId == l.TipoIndicadorId);
        if (obtenerControlMaestroTipoIndicador) {
            // Control Maestro Tipo Indicador
            if (l.Descripcion != obtenerControlMaestroTipoIndicador.Descripcion || l.Borrado != obtenerControlMaestroTipoIndicador.Borrado) {
                let controlMaestroTipoIndicadorCrear = $.extend(true, {}, obtenerControlMaestroTipoIndicador);
                controlMaestroTipoIndicadorCrear.Descripcion = l.Descripcion;
                controlMaestroTipoIndicadorCrear.Borrado = l.Borrado;
                // Converitr byte a char en Timestamp para enviar sin problema
                controlMaestroTipoIndicadorCrear.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(controlMaestroTipoIndicadorCrear.Timestamp)));
                _listado.listControlMaestroTipoIndicador.push(controlMaestroTipoIndicadorCrear);
            }

            // Control Maestro Tipo Indicador Nivel
            if (l.Nivel != obtenerControlMaestroTipoIndicador.Nivel || l.Borrado == true) {
                const obtenerControlMaestroTipoIndicadorNivel = _listControlMaestroTipoIndicadorNivel.filter(_n => _n.TipoIndicadorId == obtenerControlMaestroTipoIndicador.TipoIndicadorId);
                if (obtenerControlMaestroTipoIndicadorNivel) {
                    obtenerControlMaestroTipoIndicadorNivel.map(niv => {
                        if (!l.Nivel.some(_niv => _niv == niv.NivelId) || l.Borrado == true) {
                            let controlMaestroTipoIndicadorNivelActualizar = $.extend(true, {}, niv);
                            controlMaestroTipoIndicadorNivelActualizar.Borrado = true;
                            controlMaestroTipoIndicadorNivelActualizar.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(controlMaestroTipoIndicadorNivelActualizar.Timestamp)));
                            _listado.listControlMaestroTipoIndicadorNivel.push(controlMaestroTipoIndicadorNivelActualizar);
                        }
                    });
                }
                l.Nivel.map(niv => {
                    if (!_listControlMaestroTipoIndicadorNivel.some(_niv => _niv.NivelId == niv && _niv.TipoIndicadorId == l.TipoIndicadorId)) {
                        let controlMaestroTipoIndicadorNivelCrear = $.extend(true, {}, _controlMaestroTipoIndicadorNivel);
                        controlMaestroTipoIndicadorNivelCrear.TipoIndicadorNivelId = contadorRegistrosNuevos;
                        controlMaestroTipoIndicadorNivelCrear.TipoIndicadorId = l.TipoIndicadorId;
                        controlMaestroTipoIndicadorNivelCrear.NivelId = niv;
                        controlMaestroTipoIndicadorNivelCrear.Borrado = l.Borrado;
                        controlMaestroTipoIndicadorNivelCrear.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(controlMaestroTipoIndicadorNivelCrear.Timestamp)));
                        _listado.listControlMaestroTipoIndicadorNivel.push(controlMaestroTipoIndicadorNivelCrear);
                        contadorRegistrosNuevos--;
                    }
                });
            }
        } else {
            // Control Maestro Tipo Indicador
            let controlMaestroTipoIndicadorCrear = $.extend(true, {}, _controlMaestroTipoIndicador);
            controlMaestroTipoIndicadorCrear.TipoIndicadorId = l.TipoIndicadorId;
            controlMaestroTipoIndicadorCrear.Descripcion = l.Descripcion;
            controlMaestroTipoIndicadorCrear.Borrado = l.Borrado;
            _listado.listControlMaestroTipoIndicador.push(controlMaestroTipoIndicadorCrear);

            // Control Maestro Tipo Indicador Nivel
            l.Nivel.map(niv => {
                let controlMaestroTipoIndicadorNivelCrear = $.extend(true, {}, _controlMaestroTipoIndicadorNivel);
                controlMaestroTipoIndicadorNivelCrear.TipoIndicadorNivelId = contadorRegistrosNuevos;
                controlMaestroTipoIndicadorNivelCrear.TipoIndicadorId = l.TipoIndicadorId;
                controlMaestroTipoIndicadorNivelCrear.NivelId = niv;
                controlMaestroTipoIndicadorNivelCrear.Borrado = false;
                _listado.listControlMaestroTipoIndicadorNivel.push(controlMaestroTipoIndicadorNivelCrear);
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
        if (rowEliminar.TipoIndicadorId > 0) {
            // Actualizamos el borrado del registro a "true"
            rowEliminar.Borrado = true;

            dataGridListado.store().update(rowEliminar.TipoIndicadorId, rowEliminar)
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
            dataGridListado.store().remove(rowEliminar.TipoIndicadorId)
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
    window.location.href = '/mir/catalogos/tipoindicador/listar';
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