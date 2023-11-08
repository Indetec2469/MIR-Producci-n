//Botones
var dxButtonGuardaCambios;

//Forms
var dxForm;
var dxGridDetalles;

//Variables Globales
var cambios;

var API_FICHA = "/almacenes/reportes/reporterecibooc/";

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();
});

var inicializaVariables = function () {
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");

    dxForm = $("#dxForm").dxForm("instance");
    dxGridDetalles = $("#dxGridDetalles").dxDataGrid("instance");

    //Asignamos el source a la tabla
    dxGridDetalles.option("dataSource", []);

    cambios = false;
}

var getForm = function () {
    var modelo = $.extend(true, {}, dxForm.option('formData'));

    var fechaInicio = modelo.FechaInicio;
    var fechaFin = modelo.FechaFin;

    if (fechaInicio) {
        fechaInicio.setHours(0);
        fechaInicio.setMinutes(0);
        fechaInicio.setSeconds(0);
        fechaInicio.setMilliseconds(0);

        modelo.FechaInicio = fechaInicio.toLocaleString();
    }

    if (fechaFin) {
        fechaFin.setHours(23);
        fechaFin.setMinutes(59);
        fechaFin.setSeconds(59);
        fechaFin.setMilliseconds(0);

        modelo.FechaFin = fechaFin.toLocaleString();
    }

    return modelo;
}

var setCambios = function () {
    cambios = true;
}

var guardaCambios = function () {
    //Limpiamos y recargamos la informacion de la tabla
    var dataSource = dxGridDetalles.getDataSource();
    dataSource.store().clear();
    dataSource.reload();

    //Validamos que la informacion requerida del Formulario este completa
    if (!dxForm.validate().isValid) {
        toast("Favor de completar los campos requeridos.", 'error');

        return;
    }

    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "buscarReporte",
        data: { reporte: getForm() },
        success: function (response) {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            //Asignamos el source a la tabla
            dxGridDetalles.option("dataSource", response);
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al descargar:\n" + response.responseText, 'error');
        }
    });
}

var verReporte = function (event) {
    //Abrimos el reporte
    window.open("/almacenes/almacenes/ordencomprarecibo/rptSurtimientoOC/" + event.row.data.CompraId);
}

var recargarFicha = function () {
    // Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA;
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}