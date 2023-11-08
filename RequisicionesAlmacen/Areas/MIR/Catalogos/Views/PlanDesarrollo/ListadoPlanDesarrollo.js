var nuevo = function () {
    window.location.href = '/mir/catalogos/plandesarrollo/nuevo';
}

var editar = function (event) {
    window.location.href = '/mir/catalogos/plandesarrollo/editar/' + event.row.data.Codigo;
}