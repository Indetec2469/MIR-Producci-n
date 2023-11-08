redirigirEditar = (event) => {
    dxLoaderPanel.show();
    window.location.href = '/mir/mir/matrizconfiguracionpresupuestalseguimientovariable/editar/' + event.row.data.MIRId;
}