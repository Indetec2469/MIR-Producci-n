// VARIABLES GLOBALES //
var modalConfirmaEliminar,
    rowEliminar,
    // API
    API_FICHA = "/mir/mir/matrizindicadorresultado/";
//////////////////////

// Function Default //
$(() => {
    // Inicializamos las variables para la ficha
    inicializaVariables();

});
//////////////////////

inicializaVariables = () => {
    modalConfirmaEliminar = $('#modalConfirmaEliminar');
}

habilitaAccionVer = (event) => {
    if (event.row.data.Edicion == 0) {
        return true;
    } else {
        return false;
    }
}

habilitaAccion = (event) => {
    if (event.row.data.Edicion == 1) {
        return true;
    } else {
        return false;
    }
}

habilitaAccionVer = (event) => {
    if (event.row.data.Edicion == 1) {
        return false;
    } else {
        return true;
    }
}

// MODAL //
confirmaEliminarModal = (event) => {
    // Obtenemos una copia del objeto a eliminar
    rowEliminar = $.extend(true, {}, event.row.data);

    // Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}
///////////

validaEliminar = (event) => {
    //Obtenemos una copia del objeto a eliminar
    rowEliminar = $.extend(true, {}, event.row.data);

    if (rowEliminar.Edicion == 0) {
        toast("No es posible eliminar una MIR en proceso.", 'warning');

        rowEliminar = null;

        return;
    }

    //Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}

eliminaRow = () => {
    // Ocultamos el modal confirma eliminar
    modalConfirmaEliminar.modal('hide');

    //Enviamos la informacion al controlador
    $.ajax({
        type: 'DELETE',
        url: API_FICHA + 'eliminar',
        data: { id: rowEliminar.MIRId },
        success: function () {
            //Mostramos mensaje de Exito
            toast('Registro borrado con exito!', 'success');

            //Regresamos al listado
            recargarFicha();
        },
        error: function (response, status, error) {
            //Mostramos mensaje de error
            toast(response.responseText, 'error');
        }
    });
}

redirigirNuevo = () => {
    window.location.href = API_FICHA + 'nuevo';
}

redirigirEditar = (params) => {
    window.location.href = API_FICHA + 'editar/' + params.row.data.MIRId;
}

recargarFicha = () => {
    window.location.href = API_FICHA + 'listar';
}

toast = (message, type) => {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 3500);
}