//Componentes
var dxCboAlmacenes;
var dxCboGrupos;
var dxCboSubGrupos;
var dxCboClases;
var dxCboPartidasEspecificas;
var dxCboProductos;
var dxButtonAgregar;

var modalComentario;
var modalConfirmaEliminarArticulo;
var modalConfirmaDeshacerCambios;
var modalConfirmaEliminar;
var modalConfirmaAfectar;

var dxButtonDeshacer;
var dxButtonCancelar;
var dxButtonEliminar;
var dxButtonGuardaCambios;
var dxButtonAfectar;
var dxButtonImprimirFormato;

var dxForm;
var dxGridDetalles;
var dxFormModalComentario;

//Variables de Control
var cambios;
var eventoRegresar;
var inventarioId;
var rowEliminar;
var detallesEliminados;

var almacenId;
var almacen;
var partidasEspecificas;
var productosIds;

//Variables Estatus Detalles
var ESTATUS_EN_PROCESO = 32;
var ESTATUS_TERMINADO = 33;
var ESTATUS_CANCELADO = 34;

var API_FICHA = "/almacenes/almacenes/inventariosfisicos/";

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();

    //Respaldamos el modelo del Form
    getForm();

    //Deshabilitamos los botones de acciones
    habilitaComponentes();
});

var mostrar = function () {
    var existencias;

    //Obtenemos todos los registros que hay en el dxGridDetalles
    dxGridDetalles.getDataSource().store().load().done((res) => { existencias = res; });

    //Validamos que la informacion requerida del Formulario este completa
    return !existencias && !existencias.length;
}

var inicializaVariables = function () {
    dxCboAlmacenes = $('#dxCboAlmacenes').dxSelectBox("instance");
    dxCboGrupos = $('#dxCboGrupos').dxDropDownBox("instance");
    dxCboSubGrupos = $('#dxCboSubGrupos').dxDropDownBox("instance");
    dxCboClases = $('#dxCboClases').dxDropDownBox("instance");
    dxCboPartidasEspecificas = $('#dxCboPartidasEspecificas').dxDropDownBox("instance");
    dxCboProductos = $('#dxCboProductos').dxDropDownBox("instance");
    dxButtonAgregar = $('#dxButtonAgregar').dxButton("instance");

    modalComentario = $('#modalComentario');
    modalConfirmaEliminarArticulo = $('#modalConfirmaEliminarArticulo');
    modalConfirmaDeshacerCambios = $('#modalConfirmaDeshacerCambios');
    modalConfirmaEliminar = $('#modalConfirmaEliminar');
    modalConfirmaAfectar = $('#modalConfirmaAfectar');

    dxButtonDeshacer = $('#dxButtonDeshacer').dxButton("instance");
    dxButtonCancelar = $('#dxButtonCancelar').dxButton("instance");
    dxButtonEliminar = $('#dxButtonEliminar').dxButton("instance");
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");
    dxButtonAfectar = $('#dxButtonAfectar').dxButton("instance");
    dxButtonImprimirFormato = $('#dxButtonImprimirFormato').dxDropDownButton("instance");

    dxForm = $("#dxForm").dxForm("instance");
    dxGridDetalles = $('#dxGridDetalles').dxDataGrid("instance");
    dxFormModalComentario = $("#dxFormModalComentario").dxForm("instance");

    $("#dxTotalAjuste").css("border", "none");

    detallesEliminados = [];
    cambios = false;
    eventoRegresar = true;

    var estatusId = getForm().EstatusId;

    if ((!estatusId || estatusId == ESTATUS_EN_PROCESO) && !_fechaOperacion) {
        //Mostramos mensaje
        toast("Es necesario realizar la Configuración de Fechas correspondiente.", 'warning');
    }
}

var getForm = function () {
    var modelo = $.extend(true, {}, dxForm.option('formData'));
    modelo.InventarioFisicoId = modelo.InventarioFisicoId == null ? 0 : modelo.InventarioFisicoId;
    inventarioId = modelo.InventarioFisicoId;

    if (inventarioId == 0) {
        modelo.AlmacenId = almacenId;
        modelo.Almacen = almacen;
    } else {
        almacenId = modelo.AlmacenId;
        almacen = modelo.Almacen;
    }

    return modelo;
}

var setCambios = function () {
    cambios = true;
    habilitaComponentes();
}

var habilitaComponentes = function () {
    var estatusId = getForm().EstatusId;
    estatusId = !estatusId ? ESTATUS_EN_PROCESO : estatusId;

    var mostrarBotones = estatusId == ESTATUS_EN_PROCESO;

    if (dxCboAlmacenes) {
        dxCboAlmacenes.option("disabled", inventarioId > 0);
    }

    dxButtonAgregar.option("disabled", (!productosIds || !productosIds.length));
    dxButtonAgregar.option("visible", inventarioId == 0);
    dxButtonDeshacer.option("disabled", !cambios);
    dxButtonDeshacer.option("visible", mostrarBotones);
    dxButtonEliminar.option("disabled", cambios);
    dxButtonEliminar.option("visible", mostrarBotones && inventarioId && !cambios);
    dxButtonGuardaCambios.option("text", inventarioId > 0 ? "Guardar" : "Iniciar");
    dxButtonGuardaCambios.option("visible", mostrarBotones && (inventarioId == 0 || cambios));
    dxButtonAfectar.option("visible", mostrarBotones && inventarioId > 0 && !cambios);
    dxButtonImprimirFormato.option("visible", inventarioId > 0);
}

var editaComentario = function (event) {
    //Obtenemos una copia del objeto a modificar
    var existencia = $.extend(true, {}, event.row.data);

    //Le pasamos el objeto al Form para que cargue sus valores
    dxFormModalComentario.option("formData", existencia);

    //Mostramos el modal
    modalComentario.modal('show');
}

var guardaCambiosModal = function () {
    //Obtenemos el Objeto que se esta creando/editando en el Form 
    var existencia = dxFormModalComentario.option("formData");

    //Validamos que la informacion requerida del Formulario este completa
    if (!dxFormModalComentario.validate().isValid)
        return;

    //Obtenemos la instancia store del DataSource
    var store = dxGridDetalles.getDataSource().store();

    store.update(existencia.AlmacenProductoId, existencia)
        .done(function () {
            //Recargamos la informacion de la tabla
            dxGridDetalles.getDataSource().reload();

            //Habilitamos los botones de acciones
            setCambios(true);

            //Ocultamos el modal
            modalComentario.modal('hide');
        })
        .fail(function () {
            toast("No se pudo actualizar el registro en la tabla.", "error");
        });
}

var validaEliminarArticulo = function (event) {
    //Obtenemos una copia del objeto a eliminar
    rowEliminar = $.extend(true, {}, event.row.data);

    //Mostramos el modal de confirmacion
    modalConfirmaEliminarArticulo.modal('show');
}

var eliminaArticulo = function () {
    //Obtenemos la instancia store del DataSource
    var store = dxGridDetalles.getDataSource().store();

    //Eliminamos el registro de la tabla
    if (rowEliminar != null) {
        store.remove(rowEliminar.AlmacenProductoId)
            .done(function () {
                //Si el registro viene de la base de datos, respaldamos el registro 
                //para posteriormente eliminarlo en la base de datos
                if (rowEliminar.InventarioFisicoDetalleId > 0) {
                    //Actualizamos el estatus del registro a "Eliminado"
                    rowEliminar.Borrado = true;

                    //Respaldamos el registro que se acaba de eliminar
                    detallesEliminados.push(rowEliminar);
                }

                //Recargamos la informacion de la tabla
                dxGridDetalles.getDataSource().reload();

                setCambios();
            })
            .fail(function () {
                toast("No se pudo eliminar el registro de la tabla.", "error");
            });

        rowEliminar = null;
    }
}

var validaDeshacer = function (regresar) {
    eventoRegresar = regresar;

    if (cambios) {
        //Mostramos el modal de confirmacion
        modalConfirmaDeshacerCambios.modal('show');
    } else {
        cancelar();
    }
}

var cancelar = function () {
    if (eventoRegresar) {
        //Regresamos al listado
        regresarListado();
    } else {
        //Recargamos la ficha
        recargarFicha();
    }
}

var validaEliminar = function () {
    //Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}

var eliminar = function () {
    //Mostramos Loader
    dxLoaderPanel.show();

    //Enviamos la informacion al controlador
    $.ajax({
        type: "POST",
        url: API_FICHA + "eliminar",
        data: { inventarioFisico: getForm() },
        success: function () {
            //Mostramos mensaje de Exito
            toast("Registro borrado con exito!", 'success');

            //Regresamos al listado
            regresarListado();
        },
        error: function (response, status, error) {
            //Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al guadar:\n" + response.responseText, 'error');
        }
    });
}

var guardaCambios = function () {
    var existencias;

    //Obtenemos todos los registros que hay en el dxGridDetalles
    dxGridDetalles.getDataSource().store().load().done((res) => { existencias = res; });    

    //Validamos que la informacion requerida del Formulario este completa
    if (!existencias || !existencias.length) {
        toast("Favor de agregar artículos para inventariar.", 'error');

        return;
    }

    //Agregamos los registros borrados, para eliminarlos en base de datos
    existencias = $.merge(existencias, detallesEliminados);

    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "guardaCambios",
        data: { inventarioFisico: getForm(), existencias: existencias },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Registro guardado con exito!", 'success');

            inventarioId = response;

            //Recargamos la ficha
            recargarFicha();
        },
        error: function (response, status, error) {
            //Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al guadar:\n" + response.responseText, 'error');
        }
    });
}

var validaAfectar = function () {
    var existencias;
    var valido = true;

    //Obtenemos todos los registros que hay en el dxGridDetalles
    dxGridDetalles.getDataSource().store().load().done((res) => { existencias = res; });    

    existencias.forEach(element => {
            if (!element.Conteo) {
                toast("Favor de completar el conteo de todos los artículos.", 'error');
                valido = false;
                return;
            }
        }
    );

    if (valido) {
        //Mostramos el modal de confirmacion
        modalConfirmaAfectar.modal('show');
    }
}

var afectar = function () {
    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "afectar",
        data: { inventarioFisico: getForm() },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Inventario afectado con exito!", 'success');

            //Recargamos la ficha
            recargarFicha();
        },
        error: function (response, status, error) {
            //Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al guadar:\n" + response.responseText, 'error');
        }
    });
}

var regresarListado = function () {
    window.location.href = API_FICHA + "listar";
}

var recargarFicha = function () {
    //Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA + (inventarioId == 0 ? "nuevo" : "editar/" + inventarioId);
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}

var cboGruposChange = function (e) {
    cboObjetosGastoChange(e, 1);
}

var cboSubGruposChange = function (e) {
    cboObjetosGastoChange(e, 2);
}

var cboClasesChange = function (e) {
    cboObjetosGastoChange(e, 3);
}

var cboObjetosGastoChange = function (e, nivel) {
    var value = e.value;
    var content = e.component.content();
    var cboSiguiente;

    if (content) {
        var treeView = content.find(".dx-treeview");

        if (treeView.length) {
            syncTreeViewSelection(treeView.dxTreeView("instance"), value);
        }
    }

    switch (nivel) {
        case 1:
            cboSiguiente = dxCboSubGrupos;
        break;

        case 2:
            cboSiguiente = dxCboClases;
        break;

        case 3:
            cboSiguiente = dxCboPartidasEspecificas;
        break;
    }

    cboSiguiente.option("value", null);
    cboSiguiente.option("dataSource", null);
    cboSiguiente.option("contentTemplate", null);
    cboSiguiente.option("contentTemplate", $("#EmbeddedTreeViewMultiple"));

    if (value && value.length) {
        //Mostramos Loader
        dxLoaderPanel.show();

        //Enviamos la informacion al controlador
        $.ajax({
            type: "POST",
            url: API_FICHA + "buscaObjetoGastoPorNivel",
            data: { iniciales: value, nivel: nivel },
            success: function (response) {
                //Ocultamos Loader
                dxLoaderPanel.hide();

                cboSiguiente.option("dataSource", response);
            },
            error: function (response, status, error) {
                //Ocultamos Loader
                dxLoaderPanel.hide();

                //Mostramos mensaje de error
                toast("Error:\n" + response.responseText, 'error');
            }
        });
    }

    setCambios();
}

var cboAlmacenChange = function (e) {
    //Limpiamos y recargamos la informacion de la tabla
    var dataSource = dxGridDetalles.getDataSource();
    
    dataSource.store().clear();
    dataSource.reload();

    almacenId = e.value;
    almacen = e.component.option("text");

    llenarCboProductos();
}

var cboPartidasEspecificasChange = function (e) {
    var value = e.value;
    var content = e.component.content();

    if (content) {
        var treeView = content.find(".dx-treeview");

        if (treeView.length) {
            syncTreeViewSelection(treeView.dxTreeView("instance"), value);
        }
    }

    partidasEspecificas = value;

    llenarCboProductos();
}

var llenarCboProductos = function () {
    dxCboProductos.option("value", null);
    dxCboProductos.option("dataSource", null);
    dxCboProductos.option("contentTemplate", null);
    dxCboProductos.option("contentTemplate", $("#EmbeddedTreeViewMultiple"));
    dxCboProductos.option("disabled", true);

    if (almacenId && almacenId.length) {
        if (partidasEspecificas && partidasEspecificas.length) {
            //Mostramos Loader
            dxLoaderPanel.show();

            //Enviamos la informacion al controlador
            $.ajax({
                type: "POST",
                url: API_FICHA + "buscaProductosPorAlmacenObjetoGasto",
                data: { almacenId: almacenId, objetosGastoId: partidasEspecificas },
                success: function (response) {
                    //Ocultamos Loader
                    dxLoaderPanel.hide();

                    dxCboProductos.option("dataSource", response);
                    dxCboProductos.option("disabled", false);
                },
                error: function (response, status, error) {
                    //Ocultamos Loader
                    dxLoaderPanel.hide();

                    //Mostramos mensaje de error
                    toast("Error:\n" + response.responseText, 'error');
                }
            });
        }
    } else {
        toast("Error:\n" + 'Favor de seleccionar un Almacén', 'error');
    }

    setCambios();
}

var cboProductosChange = function (e) {
    var value = e.value;
    var content = e.component.content();

    if (content) {
        var treeView = content.find(".dx-treeview");

        if (treeView.length) {
            syncTreeViewSelection(treeView.dxTreeView("instance"), value);
        }
    }

    productosIds = value;

    setCambios();
}

var agregarProductos = function () {
    dxCboAlmacenes.option("disabled", true);

    var listadoExistencias;

    //Mostramos Loader
    dxLoaderPanel.show();

    //Enviamos la informacion al controlador
    $.ajax({
        type: "POST",
        url: API_FICHA + "consultaExistenciaAlmacen",
        data: { almacenId: almacenId, productosIds: productosIds.toString() },
        success: function (response) {
            //Ocultamos Loader
            dxLoaderPanel.hide();

            listadoExistencias = response;

            if (listadoExistencias && listadoExistencias.length) {
                //Obtenemos la instancia store del DataSource
                var store = dxGridDetalles.getDataSource().store();

                listadoExistencias.forEach(element =>
                    store.insert(element)
                        .done(function () {
                            //Recargamos la informacion de la tabla
                            dxGridDetalles.getDataSource().reload();
                        })
                        .fail(function () {
                            //toast("Error:\n No se pudieron agregar los registros a la tabla.", "error");
                        })
                );
            }
        },
        error: function (response, status, error) {
            //Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error:\n" + response.responseText, 'error');
        }
    });
}

var syncTreeViewSelection = function (treeView, value) {
    if (!value) {
        treeView.unselectAll();
        return;
    }

    value.forEach(function (key) {
        treeView.selectItem(key);
    });
}

var onOptionChanged = function (e) {
    if (e.name === "editing") {
        var editRowKey = e.component.option("editing.editRowKey"),
            changes = e.component.option("editing.changes");

        changes = changes.map((change) => {
            return {
                type: change.type,
                key: change.type !== "insert" ? change.key : undefined,
                data: change.data
            };
        });

        if (changes && changes.length) {
            setCambios();
        }
    }
};

var onCellPrepared = function (e) {
    var tooltip = $('#tooltip').dxTooltip("instance");
    var propiedad = e.column.dataField;
    var MostrarTooltip = ["UnidadAdministrativaId", "ProyectoId", "FuenteFinanciamientoId", "TipoGastoId"];

    if (e.rowType == "data" && MostrarTooltip.includes(propiedad)) {
        e.cellElement.mouseover(function () {
            tooltip.option("contentTemplate", null);

            switch (propiedad) {
                case "UnidadAdministrativaId":
                    tooltip.option("contentTemplate", document.createTextNode(e.data.UnidadAdministrativa));
                    break;

                case "ProyectoId":
                    tooltip.option("contentTemplate", document.createTextNode(e.data.Proyecto));
                    break;

                case "FuenteFinanciamientoId":
                    tooltip.option("contentTemplate", document.createTextNode(e.data.FuenteFinanciamiento));
                    break;

                case "TipoGastoId":
                    tooltip.option("contentTemplate", document.createTextNode(e.data.TipoGasto));
                    break;
            }

            tooltip.option("target", e.cellElement);
            tooltip.show();
        });

        e.cellElement.mouseout(function () {
            tooltip.hide();
        });
    }
}

var imprimeFormato = function (e) {
    if (e != null && e.itemData.id != null) {
        window.open(API_FICHA + (e.itemData.id == 1 ? "RptLibroAlmacenPorProducto/" : "RptLibroAlmacenPorClave/") + getForm().InventarioFisicoId);
    }
}