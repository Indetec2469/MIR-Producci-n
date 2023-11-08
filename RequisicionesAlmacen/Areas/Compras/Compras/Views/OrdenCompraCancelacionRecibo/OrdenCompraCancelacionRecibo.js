//Componentes
var dxCboOrdenCompra;
var dxTxtFechaOC;
var dxTxtEstatusOC;
var dxCboProveedor;
var dxCboAlmacen;

var dxCboTipoOperacion;
var dxCboTipoComprobanteFiscal;
var dxTxtFecha;
var dxTxtFechaRecepcion;
var dxTxtReferencia;
var dxTxtObservacion;

//Modales
var modalConfirmaDeshacer;
var modalConfirmaEliminarArticulo;
var modalConfirmaLimpiar;
var modalConfirmaGuardar;

//Botones
var dxButtonImprimir;
var dxButtonCancelar;
var dxButtonDeshacer;
var dxButtonGuardaCambios;

//Forms
var dxForm;
var dxGridDetalles;

//Variables Globales
var rowEliminar;
var registrosEliminados;
var ordenCompraTemp;
var detallesRecibir;

//Variables de Control
var ordenCompraId;
var statusOC;
var compraId;
var ignoraEventos;
var cambios;
var eventoRegresar;

var API_FICHA = "/compras/compras/ordencompracancelacionrecibo/";

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();

    //Respaldamos el modelo del Form
    getForm();

    //Deshabilitamos los botones de acciones
    habilitaComponentes();
});

var inicializaVariables = function () {
    dxCboOrdenCompra = $('#dxCboOrdenCompra').dxSelectBox("instance");
    dxTxtFechaOC = $('#dxTxtFechaOC').dxTextBox("instance");
    dxTxtEstatusOC = $('#dxTxtEstatusOC').dxTextBox("instance");
    dxCboProveedor = $('#dxCboProveedor').dxSelectBox("instance");
    dxCboAlmacen = $('#dxCboAlmacen').dxSelectBox("instance");

    dxCboTipoOperacion = $('#dxCboTipoOperacion').dxSelectBox("instance");
    dxCboTipoComprobanteFiscal = $('#dxCboTipoComprobanteFiscal').dxSelectBox("instance");
    dxTxtFechaCancelacion = $('#dxTxtFechaCancelacion').dxDateBox("instance");
    dxTxtReferencia = $('#dxTxtReferencia').dxTextBox("instance");
    dxTxtObservacion = $('#dxTxtObservacion').dxTextArea("instance");

    modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    modalConfirmaEliminarArticulo = $('#modalConfirmaEliminarArticulo');
    modalConfirmaLimpiar = $('#modalConfirmaLimpiar');
    modalConfirmaGuardar = $('#modalConfirmaGuardar');

    dxButtonImprimir = $('#dxButtonImprimir').dxButton("instance");
    dxButtonCancelar = $('#dxButtonCancelar').dxButton("instance");
    dxButtonDeshacer = $('#dxButtonDeshacer').dxButton("instance");
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");

    dxForm = $("#dxForm").dxForm("instance");
    dxGridDetalles = $("#dxGridDetalles").dxDataGrid("instance");

    rowEliminar = null;
    registrosEliminados = [];

    ignoraEventos = false;
    cambios = false;
    eventoRegresar = true;

    //Asignamos el límite en los campos de Fecha
    var minDate = _ejercicio + "-01-01";
    var maxDate = _ejercicio + "-12-31";

    dxTxtFechaCancelacion.option("min", minDate);
    dxTxtFechaCancelacion.option("max", maxDate);
}

var getForm = function () {
    var modelo = $.extend(true, {}, dxForm.option('formData'));
    modelo.CompraId = modelo.CompraId || 0;   
    compraId = modelo.CompraId;
    ordenCompraId = modelo.OrdenCompraId;
    statusOC = modelo.StatusOC;

    var fecha = modelo.Fecha;
    var fechaVencimiento = modelo.FechaVencimiento;
    var fechaContrarecibo = modelo.FechaContrarecibo;
    var fechaPagoProgramado = modelo.FechaPagoProgramado;
    var fechaCancelacion = modelo.FechaCancelacion;

    fecha.setHours(0);
    fecha.setMinutes(0);
    fecha.setSeconds(0);
    fecha.setMilliseconds(0);

    fechaVencimiento.setHours(0);
    fechaVencimiento.setMinutes(0);
    fechaVencimiento.setSeconds(0);
    fechaVencimiento.setMilliseconds(0);

    fechaContrarecibo.setHours(0);
    fechaContrarecibo.setMinutes(0);
    fechaContrarecibo.setSeconds(0);
    fechaContrarecibo.setMilliseconds(0);

    fechaPagoProgramado.setHours(0);
    fechaPagoProgramado.setMinutes(0);
    fechaPagoProgramado.setSeconds(0);
    fechaPagoProgramado.setMilliseconds(0);

    fechaCancelacion.setHours(0);
    fechaCancelacion.setMinutes(0);
    fechaCancelacion.setSeconds(0);
    fechaCancelacion.setMilliseconds(0);

    modelo.Fecha = fecha.toLocaleString();
    modelo.FechaVencimiento = fechaVencimiento.toLocaleString();
    modelo.FechaContrarecibo = fechaContrarecibo.toLocaleString();
    modelo.FechaPagoProgramado = fechaPagoProgramado.toLocaleString();
    modelo.FechaCancelacion = fechaCancelacion.toLocaleString();
    modelo.Ejercicio = fecha.getFullYear();

    var proveedor = _listProveedores.find(x => x.ProveedorId === modelo.ProveedorId);

    modelo.TipoOperacionId = proveedor ? proveedor.TipoOperacionId : -1;
    modelo.TipoComprobanteFiscalId = proveedor ? proveedor.TipoComprobanteFiscalId : -1;

    return modelo;
}

var habilitaComponentes = function () {
    dxButtonDeshacer.option("visible", !_soloLectura);
    dxButtonDeshacer.option("disabled", !cambios);
    dxButtonGuardaCambios.option("visible", !_soloLectura);

    //dxButtonImprimir.option("visible", !cambios && compraId > 0);
}

var setCambios = function () {
    cambios = true;
    habilitaComponentes();
}

var cboOrdenCompraChange = function (event) {
    ordenCompraTemp = event.previousValue;

    validaLimpiarTabla();    
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

    dxCboOrdenCompra.option("value", ordenCompraTemp);

    ignoraEventos = false;
}

var limpiarTabla = function () {
    //Limpiamos y recargamos la informacion de la tabla
    var dataSource = dxGridDetalles.getDataSource();
    dataSource.store().clear();
    dataSource.reload();

    ordenCompraTemp = dxCboOrdenCompra.option("value");
    ordenCompraId = null;
    statusOC = null;

    dxTxtFechaOC.option("value", null);
    dxTxtEstatusOC.option("value", null);
    dxCboProveedor.option("value", null);
    dxCboAlmacen.option("value", null);

    var oc = _listOrdenesCompra.find(x => x.OrdenCompraId === ordenCompraTemp);

    if (oc) {
        ordenCompraId = oc.OrdenCompraId;
        statusOC = oc.Status;

        dxTxtFechaOC.option("value", oc.FechaOC);
        dxTxtEstatusOC.option("value", oc.EstatusOC);
        dxCboProveedor.option("value", oc.ProveedorId);
        dxCboAlmacen.option("value", oc.AlmacenId);

        //Obtenemos la instancia store del DataSource
        var store = dxGridDetalles.getDataSource().store();

        _listOrdenesCompraDetalles.forEach(modelo => {
            if (modelo.OrdenCompraId === ordenCompraId) {
                modelo.RequisicionTimestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(modelo.RequisicionTimestamp)));
                modelo.DetalleTimestamp = btoa(String.fromCharCode.apply(null, new Uint8Array(modelo.DetalleTimestamp)));

                store.insert(modelo)
                    .done(function () {
                        //Recargamos la informacion de la tabla
                        dxGridDetalles.getDataSource().reload();
                    })
                    .fail(function () {
                        toast("No se pudo agregar el nuevo registro a la tabla.", "error");
                    });
            }
        });
    }
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
        store.remove(rowEliminar.CompraDetId)
            .done(function () {
                //Si el registro viene de la base de datos, respaldamos el registro 
                //para posteriormente eliminarlo en la base de datos
                if (rowEliminar.CompraDetId > 0) {
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

var onEditingStart = function (event) {
    if (event.data.EstatusId != ControlMaestroMapeo.AREstatusRequisicionDetalle.POR_COMPRAR) {
        event.cancel = _soloLectura;
    }
}

var onDetallesChange = function (e) {
    if (e.name === "editing") {
        var editRowKey = e.component.option("editing.editRowKey");
        var changes = e.component.option("editing.changes");

        changes = changes.map((change) => {
            return {
                type: change.type,
                key: change.type !== "insert" ? change.key : undefined,
                data: change.data
            };
        });

        if (changes && changes.length) {
            setCambios();

            var cambios = changes[0].data;
            var propiedad = Object.getOwnPropertyNames(cambios)[0];

            //Obtenemos todos los registros que hay en el dxGridDetalles
            var detalles;
            dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

            var rowEditar = detalles.find(x => x.CompraDetId === changes[0].key);

            if (propiedad == "Cantidad") {
                rowEditar.Cantidad = cambios.Cantidad === null ? null : round(cambios.Cantidad, 4);
            }

            calculaTotalPartida(rowEditar);
        }
    }
}

var calculaTotalPartida = function (modelo) {
    var cantidad = modelo.Cantidad ? round(modelo.Cantidad, 4) : 0;
    var costo = modelo.Costo ? round(modelo.Costo, 4) : 0;
    var importe = trunc(cantidad * costo, 4);
    var ieps = modelo.IEPS ? round(modelo.IEPS, 2) : 0;
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
        iva = tarifaImpuesto.Porcentaje > 0 ? round(((importe - (tarifaImpuesto.AplicaIvaAlIEPS === false ? ieps : 0)) * tarifaImpuesto.Porcentaje), 2) : 0;
        ish = tarifaImpuesto.porcISH > 0 ? round((importe) * tarifaImpuesto.porcISH, 2) : 0;
        retencionISR = tarifaImpuesto.PorcRetencionISR > 0 && (tipoComprobanteFiscalId == 2 || tipoComprobanteFiscalId == 3) ? round(importe * tarifaImpuesto.PorcRetencionISR, 2) : 0;
        retencionCedular = tarifaImpuesto.porcRetencionCedular > 0 && (tipoComprobanteFiscalId == 2 || tipoComprobanteFiscalId == 3) ? round(importe * tarifaImpuesto.porcRetencionCedular, 2) : 0;
        retencionIVA = tarifaImpuesto.PorcRetencionIVA > 0 && (tipoComprobanteFiscalId == 2 || tipoComprobanteFiscalId == 3) ? round(tarifaImpuesto.porcRetencionIVA > 0 && (tipoComprobanteFiscalId == 2 || tipoComprobanteFiscalId == 3) ? round((tarifaImpuesto.Porcentaje > 0 ? round((importe * tarifaImpuesto.Porcentaje) - ieps, 2) : 0) * tarifaImpuesto.porcRetencionIVA, 2) : 0, 2) : 0;
        totalPresupuesto = round(importe, 2) + (tarifaImpuesto.Porcentaje > 0 ? round(((importe - (tarifaImpuesto.AplicaIvaAlIEPS === true ? ieps : 0)) * tarifaImpuesto.Porcentaje), 2) : 0) + (tarifaImpuesto.porcISH > 0 ? round(importe * tarifaImpuesto.porcISH, 2) : 0) + (ajuste);

        if (tipoComprobanteFiscalId == 1 || tipoComprobanteFiscalId == 4 || tipoComprobanteFiscalId == 5) {
            total = round(importe, 2) + (tarifaImpuesto.Porcentaje > 0 ? round(((importe - (tarifaImpuesto.AplicaIvaAlIEPS === false ? ieps : 0)) * tarifaImpuesto.Porcentaje), 2) : 0) + (tarifaImpuesto.porcISH > 0 ? round(importe * tarifaImpuesto.porcISH, 2) : 0) + (ajuste);
        } else if (tipoComprobanteFiscalId == 2 || tipoComprobanteFiscalId == 3) {
            total = round(importe, 2) + (tarifaImpuesto.Porcentaje > 0 ? round(((importe - (tarifaImpuesto.AplicaIvaAlIEPS === false ? ieps : 0)) * tarifaImpuesto.Porcentaje), 2) : 0) - (tarifaImpuesto.porcRetencionIVA > 0 ? round((round((importe * tarifaImpuesto.Porcentaje) - ieps, 2)) * tarifaImpuesto.porcRetencionIVA, 2) : 0) - (tarifaImpuesto.PorcRetencionISR > 0 ? round(importe * tarifaImpuesto.PorcRetencionISR, 2) : 0) - (tarifaImpuesto.porcRetencionCedular > 0 ? round(importe * tarifaImpuesto.porcRetencionCedular, 2) : 0) + (ajuste);
        } else {
            total = round(importe, 2) + (ajuste);
        }
    } else {
        total = round(importe, 2) + (ajuste);
    }

    modelo.Importe = importe;
    modelo.IVA = round(iva, 2);
    modelo.ISH = round(ish, 2);
    modelo.RetencionISR = round(retencionISR, 2);
    modelo.RetencionCedular = round(retencionCedular, 2);
    modelo.RetencionIVA = round(retencionIVA, 2);
    modelo.TotalPresupuesto = round(totalPresupuesto, 4);
    modelo.Total = round(total, 4);

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

var validaGuardar = function () {
    //Validamos que la informacion requerida del Formulario este completa
    if (!dxForm.validate().isValid) {
        toast("Favor de completar los campos requeridos.", 'error');

        return;
    }

    //Obtenemos todos los registros que hay en el dxGridDetalles
    var detalles;
    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

    //Validamos que los detalles tengan cantidad para recibir
    detallesRecibir = [];

    detalles.forEach(m => {
        if (m.Cantidad) {
            detallesRecibir.push(m);
        }
    });

    //Validamos que la informacion requerida del Formulario este completa
    if (!detallesRecibir || !detallesRecibir.length) {
        toast("Favor de agregar artículos para cancelar.", 'error');

        return;
    }

    //Mostramos el modal de confirmacion
    modalConfirmaGuardar.modal('show');
}

var guardaCambios = function () {
    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "guardaCambios",
        data: { ordenCompraId: ordenCompraId, statusOC: statusOC, compra: getForm(), detalles: detallesRecibir },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("El registro se guardó con éxito con número de póliza [" + response.poliza + "].", 'success');

            //Asignamos el Id del Recibo
            compraId = response.compraId;

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

var imprimir = function () {
    window.open(API_FICHA + "rptSurtimientoOC/" + compraId);
}

var gridBox_displayExpr = function (item) {
    return item ? item.ProductoId + ' - ' + item.Descripcion : null;
}

var regresarListado = function () {
    window.location.href = API_FICHA + "listar";
}

var recargarFicha = function () {
    // Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA + (compraId == 0 ? "nuevo" : "ver/" + compraId);
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

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}