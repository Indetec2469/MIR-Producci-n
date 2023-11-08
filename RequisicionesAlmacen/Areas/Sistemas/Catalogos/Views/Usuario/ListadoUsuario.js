// VARIABLES GLOBALES //
var API_FICHA = "/sistemas/catalogos/usuario/";
//////////////////////

redirigirNuevo = () => {
    window.location.href = API_FICHA + 'nuevo';
}

redirigirEditar = (params) => {
    window.location.href = API_FICHA + 'editar/' + params.row.data.UsuarioId;
}