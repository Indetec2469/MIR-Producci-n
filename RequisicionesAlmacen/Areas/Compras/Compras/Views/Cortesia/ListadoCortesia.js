var API_FICHA = "/compras/compras/cortesia/";

$(document).ready(function () { });

var nuevo = function () {
    window.location.href = API_FICHA + "nuevo";
}

var ver = function (event) {
    window.location.href = API_FICHA + "ver/" + event.row.data.CortesiaId;
}

var recargarFicha = function () {
    //Recargamos la ficha
    window.location.href = API_FICHA + "listar";
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}