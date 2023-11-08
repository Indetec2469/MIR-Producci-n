//Modales
var modalConfirmaEliminar;

//Variables Globales
var rowEliminar;

var API_FICHA = "/almacenes/almacenes/solicitudporsurtir/";

var ver = function (event) {
    window.location.href = API_FICHA + "editar/" + event.row.data.RequisicionMaterialId;
}