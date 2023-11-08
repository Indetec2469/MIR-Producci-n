// VARIABLES GLOBALES //
var modalConfirmaEliminar,
    rowEliminar,
    // API
    API_FICHA = "/sistemas/catalogos/rol/";
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

// MODAL //
confirmaEliminarModal = (event) => {
    // Obtenemos una copia del objeto a eliminar
    rowEliminar = $.extend(true, {}, event.row.data);

    // Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}
///////////

eliminaRow = () => {
    // Ocultamos el modal confirma eliminar
    modalConfirmaEliminar.modal('hide');

    //Enviamos la informacion al controlador
    $.ajax({
        type: 'DELETE',
        url: API_FICHA + 'eliminar',
        data: { id: rowEliminar.RolId },
        success: function () {
            // Mostramos mensaje de Exito
            toast('Registro borrado con exito!', 'success');

            // Regresamos al listado
            recargarFicha();
        },
        error: function (response, status, error) {
            //Mostramos mensaje de error
            toast('Error al guadar:\n' + response.responseText, 'error');
        }
    });
}

redirigirNuevo = () => {
    window.location.href = API_FICHA + 'nuevo';
}

redirigirEditar = (event) => {
    window.location.href = API_FICHA + 'editar/' + event.row.data.RolId;
}

recargarFicha = () => {
    window.location.href = API_FICHA + 'listar';
}
// Toast //
toast = (message, type) => {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 3500);
}
///////////