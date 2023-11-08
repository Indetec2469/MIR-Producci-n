//Botones
var dxButtonGuardaCambios;

//Forms
var dxForm;

//Variables Globales
var cambios;

var API_FICHA = "/almacenes/reportes/reportekardex/";

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();
});

var inicializaVariables = function () {
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");

    dxForm = $("#dxForm").dxForm("instance");

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
        fechaFin.setHours(0);
        fechaFin.setMinutes(0);
        fechaFin.setSeconds(0);
        fechaFin.setMilliseconds(0);

        modelo.FechaFin = fechaFin.toLocaleString();
    }

    return modelo;
}

var setCambios = function () {
    cambios = true;
}

var guardaCambios = function () {
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

            //Descargamos el reporte en Excel
            window.open(API_FICHA + 'descargarExcel/');

            //Mostramos mensaje de Exito
            toast("Reporte descargado con exito!", 'success');            
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al descargar:\n" + response.responseText, 'error');
        }
    });
}

var recargarFicha = function () {
    // Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA;
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}