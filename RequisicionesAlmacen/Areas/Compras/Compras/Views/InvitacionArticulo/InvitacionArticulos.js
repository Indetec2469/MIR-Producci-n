//Modales
var modalConfirmaEliminar;

//Botones
var dxButtonGuardaCambios;

//Forms
var dxGridDetalles;

//Variables Globales
var rowEliminar;

//Variables de Control
var invitacionCompraId;

var API_FICHA = "/compras/compras/invitacionarticulo/";

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();

    //Deshabilitamos los botones de acciones
    habilitaComponentes();
});

var inicializaVariables = function () {
    modalConfirmaEliminar = $('#modalConfirmaEliminar');

    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");

    dxGridDetalles = $("#dxGridDetalles").dxDataGrid("instance");

    rowEliminar = null;
}

var habilitaComponentes = function () {
}

var setCambios = function () {
    habilitaComponentes();
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
        url: API_FICHA + "eliminarPorModelo",
        data: { invitacionArticulo: rowEliminar },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Registro eliminado con exito!", 'success');

            //Recargamos la ficha
            recargarFicha();
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al eliminar:\n" + response.responseText, 'error');
        }
    });

    rowEliminar = null;
}

var guardaCambios = function () {
    //Obtenemos todos los registros que hay en el dxGridDetalles
    var detalles;
    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

    //Validamos que haya registros seleccionados
    if (!detalles.find(x => x.Seleccionado === true)) {
        toast("No hay artículos seleccionados por convertir.", 'error'); return;
    }

    //Añadimos los registros por convertir
    var articulosInvitacion = [];

    detalles.forEach(m => {
        if (m.Seleccionado === true) {
            articulosInvitacion.push(m);
        }
    });    

    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "guardaCambios",
        data: { articulosInvitacion: articulosInvitacion },
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

var onEditingStart = function (event) {
    event.cancel = invitacionCompraId && event.data.InvitacionCompraId != invitacionCompraId;
}

var onDetallesChange = function (e) {
    if (e.name === "editing") {
        var editRowKey = e.component.option("editing.editRowKey");
        var changes = e.component.option("editing.changes");

        changes = changes.map((change) => {
            return {
                type: change.type,
                key: change.type !== "insert" ? change.key : undefined,
                data: change.data
            };
        });

        if (changes && changes.length) {
            setCambios();

            var cambios = changes[0].data;
            var propiedad = Object.getOwnPropertyNames(cambios)[0];

            if (propiedad == "Seleccionado") {
                var seleccionado = cambios.Seleccionado;

                //Obtenemos todos los registros que hay en el dxGridDetallesPorComprar
                var detalles;
                dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });
                
                var row = detalles.find(x => x.InvitacionCompraDetalleId === changes[0].key);
                var rowOtro = detalles.find(x => x.Seleccionado === true && x.InvitacionCompraDetalleId != row.InvitacionCompraDetalleId);

                //Obtenemos el id de la cabecera
                invitacionCompraId = seleccionado === true ? row.InvitacionCompraId : rowOtro ? rowOtro.InvitacionCompraId : null;
            }
        }
    }
}

var recargarFicha = function () {
    // Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA;
}

var toast = function (mensaje, type) {
    DevExpress.ui.notify({ message: mensaje, width: "auto" }, type, 5000);
}