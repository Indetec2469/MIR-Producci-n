//Componentes
var dxCboTipoFecha;
var dxTxtFechaOperacion;

//Modales
var modalConfirmaDeshacer;

//Botones
var dxButtonDeshacer;
var dxButtonGuardaCambios;

//Forms
var dxForm;

//Variables de Control
var ignoraEventos;
var cambios;

//Variables Estaticas
var TIPO_FECHA_OPERACION = ControlMaestroMapeo.GRTipoConfiguracionFecha.FECHA_OPERACION;
var TIPO_FECHA_SISTEMA = ControlMaestroMapeo.GRTipoConfiguracionFecha.FECHA_SISTEMA;

var API_FICHA = "/sistemas/sistema/configuracion/";

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();

    //Respaldamos el modelo del Form
    getForm();

    //Deshabilitamos los botones de acciones
    habilitaComponentes();
});

var inicializaVariables = function () {
    dxCboTipoFecha = $('#dxCboTipoFecha').dxSelectBox("instance");
    dxTxtFechaOperacion = $('#dxTxtFechaOperacion').dxDateBox("instance");

    modalConfirmaDeshacer = $('#modalConfirmaDeshacer');

    dxButtonDeshacer = $('#dxButtonDeshacer').dxButton("instance");
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");

    dxForm = $("#dxForm").dxForm("instance");

    ignoraEventos = false;
    cambios = false;

    setMinMaxFechaOperacion();
}

var cboTipoFecha = function (event) {
    //Asignamos el límite en los campos de Fecha
    setMinMaxFechaOperacion();

    //Limpiamos la Fecha de Operación
    dxTxtFechaOperacion.option("value", null);

    //Marcamos los cambios
    setCambios();
}

var setMinMaxFechaOperacion = function () {
    var tipoFecha = getForm().TipoConfiguracionFechaId;

    //Asignamos el límite en los campos de Fecha
    var minDate = tipoFecha == TIPO_FECHA_OPERACION ? _ejercicio + "-01-01" : null;
    var maxDate = tipoFecha == TIPO_FECHA_OPERACION ? new Date().toISOString().split('T')[0] : null;

    dxTxtFechaOperacion.option("min", minDate);
    dxTxtFechaOperacion.option("max", maxDate);
}

var getForm = function () {
    var modelo = $.extend(true, {}, dxForm.option('formData'));

    modelo.ConfiguracionEnteId = modelo.ConfiguracionEnteId || 0;

    var fechaOperacion = modelo.FechaOperacion;

    if (fechaOperacion) {
        fechaOperacion.setHours(0);
        fechaOperacion.setMinutes(0);
        fechaOperacion.setSeconds(0);
        fechaOperacion.setMilliseconds(0);

        modelo.FechaOperacion = fechaOperacion.toLocaleString();
    }

    return modelo;
}

var habilitaComponentes = function () {
    dxButtonDeshacer.option("disabled", !cambios);
    dxButtonGuardaCambios.option("disabled", !cambios);
    dxTxtFechaOperacion.option("disabled", getForm().TipoConfiguracionFechaId == TIPO_FECHA_SISTEMA);
}

var setCambios = function () {
    cambios = true;
    habilitaComponentes();
}

var validaDeshacer = function () {
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

var guardaCambios = function (enviarAutorizar) {
    //Obtenemos el modelo
    var modelo = getForm();

    //Validamos que la informacion requerida del Formulario este completa
    if (modelo.TipoConfiguracionFechaId == TIPO_FECHA_OPERACION && !modelo.FechaOperacion) {
        toast("Favor agregar la Fecha de Operación.", 'error');

        return;
    }

    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "guardar",
        data: { configuracion: modelo },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Registro guardado con exito!", 'success');

            //Recargamos la ficha
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
    // Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA;
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}