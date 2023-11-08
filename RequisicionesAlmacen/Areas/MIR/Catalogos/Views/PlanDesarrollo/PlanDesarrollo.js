// VARIABLES GLOBALES //
var contadorRegistrosNuevos,
    // DropDownBox
    dxDropDownBoxEstructuraPadreId,
    //Modales
    modalPNDE,
    modalCerrar,
    modalConfirmaEliminar,
    modalConfirmaEliminarRegistro,
    //Botones
    dxButtonCancelar,
    dxButtonEliminar,
    dxButtonGuardar,
    // Form
    dxForm,
    dxFormModal,
    // TreeList
    dxGridDetalles,
    dxTreeListPNDE,
    rowEliminar,
    API_FICHA = "/mir/catalogos/plandesarrollo/",
    // Variables
    esPadreEtiqueta = false;

var dxTxtFechaInicio;
var dxTxtFechaFin;

//////////////////////
// Function Default //
$(() => {
    //Inicializamos las variables para la Ficha
    inicializaVariables();
    //Deshabilitamos los botones de acciones
    habilitaComponentes(false);
});
//////////////////////
inicializaVariables = () => {
    contadorRegistrosNuevos = -1;
    // Modal
    modalPNDE = $('#modalPNDE');
    modalCerrar = $('#modalCerrar');
    modalConfirmaEliminar = $('#modalConfirmaEliminar');
    modalConfirmaEliminarRegistro = $('#modalConfirmaEliminarRegistro');
    // DropDownBox
    dxDropDownBoxEstructuraPadreId = $('#dxDropDownBoxEstructuraPadreId').dxDropDownBox("instance");
    // Button
    dxButtonCancelar = $('#dxButtonCancelar').dxButton("instance");
    dxButtonEliminar = $('#dxButtonEliminar').dxButton("instance");
    dxButtonGuardar = $('#dxButtonGuardar').dxButton("instance");    
    // Form
    dxForm = $("#dxForm").dxForm("instance");
    dxFormModal = $("#dxFormModal").dxForm("instance");
    dxGridDetalles = $('#dxGridDetalles').dxTreeList("instance");
    dxTreeListPNDE = $('#dxTreeListPNDE').dxTreeList("instance");

    dxTxtFechaInicio = $('#dxTxtFechaInicio').dxDateBox("instance");
    dxTxtFechaFin = $('#dxTxtFechaFin').dxDateBox("instance");

    //Asignamos el límite en los campos de Fecha
    //var minDate = _ejercicio + "-01-01";
    //var maxDate = _ejercicio + "-12-31";

    //dxTxtFechaInicio.option("min", minDate);
    //dxTxtFechaInicio.option("max", maxDate);
    //dxTxtFechaFin.option("min", minDate);
    //dxTxtFechaFin.option("max", maxDate);

    // Configurar las fechas y convertir para que no tendra problema con las fechas que viene controller (JSON)
    configurarFechas();
}

configurarFechas = () => {
    _planDesarrollo.FechaInicio = functionConvertDateStringToDate(_planDesarrollo.FechaInicio);
    _planDesarrollo.FechaFin = functionConvertDateStringToDate(_planDesarrollo.FechaFin);
}

habilitaComponentes = (enabled) => {
    dxButtonEliminar.option("visible", _planDesarrollo.PlanDesarrolloId > 0);
    dxButtonGuardar.option("disabled", !enabled);
}

nuevoRegistro = () => {
    //Actualizamos el Combo EstructuraPadre
    agregaTablaEstructura();
    // Inicializamos los campos del Form
    dxFormModal.resetValues();
    // Asignamos
    dxFormModal.updateData('PlanDesarrolloEstructuraId', contadorRegistrosNuevos);
    dxFormModal.updateData('PlanDesarrolloId', dxForm.option('formData').PlanDesarrolloId);
    dxFormModal.updateData('EstatusId', ControlMaestroMapeo.EstatusRegistro.ACTIVO);
    // Habilitar el campo Etiqueta
    dxFormModal.getEditor('Etiqueta').option({ disabled: false });
    // Habilitar el combo de Estructura Padre
    dxDropDownBoxEstructuraPadreId.option("readOnly", false);
    // Si la etiqueta de los nodos hermanos
    var listPNDE = [];
    // Obtenemos todos los registros que hay en el dxGridDetalles
    dxTreeListPNDE.getDataSource().store().load().done((response) => { listPNDE = response.filter(pnde => pnde.EstructuraPadreId == null && pnde.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO); });
    // Establecemos los registros con el ID de Padre
    const orden = listPNDE.filter(pnde => pnde.PlanDesarrolloEstructuraId != contadorRegistrosNuevos).length;
    if (orden != 0) {
        // Asignamos el campo Orden
        dxFormModal.updateData('Orden', orden);
        // Habilitar el campo Etiqueta
        dxFormModal.getEditor('Etiqueta').option({ disabled: true, value: listPNDE[0].Etiqueta });
        // Cambiamos el valor
        esPadreEtiqueta = true;
    } else {
        esPadreEtiqueta = false;
    }

    // Decrementamos el contador de Registros para el siguiente nuevo registro
    contadorRegistrosNuevos -= 1;
    // Marcamos el modo editar es false para el nuevo
    modalPNDE.attr("isEdit", false);
    //Mostramos el modal
    modalPNDE.modal('show');
}

editaRegistro = (event) => {
    //Actualizamos el Combo EstructuraPadre
    agregaTablaEstructura();
    //Obtenemos una copia del objeto a modificar
    var pnde = $.extend(true, {}, event.row.data);
    // Reniciar o limpiar el fomrulario
    dxFormModal.resetValues();
    //Le pasamos el objeto al Form para que cargue sus valores
    dxFormModal.option("formData", pnde);
    // Deshabilitar el combo de Estructura Padre 
    //dxDropDownBoxEstructuraPadreId.option("readOnly", true);
    if (pnde.EstructuraPadreId == null) {
        // Si la etiqueta de los nodos hermanos
        var listaPDE = [];
        // Obtenemos todos los registros que hay en el dxGridDetalles
        dxTreeListPNDE.getDataSource().store().load().done((response) => { listaPDE = response.filter(_pnde => _pnde.EstructuraPadreId == null && _pnde.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO); });
        if (listaPDE.some(pde => pde.PlanDesarrolloEstructuraId == pnde.PlanDesarrolloEstructuraId && pde.Orden == 0 && pde.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
            // Habilitar el campo Etiqueta
            dxFormModal.getEditor('Etiqueta').option({ disabled: false });
            // Cambiamos el valor
            esPadreEtiqueta = false;
        } else {
            // Deshabilitar el campo Etiqueta
            dxFormModal.getEditor('Etiqueta').option({ disabled: true });
            // Cambiamos el valor
            esPadreEtiqueta = true;
        }
    }
    // Marcamos el modo editar es true para la edita
    modalPNDE.attr("isEdit", true);
    //Mostramos el modal
    modalPNDE.modal('show');
    //Habilitamos el campo Etiqueta
    //dxTxtEtiqueta.option("readOnly", false);
}

agregaTablaEstructura = (event) => {
    let listPNDE = [];
    //Obtenemos todos los registros que hay en el TreeList
    dxTreeListPNDE.getDataSource().store().load().done((res) => { listPNDE = res; });

    dxDropDownBoxEstructuraPadreId.option("value", null);
    dxDropDownBoxEstructuraPadreId.option("dataSource", listPNDE);
    dxDropDownBoxEstructuraPadreId.option("contentTemplate", null);
    dxDropDownBoxEstructuraPadreId.option("contentTemplate", $("#templateTreeListModalPNDE"));
}
// Form //
onFieldDataChanged = (event) => {
    if (event.dataField == 'NombrePlan' || event.dataField == 'TipoPlanId' || event.dataField == 'FechaInicio' || event.dataField == 'FechaFin') {
        habilitaComponentes(true);
    }
}
//////////
// TreeList //
gridBox_displayExpr = (item) => {
    return item ? item.Nombre : null;
}

onValueChangedPNDE = (event, campo) => {
    if (event.value) {
        var listPNDE = [],
            pnde = dxFormModal.option('formData');
        // Obtenemos todos los registros que hay en el TreeList
        dxTreeListPNDE.getDataSource().store().load().done((res) => { listPNDE = res; });
        // Nombre
        if (campo == 'Nombre') {
            if (listPNDE.some(_pnde => _pnde.PlanDesarrolloEstructuraId != pnde.PlanDesarrolloEstructuraId && _pnde.Nombre.toLowerCase() == event.value.toLowerCase() && _pnde.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
                event.component.option({
                    validationStatus: 'invalid',
                    validationError: {
                        type: 'custom',
                        message: 'Ya existe un registro con el mismo Nombre. Favor de verificar.'
                    }
                });
                return;
            }
        }
    }
}
//////////////

//var existeNombre = function (estructuraId, nombre) {
//    var detalles;

//    //Obtenemos todos los registros que hay en el dxGridDetalles
//    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

//    //Buscamos un registro duplicado
//    var existe = detalles.find(x => x.PlanDesarrolloEstructuraId !== estructuraId && x.Nombre.toLowerCase() === nombre.toLowerCase());

//    //Retornamos <true> si se encontro un registro , <false> de lo contrario
//    return existe;
//}

//var existeEtiqueta = function (estructuraId, etiqueta) {
//    var detalles;

//    //Obtenemos todos los registros que hay en el dxGridDetalles
//    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

//    //Obtenemos el listado ordenado
//    var listadoOrdenado = getListadoOrdenado(detalles);

//    //Buscamos la estructura
//    var estructura = listadoOrdenado.find(x => x.PlanDesarrolloEstructuraId == estructuraId);

//    var existe = null;

//    if (estructura) {
//        //Buscamos un registro duplicado
//        existe = listadoOrdenado.find(x => x.NivelEstructura !== estructura.NivelEstructura && x.Etiqueta.toLowerCase() === etiqueta.toLowerCase());
//    }

//    //Retornamos <true> si se encontro un registro , <false> de lo contrario
//    return existe;
//}

validaHayCambiosModal = () => {
    var pnde = dxFormModal.option('formData');
    if (pnde.EstructuraPadreId != null || pnde.Nombre != '' || pnde.Etiqueta != '') {
        modalCerrar.modal('show');
    } else {
        modalPNDE.modal('hide');
    }
}

validaHayDosIguales = (pde) => {
    var listaPDE = [], existe = false;
    // Obtenemos todos los registros que hay en el TreeList
    dxTreeListPNDE.getDataSource().store().load().done((response) => { listaPDE = response.filter(_pde => _pde.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO); });
    // Buscamos un registro duplicado (Nombre)
    existe = listaPDE.some(_pde => _pde.PlanDesarrolloEstructuraId != pde.PlanDesarrolloEstructuraId && _pde.Nombre.toLowerCase() == pde.Nombre.toLowerCase());
    if (existe) {
        toast("Ya existe un registro con el mismo Nombre. Favor de verificar.", 'warning');
        // Enfocamos el campo
        dxFormModal.getEditor('Nombre').focus();
        return false;
    }
    // Buscamos un registro duplicado (Etiqueta)
    if (!esPadreEtiqueta) {
        var __planDesarrolloEstructura = listaPDE.find(_pde => _pde.PlanDesarrolloEstructuraId == pde.PlanDesarrolloEstructuraId);
        if (__planDesarrolloEstructura) {
            listaPDE = listaPDE.filter(_pde => _pde.Etiqueta.toLowerCase() != __planDesarrolloEstructura.Etiqueta.toLowerCase());
        }

        existe = listaPDE.some(_pde => _pde.PlanDesarrolloEstructuraId != pde.PlanDesarrolloEstructuraId && _pde.Etiqueta.toLowerCase() == pde.Etiqueta.toLowerCase());
        if (existe) {
            toast("Ya existe un nivel primero con la misma Etiqueta. Favor de verificar.", 'warning')
            // Enfocamos el campo
            dxFormModal.getEditor('Etiqueta').focus();
            return false;
        }
    }
        
    // Retornamos <true> si se encontro un registro , <false> de lo contrario
    return true;
}

guardaCambiosModal = () => {
    // Validamos que la informacion requerida del Formulario este completa
    if (!dxFormModal.validate().isValid) {
        toast("Favor de completar los campos requeridos.", 'error');
        return;
    }
    // Obtenemos el Objeto que se esta creando/editando en el Form 
    var pde = dxFormModal.option("formData");
    // Validamos que el Campo de Nombre o Etiqueta no se repita
    if (!validaHayDosIguales(pde))
        return
    // Obtenemos la instancia store del DataSource
    var store = dxTreeListPNDE.getDataSource().store();
    // Si es un registro nuevo (isEdit es false), lo insertamos en el DataSource
    if (modalPNDE.attr('isEdit') == 'false') {
        store.insert(pde)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxTreeListPNDE.getDataSource().reload();
                // Habilitamos los botones de acciones
                habilitaComponentes(true);
                //Ocultamos el modal
                modalPNDE.modal('hide');
            })
            .fail(function () {
                toast("No se pudo agregar el nuevo registro a la tabla.", "error");
            });
    }
    //Si es un registro que se esta editando, actualizamos su informacion en el DataSource
    else {
        // Si es por editable y el orden es 0 para cambiar el etiqueta o no
        if (pde.Orden == 0) {
            var listaPDE = [],
                __planDesarrolloEstructura = null;
            // Obtenemos todos los registros que hay en el dxGridDetalles
            //dxTreeListPNDE.getDataSource().store().load().done((response) => { listaPDE = response.filter(_pnde => _pnde.Etiqueta == pnde.Etiqueta && _pnde.PlanDesarrolloEstructuraId != pnde.PlanDesarrolloEstructuraId && _pnde.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO); });
            dxTreeListPNDE.getDataSource().store().load().done((response) => { listaPDE = response.filter(_pde => _pde.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO); });
            __planDesarrolloEstructura = listaPDE.find(_pde => _pde.PlanDesarrolloEstructuraId == pde.PlanDesarrolloEstructuraId);
            listaPDE = listaPDE.filter(_pde => _pde.Etiqueta == __planDesarrolloEstructura.Etiqueta && _pde.PlanDesarrolloEstructuraId != pde.PlanDesarrolloEstructuraId);
            if (listaPDE.some(_pde => _pde.Etiqueta != pde.Etiqueta)) {
                listaPDE.map(_pde => {
                    // Actualizamos el Etiqueta
                    _pde.Etiqueta = pde.Etiqueta;
                    store.update(_pde.PlanDesarrolloEstructuraId, _pde)
                        .fail(function () {
                            toast("No se pudo actualizar el registro en la tabla.", "error");
                        });
                });
            }
        }
        
        store.update(pde.PlanDesarrolloEstructuraId, pde)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxTreeListPNDE.getDataSource().reload();
                // Habilitamos los botones de acciones
                habilitaComponentes(true);
                //Ocultamos el modal
                modalPNDE.modal('hide');
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
    modalConfirmaEliminarRegistro.modal('show');
}

eliminaRow = () => {
    // Ocultamos el modal confirma eliminar registro
    modalConfirmaEliminarRegistro.modal('hide');
    // Eliminamos el registro de la tabla
    if (rowEliminar != null) {
        // Obtenemos la instancia del DataSource
        var treeListPNDE = dxTreeListPNDE.getDataSource();
        // Verificar si hay los registros del Padre para eliminar o actualizar
        eliminaRegistrosPadre(treeListPNDE);
        // Actualizar los ordenes
        actualizaRegistrosOrden(treeListPNDE);
        // Si el registro viene de la base de datos
        // para posteriormente eliminarlo en la base de datos
        if (rowEliminar.PlanDesarrolloEstructuraId > 0) {
            // Actualizamos el borrado del registro
            rowEliminar.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO;

            treeListPNDE.store().update(rowEliminar.PlanDesarrolloEstructuraId, rowEliminar)
                .done(function () {
                    //Recargamos la informacion de la tabla
                    treeListPNDE.reload();
                    // Habilitamos los botones de acciones
                    habilitaComponentes(true);
                })
                .fail(function () {
                    toast("No se pudo actualizar el artículo en la tabla.", "error");
                });
        } else {
            treeListPNDE.store().remove(rowEliminar.PlanDesarrolloEstructuraId)
                .done(function () {
                    // Recargamos la informacion de la tabla
                    treeListPNDE.reload();
                    // Habilitamos los botones de acciones
                    habilitaComponentes(true);
                })
                .fail(function () {
                    toast("No se pudo eliminar el artículo de la tabla.", "error");
                });
        }
        // Limpiar Row Eliminar
        rowEliminar = null;
    }
}

eliminaRegistrosPadre = (treeListPNDE) => {
    let listPNDE = [],
        listEliminadosPadre = [],
        listEstructuraPadreId = [],
        siguiente = true;

    listEstructuraPadreId.push(rowEliminar.PlanDesarrolloEstructuraId);
    //Obtenemos todos los registros que hay en el dxGridDetalles
    treeListPNDE.store().load().done((response) => listPNDE = response );
    // Busca los registros del padre para agregar lista de padre
    do {
        const _listPNDE = listPNDE.filter(_pnde => listEstructuraPadreId.indexOf(_pnde.EstructuraPadreId) >= 0 && _pnde.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO);
        if (_listPNDE.length > 0) {
            listEstructuraPadreId = [];
            _listPNDE.map(_pnde => {
                listEstructuraPadreId.push(_pnde.PlanDesarrolloEstructuraId);
                listEliminadosPadre.push(_pnde.PlanDesarrolloEstructuraId);
            });
        } else {
            siguiente = false;
        }
    } while (siguiente);

    if (listEliminadosPadre.length > 0) {
        listEliminadosPadre.map(planDesarrolloEstructuraId => {
            let pnde = $.extend(true, {}, listPNDE.find(_pnde => _pnde.PlanDesarrolloEstructuraId == planDesarrolloEstructuraId));
            if (pnde.PlanDesarrolloEstructuraId > 0) {
                pnde.EstatusId = ControlMaestroMapeo.EstatusRegistro.BORRADO;
                treeListPNDE.store().update(pnde.PlanDesarrolloEstructuraId, pnde);
            } else {
                treeListPNDE.store().remove(pnde.PlanDesarrolloEstructuraId);
            }
        });
    }
}

actualizaRegistrosOrden = (treeListPDE) => {
    if (rowEliminar.EstructuraPadreId) {
        let listaPDE = [],
            nuevoListaPlanDesarrolloEstructura = [];
        //Obtenemos todos los registros que hay en el dxGridDetalles
        treeListPDE.store().load().done((response) => listaPDE = response.filter(pde => pde.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO));
        console.log(rowEliminar);
        var __planDesarrolloEstructura = listaPDE.find(pde => pde.PlanDesarrolloEstructuraId == rowEliminar.EstructuraPadreId),
            __listaPlanDesarrolloEstructura = listaPDE.filter(pde => pde.Etiqueta == __planDesarrolloEstructura.Etiqueta);

        __listaPlanDesarrolloEstructura.map(pde => {
            listaPDE.filter(_pde => _pde.EstructuraPadreId == pde.PlanDesarrolloEstructuraId).map(_pde => {
                if (rowEliminar.Orden < _pde.Orden) {
                    _pde.Orden = _pde.Orden - 1;
                    nuevoListaPlanDesarrolloEstructura.push(_pde);
                }
            });
        });

        nuevoListaPlanDesarrolloEstructura.map(pde => treeListPDE.store().update(pde.PlanDesarrolloEstructuraId, pde));
    }
}

eliminar = () => {
    // Ocultamos Modal
    modalConfirmaEliminar.modal('hide');
    // Mostramos Loader
    dxLoaderPanel.show();
    // Enviamos la informacion al controlador
    var planDesarrollo = dxForm.option('formData');
    planDesarrollo.FechaInicio = planDesarrollo.FechaInicio.toLocaleString();
    planDesarrollo.FechaFin = planDesarrollo.FechaFin.toLocaleString();

    $.ajax({
        type: "POST",
        url: API_FICHA + "eliminar",
        data: { planDesarrollo },
        success: function () {
            // Mostramos mensaje de Exito
            toast("Registro borrado con exito!", 'success');

            // Regresamos al listado
            regresarListado();
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();
            //Mostramos mensaje de error
            toast("Error al guadar:\n" + response.responseText, 'error');
        }
    });
}

validaHayCambios = () => {
    var planDesarrollo = $.extend(true, {}, dxForm.option('formData'));
    // Obtener los datos para saber hay los datos o no hay para guardar
    const data = obtenerData(planDesarrollo);
    //console.log(data);
    if (data.planDesarrollo == null && data.listPlanDesarrolloEstructura.length == 0) {
        regresarListado();
    } else {
        modalCerrar.modal('show');
    }
}

guardaCambios = () => {
    // Mostramos Loader
    dxLoaderPanel.show();
    // Validamos que la informacion requerida del Formulario este completa
    if (!dxForm.validate().isValid) {
        toast("Favor de completar los campos requeridos.", 'error');
        // Ocultamos Loader
        dxLoaderPanel.hide();
        return;
    }
    // Obetenemos el Form
    var planDesarrollo = $.extend(true, {}, dxForm.option('formData'));
    // Validamos si la fecha fin es menor que la fecha inicio
    if (planDesarrollo.FechaFin.getTime() < planDesarrollo.FechaInicio.getTime()) {
        toast("La Fecha Fin no puede ser menor que la Fecha Inicio.", 'error');
        // Ocultamos Loader
        dxLoaderPanel.hide();
        return;
    }
    // Obtener los datos para saber hay los datos o no hay para guardar
    const data = obtenerData(planDesarrollo);
    //console.log(data);
    if (data.planDesarrollo == null && data.listPlanDesarrolloEstructura.length == 0) {
        toast("No se puede guardar ya que no hubo cambios en la información", "error");
        // Ocultamos Loader
        dxLoaderPanel.hide();
        return;
    }

    $.ajax({
        type: "POST",
        url: API_FICHA + "guardar",
        data: data,
        success: function (response) {
            // Mostramos mensaje de Exito
            toast(response, 'success');
            // Redirigir a listado
            window.location.href = API_FICHA + 'listar';
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

obtenerData = (planDesarrollo) => {
    let data = {
        planDesarrollo: null,
        listPlanDesarrolloEstructura: []
    };
    // Plan Desarrollo
    if (planDesarrollo.PlanDesarrolloId > 0) {
        if (planDesarrollo.NombrePlan != _planDesarrollo.NombrePlan || esCambiaFecha(planDesarrollo.FechaInicio, _planDesarrollo.FechaInicio) || esCambiaFecha(planDesarrollo.FechaFin, _planDesarrollo.FechaFin) || planDesarrollo.TipoPlanId != _planDesarrollo.TipoPlanId) {
            let actualizarPlanDesarrollo = $.extend(true, {}, planDesarrollo);
            actualizarPlanDesarrollo.FechaInicio = actualizarPlanDesarrollo.FechaInicio.toLocaleString();
            actualizarPlanDesarrollo.FechaFin = actualizarPlanDesarrollo.FechaFin.toLocaleString();
            data.planDesarrollo = actualizarPlanDesarrollo;
        }
    } else {
        planDesarrollo.FechaInicio = planDesarrollo.FechaInicio.toLocaleString();
        planDesarrollo.FechaFin = planDesarrollo.FechaFin.toLocaleString();
        data.planDesarrollo = planDesarrollo;
    }
    // Plan Desarrollo Estructura
    let listPlanDesarrolloEstructura = [];
    dxTreeListPNDE.getDataSource().store().load().done(response => listPlanDesarrolloEstructura = response);
    listPlanDesarrolloEstructura.map(pnde => {
        let planDesarrolloEstructura = $.extend(true, {}, pnde);
        // Hay eliminar
        if (planDesarrolloEstructura.EstatusId != ControlMaestroMapeo.EstatusRegistro.BORRADO) {
            // Si el ID es nuevo para registrar o editar
            if (planDesarrolloEstructura.PlanDesarrolloEstructuraId > 0) {
                const _planDesarrolloEstructura = $.extend(true, {}, _listPlanDesarrolloEstructura.find(_pnde => _pnde.PlanDesarrolloEstructuraId == planDesarrolloEstructura.PlanDesarrolloEstructuraId));
                if (planDesarrolloEstructura.EstructuraPadreId != _planDesarrolloEstructura.EstructuraPadreId || planDesarrolloEstructura.Nombre != _planDesarrolloEstructura.Nombre || planDesarrolloEstructura.Etiqueta != _planDesarrolloEstructura.Etiqueta || planDesarrolloEstructura.Orden != _planDesarrolloEstructura.Orden) {
                    let actualizarPlanDesarrolloEstructura = $.extend(true, {}, planDesarrolloEstructura);
                    //actualizarPlanDesarrolloEstructura.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(actualizarPlanDesarrolloEstructura.Timestamp)));
                    data.listPlanDesarrolloEstructura.push(actualizarPlanDesarrolloEstructura);
                }
            } else {
                data.listPlanDesarrolloEstructura.push(planDesarrolloEstructura);
            }
        } else {
            //planDesarrolloEstructura.Timestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(planDesarrolloEstructura.Timestamp)));
            data.listPlanDesarrolloEstructura.push(planDesarrolloEstructura);
        }
    });
    return data;
}

esCambiaFecha = (date1, date2) => {
    return new Date(date1.getFullYear() + '/' + (date1.getMonth() + 1) + '/' + date1.getDate()).getTime() != new Date(date2.getFullYear() + '/' + (date2.getMonth() + 1) + '/' + date2.getDate()).getTime();
}

onValueChangedEstructuraPadreId = (event) => {
    var listaPDE = [],
        // Obtenemos los datos que hay en el formulario
        planDesarrolloEstructura = dxFormModal.option('formData'),
        orden = null,
        nuevoListaPlanDesarrolloEstructura = [];
    // Obtenemos todos los registros que hay en el dxGridDetalles
    //dxTreeListPNDE.getDataSource().store().load().done((response) => { listPNDE = response.filter(pnde => pnde.EstructuraPadreId == event.value && pnde.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO); });
    dxTreeListPNDE.getDataSource().store().load().done((response) => { listaPDE = response.filter(pde => pde.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO); });

    if (event.value) {
        var __planDesarrolloEstructura = listaPDE.find(pde => pde.PlanDesarrolloEstructuraId == event.value),
            __listaPlanDesarrolloEstructura = listaPDE.filter(pde => pde.Etiqueta == __planDesarrolloEstructura.Etiqueta);

        __listaPlanDesarrolloEstructura.map(pde => {
            listaPDE.filter(_pde => _pde.EstructuraPadreId == pde.PlanDesarrolloEstructuraId).map(_pde => nuevoListaPlanDesarrolloEstructura.push(_pde));
        });
        listaPDE = nuevoListaPlanDesarrolloEstructura;
    } else {
        listaPDE = listaPDE.filter(pde => pde.EstructuraPadreId == null);
    }

    //if (listaPDE.some(pde => pde.EstructuraPadreId == event.value)) {
    //    listaPDE = listaPDE.filter(pde => pde.EstructuraPadreId == event.value);
    //} else {
    //    var __planDesarrolloEstructura = listaPDE.find(pde => pde.PlanDesarrolloEstructuraId == event.value),
    //        __listaPlanDesarrolloEstructura = listaPDE.filter(pde => pde.Etiqueta == __planDesarrolloEstructura.Etiqueta),
    //        nuevoListaPlanDesarrolloEstructura = [];

    //    bucle:
    //    for (let i = 0; i < __listaPlanDesarrolloEstructura.length; i++) {
    //        var ___planDesarrolloEstructura = __listaPlanDesarrolloEstructura[i];
    //        if (listaPDE.some(pde => pde.EstructuraPadreId == ___planDesarrolloEstructura.PlanDesarrolloEstructuraId)) {
    //            nuevoListaPlanDesarrolloEstructura = listaPDE.filter(pde => pde.EstructuraPadreId == ___planDesarrolloEstructura.PlanDesarrolloEstructuraId);
    //            // Cambiamos el valor si es padre (true);
    //            esPadre = true;
    //            break bucle;
    //        }
    //    }

    //    listaPDE = nuevoListaPlanDesarrolloEstructura;
    //}
    // Establecemos los registros con el ID de Padre
    orden = listaPDE.filter(pde => pde.PlanDesarrolloEstructuraId != planDesarrolloEstructura.PlanDesarrolloEstructuraId).length;
    
    if (event.value) {
        if (modalPNDE.attr('isEdit') == 'false') {
            // Asignamos el contador para orden que hay en la seleccion de estructura padre
            //if (esPadre) {
            //    dxFormModal.updateData('Orden', 0);
            //} else {
            //    dxFormModal.updateData('Orden', orden);
            //}

            dxFormModal.updateData('Orden', orden);

            if (orden == 0) {
                // Habilitar el campo Etiqueta
                dxFormModal.getEditor('Etiqueta').option({ disabled: false, value: '' });
                // Cambiamos el valor
                esPadreEtiqueta = false;
            } else {
                dxFormModal.getEditor('Etiqueta').option({ disabled: true, value: listaPDE[0].Etiqueta });
                // Cambiamos el valor
                esPadreEtiqueta = true;
            }
        } else {
            if (listaPDE.some(pde => pde.PlanDesarrolloEstructuraId == planDesarrolloEstructura.PlanDesarrolloEstructuraId && pde.Orden == 0 && pde.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO)) {
                // Habilitar el campo Etiqueta
                dxFormModal.getEditor('Etiqueta').option({ disabled: false });
                // Cambiamos el valor
                esPadreEtiqueta = false;
            } else {
                if (orden == 0) {
                    // Habilitar el campo Etiqueta
                    dxFormModal.getEditor('Etiqueta').option({ disabled: false, value: '' });
                    // Cambiamos el valor
                    esPadreEtiqueta = false;
                } else {
                    // Deshabilitar el campo Etiqueta
                    dxFormModal.getEditor('Etiqueta').option({ disabled: true, value: listaPDE[0].Etiqueta });
                    // Cambiamos el valor
                    esPadreEtiqueta = true;
                }
            }
        }
        // Ocultamos el DropDown
        dxDropDownBoxEstructuraPadreId.close();
    } else {
        if (orden == 0) {
            // Habilitar el campo Etiqueta
            dxFormModal.getEditor('Etiqueta').option({ disabled: false, value: '' });
            // Cambiamos el valor
            esPadreEtiqueta = false;
        } else {
            // Deshabilitar el campo Etiqueta
            dxFormModal.getEditor('Etiqueta').option({ disabled: true, value: listaPDE[0].Etiqueta });
            // Cambiamos el valor
            esPadreEtiqueta = true;
        }
        // Ocultamos el DropDown
        dxDropDownBoxEstructuraPadreId.close();
    }
}

regresarListado = () => {
    window.location.href = API_FICHA + "listar";
}

recargarFicha = () => {
    // Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA + (_planDesarrollo.PlanDesarrolloId > 0 ? "editar/" + _planDesarrollo.PlanDesarrolloId : "nuevo");
}

// Toast //
toast = (message, type) => {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 3500);
}
///////////