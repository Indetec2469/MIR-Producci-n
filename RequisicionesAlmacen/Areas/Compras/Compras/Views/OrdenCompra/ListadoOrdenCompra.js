//Modales
var modalConfirmaEliminar;
var dxFormModalCancelacion;

//Variables Globales
var rowEliminar;
var dxTxtFechaCancelacion;

var API_FICHA = "/compras/compras/ordencompra/";

$(document).ready(function () {
    modalConfirmaEliminar = $('#modalConfirmaEliminar');
    dxFormModalCancelacion = $("#dxFormModalCancelacion").dxForm("instance");
    dxTxtFechaCancelacion = $('#dxTxtFechaCancelacion').dxDateBox("instance");

    var date = new Date();
    dxTxtFechaCancelacion.option("value", new Date(date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate()).getTime());

    rowEliminar = null;
});

var nuevo = function () {
    window.location.href = API_FICHA + "nuevo";
}

var editar = function (event) {
    window.location.href = API_FICHA + "editar/" + event.row.data.OrdenCompraId;
}

var ver = function (event) {
    window.location.href = API_FICHA + "ver/" + event.row.data.OrdenCompraId;
}

var validaEliminar = function (event) {
    //Obtenemos una copia del objeto a eliminar
    rowEliminar = $.extend(true, {}, event.row.data);

    //Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}

var eliminaRegistro = function () {
    //Obtenemos el Objeto que se esta creando/editando en el Form 
    var modelo = dxFormModalCancelacion.option("formData");

    if (!modelo.FechaCancelacion) {
        //Mostramos mensaje de error
        toast("Favor de agregar la fecha de cancelación.", 'warning');

        return;
    }

    var fechaCancelacion = new Date(modelo.FechaCancelacion);
    fechaCancelacion.setHours(0);
    fechaCancelacion.setMinutes(0);
    fechaCancelacion.setSeconds(0);
    fechaCancelacion.setMilliseconds(0);

    fechaCancelacion = fechaCancelacion.toLocaleString();

    //Ocultamos el modal
    modalConfirmaEliminar.modal('hide');

    //Mostramos Loader
    dxLoaderPanel.show();

    //Hacemos la petición para eliminar el registro
    $.ajax({
        type: "POST",
        url: API_FICHA + "eliminarPorId",
        data: { ordenCompraId: rowEliminar.OrdenCompraId, status: rowEliminar.Status, fechaCancelacion: fechaCancelacion },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Registro cancelado con exito!", 'success');

            //Recargamos la ficha
            recargarFicha();
        },
        error: function (response, status, error) {
            //Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al cancelar:\n" + response.responseText, 'error');
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