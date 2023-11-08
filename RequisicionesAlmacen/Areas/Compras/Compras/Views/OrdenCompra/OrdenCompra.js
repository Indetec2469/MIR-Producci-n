//Componentes
var dxCboProveedor;
var dxCboAlmacen;
var dxCboTipoOperacion;
var dxCboTipoComprobanteFiscal;
var dxTxtFecha;
var dxTxtFechaRecepcion;
var dxTxtFechaVencimiento;
var dxTxtReferencia;
var dxTxtObservacion;

var dxCboUnidadAdministrativa;
var dxCboProyecto;
var dxCboFuenteFinanciamiento;

var dxCboProducto;
var dxCboImpuesto;
var dxTxtUnidadMedida;
var dxTxtCosto;
var dxTxtAjuste;
var dxTxtFechaCancelacion;

//Modales
var modalDetalle;
var modalConfirmaDeshacer;
var modalConfirmaEliminar;
var modalConfirmaEliminarArticulo;
var modalConfirmaLimpiar;

//Botones
var dxButtonCancelar;
var dxButtonDeshacer;
var dxButtonEliminar;
var dxButtonGuardaCambios;
var dxButtonEnviarAutorizar;

//Forms
var dxForm;
var dxFormModal;
var dxFormModalCancelacion;
var dxGridDetalles;

//Variables Globales
var modeloVacio;
var rowEliminar;
var registrosEliminados;

//Variables de Control
var ordenCompraId;
var ordenCompraEstatusId;
var contadorRegistrosNuevos;
var ignoraEventos;
var cambios;
var eventoRegresar;

var almacenTemp;
var unidadAdministrativaTemp;
var proyectoTemp;
var fuenteFinanciamientoTemp;

//Variables Estaticas
var ESTATUS_ACTIVA = ControlMaestroMapeo.EstatusOrdenCompra.ACTIVA;
var ESTATUS_RECIBO_PARCIAL = ControlMaestroMapeo.EstatusOrdenCompra.RECIBO_PARCIAL;
var ESTATUS_RECIBIDA = ControlMaestroMapeo.EstatusOrdenCompra.RECIBIDA;
var ESTATUS_CANCELADA = ControlMaestroMapeo.EstatusOrdenCompra.CANCELADA;
var ESTATUS_NUEVO = "N";
var ESTATUS_MODIFICADO = "M";

var API_FICHA = "/compras/compras/ordencompra/";

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
    dxCboProveedor = $('#dxCboProveedor').dxSelectBox("instance");
    dxCboAlmacen = $('#dxCboAlmacen').dxSelectBox("instance");
    dxCboTipoOperacion = $('#dxCboTipoOperacion').dxSelectBox("instance");
    dxCboTipoComprobanteFiscal = $('#dxCboTipoComprobanteFiscal').dxSelectBox("instance");
    dxTxtFecha = $('#dxTxtFecha').dxDateBox("instance");
    dxTxtFechaRecepcion = $('#dxTxtFechaRecepcion').dxDateBox("instance");
    dxTxtFechaVencimiento = $('#dxTxtFechaVencimiento').dxDateBox("instance");
    dxTxtReferencia = $('#dxTxtReferencia').dxTextBox("instance");
    dxTxtObservacion = $('#dxTxtObservacion').dxTextArea("instance");

    dxCboUnidadAdministrativa = $('#dxCboUnidadAdministrativa').dxSelectBox("instance");
    dxCboProyecto = $('#dxCboProyecto').dxSelectBox("instance");
    dxCboFuenteFinanciamiento = $('#dxCboFuenteFinanciamiento').dxSelectBox("instance");

    dxCboProducto = $('#dxCboProducto').dxDropDownBox("instance");
    dxCboImpuesto = $('#dxCboImpuesto').dxSelectBox("instance");
    dxTxtUnidadMedida = $('#dxTxtUnidadMedida').dxTextBox("instance");
    dxTxtCosto = $('#dxTxtCosto').dxNumberBox("instance");
    dxTxtAjuste = $('#dxTxtAjuste').dxNumberBox("instance");
    dxTxtFechaCancelacion = $('#dxTxtFechaCancelacion').dxDateBox("instance");

    modalDetalle = $('#modalDetalle');
    modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    modalConfirmaEliminar = $('#modalConfirmaEliminar');
    modalConfirmaEliminarArticulo = $('#modalConfirmaEliminarArticulo');
    modalConfirmaLimpiar = $('#modalConfirmaLimpiar');

    dxButtonCancelar = $('#dxButtonCancelar').dxButton("instance");
    dxButtonDeshacer = $('#dxButtonDeshacer').dxButton("instance");
    dxButtonEliminar = $('#dxButtonEliminar').dxButton("instance");
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");

    dxForm = $("#dxForm").dxForm("instance");
    dxFormModal = $("#dxFormModal").dxForm("instance");
    dxFormModalCancelacion = $("#dxFormModalCancelacion").dxForm("instance");
    dxGridDetalles = $("#dxGridDetalles").dxDataGrid("instance");

    rowEliminar = null;
    registrosEliminados = [];

    contadorRegistrosNuevos = -1;
    ignoraEventos = false;
    cambios = false;
    eventoRegresar = true;

    dxCboProducto.option("dataSource", null);

    //Asignamos el límite en los campos de Fecha
    var fechaActual = new Date().toISOString().split('T')[0];
    var minDate = _ejercicio + "-01-01";
    var maxDate = _ejercicio + "-12-31";
    var fechaOC = getFechaOC();

    dxTxtFecha.option("min", fechaOC);
    dxTxtFecha.option("max", fechaActual);
    dxTxtFechaRecepcion.option("min", fechaOC);
    dxTxtFechaRecepcion.option("max", maxDate);
    dxTxtFechaVencimiento.option("min", fechaOC);
    dxTxtFechaVencimiento.option("max", maxDate);

    dxTxtFechaCancelacion.option("value", fechaActual);

    if (!_fechaOperacion) {
        //Mostramos mensaje
        toast("Es necesario realizar la Configuración de Fechas correspondiente.", 'warning');
    }
}

var getForm = function () {
    var modelo = $.extend(true, {}, dxForm.option('formData'));
    modelo.OrdenCompraId = modelo.OrdenCompraId || 0;
    ordenCompraId = modelo.OrdenCompraId;
    ordenCompraEstatusId = modelo.Status;

    var fechaOC = modelo.Fecha;

    if (fechaOC) {
        fechaOC.setHours(new Date().getHours());
        fechaOC.setMinutes(new Date().getMinutes());
        fechaOC.setSeconds(new Date().getSeconds());
        fechaOC.setMilliseconds(0);

        modelo.Fecha = fechaOC.toLocaleString();
        modelo.Ejercicio = fechaOC.getFullYear();
    }

    var fechaRecepcion = modelo.FechaRecepcion;

    if (fechaRecepcion) {
        fechaRecepcion.setHours(0);
        fechaRecepcion.setMinutes(0);
        fechaRecepcion.setSeconds(0);
        fechaRecepcion.setMilliseconds(0);

        modelo.FechaRecepcion = fechaRecepcion.toLocaleString();
    }

    return modelo;
}

var habilitaComponentes = function () {
    dxButtonDeshacer.option("visible", !_soloLectura);
    dxButtonDeshacer.option("disabled", !cambios);
    dxButtonEliminar.option("visible", !_soloLectura && !cambios && ordenCompraId > 0 && ordenCompraEstatusId == ESTATUS_ACTIVA);
    dxButtonGuardaCambios.option("visible", !_soloLectura);

    $("#dxGridDetalles").dxDataGrid("columnOption", "Ajuste", "visible", getForm().Ajuste);
}

var setCambios = function () {
    cambios = true;
    habilitaComponentes();
}

var validaEliminar = function (event) {
    //Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}

var eliminaOC = function () {
    //Obtenemos el Objeto que se esta creando/editando en el Form 
    var modelo = dxFormModalCancelacion.option("formData");

    if (!modelo.FechaCancelacion) {
        //Mostramos mensaje de error
        toast("Favor de agregar la fecha de cancelación.", 'warning');

        return;
    }

    //Agregamos la fecha de cancelación
    var ordenCompra = getForm();

    var fechaCancelacion = new Date(modelo.FechaCancelacion);
    fechaCancelacion.setHours(new Date().getHours());
    fechaCancelacion.setMinutes(new Date().getMinutes());
    fechaCancelacion.setSeconds(new Date().getSeconds());
    fechaCancelacion.setMilliseconds(0);

    fechaCancelacion = fechaCancelacion.toLocaleString();
    ordenCompra.FechaCancelacion = fechaCancelacion;

    //Ocultamos el modal
    modalConfirmaEliminar.modal('hide');

    //Mostramos Loader
    dxLoaderPanel.show();

    //Hacemos la petición para eliminar el registro
    $.ajax({
        type: "POST",
        url: API_FICHA + "eliminarPorModelo",
        data: { ordenCompra: ordenCompra },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Registro cancelado con exito!", 'success');

            //Recargamos la ficha
            regresarListado();
        },
        error: function (response, status, error) {
            //Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al cancelar:\n" + response.responseText, 'error');
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
        store.remove(rowEliminar.OrdenCompraDetId)
            .done(function () {
                //Si el registro viene de la base de datos, respaldamos el registro 
                //para posteriormente eliminarlo en la base de datos
                if (rowEliminar.OrdenCompraDetId > 0) {
                    //Actualizamos el estatus del registro a "Cancelado"
                    rowEliminar.Status = ESTATUS_CANCELADA; //Borrar registro

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

var getFechaOC = function () {
    var modelo = $.extend(true, {}, dxForm.option('formData'));

    return modelo.Fecha ? modelo.Fecha.toISOString().split('T')[0] : null;
}

var txtFechaChange = function () {
    if (!ignoraEventos) {
        dxTxtFechaRecepcion.option("min", getFechaOC());
        dxTxtFechaVencimiento.option("min", getFechaOC());

        setCambios();
    }
}

var chkAjusteChange = function (event) {
    proveedorAjusteChange(true);
}

var cboProveedorChange = function (event) {
    dxCboTipoOperacion.option("value", null);
    dxCboTipoComprobanteFiscal.option("value", null);

    var proveedor = _listProveedores.find(x => x.ProveedorId === event.value);

    if (proveedor) {
        dxCboTipoOperacion.option("value", proveedor.TipoOperacionId);
        dxCboTipoComprobanteFiscal.option("value", proveedor.TipoComprobanteFiscalId);
    }

    proveedorAjusteChange(false);
}

var proveedorAjusteChange = function (ajuste) {
    //Obtenemos todos los registros que hay en el dxGridDetalles
    var detalles;
    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

    //Obtenemos la instancia store del DataSource
    var store = dxGridDetalles.getDataSource().store();

    detalles.forEach(modelo => {
        modelo.Ajuste = ajuste ? 0 : modelo.Ajuste;

        modelo = calculaTotalPartida(modelo, false);

        modelo.Status = modelo.OrdenCompraDetId < 0 ? modelo.Status : ESTATUS_MODIFICADO;

        store.update(modelo.OrdenCompraDetId, modelo)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxGridDetalles.getDataSource().reload();
            })
            .fail(function () {
                toast("No se pudo actualizar la tabla.", "error");
            });
    });

    setCambios();
}

var cboAlmacenChange = function (event) {
    if (!ignoraEventos) {
        almacenTemp = event.previousValue;

        validaLimpiarTabla();
    }
}

var cboUnidadAdministrativaChange = function (event) {
    if (!ignoraEventos) {
        unidadAdministrativaTemp = event.previousValue;

        validaLimpiarTabla();
    }
}

var cboProyectoChange = function (event) {
    if (!ignoraEventos) {
        proyectoTemp = event.previousValue;

        validaLimpiarTabla();
    }
}

var cboFuenteFinanciamientoChange = function (event) {
    if (!ignoraEventos) {
        fuenteFinanciamientoTemp = event.previousValue;

        validaLimpiarTabla();
    }
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

    dxCboAlmacen.option("value", almacenTemp);
    dxCboUnidadAdministrativa.option("value", unidadAdministrativaTemp);
    dxCboProyecto.option("value", proyectoTemp);
    dxCboFuenteFinanciamiento.option("value", fuenteFinanciamientoTemp);

    ignoraEventos = false;
}

var limpiarTabla = function () {
    //Limpiamos y recargamos la informacion de la tabla
    var dataSource = dxGridDetalles.getDataSource();
    dataSource.store().clear();
    dataSource.reload();

    dxCboProducto.option("dataSource", null);

    almacenTemp = dxCboAlmacen.option("value");
    unidadAdministrativaTemp = dxCboUnidadAdministrativa.option("value");
    proyectoTemp = dxCboProyecto.option("value");
    fuenteFinanciamientoTemp = dxCboFuenteFinanciamiento.option("value");
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
    dxFormModal.updateData("OrdenCompraDetId", contadorRegistrosNuevos);

    //Cambiamos el estatus del registro a "NUEVO"
    dxFormModal.updateData("Status", ESTATUS_NUEVO);
    dxFormModal.updateData("Estatus", "Activo");
    dxFormModal.updateData("Ajuste", 0);
    dxFormModal.updateData("PermiteEditar", true);

    //Decrementamos el contador de Registros para el siguiente nuevo registro
    contadorRegistrosNuevos -= 1;

    //Marcamos el modal con estatus "NUEVO"
    modalDetalle.attr("estatus", ESTATUS_NUEVO);

    //Mostramos el modal
    modalDetalle.modal('show');

    //Mostramos si se permite ajuste
    dxTxtAjuste.option("readOnly", !getForm().Ajuste);
}

var editaRegistro = function (event) {
    //Obtenemos los productos y Mostramos el modal
    getComboProductos(false, event);
}

var mostrarRegistroEditado = function (event) {
    //Obtenemos una copia del objeto a modificar
    var modelo = $.extend(true, {}, event.row.data);
    modelo.Ajuste = getForm().Ajuste ? modelo.Ajuste : 0;

    //Cambiamos el estatus del registro a "EDITADO" solo si no es un registro "NUEVO"
    modelo.Status = modelo.OrdenCompraDetId < 0 ? ESTATUS_NUEVO : ESTATUS_MODIFICADO;

    //Le pasamos el objeto al Form para que cargue sus valores
    dxFormModal.option("formData", modelo);

    //Marcamos el modal con estatus "EDITADO"
    modalDetalle.attr("estatus", ESTATUS_MODIFICADO);

    //Mostramos el modal
    modalDetalle.modal('show');

    //Mostramos si se permite ajuste
    dxTxtAjuste.option("readOnly", !getForm().Ajuste);
}

var getComboProductos = function (nuevo, event) {
    if (!dxCboProveedor.option("value")) {
        toast("Favor de seleccionar un proveedor.", 'warning');

        return;
    }

    if (!dxCboAlmacen.option("value")) {
        toast("Favor de seleccionar un almacén.", 'warning');

        return;
    }

    //Deshabilitamos el combo de Productos
    dxCboProducto.option("disabled", true);

    if (!dxCboProducto.option("dataSource") || !dxCboProducto.option("dataSource").length) {
        dxCboProducto.option("value", null);
        dxCboProducto.option("dataSource", null);
        dxCboProducto.option("contentTemplate", null);
        dxCboProducto.option("contentTemplate", $("#DataGridTemplate"));

        var almacenId = dxCboAlmacen.option("value");
        var dependenciaId = dxCboUnidadAdministrativa.option("value");
        var proyectoId = dxCboProyecto.option("value");
        var ramoId = dxCboFuenteFinanciamiento.option("value");

        //Mostramos Loader
        dxLoaderPanel.show();

        //Enviamos la informacion al controlador
        $.ajax({
            type: "POST",
            url: API_FICHA + "getProductos",
            data: { almacenId: almacenId, dependenciaId: dependenciaId, proyectoId: proyectoId, ramoId: ramoId },
            success: function (response) {
                //Ocultamos Loader
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
                //Ocultamos Loader
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

    Object.keys(event).forEach(prop => {
        dxFormModal.updateData(prop, event[prop]);
    });

    calculaTotalesModal();
}

var cboImpuestoChange = function (event) {
    dxFormModal.updateData("Impuesto", event.value ? dxCboImpuesto.option("text") : null);

    calculaTotalesModal();
}

var calculaTotalesModal = function () {
    calculaTotalPartida(dxFormModal.option("formData"), true);
}

var calculaTotalPartida = function (modelo, modal) {
    var cantidad = modelo.Cantidad ? round(modelo.Cantidad, 4) : 0;
    var costo = modelo.Costo ? round(modelo.Costo, 4) : 0;
    var importe = trunc(cantidad * costo, 4);
    var ieps = modelo.IEPS ? round(modelo.IEPS, 4) : 0;
    var ajuste = modelo.Ajuste ? round(modelo.Ajuste, 4) : 0;

    var iva = 0;
    var ish = 0;
    var retencionISR = 0;
    var retencionCedular = 0;
    var retencionIVA = 0;
    var totalPresupuesto = 0;
    var total = 0;

    var proveedor = _listProveedores.find(x => x.ProveedorId === getForm().ProveedorId);
    var tipoComprobanteFiscalId = proveedor ? proveedor.TipoComprobanteFiscalId : -1;
    var tarifaImpuesto = modelo.TarifaImpuestoId ? _listTarifasImpuesto.find(x => x.TarifaImpuestoId == modelo.TarifaImpuestoId) : null;

    if (tipoComprobanteFiscalId > 0 && tarifaImpuesto != null) {
        iva = tarifaImpuesto.Porcentaje > 0 ? round(((importe - (tarifaImpuesto.AplicaIvaAlIEPS === false ? ieps : 0)) * tarifaImpuesto.Porcentaje), 4) : 0;
        ish = tarifaImpuesto.porcISH > 0 ? round((importe) * tarifaImpuesto.porcISH, 4) : 0;
        retencionISR = tarifaImpuesto.PorcRetencionISR > 0 && (tipoComprobanteFiscalId == 2 || tipoComprobanteFiscalId == 3) ? round(importe * tarifaImpuesto.PorcRetencionISR, 4) : 0;
        retencionCedular = tarifaImpuesto.porcRetencionCedular > 0 && (tipoComprobanteFiscalId == 2 || tipoComprobanteFiscalId == 3) ? round(importe * tarifaImpuesto.porcRetencionCedular, 4) : 0;
        retencionIVA = tarifaImpuesto.PorcRetencionIVA > 0 && (tipoComprobanteFiscalId == 2 || tipoComprobanteFiscalId == 3) ? round(tarifaImpuesto.porcRetencionIVA > 0 && (tipoComprobanteFiscalId == 2 || tipoComprobanteFiscalId == 3) ? round((tarifaImpuesto.Porcentaje > 0 ? round((importe * tarifaImpuesto.Porcentaje) - ieps, 4) : 0) * tarifaImpuesto.porcRetencionIVA, 4) : 0, 4) : 0;
        totalPresupuesto = round(importe, 4) + (tarifaImpuesto.Porcentaje > 0 ? round(((importe - (tarifaImpuesto.AplicaIvaAlIEPS === true ? ieps : 0)) * tarifaImpuesto.Porcentaje), 4) : 0) + (tarifaImpuesto.porcISH > 0 ? round(importe * tarifaImpuesto.porcISH, 4) : 0) + (ajuste);

        if (tipoComprobanteFiscalId == 1 || tipoComprobanteFiscalId == 4 || tipoComprobanteFiscalId == 5) {
            total = round(importe, 4) + (tarifaImpuesto.Porcentaje > 0 ? round(((importe - (tarifaImpuesto.AplicaIvaAlIEPS === false ? ieps : 0)) * tarifaImpuesto.Porcentaje), 4) : 0) + (tarifaImpuesto.porcISH > 0 ? round(importe * tarifaImpuesto.porcISH, 4) : 0) + (ajuste);
        } else if (tipoComprobanteFiscalId == 2 || tipoComprobanteFiscalId == 3) {
            total = round(importe, 4) + (tarifaImpuesto.Porcentaje > 0 ? round(((importe - (tarifaImpuesto.AplicaIvaAlIEPS === false ? ieps : 0)) * tarifaImpuesto.Porcentaje), 4) : 0) - (tarifaImpuesto.porcRetencionIVA > 0 ? round((round((importe * tarifaImpuesto.Porcentaje) - ieps, 4)) * tarifaImpuesto.porcRetencionIVA, 4) : 0) - (tarifaImpuesto.PorcRetencionISR > 0 ? round(importe * tarifaImpuesto.PorcRetencionISR, 4) : 0) - (tarifaImpuesto.porcRetencionCedular > 0 ? round(importe * tarifaImpuesto.porcRetencionCedular, 4) : 0) + (ajuste);
        } else {
            total = round(importe, 4) + (ajuste);
        }
    } else {
        total = round(importe, 4) + (ajuste);
    }

    modelo.Importe = importe;
    modelo.IVA = round(iva, 4);
    modelo.ISH = round(ish, 4);
    modelo.RetencionISR = round(retencionISR, 4);
    modelo.RetencionCedular = round(retencionCedular, 4);
    modelo.RetencionIVA = round(retencionIVA, 4);
    modelo.TotalPresupuesto = round(totalPresupuesto, 4);
    modelo.Total = round(total, 4);

    //Si es un registro agregado
    if (modelo.OrdenCompraDetId < 0) {
        modelo.CantidadPorRecibir = modelo.Cantidad;
        modelo.CantidadRecibida = 0;
    }

    //Si se calcula desde el modal
    if (modal) {
        //Le pasamos el objeto al Form para que cargue sus valores
        dxFormModal.option("formData", modelo);
    }

    return modelo;    
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

    //Parte decimal del número
    var isNeg = num < 0;
    var decimal = num % 1;
    var entera = isNeg ? Math.ceil(num) : Math.floor(num);

    //Parte decimal como número entero
    var decimalFormated = Math.floor(
        Math.abs(decimal) * Math.pow(10, pos)
    )

    //Sustraemos del número original la parte decimal
    //y le sumamos la parte decimal que hemos formateado
    var finalNum = entera + ((decimalFormated / Math.pow(10, pos)) * (isNeg ? -1 : 1));

    return finalNum;
}

var existeProducto = function (ordenCompraDetId, almacenProductoId) {
    //Obtenemos todos los registros que hay en el dxGrid
    var detalles;
    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

    //Buscamos un registro que tenga el mismo registro
    var encontrado = detalles.find(x => x.OrdenCompraDetId !== ordenCompraDetId && x.AlmacenProductoId === almacenProductoId);

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
    if (existeProducto(modelo.OrdenCompraDetId, modelo.AlmacenProductoId)) {
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
        store.update(modelo.OrdenCompraDetId, modelo)
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

var guardaCambios = function () {
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
        toast("Favor de agregar artículos para comprar.", 'error');

        return;
    }

    //Agregamos los registros borrados, para eliminarlos en base de datos
    detalles = $.merge(detalles, registrosEliminados);

    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "guardaCambios",
        data: { ordenCompra: getForm(), detalles: detalles },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Registro guardado con exito!", 'success');

            //Regresamos al listado
            regresarListado();
        },
        error: function (response, status, error) {
            //Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al guadar:\n" + response.responseText, 'error');
        }
    });
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
    //Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA + (ordenCompraId == 0 ? "nuevo" : "editar/" + ordenCompraId);
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}