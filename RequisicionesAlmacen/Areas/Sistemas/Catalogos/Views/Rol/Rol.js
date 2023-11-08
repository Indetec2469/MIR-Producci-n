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
var listaSeleccionadoNodoMenuId = [];

//Variables de Control
var cambios;
var eventoRegresar;

var API_FICHA = '/sistemas/catalogos/rol/';

$(document).ready(function () {
    //Inicializamos las variables para la ficha
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

    //Carga los seleccionados de Rol Menu
    cargaSeleccionadosRolMenu();

    cambios = false;
    eventoRegresar = true;
}

var cargaSeleccionadosRolMenu = function () {
    if (_rol.RolId > 0) {
        _listRolMenu.map(rm => {
            listaSeleccionadoNodoMenuId.push(rm.NodoMenuId);
        });
        
        //Asignamos a componente TreeList para seleccionados
        dxTreeList.option('selectedRowKeys', listaSeleccionadoNodoMenuId);

        _rol.EsActivo = _rol.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO ? true : false;
        dxForm.getEditor('EsActivo').option('value', _rol.EsActivo);
    }
}

var habilitaComponentes = function () {
    dxButtonDeshacer.option("disabled", !cambios);
    dxButtonEliminar.option("visible", !cambios && _rol.RolId > 0);
}

var setCambios = function () {
    cambios = true;
    habilitaComponentes();
}

var onValueChangedActivo = function (event) {
    var rol = dxForm.option('formData');

    if (rol.RolId == 0) {
        event.component.option('value', true);

        //Mostramos mensaje
        toast("No se puede editar, primera guardar los datos.", 'warning');
    }

    setCambios();
}

var onSelectionChanged = function (event) {
    listaSeleccionadoNodoMenuId = event.component.getSelectedRowKeys('leavesOnly');

    if (validaHayCambios()) {
        setCambios();
    }
}

var validaHayCambios = function () {
    //Obtener los datos para saber hay los datos o no hay para guardar
    const data = obtenerData();

    return !(data.rol == null && data.listRolMenu.length == 0);
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
        type: 'POST',
        url: API_FICHA + 'guardar',
        data: obtenerData(),
        success: function (response) {
            //Mostramos mensaje de Exito
            toast(response, 'success');

            //Redirigir a listado
            window.location.href = API_FICHA + 'listar';
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
        rol: null,
        listRolMenu: []
    };

    var rol = dxForm.option('formData');

    if (rol.RolId > 0) {
        if (rol.Nombre != _rol.Nombre || rol.Descripcion != _rol.Descripcion || rol.EsActivo != _rol.EsActivo) {
            let actualizarRol = $.extend(true, {}, _rol);
            actualizarRol.Nombre = rol.Nombre;
            actualizarRol.Descripcion = rol.Descripcion;
            actualizarRol.EstatusId = rol.EsActivo ? ControlMaestroMapeo.EstatusRegistro.ACTIVO : ControlMaestroMapeo.EstatusRegistro.INACTIVO;
            actualizarRol.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(actualizarRol.Timestamp)));

            data.rol = actualizarRol;
        }
    } else {
        rol.EstatusId = rol.EsActivo ? ControlMaestroMapeo.EstatusRegistro.ACTIVO : ControlMaestroMapeo.EstatusRegistro.INACTIVO;
        data.rol = rol;
    }

    listaSeleccionadoNodoMenuId.map(nodoMenuId => {
        const rolMenu = _listRolMenu.find(rm => rm.NodoMenuId == nodoMenuId);
        if (rolMenu == null || rolMenu == undefined) {
            let nuevoRolMenu = $.extend(true, {}, _rolMenuModel);
            nuevoRolMenu.RolId = rol.RolId;
            nuevoRolMenu.NodoMenuId = nodoMenuId;
            nuevoRolMenu.EstatusId = ControlMaestroMapeo.EstatusRegistro.ACTIVO;

            data.listRolMenu.push(nuevoRolMenu);
        }
    });

    _listRolMenu.filter(rm => listaSeleccionadoNodoMenuId.indexOf(rm.NodoMenuId) == -1).map(rm => {
        let actualizarRol = $.extend(true, {}, rm);
        actualizarRol.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO;
        actualizarRol.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(actualizarRol.Timestamp)));

        data.listRolMenu.push(actualizarRol);
    });

    return data;
}

var validaEliminar = function (event) {
    //Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}

var eliminaRow = function () {
    //Ocultamos el modal confirma eliminar
    modalConfirmaEliminar.modal('hide');

    //Enviamos la informacion al controlador
    $.ajax({
        type: 'DELETE',
        url: API_FICHA + 'eliminar',
        data: { id: _rol.RolId },
        success: function () {
            //Mostramos mensaje de Exito
            toast('Registro borrado con exito!', 'success');

            //Regresamos al listado
            window.location.href = API_FICHA + 'listar ';
        },
        error: function (response, status, error) {
            //Mostramos mensaje de error
            toast('Error al guadar:\n' + response.responseText, 'error');
        }
    });
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
    window.location.href = API_FICHA + (_rol.RolId == 0 ? "nuevo" : "editar/" + _rol.RolId);
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 3500);
}