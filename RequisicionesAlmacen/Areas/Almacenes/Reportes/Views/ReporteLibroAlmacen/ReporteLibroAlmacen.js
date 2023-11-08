//Variables Globales
var objetosGasto = [];
var fecha;

var API_FICHA = "/almacenes/reportes/reportelibroalmacen/";

$(document).ready(function () {
    
});

var cboObjetosGastoChange = function (e) {
    objetosGasto = e.value;
}

var setFecha = function (e) {
    fecha = e.value;
}

var guardaCambios = function () {
    // Carga el reporte con limpiar
    $("#reporte").empty();

    //Validamos que la informacion requerida del Formulario este completa
    if (objetosGasto.length == 0) {
        toast("Favor de completar los campos requeridos.", 'error');

        return;
    }

    // Mostramos Loader
    dxLoaderPanel.show();

    //Hacemos la petición para eliminar el registro
    $.ajax({
        type: "POST",
        url: API_FICHA + "buscaReporte",
        data: { objetosGastoId: objetosGasto, fecha: (fecha || $('#dxTxtFecha').dxDateBox("instance").option("value")).toISOString().split('T')[0] },
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