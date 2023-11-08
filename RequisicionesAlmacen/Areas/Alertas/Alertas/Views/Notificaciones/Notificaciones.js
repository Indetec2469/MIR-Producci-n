//Componentes
var dxDrawer;
var dxListDrawer;
var tabAutorizaciones;
var tabNotificaciones;
var dxButtonOcultar;

//Modales
var modalConfirmaAutorizar;
var modalConfirmaRevision;
var modalConfirmaRechazar;

//Forms
var dxGridAutorizaciones;
var dxGridNotificaciones;
var dxFormModalMotivoRevision;
var dxFormModalMotivoRechazo;

//Variables de Control
var seleccionListIndex;
var seleccionadosOcultar;
var alertaModificar;
var modeloVacio;

//Variables Estaticas
var TIPO_AUTORIZACION = ControlMaestroMapeo.TipoNotificacionAlerta.AUTORIZACION;
var TIPO_NOTIFICACION = ControlMaestroMapeo.TipoNotificacionAlerta.NOTIFICACION;

var API_FICHA = "/alertas/alertas/notificaciones/";

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();

    //Deshabilitamos los botones de acciones
    habilitaComponentes();
});

var inicializaVariables = function () {
    dxDrawer = $('#dxDrawer').dxDrawer('instance');
    dxListDrawer = $('#dxListDrawer').dxList('instance');
    tabAutorizaciones = $("#tabAutorizaciones");
    tabNotificaciones = $("#tabNotificaciones");
    dxButtonOcultar = $('#dxButtonOcultar').dxButton('instance');

    modalConfirmaAutorizar = $('#modalConfirmaAutorizar');
    modalConfirmaRevision = $('#modalConfirmaRevision');
    modalConfirmaRechazar = $('#modalConfirmaRechazar');

    dxGridAutorizaciones = $("#dxGridAutorizaciones").dxDataGrid("instance");
    dxGridNotificaciones = $("#dxGridNotificaciones").dxDataGrid("instance");
    dxFormModalMotivoRevision = $("#dxFormModalMotivoRevision").dxForm("instance");
    dxFormModalMotivoRechazo = $("#dxFormModalMotivoRechazo").dxForm("instance");

    //Respaldamos el modelo vacio del Form
    modeloVacio = $.extend(true, {}, dxFormModalMotivoRevision.option('formData'));

    //Eliminamos una clase de style en template content
    $('#templateContent').removeClass('d-none');

    seleccionadosOcultar = [];
    alertaModificar = null;

    seleccionListIndex = 1;
    siguientePestania();
}

var habilitaComponentes = function () {
    dxButtonOcultar.option("disabled", true);
}

var onClickToolbar = function () {
    dxDrawer.toggle();
}

var onItemClickDrawer = function (event) {
    seleccionListIndex = event.itemData.index;

    siguientePestania();
}

var siguientePestania = function () {
    //Ocultamos las pestañas
    tabAutorizaciones.hide();
    tabNotificaciones.hide();

    switch (seleccionListIndex) {
        //Ordenes de Compra
        case 1:
            tabAutorizaciones.show();
            dxListDrawer.option('selectedItemKeys', [1]);
            break;

        //Invitaciones de Compra
        case 2:
            tabNotificaciones.show();
            dxListDrawer.option('selectedItemKeys', [2]);
            break;

        default:
            tabAutorizaciones.show();
            dxListDrawer.option('selectedItemKeys', [1]);
            break;
    }
}

var filtrarAutorizaciones = function (event) {
    return event.TipoAlertaId === TIPO_AUTORIZACION;
}

var filtrarNotificaciones = function (event) {
    return event.TipoAlertaId === TIPO_NOTIFICACION;
}

var verAlerta = function (event) {
    //Dirigimos a la ruta de acción de la alerta
    window.location.href = event.row.data.RutaAccion;
}

var onSelectionChange = function (event) {
    seleccionadosOcultar = event.selectedRowKeys;

    dxButtonOcultar.option("disabled", !seleccionadosOcultar.length);
}

var validaAutorizar = function (event) {
    //Obtenemos una copia del objeto a eliminar
    alertaModificar = $.extend(true, {}, event.row.data);

    //Mostramos el modal de confirmacion
    modalConfirmaAutorizar.modal('show');
}

var autorizar = function (event) {
    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "autorizarAlerta",
        data: { alertaId: alertaModificar.AlertaId },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Trámite Autorizado.", 'success');

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

var validaRevision = function (event) {
    //Obtenemos una copia del objeto a eliminar
    alertaModificar = $.extend(true, {}, event.row.data);

    //Inicializamos el modelo del Forms
    dxFormModalMotivoRevision.option("formData", $.extend(true, {}, modeloVacio));

    //Inicializamos los campos del Form
    dxFormModalMotivoRevision.resetValues();

    //Mostramos el modal de confirmacion
    modalConfirmaRevision.modal('show');
}

var enviarRevision = function (event) {
    //Obtenemos el Objeto que se esta creando/editando en el Form 
    var modelo = dxFormModalMotivoRevision.option("formData");

    if (!modelo.MotivoRechazo) {
        //Mostramos mensaje de error
        toast("Favor de agregar un motivo.", 'warning');

        return;
    }

    //Ocultamos el modal
    modalConfirmaRevision.modal('hide');

    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "revisionAlerta",
        data: { alertaId: alertaModificar.AlertaId, motivo: modelo.MotivoRechazo },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Trámite en Revisión.", 'success');

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

var validaRechazar = function (event) {
    //Obtenemos una copia del objeto a eliminar
    alertaModificar = $.extend(true, {}, event.row.data);

    //Inicializamos el modelo del Forms
    dxFormModalMotivoRechazo.option("formData", $.extend(true, {}, modeloVacio));

    //Inicializamos los campos del Form
    dxFormModalMotivoRechazo.resetValues();

    //Mostramos el modal de confirmacion
    modalConfirmaRechazar.modal('show');
}

var rechazar = function (event) {
    //Obtenemos el Objeto que se esta creando/editando en el Form 
    var modelo = dxFormModalMotivoRechazo.option("formData");

    if (!modelo.MotivoRechazo) {
        //Mostramos mensaje de error
        toast("Favor de agregar un motivo.", 'warning');

        return;
    }

    //Ocultamos el modal
    modalConfirmaRechazar.modal('hide');

    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "rechazarAlerta",
        data: { alertaId: alertaModificar.AlertaId, motivo: modelo.MotivoRechazo },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Trámite Rechazado.", 'success');

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

var ocultar = function (event) {
    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "ocultarAlertas",
        data: { alertasId: seleccionadosOcultar.toString() },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Notificaciones Ocultas.", 'success');

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

var recargarFicha = function () {
    //Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA;
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}