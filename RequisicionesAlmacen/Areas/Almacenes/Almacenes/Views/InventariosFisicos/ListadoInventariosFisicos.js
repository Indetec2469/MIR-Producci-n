var API_FICHA = "/almacenes/almacenes/inventariosfisicos/";

var nuevo = function () {
    window.location.href = API_FICHA + "nuevo";
}

var editar = function (event) {
    window.location.href = API_FICHA + "editar/" + event.row.data.InventarioFisicoId;
}