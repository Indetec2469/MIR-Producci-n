//Componentes
var cboAreas;

//Modales
var modalModelo;
var modalConfirmaDeshacerCambios;
var modalConfirmaEliminar;

//Botones
var dxButtonDeshacer;
var dxButtonGuardaCambios;

//Forms
var dxGridListado;
var dxFormModalModelo;
var dxGridDetalles;
var dxGridAlmacenes;

//Variables Globales
var modeloVacio;
var rowEliminar;
var registrosEliminados;

//Variables de Control
var contadorRegistrosNuevos;

//Variables Estaticas
var ESTATUS_CARGADO = 0;
var ESTATUS_NUEVO = 1;
var ESTATUS_EDITADO = 2;
var ESTATUS_ELIMINADO = 3;

var API_FICHA = "/compras/catalogos/configuracionareas/";

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();

    //Deshabilitamos los botones de acciones
    habilitaComponentes(false);

    //Respaldamos el modelo vacio del Form
    modeloVacio = $.extend(true, {}, dxFormModalModelo.option('formData'));
    modeloVacio.Detalles = [];
    modeloVacio.ListAlmacenes = [];
});

var inicializaVariables = function () {
    cboAreas = $('#dxCboAreas').dxSelectBox("instance");

    modalModelo = $('#modalModelo');
    modalConfirmaDeshacerCambios = $('#modalConfirmaDeshacerCambios');
    modalConfirmaEliminar = $('#modalConfirmaEliminar');

    dxButtonDeshacer = $('#dxButtonDeshacer').dxButton("instance");
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");

    dxGridListado = $('#dxGridListado').dxDataGrid("instance");
    dxFormModalModelo = $("#dxFormModalModelo").dxForm("instance");
    dxGridDetalles = $('#dxGridDetalles').dxTreeList("instance");
    dxGridAlmacenes = $('#dxGridAlmacenes').dxDataGrid("instance");

    rowEliminar = null;
    registrosEliminados = [];

    contadorRegistrosNuevos = 0;
}

var habilitaComponentes = function (enabled) {
    dxButtonDeshacer.option("disabled", !enabled);
    dxButtonGuardaCambios.option("disabled", !enabled);
}

var nuevoRegistro = function () {
    //Inicializamos el modelo del Forms
    dxFormModalModelo.option("formData", $.extend(true, {}, modeloVacio));

    //Inicializamos los campos del Form
    dxFormModalModelo.resetValues();

    //Limpiamos la selección del TreeList vacio
    dxGridDetalles.clearSelection();

    //Limpiamos la selección del TreeList vacio
    dxGridAlmacenes.clearSelection();
    
    //Asignamos un Id al Form
    dxFormModalModelo.updateData("ConfiguracionAreaId", contadorRegistrosNuevos);

    //Cambiamos el estatus del registro a "NUEVO"
    dxFormModalModelo.updateData("EstatusId", ESTATUS_NUEVO);

    //Decrementamos el contador de Registros para el siguiente nuevo registro
    contadorRegistrosNuevos -= 1;

    //Buscamos las Áreas ya seleccionadas
    deshabilitaAreas();

    //Marcamos el modal con estatus "NUEVO"
    modalModelo.attr("estatus", ESTATUS_NUEVO);

    //Mostramos el modal 
    modalModelo.modal('show');
}

var editaRegistro = function (event) {
    //Obtenemos una copia del objeto a modificar
    var rowEditar = $.extend(true, {}, event.row.data);

    //Cambiamos el estatus del registro a "EDITADO" solo si no es un registro "NUEVO"
    rowEditar.EstatusId = rowEditar.ConfiguracionAreaId <= 0 ? ESTATUS_NUEVO : ESTATUS_EDITADO;

    //Limpiamos la selección del TreeList vacio
    dxGridDetalles.clearSelection();

    if (!rowEditar.Detalles || rowEditar.Detalles.length == 0) {
        //Mostramos Loader
        dxLoaderPanel.show();

        //Enviamos la informacion al controlador
        $.ajax({
            type: "POST",
            url: API_FICHA + "getDatosProyectos",
            data: { configuracionAreaId: rowEditar.ConfiguracionAreaId },
            success: function (response) {
                //Ocultamos Loader
                dxLoaderPanel.hide();

                response.ListConfiguracionAreaProyectos.forEach(m => {
                    m.FechaCreacion = new Date(m.FechaCreacion.match(/\d+/).map(Number)[0]);
                    m.Timestamp = null;
                    m.EstatusId = ESTATUS_CARGADO;
                });

                //Asignamos los detalles al modelo
                rowEditar.Detalles = response.ListConfiguracionAreaProyectos;

                //Asignamos los detalles al TreeList
                dxGridDetalles.option("dataSource", response.ListDependenciasProyectos);

                seleccionarNodos(rowEditar);
            },
            error: function (response, status, error) {
                //Ocultamos Loader
                dxLoaderPanel.hide();

                //Mostramos mensaje de error
                toast("Error:\n" + response.responseText, 'error');
            }
        });
    } else {
        seleccionarNodos(rowEditar);
    }
}

var deshabilitaAreas = function () {
    //Obtenemos una copia del modelo a editar
    var rowEditar = dxFormModalModelo.option("formData");

    //Obtenemos todos los registros que hay en el dxGridListado
    var listado;    
    dxGridListado.getDataSource().store().load().done((res) => { listado = res; });

    //Obtenemos todos los registros que hay en el dxCboAreas
    var listadoAreas;
    cboAreas.getDataSource().store().load().done((res) => { listadoAreas = res; });

    //Actualizamos el DataSource
    var dataSource = new DevExpress.data.DataSource({
        store: {
            type: "array",
            key: "DependenciaId",
            data: listadoAreas
        },
        paginate: true,
        pageSize: 10
    });

    //Buscamos un registro que tenga la misma Área
    listadoAreas.forEach(m => {
        m.disabled = !!listado.find(x => x.ConfiguracionAreaId != rowEditar.ConfiguracionAreaId && x.AreaId == m.DependenciaId);

        dataSource.store().update(m.DependenciaId, m);
    });

    //Actualizamos el DataSource
    cboAreas.option("dataSource", dataSource);
}

var onTreeListSelectionChange = function (event) {
}

var cboAreasChange = function (event) {
    var areaAnteriorId = event.previousValue;
    var areaId = event.value;

    //Convertimos el id string a int
    areaAnteriorId = areaAnteriorId ? Number.parseInt(areaAnteriorId) : -1;
    areaId = areaId ? Number.parseInt(areaId) : -1;

    //Respaldamos los nodos seleccionados
    var seleccionados = dxGridDetalles.getSelectedRowsData();
    var temporales = [];

    //Limpiamos la selección del TreeList vacio
    dxGridDetalles.clearSelection();

    //Marcamos los nodos respaldados que el Área no se deseleccionó
    seleccionados.forEach(m => {
        if (m.DependenciaId != areaAnteriorId) {
            temporales.push(m.idPd);
        }
    });

    //Marcamos los nodos del Área seleccionada
    _listDependenciasProyectos.forEach(d => {
        if (d.PadreId != 0 && d.DependenciaId === areaId) {
            temporales.push(d.idPd);
        }
    });

    //Añadimos los nodos seleccionados
    dxGridDetalles.selectRows(temporales, true);
}

var seleccionarNodos = function (rowEditar) {
    //Marcamos los nodos seleccionados
    var seleccionados = [];

    rowEditar.Detalles.forEach(m => {
        if (!m.Borrado) {
            seleccionados.push(m.ProyectoDependenciaId);
        }
    });

    dxGridDetalles.selectRows(seleccionados, true);

    //Limpiamos la selección del TreeList vacio
    dxGridAlmacenes.clearSelection();

    if (!rowEditar.ListAlmacenes || rowEditar.ListAlmacenes.length == 0) {
        //Mostramos Loader
        dxLoaderPanel.show();

        //Enviamos la informacion al controlador
        $.ajax({
            type: "POST",
            url: API_FICHA + "getDatosAlmacenes",
            data: { configuracionAreaId: rowEditar.ConfiguracionAreaId },
            success: function (response) {
                //Ocultamos Loader
                dxLoaderPanel.hide();

                response.ListConfiguracionAreaAlmacenes.forEach(m => {
                    m.FechaCreacion = new Date(m.FechaCreacion.match(/\d+/).map(Number)[0]);
                    m.Timestamp = null;
                    m.EstatusId = ESTATUS_CARGADO;
                });

                //Asignamos los almacenes al modelo
                rowEditar.ListAlmacenes = response.ListConfiguracionAreaAlmacenes;

                //Asignamos los almacenes al TreeList
                dxGridAlmacenes.option("dataSource", response.ListAlmacenes);

                seleccionarAlmacenes(rowEditar);
            },
            error: function (response, status, error) {
                //Ocultamos Loader
                dxLoaderPanel.hide();

                //Mostramos mensaje de error
                toast("Error:\n" + response.responseText, 'error');
            }
        });
    } else {
        seleccionarAlmacenes(rowEditar);
    }
}

var seleccionarAlmacenes = function (rowEditar) {
    //Marcamos los nodos seleccionados
    var seleccionados = [];

    rowEditar.ListAlmacenes.forEach(m => {
        if (!m.Borrado) {
            seleccionados.push(m.AlmacenId);
        }
    });

    dxGridAlmacenes.selectRows(seleccionados, true);

    //Le pasamos el objeto al Form para que cargue sus valores
    dxFormModalModelo.option("formData", rowEditar);

    //Buscamos las Áreas ya seleccionadas
    deshabilitaAreas();

    //Marcamos el modal con estatus "EDITADO"
    modalModelo.attr("estatus", ESTATUS_EDITADO);

    //Mostramos el modal
    modalModelo.modal('show');
}

var existeArea = function (configuracionAreaId, areaId) {
    var listado;

    //Obtenemos todos los registros que hay en el dxGridListado
    dxGridListado.getDataSource().store().load().done((res) => { listado = res; });

    //Buscamos un registro que tenga la misma Área
    var encontrado = listado.find(x => x.ConfiguracionAreaId !== configuracionAreaId && x.AreaId === areaId);

    //Retornamos <true> si se encontro un registro , <false> de lo contrario
    return encontrado;
}

var guardaCambiosModal = function () {
    var rowEditar = dxFormModalModelo.option("formData");

    //Validamos que la informacion requerida del Formulario este completa
    if (!dxFormModalModelo.validate().isValid) {
        toast("Favor de completar los campos requeridos.", 'error');

        return;
    }    

    //Validamos que el Área no se repita
    if (existeArea(rowEditar.ConfiguracionAreaId, rowEditar.AreaId)) {
        toast("Ya existe un registro con la misma Área. Favor de verificar.", 'warning')

        return;
    }

    rowEditar.Codigo = rowEditar.AreaId;
    rowEditar.Area = _listAreas.find(x => x.DependenciaId == cboAreas.option("value")).Nombre;

    //Guardamos los Proyectos
    var seleccionados = dxGridDetalles.getSelectedRowsData();
    var temporales = [];
    var detalles = rowEditar.Detalles;

    //Agregamos los detalles hijo
    seleccionados.forEach(m => {
        if (m.PadreId === 0) {
            _listDependenciasProyectos.forEach(d => {
                if (d.PadreId != 0 && d.PadreId === m.idPd) {
                    temporales.push(d);
                }
            });
        }
    });

    seleccionados = seleccionados.concat(temporales);

    detalles.forEach(m => {
        m.Borrado = !seleccionados.find(x => x.PadreId != 0 && x.idPd == m.ProyectoDependenciaId);
    });

    var areas = [];
    var proyectos = [];

    seleccionados.forEach(m => {
        if (m.PadreId != 0) {
            if (!areas.find(x => x == m.DependenciaId)) {
                areas.push(m.DependenciaId);
            }

            if (!proyectos.find(x => x == m.ProyectoId)) {
                proyectos.push(m.ProyectoId);
            }

            var encontrado = detalles.find(x => x.ProyectoDependenciaId == m.idPd);

            if (!encontrado) {
                var nuevo = {
                    ConfiguracionAreaProyectoId: 0,
                    ConfiguracionAreaId: rowEditar.ConfiguracionAreaId,
                    ProyectoDependenciaId: m.idPd,
                    Borrado: false,
                }

                detalles.push(nuevo);
            } else {
                encontrado.Borrado = false;
            }
        }        
    });

    //Guardamos los Almacenes
    seleccionados = dxGridAlmacenes.getSelectedRowsData();
    var almacenes = rowEditar.ListAlmacenes;

    almacenes.forEach(m => {
        m.Borrado = !seleccionados.find(x => x.AlmacenId == m.AlmacenId);
    });

    var almacenesCount = [];

    seleccionados.forEach(m => {
        if (!almacenesCount.find(x => x == m.AlmacenId)) {
            almacenesCount.push(m.AlmacenId);
        }

        var encontrado = almacenes.find(x => x.AlmacenId == m.AlmacenId);

        if (!encontrado) {
            var nuevo = {
                ConfiguracionAreaAlmacenId: 0,
                ConfiguracionAreaId: rowEditar.ConfiguracionAreaId,
                AlmacenId: m.AlmacenId,
                Borrado: false,
            }

            almacenes.push(nuevo);
        } else {
            encontrado.Borrado = false;
        }
    });

    //Validamos que se haya seleccionado un Proyecto
    if (!rowEditar.Detalles || rowEditar.Detalles.length == 0 || !rowEditar.Detalles.find(x => x.Borrado === false)) {
        toast("Favor de seleccionar al menos un Proyecto.", 'error');

        return;
    }

    //Validamos que se haya seleccionado un Almacén
    if (!rowEditar.ListAlmacenes || rowEditar.ListAlmacenes.length == 0 || !rowEditar.ListAlmacenes.find(x => x.Borrado === false)) {
        toast("Favor de seleccionar al menos un Almacén.", 'error');

        return;
    }

    rowEditar.UnidadesAdministrativas = areas.length;
    rowEditar.Proyectos = proyectos.length;
    rowEditar.Almacenes = almacenesCount.length;

    //Obtenemos la instancia store del DataSource
    var store = dxGridListado.getDataSource().store();

    //Si es un registro nuevo, lo insertamos en el DataSource
    if (modalModelo.attr("estatus") == ESTATUS_NUEVO) {
        store.insert(rowEditar)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxGridListado.getDataSource().reload();

                //Habilitamos los botones de acciones
                habilitaComponentes(true);

                //Ocultamos el modal
                modalModelo.modal('hide');
            })
            .fail(function () {
                toast("No se pudo agregar el nuevo registro a la tabla.", "error");
            });
    }
    //Si es un registro que se esta editando, actualizamos su informacion en el DataSource
    else {
        store.update(rowEditar.ConfiguracionAreaId, rowEditar)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxGridListado.getDataSource().reload();

                //Habilitamos los botones de acciones
                habilitaComponentes(true);

                //Ocultamos el modal
                modalModelo.modal('hide');
            })
            .fail(function () {
                toast("No se pudo actualizar el registro en la tabla.", "error");
            });
    }
}

var validaEliminar = function (event) {
    //Obtenemos una copia del objeto a eliminar
    rowEliminar = $.extend(true, {}, event.row.data);

    //Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}

var eliminaRegistro = function () {
    //Obtenemos la instancia store del DataSource
    var store = dxGridListado.getDataSource().store();

    //Eliminamos el registro de la tabla
    if (rowEliminar != null) {
        store.remove(rowEliminar.ConfiguracionAreaId)
            .done(function () {
                //Si el registro viene de la base de datos, respaldamos el registro 
                //para posteriormente eliminarlo en la base de datos
                if (rowEliminar.ConfiguracionAreaId > 0) {
                    //Actualizamos el estatus del registro a "Eliminado"
                    rowEliminar.EstatusId = ESTATUS_ELIMINADO;
                    rowEliminar.Borrado = true;

                    //Respaldamos el registro que se acaba de eliminar
                    registrosEliminados.push(rowEliminar);
                }

                //Recargamos la informacion de la tabla
                dxGridListado.getDataSource().reload();

                //Habilitamos los botones de acciones
                habilitaComponentes(true);
            })
            .fail(function () {
                toast("No se pudo eliminar el registro de la tabla.", "error");
            });

        rowEliminar = null;
    }
}

var guardaCambios = function () {
    var detalles;

    //Obtenemos todos los registros que hay en el dxGridListado
    dxGridListado.getDataSource().store().load().done((res) => { detalles = res; });

    //Agregamos los registros borrados, para eliminarlos en base de datos
    detalles = $.merge(detalles, registrosEliminados);

    //Mostramos Loader
    dxLoaderPanel.show();

    //Enviamos la informacion al controlador
    $.ajax({
        type: "POST",
        url: API_FICHA + "guardar",
        data: { configuracionAreas: detalles },
        success: function () {
            //Mostramos mensaje de Exito
            toast("Registros guardados con exito!", 'success');

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

var recargarFicha = function () {
    //Recargamos la ficha
    window.location.href = API_FICHA;
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}