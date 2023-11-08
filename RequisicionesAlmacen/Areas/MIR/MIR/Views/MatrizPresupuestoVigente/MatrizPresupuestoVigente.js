

/** Constantes **/
const NIVEL_INDICADOR_COMPONENTE = 42;
const NIVEL_INDICADOR_ACTIVIDAD = 43;
const TIPO_COMPONENTE_ACTIVIDAD_PROYECTO = 54;
const TIPO_COMPONENTE_COMPONENTE_PROYECTO = 55;
const ESTATUS_PERIODO_ABIERTO = 58;

const API_FICHA = '/mir/mir/matrizpresupuestovigente/';

/** Variables Globales **/
var dxDataGridPorEjercer;
var modalConfirmaRecalcularMeses;
var modalCerrar;
var dxFormMatrizConfiguracionPresupuestal;

var controlPeriodoList;
var existenCambios = false;

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();
});

var inicializaVariables = function () {    
    dxDataGridPorEjercer = $('#dxDataGridPorEjercer').dxDataGrid("instance");
    dxFormMatrizConfiguracionPresupuestal = $("#dxFormMatrizConfiguracionPresupuestal").dxForm("instance");
    modalConfirmaRecalcularMeses = $('#modalConfirmaRecalcularMeses');
    modalCerrar = $('#modalCerrar');
    controlPeriodoList = dxFormMatrizConfiguracionPresupuestal.option('formData').ControlPeriodoList;
}

var calculateCustomSummary = function (options) {

    if (options.summaryProcess === "start") {
        options.totalValue = 0;
    }
    if (options.summaryProcess === "calculate") {

        if (options.name == 'EneroCabecera' || options.name == 'FebreroCabecera' || options.name == 'MarzoCabecera' || options.name == 'AbrilCabecera' || options.name == 'MayoCabecera' || options.name == 'JunioCabecera' || options.name == 'JulioCabecera' || options.name == 'AgostoCabecera' || options.name == 'SeptiembreCabecera' || options.name == 'OctubreCabecera' || options.name == 'NoviembreCabecera' || options.name == 'DiciembreCabecera') {
            let mes = options.name.replace('Cabecera', '');

            if (options.value[mes] == null)
                options.totalValue = null;
            else
                options.totalValue = parseFloat(Number(options.totalValue + options.value[mes]).toFixed(2));
        }

        else if (options.name == 'PorcentajeCabecera') {
            if (options.value.TipoComponenteId === TIPO_COMPONENTE_ACTIVIDAD_PROYECTO)
                options.totalValue = null;
            else
                options.totalValue = options.totalValue + options.value.Porcentaje;
        }

        else if (options.name == 'AnualCabecera') {
            //let formData = dxFormMatrizConfiguracionPresupuestal.option('formData');
            //options.totalValue = formData.MatrizPresupuestoVigente.find(f => f.MirIndicadorId === options.value.MIRIndicadorId).Anual;
            //console.log(options.value.NombreComponente);

            //options.totalValue = formData.MatrizPresupuestoVigente.find(f => f.NombreComponente === options.value.NombreComponente && f.NivelIndicadorId === NIVEL_INDICADOR_COMPONENTE).Anual;

            options.totalValue = options.totalValue + options.value.Anual;
        }


    }
    if (options.summaryProcess === "finalize") {

    }  
}

var getMesesARecalcular = function(mesaEvaluar){
    let meses = [];

    if (mesaEvaluar == 'Enero')
        meses.push({ 'Febrero': null, 'Marzo': null, 'Abril': null, 'Mayo': null, 'Junio': null, 'Julio': null, 'Agosto': null, 'Septiembre': null, 'Octubre': null, 'Noviembre': null, 'Diciembre': null });
    else if ((mesaEvaluar == 'Febrero'))
        meses.push({ 'Marzo': null, 'Abril': null, 'Mayo': null, 'Junio': null, 'Julio': null, 'Agosto': null, 'Septiembre': null, 'Octubre': null, 'Noviembre': null, 'Diciembre': null });
    else if ((mesaEvaluar == 'Marzo'))
        meses.push({ 'Abril': null, 'Mayo': null, 'Junio': null, 'Julio': null, 'Agosto': null, 'Septiembre': null, 'Octubre': null, 'Noviembre': null, 'Diciembre': null });
    else if ((mesaEvaluar == 'Abril'))
        meses.push({ 'Mayo': null, 'Junio': null, 'Julio': null, 'Agosto': null, 'Septiembre': null, 'Octubre': null, 'Noviembre': null, 'Diciembre': null });
    else if ((mesaEvaluar == 'Mayo'))
        meses.push({ 'Junio': null, 'Julio': null, 'Agosto': null, 'Septiembre': null, 'Octubre': null, 'Noviembre': null, 'Diciembre': null });
    else if ((mesaEvaluar == 'Junio'))
        meses.push({ 'Julio': null, 'Agosto': null, 'Septiembre': null, 'Octubre': null, 'Noviembre': null, 'Diciembre': null });
    else if ((mesaEvaluar == 'Julio'))
        meses.push({ 'Agosto': null, 'Septiembre': null, 'Octubre': null, 'Noviembre': null, 'Diciembre': null });
    else if ((mesaEvaluar == 'Agosto'))
        meses.push({ 'Septiembre': null, 'Octubre': null, 'Noviembre': null, 'Diciembre': null });
    else if ((mesaEvaluar == 'Septiembre'))
        meses.push({ 'Octubre': null, 'Noviembre': null, 'Diciembre': null });
    else if ((mesaEvaluar == 'Octubre'))
        meses.push({ 'Noviembre': null, 'Diciembre': null });
    else if ((mesaEvaluar == 'Noviembre'))
        meses.push({ 'Diciembre': null });

    return meses;
}

var getMesPosterior = function (mesaEvaluar) {

    if (mesaEvaluar == 'Enero')
        return 'Febrero';
    else if ((mesaEvaluar == 'Febrero'))
        return 'Marzo';
    else if ((mesaEvaluar == 'Marzo'))
        return 'Abril';
    else if ((mesaEvaluar == 'Abril'))
        return 'Mayo';
    else if ((mesaEvaluar == 'Mayo'))
        return 'Junio';
    else if ((mesaEvaluar == 'Junio'))
        return 'Julio';
    else if ((mesaEvaluar == 'Julio'))
        return 'Agosto';
    else if ((mesaEvaluar == 'Agosto'))
        return 'Septiembre';
    else if ((mesaEvaluar == 'Septiembre'))
        return 'Octubre';
    else if ((mesaEvaluar == 'Octubre'))
        return 'Noviembre';
    else if ((mesaEvaluar == 'Noviembre'))
        return 'Diciembre';
}

var getUltimoMesGuardado = function (rowData) {
    if (rowData.Febrero == null)
        return "Enero";
    else if (rowData.Marzo == null)
        return "Febrero";
    else if (rowData.Abril == null)
        return "Marzo";
    else if (rowData.Mayo == null)
        return "Abril";
    else if (rowData.Junio == null)
        return "Mayo";
    else if (rowData.Julio == null)
        return "Junio";
    else if (rowData.Agosto == null)
        return "Julio";
    else if (rowData.Septiembre == null)
        return "Agosto";
    else if (rowData.Octubre == null)
        return "Septiembre";
    else if (rowData.Noviembre == null)
        return "Octubre";
    else if (rowData.Diciembre == null)
        return "Noviembre";
    else
        return "Diciembre";
}

var cancelarRecalcularMesesModal = function () {
    modalConfirmaRecalcularMeses.modal('hide');
}

var aceptaRecalcularMesesModal = function () {

    let store = dxDataGridPorEjercer.getDataSource().store();
    let mesModificado = modalConfirmaRecalcularMeses.attr("mesModificado");
    let agrupadorId = modalConfirmaRecalcularMeses.attr("agrupadorId");
    let meses = getMesesARecalcular(mesModificado);
    let registrosActualizar;
    let validacionMesCerrado;

    //Obtenemos todos los registros que tengan el mismo agrupador
    if (agrupadorId != null) {
        store.load().done(function (dataObject) {

            //Obtenemos los registros a actualizar
            registrosActualizar = dataObject.filter(f => f.NombreComponente == agrupadorId);

            //Antes de recalcular verificamos que los meses no se encuentren cerrados
            meses.forEach(function (listadoMeses) {
                let listado = Object.keys(listadoMeses);
                listado.forEach(function (mes) {
                    if (registrosActualizar[0][mes] != null && controlPeriodoList.find(f => f.Periodo == mes).EstatusPeriodoId !== ESTATUS_PERIODO_ABIERTO ) {
                        //Ocultamos el modal
                        modalConfirmaRecalcularMeses.modal('hide');
                        //Mostramos mensaje
                        toast("El mes de " + mes + " que intenta recalcular no se encuentra abierto", "warning");
                        validacionMesCerrado = true;
                        return;
                    }
                });
            });

            if (validacionMesCerrado) return;

            for (let i = 0; i < registrosActualizar.length; i++) {                

                store.update(registrosActualizar[i].MIRIndicadorId, meses[0])
                     .done(function () {
                         store.byKey(registrosActualizar[i].MIRIndicadorId).done(function (dataItem) {

                         //Sumamos todos los meses del ROW y actualizamos la columna de ASIGNADO
                         let asignado = Number(dataItem.Enero) + Number(dataItem.Febrero) + Number(dataItem.Marzo) + Number(dataItem.Abril) + Number(dataItem.Mayo) + Number(dataItem.Junio) +
                             Number(dataItem.Julio) + Number(dataItem.Agosto) + Number(dataItem.Septiembre) + Number(dataItem.Octubre) + Number(dataItem.Noviembre) + Number(dataItem.Diciembre);

                         asignado = parseFloat(Number(asignado).toFixed(2));

                        store.update(registrosActualizar[i].MIRIndicadorId, { Asignado: asignado })
                        .done(function () {
                            //Recargamos la informacion de la tabla
                            dxDataGridPorEjercer.getDataSource().reload();
                        })
                        .fail(function () {
                            toast("No se pudo actualizar el registro en la tabla.", "error");
                        }
                        );

                         });
                     })
                     .fail(function () {
                        toast("No se pudo actualizar el registro en la tabla.", "error");
                     });
            }
        });
    }

    //Ocultamos el modal
    modalConfirmaRecalcularMeses.modal('hide');
}

var guardaCambios = function () {

    let formData;
    let datosActividades;
    let datosComponentes;
    let index;

    //Obtenemos todos las actividades que hay en el dxGridProspectosProveedor
    dxDataGridPorEjercer.getDataSource().store().load().done((res) => {

        datosActividades = res;

        //Obtenemos todos los componentes que hay en el dxGridProspectosProveedor 
        dxDataGridPorEjercer.getDataSource().load().done((data) => {
            datosComponentes = data; 

            //Verificamos que el mes que se intenta guardar no se encuentre cerrado
            let ultimoMesGuardado;
            let validacionMesCerrado;
            datosActividades.forEach(function (item) {
                ultimoMesGuardado = getUltimoMesGuardado(item);
                if (ultimoMesGuardado != 'Diciembre' && controlPeriodoList.find(f => f.Periodo == ultimoMesGuardado).EstatusPeriodoId !== ESTATUS_PERIODO_ABIERTO) {
                    toast("El mes de " + ultimoMesGuardado + " que intenta guardar no se encuentra abierto", "warning");
                    validacionMesCerrado = true;
                    return;
                }
            });

            //Si el mes se encuentra cerrado, salimos sin guardar
            if (validacionMesCerrado) return;

            //Obtenemos los datos del form
            formData = dxFormMatrizConfiguracionPresupuestal.option('formData');
            
            //Actualizamos los componentes del FormData
            datosComponentes.forEach(function (componente) {          

                let index = formData.MatrizPresupuestoVigenteH.findIndex(f => f.NombreComponente == componente.key && f.NivelIndicadorId == NIVEL_INDICADOR_COMPONENTE);
                
                formData.MatrizPresupuestoVigenteH[index].Enero = componente.aggregates[0];
                formData.MatrizPresupuestoVigenteH[index].Febrero = componente.aggregates[1];
                formData.MatrizPresupuestoVigenteH[index].Marzo = componente.aggregates[2];
                formData.MatrizPresupuestoVigenteH[index].Abril = componente.aggregates[3];
                formData.MatrizPresupuestoVigenteH[index].Mayo = componente.aggregates[4];
                formData.MatrizPresupuestoVigenteH[index].Junio = componente.aggregates[5];
                formData.MatrizPresupuestoVigenteH[index].Julio = componente.aggregates[6];
                formData.MatrizPresupuestoVigenteH[index].Agosto = componente.aggregates[7];
                formData.MatrizPresupuestoVigenteH[index].Septiembre = componente.aggregates[8];
                formData.MatrizPresupuestoVigenteH[index].Octubre = componente.aggregates[9];
                formData.MatrizPresupuestoVigenteH[index].Noviembre = componente.aggregates[10];
                formData.MatrizPresupuestoVigenteH[index].Diciembre = componente.aggregates[11];
            });

            //Actualizamos las Actividades del FormData
            datosActividades.forEach(function (item) {
                index = formData.MatrizPresupuestoVigenteH.findIndex(i => i.MIRIndicadorId == item.MIRIndicadorId);
                formData.MatrizPresupuestoVigenteH[index] = item;
            });
            
            //Mostramos Loader
            dxLoaderPanel.show();
            var fechaValue = $('#dtFecha').dxDateBox('instance').option("value");
            var fechaFormateada = fechaValue.toLocaleString();
            formData.FechaModificacion = fechaFormateada;
            //Enviamos la informacion al controlador
            $.ajax({
                type: "POST",
                url: API_FICHA + "guardar",
                data: { matrizPresupuestoVigenteViewModel: formData},
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

    else if (event.rowType == 'data' && (event.column.dataField == 'Anual' || event.column.dataField == 'Porcentaje' || event.column.caption == 'Asignado') && event.cellElement) {
        // Establecer color a celda
        event.cellElement.addClass('pixvs-datagrid-cell');
        if (event.columnIndex != 0 || event.columnIndex != 1) {
            // Establecer border a celda
            event.cellElement.addClass('pixvs-datagrid-cell-border');
        }
    }

    if (event.rowType == 'data' && event.column.dataField == 'Asignado') {

        let asignado = event.data.Asignado;
        let anual = event.data.Anual;

        if (asignado != anual) {
            event.cellElement.removeClass('pixvs-datagrid-cell');
            event.cellElement.addClass('pixvs-datagrid-cell-money');
        }
        else {
            event.cellElement.removeClass('pixvs-datagrid-cell-money');
            event.cellElement.addClass('pixvs-datagrid-cell');
        }
    }
    //if (event.rowType == 'group') {
    //    if (event.data.items[0][event.column.dataField] == null)
    //        console.log(event.cellElement);
    //}
} 

var onRowUpdating = function (event) {
    let mesModificado = Object.keys(event.newData)[0];
    let store = dxDataGridPorEjercer.getDataSource().store();

    //Verificamos que el mes siguiente del mes que se esta editando no tenga informacion, si la hay, advertimos al usuario
    if (mesModificado !== 'Diciembre') {
        store.byKey(event.key).done(function (data) {
            let mesPosterior = getMesPosterior(mesModificado);
            //Si el valor del mes posterior es diferente de NULL, preguntamos si desea recalcular los meses
            if (data[mesPosterior] != null) {
                //Cancelamos y regresamos el valor anterior de la celda
                event.cancel = true;
                dxDataGridPorEjercer.cancelEditData();

                //Mandamos llamar el modal para confirmar si se recalcula o no la informacion
                modalConfirmaRecalcularMeses.attr("mesModificado", mesModificado);
                modalConfirmaRecalcularMeses.attr("agrupadorId", data.NombreComponente);
                modalConfirmaRecalcularMeses.modal('show');
                return;
            }
        });
    }

    //Si se modifica cualquier mes solo validamos que no haya meses anteriores sin guardar
    store.byKey(event.key).done(function (data) {
        //Si es nulo, quiere decir que primero tiene que guardar el utimo mes con datos
        if (data[mesModificado] == null) {
            let ultimoMesGuardado = getUltimoMesGuardado(data);
            //Cancelamos y regresamos el valor anterior de la celda
            event.cancel = true;
            dxDataGridPorEjercer.cancelEditData();
            toast("Debe guardar primero el mes de " + ultimoMesGuardado + " para poder llenar los meses posteriores", "warning");
        }
        //Si no, entonces verificamos que el mes que se intenta guardar no este cerrado
        else if (controlPeriodoList.find(f => f.Periodo == mesModificado).EstatusPeriodoId !== ESTATUS_PERIODO_ABIERTO) {
            //Cancelamos y regresamos el valor anterior de la celda
            event.cancel = true;
            dxDataGridPorEjercer.cancelEditData();
            toast("El mes de " + mesModificado + " que intenta modificar no se encuentra abierto", "warning");
        }
    });
}

var onRowUpdated = function (event) {

    //Sumamos todos los meses dle ROW y actualizamos la columna de ASIGNADO
    let asignado = Number(event.data.Enero) + Number(event.data.Febrero) + Number(event.data.Marzo) + Number(event.data.Abril) + Number(event.data.Mayo) + Number(event.data.Junio) +
        Number(event.data.Julio) + Number(event.data.Agosto) + Number(event.data.Septiembre) + Number(event.data.Octubre) + Number(event.data.Noviembre) + Number(event.data.Diciembre);

    asignado = parseFloat(Number(asignado).toFixed(2));

    let store = dxDataGridPorEjercer.getDataSource().store();

    store.update(event.key, { Asignado: asignado })
        .done(function () {
            //Recargamos la informacion de la tabla
            dxDataGridPorEjercer.getDataSource().reload();
            //Marcamos que hay cambios por guardar
            existenCambios = true;
        })
        .fail(function () {
            toast("No se pudo actualizar el registro en la tabla.", "error");
        }
        );
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