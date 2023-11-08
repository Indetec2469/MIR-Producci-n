//Componentes
var dxCboOrdenCompra;
var dxCboProveedor;
var dxCboAlmacen;
var dxTxtFecha;
var dxTxtComentarios;

var dxCboProducto;
var dxCboCuentaProducto;
var dxTxtUnidadMedida;
var dxTxtCosto;

//Modales
var modalDetalle;
var modalConfirmaDeshacer;
var modalConfirmaEliminarArticulo;
var modalConfirmaLimpiar;
var modalConfirmaAfectar;

//Botones
var dxButtonCancelar;
var dxButtonDeshacer;
var dxButtonGuardaCambios;

//Forms
var dxForm;
var dxFormModal;
var dxGridDetalles;

//Variables Globales
var modeloVacio;
var contadorRegistrosNuevos;
var rowEliminar;

//Variables de Control
var cortesiaId;
var ignoraEventos;
var cambios;
var eventoRegresar;

var ordenCompraTemp;
var almacenTemp;
var proveedorTemp;

var ESTATUS_NUEVO = "N";
var ESTATUS_MODIFICADO = "M";

var API_FICHA = "/almacenes/almacenes/cortesia/";

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
    dxCboOrdenCompra = $('#dxCboOrdenCompra').dxSelectBox("instance");
    dxCboProveedor = $('#dxCboProveedor').dxSelectBox("instance");
    dxCboAlmacen = $('#dxCboAlmacen').dxSelectBox("instance");
    dxTxtFecha = $('#dxTxtFecha').dxDateBox("instance");
    dxTxtComentarios = $('#dxTxtComentarios').dxTextArea("instance");

    dxCboProducto = $('#dxCboProducto').dxSelectBox("instance");
    dxCboCuentaProducto = $('#dxCboCuentaProducto').dxDropDownBox("instance");
    dxTxtUnidadMedida = $('#dxTxtUnidadMedida').dxTextBox("instance");
    dxTxtCosto = $('#dxTxtCosto').dxNumberBox("instance");

    modalDetalle = $('#modalDetalle');
    modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    modalConfirmaEliminarArticulo = $('#modalConfirmaEliminarArticulo');
    modalConfirmaLimpiar = $('#modalConfirmaLimpiar');
    modalConfirmaAfectar = $('#modalConfirmaAfectar');

    dxButtonCancelar = $('#dxButtonCancelar').dxButton("instance");
    dxButtonDeshacer = $('#dxButtonDeshacer').dxButton("instance");
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");

    dxForm = $("#dxForm").dxForm("instance");
    dxFormModal = $("#dxFormModal").dxForm("instance");
    dxGridDetalles = $("#dxGridDetalles").dxDataGrid("instance");

    contadorRegistrosNuevos = -1;
    rowEliminar = null;
    ignoraEventos = false;
    cambios = false;
    eventoRegresar = true;

    getComboProductos();
    dxCboCuentaProducto.option("dataSource", null);

    //Asignamos el límite en los campos de Fecha
    var minDate = _ejercicio + "-01-01";
    var maxDate = _ejercicio + "-12-31";

    if (dxTxtFecha) {
        dxTxtFecha.option("min", minDate);
        dxTxtFecha.option("max", maxDate);
    }

    if (getForm().CortesiaId == 0 && !_fechaOperacion) {
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
    modelo.CortesiaId = modelo.CortesiaId || 0;
    cortesiaId = modelo.CortesiaId;

    var fecha = modelo.Fecha;

    if (fecha) {
        fecha.setHours(new Date().getHours());
        fecha.setMinutes(new Date().getMinutes());
        fecha.setSeconds(new Date().getSeconds());
        fecha.setMilliseconds(0);

        modelo.Fecha = fecha.toLocaleString();
    }

    return modelo;
}

var habilitaComponentes = function () {
    dxButtonDeshacer.option("visible", !_soloLectura);
    dxButtonDeshacer.option("disabled", !cambios);
    dxButtonGuardaCambios.option("visible", !_soloLectura);
}

var setCambios = function () {
    if (!ignoraEventos) {
        cambios = true;
        habilitaComponentes();
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
        store.remove(rowEliminar.CortesiaDetalleId)
            .done(function () {
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

var chkAfectaCostoChange = function (event) {
}

var cboOrdenCompraChange = function (event) {
    if (!ignoraEventos) {
        ordenCompraTemp = event.previousValue;

        proveedorTemp = dxCboProveedor.option("value");
        almacenTemp = dxCboAlmacen.option("value");

        dxCboProveedor.option("disabled", event.value);
        dxCboAlmacen.option("disabled", event.value);

        if (event.value) {
            var ordenCompra = _listOrdenesCompra.find(x => x.OrdenCompraId === event.value);

            ignoraEventos = true;

            dxCboProveedor.option("value", ordenCompra.ProveedorId);
            dxCboAlmacen.option("value", ordenCompra.AlmacenId);

            ignoraEventos = false;

            if (almacenTemp != ordenCompra.AlmacenId) {
                validaLimpiarTabla();
            }
        }
    }
}

var cboProveedorChange = function (event) {
    if (!ignoraEventos) {
        proveedorTemp = event.previousValue;

        ordenCompraTemp = dxCboOrdenCompra.option("value");
        almacenTemp = dxCboAlmacen.option("value");

        dxCboOrdenCompra.option("disabled", event.value);
    }
}

var cboAlmacenChange = function (event) {
    if (!ignoraEventos) {
        almacenTemp = event.previousValue;

        ordenCompraTemp = dxCboOrdenCompra.option("value");
        proveedorTemp = dxCboProveedor.option("value");

        dxCboOrdenCompra.option("disabled", event.value);

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

    dxCboOrdenCompra.option("value", ordenCompraTemp);
    dxCboProveedor.option("value", proveedorTemp);
    dxCboAlmacen.option("value", almacenTemp);

    ignoraEventos = false;
}

var limpiarTabla = function () {
    //Limpiamos y recargamos la informacion de la tabla
    var dataSource = dxGridDetalles.getDataSource();
    dataSource.store().clear();
    dataSource.reload();

    dxCboCuentaProducto.option("dataSource", null);

    ordenCompraTemp = dxCboOrdenCompra.option("value");
    proveedorTemp = dxCboProveedor.option("value");
    almacenTemp = dxCboAlmacen.option("value");
}

var nuevoRegistro = function () {
    if (!dxCboAlmacen.option("value")) {
        toast("Favor de seleccionar un almacén.", 'warning');

        return;
    }

    //Deshabilitamos el combo de Productos
    dxCboCuentaProducto.option("disabled", true);

    mostrarNuevoRegistro();
}

var mostrarNuevoRegistro = function () {
    //Inicializamos el modelo del Form
    dxFormModal.option("formData", $.extend(true, {}, modeloVacio));

    //Inicializamos los campos del Form
    dxFormModal.resetValues();

    //Asignamos un Id al Form
    dxFormModal.updateData("CortesiaDetalleId", contadorRegistrosNuevos);

    //Decrementamos el contador de Registros para el siguiente nuevo registro
    contadorRegistrosNuevos -= 1;

    //Marcamos el modal con estatus "NUEVO"
    modalDetalle.attr("estatus", ESTATUS_NUEVO);

    //Mostramos el modal
    modalDetalle.modal('show');
}

var editaRegistro = function (event) {
    if (!dxCboAlmacen.option("value")) {
        toast("Favor de seleccionar un almacén.", 'warning');

        return;
    }

    //Deshabilitamos el combo de Productos
    dxCboCuentaProducto.option("disabled", true);

    //Obtenemos los productos y Mostramos el modal
    getComboCuentasProductos(true, event);
}

var mostrarRegistroEditado = function (event) {
    //Obtenemos una copia del objeto a modificar
    var modelo = $.extend(true, {}, event.row.data);
    modelo.Ajuste = getForm().Ajuste ? modelo.Ajuste : 0;

    //Le pasamos el objeto al Form para que cargue sus valores
    dxFormModal.option("formData", modelo);

    //Marcamos el modal con estatus "EDITADO"
    modalDetalle.attr("estatus", ESTATUS_MODIFICADO);

    //Mostramos el modal
    modalDetalle.modal('show');
}

var getComboProductos = function (nuevo, event) {
    //Mostramos Loader
    dxLoaderPanel.show();

    //Enviamos la informacion al controlador
    $.ajax({
        type: "POST",
        url: API_FICHA + "getProductos",
        success: function (response) {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            //Actualizamos el DataSource
            var dataSource = new DevExpress.data.DataSource({
                store: {
                    type: "array",
                    key: "ProductoId",
                    data: response
                },
                paginate: true,
                pageSize: 10
            });

            //Asignamos el listado al combo
            dxCboProducto.option("dataSource", dataSource);
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error:\n" + response.responseText, 'error');
        }
    });
}

var getComboCuentasProductos = function (editado, event) {
    //Deshabilitamos el combo de Productos
    dxCboCuentaProducto.option("disabled", true);
    dxCboCuentaProducto.option("value", null);
    dxCboCuentaProducto.option("dataSource", null);
    dxCboCuentaProducto.option("contentTemplate", null);
    dxCboCuentaProducto.option("contentTemplate", $("#DataGridTemplate"));

    var almacenId = dxCboAlmacen.option("value");
    var productoId = editado ? event.row.data.ProductoId : dxCboProducto.option("value");

    if (productoId) {
        //Mostramos Loader
        dxLoaderPanel.show();

        //Enviamos la informacion al controlador
        $.ajax({
            type: "POST",
            url: API_FICHA + "getCuentasProductos",
            data: { almacenId: almacenId, productoId: productoId },
            success: function (response) {
                // Ocultamos Loader
                dxLoaderPanel.hide();

                //Asignamos el listado al combo
                dxCboCuentaProducto.option("dataSource", response);

                //Habilitamos el combo de Productos
                dxCboCuentaProducto.option("disabled", false);

                //Mostramos el modal
                if (editado) {
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
    }
}

var cboCuentaProductoChange = function (event) {
    dxCboCuentaProducto.close();

    Object.keys(event).forEach(prop => {
        if (prop == "Costo") {
            dxFormModal.updateData("PrecioUnitario", event[prop]);
        } else {
            dxFormModal.updateData(prop, event[prop]);
        }
    });

    calculaTotalesModal();
}

var calculaTotalesModal = function () {
    calculaTotalPartida(dxFormModal.option("formData"), true);
}

var calculaTotalPartida = function (modelo, modal) {
    var cantidad = modelo.Cantidad ? round(modelo.Cantidad, 4) : 0;
    var costo = modelo.PrecioUnitario ? round(modelo.PrecioUnitario, 4) : 0;
    var importe = trunc(cantidad * costo, 4);

    //Asignamos el total
    modelo.TotalPartida = trunc(cantidad * costo, 4);

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

var existeProducto = function (cortesiaDetalleId, productoId, cuentaPresupuestalEgrId) {
    //Obtenemos todos los registros que hay en el dxGrid
    var detalles;
    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

    //Buscamos un registro que tenga el mismo registro
    var encontrado = detalles.find(x => x.CortesiaDetalleId !== cortesiaDetalleId && x.ProductoId === productoId && x.CuentaPresupuestalEgrId === cuentaPresupuestalEgrId);

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
    if (existeProducto(modelo.CortesiaDetalleId, modelo.ProductoId, modelo.CuentaPresupuestalEgrId)) {
        toast("Ya existe un registro con la misma configuración de Producto y Cuenta Presupuestal. Favor de verificar.", 'warning')

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
        store.update(modelo.CortesiaDetalleId, modelo)
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

var validaAfectar = function () {
    //Validamos que se haya seleccionado un Proveedor
    if (!dxCboProveedor.option("value")) {
        dxForm.updateData("ProveedorId", null);
    }

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
        toast("Favor de agregar artículos de Cortesía.", 'error');

        return;
    }

    //Mostramos el modal de confirmacion
    modalConfirmaAfectar.modal('show');
}

var guardaCambios = function () {
    //Obtenemos todos los registros que hay en el dxGridDetalles
    var detalles;
    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "guardaCambios",
        data: { cortesia: getForm(), detalles: detalles },
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
    window.location.href = API_FICHA + (cortesiaId == 0 ? "nuevo" : "editar/" + cortesiaId);
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}