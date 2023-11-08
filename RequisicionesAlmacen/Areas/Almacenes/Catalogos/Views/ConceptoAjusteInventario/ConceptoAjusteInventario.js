// Variables Globales //
var contadorRegistrosNuevos,
    modalConceptoAjusteInventario,
    modalConfirmaDeshacer,
    modalConfirmaEliminar,
    dxFormConceptoAjusteInventario,
    dxDataGridConceptoAjusteInventario,
    dxButtonDeshacer,
    dxButtonGuardar,
    rowEliminar;
////////////////////////

// Function Default //
$(() => {
    // Inicializamos las variables para la ficha
    inicializaVariables();

    // Inhabilitamos los botones de acciones
    habilitaComponentes(false);
});
//////////////////////

inicializaVariables = () => {
    contadorRegistrosNuevos = 0;
    modalConceptoAjusteInventario = $('#modalConceptoAjusteInventario');
    modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    modalConfirmaEliminar = $('#modalConfirmaEliminar');
    dxFormConceptoAjusteInventario = $("#dxFormConceptoAjusteInventario").dxForm("instance");
    dxDataGridConceptoAjusteInventario = $("#dxDataGridConceptoAjusteInventario").dxDataGrid("instance");
    dxButtonDeshacer = $("#dxButtonDeshacer").dxButton("instance");
    dxButtonGuardar = $("#dxButtonGuardar").dxButton("instance");
}

habilitaComponentes = (enabled) => {
    dxButtonDeshacer.option("disabled", !enabled);
    dxButtonGuardar.option("disabled", !enabled);
}

// Modal //
nuevoModal = () => {

    // Limpian los campos del form
    dxFormConceptoAjusteInventario.resetValues();

    // Asignamos un ID al Form
    dxFormConceptoAjusteInventario.updateData("ConceptoAjusteInventarioId", contadorRegistrosNuevos);

    // Decrementamos el contador de Registros para el siguiente nuevo registro
    contadorRegistrosNuevos -= 1;

    // Cambiamos el titulo en el modal
    modalConceptoAjusteInventario.find(".modal-title").text("Nuevo Concepto Movimiento");

    // Marcamos el modal con modo editar es false
    modalConceptoAjusteInventario.attr("isEdit", false);

    // Mostramos el modal
    modalConceptoAjusteInventario.modal('show');
}

editaModal = (event) => {
    //Obtenemos una copia del objeto a modificar
    var controlMaestroConceptoAjusteInventario = $.extend(true, {}, event.row.data);

    //Le pasamos el objeto al Form para que cargue sus valores
    dxFormConceptoAjusteInventario.option("formData", controlMaestroConceptoAjusteInventario);

    // Cambiamos el titulo en el modal
    modalConceptoAjusteInventario.find(".modal-title").text("Editar Concepto Movimiento");

    // Cambiamos el modo editar es verdad en el modal
    modalConceptoAjusteInventario.attr("isEdit", true);

    //Mostramos el modal
    modalConceptoAjusteInventario.modal('show');
}

guardaCambiosModal = () => {

    // Validamos que la informacion requerida del Formulario este completa
    if (!dxFormConceptoAjusteInventario.validate().isValid)
        return;

    // Obtenemos el Objeto que se esta creando/editando en el Form
    var controlMaestroConceptoAjusteInventario = dxFormConceptoAjusteInventario.option("formData");

    // La validación
    if (!esValidacion(controlMaestroConceptoAjusteInventario)) {
        return;
    }

    //Agregamos la propiedad para poder actualizar
    controlMaestroConceptoAjusteInventario.Registrar = true;

    // Obtenemos la instancia store del DataSource
    var store = dxDataGridConceptoAjusteInventario.getDataSource().store();

    // Si el modo editar es falso (Nuevo) o si es verdad (Editar)
    if (modalConceptoAjusteInventario.attr("isEdit") == "false") {
        // Nuevo
        store.insert(controlMaestroConceptoAjusteInventario)
            .done(function () {

                // Recargamos la informacion de la tabla
                dxDataGridConceptoAjusteInventario.getDataSource().reload();

                // Habilitamos los botones de acciones
                habilitaComponentes(true);

                // Ocultamos el modal
                modalConceptoAjusteInventario.modal('hide');
            })
            .fail(function () {
                toast("No se pudo agregar el nuevo registro a la tabla.", "error");
            });
    } else {
        // Editar
        store.update(controlMaestroConceptoAjusteInventario.ConceptoAjusteInventarioId, controlMaestroConceptoAjusteInventario)
            .done(function () {
                // Recargamos la informacion de la tabla
                dxDataGridConceptoAjusteInventario.getDataSource().reload();

                // Habilitamos los botones de acciones
                habilitaComponentes(true);

                // Ocultamos el modal
                modalConceptoAjusteInventario.modal('hide');
            })
            .fail(function () {
                toast("No se pudo actualizar el registro en la tabla.", "error");
            });
    }

}

confirmaEliminarModal = (event) => {
    // Obtenemos una copia del objeto a eliminar
    rowEliminar = $.extend(true, {}, event.row.data);

    // Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}
///////////

eliminaRow = () => {
    // Ocultamos el modal confirma eliminar
    modalConfirmaEliminar.modal('hide');

    // Obtenemos la instancia store del DataSource
    var store = dxDataGridConceptoAjusteInventario.getDataSource().store();

    // Eliminamos el registro de la tabla
    if (rowEliminar != null) {
        // Si el registro viene de la base de datos
        // para posteriormente eliminarlo en la base de datos
        if (rowEliminar.ConceptoAjusteInventarioId > 0) {
            // Actualizamos el estatus del registro a "Borrado"
            rowEliminar.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO;

            //Agregamos la propiedad para poder actualizar
            rowEliminar.Registrar = true;

            store.update(rowEliminar.ConceptoAjusteInventarioId, rowEliminar)
                .done(function () {
                    //Recargamos la informacion de la tabla
                    dxDataGridConceptoAjusteInventario.getDataSource().reload();

                    // Habilitamos los botones de acciones
                    habilitaComponentes(true);

                })
                .fail(function () {
                    toast("No se pudo actualizar el registro en la tabla.", "error");
                });
        } else {
            store.remove(rowEliminar.ConceptoAjusteInventarioId)
                .done(function () {
                    // Recargamos la informacion de la tabla
                    dxDataGridConceptoAjusteInventario.getDataSource().reload();

                    // Habilitamos los botones de acciones
                    habilitaComponentes(true);

                })
                .fail(function () {
                    toast("No se pudo eliminar el registro de la tabla.", "error");
                });
        }

        // Limpiar Row Eliminar
        rowEliminar = null;
    }
}

guardaCambios = () => {
    // Mostramos Loader
    dxLoaderPanel.show();

    var conceptos = [];

    // Obtenemos todos los registros que hay en el DataGrid
    dxDataGridConceptoAjusteInventario.getDataSource().store().load().done(response => conceptos = response);

    var listControlMaestroConceptoAjusteInventario = [];

    //Agregamos solo los conceptos que se modificaron o insertaron
    conceptos.forEach(modelo => {
        if (modelo.Registrar) {
            listControlMaestroConceptoAjusteInventario.push(modelo);
        }
    });

    $.ajax({
        type: "POST",
        url: "/almacenes/catalogos/conceptoajusteinventario/Guardar",
        data: { listControlMaestroConceptoAjusteInventario },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast(response, 'success');

            //Recargamos la ficha
            window.location.href = '/almacenes/catalogos/conceptoajusteinventario/listar';
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
    window.location.href = '/almacenes/catalogos/conceptoajusteinventario/listar';
}

// Las Validaciones //
esValidacion = (controlMaestroConceptoAjusteInventario) => {
    var listControlMaestroConceptoAjusteInventario;

    //Obtenemos todos los registros que hay en el dxDataGridConceptoAjusteInventario
    dxDataGridConceptoAjusteInventario.getDataSource().store().load().done(response => listControlMaestroConceptoAjusteInventario = response.filter(filter => filter.EstatusId == 1));

    // Tipo Movimiento and Concepto Ajuste
    if (listControlMaestroConceptoAjusteInventario.some(cai => cai.ConceptoAjusteInventarioId != controlMaestroConceptoAjusteInventario.ConceptoAjusteInventarioId && cai.TipoMovimientoId == controlMaestroConceptoAjusteInventario.TipoMovimientoId && cai.ConceptoAjuste.toUpperCase() == controlMaestroConceptoAjusteInventario.ConceptoAjuste.toUpperCase())) {
        var dxTextBoxConceptoAjuste = dxFormConceptoAjusteInventario.getEditor("ConceptoAjuste");
        dxTextBoxConceptoAjuste.option({
            validationStatus: 'invalid',
            validationError: {
                type: "custom", message: "El concepto ajuste " + controlMaestroConceptoAjusteInventario.ConceptoAjuste + " ya existe, elige nombre de concepto ajuste otro." }
        });
        //toast("El concepto ajuste " + controlMaestroConceptoAjusteInventario.ConceptoAjuste + " ya existe, elige nombre de concepto ajuste otro.", "warning");
        return false;
    }

    return true;
}
/////////////////

// Event OnValue //
onValueCodigo = (params) => {
    let conceptoAjusteInventarioId = params.ConceptoAjusteInventarioId;
    return conceptoAjusteInventarioId > 0 ? conceptoAjusteInventarioId : 'N/A';
}

// Toast //
toast = (message, type) => {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 1500);
}