//Componentes
var dxCboEmpleado;
var dxCboDependencia;
var dxTxtFecha;
var dxCboUnidadAdministrativa;
var dxCboProyecto;

var dxCboProducto;
var dxCboUnidadMedida;
var dxTxtCostoUnitario;
var dxTxtCantidad;
var dxTxtTotalPartida;

//Modales
var modalComentario;
var modalMotivoRechazo;
var modalDetalle;
var modalConfirmaDeshacer;
var modalConfirmaEliminar;
var modalConfirmaEliminarArticulo;
var modalConfirmaLimpiar;

//Botones
var dxButtonImprimir;
var dxButtonCancelar;
var dxButtonDeshacer;
var dxButtonEliminar;
var dxButtonGuardaCambios;
var dxButtonEnviarAutorizar;
var dxButtonAgregar;

//Forms
var dxForm;
var dxFormModal;
var dxGridDetalles;
var dxFormModalComentario;
var dxFormModalMotivoRechazo;

//Variables Globales
var modeloVacio;
var rowEliminar;
var registrosEliminados;

//Variables de Control
var requisicionId;
var requisicionEstatusId;
var contadorRegistrosNuevos;
var ignoraEventos;
var cambios;
var eventoRegresar;

var unidadAdministrativaTemp;
var proyectoTemp;

//Variables Estaticas
var ESTATUS_NUEVO = ControlMaestroMapeo.AREstatusRequisicionDetalle.ACTIVO;
var ESTATUS_EDITADO = ControlMaestroMapeo.AREstatusRequisicionDetalle.MODIFICADO;
var ESTATUS_ELIMINADO = ControlMaestroMapeo.AREstatusRequisicionDetalle.CANCELADO;

var API_FICHA = "/compras/requisiciones/requisicionmaterial/";

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();

    //Respaldamos el modelo del Form
    getForm();

    //Deshabilitamos los botones de acciones
    habilitaComponentes();

    //Respaldamos el modelo vacio del Form
    modeloVacio = $.extend(true, {}, dxFormModal.option('formData'));
});

var inicializaVariables = function () {
    dxCboEmpleado = $('#dxCboEmpleado').dxSelectBox("instance");
    dxCboDependencia = $('#dxCboDependencia').dxSelectBox("instance");
    dxTxtFecha = $('#dxTxtFecha').dxDateBox("instance");
    dxCboUnidadAdministrativa = $('#dxCboUnidadAdministrativa').dxSelectBox("instance");
    dxCboProyecto = $('#dxCboProyecto').dxSelectBox("instance");

    dxCboProducto = $('#dxCboProducto').dxDropDownBox("instance");
    dxCboUnidadMedida = $('#dxCboUnidadMedida').dxSelectBox("instance");
    dxTxtCostoUnitario = $('#dxTxtCostoUnitario').dxNumberBox("instance");
    dxTxtCantidad = $('#dxTxtCantidad').dxNumberBox("instance");
    dxTxtTotalPartida = $('#dxTxtTotalPartida').dxNumberBox("instance");

    modalComentario = $('#modalComentario');
    modalMotivoRechazo = $('#modalMotivoRechazo');
    modalDetalle = $('#modalDetalle');
    modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    modalConfirmaEliminar = $('#modalConfirmaEliminar');
    modalConfirmaEliminarArticulo = $('#modalConfirmaEliminarArticulo');
    modalConfirmaLimpiar = $('#modalConfirmaLimpiar');

    dxButtonImprimir = $('#dxButtonImprimir').dxButton("instance");
    dxButtonCancelar = $('#dxButtonCancelar').dxButton("instance");
    dxButtonDeshacer = $('#dxButtonDeshacer').dxButton("instance");
    dxButtonEliminar = $('#dxButtonEliminar').dxButton("instance");
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");
    dxButtonEnviarAutorizar = $('#dxButtonEnviarAutorizar').dxButton("instance");
    dxButtonAgregar = $('#dxButtonAgregar').dxButton("instance");

    dxForm = $("#dxForm").dxForm("instance");
    dxFormModal = $("#dxFormModal").dxForm("instance");
    dxGridDetalles = $("#dxGridDetalles").dxDataGrid("instance");
    dxFormModalComentario = $("#dxFormModalComentario").dxForm("instance");
    dxFormModalMotivoRechazo = $("#dxFormModalMotivoRechazo").dxForm("instance");
    dxButtonAgregar.option("disabled", true);

    rowEliminar = null;
    registrosEliminados = [];

    contadorRegistrosNuevos = -1;
    ignoraEventos = false;
    cambios = false;
    eventoRegresar = true;

    dxCboProducto.option("dataSource", null);

    if (getForm().RequisicionMaterialId == 0) {
        //Validamos si existe una sola Unidad Administrativa
        ignoraEventos = true;

        dxCboUnidadAdministrativa.option("value", _listUnidadesAdministrativas.length == 1 ? _listUnidadesAdministrativas[0].DependenciaId : null);
        dxCboProyecto.option("disabled", !dxCboUnidadAdministrativa.option("value"));
        filtrarProyectos();

        ignoraEventos = false;
    }
    else {
        dxButtonAgregar.option("disabled", false);
    }

    //Asignamos el límite en los campos de Fecha
    var minDate = _ejercicio + "-01-01";
    var maxDate = new Date().toISOString().split('T')[0];

    dxTxtFecha.option("min", minDate);
    dxTxtFecha.option("max", maxDate);

    if (getForm().RequisicionMaterialId == 0 && !_fechaOperacion) {
        ignoraEventos = true;

        //Asignamos la Fecha de Operación
        dxTxtFecha.option("value", null);

        ignoraEventos = false;

        //Mostramos mensaje
        toast("Es necesario realizar la Configuración de Fechas correspondiente.", 'warning');
    }
}

var getForm = function () {
    var modelo = $.extend(true, {}, dxForm.option('formData'));
    modelo.RequisicionMaterialId = modelo.RequisicionMaterialId || 0;
    modelo.AreaId = dxCboDependencia.option("value");
    requisicionId = modelo.RequisicionMaterialId;
    requisicionEstatusId = modelo.EstatusId;

    var fecha = modelo.FechaRequisicion;

    if (fecha) {
        fecha.setHours(new Date().getHours());
        fecha.setMinutes(new Date().getMinutes());
        fecha.setSeconds(new Date().getSeconds());
        fecha.setMilliseconds(0);

        modelo.FechaRequisicion = fecha.toLocaleString();
    }

    return modelo;
}

var habilitaComponentes = function () {
    var permiteCancelar = requisicionId != 0 && !_soloLectura && !cambios && (requisicionEstatusId == ControlMaestroMapeo.AREstatusRequisicion.GUARDADA || requisicionEstatusId == ControlMaestroMapeo.AREstatusRequisicion.REVISION);
    var permiteEnviar = requisicionId != 0 && !_soloLectura && !cambios && (requisicionEstatusId == ControlMaestroMapeo.AREstatusRequisicion.GUARDADA || requisicionEstatusId == ControlMaestroMapeo.AREstatusRequisicion.REVISION);
    var permiteGuardar = !_soloLectura && (requisicionId == 0 || cambios || !permiteEnviar);

    dxButtonDeshacer.option("visible", !_soloLectura);
    dxButtonDeshacer.option("disabled", !cambios);

    dxButtonEliminar.option("visible", permiteCancelar);
    dxButtonEnviarAutorizar.option("visible", permiteEnviar);
    dxButtonGuardaCambios.option("visible", permiteGuardar);

    dxButtonImprimir.option("visible", !cambios && requisicionId > 0);
}

var permiteEditarArticulo = function (event) {
    var estatusEditar = [
        ControlMaestroMapeo.AREstatusRequisicionDetalle.ACTIVO,
        ControlMaestroMapeo.AREstatusRequisicionDetalle.ENVIADO,
        ControlMaestroMapeo.AREstatusRequisicionDetalle.MODIFICADO,
        ControlMaestroMapeo.AREstatusRequisicionDetalle.POR_SURTIR,
        ControlMaestroMapeo.AREstatusRequisicionDetalle.REVISION,
    ]

    return !_soloLectura && estatusEditar.includes(event.row.data.EstatusId);
}

var verComentarios = function (event) {
    return (requisicionId == 0 || event.row.data.EstatusId != ControlMaestroMapeo.AREstatusRequisicionDetalle.RECHAZADO)
        && (event.row.data.Comentarios || !_soloLectura);
}

var verMotivoRechazo = function (event) {
    return requisicionId > 0 && event.row.data.MotivoRechazo;
}

var setCambios = function () {
    if (!ignoraEventos) {
        cambios = true;
        habilitaComponentes();
    }
}

var validaEliminar = function (event) {
    //Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}

var eliminaRequisicion = function () {
    //Mostramos Loader
    dxLoaderPanel.show();

    //Hacemos la petición para eliminar el registro
    $.ajax({
        type: "POST",
        url: API_FICHA + "eliminarPorModelo",
        data: { requisicionMaterial: getForm() },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Registro eliminado con exito!", 'success');

            //Recargamos la ficha
            regresarListado();
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al eliminar:\n" + response.responseText, 'error');
        }
    });
}

var validaEliminarArticulo = function (event) {
    //Obtenemos una copia del objeto a eliminar
    rowEliminar = $.extend(true, {}, event.row.data);

    //Mostramos el modal de confirmacion
    modalConfirmaEliminarArticulo.modal('show');
}

var eliminaArticulo = function () {
    //Obtenemos la instancia store del DataSource
    var store = dxGridDetalles.getDataSource().store();

    //Eliminamos el registro de la tabla
    if (rowEliminar != null) {
        store.remove(rowEliminar.RequisicionMaterialDetalleId)
            .done(function () {
                //Si el registro viene de la base de datos, respaldamos el registro 
                //para posteriormente eliminarlo en la base de datos
                if (rowEliminar.RequisicionMaterialDetalleId > 0) {
                    //Actualizamos el estatus del registro a "Eliminado"
                    rowEliminar.EstatusId = ESTATUS_ELIMINADO; //Borrar registro

                    //Respaldamos el registro que se acaba de eliminar
                    registrosEliminados.push(rowEliminar);
                }

                //Recargamos la informacion de la tabla
                dxGridDetalles.getDataSource().reload();

                setCambios();
            })
            .fail(function () {
                toast("No se pudo eliminar el registro de la tabla.", "error");
            });

        rowEliminar = null;
    }
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

var filtrarProyectos = function (event) {
    var listadoProyectos = [];
    var dependenciaId = dxCboUnidadAdministrativa.option("value");
    var proyectosPorDependencia = _listProyectosDependencias.filter(x => x.DependenciaId == dependenciaId);

    proyectosPorDependencia.forEach(m => {
        listadoProyectos.push(_listProyectos.find(x => x.ProyectoId === m.ProyectoId));
    });

    //Actualizamos el DataSource del combo Proyectos
    var dataSource = new DevExpress.data.DataSource({
        store: {
            type: "array",
            key: "ProyectoId",
            data: listadoProyectos
        }
    });

    //Asignamos el source al combo
    dxCboProyecto.option("dataSource", dataSource);

    //Validamos si existe un solo Proyecto
    if (listadoProyectos.length == 1) {
        dxCboProyecto.option("value", listadoProyectos[0].ProyectoId);
    }
}

var cboEmpleadoChange = function (event) {
    var empleado = _listEmpleados.find(x => x.EmpleadoId == event.value);
    var areaId = dxCboDependencia.option("value");

    if (areaId != (empleado?.AreaAdscripcionId || -1)) {
        dxCboDependencia.option("value", empleado ? empleado.AreaAdscripcionId : null);
    }

    setCambios();
}

var cboDependenciaChange = function (event) {
    //Limpiamos los listados
    _listUnidadesAdministrativas = [];
    _listProyectos = [];
    _listProyectosDependencias = [];

    dxCboUnidadAdministrativa.option("dataSource", _listUnidadesAdministrativas);
    dxCboProyecto.option("dataSource", _listProyectos);

    dxCboUnidadAdministrativa.option("value", null);
    dxCboProyecto.option("value", null);

    var areaId = event.value;

    if (areaId) {
        //Enviamos la informacion al controlador
        $.ajax({
            type: "POST",
            url: API_FICHA + "getCombosPorAreaId",
            data: { areaId: areaId },
            success: function (response) {
                //Ocultamos Loader
                dxLoaderPanel.hide();

                //Asignamos los listados
                _listUnidadesAdministrativas = response.ListUnidadesAdministrativas;
                _listProyectos = response.ListProyectos;
                _listProyectosDependencias = response.ListProyectosDependencias;

                dxCboUnidadAdministrativa.option("dataSource", _listUnidadesAdministrativas);
            },
            error: function (response, status, error) {
                //Ocultamos Loader
                dxLoaderPanel.hide();

                //Mostramos mensaje de error
                toast("Error:\n" + response.responseText, 'error');
            }
        });
    }

    setCambios();
}

var cboUnidadAdministrativaChange = function (event) {
    dxCboProyecto.option("disabled", !event.value && !dxCboProyecto.option("readOnly"));

    if (!ignoraEventos) {
        unidadAdministrativaTemp = event.previousValue;

        ignoraEventos = true;

        proyectoTemp = dxCboProyecto.option("value");
        dxCboProyecto.option("value", null);
        filtrarProyectos();

        ignoraEventos = false;

        validaLimpiarTabla();
    }
}

var cboProyectoChange = function (event) {
    if (!ignoraEventos) {        
        proyectoTemp = event.previousValue;        
        validaLimpiarTabla();
    }
    dxButtonAgregar.option("disabled", dxCboProyecto.option("value") == null);
}

var validaLimpiarTabla = function () {
    if (!ignoraEventos) {
        //Obtenemos todos los registros que hay en el dxGridDetalles
        var detalles;
        dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

        //Validamos que haya registros en la tabla detalles
        if (detalles && detalles.length) {
            //Mostramos el modal de confirmacion
            modalConfirmaLimpiar.modal('show');
        } else {
            limpiarTabla();
        }

        setCambios();
    }
}

var cancelarLimpirar = function () {
    ignoraEventos = true;

    dxCboUnidadAdministrativa.option("value", unidadAdministrativaTemp);
    filtrarProyectos();

    dxCboProyecto.option("value", proyectoTemp);

    ignoraEventos = false;
}

var limpiarTabla = function () {
    //Limpiamos y recargamos la informacion de la tabla
    var dataSource = dxGridDetalles.getDataSource();
    dataSource.store().clear();
    dataSource.reload();

    dxCboProducto.option("dataSource", null);

    unidadAdministrativaTemp = dxCboUnidadAdministrativa.option("value");
    filtrarProyectos();

    proyectoTemp = dxCboProyecto.option("value");
}

var nuevoRegistro = function () {
    //Obtenemos los productos y Mostramos el modal
    getComboProductos(true, null);
}

var mostrarNuevoRegistro = function () {
    //Inicializamos el modelo del Form
    dxFormModal.option("formData", $.extend(true, {}, modeloVacio));

    //Inicializamos los campos del Form
    dxFormModal.resetValues();

    //Asignamos un Id al Form
    dxFormModal.updateData("RequisicionMaterialDetalleId", contadorRegistrosNuevos);

    //Cambiamos el estatus del registro a "NUEVO"
    dxFormModal.updateData("EstatusId", ESTATUS_NUEVO);

    //Decrementamos el contador de Registros para el siguiente nuevo registro
    contadorRegistrosNuevos -= 1;    

    //Marcamos el modal con estatus "NUEVO"
    modalDetalle.attr("estatus", ESTATUS_NUEVO);

    //Mostramos el modal
    modalDetalle.modal('show');
}

var editaRegistro = function (event) {
    //Obtenemos los productos y Mostramos el modal
    getComboProductos(false, event);
}

var mostrarRegistroEditado = function (event) {  
    //Obtenemos una copia del objeto a modificar
    var modelo = $.extend(true, {}, event.row.data);

    //Cambiamos el estatus del registro a "EDITADO" solo si no es un registro "NUEVO"
    modelo.EstatusId = modelo.RequisicionMaterialDetalleId < 0 ? ESTATUS_NUEVO : ESTATUS_EDITADO;

    //Le pasamos el objeto al Form para que cargue sus valores
    dxFormModal.option("formData", modelo);

    //Marcamos el modal con estatus "EDITADO"
    modalDetalle.attr("estatus", ESTATUS_EDITADO);

    //Mostramos el modal
    modalDetalle.modal('show');
}

var editaComentario = function (event) {
    //Obtenemos una copia del objeto a modificar
    var modelo = $.extend(true, {}, event.row.data);

    //Le pasamos el objeto al Form para que cargue sus valores
    dxFormModalComentario.option("formData", modelo);

    //Mostramos el modal
    modalComentario.modal('show');
}

var editaMotivoRechazo = function (event) {
    //Obtenemos una copia del objeto a modificar
    var modelo = $.extend(true, {}, event.row.data);

    //Le pasamos el objeto al Form para que cargue sus valores
    dxFormModalMotivoRechazo.option("formData", modelo);

    //Mostramos el modal
    modalMotivoRechazo.modal('show');
}

var getComboProductos = function (nuevo, event) {
    //Deshabilitamos el combo de Productos
    dxCboProducto.option("disabled", true);

    if (!dxCboProducto.option("dataSource") || !dxCboProducto.option("dataSource").length) {
        dxCboProducto.option("value", null);
        dxCboProducto.option("dataSource", null);
        dxCboProducto.option("contentTemplate", null);
        dxCboProducto.option("contentTemplate", $("#DataGridTemplate"));

        var areaId = dxCboDependencia.option("value");
        var unidadAdministrativaId = dxCboUnidadAdministrativa.option("value");
        var proyectoId = dxCboProyecto.option("value");

        //Mostramos Loader
        dxLoaderPanel.show();

        //Enviamos la informacion al controlador
        $.ajax({
            type: "POST",
            url: API_FICHA + "getProductos",
            data: { areaId: areaId, unidadAdministrativaId: unidadAdministrativaId, proyectoId: proyectoId },
            success: function (response) {
                // Ocultamos Loader
                dxLoaderPanel.hide();

                //Asignamos el listado al combo
                dxCboProducto.option("dataSource", response);

                //Habilitamos el combo de Productos
                dxCboProducto.option("disabled", false);

                //Mostramos el modal
                if (nuevo) {
                    mostrarNuevoRegistro();
                } else {
                    mostrarRegistroEditado(event);
                }
            },
            error: function (response, status, error) {
                // Ocultamos Loader
                dxLoaderPanel.hide();

                //Mostramos mensaje de error
                toast("Error:\n" + response.responseText, 'error');
            }
        });
    } else {
        //Habilitamos el combo de Productos
        dxCboProducto.option("disabled", false);

        //Mostramos el modal
        if (nuevo) {
            mostrarNuevoRegistro();
        } else {
            mostrarRegistroEditado(event);
        }
    }
}

var cboProductoChange = function (event) {
    dxCboProducto.close();

    dxCboUnidadMedida.option("value", event.UnidadDeMedidaId);
    dxTxtCostoUnitario.option("value", event.CostoUnitario);

    dxFormModal.updateData("AlmacenId", event.AlmacenId);
    dxFormModal.updateData("Almacen", event.Almacen);
    dxFormModal.updateData("ProductoId", event.ProductoId);
    dxFormModal.updateData("Descripcion", event.Descripcion);
    dxFormModal.updateData("Producto", event.Producto);
    dxFormModal.updateData("ProyectoId", event.ProyectoId);
    dxFormModal.updateData("Proyecto", event.Proyecto);
    dxFormModal.updateData("TipoGastoId", event.TipoGastoId);
    dxFormModal.updateData("TipoGasto", event.TipoGasto);
    dxFormModal.updateData("UnidadAdministrativaId", event.UnidadAdministrativaId);
    dxFormModal.updateData("UnidadAdministrativa", event.UnidadAdministrativa);
    dxFormModal.updateData("UnidadDeMedida", event.UnidadDeMedida);
}

var calculaTotalPartida = function () {
    var cantidad = round(dxTxtCantidad.option("value"), 4);
    var costo = round(dxTxtCostoUnitario.option("value"), 4);

    dxTxtTotalPartida.option("value", trunc(cantidad * costo, 4));
}

var round = function (num, scale) {
    if (!("" + num).includes("e")) {
        return +(Math.round(num + "e+" + scale) + "e-" + scale);
    } else {
        var arr = ("" + num).split("e");
        var sig = "";

        if (+arr[1] + scale > 0) {
            sig = "+";
        }

        return +(Math.round(+arr[0] + "e" + sig + (+arr[1] + scale)) + "e-" + scale);
    }
}

var trunc = function (num, pos) {
    var s = num.toString();
    var l = s.length;
    var decimalLength = s.indexOf('.') + 1;

    if (l - decimalLength <= pos) {
        return num;
    }

    // Parte decimal del número
    var isNeg = num < 0;
    var decimal = num % 1;
    var entera = isNeg ? Math.ceil(num) : Math.floor(num);

    // Parte decimal como número entero
    var decimalFormated = Math.floor(
        Math.abs(decimal) * Math.pow(10, pos)
    )

    // Sustraemos del número original la parte decimal
    // y le sumamos la parte decimal que hemos formateado
    var finalNum = entera + ((decimalFormated / Math.pow(10, pos)) * (isNeg ? -1 : 1));

    return finalNum;
}

var existeProducto = function (requisicionMaterialDetalleId, productoDetalleId) {
    //Obtenemos todos los registros que hay en el dxGrid
    var detalles;
    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

    //Buscamos un registro que tenga el mismo registro
    var encontrado = detalles.find(x => x.RequisicionMaterialDetalleId !== requisicionMaterialDetalleId && x.ProductoDetalleId === productoDetalleId);

    //Retornamos <true> si se encontro un registro , <false> de lo contrario
    return encontrado;
}

var guardaCambiosModal = function () {
    //Obtenemos el Objeto que se esta creando/editando en el Form 
    var modelo = dxFormModal.option("formData");    

    //Validamos que la informacion requerida del Formulario este completa
    if (!dxFormModal.validate().isValid) {
        toast("Favor de completar los campos requeridos.", 'error');

        return;
    }

    //Validamos que el Producto no se repita
    if (existeProducto(modelo.RequisicionMaterialDetalleId, modelo.ProductoDetalleId)) {
        toast("Ya existe un registro con la misma configuración de Producto y Almacén. Favor de verificar.", 'warning')

        return;
    }    

    //Obtenemos la instancia store del DataSource
    var store = dxGridDetalles.getDataSource().store();

    //Si es un registro nuevo, lo insertamos en el DataSource
    if (modalDetalle.attr("estatus") == ESTATUS_NUEVO) {
        store.insert(modelo)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxGridDetalles.getDataSource().reload();

                setCambios();

                //Ocultamos el modal
                modalDetalle.modal('hide');
            })
            .fail(function () {
                toast("No se pudo agregar el nuevo registro a la tabla.", "error");
            });
    }
    //Si es un registro que se esta editando, actualizamos su informacion en el DataSource
    else {
        store.update(modelo.RequisicionMaterialDetalleId, modelo)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxGridDetalles.getDataSource().reload();

                setCambios();

                //Ocultamos el modal
                modalDetalle.modal('hide');
            })
            .fail(function () {
                toast("No se pudo actualizar el registro en la tabla.", "error");
            });
    }
}

var guardaCambiosComentario = function () {
    //Obtenemos el Objeto que se esta creando/editando en el Form 
    var modelo = dxFormModalComentario.option("formData");    

    //Obtenemos la instancia store del DataSource
    var store = dxGridDetalles.getDataSource().store();

    store.update(modelo.RequisicionMaterialDetalleId, modelo)
        .done(function () {
            //Recargamos la informacion de la tabla
            dxGridDetalles.getDataSource().reload();

            setCambios();

            //Ocultamos el modal
            modalComentario.modal('hide');
        })
        .fail(function () {
            toast("No se pudo actualizar el registro en la tabla.", "error");
        });
}

var guardar = function () {
    guardaCambios(false);
}

var enviarAutorizar = function () {
    guardaCambios(true);
}

var guardaCambios = function (enviarAutorizar) {
    //Validamos que la informacion requerida del Formulario este completa
    if (!dxForm.validate().isValid) {
        toast("Favor de completar los campos requeridos.", 'error');

        return;
    }

    //Obtenemos todos los registros que hay en el dxGridDetalles
    var detalles;
    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

    //Validamos que la informacion requerida del Formulario este completa
    if (!detalles || !detalles.length) {
        toast("Favor de agregar artículos para inventariar.", 'error');

        return;
    }

    //Agregamos los registros borrados, para eliminarlos en base de datos
    detalles = $.merge(detalles, registrosEliminados);

    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + (enviarAutorizar ? "enviarAutorizar" : "guardaCambios"),
        data: { requisicionMaterial: getForm(), detalles: detalles },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Registro guardado con exito!", 'success');

            //Regresamos al listado
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

var imprimir = function () {
    window.open(API_FICHA + "rptSolicitudMateriales/" + requisicionId);
}

var gridBox_displayExpr = function (item) {
    return item ? item.ProductoId + ' - ' + item.Descripcion : null;
}

var onCellPrepared = function (e) {
    var tooltip = $('#tooltip').dxTooltip("instance");
    var propiedad = e.column.dataField;
    var MostrarTooltip = ["UnidadAdministrativaId", "ProyectoId", "FuenteFinanciamientoId", "TipoGastoId"];

    if (e.rowType == "data" && MostrarTooltip.includes(propiedad)) {
        e.cellElement.mouseover(function () {
            tooltip.option("contentTemplate", null);

            switch (propiedad) {
                case "UnidadAdministrativaId":
                    tooltip.option("contentTemplate", document.createTextNode(e.data.UnidadAdministrativa));
                    break;

                case "ProyectoId":
                    tooltip.option("contentTemplate", document.createTextNode(e.data.Proyecto));
                    break;

                case "FuenteFinanciamientoId":
                    tooltip.option("contentTemplate", document.createTextNode(e.data.FuenteFinanciamiento));
                    break;

                case "TipoGastoId":
                    tooltip.option("contentTemplate", document.createTextNode(e.data.TipoGasto));
                    break;
            }

            tooltip.option("target", e.cellElement);
            tooltip.show();
        });

        e.cellElement.mouseout(function () {
            tooltip.hide();
        });
    }
}

var regresarListado = function () {
    window.location.href = API_FICHA + "listar";
}

var recargarFicha = function () {
    // Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA + (requisicionId == 0 ? "nuevo" : "editar/" + requisicionId);
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}