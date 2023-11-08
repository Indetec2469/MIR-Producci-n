//Modales
var modalConfirmaDeshacer;
var modalConfirmaEliminar;

//Botones
var dxButtonCancelar;
var dxButtonDeshacer;
var dxButtonEliminar;
var dxButtonGuardaCambios;

//Forms
var dxForm;
var dxTreeList;

//Variables Globales
var listaSeleccionadoPermisoFichaId = [];

//Variables de Control
var cambios;
var eventoRegresar;

var API_FICHA = '/sistemas/catalogos/usuario/';

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();

    //Deshabilitamos los botones de acciones
    habilitaComponentes();
});

var inicializaVariables = function () {
    modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    modalConfirmaEliminar = $('#modalConfirmaEliminar');

    dxButtonCancelar = $('#dxButtonCancelar').dxButton("instance");
    dxButtonDeshacer = $('#dxButtonDeshacer').dxButton("instance");
    dxButtonEliminar = $('#dxButtonEliminar').dxButton("instance");
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");

    dxForm = $('#dxForm').dxForm('instance');
    dxTreeList = $('#dxTreeList').dxTreeList('instance');

    if (_usuario.UsuarioId > 0) {
        dxForm.getEditor('ConfirmarContrasenia').option('value', dxForm.getEditor('Contrasenia').option('value'));
    }

    //Carga los seleccionados de Rol Menu
    cargaSeleccionadosUsuarioPermiso();

    cambios = false;
    eventoRegresar = true;
}

var cargaSeleccionadosUsuarioPermiso = function () {
    if (_usuario.UsuarioId > 0) {
        _listaUsuarioPermiso.map(up => {
            listaSeleccionadoPermisoFichaId.push(up.PermisoFichaId);
        });

        //Asignamos a componente TreeList para seleccionados
        dxTreeList.option('selectedRowKeys', listaSeleccionadoPermisoFichaId);
    }
}

var habilitaComponentes = function () {
    dxButtonDeshacer.option("disabled", !cambios);
    dxButtonEliminar.option("visible", !cambios && _usuario.UsuarioId > 0);
}

var setCambios = function () {
    cambios = true;
    habilitaComponentes();
}

var onSelectionChanged = function (event) {
    listaSeleccionadoPermisoFichaId = event.component.getSelectedRowKeys('leavesOnly');

    if (validaHayCambios()) {
        setCambios();
    }
}

var validaHayCambios = function () {
    //Obtener los datos para saber hay los datos o no hay para guardar
    const data = obtenerData();

    return !(data.usuario == null && data.listaUsuarioPermiso.length == 0);
}

var guardaCambios = function () {
    //Validamos que la informacion requerida del Formulario este completa
    if (!dxForm.validate().isValid) {
        toast("Favor de completar los campos requeridos.", 'error');

        return;
    }

    //Obtener los datos para saber hay los datos o no hay para guardar
    if (!validaHayCambios()) {
        toast("No existen cambios pendientes por guardar.", "warning");

        return;
    }

    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "guardar",
        data: obtenerData(),
        success: function (response) {
            //Mostramos mensaje de Exito
            toast(response, 'success');

            //Redirigir a listado
            regresarListado();
        },
        error: function (response, status, error) {
            //Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast(response.responseText, "error");
        }
    });
}

var obtenerData = function () {
    let data = {
        usuario: null,
        listaUsuarioPermiso: []
    };

    var usuario = $.extend(true, {}, dxForm.option('formData'));

    if (usuario.UsuarioId > 0) {
        if (usuario.NombreUsuario != _usuario.NombreUsuario || usuario.Contrasenia != _usuario.Contrasenia || usuario.RolId != _usuario.RolId || usuario.EmpleadoId != _usuario.EmpleadoId || usuario.Activo != _usuario.Activo) {
            let actualizarUsuario = usuario;
            data.usuario = actualizarUsuario;
        }
    } else {
        data.usuario = usuario;
    }

    listaSeleccionadoPermisoFichaId.map(permisoFichaId => {
        const usuarioPermiso = _listaUsuarioPermiso.find(up => up.PermisoFichaId == permisoFichaId);

        if (usuarioPermiso == null || usuarioPermiso == undefined) {
            let nuevoUsuarioPermiso = $.extend(true, {}, _usuarioPermisoModel);
            nuevoUsuarioPermiso.UsuarioId = usuario.UsuarioId;
            nuevoUsuarioPermiso.PermisoFichaId = permisoFichaId;
            nuevoUsuarioPermiso.EstatusId = ControlMaestroMapeo.EstatusRegistro.ACTIVO;

            data.listaUsuarioPermiso.push(nuevoUsuarioPermiso);
        }
    });

    _listaUsuarioPermiso.filter(up => listaSeleccionadoPermisoFichaId.indexOf(up.PermisoFichaId) == -1).map(up => {
        let actualizarUsuarioPermiso = $.extend(true, {}, up);
        actualizarUsuarioPermiso.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO;
        actualizarUsuarioPermiso.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(actualizarUsuarioPermiso.Timestamp)));

        data.listaUsuarioPermiso.push(actualizarUsuarioPermiso);
    });

    return data;
}

var validaEliminar = function (event) {
    //Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}

var eliminar = function () {
    //Mostramos Loader
    dxLoaderPanel.show();

    //Enviamos la informacion al controlador
    $.ajax({
        type: 'DELETE',
        url: API_FICHA + 'eliminar',
        data: { id: _usuario.UsuarioId },
        success: function () {
            //Mostramos mensaje de Exito
            toast('Registro borrado con éxito!', 'success');

            //Regresamos al listado
            regresarListado();
        },
        error: function (response, status, error) {
            //Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast(response.responseText, 'error');
        }
    });
}

var onFieldDataChanged = function (event) {
    if (event.dataField != 'ConfirmarContrasenia') {

        setCambios()
    }
}

displayEmpleado = (event) => {
    if (event) {
        return event.NumeroEmpleado + ' - ' + event.Nombre + ' ' + event.PrimerApellido;
    }
}

var comparisonContrasenia = function () {
    return dxForm.option('formData').Contrasenia;
}

var onValueChangedActivo = function (event) {
    if (event.event) {
        var usuario = dxForm.option('formData');

        if (usuario.UsuarioId == 0) {
            event.component.option('value', true);
            //Mostramos mensaje
            toast("No se puede editar, primera guardar los datos.", 'warning');
        }

        setCambios();
    }
}

var validationCallbackValidaUsuario = function (event) {
    let esValida = true;
    var usuario = dxForm.option('formData');

    usuario.NombreUsuario = event.value;

    $.ajax({
        async: false,
        type: 'POST',
        url: API_FICHA + 'validausuario',
        data: { usuario: usuario },
        success: function (response) {
            esValida = response;
        }
    });

    return esValida;
}

var validaDeshacer = function (regresar) {
    eventoRegresar = regresar;

    if (cambios) {
        //Mostramos el modal de confirmacion
        modalConfirmaDeshacer.modal('show');
    } else {
        cancelar();
    }
}

var cancelar = function () {
    if (eventoRegresar) {
        //Regresamos al listado
        regresarListado();
    } else {
        //Recargamos la ficha
        recargarFicha();
    }
}

var regresarListado = function () {
    window.location.href = API_FICHA + "listar";
}

var recargarFicha = function () {
    //Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA + (_usuario.UsuarioId == 0 ? "nuevo" : "editar/" + _usuario.UsuarioId);
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 3500);
}