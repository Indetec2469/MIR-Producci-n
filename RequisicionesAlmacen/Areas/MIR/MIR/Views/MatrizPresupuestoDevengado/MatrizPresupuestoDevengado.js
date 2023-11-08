/** Constantes **/
const NIVEL_INDICADOR_COMPONENTE = 42;
const NIVEL_INDICADOR_ACTIVIDAD = 43;
const TIPO_COMPONENTE_ACTIVIDAD_PROYECTO = 54;
const TIPO_COMPONENTE_COMPONENTE_PROYECTO = 55;
const ESTATUS_PERIODO_ABIERTO = 58;

const API_FICHA = '/mir/mir/matrizpresupuestodevengado/';

/** Variables Globales **/
var dxDataGridDevengado;
var modalCerrar;
var dxFormMatrizConfiguracionPresupuestal;

var controlPeriodoList;
var existenCambios = false;


$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();
});

var inicializaVariables = function () {
    dxDataGridDevengado = $('#dxDataGridDevengado').dxDataGrid("instance");
    dxFormMatrizConfiguracionPresupuestal = $("#dxFormMatrizConfiguracionPresupuestal").dxForm("instance");
    modalCerrar = $('#modalCerrar');
    controlPeriodoList = dxFormMatrizConfiguracionPresupuestal.option('formData').ControlPeriodoList;
}

var calculateCustomSummary = function (options) {

    if (options.summaryProcess === "start") { options.totalValue = 0; }

    if (options.summaryProcess === "calculate") {

        if (options.name == 'EneroCabecera' || options.name == 'FebreroCabecera' || options.name == 'MarzoCabecera' || options.name == 'AbrilCabecera' || options.name == 'MayoCabecera' || options.name == 'JunioCabecera' || options.name == 'JulioCabecera' || options.name == 'AgostoCabecera' || options.name == 'SeptiembreCabecera' || options.name == 'OctubreCabecera' || options.name == 'NoviembreCabecera' || options.name == 'DiciembreCabecera') {
            let formData = dxFormMatrizConfiguracionPresupuestal.option('formData');
            let mes = options.name.replace('Cabecera', '');
            options.totalValue = formData.MatrizPresupuestoDevengadoH.find(f => f.NombreComponente === options.value.NombreComponente && f.NivelIndicadorId === NIVEL_INDICADOR_COMPONENTE)[mes];
        }

        else if (options.name == 'EneroDiferencia' || options.name == 'FebreroDiferencia' || options.name == 'MarzoDiferencia' || options.name == 'AbrilDiferencia' || options.name == 'MayoDiferencia' || options.name == 'JunioDiferencia' || options.name == 'JulioDiferencia' || options.name == 'AgostoDiferencia' || options.name == 'SeptiembreDiferencia' || options.name == 'OctubreDiferencia' || options.name == 'NoviembreDiferencia' || options.name == 'DiciembreDiferencia' || options.name == 'AnualDiferencia') {
            let formData = dxFormMatrizConfiguracionPresupuestal.option('formData');
            let mes = options.name.replace('Diferencia', '');
            let totalCabecera = formData.MatrizPresupuestoDevengadoH.find(f => f.NombreComponente === options.value.NombreComponente && f.NivelIndicadorId === NIVEL_INDICADOR_COMPONENTE)[mes];
            let actividades = formData.MatrizPresupuestoDevengadoH.filter(f => f.NombreComponente === options.value.NombreComponente && f.NivelIndicadorId === NIVEL_INDICADOR_ACTIVIDAD);
            let indice = actividades.findIndex(f => f.MIRIndicadorId === options.value.MIRIndicadorId);

            options.totalValue = parseFloat(Number(options.totalValue + options.value[mes]).toFixed(2));

            // si es el la ultima actividad le restamos el total de la cabecera
            if (indice == (actividades.length - 1)) {
                options.totalValue = parseFloat(Number(totalCabecera - options.totalValue).toFixed(2));
            }
        }

        else if (options.name == 'PorcentajeCabecera') {
            if (options.value.TipoComponenteId === TIPO_COMPONENTE_ACTIVIDAD_PROYECTO)
                options.totalValue = null;
            else
                options.totalValue +=  options.value.Porcentaje;
        }

        else if (options.name == 'AnualCabecera') {
            let formData = dxFormMatrizConfiguracionPresupuestal.option('formData');
            options.totalValue = formData.MatrizPresupuestoDevengadoH.find(f => f.NombreComponente === options.value.NombreComponente && f.NivelIndicadorId === NIVEL_INDICADOR_COMPONENTE).Anual;
        }
    }
    if (options.summaryProcess === "finalize") {        

    }
}

var guardaCambios = function () {

    let formData = dxFormMatrizConfiguracionPresupuestal.option('formData');
    let datosActividades;
    let datosComponentes;
    let index;
    let formularioValido = true;
    

    //Obtenemos todos las actividades que hay en el dxDataGridDevengado
    dxDataGridDevengado.getDataSource().store().load().done((res) => {

        datosActividades = res;

        //Obtenemos todos los componentes que hay en el dxDataGridDevengado
        dxDataGridDevengado.getDataSource().load().done((data) => {
            datosComponentes = data;  

            //Verificamos que haya diferencias en ninguno de los componentes
            datosComponentes.forEach(function (item) {
                //console.log(formData.MatrizPresupuestoDevengado.find(f => f.NombreComponente == item.key && f.NivelIndicadorId == NIVEL_INDICADOR_COMPONENTE));
                let totalCabecera = formData.MatrizPresupuestoDevengadoH.find(f => f.NombreComponente == item.key && f.NivelIndicadorId == NIVEL_INDICADOR_COMPONENTE).Anual;
                let totalActividades = datosActividades.filter(f => f.NombreComponente == item.key && f.NivelIndicadorId == NIVEL_INDICADOR_ACTIVIDAD).map(m => m.Anual).reduce((a, b) => a + b, 0);
                totalActividades = parseFloat(Number(totalActividades).toFixed(2));

                if (totalCabecera - totalActividades != 0) {
                    formularioValido = false;
                    toast("Existen diferencias en el Componente " + item.key + ". Favor de revisar.", "warning");
                    return;
                }
            });         

            if (!formularioValido) return;

            //Obtenemos los datos del form
            formData = dxFormMatrizConfiguracionPresupuestal.option('formData');

            //Actualizamos las Actividades del FormData
            datosActividades.forEach(function (item) {
                index = formData.MatrizPresupuestoDevengadoH.findIndex(i => i.MIRIndicadorId == item.MIRIndicadorId);
                formData.MatrizPresupuestoDevengadoH[index] = item;
            });
            var fechaValue = $('#dtFecha').dxDateBox('instance').option("value");
            var fechaFormateada = fechaValue.toLocaleString();
            formData.FechaModificacion = fechaFormateada;

            //Mostramos Loader
            dxLoaderPanel.show();

            //Enviamos la informacion al controlador
            $.ajax({
                type: "POST",
                url: API_FICHA + "guardar",
                data: { MatrizPresupuestoDevengadoViewModel: formData },
                success: function () {
                    //Mostramos mensaje de Exito
                    toast("Registros guardados con exito!", 'success');

                    //Recargamos la ficha
                    window.location.href = API_FICHA + 'editar/' + formData.MIRId;
                },
                error: function (response, status, error) {
                    //Ocultamos Loader
                    dxLoaderPanel.hide();

                    //Mostramos mensaje de error
                    toast("Error al guadar:\n" + response.responseText, 'error');
                }
            });
        });
    });
}

var onCellPrepared = function (event) {

    if (event.rowType == "header") {
        event.cellElement.css("text-align", "center");
    }

    else if (event.rowType == 'data' && (event.column.dataField == 'Anual' || event.column.dataField == 'Porcentaje' ) && event.cellElement) {
        // Establecer color a celda
        event.cellElement.addClass('pixvs-datagrid-cell');
        if (event.columnIndex != 0 || event.columnIndex != 1) {
            // Establecer border a celda
            event.cellElement.addClass('pixvs-datagrid-cell-border');
        }
    }

    else if (event.rowType == 'groupFooter') {
        // Establecer color a celda
        event.cellElement.addClass('pixvs-datagrid-cell');
        // Establecer border a celda
        event.cellElement.addClass('pixvs-datagrid-cell-border');        
    }

    if (event.rowType == 'data' && (event.column.dataField == 'Enero' || event.column.dataField == 'Febrero' || event.column.dataField == 'Marzo' || event.column.dataField == 'Abril' || event.column.dataField == 'Mayo' || event.column.dataField == 'Junio' || event.column.dataField == 'Julio' || event.column.dataField == 'Agosto' || event.column.dataField == 'Septiembre' || event.column.dataField == 'Octubre' || event.column.dataField == 'Noviembre' || event.column.dataField == 'Diciembre') && event.cellElement) {
        if (event.data.ConfiguracionPresupuestoDetalleId > 0) {
            let formData = dxFormMatrizConfiguracionPresupuestal.option('formData');
            if (formData.MatrizPresupuestoDevengadoRespaldo.find(f => f.MIRIndicadorId == event.data.MIRIndicadorId)[event.column.dataField] - event.data[event.column.dataField] != 0) 
                event.cellElement.addClass('pixvs-datagrid-cell-save');
        } else {
            event.cellElement.addClass('pixvs-datagrid-cell-save');
        }
    }

    if (event.rowType == 'groupFooter' && (event.column.dataField == 'Enero' || event.column.dataField == 'Febrero' || event.column.dataField == 'Marzo' || event.column.dataField == 'Abril' || event.column.dataField == 'Mayo' || event.column.dataField == 'Junio' || event.column.dataField == 'Julio' || event.column.dataField == 'Agosto' || event.column.dataField == 'Septiembre' || event.column.dataField == 'Octubre' || event.column.dataField == 'Noviembre' || event.column.dataField == 'Diciembre' || event.column.dataField == 'Anual')) {
        let formData = dxFormMatrizConfiguracionPresupuestal.option('formData');
        let totalCabecera = formData.MatrizPresupuestoDevengadoH.find(f => f.NombreComponente == event.key && f.NivelIndicadorId == NIVEL_INDICADOR_COMPONENTE)[event.column.dataField];
        let totalActividades = event.data.items.map(m => m[event.column.dataField]).reduce((a, b) => a + b, 0);
        totalActividades = parseFloat(Number(totalActividades).toFixed(2));

        if (totalCabecera - totalActividades != 0) {
            event.cellElement.removeClass('pixvs-datagrid-cell');
            event.cellElement.addClass('pixvs-datagrid-cell-money');
        }
        else {
            event.cellElement.removeClass('pixvs-datagrid-cell-money');
            event.cellElement.addClass('pixvs-datagrid-cell');
        }
    }
}

var onRowUpdating = function (event) {
    let mesModificado = Object.keys(event.newData)[0];
    
    //Verificamos que el mes de la celda que se esta editando se encuentre Abierto
    if (controlPeriodoList.find(f => f.Periodo == mesModificado).EstatusPeriodoId !== ESTATUS_PERIODO_ABIERTO) {
        //Cancelamos y regresamos el valor anterior de la celda
        event.cancel = true;
        dxDataGridDevengado.cancelEditData();
        toast("El mes de " + mesModificado + " que intenta modificar no se encuentra abierto", "warning");
    } 
}

var onRowUpdated = function (event) {

    //Sumamos todos los meses del ROW y actualizamos la columna de Anual
    let store = dxDataGridDevengado.getDataSource().store();
    let anual = Number(event.data.Enero) + Number(event.data.Febrero) + Number(event.data.Marzo) + Number(event.data.Abril) + Number(event.data.Mayo) + Number(event.data.Junio) +
        Number(event.data.Julio) + Number(event.data.Agosto) + Number(event.data.Septiembre) + Number(event.data.Octubre) + Number(event.data.Noviembre) + Number(event.data.Diciembre);

    anual = parseFloat(Number(anual).toFixed(2));    

    store.update(event.key, { Anual: anual})
        .done(function () {
            //Recargamos la informacion de la tabla
            dxDataGridDevengado.getDataSource().reload();
            //Marcamos que hay cambios por guardar
            existenCambios = true;
        })
        .fail(function () {
            toast("No se pudo actualizar el registro en la tabla.", "error");
        }
    );
}

var onRowPrepared = function (event) {
    if (event.rowType == "data") {
        if (event.data.TipoComponenteId == TIPO_COMPONENTE_ACTIVIDAD_PROYECTO && event.data.Porcentaje == 100.00) 
            event.cells.forEach(function (item) { if (item.cellElement != null) item.cellElement.addClass('pixvs-datagrid-cell'); });
    }
} 

var onEditingStart = function (event) {
    console.log(event);
    if (event.data.TipoComponenteId == TIPO_COMPONENTE_ACTIVIDAD_PROYECTO && event.data.Porcentaje == 100.00) {
        event.cancel = true;
    }
    ////Verificamos que el mes de la celda que se esta editando se encuentre Abierto
    //if (controlPeriodoList.find(f => f.Periodo == event.column.dataField).EstatusPeriodoId !== ESTATUS_PERIODO_ABIERTO) {
    //    //Cancelamos y regresamos el valor anterior de la celda
    //    event.cancel = true;
    //    toast("El mes de " + event.column.dataField + " que intenta modificar no se encuentra abierto", "warning");
    //}
}


validaHayCambios = () => {
    if (existenCambios)
        modalCerrar.modal('show');
    else
        window.location.href = API_FICHA + "listar";
}

// Toast //
toast = (message, type) => {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 3500);
}