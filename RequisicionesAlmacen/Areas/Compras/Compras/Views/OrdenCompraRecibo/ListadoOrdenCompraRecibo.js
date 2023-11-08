var API_FICHA = "/compras/compras/ordencomprarecibo/";

var nuevo = function () {
    window.location.href = API_FICHA + "nuevo";
}

var ver = function (event) {
    window.location.href = API_FICHA + "ver/" + event.row.data.CompraId;
}

var recargarFicha = function () {
    //Recargamos la ficha
    window.location.href = API_FICHA + "listar";
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}