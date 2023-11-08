// VARIABLES GLOBALES //
var contadorRegistrosNuevos,
    // Modal
    modalConfirmaEliminar,
    modalCerrar,
    //modalConfirmaDeshacer,
    modalConfirmaDeshacerNivel,
    modalConfirmaEliminarNivel,
    // Drawer
    dxDrawer,
    dxListDrawer,
    // Content
    contenido,
    // DataGrid
    dxDataGridContenido,
    // Form
    dxFormContenido,
    // SelectBox
    dxSelectBoxEjercicio,
    dxSelectBoxPlanDesarrollo,
    // DropDownBox
    dxDropDownBoxPlanDesarrolloEstructura,
    // Button
    dxButtonEliminar,
    //dxButtonDeshacer,
    dxButtonGuardar,
    // Table
    tablaNivelActividadComponente,
    tablaNivelActividadFormulario,
    // Listas
    listMatrizIndicadorResultadoIndicador = [],
    listMatrizIndicadorResultadoIndicadorMeta = [],
    listMatrizIndicadorResultadoIndicadorFormulaVariable = [],
    // TabPanel
    dxTabPanel,
    // Otro
    eliminaRowNivel,
    // API
    API_FICHA = '/mir/mir/matrizindicadorresultado/';

// Matriz Indicador Resultado
var matrizIndicadorResultado = null;

var nivelModel = {
    caption: '',
    name: '',
    nivelId: null,
    nivelIdComponente: null
}

var seleccionModel = {
    actual: null,
    siguiente: null
}
// Variables
let esModoEditar = _matrizIndicadorResultado.MIRId > 0 ? true : false, esEditar = false, esActividad = false, esFormulario = false, esMenu = false, globalMatrizIndicadorResultadoIndicador = null, proyectoPorcentajeTotal = 0, esModoLectura = false, listaMetaGlobal = [];
//////////////////////

// Function Default //
$(() => {
    // Inicializamos las variables para la ficha
    inicializaVariables();
});
//////////////////////

inicializaVariables = () => {
    // Eliminamos una clase de style en template content
    $('#templateContent').removeClass('d-none');

    contadorRegistrosNuevos = -1;
    // Modal
    modalConfirmaEliminar = $('#modalConfirmaEliminar');
    modalCerrar = $('#modalCerrar');
    //modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    modalConfirmaDeshacerNivel = $('#modalConfirmaDeshacerNivel');
    modalConfirmaEliminarNivel = $('#modalConfirmaEliminarNivel');
    // Drawer
    dxDrawer = $('#dxDrawer').dxDrawer('instance');
    dxListDrawer = $('#dxListDrawer').dxList('instance');
    // Content
    contenido = $('#contenido');
    // Button
    dxButtonEliminar = $("#dxButtonEliminar").dxButton("instance");
    //dxButtonDeshacer = $("#dxButtonDeshacer").dxButton("instance");
    dxButtonGuardar = $("#dxButtonGuardar").dxButton("instance");
    // Carga Tipo Componente con el usuario tiene permiso para visualizar y modificar el proyecto
    cargaTipoComponente();
    // Convertir la fecha
    if (new Date(_matrizIndicadorResultado.FechaFinConfiguracion) == 'Invalid Date') {
        _matrizIndicadorResultado.FechaFinConfiguracion = new Date(parseInt(_matrizIndicadorResultado.FechaFinConfiguracion.substr(6)))
    }
    // Si es editar para habilitar los combos y alunos
    //if (esModoEditar) {
    //    // Fecha Fin Configuracion si la fecha es raro entonces convertir a fecha con Date
    //    if (new Date(_matrizIndicadorResultado.FechaFinConfiguracion) == 'Invalid Date') {
    //        _matrizIndicadorResultado.FechaFinConfiguracion = new Date(parseInt(_matrizIndicadorResultado.FechaFinConfiguracion.substr(6)))
    //    }
    //    // Habilitar los combos
    //    //habilitaCombos();
    //}
    // Asignamos
    matrizIndicadorResultado = $.extend(true, {}, _matrizIndicadorResultado);
    // Verificar si la fecha de Fin Configuracion es vencer para cambiar el modo LEctura
    esModoLecturaPorVencidoFechaFinCongiguracion();
    if (esModoLectura) {
        habilitaVisibleBotones(true);
    } else {
        dxButtonEliminar.option("visible", matrizIndicadorResultado.MIRId > 0);
    }
    // Listas
    listMatrizIndicadorResultadoIndicador = _listMatrizIndicadorResultadoIndicador ? $.extend(true, [], _listMatrizIndicadorResultadoIndicador) : [];
    listMatrizIndicadorResultadoIndicadorMeta = _listMatrizIndicadorResultadoIndicadorMeta ? $.extend(true, [], _listMatrizIndicadorResultadoIndicadorMeta) : [];
    listMatrizIndicadorResultadoIndicadorFormulaVariable = _listMatrizIndicadorResultadoIndicadorFormulaVariable ? $.extend(true, [], _listMatrizIndicadorResultadoIndicadorFormulaVariable) : [];
    // Establecer 1 al seleccion actual (1 es Datos Generales)
    seleccionModel.actual = 1;
    // Inicia carga el contenido de datos generales
    cargaContenidoDatosGenerales();
}

contentTemplateTreeListPlanDesarrolloEstructura = (event) => {
    var treeView = $('<div/>').dxTreeList({
        elementAttr: { id: 'dxTreeListEstructura'} ,
        dataSource: event.component.getDataSource(),
        rootValue: 0,
        keyExpr: 'PlanDesarrolloEstructuraId',
        parentIdExpr: 'EstructuraPadreId',
        allowColumnReordering: false,
        columnAutoWidth: true,
        wordWrapEnabled: true,
        showBorders: true,
        noDataText: 'Sin registros',
        selection: {
            mode: 'single'
        },
        columns: [
            {
                dataField: 'Nombre',
                allowSorting: false
            }, {
                dataField: 'Etiqueta',
                allowSorting: false
            }
        ],
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [0, 10, 20, 30, 40, 50],
            showInfo: true,
            infoText: 'Página {0} de {1} ( {2} Registros )'
        },
        onSelectionChanged: (_event) => {
            if (_event.selectedRowKeys.length) {
                event.component.option('value', _event.selectedRowKeys[0]);
                event.component.close();
            }
        }
    })
    return treeView;
}

// Cargar los datos y los combos //
// Carga Tipo Componente con el usuario permiso
cargaTipoComponente = () => {
    if (!_esPermisoProyecto) {
        let index = _listTipoComponente.findIndex(tc => tc.ControlId == ControlMaestroMapeo.TipoComponente.RELACION_COMPONENTE);
        _listTipoComponente.splice(index, 1);
    }
}
// Carga el contenido de Datos Generales
cargaContenidoDatosGenerales = () => {
    // Limpiamos el contenido
    contenido.empty();

    contenido.append($('<div/>', {
        class: 'row'
    }).append($('<div/>', {
        class: 'col-12'
    }).append($('<h2/>', {
        class: 'text-center',
        text: 'Nueva Matriz de Indicadores para Resultados'
    })).append($('<div/>', {
        class: 'd-flex flex-row justify-content-end'
    }).append($('<div/>', {
        id: 'dxButtonNext'
    }))))).append($('<div/>', {
        class: 'row'
    }).append($('<div/>', {
        class: 'col-12'
    }).append($('<div/>', {
        id: 'dxFormContenido'
    }))));
    // Button
    $('#dxButtonNext').dxButton({
        elementAttr: { class: 'btn-primary btn-oblong btn-width-100' },
        text: "Siguiente",
        onClick: (event) => {
            onClickSiguiente(event, 2);
        }
    });
    // Form
    dxFormContenido = $('#dxFormContenido').dxForm({
        formData: matrizIndicadorResultado,
        requiredMark: '(*)',
        labelLocation: 'top',
        readOnly: esModoLectura,
        items: [
            {
                itemType: 'group',
                colCount: 12,
                caption: 'Datos Generales',
                name: 'DatosGenerales',
                items: [
                    {
                        itemType: 'group',
                        colSpan: 12,
                        colCount: 12,
                        name: 'DatosGeneralesGrupo',
                        items: [
                            {
                                dataField: 'Ejercicio',
                                colSpan: 4,
                                editorType: 'dxSelectBox',
                                editorOptions: {
                                    dataSource: _listEjercicio,
                                    displayExpr: 'Ejercicio',
                                    valueExpr: 'Ejercicio',
                                    searchEnaled: true,
                                    searchMode: 'contains',
                                    showClearButton: true,
                                    noDataText: 'Sin registros que mostrar',
                                    placeholder: 'Selecciona una opción',
                                    onValueChanged: onValueChangedEjercicio,
                                    onInitialized: (event) => dxSelectBoxEjercicio = event.component,
                                    readOnly: true
                                },
                                validationRules: [{
                                    type: "required",
                                    message: "Ejercicio requerido"
                                }]
                            }, {
                                dataField: 'PlanDesarrolloId',
                                label: {
                                    text: 'Plan de Desarrollo'
                                },
                                colSpan: 8,
                                editorType: 'dxSelectBox',
                                editorOptions: {
                                    dataSource: matrizIndicadorResultado.Ejercicio ? cargaPlanDesarrollo(matrizIndicadorResultado.Ejercicio) : null,
                                    displayExpr: displayExprPlanDesarrollo,
                                    valueExpr: 'PlanDesarrolloId',
                                    searchEnaled: true,
                                    searchMode: 'contains',
                                    showClearButton: true,
                                    noDataText: 'Sin registros que mostrar',
                                    placeholder: 'Selecciona una opción',
                                    onValueChanged: onValueChangedPlanDesarrollo,
                                    onInitialized: (event) => dxSelectBoxPlanDesarrollo = event.component,
                                    disabled: matrizIndicadorResultado.Ejercicio ? false : true
                                },
                                validationRules: [{
                                    type: "required",
                                    message: "Plan de Desarrollo requerido"
                                }]
                            }, {
                                dataField: 'PlanDesarrolloEstructuraId',
                                label: {
                                    text: 'Último Nivel del Plan de Desarrollo relacionado a la MIR'
                                },
                                colSpan: 12,
                                editorType: 'dxDropDownBox',
                                editorOptions: {
                                    dataSource: matrizIndicadorResultado.PlanDesarrolloId ? cargaPlanDesarrolloEstructura(matrizIndicadorResultado.PlanDesarrolloId) : null,
                                    displayExpr: (event) => { return event ? event.Nombre : '' },
                                    valueExpr: 'PlanDesarrolloEstructuraId',
                                    onInitialized: (event) => dxDropDownBoxPlanDesarrolloEstructura = event.component,
                                    contentTemplate: contentTemplateTreeListPlanDesarrolloEstructura,
                                    disabled: matrizIndicadorResultado.PlanDesarrolloId ? false : true
                                },
                                validationRules: [{
                                    type: "required",
                                    message: "Último Nivel del Plan de Desarrollo relacionado a la MIR requerido"
                                }]
                            }, {
                                dataField: 'PoblacionObjetivo',
                                label: {
                                    text: 'Población Objetivo'
                                },
                                colSpan: 12,
                                editorType: 'dxTextArea',
                                editorOptions: {
                                    height: 100
                                }
                            }, {
                                dataField: 'ProgramaPresupuestarioId',
                                label: {
                                    text: 'Programa Presupuestario'
                                },
                                colSpan: 8,
                                editorType: 'dxSelectBox',
                                editorOptions: {
                                    dataSource: {
                                        store: _listProgramaGobierno,
                                        paginate: true,
                                        pageSize: 10
                                    },
                                    displayExpr: (event) => { return event ? event.ProgramaGobiernoId + ' - ' + event.Nombre : null },
                                    valueExpr: 'ProgramaGobiernoId',
                                    searchEnaled: true,
                                    searchMode: 'contains',
                                    showClearButton: true,
                                    noDataText: 'Sin registros que mostrar',
                                    placeholder: 'Selecciona una opción',
                                    onValueChanged: () => cargaProducto(),
                                },
                                validationRules: [{
                                    type: "required",
                                    message: "Programa Presupuestario requerido"
                                }]
                            }, {
                                dataField: 'FechaFinConfiguracion',
                                label: {
                                    text: 'Fecha Fin Configuración'
                                },
                                colSpan: 4,
                                editorType: 'dxDateBox',
                                editorOptions: {
                                    value: new Date(matrizIndicadorResultado.FechaFinConfiguracion) != 'Invalid Date' ? matrizIndicadorResultado.FechaFinConfiguracion : null,
                                    showClearButton: true,
                                    type: 'date',
                                    min: new Date(parseInt(matrizIndicadorResultado.Ejercicio), 0, 1)
                                    //,max: new Date(parseInt(matrizIndicadorResultado.Ejercicio), 11, 31)
                                },
                                validationRules: [{
                                    type: "required",
                                    message: "Fecha Fin Configuración requerida"
                                }]
                            }
                        ]
                    }
                ]

            },
            //{
            //    itemType: 'group',
            //    colCount: 12,
            //    caption: 'Alineación al Plan de Desarrollo',
            //    name: 'AlineacionPND',
            //    items: [
            //        {
            //            itemType: 'group',
            //            colSpan: 12,
            //            colCount: 12,
            //            name: 'AlineacionPNDGrupo',
            //            items: [
            //                {
            //                    dataField: 'PlanDesarrolloEstructuraId',
            //                    label: {
            //                        text: 'Tipo de Plan'
            //                    },
            //                    colSpan: 4,
            //                    editorType: 'dxDropDownBox',
            //                    editorOptions: {
            //                        dataSource: matrizIndicadorResultado.PlanDesarrolloId ? cargaPlanDesarrolloEstructura(matrizIndicadorResultado.PlanDesarrolloId) : null,
            //                        displayExpr: (event) => { return event ? event.Nombre : '' },
            //                        valueExpr: 'PlanDesarrolloEstructuraId',
            //                        onInitialized: (event) => dxDropDownBoxPlanDesarrolloEstructura = event.component,
            //                        contentTemplate: contentTemplateTreeListPlanDesarrolloEstructura,
            //                        disabled: matrizIndicadorResultado.PlanDesarrolloId ? false : true
            //                    },
            //                    validationRules: [{
            //                        type: "required",
            //                        message: "Tipo de Plan requerido"
            //                    }]
            //                }
            //            ]
            //        }
            //    ]
            //}
        ]
    }).dxForm('instance');
}
// Carga el contenido de DataGrid
cargaContenidoDataGrid = (nivel) => {
    // Limpiamos el contenido
    contenido.empty();
    // Obtener nivel
    nivelModel = obtenerNivel(nivel);

    contenido.append($('<div/>', {
        class: 'row'
    }).append($('<div/>', {
        class: 'col-12'
    }).append($('<h2/>', {
        class: 'text-center',
        text: 'Nueva Matriz de Indicadores para Resultados'
    })).append($('<div/>', {
        class: 'd-flex flex-column flex-sm-column flex-md-row flex-lg-row flex-xl-row justify-content-end'
    }).append($('<div/>', {
        id: 'dxButtonAdd'
    })).append($('<div/>', {
        id: 'dxButtonPrevious'
    })).append($('<div/>', {
        id: 'dxButtonNext'
    }))))).append($('<div/>', {
        class: 'row'
    }).append($('<div/>', {
        class: 'col-12'
    }).append($('<div/>', {
        id: 'dxFormContenido'
    }))));
    // Button
    if (!esModoLectura) {
        if (nivelModel.nivelId != ControlMaestroMapeo.Nivel.ACTIVIDAD) {
            $('#dxButtonAdd').dxButton({
                elementAttr: { class: 'btn-outline-primary btn-oblong btn-width-100 ml-auto ml-sm-auto ml-md-0 ml-lg-0 ml-xl-0 mr-auto mr-sm-auto mr-md-3 mr-lg-3 mr-xl-3 mb-2 mb-sm-2 mb-md-0 mb-lg-0 mb-xl-0' },
                text: "Agregar",
                onClick: () => onClickAgregarNivel(nivel)
            });
        }
    }

    $('#dxButtonPrevious').dxButton({
        elementAttr: { class: 'btn-primary btn-oblong btn-width-100 ml-auto ml-sm-auto ml-md-0 ml-lg-0 ml-xl-0 mr-auto mr-sm-auto mr-md-3 mr-lg-3 mr-xl-3 mb-2 mb-sm-2 mb-md-0 mb-lg-0 mb-xl-0' },
        text: "Anterior",
        onClick: (event) => {
            onClickAnterior(event, seleccionModel.actual - 1);
        }
    });
    $('#dxButtonNext').dxButton({
        elementAttr: { class: 'btn-primary btn-oblong btn-width-100 ml-auto ml-sm-auto ml-md-0 ml-lg-0 ml-xl-0 mr-auto mr-sm-auto mr-md-0 mr-lg-0 mr-xl-0 mb-2 mb-sm-2 mb-md-0 mb-lg-0 mb-xl-0' },
        text: "Siguiente",
        onClick: (event) => {
            onClickSiguiente(event, seleccionModel.actual + 1);
        }
    });
    // Form
    dxFormContenido = $('#dxFormContenido').dxForm({
        labelLocation: 'top',
        items: [
            {
                itemType: 'group',
                colCount: 12,
                caption: nivelModel.caption,
                name: nivelModel.name,
                items: [
                    {
                        itemType: 'simple',
                        colSpan: 12,
                        template: () => {
                            return $('<div/>', {
                                id: 'dxDataGridContenido'
                            }).dxDataGrid({
                                //dataSource: {
                                //    store: new DevExpress.data.ArrayStore({
                                //        data: listMatrizIndicadorResultadoIndicador,
                                //        key: 'MIRIndicadorId'
                                //    }),
                                //    filter: [['EstatusId', '=', ControlMaestroMapeo.EstatusRegistro.ACTIVO], 'and', ['NivelIndicadorId', '=', nivelModel.nivelId == ControlMaestroMapeo.Nivel.ACTIVIDAD ? nivelModel.nivelIdComponente : nivelModel.nivelId]]
                                //},
                                onInitialized: onInitializedDataGridContenido,
                                showBorders: true,
                                columnAutoWidth: true,
                                allowColumnResizing: true,
                                columnResizingMode: 'widget',
                                rowAlternationEnabled: true,
                                noDataText: 'Sin registros',
                                paging: {
                                    pageSize: 5
                                },
                                pager: {
                                    showPageSizeSelector: true,
                                    allowedPageSizes: [5, 10, 20, 40, 60],
                                    showInfo: true,
                                    infoText: 'Página {0} de {1} ( {2} Registros )'
                                },
                                searchPanel: {
                                    placeholder: 'Buscar...',
                                    searchVisibleColumnsOnly: true,
                                    highlightSearchText: false,
                                    visible: true,
                                    width: 200
                                },
                                columns: [
                                    {
                                        dataField: 'Codigo',
                                        caption: 'Código',
                                        calculateCellValue: calculateCellValueCodigo
                                    }, {
                                        dataField: 'NivelIndicadorId',
                                        caption: 'Nivel',
                                        lookup: {
                                            dataSource: _listNivel,
                                            valueExpr: 'ControlId',
                                            displayExpr: 'Valor'
                                        }
                                    }, {
                                        dataField: 'NombreIndicador',
                                        caption: 'Nombre del Indicador'
                                    }, {
                                        dataField: 'FrecuenciaMedicionId',
                                        caption: 'Frecuencia de medición',
                                        lookup: {
                                            dataSource: _listControlMaestroFrecuenciaMedicionConNivel.filter(fmn => fmn.NivelId == (nivelModel.nivelId == ControlMaestroMapeo.Nivel.ACTIVIDAD ? ControlMaestroMapeo.Nivel.COMPONENTE : nivelModel.nivelId)),
                                            valueExpr: 'FrecuenciaMedicionId',
                                            displayExpr: 'Descripcion'
                                        }
                                    }, {
                                        dataField: 'SentidoId',
                                        caption: 'Sentido',
                                        lookup: {
                                            dataSource: _listSentido,
                                            valueExpr: 'ControlId',
                                            displayExpr: 'Valor'
                                        }
                                    }, {
                                        width: 100,
                                        caption: 'Acciones',
                                        fixed: true,
                                        fixedPosition: 'right',
                                        type: 'buttons',
                                        buttons: nivelModel.nivelId == ControlMaestroMapeo.Nivel.ACTIVIDAD ?
                                            [
                                                {
                                                    hint: 'Agregar',
                                                    icon: 'add',
                                                    onClick: onClickVerActividad
                                                }
                                            ]
                                            :
                                            [
                                                {
                                                    hint: 'Ver',
                                                    icon: 'doc',
                                                    onClick: (event) => onClickEditarNivel(event, nivel),
                                                    visible: esModoLectura
                                                }, {
                                                    hint: 'Editar',
                                                    icon: 'edit',
                                                    onClick: (event) => onClickEditarNivel(event, nivel),
                                                    visible: !esModoLectura
                                                }, {
                                                    hint: 'Eliminar',
                                                    icon: 'trash',
                                                    onClick: (event) => onClickEliminarNivel(event, nivel),
                                                    visible: !esModoLectura
                                                }
                                            ]

                                    }
                                ]
                            });
                        }
                    }
                ]

            }
        ]
    }).dxForm('instance');
    // DataGrid
    //dxDataGridContenido = $('#dxDataGridContenido').dxDataGrid({
    //    dataSource: {
    //        store: new DevExpress.data.ArrayStore({
    //            data: listMatrizIndicadorResultadoIndicador,
    //            key: 'MIRIndicadorId'
    //        }),
    //        filter: [['EstatusId', '=', ControlMaestroMapeo.EstatusRegistro.ACTIVO], 'and', ['NivelIndicadorId', '=', nivelModel.nivelId == ControlMaestroMapeo.Nivel.ACTIVIDAD ? nivelModel.nivelIdComponente : nivelModel.nivelId]]
    //    },
    //    showBorders: true,
    //    onInitialized: test,
    //    columnAutoWidth: true,
    //    allowColumnResizing: true,
    //    columnResizingMode: 'widget',
    //    rowAlternationEnabled: true,
    //    noDataText: 'Sin registros',
    //    paging: {
    //        pageSize: 5
    //    },
    //    pager: {
    //        showPageSizeSelector: true,
    //        allowedPageSizes: [5, 10, 20, 40, 60],
    //        showInfo: true,
    //        infoText: 'Página {0} de {1} ( {2} Registros )'
    //    },
    //    searchPanel: {
    //        placeholder: 'Buscar...',
    //        searchVisibleColumnsOnly: true,
    //        highlightSearchText: false,
    //        visible: true,
    //        width: 200
    //    },
    //    columns: [
    //        {
    //            dataField: 'Codigo',
    //            caption: 'Código',
    //            calculateCellValue: calculateCellValueCodigo
    //        }, {
    //            dataField: 'NivelIndicadorId',
    //            caption: 'Nivel',
    //            lookup: {
    //                dataSource: _listNivel,
    //                valueExpr: 'ControlId',
    //                displayExpr: 'Valor'
    //            }
    //        }, {
    //            dataField: 'NombreIndicador',
    //            caption: 'Nombre del Indicador'
    //        }, {
    //            dataField: 'FrecuenciaMedicionId',
    //            caption: 'Frecuencia de medición',
    //            lookup: {
    //                dataSource: _listControlMaestroFrecuenciaMedicionConNivel.filter(fmn => fmn.NivelId == nivelModel.nivelId),
    //                valueExpr: 'FrecuenciaMedicionId',
    //                displayExpr: 'Descripcion'
    //            }
    //        }, {
    //            dataField: 'SentidoId',
    //            caption: 'Sentido',
    //            lookup: {
    //                dataSource: _listSentido,
    //                valueExpr: 'ControlId',
    //                displayExpr: 'Valor'
    //            }
    //        }, {
    //            width: 100,
    //            caption: 'Acciones',
    //            fixed: true,
    //            fixedPosition: 'right',
    //            type: 'buttons',
    //            buttons: nivelModel.nivelId == ControlMaestroMapeo.Nivel.ACTIVIDAD ?
    //                [
    //                    {
    //                        hint: 'Agregar',
    //                        icon: 'add',
    //                        onClick: onClickVerActividad
    //                    }
    //                ]
    //                :
    //                [
    //                    {
    //                        hint: 'Ver',
    //                        icon: 'doc',
    //                        onClick: (event) => onClickEditarNivel(event, nivel),
    //                        visible: esModoLectura
    //                    },{
    //                        hint: 'Editar',
    //                        icon: 'edit',
    //                        onClick: (event) => onClickEditarNivel(event, nivel),
    //                        visible: !esModoLectura
    //                    }, {
    //                        hint: 'Eliminar',
    //                        icon: 'trash',
    //                        onClick: (event) => onClickEliminarNivel(event, nivel),
    //                        visible: !esModoLectura
    //                    }
    //                ]

    //        }
    //    ]
    //}).dxDataGrid('instance');
}

onInitializedDataGridContenido = (event) => {
    dxDataGridContenido = event.component;
    setTimeout(() => {
        var dataSourcePorEjercer = new DevExpress.data.DataSource({
            store: new DevExpress.data.ArrayStore({
                data: listMatrizIndicadorResultadoIndicador,
                key: 'MIRIndicadorId'
            }),
            filter: [['EstatusId', '=', ControlMaestroMapeo.EstatusRegistro.ACTIVO], 'and', ['NivelIndicadorId', '=', nivelModel.nivelId == ControlMaestroMapeo.Nivel.ACTIVIDAD ? nivelModel.nivelIdComponente : nivelModel.nivelId]]
        });
        dxDataGridContenido.option('dataSource', dataSourcePorEjercer);
    }, 0);
}
// Carga el contenido de DataGrid Actividad
cargaContenidoDataGridActividad = () => {
    // Limpiamos el contenido
    contenido.empty();
    // Obtener nivel
    //nivelModel = obtenerNivel(nivel);

    contenido.append($('<div/>', {
        class: 'row'
    }).append($('<div/>', {
        class: 'col-12'
    }).append($('<h2/>', {
        class: 'text-center',
        text: 'Actividad'
    })).append($('<div/>', {
        class: 'd-flex flex-column flex-sm-column flex-md-row flex-lg-row flex-xl-row justify-content-end'
    }).append($('<div/>', {
        id: 'dxButtonPrevious'
    }))))).append($('<div/>', {
        class: 'row'
    }).append($('<div/>', {
        class: 'col-12'
    }).append($('<div/>', {
        id: 'dxFormContenido'
    }))));
    // Button
    $('#dxButtonPrevious').dxButton({
        elementAttr: { class: 'btn-primary btn-oblong btn-width-100 ml-auto ml-sm-auto ml-md-0 ml-lg-0 ml-xl-0 mr-auto mr-sm-auto mr-md-0 mr-lg-0 mr-xl-0 mb-2 mb-sm-2 mb-md-0 mb-lg-0 mb-xl-0' },
        text: "Anterior",
        onClick: (event) => {
            esActividad = false;
            // Habilitamos los botones de acciones
            habilitaComponentes(true);

            onClickAnterior(event, seleccionModel.actual)
            //siguientePestania();
        }
    });
    // Form
    dxFormContenido = $('#dxFormContenido').dxForm({
        labelLocation: 'top',
        items: [
            {
                itemType: 'group',
                colCount: 12,
                caption: 'Datos del Componente',
                name: 'DatosComponente',
                items: [
                    {
                        itemType: 'group',
                        colSpan: 12,
                        template: () => {
                            return $('<div/>').append($('<table/>', {
                                id: 'tablaNivelActividadComponente',
                                class: 'table table-bordered table-colored table-dark'
                            }).append($('<tbody/>').ready(() => { cargaTablaNivelActividadComponente(); })));
                        }
                    }
                ]
            }, {
                itemType: 'group',
                colCount: 12,
                items: [
                    {
                        itemType: 'group',
                        colSpan: 12,
                        template: () => {
                            return $('<div/>', {
                                class: 'd-flex flex-column flex-sm-column flex-md-row flex-lg-row flex-xl-row justify-content-end'
                            }).append($('<div/>', {
                                id: 'dxButtonAdd'
                            }).ready(() => {
                                // Button
                                if (!esModoLectura) {
                                    $('#dxButtonAdd').dxButton({
                                        elementAttr: { class: 'btn-primary btn-oblong btn-width-100 ml-auto ml-sm-auto ml-md-0 ml-lg-0 ml-xl-0 mr-auto mr-sm-auto mr-md-3 mr-lg-3 mr-xl-3 mb-2 mb-sm-2 mb-md-0 mb-lg-0 mb-xl-0' },
                                        text: "Agregar",
                                        onClick: () => onClickAgregarNivel('actividad')
                                    });
                                }
                            }));
                        }
                    }
                ]
            }, {
                itemType: 'group',
                colCount: 12,
                caption: nivelModel.caption,
                name: nivelModel.name,
                items: [
                    {
                        itemType: 'group',
                        colSpan: 12,
                        template: () => {
                            return $('<div/>', {
                                id: 'dxDataGridContenido'
                            }).dxDataGrid({
                                dataSource: {
                                    store: new DevExpress.data.ArrayStore({
                                        data: listMatrizIndicadorResultadoIndicador,
                                        key: 'MIRIndicadorId'
                                    }),
                                    filter: [['EstatusId', '=', ControlMaestroMapeo.EstatusRegistro.ACTIVO], 'and', ['NivelIndicadorId', '=', ControlMaestroMapeo.Nivel.ACTIVIDAD], 'and', ['MIRIndicadorComponenteId', '=', globalMatrizIndicadorResultadoIndicador.MIRIndicadorId]]
                                },
                                onInitialized: onInitializedDataGridActividad,
                                showBorders: true,
                                columnAutoWidth: true,
                                allowColumnResizing: true,
                                columnResizingMode: 'widget',
                                rowAlternationEnabled: true,
                                noDataText: 'Sin registros',
                                paging: {
                                    pageSize: 5
                                },
                                pager: {
                                    showPageSizeSelector: true,
                                    allowedPageSizes: [5, 10, 20, 40, 60],
                                    showInfo: true,
                                    infoText: 'Página {0} de {1} ( {2} Registros )'
                                },
                                searchPanel: {
                                    placeholder: 'Buscar...',
                                    searchVisibleColumnsOnly: true,
                                    highlightSearchText: false,
                                    visible: true,
                                    width: 200
                                },
                                columns: [
                                    {
                                        dataField: 'Codigo',
                                        caption: 'Código',
                                        calculateCellValue: calculateCellValueCodigo
                                    }, {
                                        dataField: 'NivelIndicadorId',
                                        caption: 'Nivel',
                                        lookup: {
                                            dataSource: _listNivel,
                                            valueExpr: 'ControlId',
                                            displayExpr: 'Valor'
                                        }
                                    }, {
                                        dataField: 'NombreIndicador',
                                        caption: 'Nombre del Indicador'
                                    }, {
                                        dataField: 'FrecuenciaMedicionId',
                                        caption: 'Frecuencia de medición',
                                        lookup: {
                                            dataSource: _listControlMaestroFrecuenciaMedicionConNivel.filter(fmn => fmn.NivelId == nivelModel.nivelId),
                                            valueExpr: 'FrecuenciaMedicionId',
                                            displayExpr: 'Descripcion'
                                        }
                                    }, {
                                        dataField: 'SentidoId',
                                        caption: 'Sentido',
                                        lookup: {
                                            dataSource: _listSentido,
                                            valueExpr: 'ControlId',
                                            displayExpr: 'Valor'
                                        }
                                    }, {
                                        width: 100,
                                        caption: 'Acciones',
                                        fixed: true,
                                        fixedPosition: 'right',
                                        type: 'buttons',
                                        buttons: [
                                            {
                                                hint: 'Ver',
                                                icon: 'doc',
                                                onClick: (event) => onClickEditarNivel(event, 'actividad'),
                                                visible: esModoLectura
                                            },
                                            {
                                                hint: 'Editar',
                                                icon: 'edit',
                                                onClick: (event) => onClickEditarNivel(event, 'actividad'),
                                                visible: !esModoLectura
                                            }, {
                                                hint: 'Eliminar',
                                                icon: 'trash',
                                                onClick: (event) => onClickEliminarNivel(event, 'actividad'),
                                                visible: !esModoLectura
                                            }
                                        ]
                                    }
                                ]
                            });
                        }
                    }
                ]
            }
        ]
    }).dxForm('instance');
    // Carga la tabla de Nivel Actividad Componente en Datos del Componente
    //cargaTablaNivelActividadComponente();
    

    // DataGrid
    //dxDataGridContenido = $('#dxDataGridContenido').dxDataGrid({
    //    dataSource: {
    //        store: new DevExpress.data.ArrayStore({
    //            data: listMatrizIndicadorResultadoIndicador,
    //            key: 'MIRIndicadorId'
    //        }),
    //        filter: [['EstatusId', '=', ControlMaestroMapeo.EstatusRegistro.ACTIVO], 'and', ['NivelIndicadorId', '=', ControlMaestroMapeo.Nivel.ACTIVIDAD], 'and', ['MIRIndicadorComponenteId', '=', globalMatrizIndicadorResultadoIndicador.MIRIndicadorId]]
    //    },
    //    showBorders: true,
    //    columnAutoWidth: true,
    //    allowColumnResizing: true,
    //    columnResizingMode: 'widget',
    //    rowAlternationEnabled: true,
    //    noDataText: 'Sin registros',
    //    paging: {
    //        pageSize: 5
    //    },
    //    pager: {
    //        showPageSizeSelector: true,
    //        allowedPageSizes: [5, 10, 20, 40, 60],
    //        showInfo: true,
    //        infoText: 'Página {0} de {1} ( {2} Registros )'
    //    },
    //    searchPanel: {
    //        placeholder: 'Buscar...',
    //        searchVisibleColumnsOnly: true,
    //        highlightSearchText: false,
    //        visible: true,
    //        width: 200
    //    },
    //    columns: [
    //        {
    //            dataField: 'Codigo',
    //            caption: 'Código',
    //            calculateCellValue: calculateCellValueCodigo
    //        }, {
    //            dataField: 'NivelIndicadorId',
    //            caption: 'Nivel',
    //            lookup: {
    //                dataSource: _listNivel,
    //                valueExpr: 'ControlId',
    //                displayExpr: 'Valor'
    //            }
    //        }, {
    //            dataField: 'NombreIndicador',
    //            caption: 'Nombre del Indicador'
    //        }, {
    //            dataField: 'FrecuenciaMedicionId',
    //            caption: 'Frecuencia de medición',
    //            lookup: {
    //                dataSource: _listControlMaestroFrecuenciaMedicionConNivel.filter(fmn => fmn.NivelId == nivelModel.nivelId),
    //                valueExpr: 'FrecuenciaMedicionId',
    //                displayExpr: 'Descripcion'
    //            }
    //        }, {
    //            dataField: 'SentidoId',
    //            caption: 'Sentido',
    //            lookup: {
    //                dataSource: _listSentido,
    //                valueExpr: 'ControlId',
    //                displayExpr: 'Valor'
    //            }
    //        }, {
    //            width: 100,
    //            caption: 'Acciones',
    //            fixed: true,
    //            fixedPosition: 'right',
    //            type: 'buttons',
    //            buttons: [
    //                {
    //                    hint: 'Ver',
    //                    icon: 'doc',
    //                    onClick: (event) => onClickEditarNivel(event, 'actividad'),
    //                    visible: esModoLectura
    //                },
    //                {
    //                    hint: 'Editar',
    //                    icon: 'edit',
    //                    onClick: (event) => onClickEditarNivel(event, 'actividad'),
    //                    visible: !esModoLectura
    //                }, {
    //                    hint: 'Eliminar',
    //                    icon: 'trash',
    //                    onClick: (event) => onClickEliminarNivel(event, 'actividad'),
    //                    visible: !esModoLectura
    //                }
    //            ]
    //        }
    //    ]
    //}).dxDataGrid('instance');
}

onInitializedDataGridActividad = (event) => {
    // Carga la tabla de Nivel Actividad Componente en Datos del Componente
    //cargaTablaNivelActividadComponente();

    dxDataGridContenido = event.component;
    setTimeout(() => {
        var dataSourcePorEjercer = new DevExpress.data.DataSource({
            store: new DevExpress.data.ArrayStore({
                data: listMatrizIndicadorResultadoIndicador,
                key: 'MIRIndicadorId'
            }),
            filter: [['EstatusId', '=', ControlMaestroMapeo.EstatusRegistro.ACTIVO], 'and', ['NivelIndicadorId', '=', ControlMaestroMapeo.Nivel.ACTIVIDAD], 'and', ['MIRIndicadorComponenteId', '=', globalMatrizIndicadorResultadoIndicador.MIRIndicadorId]]
        });
        dxDataGridContenido.option('dataSource', dataSourcePorEjercer);
    }, 0);
}

// Carga el contenido de formulario
cargaContenidoFormulario = (nivel) => {
    // Cambiamos el valor de formulario
    esFormulario = true;
    // Limpiamos el contenido
    contenido.empty();
    // Obtener nivel
    nivelModel = obtenerNivel(nivel);

    contenido.append($('<div/>', {
        class: 'row'
    }).append($('<div/>', {
        class: 'col-12'
    }).append($('<h2/>', {
        class: 'text-center',
        text: nivelModel.caption
    })).append($('<div/>', {
        class: 'd-flex flex-column flex-sm-column flex-md-row flex-lg-row flex-xl-row justify-content-end'
    }).append($('<div/>', {
        id: 'dxButtonCancel'
    })).append($('<div/>', {
        id: 'dxButtonSave'
    }))))).append($('<div/>', {
        class: 'row'
    }).append($('<div/>', {
        class: 'col-12'
    }).append($('<div/>', {
        id: 'dxFormContenido'
    }))));
    // Button
    $('#dxButtonCancel').dxButton({
        elementAttr: { class: 'btn-normal btn-oblong btn-width-100 ml-auto ml-sm-auto ml-md-0 ml-lg-0 ml-xl-0 mr-auto mr-sm-auto mr-md-3 mr-lg-3 mr-xl-3 mb-2 mb-sm-2 mb-md-0 mb-lg-0 mb-xl-0' },
        text: esModoLectura ? 'Anterior' : 'Cancelar',
        onClick: () => esModoLectura ? onClickModalDeshacerNivel() : modalConfirmaDeshacerNivel.modal('show')
    });
    if (!esModoLectura) {
        $('#dxButtonSave').dxButton({
            elementAttr: { class: 'btn-success btn-oblong btn-width-100 ml-auto ml-sm-auto ml-md-0 ml-lg-0 ml-xl-0 mr-auto mr-sm-auto mr-md-0 mr-lg-0 mr-xl-0 mb-2 mb-sm-2 mb-md-0 mb-lg-0 mb-xl-0' },
            text: "Guardar",
            onClick: () => guardaNivel(nivel)
        });
    }
    // Form
    dxFormContenido = $('#dxFormContenido').dxForm({
        labelLocation: 'top',
        formData: $.extend(true, {}, _matrizIndicadorResultadoIndicadorModel),
        requiredMark: '(*)',
        readOnly: esModoLectura,
        //customizeItem: customizeItemFormContenido,
        items: obtenerItemsNivel(nivel)
    }).dxForm('instance');
}
//customizeItemFormContenido = (event) => {
//    //console.log(event);
//    if (event.validationRules)
//        event.validationRules[0].dataField = event.dataField;
//    event.editorOptions = { ...event.editorOptions, holamundo: event.dataField };
//}
// Carga el contenido de forulario actividad
cargaContenidoFormularioActividad = (nivel) => {
    // Cambiamos el valor de formulario
    esFormulario = true;
    // Limpiamos el contenido
    contenido.empty();
    // Obtener nivel
    nivelModel = obtenerNivel(nivel);

    contenido.append($('<div/>', {
        class: 'row'
    }).append($('<div/>', {
        class: 'col-12'
    }).append($('<h2/>', {
        class: 'text-center',
        text: nivelModel.caption
    })).append($('<div/>', {
        class: 'd-flex flex-column flex-sm-column flex-md-row flex-lg-row flex-xl-row justify-content-end'
    }).append($('<div/>', {
        id: 'dxButtonCancel'
    })).append($('<div/>', {
        id: 'dxButtonSave'
    }))))).append($('<div/>', {
        class: 'row'
    }).append($('<div/>', {
        class: 'col-12'
    }).append($('<div/>', {
        id: 'dxFormContenido'
    }))));
    // Button
    $('#dxButtonCancel').dxButton({
        elementAttr: { class: 'btn-normal btn-oblong btn-width-100 ml-auto ml-sm-auto ml-md-0 ml-lg-0 ml-xl-0 mr-auto mr-sm-auto mr-md-3 mr-lg-3 mr-xl-3 mb-2 mb-sm-2 mb-md-0 mb-lg-0 mb-xl-0' },
        text: esModoLectura ? 'Anterior' : 'Cancelar',
        onClick: () => esModoLectura ? onClickModalDeshacerNivel() : modalConfirmaDeshacerNivel.modal('show')
    });
    if (!esModoLectura) {
        $('#dxButtonSave').dxButton({
            elementAttr: { class: 'btn-success btn-oblong btn-width-100 ml-auto ml-sm-auto ml-md-0 ml-lg-0 ml-xl-0 mr-auto mr-sm-auto mr-md-0 mr-lg-0 mr-xl-0 mb-2 mb-sm-2 mb-md-0 mb-lg-0 mb-xl-0' },
            text: "Guardar",
            onClick: () => guardaNivel(nivel)
        });
    }
    // Form
    dxFormContenido = $('#dxFormContenido').dxForm({
        labelLocation: 'top',
        formData: $.extend(true, {}, _matrizIndicadorResultadoIndicadorModel),
        requiredMark: '(*)',
        readOnly: esModoLectura,
        items: obtenerItemsNivel(nivel)
    }).dxForm('instance');
    // Carga la tabla de Nivel Actividad Componente en Datos del Componente
    //cargaTablaNivelActividadComponente();
}
// Carga el contenido de resumen
cargaContenidoResumen = () => {
    // Limpiamos el contenido
    contenido.empty();

    contenido.append($('<div/>', {
        class: 'row'
    }).append($('<div/>', {
        class: 'col-12'
    }).append($('<h2/>', {
        class: 'text-center',
        text: 'Nueva Matriz de Indicadores para Resultados'
    })).append($('<div/>', {
        class: 'd-flex flex-column flex-sm-column flex-md-row flex-lg-row flex-xl-row justify-content-end'
    }).append($('<div/>', {
        id: 'dxButtonPrevious'
    })).append($('<div/>', {
        id: 'dxButtonEnd'
    }))))).append($('<div/>', {
        class: 'row'
    }).append($('<div/>', {
        class: 'col-12'
    }).append($('<div/>', {
        id: 'dxFormContenido'
    }))));
    // Button
    $('#dxButtonPrevious').dxButton({
        elementAttr: { class: 'btn-primary btn-oblong btn-width-100 ml-auto mr-3 mr-sm-3 mr-md-3 mr-lg-3 mr-xl-3' },
        text: "Anterior",
        onClick: (event) => onClickAnterior(event, seleccionModel.actual - 1)
    });
    if (!esModoLectura) {
        $('#dxButtonEnd').dxButton({
            elementAttr: { class: 'btn-primary btn-oblong btn-width-100 mt-0 mt-sm-0 mt-md-0 mt-lg-0 mt-xl-0' },
            text: "Finalizar",
            onClick: () => guardaCambios()
        });
    }
    // Form
    dxFormContenido = $('#dxFormContenido').dxForm({
        labelLocation: 'top',
        items: [
            {
                itemType: 'group',
                colCount: 12,
                caption: 'Resumen',
                name: 'Resumen',
                items: [
                    {
                        itemType: 'group',
                        colSpan: 12,
                        template: () => {
                            return $('<div/>', {
                                id: 'dxDataGridContenido'
                            }).dxDataGrid({
                                selection: {
                                    mode: 'sigle'
                                },
                                onInitialized: onInitializedDataGridResumen,
                                showBorders: true,
                                columnAutoWidth: true,
                                allowColumnResizing: true,
                                columnResizingMode: 'widget',
                                rowAlternationEnabled: true,
                                noDataText: 'Sin registros',
                                paging: {
                                    pageSize: 10
                                },
                                pager: {
                                    showPageSizeSelector: true,
                                    allowedPageSizes: [5, 10, 20, 40, 60],
                                    showInfo: true,
                                    infoText: 'Página {0} de {1} ( {2} Registros )'
                                },
                                searchPanel: {
                                    placeholder: 'Buscar...',
                                    searchVisibleColumnsOnly: true,
                                    highlightSearchText: false,
                                    visible: true,
                                    width: 200
                                },
                                columns: [
                                    {
                                        dataField: 'Componente',
                                        groupIndex: 0,
                                        //groupCellTemplate: groupCellTemplateResumen
                                    }, {
                                        dataField: 'Actividad'
                                    }, {
                                        dataField: 'NombreProyecto',
                                        caption: 'Relación Presupuestal',
                                        cellTemplate: cellTemplateResumen
                                    }, {
                                        dataField: 'PorcentajeProyecto',
                                        caption: 'Porcentaje Proyecto para Objetivo',
                                        format: "#0.00 '%'",
                                        alignment: 'right',
                                        calculateCellValue: calculateCellValueResumenPorcentajeProyecto
                                    }, {
                                        dataField: 'PorcentajeActividad',
                                        caption: 'Porcentaje Componente para Actividades',
                                        format: "#0.00 '%'",
                                        alignment: 'right',
                                    }, {
                                        dataField: 'Monto',
                                        caption: 'Monto',
                                        format: "$ #,##0.00",
                                        alignment: 'right',
                                    }, esModoLectura ? null : {
                                        width: 100,
                                        caption: 'Acciones',
                                        fixed: true,
                                        fixedPosition: 'right',
                                        type: 'buttons',
                                        buttons: [
                                            {
                                                hint: 'Editar',
                                                icon: 'edit',
                                                onClick: onClickEditarResumen
                                            }
                                        ]
                                    }
                                ],
                                summary: {
                                    groupItems: [
                                        {
                                            showInColumn: 'PorcentajeProyecto',
                                            name: 'PorcentajeProyecto',
                                            summaryType: 'custom',
                                            showInGroupFooter: false,
                                            alignByColumn: true,
                                            valueFormat: "#0.00 '%'"
                                        }, {
                                            column: 'PorcentajeActividad',
                                            summaryType: 'sum',
                                            showInGroupFooter: false,
                                            alignByColumn: true,
                                            displayFormat: '{0}',
                                            valueFormat: "#0.00 '%'"
                                        }, {
                                            column: 'Monto',
                                            summaryType: 'sum',
                                            showInGroupFooter: false,
                                            alignByColumn: true,
                                            displayFormat: '{0}',
                                            valueFormat: "$ #,##0.00"
                                        }
                                    ],
                                    calculateCustomSummary: calculateCustomSummaryResumen
                                }
                            });
                        }
                    }
                ]

            }
        ]
    }).dxForm('instance');
    // DataGrid
    //dxDataGridContenido = $('#dxDataGridContenido').dxDataGrid({
    //    selection: {
    //        mode: 'sigle'
    //    },
    //    showBorders: true,
    //    columnAutoWidth: true,
    //    allowColumnResizing: true,
    //    columnResizingMode: 'widget',
    //    rowAlternationEnabled: true,
    //    noDataText: 'Sin registros',
    //    paging: {
    //        pageSize: 10
    //    },
    //    pager: {
    //        showPageSizeSelector: true,
    //        allowedPageSizes: [5, 10, 20, 40, 60],
    //        showInfo: true,
    //        infoText: 'Página {0} de {1} ( {2} Registros )'
    //    },
    //    searchPanel: {
    //        placeholder: 'Buscar...',
    //        searchVisibleColumnsOnly: true,
    //        highlightSearchText: false,
    //        visible: true,
    //        width: 200
    //    },
    //    columns: [
    //        {
    //            dataField: 'Componente',
    //            groupIndex: 0,
    //            groupCellTemplate: groupCellTemplateResumen
    //        }, {
    //            dataField: 'Actividad'
    //        }, {
    //            dataField: 'NombreProyecto',
    //            caption: 'Relación Presupuestal',
    //            cellTemplate: cellTemplateResumen
    //        }, {
    //            dataField: 'PorcentajeProyecto',
    //            caption: 'Porcentaje Proyecto para Objetivo',
    //            format: "#0.00 '%'",
    //            alignment: 'right',
    //            calculateCellValue: calculateCellValueResumenPorcentajeProyecto
    //        }, {
    //            dataField: 'PorcentajeActividad',
    //            caption: 'Porcentaje Componente para Actividades',
    //            format: "#0.00 '%'",
    //            alignment: 'right',
    //        }, {
    //            dataField: 'Monto',
    //            caption: 'Monto',
    //            format: "$ #,##0.00",
    //            alignment: 'right',
    //        }, esModoLectura ? null : {
    //            width: 100,
    //            caption: 'Acciones',
    //            fixed: true,
    //            fixedPosition: 'right',
    //            type: 'buttons',
    //            buttons: [
    //                {
    //                    hint: 'Editar',
    //                    icon: 'edit',
    //                    onClick: onClickEditarResumen
    //                }
    //            ]
    //        }
    //    ],
    //    summary: {
    //        groupItems: [
    //            {
    //                showInColumn: 'PorcentajeProyecto',
    //                name: 'PorcentajeProyecto',
    //                summaryType: 'custom',
    //                showInGroupFooter: false,
    //                alignByColumn: true,
    //                valueFormat: "#0.00 '%'"
    //            }, {
    //                column: 'PorcentajeActividad',
    //                summaryType: 'sum',
    //                showInGroupFooter: false,
    //                alignByColumn: true,
    //                displayFormat: '{0}',
    //                valueFormat: "#0.00 '%'"
    //            }, {
    //                column: 'Monto',
    //                summaryType: 'sum',
    //                showInGroupFooter: false,
    //                alignByColumn: true,
    //                displayFormat: '{0}',
    //                valueFormat: "$ #,##0.00"
    //            }
    //        ],
    //        calculateCustomSummary: calculateCustomSummaryResumen
    //    }
    //}).dxDataGrid('instance');
}

onInitializedDataGridResumen = (event) => {
    dxDataGridContenido = event.component;
    setTimeout(() => {
        cargaResumen();
    }, 0);
}

obtenerNivel = (nivel) => {
    switch (nivel) {
        case 'fin':
            return {
                caption: 'Fin',
                name: 'NivelFin',
                nivelId: ControlMaestroMapeo.Nivel.FIN
            };
        case 'proposito':
            return {
                caption: 'Propósito',
                name: 'NivelProposito',
                nivelId: ControlMaestroMapeo.Nivel.PROPOSITO
            };
        case 'componente':
            return {
                caption: 'Componente',
                name: 'NivelComponente',
                nivelId: ControlMaestroMapeo.Nivel.COMPONENTE
            };
        case 'actividad':
            return {
                caption: 'Actividad',
                name: 'NivelActividad',
                nivelId: ControlMaestroMapeo.Nivel.ACTIVIDAD,
                nivelIdComponente: ControlMaestroMapeo.Nivel.COMPONENTE
            };
    }
}

obtenerItemsNivel = (nivel) => {
    let extraTipoComponente = {
        visible: false
    },
        extraProyecto = {
            visible: false
        },
        extraDatosComponente = {
            visible: false
        },
        extraActividadRelacionComponente = {
            visible: false
        };

    if (nivel == 'componente') {
        extraTipoComponente =
        {
            dataField: 'TipoComponenteId',
            label: {
                text: 'Tipo de componente'
            },
            colSpan: 12,
            editorType: 'dxRadioGroup',
            editorOptions: {
                dataSource: {
                    store: new DevExpress.data.ArrayStore({
                        data: _listTipoComponente,
                        key: 'ControlId'
                    })
                },
                displayExpr: 'Valor',
                valueExpr: 'ControlId',
                layout: 'horizontal',
                onValueChanged: onValueChangedTipoComponente
            },
            validationRules: [
                {
                    type: 'required',
                    message: 'Tipo de componente requerido'
                }
            ]
        }
    }

    if (nivel == 'componente' || (nivel == 'actividad' ? nivel == 'actividad' && globalMatrizIndicadorResultadoIndicador.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_ACTIVIDAD : false)) {
        extraProyecto =
        {
            itemType: 'group',
            colSpan: 12,
            colCount: 12,
            name: 'DatosIndicadorGrupoProyecto',
            visible: esActividad ? globalMatrizIndicadorResultadoIndicador.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_ACTIVIDAD ? _esPermisoProyecto ? true : false : false : _esPermisoProyecto ? true : false,
            items: [
                {
                    dataField: 'ProyectoId',
                    label: {
                        text: 'Proyecto'
                    },
                    colSpan: 12,
                    editorType: 'dxSelectBox',
                    editorOptions: {
                        dataSource: {
                            store: _listProyecto,
                            paginate: true,
                            pageSize: 10
                        },
                        displayExpr: (event) => { return event ? event.ProyectoId + ' - ' + event.Nombre : ''; },
                        valueExpr: 'ProyectoId',
                        searchEnabled: true,
                        searchMode: 'contains',
                        showClearButton: true,
                        noDataText: 'Sin registros que mostrar',
                        placeholder: 'Selecciona una opción',
                        onValueChanged: (event) => onValueChangedProyecto(event, nivel)
                    },
                    validationRules: [
                        {
                            type: 'required',
                            message: 'Proyecto requerido'
                        }
                    ]
                }, {
                    dataField: 'PorcentajeProyecto',
                    label: {
                        text: 'Porcentaje Proyecto'
                    },
                    colSpan: 6,
                    editorType: 'dxNumberBox',
                    editorOptions: {
                        format: "#0.## '%'",
                        elementAttr: {
                            class: 'text-right'
                        },
                        onValueChanged: (event) => onValueChangedPorcentajeProyecto(event, nivel)
                    },
                    validationRules: [
                        {
                            type: 'required',
                            message: 'Porcentaje Proyecto requerido'
                        },
                        {
                            type: 'range',
                            min: 0,
                            max: 100,
                            message: 'El porcentaje debe ser 0 al 100%'
                        }
                    ]
                }, {
                    dataField: 'MontoProyecto',
                    label: {
                        text: 'Monto Total Proyecto'
                    },
                    colSpan: 6,
                    editorType: 'dxNumberBox',
                    editorOptions: {
                        format: "$ #,##0.00",
                        elementAttr: {
                            class: 'text-right'
                        },
                        readOnly: true
                    },
                    validationRules: [
                        {
                            type: 'required',
                            message: 'Monto Total Proyecto requerido'
                        }
                    ]
                }
            ]
        }
    }
    if (nivel == 'actividad') {
        extraDatosComponente =
        {
            itemType: 'group',
            colCount: 12,
            caption: 'Datos del Componente',
            name: 'DatosComponente',
            items: [
                {
                    itemType: 'simple',
                    colSpan: 12,
                    template: () => {
                        return $('<div/>').append($('<table/>', {
                            id: 'tablaNivelActividadComponente',
                            class: 'table table-bordered table-colored table-dark'
                        }).append($('<tbody/>').ready(() => { cargaTablaNivelActividadComponente(); })));
                    }
                }
            ]
        }

        if (globalMatrizIndicadorResultadoIndicador.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_COMPONENTE) {
            extraActividadRelacionComponente = {
                itemType: 'group',
                colSpan: 12,
                colCount: 12,
                name: 'DatosIndicadorGrupoProyecto',
                items: [
                    {
                        dataField: 'PorcentajeActividad',
                        label: {
                            text: 'Porcentaje Actividad'
                        },
                        colSpan: 4,
                        editorType: 'dxNumberBox',
                        editorOptions: {
                            format: "#0.## '%'",
                            elementAttr: {
                                class: 'text-right'
                            },
                            onValueChanged: onValueChangedPorcentajeActividad,
                            onFocusIn: onFocusInPorcentajeActividad
                        },
                        validationRules: [
                            {
                                type: 'required',
                                message: 'Porcentaje Actividad requerido'
                            },
                            {
                                type: 'range',
                                min: 0,
                                max: 100,
                                message: 'El porcentaje debe ser 0 al 100%'
                            }
                        ]
                    }, {
                        dataField: 'MontoActividad',
                        label: {
                            text: 'Monto Actividad'
                        },
                        colSpan: 4,
                        editorType: 'dxNumberBox',
                        editorOptions: {
                            format: "$ #,##0.00",
                            elementAttr: {
                                class: 'text-right'
                            },
                            readOnly: true
                        },
                        validationRules: [
                            {
                                type: 'required',
                                message: 'Monto Actividad requerido'
                            }
                        ]
                    }, {
                        dataField: 'NombreIndicador',
                        label: {
                            text: 'Nombre del Indicador'
                        },
                        colSpan: 4,
                        editorType: 'dxTextBox',
                        validationRules: [
                            {
                                type: 'required',
                                message: 'Nombre del Indicador requerido'
                            },
                            {
                                type: 'stringLength',
                                max: 500,
                                message: 'La máxima de 500 caracteres'
                            }
                        ],
                    }
                ]
            }
        }
    }
    return [
        extraDatosComponente,
        {
            itemType: 'group',
            colCount: 12,
            caption: 'Datos del indicador',
            name: 'DatosIndicador',
            items: [
                extraTipoComponente,
                extraProyecto,
                extraActividadRelacionComponente,
                {
                    dataField: 'NombreIndicador',
                    label: {
                        text: 'Nombre del Indicador'
                    },
                    colSpan: 12,
                    editorType: 'dxTextBox',
                    validationRules: [
                        {
                            type: 'required',
                            message: 'Nombre del Indicador requerido'
                        },
                        {
                            type: 'stringLength',
                            max: 500,
                            message: 'La máxima de 500 caracteres'
                        }
                    ],
                    visible: esActividad ? globalMatrizIndicadorResultadoIndicador.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_COMPONENTE ? false : true : true
                }, {
                    dataField: 'ResumenNarrativo',
                    label: {
                        text: 'Resumen Narrativo'
                    },
                    colSpan: 6,
                    editorType: 'dxTextArea',
                    editorOptions: {
                        height: 100
                    },
                    validationRules: [
                        {
                            type: 'required',
                            message: 'Resumen Narrativo requerido'
                        }
                    ]
                }, {
                    dataField: 'DefinicionIndicador',
                    label: {
                        text: 'Definición del indicador'
                    },
                    colSpan: 6,
                    editorType: 'dxTextArea',
                    editorOptions: {
                        height: 100
                    },
                    validationRules: [
                        {
                            type: 'required',
                            message: 'Defeinición del indicador requerido'
                        }
                    ]
                }, {
                    dataField: 'TipoIndicadorId',
                    label: {
                        text: 'Tipo de indicador'
                    },
                    colSpan: 4,
                    editorType: 'dxSelectBox',
                    editorOptions: {
                        dataSource: {
                            store: _listControlMaestroTipoIndicadorConNivel,
                            filter: ['NivelId', '=', nivelModel.nivelId]
                        },
                        displayExpr: "Descripcion",
                        valueExpr: 'TipoIndicadorId',
                        searchEnabled: true,
                        searchMode: 'contains',
                        showClearButton: true,
                        noDataText: 'Sin registros que mostrar',
                        placeholder: 'Selecciona una opción'
                    },
                    validationRules: [
                        {
                            type: 'required',
                            message: 'Tipo de indicador requerido'
                        }
                    ]
                }, {
                    dataField: 'DimensionId',
                    label: {
                        text: 'Dimensión'
                    },
                    colSpan: 4,
                    editorType: 'dxSelectBox',
                    editorOptions: {
                        dataSource: {
                            store: _listControlMaestroDimensionConNivel,
                            filter: ['NivelId', '=', nivelModel.nivelId]
                        },
                        displayExpr: "Descripcion",
                        valueExpr: 'DimensionId',
                        searchEnabled: true,
                        searchMode: 'contains',
                        showClearButton: true,
                        noDataText: 'Sin registros que mostrar',
                        placeholder: 'Selecciona una opción',
                        onValueChanged: (event) => onValueChangedDimension(event, nivel)
                    },
                    validationRules: [
                        {
                            type: 'required',
                            message: 'Dimensión requerido'
                        }
                    ]
                }, {
                    dataField: 'UnidadMedidaId',
                    label: {
                        text: 'Unidad de medida'
                    },
                    colSpan: 4,
                    editorType: 'dxSelectBox',
                    editorOptions: {
                        dataSource: _listControlMaestroUnidadMedidaConDimension,
                        displayExpr: "Nombre",
                        valueExpr: 'UnidadMedidaId',
                        searchEnabled: true,
                        searchMode: 'contains',
                        showClearButton: true,
                        noDataText: 'Sin registros que mostrar',
                        placeholder: 'Selecciona una opción',
                        disabled: true,
                        onValueChanged: (event) => onValueChangedUnidadMedida(event, nivel)
                    },
                    validationRules: [
                        {
                            type: 'required',
                            message: 'Unidad de medida requerido'
                        }
                    ]
                }, {
                    dataField: 'FrecuenciaMedicionId',
                    label: {
                        text: 'Frecuencia de medición'
                    },
                    colSpan: 6,
                    editorType: 'dxSelectBox',
                    editorOptions: {
                        dataSource: _listControlMaestroFrecuenciaMedicionConNivel.filter(fmn => fmn.NivelId == nivelModel.nivelId),
                        displayExpr: 'Descripcion',
                        valueExpr: 'FrecuenciaMedicionId',
                        searchEnabled: true,
                        searchMode: 'contains',
                        showClearButton: true,
                        noDataText: 'Sin registros que mostrar',
                        placeholder: 'Selecciona una opción',
                        onValueChanged: (event) => onValueChanedFrecuenciaMedicion(event, nivel)
                    },
                    validationRules: [
                        {
                            type: 'required',
                            message: 'Frecuencia de medición requerido'
                        }
                    ]
                }, {
                    dataField: 'SentidoId',
                    label: {
                        text: 'Sentido'
                    },
                    colSpan: 6,
                    editorType: 'dxSelectBox',
                    editorOptions: {
                        dataSource: _listSentido,
                        displayExpr: "Valor",
                        valueExpr: 'ControlId',
                        searchEnabled: true,
                        searchMode: 'contains',
                        showClearButton: true,
                        noDataText: 'Sin registros que mostrar',
                        placeholder: 'Selecciona una opción',
                        onValueChanged: (event) => onValueChanedSentido(event, nivel)
                    },
                    validationRules: [
                        {
                            type: 'required',
                            message: 'Sentido requerido'
                        }
                    ]
                }
            ]
        }, {
            itemType: 'group',
            colCount: 12,
            caption: 'Datos de control y cálculo',
            name: 'DatosControlCalculo',
            items: [
                {
                    itemType: 'tabbed',
                    colSpan: 12,
                    name: 'DatosControlCalculoPestania',
                    tabPanelOptions: {
                        deferRendering: false,
                        animationEnabled: true,
                        onTitleClick: (event) => onTitleClickPestania(event, nivel),
                        selectedIndex: 0,
                        onInitialized: (event) => onInitializedTabPanel(event, nivel)
                    },
                    tabs: [
                        {
                            title: 'Linea base',
                            items: [
                                {
                                    itemType: 'group',
                                    colCount: 12,
                                    cssClass: 'mb-4',
                                    items: [
                                        {
                                            dataField: 'AnioBase',
                                            label: {
                                                text: 'Año base'
                                            },
                                            colSpan: 6,
                                            editorType: 'dxTextBox',
                                            editorOptions: {
                                                mask: 'XXXX',
                                                maskRules: { X: /[0-9]/ },
                                                maskInvalidMessage: 'Dato no válido',
                                                elementAttr: {
                                                    class: 'text-right'
                                                },
                                                onChange: onChangeAnioBase
                                            },
                                            validationRules: [
                                                {
                                                    type: 'required',
                                                    message: 'Año base requerido'
                                                },
                                                {
                                                    type: 'stringLength',
                                                    max: 4,
                                                    message: 'La máxima de 4 caracteres'
                                                }
                                            ]
                                        }, {
                                            dataField: 'ValorBase',
                                            label: {
                                                text: 'Valor'
                                            },
                                            colSpan: 6,
                                            editorType: 'dxNumberBox',
                                            editorOptions: {
                                                format: '#,##0.##',
                                                elementAttr: {
                                                    class: 'text-right'
                                                },
                                                onFocusIn: (event) => onFocusInValorBase(event, nivel)
                                            },
                                            validationRules: [
                                                {
                                                    type: 'required',
                                                    message: 'Valor requerido'
                                                },
                                                {
                                                    type: 'range',
                                                    min: 0,
                                                    max: Number.MAX_SAFE_INTEGER,
                                                    message: 'El monto debe ser positivo'
                                                }
                                            ]
                                        }, {
                                            dataField: 'DescripcionBase',
                                            label: {
                                                text: 'Descripción'
                                            },
                                            colSpan: 12,
                                            editorType: 'dxTextBox',
                                            validationRules: [
                                                {
                                                    type: 'required',
                                                    message: 'Descripción requerido'
                                                },
                                                {
                                                    type: 'stringLength',
                                                    min: 0,
                                                    max: 200,
                                                    message: 'La máxima de 200 caracteres'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }, {
                            title: 'Meta',
                            items: [
                                {
                                    itemType: 'group',
                                    colCount: 12,
                                    items: [
                                        {
                                            colSpan: 12,
                                            template: () => {
                                                return $('<div>', {
                                                    id: 'dxFormMeta'
                                                });
                                            }
                                        }
                                    ]
                                }
                            ]
                        }, {
                            title: 'Semaforización',
                            items: [
                                {
                                    itemType: 'group',
                                    colCount: 12,
                                    items: [
                                        {
                                            colSpan: 2,
                                            cssClass: 'h-100 position-relative',
                                            template: () => templateEnTabDondeForm('Aceptable')
                                        }, {
                                            dataField: 'AceptableDesde',
                                            label: {
                                                text: 'Desde'
                                            },
                                            colSpan: 5,
                                            editorType: 'dxNumberBox',
                                            editorOptions: {
                                                format: "#0.## '%'",
                                                elementAttr: {
                                                    class: 'text-right'
                                                },
                                                onValueChanged: (event) => onValueChangedSemaforizacion(event, nivel, 'aceptable', false)
                                            },
                                            validationRules: [
                                                {
                                                    type: 'required',
                                                    message: 'Desde requerido'
                                                },
                                                {
                                                    type: 'range',
                                                    min: 0,
                                                    max: 100,
                                                    message: 'El porcentaje debe ser 0 al 100%'
                                                }
                                            ]
                                        }, {
                                            dataField: 'AceptableHasta',
                                            label: {
                                                text: 'Hasta'
                                            },
                                            colSpan: 5,
                                            editorType: 'dxNumberBox',
                                            editorOptions: {
                                                format: "#0.## '%'",
                                                elementAttr: {
                                                    class: 'text-right'
                                                },
                                                onValueChanged: (event) => onValueChangedSemaforizacion(event, nivel, 'aceptable', true)
                                            },
                                            validationRules: [
                                                {
                                                    type: 'required',
                                                    message: 'Hasta requerido'
                                                },
                                                {
                                                    type: 'range',
                                                    min: 0,
                                                    max: 100,
                                                    message: 'El porcentaje debe ser 0 al 100%'
                                                }
                                            ]
                                        }
                                    ]
                                }, {
                                    itemType: 'group',
                                    colCount: 12,
                                    items: [
                                        {
                                            colSpan: 2,
                                            cssClass: 'h-100 position-relative',
                                            template: () => templateEnTabDondeForm('Con Riesgo')
                                        }, {
                                            dataField: 'ConRiesgoDesde',
                                            label: {
                                                text: 'Desde'
                                            },
                                            colSpan: 5,
                                            editorType: 'dxNumberBox',
                                            editorOptions: {
                                                format: "#0.## '%'",
                                                elementAttr: {
                                                    class: 'text-right'
                                                },
                                                onValueChanged: (event) => onValueChangedSemaforizacion(event, nivel, 'conriesgo', null),
                                                onFocusIn: () => onFocusInConRiesgo(nivel)
                                            },
                                            validationRules: [
                                                {
                                                    type: 'required',
                                                    message: 'Desde requerido'
                                                },
                                                {
                                                    type: 'range',
                                                    min: 0,
                                                    max: 100,
                                                    message: 'El porcentaje debe ser 0 al 100%'
                                                }
                                            ]
                                        }, {
                                            dataField: 'ConRiesgoHasta',
                                            label: {
                                                text: 'Hasta'
                                            },
                                            colSpan: 5,
                                            editorType: 'dxNumberBox',
                                            editorOptions: {
                                                format: "#0.## '%'",
                                                elementAttr: {
                                                    class: 'text-right'
                                                },
                                                onValueChanged: (event) => onValueChangedSemaforizacion(event, nivel, 'conriesgo', null),
                                                onFocusIn: () => onFocusInConRiesgo(nivel)
                                            },
                                            validationRules: [
                                                {
                                                    type: 'required',
                                                    message: 'Hasta requerido'
                                                },
                                                {
                                                    type: 'range',
                                                    min: 0,
                                                    max: 100,
                                                    message: 'El porcentaje debe ser 0 al 100%'
                                                }
                                            ]
                                        }
                                    ]
                                }, {
                                    itemType: 'group',
                                    colCount: 12,
                                    items: [
                                        {
                                            colSpan: 2,
                                            cssClass: 'h-100 position-relative',
                                            template: () => templateEnTabDondeForm('Critico')
                                        }, {
                                            dataField: 'CriticoPorDebajo',
                                            label: {
                                                text: 'Por debajo'
                                            },
                                            colSpan: 5,
                                            editorType: 'dxNumberBox',
                                            editorOptions: {
                                                format: "#0.## '%'",
                                                elementAttr: {
                                                    class: 'text-right'
                                                },
                                                readOnly: true
                                            },
                                            validationRules: [
                                                {
                                                    type: 'required',
                                                    message: 'Por debajo requerido'
                                                },
                                                {
                                                    type: 'range',
                                                    min: 0,
                                                    max: 100,
                                                    message: 'El porcentaje debe ser 0 al 100%'
                                                }
                                            ]
                                        }, {
                                            dataField: 'CriticoPorEncima',
                                            label: {
                                                text: 'Por encima'
                                            },
                                            colSpan: 5,
                                            editorType: 'dxNumberBox',
                                            editorOptions: {
                                                format: "#0.## '%'",
                                                elementAttr: {
                                                    class: 'text-right'
                                                },
                                                readOnly: true
                                            },
                                            validationRules: [
                                                {
                                                    type: 'required',
                                                    message: 'Por encima requerido'
                                                },
                                                {
                                                    type: 'range',
                                                    min: 0,
                                                    max: 100,
                                                    message: 'El porcentaje debe ser 0 al 100%'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }, {
                            title: 'Método de cálculo',
                            items: [
                                {
                                    itemType: 'group',
                                    colCount: 12,
                                    items: [
                                        {
                                            dataField: 'FormulaId',
                                            label: {
                                                text: 'Fórmula'
                                            },
                                            colSpan: 12,
                                            editorType: 'dxSelectBox',
                                            editorOptions: {
                                                dataSource: _listControlMaestroUnidadMedidaConDimension,
                                                displayExpr: 'Nombre',
                                                valueExpr: 'UnidadMedidaId',
                                                readOnly: true
                                            },
                                            validationRules: [
                                                {
                                                    type: 'required',
                                                    message: 'Fórmula requerida'
                                                }
                                            ]
                                        }, {
                                            dataField: 'DescripcionFormula',
                                            label: {
                                                text: 'Descripción de la fórmula'
                                            },
                                            colSpan: 8,
                                            editorType: 'dxTextArea',
                                            editorOptions: {
                                                height: 100
                                            },
                                            validationRules: [
                                                {
                                                    type: 'required',
                                                    message: 'Descripción de la fórmula requerido'
                                                }
                                            ]
                                        }, {
                                            colSpan: 4,
                                            template: () => {
                                                return $('<div/>', {
                                                    id: 'dxFormFormula'
                                                });
                                            }
                                        }

                                    ]
                                }
                            ]
                        }, {
                            title: 'Fuentes y medios',
                            items: [
                                {
                                    itemType: 'group',
                                    colCount: 12,
                                    cssClass: 'mb-4',
                                    items: [
                                        {
                                            dataField: 'FuenteInformacion',
                                            label: {
                                                text: 'Fuentes de información'
                                            },
                                            colSpan: 6,
                                            editorType: 'dxTextArea',
                                            editorOptions: {
                                                height: 100
                                            },
                                            validationRules: [
                                                {
                                                    type: 'required',
                                                    message: 'Fuentes de información requerido'
                                                }
                                            ]
                                        }, {
                                            dataField: 'MedioVerificacion',
                                            label: {
                                                text: 'Medios de verificación'
                                            },
                                            colSpan: 6,
                                            editorType: 'dxTextArea',
                                            editorOptions: {
                                                height: 100
                                            },
                                            validationRules: [
                                                {
                                                    type: 'required',
                                                    message: 'Medios de verificación requerido'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ]
        }
    ]
}

onInitializedTabPanel = (event, nivel) => {
    dxTabPanel = event.component;
    setTimeout(() => {
        // Por caso, si cambia el tama;a de pantalla el contenido se vacio
        // Para cargan los formularios de Meta y Formula
        if (!$('#dxFormMeta').hasClass('dx-form') || !$('#dxFormFormula').hasClass('dx-form')) {
            var formData = dxFormContenido.option('formData');
            if (formData.FrecuenciaMedicionId != null && !$('#dxFormMeta').hasClass('dx-form')) {
                cargaTemplateMeta(formData.FrecuenciaMedicionId, nivel);
                // Carga Meta cuando el registro es editar para mostrar los campos en Meta
                cargaMeta(dxFormContenido.option('formData').MIRIndicadorId);
            }

            if (formData.UnidadMedidaId != null && !$('#dxFormFormula').hasClass('dx-form')) {
                templateFormula(formData.MIRIndicadorId, formData.UnidadMedidaId);
            }
        }
    }, 0);
}

cargarEjericio = () => {
    // Crear arreglo de fecha para ejercicio
    let ejercicio = [];
    ejercicio.push({ Ejercicio: '2021' });
    ejercicio.push({ Ejercicio: '2022' });
    ejercicio.push({ Ejercicio: '2026' });
    // Conjunto los datos de ejercicio
    var dataSource = new DevExpress.data.DataSource({
        store: {
            type: 'array',
            key: 'Ejercicio',
            data: ejercicio
        }
    });
    dxSelectBoxEjercicio.option("dataSource", dataSource);
}

cargaPlanDesarrollo = (ejercicio) => {
    // Crear arreglo de plan de desarrollo con ejercicio
    ejercicio = parseInt(ejercicio);
    let listPlanDesarrollo = _listPlanDesarrollo.filter(pnd => new Date(parseInt(pnd.FechaInicio.substr(6))).getFullYear() <= ejercicio && new Date(parseInt(pnd.FechaFin.substr(6))).getFullYear() >= ejercicio);
    // Conjunto los datos de ejercicio
    return new DevExpress.data.DataSource({
        store: {
            type: 'array',
            key: 'TipoIndicadorId',
            data: listPlanDesarrollo
        }
    });
    //dxSelectBoxPlanDesarrollo.option("dataSource", dataSource);
}

cargaPlanDesarrolloEstructura = (planDesarrolloId) => {
    // Crear arreglo de plan de desarrollo estructura con el ID de plan de desarrollo
    let listPlanDesarrolloEstructura = buscaUltimoNodoPlanDesarrolloEstructura($.extend(true, [], _listPlanDesarrolloEstructura.filter(pnde => pnde.PlanDesarrolloId == planDesarrolloId)));
    // Conjunto los datos de ejercicio
    return new DevExpress.data.DataSource({
        store: {
            type: 'array',
            key: 'PlanDesarrolloEstructuraId',
            data: listPlanDesarrolloEstructura
        }
    });
    //dxDropDownBoxPlanDesarrolloEstructura.option("dataSource", dataSource);
}

// Carga Proyecto
cargaProducto = () => {
    if (matrizIndicadorResultado.Ejercicio != '' && matrizIndicadorResultado.ProgramaPresupuestarioId != '') {
        dxLoaderPanel.show();
        $.ajax({
            type: 'POST',
            url: API_FICHA + 'obtenerproyectos',
            data: { ejercicio: matrizIndicadorResultado.Ejercicio, programaPresupuestarioId: matrizIndicadorResultado.ProgramaPresupuestarioId },
            success: function (response) {
                // Establecer lista de proyecto
                _listProyecto = response;
                // Ocultamos Loader
                dxLoaderPanel.hide();
            },
            error: function (response, status, error) {
                // Ocultamos Loader
                dxLoaderPanel.hide();
                // Mostramos mensaje de error
                //toast(response.responseText, "error");
                toast('No se puede cargar los proyectos, inténtalo de nuevo más tarde', "error");
            }
        });
    } else {
        _listProyecto = [];
    }
}

// Carga los proyectos con componentes / actividades de 100%, ya no mostrarlo en el combo de proyecto
cargaProyectoPorPorcentaje = () => {
    _listProyecto.map(proyecto => {
        let _proyectoPorcentajeTotal = 0;
        // Relacion Componente y Actividad -> Proyecto
        listMatrizIndicadorResultadoIndicador.filter(miri => miri.ProyectoId == proyecto.ProyectoId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).map(miri => _proyectoPorcentajeTotal += miri.PorcentajeProyecto);
        if (_proyectoPorcentajeTotal >= 100) {
            proyecto.visible = false;
        } else {
            proyecto.visible = true;
        }
    });
}
// Carga los proyectos con componente cuando hay un o mas proyectos de 100%, ya no mostrarlo en el combo de proyectos
//cargaProyectoComponente = () => {
//    _listProyecto.map(proyecto => {
//        let _proyectoPorcentajeTotal = 0;
//        // Relacion Componente -> Proyecto
//        listMatrizIndicadorResultadoIndicador.filter(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE && miri.ProyectoId == proyecto.ProyectoId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).map(miri => _proyectoPorcentajeTotal += miri.PorcentajeProyecto);
//        // Relacion Actividad -> Proyecto
//        listMatrizIndicadorResultadoIndicador.filter(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.ACTIVIDAD && miri.ProyectoId == proyecto.ProyectoId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).map(miri => _proyectoPorcentajeTotal += miri.PorcentajeProyecto);
//        if (_proyectoPorcentajeTotal >= 100) {
//            proyecto.visible = false;
//        } else {
//            proyecto.visible = true;
//        }
//    });
//}

// Carga los proyectos con actividad cuando hay un o mas proyectos de 100%, ya no mostrarlo en el combo de proyectos
//cargaProyectoActividad = () => {
//    //_listProyecto.map(proyecto => {
//    //    proyecto.visible = true;
//    //});
//    _listProyecto.map(proyecto => {
//        let _proyectoPorcentajeTotal = 0;
//        // Relacion Componente -> Proyecto
//        listMatrizIndicadorResultadoIndicador.filter(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE && miri.ProyectoId == proyecto.ProyectoId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).map(miri => _proyectoPorcentajeTotal += miri.PorcentajeProyecto);
//        // Relacion Actividad -> Proyecto
//        listMatrizIndicadorResultadoIndicador.filter(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.ACTIVIDAD && miri.ProyectoId == proyecto.ProyectoId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).map(miri => _proyectoPorcentajeTotal += miri.PorcentajeProyecto);
//        if (_proyectoPorcentajeTotal >= 100) {
//            proyecto.visible = false;
//        } else {
//            proyecto.visible = true;
//        }
//    });
//}

// Carga Semaforización
cargaSemaforizacion = (esEvento, sentidoId, nivel) => {
    if (esEvento) {
        // Ascendente
        if (sentidoId == ControlMaestroMapeo.Sentido.ASCENDENTE) {
            dxFormContenido.getEditor('AceptableDesde').option({ value: null });
            dxFormContenido.getEditor('AceptableHasta').option({ value: null });
            dxFormContenido.getEditor('ConRiesgoDesde').option({ value: null, 'readOnly': false });
            dxFormContenido.getEditor('ConRiesgoHasta').option({ value: null, 'readOnly': true });
            dxFormContenido.getEditor('CriticoPorDebajo').option({ value: null });
            dxFormContenido.getEditor('CriticoPorEncima').option({ value: null });
        }
        // Descendente
        if (sentidoId == ControlMaestroMapeo.Sentido.DESCENDENTE) {
            dxFormContenido.getEditor('AceptableDesde').option({ value: null });
            dxFormContenido.getEditor('AceptableHasta').option({ value: null });
            dxFormContenido.getEditor('ConRiesgoDesde').option({ value: null, 'readOnly': true });
            dxFormContenido.getEditor('ConRiesgoHasta').option({ value: null, 'readOnly': false });
            dxFormContenido.getEditor('CriticoPorDebajo').option({ value: null });
            dxFormContenido.getEditor('CriticoPorEncima').option({ value: null });
        }
    } else {
        // Este funcion setTimeout hacer funciona para los campos solo lectura
        setTimeout(() => {
            // Ascendente
            if (sentidoId == ControlMaestroMapeo.Sentido.ASCENDENTE) {
                dxFormContenido.getEditor('ConRiesgoDesde').option({ 'readOnly': false });
                dxFormContenido.getEditor('ConRiesgoHasta').option({ 'readOnly': true });
            }
            // Descendente
            if (sentidoId == ControlMaestroMapeo.Sentido.DESCENDENTE) {
                dxFormContenido.getEditor('ConRiesgoDesde').option({ 'readOnly': true });
                dxFormContenido.getEditor('ConRiesgoHasta').option({ 'readOnly': false });
            }
        }, 0);
    }
}
// Cargar Meta cuando el registro es editar
cargaMeta = (mirIndicadorId) => {
    if (listMatrizIndicadorResultadoIndicadorMeta.some(mirim => mirim.MIRIndicadorId == mirIndicadorId && mirim.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
        var dxFormMeta = $('#dxFormMeta').dxForm('instance'),
            items = dxFormMeta.option('items')[0].items;
        items.map((item, index) => {
            var varMirim = listMatrizIndicadorResultadoIndicadorMeta.find(mirim => mirim.MIRIndicadorId == mirIndicadorId && mirim.Orden == index && mirim.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO);
            dxFormMeta.getEditor(item.dataField).option('value', varMirim ? varMirim.Valor : null);
        });
    } else {
        if (!esEditar) {
            var dxFormMeta = $('#dxFormMeta').dxForm('instance'),
                items = dxFormMeta.option('items')[0].items;
            listaMetaGlobal.map(meta => {
                dxFormMeta.getEditor(meta.dataField).option('value', meta.value);
            });
        }
    }
    
}
// Carga Formula Variable cuando el registro es editar
cargaFormulaVariable = (mirIndicadorId) => {
    if (listMatrizIndicadorResultadoIndicadorMeta.some(mirim => mirim.MIRIndicadorId == mirIndicadorId && mirim.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
        var dxFormMeta = $('#dxFormMeta').dxForm('instance'),
            items = dxFormMeta.option('items')[0].items;
        items.map((item, index) => {
            dxFormMeta.getEditor(item.dataField).option('value', listMatrizIndicadorResultadoIndicadorMeta.find(mirim => mirim.MIRIndicadorId == mirIndicadorId && mirim.Orden == index && mirim.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).Valor);
        });
    }
}

// Carga la tabla de nivel activiad componente en datos del componente
cargaTablaNivelActividadComponente = () => {
    var tablaNivelActividadComponente = $('#tablaNivelActividadComponente').find("tbody");
    // Limpiar la tabla
    tablaNivelActividadComponente.empty();
    // Tipo Compnente -> Relacion Actividad
    if (globalMatrizIndicadorResultadoIndicador.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_ACTIVIDAD) {
        tablaNivelActividadComponente.append("<tr class='d-flex'><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>Tipo de Componente:</th ><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>" + _listTipoComponente.find(tp => tp.ControlId == globalMatrizIndicadorResultadoIndicador.TipoComponenteId).Valor + "</td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></td></tr><tr class='d-flex'><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>Nombre del Componente</th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>" + globalMatrizIndicadorResultadoIndicador.NombreIndicador + "</td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></td></tr>");
    }
    // Tipo Compnente -> Relacion Componente
    if (globalMatrizIndicadorResultadoIndicador.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_COMPONENTE) {
        const options = { style: 'currency', currency: 'MXN' }
        const numberFormat = new Intl.NumberFormat('es-MX', options);
        tablaNivelActividadComponente.append("<tr class='d-flex'><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>Tipo de Componente:</th ><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>" + _listTipoComponente.find(tp => tp.ControlId == globalMatrizIndicadorResultadoIndicador.TipoComponenteId).Valor + "</td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>Proyecto:</th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>" + globalMatrizIndicadorResultadoIndicador.ProyectoId + ' - ' + _listProyecto.find(p => p.ProyectoId == globalMatrizIndicadorResultadoIndicador.ProyectoId).Nombre + "</td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></td></tr><tr class='d-flex'><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>Nombre del Componente</th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>" + globalMatrizIndicadorResultadoIndicador.NombreIndicador + "</td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>Porcentaje:</th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>" + globalMatrizIndicadorResultadoIndicador.PorcentajeProyecto + "%</td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>Monto:</th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>" + numberFormat.format(globalMatrizIndicadorResultadoIndicador.MontoProyecto) + "</td></tr>");
    }
}

// Carga la tabla de nivel activiad componente en datos del componente
cargaTablaNivelActividadFormulario = (matrizIndicadorResultadoIndicador) => {
    // Limpiar la tabla
    tablaNivelActividadFormulario.find("tbody").empty();
    // Tipo Compnente -> Relacion Actividad
    if (matrizIndicadorResultadoIndicador.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_ACTIVIDAD) {
        tablaNivelActividadFormulario.find("tbody").append("<tr class='d-flex'><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>Tipo de Componente:</th ><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>" + _listTipoComponente.find(tp => tp.ControlId == matrizIndicadorResultadoIndicador.TipoComponenteId).Valor + "</td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></td></tr><tr class='d-flex'><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>Nombre del Componente</th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>" + matrizIndicadorResultadoIndicador.NombreIndicador + "</td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></td></tr>");
    }
    // Tipo Compnente -> Relacion Componente
    if (matrizIndicadorResultadoIndicador.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_COMPONENTE) {
        const options = { style: 'currency', currency: 'MXN' }
        const numberFormat = new Intl.NumberFormat('es-MX', options);
        tablaNivelActividadFormulario.find("tbody").append("<tr class='d-flex'><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>Tipo de Componente:</th ><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>" + _listTipoComponente.find(tp => tp.ControlId == matrizIndicadorResultadoIndicador.TipoComponenteId).Valor + "</td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>Proyecto:</th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>" + _listProyecto.find(p => p.ProyectoId == matrizIndicadorResultadoIndicador.ProyectoId).Nombre + "</td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'></td></tr><tr class='d-flex'><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>Nombre del Componente</th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>" + matrizIndicadorResultadoIndicador.NombreIndicador + "</td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>Porcentaje:</th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>" + matrizIndicadorResultadoIndicador.PorcentajeProyecto + "%</td><th class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>Monto:</th><td class='col-6 col-sm-6 col-md-2 col-lg-2 col-xl-2'>" + numberFormat.format(matrizIndicadorResultadoIndicador.MontoProyecto) + "</td></tr>");
    }
}

// Carga resumen
cargaResumen = () => {
    const resumenModel = {
        MIRIndicadorId: null,
        NombreProyecto: '',
        PorcentajeProyecto: null,
        PorcentajeActividad: null,
        Monto: null,
        MIRIndicadorComponenteId: null,
        TipoComponenteId: null,
        Componente: '',
        Actividad: '',
        PorcentajeProyectoOriginal: null,
        MontoProyecto: null
    }, listResumen = [];

    let componenteCount = 1, componenteTitulo = '', actividadCount = null;

    listMatrizIndicadorResultadoIndicador.filter(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).map(miri => {
        // Titulo
        //componenteTitulo = 'C' + componenteCount;
        //componenteTitulo = 'C' + componenteCount + ' - ' + miri.NombreIndicador;
        actividadCount = 1;
        listMatrizIndicadorResultadoIndicador.filter(_miri => _miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.ACTIVIDAD && _miri.MIRIndicadorComponenteId == miri.MIRIndicadorId && _miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).map(_miri => {
            // Creamos los objetos nuevos
            let resumen = $.extend(true, {}, resumenModel);
            resumen.MIRIndicadorId = _miri.MIRIndicadorId;
            // Tipo Componente -> Relacion Actividad
            if (miri.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_ACTIVIDAD) {
                resumen.NombreProyecto = _listProyecto.some(p => p.ProyectoId == _miri.ProyectoId) ? _listProyecto.find(p => p.ProyectoId == _miri.ProyectoId).Nombre : '';
                resumen.PorcentajeProyecto = _miri.PorcentajeProyecto;
                resumen.Monto = _miri.MontoProyecto;
                // Titulo
                componenteTitulo = 'C' + componenteCount + ' - ' + miri.NombreIndicador;
            }
            // Tipo Componente -> Relacion Componente
            if (miri.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_COMPONENTE) {
                resumen.NombreProyecto = _listProyecto.some(p => p.ProyectoId == miri.ProyectoId) ? _listProyecto.find(p => p.ProyectoId == miri.ProyectoId).Nombre : '';
                resumen.PorcentajeActividad = _miri.PorcentajeActividad;
                resumen.Monto = _miri.MontoActividad;
                // Titulo
                componenteTitulo = 'C' + componenteCount + ' - ' + miri.NombreIndicador + ' ' + resumen.NombreProyecto;
            }
            // Titulo
            resumen.Componente = componenteTitulo;
            //resumen.Actividad = 'Actividad ' + actividadCount;
            resumen.Actividad = _miri.NombreIndicador;
            // Nivel Componente
            resumen.MIRIndicadorComponenteId = miri.MIRIndicadorId;
            resumen.TipoComponenteId = miri.TipoComponenteId
            resumen.MontoProyecto = miri.MontoProyecto ? miri.MontoProyecto : null;
            resumen.PorcentajeProyectoOriginal = miri.PorcentajeProyecto;
            // Insernamos a listado de resumen con arreglo (array)
            listResumen.push(resumen);
            // Count
            actividadCount++;
        });
        // Count
        componenteCount++;
    });
    // Establecer los datos a DataGrid
    var dataSource = new DevExpress.data.DataSource({
        store: {
            type: 'array',
            key: 'MIRIndicadorId',
            data: listResumen
        }
    });
    dxDataGridContenido.option('dataSource', dataSource)
}
///////////////////////////////////

// HABILITAR //
habilitaCombos = () => {
    dxFormContenido.getEditor('PlanDesarrolloId').option({ disabled: false });
    // Cargar los combos por fecha de ejericio
    cargaPlanDesarrollo(_matrizIndicadorResultado.Ejercicio);

    dxFormContenido.getEditor('PlanDesarrolloEstructuraId').option({ disabled: false });
    // Cargar los combos plan de desarrollo estructura
    cargaPlanDesarrolloEstructura(_matrizIndicadorResultado.PlanDesarrolloId);
}

habilitaComponentes = (enabled) => {
    if (matrizIndicadorResultado.MIRId > 0)
        dxButtonEliminar.option("disabled", !enabled);
    //dxButtonDeshacer.option("disabled", !enabled);
    dxButtonGuardar.option("disabled", !enabled);
}

habilitaVisibleBotones = (enabled) => {
    dxButtonEliminar.option('visible', !enabled);
    //dxButtonDeshacer.option('visible', !enabled);
    dxButtonGuardar.option('visible', !enabled);
}
///////////////

// List con Drawer //
onItemClickDrawer = (event) => {
    // Obtener el index que seleccionar en lista
    seleccionModel.siguiente = event.itemData.index;
    // Cambiamos el valor de menu
    esMenu = true;

    if (esActividad)
        esActividad = false;

    if (esFormulario) {
        modalConfirmaDeshacerNivel.modal('show');
    } else {
        verificarSeleccion();
    }
}
/////////////////////

// Datos Generales //
displayExprPlanDesarrollo = (event) => {
    if (event) {
        if (new Date(event.FechaInicio) != 'Invalid Date') {
            return event.NombrePlan + ' ' + event.FechaInicio.getFullYear() + ' - ' + event.FechaFin.getFullYear();
        } else {
            return event.NombrePlan + ' ' + new Date(parseInt(event.FechaInicio.substr(6))).getFullYear() + ' - ' + new Date(parseInt(event.FechaFin.substr(6))).getFullYear();
        }
    }
    return null;
}

onValueChangedPlanDesarrollo = (event) => {
    if (event.value) {
        // Habilitar el campo PlanDesarrolloEstructuraId (Tipo de Plan) y carga los combos
        dxFormContenido.getEditor('PlanDesarrolloEstructuraId').option({ value: '', disabled: false, dataSource: cargaPlanDesarrolloEstructura(event.value) });
    } else {
        dxFormContenido.getEditor('PlanDesarrolloEstructuraId').option({ value: '', disabled: true });
    }
}
/////////////////////

// OnClick //
onClickToolbar = () => {
    dxDrawer.toggle();
}
// nuemro de drawer (numeroDrawer): 
// 1 - Datos Generales
// 2 - Fin
// 3 - Proposito
// 4 - Componente
// 5 - Actividad
// 6 - Resumen
onClickAnterior = (event, siguiente) => {
    seleccionModel.actual = siguiente;
    seleccionModel.siguiente = siguiente;
    cargaContenidoSwitch();
}
onClickSiguiente = (event, siguiente) => {
    seleccionModel.siguiente = siguiente;
    siguientePestania();
}

onClickAgregarNivel = (nivel) => {
    // Cambiamos el boolean esEditar
    esEditar = false;
    listaMetaGlobal = [];
    // Deshabilitamos los botones de acciones
    habilitaComponentes(false);
    // Carga el contenido formulario dentro del nivel
    if (nivel == 'actividad') {
        // Tipo Componente -> Relacion Componente
        if (globalMatrizIndicadorResultadoIndicador.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_COMPONENTE) {
            let totalPorcentajeActividad = 0;
            listMatrizIndicadorResultadoIndicador.filter(miri => miri.MIRIndicadorComponenteId == globalMatrizIndicadorResultadoIndicador.MIRIndicadorId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).map(miri => {
                totalPorcentajeActividad += miri.PorcentajeActividad;
            });
            if (totalPorcentajeActividad >= globalMatrizIndicadorResultadoIndicador.PorcentajeProyecto) {
                toast('Ya no esta disponible el porcentaje para el proyecto.', 'error');
                return;
            }
        }
        // Carga los proyectos
        cargaProyectoPorPorcentaje()

        cargaContenidoFormularioActividad(nivel);
    } else {
        if (nivel == 'componente')
            cargaProyectoPorPorcentaje();

        cargaContenidoFormulario(nivel);
    }
    //dxFormContenido.option('formData', $.extend(true, {}, _matrizIndicadorResultadoIndicadorModel));
    dxFormContenido.resetValues();
    // Obtener Datos Generales
    // Asignamos algo de los datos al formulario
    dxFormContenido.updateData('MIRId', matrizIndicadorResultado.MIRId);
    dxFormContenido.updateData('MIRIndicadorId', contadorRegistrosNuevos);
    dxFormContenido.updateData('NivelIndicadorId', nivelModel.nivelId);
    //dxFormContenido.updateData('AnioBase', matrizIndicadorResultado.Ejercicio);
    dxFormContenido.updateData('EstatusId', ControlMaestroMapeo.EstatusRegistro.ACTIVO);
    if (nivel == 'actividad') {
        dxFormContenido.updateData('TipoComponenteId', globalMatrizIndicadorResultadoIndicador.TipoComponenteId);
        dxFormContenido.updateData('MIRIndicadorComponenteId', globalMatrizIndicadorResultadoIndicador.MIRIndicadorId);
    }
    contadorRegistrosNuevos -= 1;
}

onClickEditarNivel = (event, nivel) => {
    // Cambiamos el boolean esEditar
    esEditar = true;
    // Deshabilitamos los botones de acciones
    habilitaComponentes(false);
    // Carga el contenido formulario dentro del nivel
    if (nivel == 'actividad') {
        // Carga los proyectos
        cargaProyectoPorPorcentaje()

        cargaContenidoFormularioActividad(nivel);
    } else {
        if (nivel == 'componente')
            cargaProyectoPorPorcentaje();

        cargaContenidoFormulario(nivel);
    }

    dxFormContenido.resetValues();
    // Asignamos los datos a Formulario
    dxFormContenido.option('formData', $.extend(true, {}, listMatrizIndicadorResultadoIndicador.find(miri => miri.MIRIndicadorId == event.row.data.MIRIndicadorId)));
}

onClickVerActividad = (event) => {
    // Cambiamos el boolean esActividad para actividad
    esActividad = true;

    const matrizIndicadorResultadoIndicador = $.extend(true, {}, listMatrizIndicadorResultadoIndicador.find(miri => miri.MIRIndicadorId == event.row.data.MIRIndicadorId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO));
    globalMatrizIndicadorResultadoIndicador = matrizIndicadorResultadoIndicador;
    // Carga el contenido DataGrid dentro del Nivel Actividad
    cargaContenidoDataGridActividad();
}

onClickEditarResumen = (event) => {
    // Deshabilitamos los botones de acciones
    habilitaComponentes(false);
    // Cambiamos el index 5 es seleccion de nivel actividad
    seleccionModel.siguiente = 5;
    // Cambiamos el boolean esEditar
    esActividad = true;
    const matrizIndicadorResultadoIndicador = $.extend(true, {}, listMatrizIndicadorResultadoIndicador.find(miri => miri.MIRIndicadorId == event.row.data.MIRIndicadorComponenteId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO));
    globalMatrizIndicadorResultadoIndicador = matrizIndicadorResultadoIndicador;
    // Carga la tabla de Nivel Actividad Componente en Datos del Componente
    cargaContenidoFormularioActividad('actividad');
    dxFormContenido.resetValues();
    // Asignamos los datos a formulario
    dxFormContenido.option('formData', $.extend(true, {}, listMatrizIndicadorResultadoIndicador.find(miri => miri.MIRIndicadorId == event.row.data.MIRIndicadorId)));
}

onClickEliminarNivel = (event, nivel) => {
    // Obtenemos una copia del objeto a eliminar
    eliminaRowNivel = $.extend(true, {}, event.row.data);
    // Cambiamos el nivel en el modal
    modalConfirmaEliminarNivel.attr("nivel", nivel);
    // Mostramos el modal
    modalConfirmaEliminarNivel.modal('show');
}

onClickEliminaRowNivel = () => {
    if (eliminaRowNivel != null) {
        // Si el registro viene de la base de datos
        // para posteriormente eliminarlo en la base de datos
        if (eliminaRowNivel.MIRIndicadorId > 0) {
            // Actualizamos el estatus del registro a "Borrado"
            var matrizIndicadorResultadoIdicador = listMatrizIndicadorResultadoIndicador.find(miri => miri.MIRIndicadorId == eliminaRowNivel.MIRIndicadorId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO);
            matrizIndicadorResultadoIdicador.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO;
            // Eliminamos los registros de MIRIM
            var __listMatrizIndicadorResultadoIndicadorMeta = listMatrizIndicadorResultadoIndicadorMeta.filter(mirim => mirim.MIRIndicadorId == eliminaRowNivel.MIRIndicadorId && mirim.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO);
            if (__listMatrizIndicadorResultadoIndicadorMeta.some(mirim => mirim.MIRIndicadorMetaId > 0)) {
                __listMatrizIndicadorResultadoIndicadorMeta.map(mirim => {
                    mirim.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO;
                });
            } else {
                const eliminarListmatrizIndicadorResultadoIndicadorMeta = $.extend(true, [], __listMatrizIndicadorResultadoIndicadorMeta);
                eliminarListmatrizIndicadorResultadoIndicadorMeta.map(mirim => {
                    let index = listMatrizIndicadorResultadoIndicadorMeta.findIndex(_mirim => _mirim.MIRIndicadorMetaId == mirim.MIRIndicadorMetaId);
                    listMatrizIndicadorResultadoIndicadorMeta.splice(index, 1);
                });
            }
            // Eliminamos los registros de MIRIFV
            var __listMatrizIndicadorResultadoIndicadorFormulaVariable = listMatrizIndicadorResultadoIndicadorFormulaVariable.filter(fv => fv.MIRIndicadorId == eliminaRowNivel.MIRIndicadorId && fv.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO);
            if (__listMatrizIndicadorResultadoIndicadorFormulaVariable.some(fv => fv.MIRIndicadorFormulaVariableId > 0)) {
                __listMatrizIndicadorResultadoIndicadorFormulaVariable.map(fv => {
                    fv.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO;
                });
            } else {
                const eliminarListmatrizIndicadorResultadoIndicadorFormulaVariable = $.extend(true, [], __listMatrizIndicadorResultadoIndicadorFormulaVariable);
                eliminarListmatrizIndicadorResultadoIndicadorFormulaVariable.map(fv => {
                    let index = listMatrizIndicadorResultadoIndicadorFormulaVariable.findIndex(_fv => _fv.MIRIndicadorFormulaVariableId == fv.MIRIndicadorFormulaVariableId);
                    listMatrizIndicadorResultadoIndicadorFormulaVariable.splice(index, 1);
                });
            }
            // Eliminamos los registros de MIRI -> Nivel Actividad
            var __listMatrizIndicadorResultadoIndicador = listMatrizIndicadorResultadoIndicador.filter(miri => miri.MIRIndicadorComponenteId == eliminaRowNivel.MIRIndicadorId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO);
            if (__listMatrizIndicadorResultadoIndicador.some(miri => miri.MIRIndicadorId > 0)) {
                __listMatrizIndicadorResultadoIndicador.map(miri => {
                    miri.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO;
                });
            } else {
                const eliminarListmatrizIndicadorResultadoIndicador = $.extend(true, [], __listMatrizIndicadorResultadoIndicador);
                eliminarListmatrizIndicadorResultadoIndicador.map(miri => {
                    let index = listMatrizIndicadorResultadoIndicador.findIndex(_miri => _miri.MIRIndicadorId == miri.MIRIndicadorId);
                    listMatrizIndicadorResultadoIndicador.splice(index, 1);
                });
            }
        } else {
            // Eliminamos el registro de MIRI
            listMatrizIndicadorResultadoIndicador.splice(listMatrizIndicadorResultadoIndicador.findIndex(miri => miri.MIRIndicadorId == eliminaRowNivel.MIRIndicadorId), 1);
            // Eliminamos los registros de MIRIM
            const eliminarListmatrizIndicadorResultadoIndicadorMeta = $.extend(true, [], listMatrizIndicadorResultadoIndicadorMeta.filter(mirim => mirim.MIRIndicadorId == eliminaRowNivel.MIRIndicadorId));
            eliminarListmatrizIndicadorResultadoIndicadorMeta.map(mirim => {
                let index = listMatrizIndicadorResultadoIndicadorMeta.findIndex(_mirim => _mirim.MIRIndicadorMetaId == mirim.MIRIndicadorMetaId);
                listMatrizIndicadorResultadoIndicadorMeta.splice(index, 1);
            });
            // Eliminamos los registros de MIRIFV
            const eliminarListmatrizIndicadorResultadoIndicadorFormulaVariable = $.extend(true, [], listMatrizIndicadorResultadoIndicadorFormulaVariable.filter(fv => fv.MIRIndicadorId == eliminaRowNivel.MIRIndicadorId));
            eliminarListmatrizIndicadorResultadoIndicadorFormulaVariable.map(fv => {
                let index = listMatrizIndicadorResultadoIndicadorFormulaVariable.findIndex(_fv => _fv.MIRIndicadorFormulaVariableId == fv.MIRIndicadorFormulaVariableId);
                listMatrizIndicadorResultadoIndicadorFormulaVariable.splice(index, 1);
            });
            // Eliminamos el registro de MIRI -> Nivel Actividad
            const eliminarListmatrizIndicadorResultadoIndicador = $.extend(true, [], listMatrizIndicadorResultadoIndicador.filter(miri => miri.MIRIndicadorComponenteId == eliminaRowNivel.MIRIndicadorId));
            eliminarListmatrizIndicadorResultadoIndicador.map(miri => {
                let index = listMatrizIndicadorResultadoIndicadorMeta.findIndex(miri => miri.MIRIndicadorId == mirim.MIRIndicadorId);
                listMatrizIndicadorResultadoIndicador.splice(index, 1);
            });
        }
        // Cerramos las pestañas del Nivel
        //cerrarPestaniaNivel();
        dxDataGridContenido.getDataSource().reload();
        // Habilitamos los botones de acciones
        habilitaComponentes(true);

        modalConfirmaEliminarNivel.modal('hide');
    }
}

onClickModalDeshacerNivel = () => {
    modalConfirmaDeshacerNivel.modal("hide");
    // Cambiamos el valor de formulario
    esFormulario = false;
    // Habilitamos los botones de acciones
    habilitaComponentes(true);

    if (esMenu) {
        esMenu = false;
        verificarSeleccion();
    } else {
        cargaContenidoSwitch();
    }
}

onClickEliminar = () => {
    modalConfirmaEliminar.modal("hide");
    if (matrizIndicadorResultado.MIRId > 0) {
        matrizIndicadorResultado.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO;
        listMatrizIndicadorResultadoIndicador.map(miri => miri.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO);
        listMatrizIndicadorResultadoIndicadorMeta.map(mirim => mirim.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO);
        guardaCambios();
    } else {
        regresarListado();
    }
}

////////////

// OnChange //
onChangeAnioBase = (event) => {
    /*const ejercicio = parseInt(dxFormContenido.getEditor('Ejercicio').option('value'));*/
    const preEjercicio = parseInt(event.component.option('value'));
    if (preEjercicio > parseInt(matrizIndicadorResultado.Ejercicio)) {
        event.component.option('value', matrizIndicadorResultado.Ejercicio);
        toast('No se puede mayor que ' + matrizIndicadorResultado.Ejercicio, 'error');
    }
}
//////////////

// OnTitleClick //
onTitleClickPestania = (event, nivel) => {
    // Meta
    if (event.itemIndex == 1) {
        // Verificar las selecciones para los campos Frecuencia de medicion y Sentido
        if (!validaFrecuenciaMedicionYSentido(event, nivel))
            return;

        //var matrizIndicadorResultadoIndicador = dxFormContenido.option('formData'), esValidaValorBase = dxFormContenido.getEditor('ValorBase').option('isValid');
        // Verificar si cuando cambiar tamaño de ventana o algun que no cargar función entonces este te ayuda para cargar las metas con editar o si es nuevo pon 0 de todos.
        //if (!$('#dxFormMeta').hasClass('dx-form')) {
        //    cargaTemplateMeta(matrizIndicadorResultadoIndicador.FrecuenciaMedicionId, nivel);
        //    // Carga Meta cuando el registro es editar para mostrar los campos en Meta
        //    cargaMeta(matrizIndicadorResultadoIndicador.MIRIndicadorId);
        //}
    }
    // Semaforización
    if (event.itemIndex == 2) {
        var dxForm = dxFormContenido.getEditor('SentidoId');
        if (dxForm.option('value') == null) {
            toast('Necesita llenar el campo Sentido', 'error');
            event.component.option('selectedIndex', 0);
            dxForm.focus();
            return;
        }
    }
    // Método de cálculo
    if (event.itemIndex == 3) {
        //if (!$('#dxFormFormula').hasClass('dx-form')) {
            //var matrizIndicadorResultadoIndicador = null;
            //// Nivel Fin
            //if (nivel == 'fin')
            //    matrizIndicadorResultadoIndicador = dxFormNivelFinFormulario.option('formData');
            //// Nivel Proposito
            //if (nivel == 'proposito')
            //    matrizIndicadorResultadoIndicador = dxFormNivelPropositoFormulario.option('formData');
            //// Nivel Componente
            //if (nivel == 'componente')
            //    matrizIndicadorResultadoIndicador = dxFormNivelComponenteFormulario.option('formData');
            //// Nivel Actividad
            //if (nivel == 'actividad')
            //    matrizIndicadorResultadoIndicador = dxFormNivelActividadFormulario.option('formData');

            //if (matrizIndicadorResultadoIndicador.FormulaId) {
            //    templateFormula(_listFormulaConDimension.find(f => f.FormulaId == matrizIndicadorResultadoIndicador.FormulaId));
            //}
        //}
    }
}
//////////////////

// OnValueChanged //
onValueChangedEjercicio = (event) => {
    if (event.value) {
        dxLoaderPanel.show();
        dxFormContenido.getEditor('PlanDesarrolloId').option({ value: '', disabled: false, dataSource: cargaPlanDesarrollo(event.value) });
        dxFormContenido.getEditor('PlanDesarrolloEstructuraId').option({ value: '', disabled: true });
        dxFormContenido.getEditor('FechaFinConfiguracion').option({ value: new Date(parseInt(event.value), 1, 15) });
        // Carga los combos de proyecto con ejercicio y programa gobierno
        cargaProducto();
    } else {
        dxFormContenido.getEditor('PlanDesarrolloId').option({ value: '', disabled: true });
        dxFormContenido.getEditor('PlanDesarrolloEstructuraId').option({ value: '', disabled: true });
        dxFormContenido.getEditor('FechaFinConfiguracion').option({ value: null });
    }
}

onValueChangedProyecto = (event, nivel) => {
    if (event.event) {
        var dxForm = dxFormContenido;
        if (event.value) {
            dxForm.getEditor('MontoProyecto').option({ value: _listProyecto.find(proyecto => proyecto.ProyectoId == event.value).PresupuestoVigente });
            // Verificar el proyecto existe en otro componente
            if (nivel == 'componente' || nivel == 'actividad') {
                const mirIndicadorId = dxForm.option('formData').MIRIndicadorId;
                let totalPorcentaje = 0;
                if (listMatrizIndicadorResultadoIndicador.some(miri => miri.ProyectoId == event.value && miri.MIRIndicadorId != mirIndicadorId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
                    if (dxForm.option('formData').NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE) {
                        toast('Este Proyecto ya esta siendo utilizado en otro Componente.', 'info');
                    } else {
                        toast('Este Proyecto ya esta siendo utilizado en otra Actividad.', 'info');
                    }
                    // Establecer el porcentaje 0
                    proyectoPorcentajeTotal = 0;
                    listMatrizIndicadorResultadoIndicador.filter(miri => miri.ProyectoId == event.value && miri.MIRIndicadorId != mirIndicadorId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).map(miri => proyectoPorcentajeTotal += miri.PorcentajeProyecto);
                    totalPorcentaje = 100 - proyectoPorcentajeTotal;
                    //dxForm.getEditor('PorcentajeProyecto').option({ value: 100 - proyectoPorcentajeTotal });
                } else {
                    totalPorcentaje = 100;
                    //dxForm.getEditor('PorcentajeProyecto').option({ value: 100 });
                }
                var editorPorcentajeProyecto = dxForm.getEditor('PorcentajeProyecto');
                editorPorcentajeProyecto.option({ value: totalPorcentaje });
                editorPorcentajeProyecto.element().dxValidator({
                    validationRules: [{
                        type: 'range',
                        max: totalPorcentaje,
                        message: 'El Porcentaje debe ser menor que ' + totalPorcentaje + '%'
                    }]
                });
            }
            //else {
            //    if (nivel == 'actividad') {
            //        if (dxForm.option('formData').PorcentajeProyecto == null) {
            //            dxForm.getEditor('PorcentajeProyecto').option({ value: 100 });
            //        }
            //    } else {
            //        dxForm.getEditor('PorcentajeProyecto').option({ value: 100 });
            //    }
            //}
        } else {
            dxForm.getEditor('MontoProyecto').option({ value: null });
        }
    }
}

onValueChangedPorcentajeProyecto = (event, nivel) => {
    if (event.value) {
        if (nivel == 'componente' || nivel == 'actividad') {
            var dxForm = dxFormContenido;
            const formData = dxForm.option('formData');
            if (listMatrizIndicadorResultadoIndicador.some(miri => miri.ProyectoId == formData.ProyectoId && miri.MIRIndicadorId != formData.MIRIndicadorId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
                if (!event.event) {
                    proyectoPorcentajeTotal = 0;
                    listMatrizIndicadorResultadoIndicador.filter(miri => miri.ProyectoId == formData.ProyectoId && miri.MIRIndicadorId != formData.MIRIndicadorId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).map(miri => proyectoPorcentajeTotal += miri.PorcentajeProyecto);
                }
                event.component.element().dxValidator({
                    validationRules: [{
                        type: 'range',
                        max: 100 - proyectoPorcentajeTotal,
                        message: 'El Porcentaje debe ser menor que ' + (100 - proyectoPorcentajeTotal) + '%'
                    }]
                });
                //if ((100 - proyectoPorcentajeTotal) < event.value) {
                //    event.component.element().dxValidator({
                //        validationRules: [{
                //            type: 'range',
                //            max: 100 - proyectoPorcentajeTotal,
                //            message: 'El Porcentaje debe ser menor que ' + (100 - proyectoPorcentajeTotal) + '%'
                //        }]
                //    });
                //    //event.component.option({
                //    //    validationStatus: 'invalid',
                //    //    validationError: {
                //    //        type: 'custom',
                //    //        message: 'El Porcentaje debe ser menor que ' + (100 - proyectoPorcentajeTotal) + '%.'
                //    //    }
                //    //});
                //}
            }
        }
    }
}
// Este es para la seleccion de Dimension para la Unidad de Medida y Formula
onValueChangedDimension = (event, nivel) => {
    if (event.value) {
        // Unidad Medida
        var dxSelectBoxUnidadMedida = dxFormContenido.getEditor('UnidadMedidaId');
        dxSelectBoxUnidadMedida.option('disabled', false);
        var dataSourceUnidadMedida = dxSelectBoxUnidadMedida.getDataSource();
        dataSourceUnidadMedida.filter(['DimensionId', '=', event.value]);
        dataSourceUnidadMedida.load();
        // Formula
        //var dxSelectBoxFormula = dxForm.getEditor('FormulaId');
        //dxSelectBoxFormula.option('disabled', false);
        //var dataSourceFormula = dxSelectBoxFormula.getDataSource();
        //dataSourceFormula.filter(['DimensionId', '=', event.value]);
        //dataSourceFormula.load();
    } else {
        dxFormContenido.getEditor('UnidadMedidaId').option({ disabled: true, value: null });
        //dxForm.getEditor('FormulaId').option({ disabled: true, value: null });
    }
}
// Unidad Medida
onValueChangedUnidadMedida = (event, nivel) => {
    if (event.value) {
        if (dxFormContenido.option('formData').FormulaId != event.value) {
            const unidadMedida = _listControlMaestroUnidadMedidaConDimension.find(um => um.UnidadMedidaId == event.value);
            dxFormContenido.updateData('FormulaId', unidadMedida.UnidadMedidaId);
            dxFormContenido.updateData('DescripcionFormula', unidadMedida.Formula);
        }
        templateFormula(dxFormContenido.option('formData').MIRIndicadorId, event.value);
    }
}
// Este es para las selecciones para Frecuencia de medición y Sentio
onValueChanedFrecuenciaMedicion = (event, nivel) => {
    if (event.event) {
        //dxFormNivelFinFormulario.itemOption('DatosControlCalculo.DatosControlCalculoPestania', 'tabPanelOptions', { disabled: false });
        var matrizIndicadorResultadoIndicador = dxFormContenido.option("formData");

        if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId) {
            // Mostramos Loader
            dxLoaderPanel.show();
            setTimeout(() => {
                // Template Meta para cuando el ususario selecario seleccione el campo Frecuencia Medicion
                cargaTemplateMeta(matrizIndicadorResultadoIndicador.FrecuenciaMedicionId, nivel);
                // Si el campo Valor existe mostrar el valor a los campos en meta
                //existeValorBase(nivel);
                // Cargar Meta cuando el registro es editar para mostrar los campos en Meta
                cargaMeta(matrizIndicadorResultadoIndicador.MIRIndicadorId);
                // Mostramos Loader
                dxLoaderPanel.hide();
            }, 100);
        }
    }
}
// Este es para la seleccion para Frecuencia de medición
onValueChanedSentido = (event, nivel) => {
    if (event.value) {
        if (event.value == ControlMaestroMapeo.Sentido.ASCENDENTE || event.value == ControlMaestroMapeo.Sentido.DESCENDENTE) {
            cargaSemaforizacion(event.event, event.value, nivel);
        }
    }
}

onValueChangedValorBase = (event) => {
    if (event.value) {
        try {
            var dxFormMeta = $('#dxFormMeta').dxForm('instance'),
                items = dxFormMeta.option('items')[0].items;
            items.map(item => {
                dxFormMeta.getEditor(item.dataField).option('value', event.value);
            });
        } catch { }
    }
}

onValueChangedMeta = (event, nivel) => {
    if (event.value) {
        var valorBase = null,
            sentidoId = null;
        // Nivel Fin
        if (nivel == 'fin') {
            valorBase = dxFormNivelFinFormulario.getEditor('ValorBase').option('value');
            sentidoId = dxFormNivelFinFormulario.getEditor('SentidoId').option('value');
        }
        // Nivel Proposito
        if (nivel == 'proposito') {
            valorBase = dxFormNivelPropositoFormulario.getEditor('ValorBase').option('value');
            sentidoId = dxFormNivelPropositoFormulario.getEditor('SentidoId').option('value');
        }
        // Nivel Componente
        if (nivel == 'componente') {
            valorBase = dxFormNivelComponenteFormulario.getEditor('ValorBase').option('value');
            sentidoId = dxFormNivelComponenteFormulario.getEditor('SentidoId').option('value');
        }
        // Nivel Actividad
        if (nivel == 'actividad') {
            valorBase = dxFormNivelActividadFormulario.getEditor('ValorBase').option('value');
            sentidoId = dxFormNivelActividadFormulario.getEditor('SentidoId').option('value');
        }

        if (sentidoId == ControlMaestroMapeo.Sentido.ASCENDENTE) {
            if (valorBase > event.value) {
                event.component.option({
                    validationStatus: 'invalid',
                    validationError: {
                        type: 'custom',
                        message: 'El valor debe ser mayor que $' + valorBase + '.'
                    }
                });
            }
        } else {
            if (valorBase < event.value) {
                event.component.option({
                    validationStatus: 'invalid',
                    validationError: {
                        type: 'custom',
                        message: 'El valor debe ser menor que $' + valorBase + '.'
                    }
                });
            }
        }
    }
}
// Semaforizacion
onValueChangedSemaforizacion = (event, nivel, semaforizacion, esMayor) => {
    if (event.value && event.event) {
        // Acetable
        if (semaforizacion == 'aceptable') {
            // El valor es mayor o menor
            let valor = esMayor ? dxFormContenido.getEditor('AceptableDesde').option('value') : dxFormContenido.getEditor('AceptableHasta').option('value');

            if (valor != null) {
                let _valor = event.value;
                if (!esMayor ? valor < _valor : valor > _valor) {
                    event.component.option({
                        validationStatus: 'invalid',
                        validationError: {
                            type: 'custom',
                            message: esMayor ? 'Debe ser mayor que ' + valor + '%.' : 'Debe ser menor que ' + valor + '%.'
                        }
                    });
                    return;
                }
            }
            // Ascendente
            if (dxFormContenido.getEditor('SentidoId').option('value') == ControlMaestroMapeo.Sentido.ASCENDENTE) {
                if (!esMayor)
                    dxFormContenido.getEditor('ConRiesgoHasta').option('value', event.value);
                if (esMayor)
                    dxFormContenido.getEditor('CriticoPorEncima').option('value', event.value);

            }
            // Descendente
            if (dxFormContenido.getEditor('SentidoId').option('value') == ControlMaestroMapeo.Sentido.DESCENDENTE) {
                if (esMayor)
                    dxFormContenido.getEditor('ConRiesgoDesde').option('value', event.value);
                if (!esMayor)
                    dxFormContenido.getEditor('CriticoPorDebajo').option('value', event.value);
            }
        }
        // Con Riesgo
        if (semaforizacion == 'conriesgo') {
            // El valor es mayor o menor
            let sentidoId = dxFormContenido.getEditor('SentidoId').option('value'),
                valor = sentidoId == ControlMaestroMapeo.Sentido.ASCENDENTE ? dxFormContenido.getEditor('ConRiesgoHasta').option('value') : dxFormContenido.getEditor('ConRiesgoDesde').option('value');
            // Verificar el sentido es Ascendente o Descendente
            if (valor != null) {
                let _valor = event.value;
                if (sentidoId == ControlMaestroMapeo.Sentido.ASCENDENTE ? valor < _valor : valor > _valor) {
                    event.component.option({
                        validationStatus: 'invalid',
                        validationError: {
                            type: 'custom',
                            message: sentidoId == ControlMaestroMapeo.Sentido.ASCENDENTE ? 'El Ascendente, debe ser menor que ' + valor + '%.' : 'El Descendente, debe ser mayor que ' + valor + '%.'
                        }
                    });

                    return;
                }
            }
            // Ascendente
            if (sentidoId == ControlMaestroMapeo.Sentido.ASCENDENTE) {
                dxFormContenido.getEditor('CriticoPorDebajo').option('value', event.value);
            }
            // Descendente
            if (sentidoId == ControlMaestroMapeo.Sentido.DESCENDENTE) {
                dxFormContenido.getEditor('CriticoPorEncima').option('value', event.value);
            }
        }
    }
}

onValueChangedTipoComponente = (event) => {
    if (event.value) {
        if (event.value == ControlMaestroMapeo.TipoComponente.RELACION_ACTIVIDAD) {
            dxFormContenido.itemOption('DatosIndicador.DatosIndicadorGrupoProyecto', 'visible', false);
            dxFormContenido.updateData('ProyectoId', null);
            proyectoPorcentajeTotal = 0;
            dxFormContenido.updateData('PorcentajeProyecto', null);
            dxFormContenido.updateData('MontoProyecto', null);
        }
        if (event.value == ControlMaestroMapeo.TipoComponente.RELACION_COMPONENTE) {
            dxFormContenido.itemOption('DatosIndicador.DatosIndicadorGrupoProyecto', 'visible', true);
            var matrizIndicadorResultadoIndicador = dxFormContenido.option('formData');
            if (matrizIndicadorResultadoIndicador.ProyectoId == null) {
                proyectoPorcentajeTotal = 0;
                dxFormContenido.updateData('PorcentajeProyecto', 100);
                dxFormContenido.updateData('MontoProyecto', null);
            }
        }
    }
}

onValueChangedPorcentajeActividad = (event) => {
    if (event.value) {
        if (event.component.option('isValid')) {
            let porcentajeActividadTotal = event.value;
            listMatrizIndicadorResultadoIndicador.filter(miri => miri.MIRIndicadorComponenteId == globalMatrizIndicadorResultadoIndicador.MIRIndicadorId && miri.MIRIndicadorId != dxFormContenido.option('formData').MIRIndicadorId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).map(miri => porcentajeActividadTotal += miri.PorcentajeActividad);
            if (porcentajeActividadTotal > globalMatrizIndicadorResultadoIndicador.PorcentajeProyecto) {
                porcentajeActividadTotal = globalMatrizIndicadorResultadoIndicador.PorcentajeProyecto - (porcentajeActividadTotal - event.value);
                dxFormContenido.getEditor('PorcentajeActividad').element().dxValidator({
                    validationRules: [{
                        type: 'range',
                        max: porcentajeActividadTotal,
                        message: 'El Porcentaje debe ser menor que ' + porcentajeActividadTotal + '%'
                    }]
                });
            } else {
                dxFormContenido.updateData('MontoActividad', parseFloat(parseFloat(globalMatrizIndicadorResultadoIndicador.MontoProyecto * (event.value / 100)).toFixed(2)));
            }
        }
    }
}

///////////////////

// OnFocusIn //
onFocusInValorBase = (event, nivel) => {
    if (!validaFrecuenciaMedicionYSentido(null, nivel))
        return;
}

onFocusInPorcentajeActividad = (event) => {
    let porcentajeActividadTotal = 0;
    listMatrizIndicadorResultadoIndicador.filter(miri => miri.MIRIndicadorComponenteId == globalMatrizIndicadorResultadoIndicador.MIRIndicadorId && miri.MIRIndicadorId != dxFormContenido.option('formData').MIRIndicadorId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).map(miri => porcentajeActividadTotal += miri.PorcentajeActividad);
    porcentajeActividadTotal = globalMatrizIndicadorResultadoIndicador.PorcentajeProyecto - porcentajeActividadTotal;
    toast('El Porcentaje disponible es del ' + porcentajeActividadTotal + '% ', 'info');
}


onFocusInConRiesgo = (nivel) => {
    if (!validaConRiesgoDesdeYConRiesgoHasta(nivel))
        return;
}
///////////////

// GroupCellTemplate //
groupCellTemplateResumen = (element, options) => {
    if (options.data.items != null && options.data.items.length > 0) {
        if (options.data.items[0].TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_ACTIVIDAD) {
            //element.text('Componente ' + options.value);
            element.text(options.value);
        }
        if (options.data.items[0].TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_COMPONENTE) {
            //element.text('Componente ' + options.value + ' ' + options.data.items[0].NombreProyecto);
            element.text(options.value + ' ' + options.data.items[0].NombreProyecto);
        }
    }

}
///////////////////////

// CellTemplate //
cellTemplateResumen = (element, options) => {
    element.text(options.value);
    //if (options.data.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_ACTIVIDAD) {
    //    element.text(options.value);
    //}
    //if (options.data.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_COMPONENTE) {
    //    element.text('');
    //}
}
//////////////////

// CalculateCustom //
calculateCustomSummaryResumen = (event) => {
    if (event.name == 'PorcentajeProyecto') {
        if (event.summaryProcess == 'calculate') {
            event.totalValue = event.value.PorcentajeProyectoOriginal;
        }
    }
}
/////////////////////

guardaDatos = () => {
    // Datos Generales
    if (seleccionModel.actual == 1) {
        if (!dxFormContenido.validate().isValid) {
            // Validacion y enfocar a la celda
            dxFormContenido.validate().brokenRules[0].validator.focus();
            dxListDrawer.option('selectedItemKeys', [seleccionActual]);
            return false;
        }
        matrizIndicadorResultado = dxFormContenido.option('formData');
        seleccionModel.actual = seleccionModel.siguiente;
        return true;
    }
    // Nivel Fin
    if (seleccionModel.actual == 2) {
        if (!listMatrizIndicadorResultadoIndicador.some(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.FIN && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
            toast('Debe agregar 1 o más de Fin.', 'error');
            return false;
        }
        seleccionModel.actual = seleccionModel.siguiente;
        return true;
    }
    // Nivel Propósito
    if (seleccionModel.actual == 3) {
        if (!listMatrizIndicadorResultadoIndicador.some(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.PROPOSITO && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
            toast('Debe agregar 1 de Propósito.', 'error');
            return false;
        }
        seleccionModel.actual = seleccionModel.siguiente;
        return true;
    }
    // Nivel Componente
    if (seleccionModel.actual == 4) {
        if (!listMatrizIndicadorResultadoIndicador.some(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
            toast('Debe agregar 1 o más de Componente.', 'error');
            return false;
        }
        seleccionModel.actual = seleccionModel.siguiente;
        return true;
    }
    // Nivel Actividad
    if (seleccionModel.actual == 5) {
        const __listMatrizIndicadorResultadoIndicador = $.extend(true, [], listMatrizIndicadorResultadoIndicador.filter(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO));
        let esCompletoActividad = true, nombreComponente = '';
        for (let i = 0; i < __listMatrizIndicadorResultadoIndicador.length; i++) {
            if (!listMatrizIndicadorResultadoIndicador.some(miri => miri.MIRIndicadorComponenteId == __listMatrizIndicadorResultadoIndicador[i].MIRIndicadorId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
                esCompletoActividad = false;
                nombreComponente = __listMatrizIndicadorResultadoIndicador[i].NombreIndicador;
                break;
            }
        }
        if (!esCompletoActividad) {
            toast('Debe agregar 1 o más de Actividad por Componente, el Componente: ' + nombreComponente, 'error');
            return false;
        }
        seleccionModel.actual = seleccionModel.siguiente;
        return true;
    }
    // Resumen
    if (seleccionModel.actual == 6) {
        seleccionModel.actual = seleccionModel.siguiente;
        return true;
    }
}

// Otro //
siguientePestania = () => {
    if (!guardaDatos())
        return;

    cargaContenidoSwitch();
}

cargaContenidoSwitch = () => {
    switch (seleccionModel.actual) {
        case 1: // Datos Generales
            dxListDrawer.option('selectedItemKeys', [1]);
            cargaContenidoDatosGenerales();
            break;
        case 2: // Nivel Fin
            dxListDrawer.option('selectedItemKeys', [2]);
            cargaContenidoDataGrid('fin');
            break;
        case 3: // Nivel Proposito
            dxListDrawer.option('selectedItemKeys', [3]);
            cargaContenidoDataGrid('proposito');
            break;
        case 4: // Nivel Componente
            dxListDrawer.option('selectedItemKeys', [4]);
            cargaContenidoDataGrid('componente');
            break;
        case 5: // Nivel Actividad
            if (esActividad) {
                dxListDrawer.option('selectedItemKeys', [5]);
                cargaContenidoDataGridActividad();
            } else {
                dxListDrawer.option('selectedItemKeys', [5]);
                cargaContenidoDataGrid('actividad');
            }
            break;
        case 6: // Resumen
            dxListDrawer.option('selectedItemKeys', [6]);
            cargaContenidoResumen();
            //cargaResumen();
            break;
        default:
            dxListDrawer.option('selectedItemKeys', [1]);
            cargaContenidoDatosGenerales();
            break;
    }
}

verificarSeleccion = () => {
    let esSiguiente = true;
    for (let i = 1; i < seleccionModel.siguiente; i++) {
        // Datos Generales
        if (seleccionModel.actual == 1) {
            if (!dxFormContenido.validate().isValid) {
                dxListDrawer.option('selectedItemKeys', [1]);
                seleccionModel.actual = i;
                esSiguiente = false;
                break;
            }
            matrizIndicadorResultado = dxFormContenido.option('formData');
        }
        // Nivel Fin
        if (i == 2) {
            if (!listMatrizIndicadorResultadoIndicador.some(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.FIN && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
                toast('Debe agregar 1 o más de Fin.', 'error');
                dxListDrawer.option('selectedItemKeys', [2]);
                seleccionModel.actual = i;
                esSiguiente = false;
                break;
            }
        }
        // Nivel Propósito
        if (i == 3) {
            if (!listMatrizIndicadorResultadoIndicador.some(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.PROPOSITO && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
                toast('Debe agregar 1 de Propósito.', 'error');
                dxListDrawer.option('selectedItemKeys', [3]);
                seleccionModel.actual = i;
                esSiguiente = false;
                break;
            }
        }
        // Nivel Componente
        if (i == 4) {
            if (!listMatrizIndicadorResultadoIndicador.some(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
                toast('Debe agregar 1 o más de Componente.', 'error');
                dxListDrawer.option('selectedItemKeys', [4]);
                seleccionModel.actual = i;
                esSiguiente = false;
                break;
            }
        }
        // Nivel Actividad
        if (i == 5) {
            const __listMatrizIndicadorResultadoIndicador = $.extend(true, [], listMatrizIndicadorResultadoIndicador.filter(miri => miri.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO));
            let esCompletoActividad = true, nombreComponente = '';
            for (let i = 0; i < __listMatrizIndicadorResultadoIndicador.length; i++) {
                if (!listMatrizIndicadorResultadoIndicador.some(miri => miri.MIRIndicadorComponenteId == __listMatrizIndicadorResultadoIndicador[i].MIRIndicadorId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
                    esCompletoActividad = false;
                    nombreComponente = __listMatrizIndicadorResultadoIndicador[i].NombreIndicador;
                    break;
                }
            }
            if (!esCompletoActividad) {
                toast('Debe agregar 1 o más de Actividad por Componente, el Componente: ' + nombreComponente, 'error');
                dxListDrawer.option('selectedItemKeys', [5]);
                seleccionModel.actual = i;
                esSiguiente = false;
                break;
            }
        }
        // Resumen
        if (i == 6) {
            //seleccionModel.actual = i;
        }
    }

    if (esSiguiente)
        seleccionModel.actual = seleccionModel.siguiente;

    cargaContenidoSwitch();
}

existeValorBase = (nivel) => {
    let valorBase = null;
    // Nivel Fin
    if (nivel == 'fin')
        valorBase = dxFormNivelFinFormulario.getEditor('ValorBase').option('value');
    // Nivel Proposito
    if (nivel == 'proposito')
        valorBase = dxFormNivelPropositoFormulario.getEditor('ValorBase').option('value');
    // Nivel Componente
    if (nivel == 'componente')
        valorBase = dxFormNivelComponenteFormulario.getEditor('ValorBase').option('value');
    // Nivel Actividad
    if (nivel == 'actividad')
        valorBase = dxFormNivelActividadFormulario.getEditor('ValorBase').option('value');

    if (valorBase) {
        var dxFormMeta = $('#dxFormMeta').dxForm('instance'),
            items = dxFormMeta.option('items')[0].items;
        items.map(item => {
            dxFormMeta.getEditor(item.dataField).option('value', valorBase);
        });
    }
}

recargarFicha = () => {
    //modalConfirmaDeshacer.modal('hide');
    var matrizIndicadorResultado = dxFormContenido.option("formData");
    if (matrizIndicadorResultado.MIRId > 0) {
        window.location.href = API_FICHA + 'editar/' + matrizIndicadorResultado.MIRId;
    } else {
        window.location.href = API_FICHA + 'nuevo';
    }

}

esVencidoFechaFinCongiguracion = () => {
    let fechaFinConfiguracion = matrizIndicadorResultado.FechaFinConfiguracion, valida = false;
    // Obtener la fecha solo fecha no horario y convertir a times
    fechaFinConfiguracion = new Date(fechaFinConfiguracion.getFullYear() + '/' + (fechaFinConfiguracion.getMonth() + 1) + '/' + fechaFinConfiguracion.getDate()).getTime();
    // Enviar y Obtener la fecha del servidor
    $.ajax({
        type: 'POST',
        url: API_FICHA + 'fechadelservidor',
        async: false,
        success: function (response) {
            var date = new Date(parseInt(response.substr(6))),
                fechaActual = new Date(date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate()).getTime();
            if (fechaActual > fechaFinConfiguracion) {
                toast('La fecha no puede ser anterior. Por favor selecciona una fecha correcta.', 'error');
                //setTimeout(() => {
                //    window.location.href = '/mir/mir/matrizindicadorresultado/listar';
                //}, 5000);
                valida = true;
            } else {
                valida = false;
            }
        },
        error: function (response, status, error) {
            toast('No se puede guardar los cambios, inténtalo de nuevo más tarde', "error");
            valida = false;
        }
    });
    return valida;
}

esModoLecturaPorVencidoFechaFinCongiguracion = () => {
    if (matrizIndicadorResultado.MIRId > 0) {
        let fechaFinConfiguracion = matrizIndicadorResultado.FechaFinConfiguracion;
        // Obtener la fecha solo fecha no horario y convertir a times
        fechaFinConfiguracion = new Date(fechaFinConfiguracion.getFullYear() + '/' + (fechaFinConfiguracion.getMonth() + 1) + '/' + fechaFinConfiguracion.getDate()).getTime();
        var date = new Date(),
            fechaActual = new Date(date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate()).getTime();

        if (fechaActual > fechaFinConfiguracion)
            esModoLectura = true;
    }
}

// Buscar ultimo de los nodos de Plan Desarrollo Estructura
buscaUltimoNodoPlanDesarrolloEstructura = (listaPlanDesarrolloEstructura) => {
    let nuevoListaPlanDesarrollo = [];
    listaPlanDesarrolloEstructura.map(pd => {
        if (!listaPlanDesarrolloEstructura.some(_pd => _pd.EstructuraPadreId == pd.PlanDesarrolloEstructuraId)) {
            pd.EstructuraPadreId = null;
            nuevoListaPlanDesarrollo.push(pd);
        }
    });
    return nuevoListaPlanDesarrollo;
}

validaHayCambiosModal = () => {
    // Obtener los datos para saber hay los datos o no hay para guardar
    const data = obtenerData();
    if (data.matrizIndicadorResultado == null && data.listMatrizIndicadorResultadoIndicador.length == 0 && data.listMatrizIndicadorResultadoIndicadorMeta.length == 0 && data.listMatrizIndicadorResultadoIndicadorFormulaVariable.length == 0) {
        regresarListado();
    } else {
        modalCerrar.modal('show');
    }
}

regresarListado = () => {
    window.location.href = API_FICHA + "listar";
}
//////////

// CalculateCellValue //
calculateCellValueCodigo = (event) => {
    if (event.Codigo) {
        return event.Codigo;
    }
    return "Pendiente";
}

calculateCellValueResumenPorcentajeProyecto = (event) => {
    if (event.TipoComponenteId == ControlMaestroMapeo.TipoComponente.RELACION_ACTIVIDAD) {
        return event.PorcentajeProyecto;
    }
    return null;
}
////////////////////////

// LAS VALIDACIONES //
// Validar para los campos Frecuencia medicion y Sentido
validaFrecuenciaMedicionYSentido = (event, nivel) => {
    var matrizIndicadorResultadoIndicador = dxFormContenido.option('formData');
    // Verificar que no esta llenados los campos Frecuencia de medición y Sentido
    if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == null && matrizIndicadorResultadoIndicador.SentidoId == null) {
        if (event)
            event.component.option('selectedIndex', 0);

        toast('Necesita llenar los campos Frecuencia de medición y Sentido.', 'error');

        dxFormContenido.getEditor('FrecuenciaMedicionId').focus();

        return false;
    }
    // Verificar que no esta llenado el campo Frecuencia de medición
    if (matrizIndicadorResultadoIndicador.FrecuenciaMedicionId == null) {
        if (event)
            event.component.option('selectedIndex', 0);
        toast('Necesita llenar el campo Frecuencia de medición.', 'error');

        dxFormContenido.getEditor('FrecuenciaMedicionId').focus();

        return false;
    }
    // Verificar que no esta llenado el campo Sentido
    if (matrizIndicadorResultadoIndicador.SentidoId == null) {
        if (event)
            event.component.option('selectedIndex', 0);
        toast('Necesita llenar el campo Sentido.', 'error');

        dxFormContenido.getEditor('SentidoId').focus();

        return false;
    }
    return true;
}

// Validar para los campos ConRiesgoDesde y ConRiesgoHasta
validaConRiesgoDesdeYConRiesgoHasta = (nivel) => {
    var matrizIndicadorResultadoIndicador = dxFormContenido.option('formData'),
        esValida = matrizIndicadorResultadoIndicador.SentidoId == ControlMaestroMapeo.Sentido.ASCENDENTE ? dxFormContenido.getEditor('AceptableDesde').option('isValid') : dxFormContenido.getEditor('AceptableHasta').option('isValid');
    // Verificar que no esta llenado o validado el campo AceptableDesde o AceptableHasta
    // Ascendente
    if (matrizIndicadorResultadoIndicador.SentidoId == ControlMaestroMapeo.Sentido.ASCENDENTE) {
        let _esValida = true;
        // Si el valor es null
        if (matrizIndicadorResultadoIndicador.AceptableDesde == null) {
            toast('Necesita llenar el campo Aceptable Desde.', 'error');
            _esValida = false;
        }
        // Si la valida es false
        if (_esValida && !esValida) {
            toast('Necesita validar el campo Aceptable Desde.', 'error');
            _esValida = false;
        }
        if (!_esValida) {
            dxFormContenido.getEditor('AceptableDesde').focus();
            return false;
        }
    }
    // Descendente
    if (matrizIndicadorResultadoIndicador.SentidoId == ControlMaestroMapeo.Sentido.DESCENDENTE) {
        let _esValida = true
        if (matrizIndicadorResultadoIndicador.AceptableHasta == null) {
            toast('Necesita llenar el campo Aceptable Hasta.', 'error');
            _esValida = false;
        }
        // Si la valida es false
        if (_esValida && !esValida) {
            toast('Necesita validar el campo Aceptable Hasta.', 'error');
            _esValida = false;
        }
        if (!_esValida) {
            dxFormContenido.getEditor('AceptableHasta').focus();
            return false;
        }
    }
    return true;
}
//////////////////////

guardaCambios = () => {
    // Ocultamos Loader
    dxLoaderPanel.show();
    // Verificar si la fecha ha vencido
    if (esVencidoFechaFinCongiguracion()) {
        // Ocultamos Loader
        dxLoaderPanel.hide();
        return;
    }
        
    // Datos Generales
    if (seleccionModel.actual == 1)
        if (!dxFormContenido.validate().isValid) {
            // Ocultamos Loader
            dxLoaderPanel.hide();
            return;
        }
            

    // Obtener los datos para saber hay los datos o no hay para guardar
    const data = obtenerData();
    if (data.matrizIndicadorResultado == null && data.listMatrizIndicadorResultadoIndicador.length == 0 && data.listMatrizIndicadorResultadoIndicadorMeta.length == 0 && data.listMatrizIndicadorResultadoIndicadorFormulaVariable.length == 0) {
        toast("No se puede guardar ya que no hubo cambios en la información", "error");
        // Ocultamos Loader
        dxLoaderPanel.hide();
        return;
    }

    $.ajax({
        type: 'POST',
        url: API_FICHA + 'guardar',
        data: data,
        success: function (response) {
            // Mostramos mensaje de Exito
            toast(response, 'success');
            // Redirigir a listado
            regresarListado();
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();
            // Mostramos mensaje de error
            toast(response.responseText, "error");
            //toast('No se puede guardar los cambios, inténtalo de nuevo más tarde', "error");
        }
    });
}

obtenerData = () => {
    let data = {
        matrizIndicadorResultado: null,
        listMatrizIndicadorResultadoIndicador: [],
        listMatrizIndicadorResultadoIndicadorMeta: [],
        listMatrizIndicadorResultadoIndicadorFormulaVariable: []
    };

    // Matriz Indicador Resultado
    //var matrizIndicadorResultado = dxFormDatosGenerales.option('formData');
    if (matrizIndicadorResultado.EstatusId != ControlMaestroMapeo.EstatusRegistro.BORRADO) {
        if (matrizIndicadorResultado.MIRId > 0) {
            if (matrizIndicadorResultado.Ejercicio != _matrizIndicadorResultado.Ejercicio || matrizIndicadorResultado.PlanDesarrolloId != _matrizIndicadorResultado.PlanDesarrolloId || matrizIndicadorResultado.PoblacionObjetivo != _matrizIndicadorResultado.PoblacionObjetivo || matrizIndicadorResultado.ProgramaPresupuestarioId != _matrizIndicadorResultado.ProgramaPresupuestarioId || matrizIndicadorResultado.FechaFinConfiguracion.getTime() != _matrizIndicadorResultado.FechaFinConfiguracion.getTime() || matrizIndicadorResultado.PlanDesarrolloEstructuraId != _matrizIndicadorResultado.PlanDesarrolloEstructuraId) {
                let __matrizIndicadorResultado = $.extend(true, {}, matrizIndicadorResultado);
                __matrizIndicadorResultado.FechaFinConfiguracion = __matrizIndicadorResultado.FechaFinConfiguracion.toUTCString();
                __matrizIndicadorResultado.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(__matrizIndicadorResultado.Timestamp)));
                data.matrizIndicadorResultado = __matrizIndicadorResultado;
            }
        } else {
            data.matrizIndicadorResultado = matrizIndicadorResultado;
            data.matrizIndicadorResultado.FechaFinConfiguracion = data.matrizIndicadorResultado.FechaFinConfiguracion.toUTCString();
        }
    } else {
        data.matrizIndicadorResultado = matrizIndicadorResultado;
        data.matrizIndicadorResultado.FechaFinConfiguracion = data.matrizIndicadorResultado.FechaFinConfiguracion.toUTCString();
        data.matrizIndicadorResultado.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(data.matrizIndicadorResultado.Timestamp)));
    }


    // Matriz Indicador Resultado Indicador -> Nivel Fin
    listMatrizIndicadorResultadoIndicador.filter(nivelFin => nivelFin.NivelIndicadorId == ControlMaestroMapeo.Nivel.FIN).map(nivelFin => {
        // Hay eliminar
        if (nivelFin.EstatusId != ControlMaestroMapeo.EstatusRegistro.BORRADO) {
            // Si el ID es nuevo para registrar o editar
            if (nivelFin.MIRIndicadorId > 0) {
                const matrizIndicadorResultadoIndicador = _listMatrizIndicadorResultadoIndicador.find(_nivelFin => _nivelFin.MIRIndicadorId == nivelFin.MIRIndicadorId);
                if (nivelFin.NombreIndicador != matrizIndicadorResultadoIndicador.NombreIndicador || nivelFin.ResumenNarrativo != matrizIndicadorResultadoIndicador.ResumenNarrativo || nivelFin.DefinicionIndicador != matrizIndicadorResultadoIndicador.DefinicionIndicador || nivelFin.TipoIndicadorId != matrizIndicadorResultadoIndicador.TipoIndicadorId || nivelFin.DimensionId != matrizIndicadorResultadoIndicador.DimensionId || nivelFin.UnidadMedidaId != matrizIndicadorResultadoIndicador.UnidadMedidaId || nivelFin.FrecuenciaMedicionId != matrizIndicadorResultadoIndicador.FrecuenciaMedicionId || nivelFin.SentidoId != matrizIndicadorResultadoIndicador.SentidoId || nivelFin.AnioBase != matrizIndicadorResultadoIndicador.AnioBase || nivelFin.ValorBase != matrizIndicadorResultadoIndicador.ValorBase || nivelFin.DescripcionBase != matrizIndicadorResultadoIndicador.DescripcionBase || nivelFin.AceptableDesde != matrizIndicadorResultadoIndicador.AceptableDesde || nivelFin.AceptableHasta != matrizIndicadorResultadoIndicador.AceptableHasta || nivelFin.ConRiesgoDesde != matrizIndicadorResultadoIndicador.ConRiesgoDesde || nivelFin.ConRiesgoHasta != matrizIndicadorResultadoIndicador.ConRiesgoHasta || nivelFin.CriticoPorDebajo != matrizIndicadorResultadoIndicador.CriticoPorDebajo || nivelFin.CriticoPorEncima != matrizIndicadorResultadoIndicador.CriticoPorEncima || nivelFin.FormulaId != matrizIndicadorResultadoIndicador.FormulaId || nivelFin.FuenteInformacion != matrizIndicadorResultadoIndicador.FuenteInformacion || nivelFin.MedioVerificacion != matrizIndicadorResultadoIndicador.MedioVerificacion) {
                    let _matrizIndicadorResultadoIndicador = $.extend(true, {}, nivelFin);
                    _matrizIndicadorResultadoIndicador.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(_matrizIndicadorResultadoIndicador.Timestamp)));
                    data.listMatrizIndicadorResultadoIndicador.push(_matrizIndicadorResultadoIndicador);
                }
            } else {
                data.listMatrizIndicadorResultadoIndicador.push(nivelFin);
            }
        } else {
            nivelFin.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(nivelFin.Timestamp)));
            data.listMatrizIndicadorResultadoIndicador.push(nivelFin);
        }
    });

    // Matriz Indicador Resultado Indicador -> Nivel Proposito
    listMatrizIndicadorResultadoIndicador.filter(nivelProposito => nivelProposito.NivelIndicadorId == ControlMaestroMapeo.Nivel.PROPOSITO).map(nivelProposito => {
        // Hay eliminar
        if (nivelProposito.EstatusId != ControlMaestroMapeo.EstatusRegistro.BORRADO) {
            // Si el ID es nuevo para registrar o editar
            if (nivelProposito.MIRIndicadorId > 0) {
                const matrizIndicadorResultadoIndicador = _listMatrizIndicadorResultadoIndicador.find(_nivelProposito => _nivelProposito.MIRIndicadorId == nivelProposito.MIRIndicadorId);
                if (nivelProposito.NombreIndicador != matrizIndicadorResultadoIndicador.NombreIndicador || nivelProposito.ResumenNarrativo != matrizIndicadorResultadoIndicador.ResumenNarrativo || nivelProposito.DefinicionIndicador != matrizIndicadorResultadoIndicador.DefinicionIndicador || nivelProposito.TipoIndicadorId != matrizIndicadorResultadoIndicador.TipoIndicadorId || nivelProposito.DimensionId != matrizIndicadorResultadoIndicador.DimensionId || nivelProposito.UnidadMedidaId != matrizIndicadorResultadoIndicador.UnidadMedidaId || nivelProposito.FrecuenciaMedicionId != matrizIndicadorResultadoIndicador.FrecuenciaMedicionId || nivelProposito.SentidoId != matrizIndicadorResultadoIndicador.SentidoId || nivelProposito.AnioBase != matrizIndicadorResultadoIndicador.AnioBase || nivelProposito.ValorBase != matrizIndicadorResultadoIndicador.ValorBase || nivelProposito.DescripcionBase != matrizIndicadorResultadoIndicador.DescripcionBase || nivelProposito.AceptableDesde != matrizIndicadorResultadoIndicador.AceptableDesde || nivelProposito.AceptableHasta != matrizIndicadorResultadoIndicador.AceptableHasta || nivelProposito.ConRiesgoDesde != matrizIndicadorResultadoIndicador.ConRiesgoDesde || nivelProposito.ConRiesgoHasta != matrizIndicadorResultadoIndicador.ConRiesgoHasta || nivelProposito.CriticoPorDebajo != matrizIndicadorResultadoIndicador.CriticoPorDebajo || nivelProposito.CriticoPorEncima != matrizIndicadorResultadoIndicador.CriticoPorEncima || nivelProposito.FormulaId != matrizIndicadorResultadoIndicador.FormulaId || nivelProposito.FuenteInformacion != matrizIndicadorResultadoIndicador.FuenteInformacion || nivelProposito.MedioVerificacion != matrizIndicadorResultadoIndicador.MedioVerificacion) {
                    let _matrizIndicadorResultadoIndicador = $.extend(true, {}, nivelProposito);
                    _matrizIndicadorResultadoIndicador.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(_matrizIndicadorResultadoIndicador.Timestamp)));
                    data.listMatrizIndicadorResultadoIndicador.push(_matrizIndicadorResultadoIndicador);
                }
            } else {
                data.listMatrizIndicadorResultadoIndicador.push(nivelProposito);
            }
        } else {
            nivelProposito.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(nivelProposito.Timestamp)));
            data.listMatrizIndicadorResultadoIndicador.push(nivelProposito);
        }
    });

    // Matriz Indicador Resultado Indicador -> Nivel Componente
    listMatrizIndicadorResultadoIndicador.filter(nivelComponente => nivelComponente.NivelIndicadorId == ControlMaestroMapeo.Nivel.COMPONENTE).map(nivelComponente => {
        // Hay eliminar
        if (nivelComponente.EstatusId != ControlMaestroMapeo.EstatusRegistro.BORRADO) {
            // Si el ID es nuevo para registrar o editar
            if (nivelComponente.MIRIndicadorId > 0) {
                const matrizIndicadorResultadoIndicador = _listMatrizIndicadorResultadoIndicador.find(_nivelComponente => _nivelComponente.MIRIndicadorId == nivelComponente.MIRIndicadorId);
                if (nivelComponente.ProyectoId != matrizIndicadorResultadoIndicador.ProyectoId || nivelComponente.PorcentajeProyecto != matrizIndicadorResultadoIndicador.PorcentajeProyecto || nivelComponente.MontoProyecto != matrizIndicadorResultadoIndicador.MontoProyecto || nivelComponente.NombreIndicador != matrizIndicadorResultadoIndicador.NombreIndicador || nivelComponente.ResumenNarrativo != matrizIndicadorResultadoIndicador.ResumenNarrativo || nivelComponente.DefinicionIndicador != matrizIndicadorResultadoIndicador.DefinicionIndicador || nivelComponente.TipoIndicadorId != matrizIndicadorResultadoIndicador.TipoIndicadorId || nivelComponente.DimensionId != matrizIndicadorResultadoIndicador.DimensionId || nivelComponente.UnidadMedidaId != matrizIndicadorResultadoIndicador.UnidadMedidaId || nivelComponente.FrecuenciaMedicionId != matrizIndicadorResultadoIndicador.FrecuenciaMedicionId || nivelComponente.SentidoId != matrizIndicadorResultadoIndicador.SentidoId || nivelComponente.AnioBase != matrizIndicadorResultadoIndicador.AnioBase || nivelComponente.ValorBase != matrizIndicadorResultadoIndicador.ValorBase || nivelComponente.DescripcionBase != matrizIndicadorResultadoIndicador.DescripcionBase || nivelComponente.AceptableDesde != matrizIndicadorResultadoIndicador.AceptableDesde || nivelComponente.AceptableHasta != matrizIndicadorResultadoIndicador.AceptableHasta || nivelComponente.ConRiesgoDesde != matrizIndicadorResultadoIndicador.ConRiesgoDesde || nivelComponente.ConRiesgoHasta != matrizIndicadorResultadoIndicador.ConRiesgoHasta || nivelComponente.CriticoPorDebajo != matrizIndicadorResultadoIndicador.CriticoPorDebajo || nivelComponente.CriticoPorEncima != matrizIndicadorResultadoIndicador.CriticoPorEncima || nivelComponente.FormulaId != matrizIndicadorResultadoIndicador.FormulaId || nivelComponente.FuenteInformacion != matrizIndicadorResultadoIndicador.FuenteInformacion || nivelComponente.MedioVerificacion != matrizIndicadorResultadoIndicador.MedioVerificacion) {
                    let _matrizIndicadorResultadoIndicador = $.extend(true, {}, nivelComponente);
                    _matrizIndicadorResultadoIndicador.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(_matrizIndicadorResultadoIndicador.Timestamp)));
                    data.listMatrizIndicadorResultadoIndicador.push(_matrizIndicadorResultadoIndicador);
                }
            } else {
                data.listMatrizIndicadorResultadoIndicador.push(nivelComponente);
            }
        } else {
            nivelComponente.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(nivelComponente.Timestamp)));
            data.listMatrizIndicadorResultadoIndicador.push(nivelComponente);
        }
    });

    // Matriz Indicador Resultado Indicador -> Nivel Actividad
    listMatrizIndicadorResultadoIndicador.filter(nivelActividad => nivelActividad.NivelIndicadorId == ControlMaestroMapeo.Nivel.ACTIVIDAD).map(nivelActividad => {
        // Hay eliminar
        if (nivelActividad.EstatusId != ControlMaestroMapeo.EstatusRegistro.BORRADO) {
            // Si el ID es nuevo para registrar o editar
            if (nivelActividad.MIRIndicadorId > 0) {
                const matrizIndicadorResultadoIndicador = _listMatrizIndicadorResultadoIndicador.find(_nivelActividad => _nivelActividad.MIRIndicadorId == nivelActividad.MIRIndicadorId);
                if (nivelActividad.ProyectoId != matrizIndicadorResultadoIndicador.ProyectoId || nivelActividad.PorcentajeProyecto != matrizIndicadorResultadoIndicador.PorcentajeProyecto || nivelActividad.MontoProyecto != matrizIndicadorResultadoIndicador.MontoProyecto || nivelActividad.PorcentajeActividad != matrizIndicadorResultadoIndicador.PorcentajeActividad || nivelActividad.MontoActividad != matrizIndicadorResultadoIndicador.MontoActividad || nivelActividad.NombreIndicador != matrizIndicadorResultadoIndicador.NombreIndicador || nivelActividad.ResumenNarrativo != matrizIndicadorResultadoIndicador.ResumenNarrativo || nivelActividad.DefinicionIndicador != matrizIndicadorResultadoIndicador.DefinicionIndicador || nivelActividad.TipoIndicadorId != matrizIndicadorResultadoIndicador.TipoIndicadorId || nivelActividad.DimensionId != matrizIndicadorResultadoIndicador.DimensionId || nivelActividad.UnidadMedidaId != matrizIndicadorResultadoIndicador.UnidadMedidaId || nivelActividad.FrecuenciaMedicionId != matrizIndicadorResultadoIndicador.FrecuenciaMedicionId || nivelActividad.SentidoId != matrizIndicadorResultadoIndicador.SentidoId || nivelActividad.AnioBase != matrizIndicadorResultadoIndicador.AnioBase || nivelActividad.ValorBase != matrizIndicadorResultadoIndicador.ValorBase || nivelActividad.DescripcionBase != matrizIndicadorResultadoIndicador.DescripcionBase || nivelActividad.AceptableDesde != matrizIndicadorResultadoIndicador.AceptableDesde || nivelActividad.AceptableHasta != matrizIndicadorResultadoIndicador.AceptableHasta || nivelActividad.ConRiesgoDesde != matrizIndicadorResultadoIndicador.ConRiesgoDesde || nivelActividad.ConRiesgoHasta != matrizIndicadorResultadoIndicador.ConRiesgoHasta || nivelActividad.CriticoPorDebajo != matrizIndicadorResultadoIndicador.CriticoPorDebajo || nivelActividad.CriticoPorEncima != matrizIndicadorResultadoIndicador.CriticoPorEncima || nivelActividad.FormulaId != matrizIndicadorResultadoIndicador.FormulaId || nivelActividad.FuenteInformacion != matrizIndicadorResultadoIndicador.FuenteInformacion || nivelActividad.MedioVerificacion != matrizIndicadorResultadoIndicador.MedioVerificacion) {
                    let _matrizIndicadorResultadoIndicador = $.extend(true, {}, nivelActividad);
                    _matrizIndicadorResultadoIndicador.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(_matrizIndicadorResultadoIndicador.Timestamp)));
                    data.listMatrizIndicadorResultadoIndicador.push(_matrizIndicadorResultadoIndicador);
                }
            } else {
                data.listMatrizIndicadorResultadoIndicador.push(nivelActividad);
            }
        } else {
            nivelActividad.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(nivelActividad.Timestamp)));
            data.listMatrizIndicadorResultadoIndicador.push(nivelActividad);
        }
    });

    // Matriz Indicador Resultado Indicador Meta
    listMatrizIndicadorResultadoIndicadorMeta.map(meta => {
        // Si el estatus es borrado para registrar
        if (meta.EstatusId != ControlMaestroMapeo.EstatusRegistro.BORRADO) {
            // Si el ID es nuevo para registrar o editar
            if (meta.MIRIndicadorMetaId > 0) {
                const matrizIndicadorResultadoIndicadorMeta = _listMatrizIndicadorResultadoIndicadorMeta.find(_meta => _meta.MIRIndicadorId == meta.MIRIndicadorId && _meta.Orden == meta.Orden);
                if (meta.Etiqueta != matrizIndicadorResultadoIndicadorMeta.Etiqueta || meta.Valor != matrizIndicadorResultadoIndicadorMeta.Valor) {
                    let _matrizIndicadorResultadoIndicadorMeta = $.extend(true, {}, meta);
                    _matrizIndicadorResultadoIndicadorMeta.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(_matrizIndicadorResultadoIndicadorMeta.Timestamp)));;
                    data.listMatrizIndicadorResultadoIndicadorMeta.push(_matrizIndicadorResultadoIndicadorMeta);
                }
            } else {
                data.listMatrizIndicadorResultadoIndicadorMeta.push(meta);
            }
        } else {
            meta.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(meta.Timestamp)));
            data.listMatrizIndicadorResultadoIndicadorMeta.push(meta);
        }
    });

    // Matriz Indicador Resultado Indicador Formula Variable
    listMatrizIndicadorResultadoIndicadorFormulaVariable.map(fv => {
        // Si el estatus es borrado para registrar
        if (fv.EstatusId != ControlMaestroMapeo.EstatusRegistro.BORRADO) {
            // Si el ID es nuevo para registrar o editar
            if (fv.MIRIndicadorFormulaVariableId > 0) {
                const matrizIndicadorResultadoIndicadorFormulaVariable = _listMatrizIndicadorResultadoIndicadorFormulaVariable.find(_fv => _fv.MIRIndicadorFormulaVariableId == fv.MIRIndicadorFormulaVariableId);
                if (fv.UnidadMedidaFormulaVariableId != matrizIndicadorResultadoIndicadorFormulaVariable.UnidadMedidaFormulaVariableId || fv.DescripcionVariable != matrizIndicadorResultadoIndicadorFormulaVariable.DescripcionVariable) {
                    let actualizarMatrizIndicadorResultadoIndicadorFormulaVariable = $.extend(true, {}, fv);
                    actualizarMatrizIndicadorResultadoIndicadorFormulaVariable.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(actualizarMatrizIndicadorResultadoIndicadorFormulaVariable.Timestamp)));;
                    data.listMatrizIndicadorResultadoIndicadorFormulaVariable.push(actualizarMatrizIndicadorResultadoIndicadorFormulaVariable);
                }
            } else {
                data.listMatrizIndicadorResultadoIndicadorFormulaVariable.push(fv);
            }
        } else {
            fv.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(fv.Timestamp)));
            data.listMatrizIndicadorResultadoIndicadorFormulaVariable.push(fv);
        }
    });

    return data;
}

guardaNivel = (nivel) => {
    // Validamos que la informacion requerida del Formulario este completa
    var validaForm = dxFormContenido.validate();
    if (!validaForm.isValid) {
        // Validacion y enfocar a la celda
        //validaCampo(validaForm);
        // Enfocar a la celda
        validaForm.brokenRules[0].validator.focus();
        //toast('Revisa los campos que faltan', 'error');
        toast(validaForm.brokenRules[0].message, 'error');
        return;
    }
    // inicializamos el formulario del Meta
    var dxFormMeta = $('#dxFormMeta').dxForm('instance'),
        validaFormMeta = dxFormMeta.validate();
    if (!validaFormMeta.isValid) {
        // Para cambiar la prestania
        if (dxTabPanel.option('selectedIndex') != 1) {
            dxTabPanel.option('selectedIndex', 1);
        }
        validaFormMeta.brokenRules[0].validator.focus();
        toast(validaFormMeta.brokenRules[0].message, 'error');
        return;
    }
        
    // inicializamos el formulario del Formula Variable
    var dxFormFormula = $('#dxFormFormula').dxForm('instance'),
        validaFormFormula = dxFormFormula.validate();
    if (!validaFormFormula.isValid) {
        // Para cambiar la prestania
        if (dxTabPanel.option('selectedIndex') != 3) {
            dxTabPanel.option('selectedIndex', 3);
        }
        validaFormFormula.brokenRules[0].validator.focus();
        toast(validaFormFormula.brokenRules[0].message, 'error');
        return;
    }
        

    var matrizIndicadorResultadoIndicador = dxFormContenido.option('formData');

    // Si el ID ya esta registrado para modificar o no esta registrado para agregar nuevo
    if (esEditar) {
        let index = listMatrizIndicadorResultadoIndicador.findIndex(miri => miri.MIRIndicadorId == matrizIndicadorResultadoIndicador.MIRIndicadorId && miri.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO);
        listMatrizIndicadorResultadoIndicador[index] = $.extend(true, {}, matrizIndicadorResultadoIndicador);
        // Agregar o Editar para Meta
        agregarOEditarMatrizIndicadorResultadoIndicadorMeta(matrizIndicadorResultadoIndicador, dxFormMeta)
        // Agregar o Editar para la Fórmula Variable
        agregarOEditarMatrizIndicadorResultadoIndicadorFormulaVariable(matrizIndicadorResultadoIndicador, dxFormFormula);
    } else {
        listMatrizIndicadorResultadoIndicador.push($.extend(true, {}, matrizIndicadorResultadoIndicador));
        // Agregar o Editar para Meta
        agregarOEditarMatrizIndicadorResultadoIndicadorMeta(matrizIndicadorResultadoIndicador, dxFormMeta);
        // Agregar o Editar para la Fórmula Variable
        agregarOEditarMatrizIndicadorResultadoIndicadorFormulaVariable(matrizIndicadorResultadoIndicador, dxFormFormula);
    }
    // Cambiamos el valor de formulario
    esFormulario = false;
    // Habilitamos los botones de acciones
    habilitaComponentes(true);
    // Cerramos las pestañas del Nivel
    cargaContenidoSwitch();
}

//validaCampo = (validaForm) => {
//    let esCambiaPestania = false;
//    const campo = validaForm.brokenRules[0].validator.option('adapter').editor.option('name');
//    var tabPanelOptions = $.extend(true, {}, dxFormContenido.itemOption('DatosControlCalculo.DatosControlCalculoPestania').tabPanelOptions);
//    // Para Pestania
//    if (campo == 'AnioBase' || campo == 'ValorBase' || campo == 'DescripcionBase') {
//        tabPanelOptions.selectedIndex = 0;
//        esCambiaPestania = true;
//    }
//    if (campo == 'AceptableDesde' || campo == 'AceptableHasta' || campo == 'ConRiesgoDesde' || campo == 'ConRiesgoHasta' || campo == 'CriticoPorDebajo' || campo == 'CriticoPorEncima') {
//        tabPanelOptions.selectedIndex = 2;
//        esCambiaPestania = true;
//    }
//    if (campo == 'FuenteInformacion' || campo == 'MedioVerificacion') {
//        tabPanelOptions.selectedIndex = 4;
//        esCambiaPestania = true;
//    }
//    console.log(validaForm.brokenRules[0])
//    if (esCambiaPestania) {
//        // Actualizamos la validacion otra vez por cambiar pestania
//        validaForm = dxFormContenido.validate();
//    }
//    // Enfocar a la celda
//    validaForm.brokenRules[0].validator.focus();
//}

agregarOEditarMatrizIndicadorResultadoIndicadorMeta = (matrizIndicadorResultadoIndicador, dxFormMeta) => {
    if (esEditar) {
        var itemsMeta = dxFormMeta.option('items')[0].items,
            __listMatrizIndicadorResultadoIndicadorMeta = listMatrizIndicadorResultadoIndicadorMeta.filter(mirim => mirim.MIRIndicadorId == matrizIndicadorResultadoIndicador.MIRIndicadorId && mirim.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO);
        if (itemsMeta.length == __listMatrizIndicadorResultadoIndicadorMeta.length) {
            itemsMeta.map((meta, index) => {
                __listMatrizIndicadorResultadoIndicadorMeta.filter(mirim => mirim.Orden == index).map(mirim => {
                    mirim.Etiqueta = meta.label.text;
                    mirim.Valor = dxFormMeta.getEditor(meta.dataField).option('value');
                });
            });
        } else {
            const eliminarListmatrizIndicadorResultadoIndicadorMeta = $.extend(true, [], __listMatrizIndicadorResultadoIndicadorMeta);
            // Verificar si el ID ya esta resgitrado solo cambiar el estatus o no esta registrado eliminar
            if (eliminarListmatrizIndicadorResultadoIndicadorMeta.some(mirim => mirim.MIRIndicadorMetaId > 0)) {
                __listMatrizIndicadorResultadoIndicadorMeta.map(mirim => {
                    mirim.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO;
                });
            } else {
                eliminarListmatrizIndicadorResultadoIndicadorMeta.map(mirim => {
                    let index = listMatrizIndicadorResultadoIndicadorMeta.findIndex(_mirim => _mirim.MIRIndicadorMetaId == mirim.MIRIndicadorMetaId);
                    listMatrizIndicadorResultadoIndicadorMeta.splice(index, 1);
                });
            }
            // Agregar nuevo mirim
            itemsMeta.map((meta, index) => {
                let matrizIndicadorResultadoIndicadorMetaModel = $.extend(true, {}, _matrizIndicadorResultadoIndicadorMetaModel);
                matrizIndicadorResultadoIndicadorMetaModel.MIRIndicadorMetaId = contadorRegistrosNuevos;
                matrizIndicadorResultadoIndicadorMetaModel.MIRIndicadorId = matrizIndicadorResultadoIndicador.MIRIndicadorId;
                matrizIndicadorResultadoIndicadorMetaModel.Etiqueta = meta.label.text;
                matrizIndicadorResultadoIndicadorMetaModel.Valor = dxFormMeta.getEditor(meta.dataField).option('value');
                matrizIndicadorResultadoIndicadorMetaModel.Orden = index;
                matrizIndicadorResultadoIndicadorMetaModel.EstatusId = ControlMaestroMapeo.EstatusRegistro.ACTIVO;
                // Agregar el dato de matriz indicador resultado indicador meta a lista
                listMatrizIndicadorResultadoIndicadorMeta.push($.extend(true, {}, matrizIndicadorResultadoIndicadorMetaModel));

                contadorRegistrosNuevos--;
            });
        }
    } else {
        // Mensual
        var itemsMeta = dxFormMeta.option('items')[0].items;
        itemsMeta.map((meta, index) => {
            let matrizIndicadorResultadoIndicadorMetaModel = $.extend(true, {}, _matrizIndicadorResultadoIndicadorMetaModel);
            matrizIndicadorResultadoIndicadorMetaModel.MIRIndicadorMetaId = contadorRegistrosNuevos;
            matrizIndicadorResultadoIndicadorMetaModel.MIRIndicadorId = matrizIndicadorResultadoIndicador.MIRIndicadorId;
            matrizIndicadorResultadoIndicadorMetaModel.Etiqueta = meta.label.text;
            matrizIndicadorResultadoIndicadorMetaModel.Valor = dxFormMeta.getEditor(meta.dataField).option('value');
            matrizIndicadorResultadoIndicadorMetaModel.Orden = index;
            matrizIndicadorResultadoIndicadorMetaModel.EstatusId = ControlMaestroMapeo.EstatusRegistro.ACTIVO;
            // Agregar el dato de matriz indicador resultado indicador meta a lista
            listMatrizIndicadorResultadoIndicadorMeta.push($.extend(true, {}, matrizIndicadorResultadoIndicadorMetaModel));

            contadorRegistrosNuevos--;
        });
    }
}

agregarOEditarMatrizIndicadorResultadoIndicadorFormulaVariable = (matrizIndicadorResultadoIndicador, dxFormFormula) => {
    if (esEditar) {
        var getListMatrizIndicadorResultadoIndicadorFormulaVariable = dxFormFormula.option('formData'),
            __listMatrizIndicadorResultadoIndicadorFormulaVariable = listMatrizIndicadorResultadoIndicadorFormulaVariable.filter(fv => fv.MIRIndicadorId == matrizIndicadorResultadoIndicador.MIRIndicadorId && fv.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO),
            listUnidadMedidaFormulaVariableId = [];
        getListMatrizIndicadorResultadoIndicadorFormulaVariable.map(fv => listUnidadMedidaFormulaVariableId.push(fv.UnidadMedidaFormulaVariableId));

        if (listMatrizIndicadorResultadoIndicadorFormulaVariable.some(fv => fv.MIRIndicadorId == matrizIndicadorResultadoIndicador.MIRIndicadorId && listUnidadMedidaFormulaVariableId.indexOf(fv.UnidadMedidaFormulaVariableId) >= 0 && fv.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
            getListMatrizIndicadorResultadoIndicadorFormulaVariable.map(fv => {
                var actualizarMatrizIndicadorResultadoIndicadorFormulaVariable = __listMatrizIndicadorResultadoIndicadorFormulaVariable.find(_fv => _fv.UnidadMedidaFormulaVariableId == fv.UnidadMedidaFormulaVariableId);
                if (fv.DescripcionVariable != actualizarMatrizIndicadorResultadoIndicadorFormulaVariable.DescripcionVariable)
                    actualizarMatrizIndicadorResultadoIndicadorFormulaVariable.DescripcionVariable = fv.DescripcionVariable;
            });
        } else {
            // Eliminar
            let eliminarListMatrizIndicadorResultadoIndicadorFormulaVariable = [];
            __listMatrizIndicadorResultadoIndicadorFormulaVariable.map(fv => {
                if (fv.MIRIndicadorFormulaVariableId > 0) {
                    fv.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO;
                } else {
                    eliminarListMatrizIndicadorResultadoIndicadorFormulaVariable.push(fv);
                }
            });
            eliminarListMatrizIndicadorResultadoIndicadorFormulaVariable.map(fv => {
                let index = listMatrizIndicadorResultadoIndicadorFormulaVariable.findIndex(_fv => _fv.MIRIndicadorFormulaVariableId == fv.MIRIndicadorFormulaVariableId);
                listMatrizIndicadorResultadoIndicadorFormulaVariable.splice(index, 1);
            })
            // Agregar nuevo MIRIFV
            getListMatrizIndicadorResultadoIndicadorFormulaVariable.map(fv => {
                let nuevoMatrizIndicadorResultadoIndicadorFormulaVariableModel = $.extend(true, {}, _matrizIndicadorResultadoIndicadorFormulaVariableModel);
                nuevoMatrizIndicadorResultadoIndicadorFormulaVariableModel.MIRIndicadorFormulaVariableId = contadorRegistrosNuevos;
                nuevoMatrizIndicadorResultadoIndicadorFormulaVariableModel.MIRIndicadorId = fv.MIRIndicadorId;
                nuevoMatrizIndicadorResultadoIndicadorFormulaVariableModel.UnidadMedidaFormulaVariableId = fv.UnidadMedidaFormulaVariableId;
                nuevoMatrizIndicadorResultadoIndicadorFormulaVariableModel.DescripcionVariable = fv.DescripcionVariable;
                nuevoMatrizIndicadorResultadoIndicadorFormulaVariableModel.EstatusId = ControlMaestroMapeo.EstatusRegistro.ACTIVO;
                // Agregar el dato de matriz indicador resultado indicador formula variable a lista
                listMatrizIndicadorResultadoIndicadorFormulaVariable.push(nuevoMatrizIndicadorResultadoIndicadorFormulaVariableModel);

                contadorRegistrosNuevos--;
            });
        }
    } else {
        // Nuevo
        var getListMatrizIndicadorResultadoIndicadorFormulaVariable = dxFormFormula.option('formData');
        getListMatrizIndicadorResultadoIndicadorFormulaVariable.map(fv => {
            let nuevoMatrizIndicadorResultadoIndicadorFormulaVariableModel = $.extend(true, {}, _matrizIndicadorResultadoIndicadorFormulaVariableModel);
            nuevoMatrizIndicadorResultadoIndicadorFormulaVariableModel.MIRIndicadorFormulaVariableId = contadorRegistrosNuevos;
            nuevoMatrizIndicadorResultadoIndicadorFormulaVariableModel.MIRIndicadorId = fv.MIRIndicadorId;
            nuevoMatrizIndicadorResultadoIndicadorFormulaVariableModel.UnidadMedidaFormulaVariableId = fv.UnidadMedidaFormulaVariableId;
            nuevoMatrizIndicadorResultadoIndicadorFormulaVariableModel.DescripcionVariable = fv.DescripcionVariable;
            nuevoMatrizIndicadorResultadoIndicadorFormulaVariableModel.EstatusId = ControlMaestroMapeo.EstatusRegistro.ACTIVO;
            // Agregar el dato de matriz indicador resultado indicador formula variable a lista
            listMatrizIndicadorResultadoIndicadorFormulaVariable.push(nuevoMatrizIndicadorResultadoIndicadorFormulaVariableModel);

            contadorRegistrosNuevos--;
        });
    }
}

// Template //
templateEnTabDondeForm = (etiqueta) => {
    return "<div class='pixvs-position-v-h-center'><span class='dx-field-item-label-text'>" + etiqueta + "</span></div>";
}

// Este es un funcion para carga de la meta a cual: Mensual, Trimestral, Semestral, Anual, Bianual, Trianual, Sexenal
cargaTemplateMeta = (frecuenciaMedicionId, nivel) => {
    if (!nivel) {
        $('#dxFormMeta').empty();
    } else {
        $('#dxFormMeta').dxForm({
            labelLocation: 'top',
            requiredMark: '(*)',
            onFieldDataChanged: (event) => {
                if (!esEditar)
                    if (!listaMetaGlobal.find(meta => meta.dataField == event.dataField)) {
                        listaMetaGlobal.push(event);
                    }
            },
            readOnly: esModoLectura,
            items: obtenerItemsFrecuenciaMedicion(frecuenciaMedicionId)
        });
    }
}

obtenerItemsFrecuenciaMedicion = (FrecuenciaMedicionId) => {
    switch (FrecuenciaMedicionId) {
        case ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.MENSUAL:
            return cargaMensual();
        case ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.TRIMESTRAL:
            return cargaTrimestral();
        case ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.SEMESTRAL:
            return cargaSemestral();
        case ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.ANUAL:
            return cargaAnual();
        case ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.BIANUAL:
            return cargaBianual();
        case ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.TRIANUAL:
            return cargaTrianual();
        case ControlMaestroFrecuenciaMedicionMapeo.FrecuenciaMedicion.SEXENAL:
            return cargaSexenal();
    }
}

// Mensual
cargaMensual = () => {
    return [{
        itemType: 'group',
        colCount: 12,
        cssClass: 'mb-4',
        items: [{
            colSpan: 3,
            dataField: 'Variable1',
            isRequired: true,
            label: {
                text: 'Enero'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable2',
            isRequired: true,
            label: {
                text: 'Febrero'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable3',
            isRequired: true,
            label: {
                text: 'Marzo'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable4',
            isRequired: true,
            label: {
                text: 'Abril'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable5',
            isRequired: true,
            label: {
                text: 'Mayo'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable6',
            isRequired: true,
            label: {
                text: 'Junio'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable7',
            isRequired: true,
            label: {
                text: 'Julio'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable8',
            isRequired: true,
            label: {
                text: 'Agosto'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable9',
            isRequired: true,
            label: {
                text: 'Septiembre'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable10',
            isRequired: true,
            label: {
                text: 'Octubre'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable11',
            isRequired: true,
            label: {
                text: 'Noviembre'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable12',
            isRequired: true,
            label: {
                text: 'Diciembre'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }]
    }];
}
// Trimestral
cargaTrimestral = () => {
    return [{
        itemType: 'group',
        colCount: 12,
        cssClass: 'mb-4',
        items: [{
            colSpan: 3,
            dataField: 'Variable1',
            isRequired: true,
            label: {
                text: 'Primer Trimestre'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable2',
            isRequired: true,
            label: {
                text: 'Segundo Trimestre'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable3',
            isRequired: true,
            label: {
                text: 'Tercer Trimestre'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable4',
            isRequired: true,
            label: {
                text: 'Cuarto Trimestre'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }]
    }];
}
// Semestral
cargaSemestral = () => {
    return [{
        itemType: 'group',
        colCount: 12,
        cssClass: 'mb-4',
        items: [{
            colSpan: 3,
            dataField: 'Variable1',
            isRequired: true,
            label: {
                text: 'Primer Semestre'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable2',
            isRequired: true,
            label: {
                text: 'Segundo Semestre'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }]
    }];
}
// Anual
cargaAnual = () => {
    return [{
        itemType: 'group',
        colCount: 12,
        cssClass: 'mb-4',
        items: [{
            colSpan: 3,
            dataField: 'Variable1',
            isRequired: true,
            label: {
                text: 'Año actual'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }]
    }];
}
// Bianual
cargaBianual = () => {
    return [{
        itemType: 'group',
        colCount: 12,
        cssClass: 'mb-4',
        items: [{
            colSpan: 3,
            dataField: 'Variable1',
            isRequired: true,
            label: {
                text: 'Primer Año'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable2',
            isRequired: true,
            label: {
                text: 'Segundo Año'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }]
    }];
}
// Trianual
cargaTrianual = () => {
    return [{
        itemType: 'group',
        colCount: 12,
        cssClass: 'mb-4',
        items: [{
            colSpan: 3,
            dataField: 'Variable1',
            isRequired: true,
            label: {
                text: 'Primer Año'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable2',
            isRequired: true,
            label: {
                text: 'Segundo Año'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }, {
            colSpan: 3,
            dataField: 'Variable3',
            isRequired: true,
            label: {
                text: 'Tercer Año'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }]
    }];
}
// Sexenal
cargaSexenal = () => {
    return [{
        itemType: 'group',
        colCount: 12,
        cssClass: 'mb-4',
        items: [{
            colSpan: 3,
            dataField: 'Variable1',
            isRequired: true,
            label: {
                text: 'Sexenio'
            },
            editorType: 'dxNumberBox',
            editorOptions: {
                format: '#,##0.##',
                elementAttr: {
                    class: 'text-right'
                }
                /*onValueChanged: (event) => onValueChangedMeta(event, nivel)*/
            }
        }]
    }];
}
// Formula Variable
templateFormula = (mirIndicadorId, unidadMedidaId) => {
    // Crear las variables
    let listFormulaVariable = [],
        itemsListFormulaVariable = [],
        listUnidadMedidaFormulaVariableId = [];
    _listControlMaestroUnidadMedidaFormulaVariable.filter(fv => fv.UnidadMedidaId == unidadMedidaId).map(fv => listUnidadMedidaFormulaVariableId.push(fv.UnidadMedidaFormulaVariableId));
    // Busca si hay los registros de formula variable o no para pasar los datos.
    if (listMatrizIndicadorResultadoIndicadorFormulaVariable.some(fv => fv.MIRIndicadorId == mirIndicadorId && listUnidadMedidaFormulaVariableId.indexOf(fv.UnidadMedidaFormulaVariableId) >= 0 && fv.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
        listMatrizIndicadorResultadoIndicadorFormulaVariable.filter(fv => fv.MIRIndicadorId == mirIndicadorId && fv.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).map(fv => {
            listFormulaVariable.push(fv);
        });
    } else {
        _listControlMaestroUnidadMedidaFormulaVariable.filter(fv => fv.UnidadMedidaId == unidadMedidaId).map(fv => {
            let matrizIndicadorResultadoIndicadorFormulaVariableModel = $.extend(true, {}, _matrizIndicadorResultadoIndicadorFormulaVariableModel);
            matrizIndicadorResultadoIndicadorFormulaVariableModel.MIRIndicadorFormulaVariableId = 0;
            matrizIndicadorResultadoIndicadorFormulaVariableModel.MIRIndicadorId = mirIndicadorId;
            matrizIndicadorResultadoIndicadorFormulaVariableModel.UnidadMedidaFormulaVariableId = fv.UnidadMedidaFormulaVariableId;
            matrizIndicadorResultadoIndicadorFormulaVariableModel.DescripcionVariable = fv.Variable;
            listFormulaVariable.push(matrizIndicadorResultadoIndicadorFormulaVariableModel);
        });
    }
    // Agregar un arreglo para el item de el formulario
    listFormulaVariable.map((fv, index) => {
        itemsListFormulaVariable.push({
            colSpan: 12,
            dataField: '' + index + '.DescripcionVariable',
            isRequired: true,
            validationRules: [{
                type: 'required',
                message: 'Descripción Variable ' + (index + 1) + ' requerida'
            }],
            label: {
                text: 'Descripción variable ' + (index + 1)
            },
            editorType: 'dxTextBox'
        });
    });
    // Crear el formulario de fórmula variable
    if (listFormulaVariable) {
        $('#dxFormFormula').dxForm({
            formData: listFormulaVariable,
            labelLocation: 'top',
            requiredMark: '(*)',
            items: [{
                itemType: 'group',
                colCount: 12,
                items: itemsListFormulaVariable
            }]
        });
    } else {
        $('#dxFormFormula').dxForm({
            labelLocation: 'top',
            items: [{
                itemType: 'empty'
            }]
        });
    }
}
//////////////

// Toast //
toast = (message, type) => {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 3500);
}
///////////