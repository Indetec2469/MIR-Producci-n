//Modales
var modalProspectoProveedor;
var modalConfirmaDeshacerCambios;
var modalConfirmaEliminar;

//Componentes
var dxCboEstado;
var dxCboMunicipio;

//Botones
var dxButtonDeshacer;
var dxButtonGuardaCambios;

//Grids
var dxGridProspectosProveedor;

//Forms
var dxFormModalProveedorProspecto;

//Variables Globales
var modeloVacio;
var rowEliminar;
var registrosEliminados;
var contadorRegistrosNuevos;

//Variables Estaticas
var ESTATUS_NUEVO = 0;
var ESTATUS_CARGADO = 1;
var ESTATUS_EDITADO = 2;
var ESTATUS_ELIMINADO = 3;

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();

    //Deshabilitamos los botones de acciones
    habilitaComponentes(false);

    //Respaldamos el modelo vacio del Form
    modeloVacio = $.extend(true, {}, dxFormModalProveedorProspecto.option('formData'));
});

var inicializaVariables = function () {
    modalProspectoProveedor = $('#modalProspectoProveedor');
    modalConfirmaDeshacerCambios = $('#modalConfirmaDeshacerCambios');
    modalConfirmaEliminar = $('#modalConfirmaEliminar');

    dxCboEstado = $('#dxCboEstado').dxSelectBox("instance");
    dxCboMunicipio = $('#dxCboMunicipio').dxSelectBox("instance");

    dxButtonDeshacer = $('#dxButtonDeshacer').dxButton("instance");
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");

    dxGridProspectosProveedor = $('#dxGridProspectosProveedor').dxDataGrid("instance");
    dxFormModalProveedorProspecto = $("#dxFormModalProspectoProveedor").dxForm("instance");

    contadorRegistrosNuevos = 0;
    registrosEliminados = [];
}

var habilitaComponentes = function (enabled) {
    dxButtonDeshacer.option("disabled", !enabled);
    dxButtonGuardaCambios.option("disabled", !enabled);
}

var cboPaisChange = function (event) {
    var paisId = event.selectedItem?.PaisId || null;

    dxCboEstado.option("value", null);
    dxCboEstado.option("dataSource", null);

    if (paisId && paisId == 'MX') {
        dxCboEstado.option("dataSource", _listEstados);
    }
}

var cboEstadoChange = function (event) {
    var estadoId = event.selectedItem?.EstadoId || null;

    dxCboMunicipio.option("value", null);
    dxCboMunicipio.option("dataSource", null);

    if (estadoId) {
        dxCboMunicipio.option("dataSource", _listMunicipios.filter(x => x.EstadoId == estadoId));
    }
}

var nuevoProspectoProveedor = function () {
    //Inicializamos el modelo del Form
    dxFormModalProveedorProspecto.option("formData", $.extend(true, {}, modeloVacio));

    //Inicializamos los campos del Form
    dxFormModalProveedorProspecto.resetValues();    
    
    //Asignamos un ProspectoProveedorId al Form
    dxFormModalProveedorProspecto.updateData("ProveedorProspectoId", contadorRegistrosNuevos);

    //Cambiamos el estatus del registro a "NUEVO"
    dxFormModalProveedorProspecto.updateData("EstatusId", ESTATUS_NUEVO);

    //Le ponemos un Codigo temporal al nuevo proveedor
    dxFormModalProveedorProspecto.updateData("CodigoProspecto", 'PAP000000');

    //Decrementamos el contador de Registros para el siguiente nuevo registro
    contadorRegistrosNuevos -= 1;

    //Marcamos el modal con estatus "NUEVO"
    modalProspectoProveedor.attr("estatus", ESTATUS_NUEVO);    

    //Mostramos el modal 
    modalProspectoProveedor.modal('show');
}

var editaProspectoProveedor = function (event) {
    //Obtenemos una copia del objeto a modificar
    var prospectoProveedor = $.extend(true, {}, event.row.data);

    //Cambiamos el estatus del registro a "EDITADO" solo si no es un registro "NUEVO"
    prospectoProveedor.EstatusId = prospectoProveedor.ProveedorProspectoId <= 0 ? ESTATUS_NUEVO : ESTATUS_EDITADO;

    //Le pasamos el objeto al Form para que cargue sus valores
    dxFormModalProveedorProspecto.option("formData", prospectoProveedor);

    //Marcamos el modal con estatus "EDITADO"
    modalProspectoProveedor.attr("estatus", ESTATUS_EDITADO);

    //Mostramos el modal
    modalProspectoProveedor.modal('show');
}

var existeRFCRazonSocial = function (proveedorProspectoId, rfc, razonSocial) {
    var prospectos;

    //Obtenemos todos los registros que hay en el dxGridProspectosProveedor
    dxGridProspectosProveedor.getDataSource().store().load().done((res) => { prospectos = res; });

    //Buscamos un registro que tenga el mismo RFC y Razon Social y que no tenga el mismo ID del registro que estamos validando
    var prospectoEncontrado = prospectos.find(x => x.ProveedorProspectoId !== proveedorProspectoId && x.RFC.toLowerCase() === rfc.toLowerCase() && x.RazonSocial.toLowerCase() === razonSocial.toLowerCase());

    //Retornamos <true> si se encontro un registro , <false> de lo contrario
    return prospectoEncontrado;
}

var guardaCambiosModal = function () {
    //Obtenemos el Objeto que se esta creando/editando en el Form 
    var prospectoProveedor = dxFormModalProveedorProspecto.option("formData");

    //Validamos que la informacion requerida del Formulario este completa
    if (!dxFormModalProveedorProspecto.validate().isValid)
        return;

    //Validamos que el RFC y Razón Social no se repitan 
    if (existeRFCRazonSocial(prospectoProveedor.ProveedorProspectoId, prospectoProveedor.RFC, prospectoProveedor.RazonSocial)) {
        toast("Ya existe un registro con el mismo RFC y Razón Social. Favor de verificar.", 'warning')
        return;
    }

    //Obtenemos la instancia store del DataSource
    var store = dxGridProspectosProveedor.getDataSource().store();

    //Si es un registro nuevo, lo insertamos en el DataSource
    if (modalProspectoProveedor.attr("estatus") == ESTATUS_NUEVO) {
        store.insert(prospectoProveedor)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxGridProspectosProveedor.getDataSource().reload();

                //Habilitamos los botones de acciones
                habilitaComponentes(true);

                //Ocultamos el modal
                modalProspectoProveedor.modal('hide');
            })
            .fail(function () {
                toast("No se pudo agregar el nuevo registro a la tabla.", "error");
            });
    }
    //Si es un registro que se esta editando, actualizamos su informacion en el DataSource
    else {
        store.update(prospectoProveedor.ProveedorProspectoId, prospectoProveedor)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxGridProspectosProveedor.getDataSource().reload();
                //Habilitamos los botones de acciones
                habilitaComponentes(true);
                //Ocultamos el modal
                modalProspectoProveedor.modal('hide');
            })
            .fail(function () {
                toast("No se pudo actualizar el registro en la tabla.", "error");
            });
    }
}

var validaEliminar = function (event) {
    //Obtenemos una copia del objeto a eliminar
    rowEliminar = $.extend(true, {}, event.row.data);

    //Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}

var eliminaProspectoProveedor = function () {
    //Obtenemos la instancia store del DataSource
    var store = dxGridProspectosProveedor.getDataSource().store();

    //Eliminamos el registro de la tabla
    if (rowEliminar != null) {
        store.remove(rowEliminar.ProveedorProspectoId)
            .done(function () {
                //Si el registro viene de la base de datos, respaldamos el registro 
                //para posteriormente eliminarlo en la base de datos
                if (rowEliminar.ProveedorProspectoId > 0) {
                    //Actualizamos el estatus del registro a "Eliminado"
                    rowEliminar.EstatusId = ESTATUS_ELIMINADO;

                    //Respaldamos el registro que se acaba de eliminar
                    registrosEliminados.push(rowEliminar);
                }

                //Recargamos la informacion de la tabla
                dxGridProspectosProveedor.getDataSource().reload();

                //Habilitamos los botones de acciones
                habilitaComponentes(true);
            })
            .fail(function () {
                toast("No se pudo eliminar el registro de la tabla.", "error");
            });

        rowEliminar = null;
    }
}

var guardaCambios = function () {
    var prospectosProveedor;

    //Obtenemos todos los registros que hay en el dxGridProspectosProveedor
    dxGridProspectosProveedor.getDataSource().store().load().done((res) => { prospectosProveedor = res; });

    //Agregamos los registros borrados, para eliminarlos en base de datos
    prospectosProveedor = $.merge(prospectosProveedor, registrosEliminados);

    //Mostramos Loader
    dxLoaderPanel.show();

    //Enviamos la informacion al controlador
    $.ajax({
        type: "POST",
        url: "/compras/catalogos/proveedoresprospectos/guardar",
        data: { prospectosProveedor: prospectosProveedor },
        success: function () {
            //Mostramos mensaje de Exito
            toast("Registros guardados con exito!", 'success');

            //Recargamos la ficha
            recargarFicha();
        },
        error: function (response, status, error) {
            //Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al guadar:\n" + response.responseText, 'error');
        }
    });
}

var recargarFicha = function () {
    //Recargamos la ficha
    window.location.href = '/compras/catalogos/proveedoresprospectos';
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}