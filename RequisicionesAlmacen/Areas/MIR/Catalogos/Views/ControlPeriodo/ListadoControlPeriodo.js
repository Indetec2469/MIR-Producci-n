// VARIABLES GLOBALES //
// Modal
var modalAdvertencia,
    modalConfirmaDeshacer,
    // Button
    dxButtonDeshacer,
    dxButtonGuardar,
    // DataGrid
    dxDataGridListado,
    // API
    API_FICHA = '/mir/catalogos/controlperiodo/';

var seleccionRow;
//////////////////////

// Function Default //
$(() => {
    // Inicializamos las variables para la ficha
    inicializaVariables();

    // Inhabilitamos los botones de acciones
    habilitaComponentes(false);
});
//////////////////////

inicializaVariables = () => {
    // Modal
    modalAdvertencia = $('#modalAdvertencia');
    modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    // Buton
    dxButtonDeshacer = $('#dxButtonDeshacer').dxButton('instance');
    dxButtonGuardar = $('#dxButtonGuardar').dxButton('instance');
    // DataGrid
    dxDataGridListado = $('#dxDataGridListado').dxDataGrid('instance');
}

// DataGrid //
onEditorPrepared = (event) => {
    seleccionRow = event.row;
    event.editorElement.dxSelectBox('instance').option('onValueChanged', _event => {
        if (_event.value == ControlMaestroMapeo.MIEstatusPeriodo.AUDITADO) {
            event.setValue(_event.previousValue);
            modalAdvertencia.modal('show');
            return;
        }

        event.setValue(_event.value);
        // Habilitamos los botones de acciones
        habilitaComponentes(true);

        return;
    });
}
//////////////

// OnClick //
onClickModalAdvertenciaAcepta = () => {
    // Ocultamos el modal
    modalAdvertencia.modal('hide');
    dxDataGridListado.cellValue(seleccionRow.rowIndex, 'EstatusPeriodoId', ControlMaestroMapeo.MIEstatusPeriodo.AUDITADO);
    // Habilitamos los botones de acciones
    habilitaComponentes(true);
}
/////////////

guardaCambios = () => {
    // Mostramos Loader
    dxLoaderPanel.show();
    // Obtener los datos para saber hay los datos o no hay para guardar
    const data = obtenerData();
    //console.log(data);
    if (data.listControlMaestroControlPeriodo.length == 0) {
        toast("No se puede guardar ya que no hubo cambios en la información", "error");
        // Ocultamos Loader
        dxLoaderPanel.hide();
        return;
    }

    $.ajax({
        type: 'POST',
        url: API_FICHA + 'guardar',
        data: data,
        success: function (response) {
            // Mostramos mensaje de Exito
            toast(response, 'success');

            // Redirigir a listado
            recargarFicha();
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            // Mostramos mensaje de error
            toast(response.responseText, "error");
            //toast('No se puede guardar los cambios, inténtalo de nuevo más tarde', "error");
        }
    });
}

obtenerData = () => {
    let data = {
        listControlMaestroControlPeriodo: []
    };
    // Control Maestro Control Periodo
    var listControlMaestroControlPeriodo = [];
    dxDataGridListado.getDataSource().store().load().done(response => listControlMaestroControlPeriodo = response);
    listControlMaestroControlPeriodo.map(cmcp => {
        const obtenerControlMaestroControlPeriodo = _listControlMaestroControlPeriodo.find(_cmcp => _cmcp.ControlPeriodoId == cmcp.ControlPeriodoId);
        if (cmcp.EstatusPeriodoId != obtenerControlMaestroControlPeriodo.EstatusPeriodoId) {
            actualizarControlMaestroControlPeriodo = $.extend(true, {}, cmcp);
            actualizarControlMaestroControlPeriodo.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(obtenerControlMaestroControlPeriodo.Timestamp)));
            data.listControlMaestroControlPeriodo.push(actualizarControlMaestroControlPeriodo)
        }
    })

    return data;
}

// Otro //
habilitaComponentes = (enabled) => {
    dxButtonDeshacer.option("disabled", !enabled);
    dxButtonGuardar.option("disabled", !enabled);
}

recargarFicha = () => {
    // Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA + 'listar';
}

validaHayCambios = () => {
    // Obtener los datos para saber hay los datos o no hay para guardar
    const data = obtenerData();
    //console.log(data);
    if (data.listControlMaestroControlPeriodo.length == 0) {
        recargarFicha();
    } else {
        modalConfirmaDeshacer.modal('show');
    }
}
//////////

// Toast //
toast = (message, type) => {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 3500);
}
///////////