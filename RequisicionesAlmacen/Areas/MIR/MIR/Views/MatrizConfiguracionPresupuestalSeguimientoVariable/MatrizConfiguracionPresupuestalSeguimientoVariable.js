// VARIABLES GLOBALES //
var contadorRegistrosNuevos,
    // LoadPanel
    dxLoadPanelDatosIndicadores,
    // Modal
    modalCerrar,
    // Form
    dxFormSeguimientoVariable,
    // DataGrid
    dxDataGridSeguimientoVariable,
    //Botones
    dxButtonGuardar,
    // Lista
    listMCPSeguimientoVariable = [],
    // Matriz Indicador Resultado Indicador
    matrizIndicadorResultadoIndicador,
    // API
    API_FICHA = "/mir/mir/matrizconfiguracionpresupuestalseguimientovariable/";
//////////////////////
// Function Default //
$(() => {
    // Inicializamos las variables para la ficha
    inicializaVariables();
    // Deshabilitamos los botones de acciones
    habilitaComponentes(false);
});
//////////////////////

inicializaVariables = () => {
    contadorRegistrosNuevos = -1;
    // LoadPanel
    dxLoadPanelDatosIndicadores = $('#dxLoadPanelDatosIndicadores').dxLoadPanel('instance');
    // Modal
    modalCerrar = $('#modalCerrar');
    // Form
    //dxFormSeguimientoVariable = $('#dxFormSeguimientoVariable').dxForm('instance');
    // Button
    dxButtonGuardar = $('#dxButtonGuardar').dxButton("instance");
    // Asignamos lista de Matriz Configuracion Presupuestal Seguimiento Variable
    listMCPSeguimientoVariable = $.extend(true, [], _listMatrizConfiguracionPresupuestalSeguimientoVariable);
    cargaMCPSeguimientoVariable();
}

// Form //
onInitializedFormSeguimientoVariable = (e) => {
    dxFormSeguimientoVariable = e.component;
}
//////////

habilitaComponentes = (enabled) => {
    dxButtonGuardar.option("disabled", !enabled);
}

cargaMCPSeguimientoVariable = () => {
    // Crear o Asignar
    _listMatrizIndicadorResultadoIndicadorFormulaVariable.map(fv => {
        var seguimientoVariable = listMCPSeguimientoVariable.find(sv => sv.MIRIndicadorFormulaVariableId == fv.MIRIndicadorFormulaVariableId);
        if (!seguimientoVariable) {
            let nuevoMatrizConfiguracionPresupuestalSeguimientoVariable = $.extend(true, {}, matrizConfiguracionPresupuestalSeguimientoVariableModel);
            nuevoMatrizConfiguracionPresupuestalSeguimientoVariable.MIRSeguimientoVariableId = contadorRegistrosNuevos;
            nuevoMatrizConfiguracionPresupuestalSeguimientoVariable.MIRIndicadorFormulaVariableId = fv.MIRIndicadorFormulaVariableId;
            nuevoMatrizConfiguracionPresupuestalSeguimientoVariable.EstatusId = ControlMaestroMapeo.EstatusRegistro.ACTIVO;
            // Agregar nuevo mcpsv en lista.
            listMCPSeguimientoVariable.push(nuevoMatrizConfiguracionPresupuestalSeguimientoVariable);
            // Contador
            contadorRegistrosNuevos--;
        } else {
            // Obtener MIRI
            const matrizIndicadorResultadoIndicador = $.extend(true, {}, _listMatrizIndicadorResultadoIndicador.find(miri => miri.MIRIndicadorId == fv.MIRIndicadorId));

            var actualizarMatrizConfiguracionPresupuestalSeguimientoVariable = seguimientoVariable;
            // Trimestral
            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.TRIMESTRAL) {
                // T1 Total
                actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.T1Total = null;
                if (actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Enero != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Febrero != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Marzo != null)
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.T1Total = actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Enero + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Febrero + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Marzo;
                // T2 Total
                actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.T2Total = null;
                if (actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Abril != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Mayo != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Junio != null)
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.T2Total = actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Abril + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Mayo + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Junio;
                // T3 Total
                actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.T3Total = null;
                if (actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Julio != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Agosto != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Septiembre != null)
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.T3Total = actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Julio + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Agosto + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Septiembre;
                // T4 Total
                actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.T4Total = null;
                if (actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Octubre != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Noviembre != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Diciembre != null)
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.T4Total = actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Octubre + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Noviembre + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Diciembre;
                // Total Anul
                actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.TotalAnual =
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Enero +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Febrero +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Marzo +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Abril +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Mayo +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Junio +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Julio +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Agosto +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Septiembre +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Octubre +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Noviembre +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Diciembre;
            }
            // Semestral
            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.SEMESTRAL) {
                // S1 Total
                actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.S1Total = null;
                if (actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Enero != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Febrero != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Marzo != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Abril != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Mayo != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Junio != null)
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.S1Total = actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Enero + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Febrero + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Marzo + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Abril + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Mayo + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Junio;
                // S2 Total
                actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.S2Total = null;
                if (actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Julio != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Agosto != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Septiembre != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Octubre != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Noviembre != null || actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Diciembre != null)
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.S2Total = actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Julio + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Agosto + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Septiembre + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Octubre + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Noviembre + actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Diciembre;
                // Total Anul
                actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.TotalAnual =
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Enero +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Febrero +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Marzo +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Abril +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Mayo +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Junio +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Julio +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Agosto +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Septiembre +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Octubre +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Noviembre +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Diciembre;
            }
            // Anual
            if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.ANUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.BIANUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.TRIANUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.SEXENAL) {
                actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.TotalAnual =
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Enero +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Febrero +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Marzo +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Abril +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Mayo +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Junio +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Julio +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Agosto +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Septiembre +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Octubre +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Noviembre +
                    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable.Diciembre;
            }
        }
    });
}

// TreeView //
onItemClick = (event) => {
    if (!!!event.itemData.Items) {
        // Mostramos Loader
        dxLoadPanelDatosIndicadores.show();
        dxFormSeguimientoVariable.resetValues();
        // Obtenemos MIRI
        matrizIndicadorResultadoIndicador = $.extend(true, {}, _listMatrizIndicadorResultadoIndicador.find(miri => miri.MIRIndicadorId == event.itemData.Id));
        dxFormSeguimientoVariable.option('formData', matrizIndicadorResultadoIndicador);
        cargaDataGridSeguimientoVariable();
        // Agregamos los IDs de Seguimiento Variable
        let listSeguimientoVariableId = [];
        _listMatrizIndicadorResultadoIndicadorFormulaVariable.filter(fv => fv.MIRIndicadorId == matrizIndicadorResultadoIndicador.MIRIndicadorId).map(fv => {
            // Agregar ID de MCPSV en lista
            listSeguimientoVariableId.push(fv.MIRIndicadorFormulaVariableId);
        });
        // Creamos DataSource de Seguimiento Variable para el DataGrid
        var dataSource = new DevExpress.data.DataSource({
            store: {
                type: "array",
                key: "MIRSeguimientoVariableId",
                data: listMCPSeguimientoVariable.filter(sv => listSeguimientoVariableId.indexOf(sv.MIRIndicadorFormulaVariableId) >= 0)
            }
        });
        // Asignamos DataSource de Seguimiento Variable al DataGrid
        dxDataGridSeguimientoVariable.option('dataSource', dataSource);
        // Ocultamos Loader
        dxLoadPanelDatosIndicadores.hide();
    }
}
//////////////

cargaDataGridSeguimientoVariable = () => {
    dxDataGridSeguimientoVariable = $('#dxDataGridSeguimientoVariable').dxDataGrid({
        editing: {
            mode: 'cell',
            allowUpdating: true,
            useIcons: true
        },
        loadPanel: {
            text: 'Cargando...'
        },
        showBorders: true,
        columnAutoWidth: true,
        allowColumnResizing: true,
        columnResizingMode: 'widget',
        rowAlternationEnabled: false,
        showColumnHeaders: true,
        showColumnLines: true,
        showRowLines: true,
        noDataText: 'Sin registros',
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 20, 40, 80],
            showInfo: true,
            infoText: 'Página {0} de {1} ( {2} Registros )'
        },
        columns: obtenerColumnsFrecuenciaMedicion(matrizIndicadorResultadoIndicador.FrecuenciaMedicionId),
        onRowUpdating: onRowUpdating,
        onEditorPreparing: onEditorPreparing
    }).dxDataGrid("instance");

}

//test = (e) => {
//    $(window).resize(() => {
//        console.log('sdas');
//        e.component.repaint();
//    })
//}

obtenerColumnsFrecuenciaMedicion = (FrecuenciaMedicionId) => {
    switch (FrecuenciaMedicionId) {
        case ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.MENSUAL:
            return cargaMensual();
        case ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.TRIMESTRAL:
            return cargaTrimestral();
        case ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.SEMESTRAL:
            return cargaSemestral();
        case ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.ANUAL:
            return cargaABTS();
        case ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.BIANUAL:
            return cargaABTS();
        case ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.TRIANUAL:
            return cargaABTS();
        case ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.SEXENAL:
            return cargaABTS();
    }

}

cargaMensual = () => {
    return [{
        caption: 'Variables',
        allowEditing: false,
        width: '250.00',
        calculateDisplayValue: calculateDisplayValue
    }, {
        caption: 'Enero',
        dataField: 'Enero',
        dataType: 'number',
        format: '#,##0.##',
        editorOptions: {
            format: '#,##0.##',
            valueChangeEvent: 'keyup',
            onKeyDown: function (e) {
                if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                    e.component.option('value', "");
                }
            }
        }
    }, {
        caption: 'Febrero',
        dataField: 'Febrero',
        dataType: 'number',
        format: '#,##0.##',
        editorOptions: {
            format: '#,##0.##',
            valueChangeEvent: 'keyup',
            onKeyDown: function (e) {
                if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                    e.component.option('value', "");
                }
            }
        }
    }, {
        caption: 'Marzo',
        dataField: 'Marzo',
        dataType: 'number',
        format: '#,##0.##',
        editorOptions: {
            format: '#,##0.##',
            valueChangeEvent: 'keyup',
            onKeyDown: function (e) {
                if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                    e.component.option('value', "");
                }
            }
        }
    }, {
        caption: 'Abril',
        dataField: 'Abril',
        dataType: 'number',
        format: '#,##0.##',
        editorOptions: {
            format: '#,##0.##',
            valueChangeEvent: 'keyup',
            onKeyDown: function (e) {
                if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                    e.component.option('value', "");
                }
            }
        }
    }, {
        caption: 'Mayo',
        dataField: 'Mayo',
        dataType: 'number',
        format: '#,##0.##',
        editorOptions: {
            format: '#,##0.##',
            valueChangeEvent: 'keyup',
            onKeyDown: function (e) {
                if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                    e.component.option('value', "");
                }
            }
        }
    }, {
        caption: 'Junio',
        dataField: 'Junio',
        dataType: 'number',
        format: '#,##0.##',
        editorOptions: {
            format: '#,##0.##',
            valueChangeEvent: 'keyup',
            onKeyDown: function (e) {
                if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                    e.component.option('value', "");
                }
            }
        }
    }, {
        caption: 'Julio',
        dataField: 'Julio',
        dataType: 'number',
        format: '#,##0.##',
        editorOptions: {
            format: '#,##0.##',
            valueChangeEvent: 'keyup',
            onKeyDown: function (e) {
                if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                    e.component.option('value', "");
                }
            }
        }
    }, {
        caption: 'Agosto',
        dataField: 'Agosto',
        dataType: 'number',
        format: '#,##0.##',
        editorOptions: {
            format: '#,##0.##',
            valueChangeEvent: 'keyup',
            onKeyDown: function (e) {
                if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                    e.component.option('value', "");
                }
            }
        }
    }, {
        caption: 'Septiembre',
        dataField: 'Septiembre',
        dataType: 'number',
        format: '#,##0.##',
        editorOptions: {
            format: '#,##0.##',
            valueChangeEvent: 'keyup',
            onKeyDown: function (e) {
                if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                    e.component.option('value', "");
                }
            }
        }
    }, {
        caption: 'Octubre',
        dataField: 'Octubre',
        dataType: 'number',
        format: '#,##0.##',
        editorOptions: {
            format: '#,##0.##',
            valueChangeEvent: 'keyup',
            onKeyDown: function (e) {
                if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                    e.component.option('value', "");
                }
            }
        }
    }, {
        caption: 'Noviembre',
        dataField: 'Noviembre',
        dataType: 'number',
        format: '#,##0.##',
        editorOptions: {
            format: '#,##0.##',
            valueChangeEvent: 'keyup',
            onKeyDown: function (e) {
                if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                    e.component.option('value', "");
                }
            }
        }
    }, {
        caption: 'Diciembre',
        dataField: 'Diciembre',
        dataType: 'number',
        format: '#,##0.##',
        editorOptions: {
            format: '#,##0.##',
            valueChangeEvent: 'keyup',
            onKeyDown: function (e) {
                if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                    e.component.option('value', "");
                }
            }
        }
    }];
}

cargaTrimestral = () => {
    return [
        {
            caption: 'Variables',
            allowEditing: false,
            width: '250.00',
            calculateDisplayValue: calculateDisplayValue
        }, {
            caption: 'Primer Trimestre',
            columns: [{
                caption: 'Enero',
                dataField: 'Enero',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Febrero',
                dataField: 'Febrero',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Marzo',
                dataField: 'Marzo',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }]
        }, {
            caption: 'T1 Total',
            dataField: 'T1Total',
            dataType: 'number',
            allowEditing: false,
            format: '#,##0.##',
            cssClass: 'pixvs-datagrid-cell',
        }, {
            caption: 'Segundo Trimestre',
            columns: [{
                caption: 'Abril',
                dataField: 'Abril',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Mayo',
                dataField: 'Mayo',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Junio',
                dataField: 'Junio',
                dataType: 'number',
                format: '#,##0.##',
                    editorOptions: {
                        format: '#,##0.##',
                        valueChangeEvent: 'keyup',
                        onKeyDown: function (e) {
                            if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                                e.component.option('value', "");
                            }
                        }
                    }
            }]
        }, {
            caption: 'T2 Total',
            dataField: 'T2Total',
            dataType: 'number',
            allowEditing: false,
            format: '#,##0.##',
            cssClass: 'pixvs-datagrid-cell',
        }, {
            caption: 'Tercer Trimestre',
            columns: [{
                caption: 'Julio',
                dataField: 'Julio',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Agosto',
                dataField: 'Agosto',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Septiembre',
                dataField: 'Septiembre',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }]
        }, {
            caption: 'T3 Total',
            dataField: 'T3Total',
            dataType: 'number',
            allowEditing: false,
            format: '#,##0.##',
            cssClass: 'pixvs-datagrid-cell',
        }, {
            caption: 'Cuatro Trimestre',
            columns: [{
                caption: 'Octubre',
                dataField: 'Octubre',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Noviembre',
                dataField: 'Noviembre',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Diciembre',
                dataField: 'Diciembre',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }]
        }, {
            caption: 'T4 Total',
            dataField: 'T4Total',
            dataType: 'number',
            allowEditing: false,
            format: '#,##0.##',
            cssClass: 'pixvs-datagrid-cell',
        }, {
            caption: 'Total Anual',
            dataField: 'TotalAnual',
            dataType: 'number',
            allowEditing: false,
            format: '#,##0.##',
            cssClass: 'pixvs-datagrid-cell',
        }
    ];
}

cargaSemestral = () => {
    return [
        {
            caption: 'Variables',
            allowEditing: false,
            width: '250.00',
            calculateDisplayValue: calculateDisplayValue
        }, {
            caption: 'Primer Semestre',
            columns: [{
                caption: 'Enero',
                dataField: 'Enero',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Febrero',
                dataField: 'Febrero',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Marzo',
                dataField: 'Marzo',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Abril',
                dataField: 'Abril',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Mayo',
                dataField: 'Mayo',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Junio',
                dataField: 'Junio',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }]
        }, {
            caption: 'S1 Total',
            dataField: 'S1Total',
            dataType: 'number',
            allowEditing: false,
            format: '#,##0.##',
            cssClass: 'pixvs-datagrid-cell',
        }, {
            caption: 'Segundo Semestre',
            columns: [{
                caption: 'Julio',
                dataField: 'Julio',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Agosto',
                dataField: 'Agosto',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Septiembre',
                dataField: 'Septiembre',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                    }
            }, {
                caption: 'Octubre',
                dataField: 'Octubre',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Noviembre',
                dataField: 'Noviembre',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }, {
                caption: 'Diciembre',
                dataField: 'Diciembre',
                dataType: 'number',
                format: '#,##0.##',
                editorOptions: {
                    format: '#,##0.##',
                    valueChangeEvent: 'keyup',
                    onKeyDown: function (e) {
                        if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                            e.component.option('value', "");
                        }
                    }
                }
            }]
        }, {
            caption: 'S2 Total',
            dataField: 'S2Total',
            dataType: 'number',
            allowEditing: false,
            format: '#,##0.##',
            cssClass: 'pixvs-datagrid-cell',
        }, {
            caption: 'Total Anual',
            dataField: 'TotalAnual',
            name: 'TotalAnual',
            dataType: 'number',
            allowEditing: false,
            format: '#,##0.##',
            cssClass: 'pixvs-datagrid-cell',
        }
    ];
}

cargaABTS = () => {
    return [{
        caption: 'Variables',
        allowEditing: false,
        width: '250.00',
        calculateDisplayValue: calculateDisplayValue
    }, {
        caption: 'Primer Año',
        columns: [{
            caption: 'Enero',
            dataField: 'Enero',
            dataType: 'number',
            format: '#,##0.##',            
            editorOptions: {
                format: '#,##0.##',
                valueChangeEvent: 'keyup',
                onKeyDown: function (e) {
                    if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                        e.component.option('value', "");
                    } 
                }
            }
        }, {
            caption: 'Febrero',
            dataField: 'Febrero',
            dataType: 'number',
            format: '#,##0.##',
            editorOptions: {
                format: '#,##0.##',
                valueChangeEvent: 'keyup',
                onKeyDown: function (e) {
                    if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                        e.component.option('value', "");
                    }
                }
            }
        }, {
            caption: 'Marzo',
            dataField: 'Marzo',
            dataType: 'number',
            format: '#,##0.##',
            editorOptions: {
                format: '#,##0.##',
                valueChangeEvent: 'keyup',
                onKeyDown: function (e) {
                    if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                        e.component.option('value', "");
                    }
                }
            }
        }, {
            caption: 'Abril',
            dataField: 'Abril',
            dataType: 'number',
            format: '#,##0.##',
            editorOptions: {
                format: '#,##0.##',
                valueChangeEvent: 'keyup',
                onKeyDown: function (e) {
                    if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                        e.component.option('value', "");
                    }
                }
            }
        }, {
            caption: 'Mayo',
            dataField: 'Mayo',
            dataType: 'number',
            format: '#,##0.##',
            editorOptions: {
                format: '#,##0.##',
                valueChangeEvent: 'keyup',
                onKeyDown: function (e) {
                    if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                        e.component.option('value', "");
                    }
                }
            }
        }, {
            caption: 'Junio',
            dataField: 'Junio',
            dataType: 'number',
            format: '#,##0.##',
            editorOptions: {
                format: '#,##0.##',
                valueChangeEvent: 'keyup',
                onKeyDown: function (e) {
                    if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                        e.component.option('value', "");
                    }
                }
            }
        }, {
            caption: 'Julio',
            dataField: 'Julio',
            dataType: 'number',
            format: '#,##0.##',
            editorOptions: {
                format: '#,##0.##',
                valueChangeEvent: 'keyup',
                onKeyDown: function (e) {
                    if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                        e.component.option('value', "");
                    }
                }
            }
        }, {
            caption: 'Agosto',
            dataField: 'Agosto',
            dataType: 'number',
            format: '#,##0.##',
            editorOptions: {
                format: '#,##0.##',
                valueChangeEvent: 'keyup',
                onKeyDown: function (e) {
                    if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                        e.component.option('value', "");
                    }
                }
            }
        }, {
            caption: 'Septiembre',
            dataField: 'Septiembre',
            dataType: 'number',
            format: '#,##0.##',
            editorOptions: {
                format: '#,##0.##',
                valueChangeEvent: 'keyup',
                onKeyDown: function (e) {
                    if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                        e.component.option('value', "");
                    }
                }
            }
        }, {
            caption: 'Octubre',
            dataField: 'Octubre',
            dataType: 'number',
            format: '#,##0.##',
            editorOptions: {
                format: '#,##0.##',
                valueChangeEvent: 'keyup',
                onKeyDown: function (e) {
                    if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                        e.component.option('value', "");
                    }
                }
            }
        }, {
            caption: 'Noviembre',
            dataField: 'Noviembre',
            dataType: 'number',
            format: '#,##0.##',
            editorOptions: {
                format: '#,##0.##',
                valueChangeEvent: 'keyup',
                onKeyDown: function (e) {
                    if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                        e.component.option('value', "");
                    }
                }
            }
        }, {
            caption: 'Diciembre',
            dataField: 'Diciembre',
            dataType: 'number',
            format: '#,##0.##',
            editorOptions: {
                format: '#,##0.##',
                valueChangeEvent: 'keyup',
                onKeyDown: function (e) {
                    if (e.event.keyCode === 8 && e.component.option('value') === 0) {
                        e.component.option('value', "");
                    }
                }
            }
        }]
    }, {
        caption: 'Total Anual',
        dataField: 'TotalAnual',
        name: 'TotalAnual',
        dataType: 'number',
        allowEditing: false,
        format: '#,##0.##',
        cssClass: 'pixvs-datagrid-cell',
    }];
}

// DataGrid //
calculateDisplayValue = (event) => {
    return _listMatrizIndicadorResultadoIndicadorFormulaVariable.find(fv => fv.MIRIndicadorFormulaVariableId == event.MIRIndicadorFormulaVariableId).DescripcionVariable;
}

onRowUpdating = (event) => {
    let mcpsv = event.oldData;
    Object.assign(mcpsv, event.newData);
    // Trimestral
    if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.TRIMESTRAL) {
        if (mcpsv.Enero != null || mcpsv.Febrero != null || mcpsv.Marzo != null)
            event.newData.T1Total = mcpsv.Enero + mcpsv.Febrero + mcpsv.Marzo;
        if (mcpsv.Abril != null || mcpsv.Mayo != null || mcpsv.Junio != null)
            event.newData.T2Total = mcpsv.Abril + mcpsv.Mayo + mcpsv.Junio;
        if (mcpsv.Julio != null || mcpsv.Agosto != null || mcpsv.Septiembre != null)
            event.newData.T3Total = mcpsv.Julio + mcpsv.Agosto + mcpsv.Septiembre;
        if (mcpsv.Octubre != null || mcpsv.Noviembre != null || mcpsv.Diciembre != null)
            event.newData.T4Total = mcpsv.Octubre + mcpsv.Noviembre + mcpsv.Diciembre;
        event.newData.TotalAnual = mcpsv.Enero + mcpsv.Febrero + mcpsv.Marzo + mcpsv.Abril + mcpsv.Mayo + mcpsv.Junio + mcpsv.Julio + mcpsv.Agosto + mcpsv.Septiembre + mcpsv.Octubre + mcpsv.Noviembre + mcpsv.Diciembre;
    }
    // Semestral
    if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.SEMESTRAL) {
        if (mcpsv.Enero != null || mcpsv.Febrero != null || mcpsv.Marzo != null || mcpsv.Abril != null || mcpsv.Mayo != null || mcpsv.Junio != null)
            event.newData.S1Total = mcpsv.Enero + mcpsv.Febrero + mcpsv.Marzo + mcpsv.Abril + mcpsv.Mayo + mcpsv.Junio;
        if (mcpsv.Julio != null || mcpsv.Agosto != null || mcpsv.Septiembre != null || mcpsv.Octubre != null || mcpsv.Noviembre != null || mcpsv.Diciembre != null)
            event.newData.S2Total = mcpsv.Julio + mcpsv.Agosto + mcpsv.Septiembre + mcpsv.Octubre + mcpsv.Noviembre + mcpsv.Diciembre;
        event.newData.TotalAnual = mcpsv.Enero + mcpsv.Febrero + mcpsv.Marzo + mcpsv.Abril + mcpsv.Mayo + mcpsv.Junio + mcpsv.Julio + mcpsv.Agosto + mcpsv.Septiembre + mcpsv.Octubre + mcpsv.Noviembre + mcpsv.Diciembre;
    }
    // Anual
    if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.ANUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.BIANUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.TRIANUAL || matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.SEXENAL) {
        event.newData.TotalAnual = mcpsv.Enero + mcpsv.Febrero + mcpsv.Marzo + mcpsv.Abril + mcpsv.Mayo + mcpsv.Junio + mcpsv.Julio + mcpsv.Agosto + mcpsv.Septiembre + mcpsv.Octubre + mcpsv.Noviembre + mcpsv.Diciembre;
    }
    // Actualizar los datos
    var actualizarMatrizConfiguracionPresupuestalSeguimientoVariable = listMCPSeguimientoVariable.find(sv => sv.MIRSeguimientoVariableId == mcpsv.MIRSeguimientoVariableId);
    actualizarMatrizConfiguracionPresupuestalSeguimientoVariable = $.extend(true, {}, mcpsv);
    // Habilitamos los botones de acciones
    habilitaComponentes(true);
}

onEditorPreparing = (event) => {
    if (event.parentType == 'dataRow' && event.row) {
        // Verificar el control periodo para que saber si el mes esta abierto poder editar o si no no pueda editar como deshabilitar
        if (_listaControlMaestroControlPeriodo.find(cp => cp.Codigo == funcionObtenerCodigoMes(event.dataField)).EstatusPeriodoId != ControlMaestroMapeo.MIEstatusPeriodo.ABIERTO) {
            toast('El mes ' + event.dataField + ' no se encuentra abierto.', 'warning');
            // Deshabilitar la celda
            event.editorOptions.disabled = true;
        }
            
    }
}
//////////////

// Toast //
toast = (message, type) => {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 3500);
}
///////////

guardaCambios = () => {
    // Mostramos Loader
    dxLoaderPanel.show();
    // Obtener los datos para saber hay los datos o no hay para guardar
    const data = obtenerData();
    if (data.listMatrizConfiguracionPresupuestalSeguimientoVariable.length == 0) {
        toast('No se puede guardar ya que no hubo cambios en la información', 'error');
        // Ocultamos Loader
        dxLoaderPanel.hide();
        return;
    }
    var fechaValue = $('#dtFecha').dxDateBox('instance').option("value");
    var fechaFormateada = fechaValue.toLocaleString();
    data.FechaModificacion = fechaFormateada;
    $.ajax({
        type: 'POST',
        url: API_FICHA + 'guardar',
        data: data,
        success: function (response) {
            // Mostramos mensaje de Exito
            toast(response, 'success');

            // Redirigir a listado
            window.location.href = API_FICHA + 'listar';
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            // Mostramos mensaje de error
            //toast(response.responseText, "error");
            toast('No se puede guardar los cambios, inténtalo de nuevo más tarde', "error");
        }
    });

}

obtenerData = () => {
    let data = {
        listMatrizConfiguracionPresupuestalSeguimientoVariable: []
    };
    // Matriz Configuracion Presupuestal Seguimiento Variable
    listMCPSeguimientoVariable.map(sv => {
        if (sv.MIRSeguimientoVariableId > 0) {
            const matrizConfiguracionPresupuestalSeguimientoVariable = $.extend(true, {}, _listMatrizConfiguracionPresupuestalSeguimientoVariable.find(_sv => _sv.MIRSeguimientoVariableId == sv.MIRSeguimientoVariableId));
            if (sv.Enero != matrizConfiguracionPresupuestalSeguimientoVariable.Enero || sv.Febrero != matrizConfiguracionPresupuestalSeguimientoVariable.Febrero || sv.Marzo != matrizConfiguracionPresupuestalSeguimientoVariable.Marzo || sv.Abril != matrizConfiguracionPresupuestalSeguimientoVariable.Abril || sv.Mayo != matrizConfiguracionPresupuestalSeguimientoVariable.Mayo || sv.Junio != matrizConfiguracionPresupuestalSeguimientoVariable.Junio || sv.Julio != matrizConfiguracionPresupuestalSeguimientoVariable.Julio || sv.Agosto != matrizConfiguracionPresupuestalSeguimientoVariable.Agosto || sv.Septiembre != matrizConfiguracionPresupuestalSeguimientoVariable.Septiembre || sv.Octubre != matrizConfiguracionPresupuestalSeguimientoVariable.Octubre || sv.Noviembre != matrizConfiguracionPresupuestalSeguimientoVariable.Noviembre || sv.Diciembre != matrizConfiguracionPresupuestalSeguimientoVariable.Diciembre) {
                let _matrizConfiguracionPresupuestalSeguimientoVariable = $.extend(true, {}, sv);
                _matrizConfiguracionPresupuestalSeguimientoVariable.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(_matrizConfiguracionPresupuestalSeguimientoVariable.Timestamp)));
                data.listMatrizConfiguracionPresupuestalSeguimientoVariable.push(_matrizConfiguracionPresupuestalSeguimientoVariable);
            }
        } else {
            if (sv.Enero != null || sv.Febrero != null || sv.Marzo != null || sv.Abril != null || sv.Mayo != null || sv.Junio != null || sv.Julio != null || sv.Agosto != null || sv.Septiembre != null || sv.Octubre != null || sv.Noviembre != null || sv.Diciembre != null) {
                data.listMatrizConfiguracionPresupuestalSeguimientoVariable.push(sv);
            }
        }
    });

    return data;
}

// Otro //
regresarListado = () => {
    window.location.href = API_FICHA + "listar";
}

recargarFicha = () => {
    window.location.href = API_FICHA + 'editar/' + _consultaMatrizIndicadorResultado.MIRId;
}

validaHayCambios = () => {
    // Obtener los datos para saber hay los datos o no hay para guardar
    const data = obtenerData();
    if (data.listMatrizConfiguracionPresupuestalSeguimientoVariable.length == 0) {
        regresarListado();
    } else {
        modalCerrar.modal('show');
    }
}
//////////