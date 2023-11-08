var nuevoEmpleado = function () {
    window.location.href = '/rh/ingreso/empleados/nuevo';
}

var editaEmpleado = function (event) {
    window.location.href = '/rh/ingreso/empleados/editar/' + event.row.data.EmpleadoId;
}

var nombreCompletoEmpleado = function (event) {
    return event.Nombre + " " + event.PrimerApellido + (event.SegundoApellido ? " " + event.SegundoApellido : "");
}