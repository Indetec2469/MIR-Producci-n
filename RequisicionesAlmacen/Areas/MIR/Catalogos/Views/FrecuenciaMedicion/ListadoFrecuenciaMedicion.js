// VARIABLES GLOBALES //
var dxDataGridListado;

const listadoModel = {
    FrecuenciaMedicionId: null,
    Descripcion: '',
    Nivel: []
};
//////////////////////

// Function Default //
$(() => {
    // Inicializamos las variables para la ficha
    inicializaVariables();

});
//////////////////////

inicializaVariables = () => {
    dxDataGridListado = $("#dxDataGridListado").dxDataGrid("instance");
    // Cargar los datos a listado
    cargaListado();
}

cargaListado = () => {
    let listadoDataSource = [];
    _listControlMaestroFrecuenciaMedicion.map(fm => {
        // Creamos el modelo
        let nuevoListadoModel = $.extend(true, {}, listadoModel);
        nuevoListadoModel.FrecuenciaMedicionId = fm.FrecuenciaMedicionId;
        nuevoListadoModel.Descripcion = fm.Descripcion;
        // Frecuencia Medicion Nivel
        _listControlMaestroFrecuenciaMedicionNivel.filter(fmn => fmn.FrecuenciaMedicionId == fm.FrecuenciaMedicionId).map(fmn => {
            nuevoListadoModel.Nivel.push(fmn.NivelId);
        });
        // Asignamos a lista
        listadoDataSource.push(nuevoListadoModel);
    });
    // Establecer los datos a DataGrid
    var dataSource = new DevExpress.data.DataSource({
        store: {
            type: 'array',
            key: 'FrecuenciaMedicionId',
            data: listadoDataSource
        }
    });
    dxDataGridListado.option("dataSource", dataSource);
}

cellTemplate = (container, options) => {
    var noBreakSpace = "\u00A0",
        text = (options.value || []).map(element => {
            return options.column.lookup.calculateCellValue(element);
        }).join(", ");
    container.text(text || noBreakSpace).attr("title", text);
}