//Componentes
var dxCboOrdenCompra;
var dxTxtFechaOC;
var dxTxtEstatusOC;
var dxTxtPolizaOC;
var dxCboProveedor;
var dxCboAlmacen;

var dxCboTipoOperacion;
var dxCboTipoComprobanteFiscal;
var dxTxtFecha;
var dxTxtFechaVencimiento;
var dxTxtFechaContrarecibo;
var dxTxtFechaPagoProgramado;
var dxTxtReferencia;
var dxTxtObservacion;

//Modales
var modalConfirmaDeshacer;
var modalConfirmaEliminarArticulo;
var modalConfirmaLimpiar;

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

//Variables de Control
var ordenCompraId;
var statusOC;
var compraId;
var ignoraEventos;
var cambios;
var eventoRegresar;

var API_FICHA = "/almacenes/almacenes/ordencomprarecibo/";

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
    dxTxtPolizaOC = $('#dxTxtPolizaOC').dxTextBox("instance");
    dxCboProveedor = $('#dxCboProveedor').dxSelectBox("instance");
    dxCboAlmacen = $('#dxCboAlmacen').dxSelectBox("instance");

    dxCboTipoOperacion = $('#dxCboTipoOperacion').dxSelectBox("instance");
    dxCboTipoComprobanteFiscal = $('#dxCboTipoComprobanteFiscal').dxSelectBox("instance");
    dxTxtFecha = $('#dxTxtFecha').dxDateBox("instance");
    dxTxtFechaVencimiento = $('#dxTxtFechaVencimiento').dxDateBox("instance");
    dxTxtFechaContrarecibo = $('#dxTxtFechaContrarecibo').dxDateBox("instance");
    dxTxtFechaPagoProgramado = $('#dxTxtFechaPagoProgramado').dxDateBox("instance");

    dxTxtReferencia = $('#dxTxtReferencia').dxTextBox("instance");
    dxTxtObservacion = $('#dxTxtObservacion').dxTextArea("instance");

    modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    modalConfirmaEliminarArticulo = $('#modalConfirmaEliminarArticulo');
    modalConfirmaLimpiar = $('#modalConfirmaLimpiar');

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
    var fechaActual = new Date().toISOString().split('T')[0];
    var minDate = _ejercicio + "-01-01";
    var maxDate = _ejercicio + "-12-31";

    dxTxtFecha.option("min", minDate);
    dxTxtFecha.option("max", fechaActual);
    dxTxtFechaVencimiento.option("min", minDate);
    dxTxtFechaVencimiento.option("max", maxDate);
    dxTxtFechaContrarecibo.option("min", minDate);
    dxTxtFechaContrarecibo.option("max", maxDate);
    dxTxtFechaPagoProgramado.option("min", minDate);
    dxTxtFechaPagoProgramado.option("max", maxDate);

    if (getForm().CompraId == 0 && !_fechaOperacion) {
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
    modelo.CompraId = modelo.CompraId || 0;
    compraId = modelo.CompraId;

    var fecha = modelo.Fecha;

    if (fecha) {
        fecha.setHours(new Date().getHours());
        fecha.setMinutes(new Date().getMinutes());
        fecha.setSeconds(new Date().getSeconds());
        fecha.setMilliseconds(0);

        modelo.Fecha = fecha.toLocaleString();
        modelo.Ejercicio = fecha.getFullYear();
    }

    var fechaVencimiento = modelo.FechaVencimiento;
    var fechaContrarecibo = modelo.FechaContrarecibo;
    var fechaPagoProgramado = modelo.FechaPagoProgramado;

    if (fechaVencimiento && fechaContrarecibo && fechaPagoProgramado) {
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

        modelo.FechaVencimiento = fechaVencimiento.toLocaleString();
        modelo.FechaContrarecibo = fechaContrarecibo.toLocaleString();
        modelo.FechaPagoProgramado = fechaPagoProgramado.toLocaleString();
    }

    var proveedor = _listProveedores.find(x => x.ProveedorId === modelo.ProveedorId);

    modelo.TipoOperacionId = proveedor ? proveedor.TipoOperacionId : -1;
    modelo.TipoComprobanteFiscalId = proveedor ? proveedor.TipoComprobanteFiscalId : -1;

    return modelo;
}

var habilitaComponentes = function () {
    dxButtonDeshacer.option("visible", !_soloLectura);
    dxButtonDeshacer.option("disabled", !cambios);
    dxButtonGuardaCambios.option("visible", !_soloLectura);

    dxButtonImprimir.option("visible", !cambios && compraId > 0);
}

var setCambios = function () {
    if (!ignoraEventos) {
        cambios = true;
        habilitaComponentes();
    }
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
    dxTxtPolizaOC.option("value", null);
    dxCboProveedor.option("value", null);
    dxCboAlmacen.option("value", null);

    var oc = _listOrdenesCompra.find(x => x.OrdenCompraId === ordenCompraTemp);    

    if (oc) {
        ordenCompraId = oc.OrdenCompraId;
        statusOC = oc.Status;

        dxTxtFechaOC.option("value", oc.FechaOC);
        dxTxtEstatusOC.option("value", oc.EstatusOC);
        dxTxtPolizaOC.option("value", oc.PolizaOC);
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

        //Calculamos los totales de los detalles
        calculaDetalles();
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

var calculaDetalles = function () {
    //Obtenemos todos los registros que hay en el dxGridDetalles
    var detalles;
    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

    detalles.forEach(rowEditar => {
        calculaTotalPartida(rowEditar);
    });
}

var calculaTotalPartida = function (modelo) {
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

var guardaCambios = function () {
    //Validamos que la informacion requerida del Formulario este completa
    if (!dxForm.validate().isValid) {
        toast("Favor de completar los campos requeridos.", 'error');

        return;
    }

    //Obtenemos todos los registros que hay en el dxGridDetalles
    var detalles;
    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

    //Validamos que los detalles tengan cantidad para recibir
    var detallesRecibir = [];

    detalles.forEach(m => {
        if (m.Cantidad) {
            detallesRecibir.push(m);
        }
    });

    //Validamos que la informacion requerida del Formulario este completa
    if (!detallesRecibir || !detallesRecibir.length) {
        toast("Favor de agregar artículos para recibir.", 'error');

        return;
    }

    //Validamos la cantidad por recibir
    var mensaeCantidad = '';

    detallesRecibir.forEach(m => {
        if (m.Cantidad > m.CantidadPorRecibir) {
            mensaeCantidad = "La cantidad para el producto [" + m.ProductoId + " - " + m.Descripcion + "] no puede ser mayor a la Cantidad por Recibir (" + m.CantidadPorRecibir + ").";
        }
    });

    if (mensaeCantidad) {
        toast(mensaeCantidad, 'error');

        return;
    }

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

            //Mostramos el reporte
            imprimir();

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