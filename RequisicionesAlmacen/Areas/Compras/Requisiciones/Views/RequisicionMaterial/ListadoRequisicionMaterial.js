//Modales
var modalConfirmaEliminar;

//Variables Globales
var rowEliminar;

var API_FICHA = "/compras/requisiciones/requisicionmaterial/";

$(document).ready(function () {
    modalConfirmaEliminar = $('#modalConfirmaEliminar');
    rowEliminar = null;
});

var nuevo = function () {
    window.location.href = API_FICHA + "nuevo";
}

var editar = function (event) {
    window.location.href = API_FICHA + "editar/" + event.row.data.RequisicionMaterialId;
}

var ver = function (event) {
    window.location.href = API_FICHA + "ver/" + event.row.data.RequisicionMaterialId;
}

var validaEliminar = function (event) {
    //Obtenemos una copia del objeto a eliminar
    rowEliminar = $.extend(true, {}, event.row.data);

    //Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}

var eliminaRegistro = function () {
    //Mostramos Loader
    dxLoaderPanel.show();

    //Hacemos la petición para eliminar el registro
    $.ajax({
        type: "POST",
        url: API_FICHA + "eliminarPorId",
        data: { requisicionMaterialId: rowEliminar.RequisicionMaterialId, timestamp: rowEliminar.Timestamp },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Registro eliminado con exito!", 'success');

            //Recargamos la ficha
            recargarFicha();
        },
        error: function (response, status, error) {
            //Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al eliminar:\n" + response.responseText, 'error');
        }
    });

    rowEliminar = null;
}

var recargarFicha = function () {
    //Recargamos la ficha
    window.location.href = API_FICHA + "listar";
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}