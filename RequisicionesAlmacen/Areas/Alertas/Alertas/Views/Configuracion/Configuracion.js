//Componentes
var dxTreeList;
var dxButtonAgregar;
var dxRadioBtnTipoNotificacion;
var dxCboEmpleado;

//Modales
var modalConfirmaDeshacer;
var modalConfirmaEliminarRegistro;

//Botones
var dxButtonDeshacer;
var dxButtonGuardaCambios;

//Forms
var dxForm;
var dxGridDetalles;

//Variables Globales
var modeloVacio;
var rowEliminar;
var registrosEliminados;

//Variables de Control
var contadorRegistrosNuevos;
var ignoraEventos;
var cambios;
var eventoRegresar;

//Variables Estaticas
var ESTATUS_NUEVO = ControlMaestroMapeo.EstatusRegistro.ACTIVO;
var ESTATUS_EDITADO = 2;
var ESTATUS_ELIMINADO = ControlMaestroMapeo.EstatusRegistro.BORRADO;

var TIPO_AUTORIZACION = ControlMaestroMapeo.TipoNotificacionAlerta.AUTORIZACION;
var TIPO_NOTIFICACION = ControlMaestroMapeo.TipoNotificacionAlerta.NOTIFICACION;

var API_FICHA = "/alertas/alertas/configuracion/";

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();

    //Deshabilitamos los botones de acciones
    habilitaComponentes();
});

var inicializaVariables = function () {
    dxTreeList = $('#dxTreeList').dxTreeList("instance");
    dxButtonAgregar = $('#dxButtonAgregar').dxButton("instance");
    dxRadioBtnTipoNotificacion = $('#dxRadioBtnTipoNotificacion').dxRadioGroup("instance");
    dxCboEmpleado = $('#dxCboEmpleado').dxSelectBox("instance");

    dxButtonAgregar.option("disabled", true);
    dxRadioBtnTipoNotificacion.option("disabled", true);
    dxCboEmpleado.option("disabled", true);

    modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    modalConfirmaEliminarRegistro = $('#modalConfirmaEliminarRegistro');

    dxButtonDeshacer = $('#dxButtonDeshacer').dxButton("instance");
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");

    dxForm = $("#dxForm").dxForm("instance");
    dxGridDetalles = $("#dxGridDetalles").dxDataGrid("instance");

    rowEliminar = null;
    registrosEliminados = [];

    contadorRegistrosNuevos = -1;
    ignoraEventos = false;
    cambios = false;
    eventoRegresar = true;
}

var habilitaComponentes = function () {
    dxTreeList.option("disabled", cambios);
    dxButtonDeshacer.option("disabled", !cambios);
}

var setCambios = function () {
    cambios = true;
    habilitaComponentes();
    inicializaForm();
}

var inicializaForm = function () {
    //Inicializamos el modelo del Forms
    dxForm.option("formData", $.extend(true, {}, modeloVacio));

    //Inicializamos los campos del Form
    dxForm.resetValues();

    //Deseleccionamos el registro
    dxGridDetalles.option("selectedRowKeys", []);

    //Cambiamos el texto del botón agregar
    dxButtonAgregar.option("text", "Agregar");
}

var validaEliminarRegistro = function (event) {
    //Obtenemos una copia del objeto a eliminar
    rowEliminar = $.extend(true, {}, event.row.data);

    //Mostramos el modal de confirmacion
    modalConfirmaEliminarRegistro.modal('show');
}

var eliminaRegistro = function () {
    //Obtenemos la instancia store del DataSource
    var store = dxGridDetalles.getDataSource().store();

    //Eliminamos el registro de la tabla
    if (rowEliminar != null) {
        store.remove(rowEliminar.AlertaConfiguracionId)
            .done(function () {
                //Si el registro viene de la base de datos, respaldamos el registro 
                //para posteriormente eliminarlo en la base de datos
                if (rowEliminar.AlertaConfiguracionId > 0) {
                    //Actualizamos el estatus del registro a "Eliminado"
                    rowEliminar.EstatusId = ESTATUS_ELIMINADO; //Borrar registro

                    //Respaldamos el registro que se acaba de eliminar
                    registrosEliminados.push(rowEliminar);
                }

                //Recargamos la informacion de la tabla
                dxGridDetalles.getDataSource().reload();

                //Marcamos los cambios
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
        modalConfirmaDeshacer.modal('show');
    } else {
        cancelar();
    }
}

var cancelar = function () {
    //Recargamos la ficha
    recargarFicha();
}

var existeRegistro = function (alertaConfiguracionId,
    alertaEtapaAccionId,
    empleadoId,
    figuraId,
    tipoNotificacionId) {

    //Obtenemos todos los registros que hay en el dxGrid
    var detalles;
    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

    //Buscamos un registro que tenga el mismo registro
    var encontrado = detalles.find(x => x.AlertaConfiguracionId !== alertaConfiguracionId
        && x.AlertaEtapaAccionId === alertaEtapaAccionId
        && x.EmpleadoId === empleadoId
        && x.FiguraId === figuraId
        && x.TipoNotificacionId === tipoNotificacionId);

    //Retornamos <true> si se encontro un registro , <false> de lo contrario
    return encontrado;
}

var limpiarTabla = function () {
    //Limpiamos y recargamos la informacion de la tabla
    var dataSource = dxGridDetalles.getDataSource();
    dataSource.store().clear();
    dataSource.reload();

    //Deseleccionamos el registro
    dxGridDetalles.option("selectedRowKeys", []);
}

var onSelectionChangedTree = function (event) {
    //Inicializamos la tabla
    limpiarTabla();

    var nodoEtapa = event.selectedRowsData[0];

    dxRadioBtnTipoNotificacion.option("value", null);
    dxCboEmpleado.option("value", null);

    dxButtonAgregar.option("disabled", nodoEtapa.Nivel != 5);
    dxRadioBtnTipoNotificacion.option("disabled", true);
    dxCboEmpleado.option("disabled", nodoEtapa.Nivel != 5);

    dxForm.updateData("AlertaEtapaAccionId", nodoEtapa.AlertaEtapaAccionId);

    if (nodoEtapa.Nivel == 5) {
        if (nodoEtapa.PermiteAutorizacion && nodoEtapa.PermiteNotificacion) {
            dxRadioBtnTipoNotificacion.option("disabled", false);
        } else if (nodoEtapa.PermiteAutorizacion) {
            dxRadioBtnTipoNotificacion.option("value", TIPO_AUTORIZACION);
        } else if (nodoEtapa.PermiteNotificacion) {
            dxRadioBtnTipoNotificacion.option("value", TIPO_NOTIFICACION);
        }

        var detalles = [];

        _listAlertaConfiguracion.forEach(modelo => {
            if (modelo.AlertaEtapaAccionId === nodoEtapa.AlertaEtapaAccionId) {
                var nuevoRegistro = {
                    AlertaConfiguracionId: modelo.AlertaConfiguracionId,
                    AlertaEtapaAccionId: modelo.AlertaEtapaAccionId,
                    EmpleadoId: modelo.EmpleadoId,
                    FiguraId: modelo.FiguraId,
                    TipoNotificacionId: modelo.TipoNotificacionId,
                    EnPlataforma: modelo.EnPlataforma,
                    EnCorreoElectronico: modelo.EnCorreoElectronico,
                    Sistema: modelo.Sistema,
                    EstatusId: modelo.EstatusId,
                    Timestamp: btoa(String.fromCharCode.apply(null, new Uint8Array(modelo.Timestamp)))
                }

                detalles.push(nuevoRegistro);
            }
        });

        //Actualizamos el DataSource
        var dataSource = new DevExpress.data.DataSource({
            store: {
                type: "array",
                key: "AlertaConfiguracionId",
                data: detalles
            },
            paginate: true,
            pageSize: 10
        });

        //Asignamos el source a la tabla
        dxGridDetalles.option("dataSource", dataSource);
    }

    //Respaldamos el modelo vacio del Form
    modeloVacio = $.extend(true, {}, dxForm.option('formData'));
}

var onSelectionChangedDetalle = function (event) {
    var data = event.selectedRowsData;

    if (data.length > 0) {
        var modelo = data[0];

        if (!modelo.Sistema) {
            modelo.EmpleadoId = modelo.EmpleadoId || modelo.FiguraId;
            modelo.FiguraId = null;

            //Inicializamos el modelo del Forms
            dxForm.option("formData", $.extend(true, {}, modelo));

            //Cambiamos el texto del botón agregar
            dxButtonAgregar.option("text", "Guardar");
        } else {
            //Restablecemos el form
            inicializaForm();
        }
    }
}

var agregarRegistro = function () {
    //Obtenemos el Objeto que se esta creando/editando en el Form 
    var modelo = dxForm.option("formData");

    //Validamos que la informacion requerida del Formulario este completa
    if (!dxForm.validate().isValid) {
        toast("Favor de seleccionar un Tipo de Notificación.", 'error');

        return;
    }

    if (!modelo.EmpleadoId) {
        toast("Favor de seleccionar un Empleado para notificar.", 'error');

        return;
    }

    var figuraEmpleado = _listEmpleado.find(x => x.Id == modelo.EmpleadoId);

    var nuevoRegistro = {
        AlertaConfiguracionId: modelo.AlertaConfiguracionId || contadorRegistrosNuevos,
        AlertaEtapaAccionId: modelo.AlertaEtapaAccionId,
        EmpleadoId: !figuraEmpleado.Figura ? figuraEmpleado.Id : null,
        FiguraId: figuraEmpleado.Figura ? figuraEmpleado.Id : null,
        TipoNotificacionId: modelo.TipoNotificacionId,
        EnPlataforma: true,
        EnCorreoElectronico: false,
        Sistema: false,
        EstatusId: modelo.AlertaConfiguracionId ? ESTATUS_EDITADO : ESTATUS_NUEVO
    }

    //Validamos que no se repita
    if (existeRegistro(nuevoRegistro.AlertaConfiguracionId,
        nuevoRegistro.AlertaEtapaAccionId,
        nuevoRegistro.EmpleadoId,
        nuevoRegistro.FiguraId,
        nuevoRegistro.TipoNotificacionId)) {

        toast("Ya existe un registro con la misma configuración. Favor de verificar.", 'warning')

        return;
    }

    var detalles;
    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

    //Obtenemos la instancia store del DataSource
    var store = dxGridDetalles.getDataSource().store();

    //Si es un registro nuevo, lo insertamos en el DataSource
    if (nuevoRegistro.EstatusId == ESTATUS_NUEVO) {
        store.insert(nuevoRegistro)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxGridDetalles.getDataSource().reload();

                //Marcamos los cambios
                setCambios();

                //Decrementamos el contador de Registros para el siguiente nuevo registro
                contadorRegistrosNuevos -= 1;
            })
            .fail(function () {
                toast("No se pudo agregar el nuevo registro a la tabla.", "error");
            });
    }
    //Si es un registro que se esta editando, actualizamos su informacion en el DataSource
    else {
        store.update(nuevoRegistro.AlertaConfiguracionId, nuevoRegistro)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxGridDetalles.getDataSource().reload();

                //Marcamos los cambios
                setCambios();
            })
            .fail(function () {
                toast("No se pudo actualizar el registro en la tabla.", "error");
            });
    }
}

var guardaCambios = function () {
    if (!cambios) {
        toast("No existen cambios pendientes por guardar.", "warning");

        return;
    }

    //Obtenemos todos los registros que hay en el dxGridDetalles
    var detalles;
    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

    //Agregamos los registros borrados, para eliminarlos en base de datos
    detalles = $.merge(detalles, registrosEliminados);

    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "guardaCambios",
        data: { detalles: detalles },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Registros guardados con exito!", 'success');

            //Regresamos al listado
            recargarFicha();
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al guadar:\n" + response.responseText, 'error');
        }
    });
}

var displayExpr_empleado = function (item) {
    var empleadoFigura = _listEmpleado.find(x => x.Id == (item.EmpleadoId || item.FiguraId));

    return empleadoFigura ? empleadoFigura.Nombre : null;
}

var displayExpr_figura = function (item) {
    return item.FiguraId ? true : false;
}

var displayExpr_tipo = function (item) {
    var tipoNotificacion = _listTipoNotificacion.find(x => x.ControlId == item.TipoNotificacionId);

    return tipoNotificacion ? tipoNotificacion.Valor : null;
}

var recargarFicha = function () {
    // Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA;
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}