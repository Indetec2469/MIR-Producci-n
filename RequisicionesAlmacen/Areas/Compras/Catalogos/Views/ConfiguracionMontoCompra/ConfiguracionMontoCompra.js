//Variables Globales //
var contadorRegistrosNuevos,
    modalConfiguracionMontoCompra,
    modalConfirmaDeshacer,
    modalConfirmaEliminar,
    dxFormConfiguracionMontoCompra,
    dxDataGridConfiguracionMontoCompra,
    dxButtonDeshacer,
    dxButtonGuardar,
    dxPopupDeshacer,
    dxNumberBoxMontoMinimo,
    dxChkSinLimite,
    dxNumberBoxMontoMaximo,
    rowEliminar;
////////////////////////

//Function Default //
$(() => {
    //Inicializamos las variables para la ficha
    inicializaVariables();

    //Inhabilitamos los botones de acciones
    habilitaComponentes(false);
});
//////////////////////

inicializaVariables = () => {
    contadorRegistrosNuevos = 0;
    modalConfiguracionMontoCompra = $('#modalConfiguracionMontoCompra');
    modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    modalConfirmaEliminar = $('#modalConfirmaEliminar');
    dxFormConfiguracionMontoCompra = $("#dxFormConfiguracionMontoCompra").dxForm("instance");
    dxDataGridConfiguracionMontoCompra = $("#dxDataGridConfiguracionMontoCompra").dxDataGrid("instance");
    dxButtonDeshacer = $("#dxButtonDeshacer").dxButton("instance");
    dxButtonGuardar = $("#dxButtonGuardar").dxButton("instance");
    dxNumberBoxMontoMinimo = $("#dxNumberBoxMontoMinimo").dxNumberBox("instance");
    dxNumberBoxMontoMaximo = $("#dxNumberBoxMontoMaximo").dxNumberBox("instance");
    dxChkSinLimite = $("#dxChkSinLimite").dxCheckBox("instance");
}

habilitaComponentes = (enabled) => {
    dxButtonDeshacer.option("disabled", !enabled);
    dxButtonGuardar.option("disabled", !enabled);
}

//Modal //
nuevoModal = () => {

    //Inicializamos el modelo del Form
    dxFormConfiguracionMontoCompra.option("formData", $.extend(true, {}, _controlMaestroConfiguracionMontoCompra));

    //Limpian los campos del form
    dxFormConfiguracionMontoCompra.resetValues();

    //Asignamos un ProspectoProveedorId al Form
    dxFormConfiguracionMontoCompra.updateData("ConfiguracionMontoId", contadorRegistrosNuevos);

    //Decrementamos el contador de Registros para el siguiente nuevo registro
    contadorRegistrosNuevos -= 1;

    //Cambiamos el titulo en el modal
    modalConfiguracionMontoCompra.find(".modal-title").text("Nueva Configuración");

    //Cambiamos el modo editar es falso en el modal
    modalConfiguracionMontoCompra.attr("isEdit", false);

    //Mosntramos el modal
    modalConfiguracionMontoCompra.modal('show');
}

editaModal = (event) => {
    //Obtenemos una copia del objeto a modificar
    var controlMaestroConfiguracionMontoCompra = $.extend(true, {}, event.row.data);

    //Le pasamos el objeto al Form para que cargue sus valores
    dxFormConfiguracionMontoCompra.option("formData", controlMaestroConfiguracionMontoCompra);

    //Cambiamos el titulo en el modal
    modalConfiguracionMontoCompra.find(".modal-title").text("Editar Configuración");

    //Cambiamos el modo editar es verdad en el modal
    modalConfiguracionMontoCompra.attr("isEdit", true);

    //Mostramos el modal
    modalConfiguracionMontoCompra.modal('show');

    //Validamos el Monto Máximo
    if (!controlMaestroConfiguracionMontoCompra.MontoMaximo) {
        getdxNumberBoxInstance();

        dxChkSinLimite.option("value", true);
    }
}

guardaCambiosModal = () => {

    //Validamos que la informacion requerida del Formulario este completa
    if (!dxFormConfiguracionMontoCompra.validate().isValid)
        return;

    //Obtenemos el Objeto que se esta creando/editando en el Form
    var controlMaestroConfiguracionMontoCompra = dxFormConfiguracionMontoCompra.option("formData");

    //La validación
    if (!esValidacion(controlMaestroConfiguracionMontoCompra)) {
        return;
    }

    //Obtenemos la instancia store del DataSource
    var store = dxDataGridConfiguracionMontoCompra.getDataSource().store();

    //Si el modo editar es falso (Nuevo) o si es verdad (Editar)
    if (modalConfiguracionMontoCompra.attr("isEdit") == "false") {
        //Nuevo
        store.insert(controlMaestroConfiguracionMontoCompra)
            .done(function () {

                //Recargamos la informacion de la tabla
                dxDataGridConfiguracionMontoCompra.getDataSource().reload();

                //Habilitamos los botones de acciones
                habilitaComponentes(true);

                //Ocultamos el modal
                modalConfiguracionMontoCompra.modal('hide');
            })
            .fail(function () {
                toast("No se pudo agregar el nuevo registro a la tabla.", "error");
            });
    } else {
        //Editar
        store.update(controlMaestroConfiguracionMontoCompra.ConfiguracionMontoId, controlMaestroConfiguracionMontoCompra)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxDataGridConfiguracionMontoCompra.getDataSource().reload();

                //Habilitamos los botones de acciones
                habilitaComponentes(true);

                //Ocultamos el modal
                modalConfiguracionMontoCompra.modal('hide');
            })
            .fail(function () {
                toast("No se pudo actualizar el registro en la tabla.", "error");
            });
    }
}

confirmaEliminarModal = (event) => {
    //Obtenemos una copia del objeto a eliminar
    rowEliminar = $.extend(true, {}, event.row.data);

    //Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}
///////////

eliminaRow = () => {
    //Ocultamos el modal confirma eliminar
    modalConfirmaEliminar.modal('hide');

    //Obtenemos la instancia store del DataSource
    var store = dxDataGridConfiguracionMontoCompra.getDataSource().store();

    //Eliminamos el registro de la tabla
    if (rowEliminar != null) {
        //Si el registro viene de la base de datos
        //para posteriormente eliminarlo en la base de datos
        if (rowEliminar.ConfiguracionMontoId > 0) {
            //Actualizamos el estatus del registro a "Borrado"
            rowEliminar.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO;

            store.update(rowEliminar.ConfiguracionMontoId, rowEliminar)
                .done(function () {
                    //Recargamos la informacion de la tabla
                    dxDataGridConfiguracionMontoCompra.getDataSource().reload();

                    //Habilitamos los botones de acciones
                    habilitaComponentes(true);

                })
                .fail(function () {
                    toast("No se pudo actualizar el registro en la tabla.", "error");
                });
        } else {
            store.remove(rowEliminar.ConfiguracionMontoId)
                .done(function () {
                    //Recargamos la informacion de la tabla
                    dxDataGridConfiguracionMontoCompra.getDataSource().reload();

                    //Habilitamos los botones de acciones
                    habilitaComponentes(true);

                })
                .fail(function () {
                    toast("No se pudo eliminar el registro de la tabla.", "error");
                });
        }

        //Limpiar Row Eliminar
        rowEliminar = null;
    }
}

guardaCambios = () => {
    //Mostramos Loader
    dxLoaderPanel.show();

    var listControlMaestroConfiguracionMontoCompra;

    //Obtenemos todos los registros que hay en el DataGrid
    dxDataGridConfiguracionMontoCompra.getDataSource().store().load().done(response => listControlMaestroConfiguracionMontoCompra = response);

    //Enviamos la informacion al controlador
    $.ajax({
        type: "POST",
        url: "/compras/catalogos/configuracionmontocompra/guardar",
        data: { listControlMaestroConfiguracionMontoCompra },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast(response, 'success');

            //Recargamos la ficha
            window.location.href = '/compras/catalogos/configuracionmontocompra/listar';
        },
        error: function (response, status, error) {
            //Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast(response.responseText, "error");
        }
    });
}

recargarFicha = () => {
    modalConfirmaDeshacer.modal('hide');
    window.location.href = '/compras/catalogos/configuracionmontocompra/listar';
}

var getdxNumberBoxInstance = function () {
    //dxNumberBox Instance
    dxNumberBoxMontoMinimo = $("#dxNumberBoxMontoMinimo").dxNumberBox("instance");
    dxNumberBoxMontoMaximo = $("#dxNumberBoxMontoMaximo").dxNumberBox("instance");
    dxChkSinLimite = $("#dxChkSinLimite").dxCheckBox("instance");
}

//Event onChange //
onChangeTipoCompra = (params) => {
    getdxNumberBoxInstance();

    var category = params.value;

    if (category != null) {
        dxFormConfiguracionMontoCompra.itemOption("isTipoCompra", "visible", true);

        //Control Maestro: Tipo Compra -> Compra Directa -> 4
        if (category == ControlMaestroMapeo.TipoCompra.COMPRA_DIRECTA) {
            dxFormConfiguracionMontoCompra.itemOption("isCompraDirecta", "visible", false);
            dxFormConfiguracionMontoCompra.updateData("NumeroMinimoProveedores", null);
        } else {
            dxFormConfiguracionMontoCompra.itemOption("isCompraDirecta", "visible", true);
        }

    } else {
        dxFormConfiguracionMontoCompra.itemOption("isTipoCompra", "visible", false);
        dxFormConfiguracionMontoCompra.itemOption("isCompraDirecta", "visible", false);
    }
}

onChangeSinLimite = (params) => {
    getdxNumberBoxInstance();

    var value = params.value;

    //Is Sin Límite
    if (value) {
        dxNumberBoxMontoMaximo.option({ "value": null, "disabled": true });
    } else {
        dxNumberBoxMontoMaximo.option({ "value": dxNumberBoxMontoMinimo.option("value"), "disabled": false });
    }
}

//Event onValue //
onValueNuermoMinioProveddores = (params) => {
    var value = params.NumeroMinimoProveedores;

    return (value == null ? "N/A" : value);
}

onValueMontoMaximo = (params) => {
    var value = params.MontoMaximo;

    return (value == null ? "Sin límite" : value);
}

//Event onFocus //
onFocusOutMontoMinimo = (params) => {
    getdxNumberBoxInstance();

    var valueMontoMinimo = dxNumberBoxMontoMinimo.option("value");

    if (!dxChkSinLimite.option("value") && (dxNumberBoxMontoMaximo.option("value") == 0 || dxNumberBoxMontoMaximo.option("value") == null)) {
        dxNumberBoxMontoMaximo.option("value", valueMontoMinimo);
    }
}

onFocusOutMontoMaximo = (params) => {
    getdxNumberBoxInstance();

    var valueMontoMinimo = dxNumberBoxMontoMinimo.option("value");

    //Monto Maximo no menor que Monto Minimo
    if (dxNumberBoxMontoMaximo.option("value") <= valueMontoMinimo) {
        dxNumberBoxMontoMaximo.option("value", valueMontoMinimo);
    }
}

var esValidacion = function (configuracion) {
    var listconfiguracionMontoCompra;

    //Obtenemos todos los registros que hay en el DataGrid
    dxDataGridConfiguracionMontoCompra.getDataSource().store().load().done(response => listconfiguracionMontoCompra = response.filter(filter => filter.EstatusId == 1));

    //Tipo Compra
    if (listconfiguracionMontoCompra.some(cmc => cmc.ConfiguracionMontoId != configuracion.ConfiguracionMontoId && cmc.TipoCompraId == configuracion.TipoCompraId)) {
        var dxSelectBoxTipoCompra = dxFormConfiguracionMontoCompra.getEditor("TipoCompraId");

        dxSelectBoxTipoCompra.option({
            validationStatus: 'invalid',
            validationError: { type: "custom", message: "Ya existe el tipo de compra." }
        });

        return false;
    }

    for (const cmc of listconfiguracionMontoCompra.filter(cmc => cmc.TipoCompraId != configuracion.TipoCompraId)) {
        //Validar Monto Mínimo
        if (configuracion.MontoMinimo >= cmc.MontoMinimo && (!cmc.MontoMaximo || configuracion.MontoMinimo <= cmc.MontoMaximo)) {
            toast("El monto mínimo ya existe en el tipo de compra: " + _listTipoCompra.find(tc => tc.ControlId == cmc.TipoCompraId).Valor + ", el monto mínimo debe ser menor a $" + (cmc.MontoMinimo).toFixed(2) + (!cmc.MontoMaximo ? "" : " o mayor a $" + (cmc.MontoMaximo).toFixed(2)), "warning");

            return false;
        }

        //Validar Monto Máximo
        if ((!configuracion.MontoMaximo && configuracion.MontoMinimo <= cmc.MontoMinimo)
            || (configuracion.MontoMaximo && configuracion.MontoMaximo >= cmc.MontoMinimo && (!cmc.MontoMaximo || configuracion.MontoMaximo <= cmc.MontoMaximo))) {
                toast("El monto máximo ya existe en el tipo de compra: " + _listTipoCompra.find(tc => tc.ControlId == cmc.TipoCompraId).Valor + ", el monto máximo debe ser menor a $" + (cmc.MontoMinimo).toFixed(2), "warning");

                return false;
        }
    }

    return true;
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 1500);
}