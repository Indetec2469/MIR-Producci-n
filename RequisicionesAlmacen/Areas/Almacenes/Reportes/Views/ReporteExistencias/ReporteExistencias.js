//Combos
var dxCboTipoReporte;
var dxCboTipoGasto;
var dxCboUnidadAdministrativa;
var dxCboProyecto;
var dxCboFuenteFinanciamiento;
var dxCboObjetoGasto;

//Botones
var dxButtonGuardaCambios;

//Forms
var dxForm;

//Variables Globales
var cambios;

var API_FICHA = "/almacenes/reportes/reporteexistencias/";

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();

    //Habiliatamos los componentes
    habilitaComponentes();
});

var inicializaVariables = function () {
    dxCboTipoReporte = $('#dxCboTipoReporte').dxSelectBox("instance");
    dxCboUnidadAdministrativa = $('#dxCboUnidadAdministrativa').dxSelectBox("instance");
    dxCboProyecto = $('#dxCboProyecto').dxSelectBox("instance");
    dxCboFuenteFinanciamiento = $('#dxCboFuenteFinanciamiento').dxSelectBox("instance");
    dxCboTipoGasto = $('#dxCboTipoGasto').dxSelectBox("instance");
    dxCboObjetoGasto = $('#dxCboObjetoGasto').dxSelectBox("instance");

    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");

    dxForm = $("#dxForm").dxForm("instance");

    cambios = false;
}

var habilitaComponentes = function () {
    dxCboUnidadAdministrativa.option("disabled", true);
    dxCboProyecto.option("disabled", true);
    dxCboFuenteFinanciamiento.option("disabled", true);
    dxCboTipoGasto.option("disabled", true);
    dxCboObjetoGasto.option("disabled", true);

    if (getForm().Id == 0) {
        dxCboUnidadAdministrativa.option("value", null);
        dxCboProyecto.option("value", null);
        dxCboFuenteFinanciamiento.option("value", null);
        dxCboTipoGasto.option("value", null);
        dxCboObjetoGasto.option("value", null);
    } else if (getForm().Id == 1) {
        dxCboObjetoGasto.option("value", null);

        dxCboUnidadAdministrativa.option("disabled", false);
        dxCboProyecto.option("disabled", false);
        dxCboFuenteFinanciamiento.option("disabled", false);
        dxCboTipoGasto.option("disabled", false);
    } else if (getForm().Id == 2) {
        dxCboUnidadAdministrativa.option("value", null);
        dxCboProyecto.option("value", null);
        dxCboFuenteFinanciamiento.option("value", null);
        dxCboTipoGasto.option("value", null);

        dxCboObjetoGasto.option("disabled", false);
    }
}

var getForm = function () {
    var modelo = $.extend(true, {}, dxForm.option('formData'));

    return modelo;
}

var setCambios = function () {
    cambios = true;

    habilitaComponentes();

    // Carga el reporte con limpiar
    $("#reporte").empty();
}

var guardaCambios = function () {
    // Carga el reporte con limpiar
    $("#reporte").empty();

    //Validamos que la informacion requerida del Formulario este completa
    if (!dxForm.validate().isValid) {
        toast("Favor de completar los campos requeridos.", 'error');

        return;
    }

    // Mostramos Loader
    dxLoaderPanel.show();

    //Hacemos la petición para eliminar el registro
    $.ajax({
        type: "POST",
        url: API_FICHA + "buscaReporte",
        data: { reporte: getForm() },
        success: function (response) {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            // Carga el reporte
            $("#reporte").html(response);
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al cargar el reporte:\n" + response.responseText, 'error');
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