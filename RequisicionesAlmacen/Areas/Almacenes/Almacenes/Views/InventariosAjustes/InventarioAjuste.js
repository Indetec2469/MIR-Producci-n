//Componentes
var dxCboAlmacen;
var dxCboProducto;
var dxCboTipoMovimiento;
var dxCboConceptoAjuste;
var dxTxtCantidad;
var dxTxtComentarios;

var dxTxtUM;
var dxTxtCostoPromedio;
var dxTxtExistenciaActual;
var dxTxtExistenciaFinal;

var ignoraEventos = false;
var dxFileZone;

var dxButtonCancelarAgregar;
var dxButtonGuardarAgregar;

var modalComentario;
var modalCancelarAgregar;
var modalConfirmaEliminarArticulo;
var modalConfirmaDeshacerCambios;
var modalConfirmaGuardar;

var dxButtonDeshacer;
var dxButtonCancelar;
var dxButtonGuardaCambios;

var dxForm;
var dxGridDetalles;
var modalModelo;
var dxFormModalComentario;

//Variables de Control
var inventarioId;
var modeloVacio;
var contadorRegistrosNuevos;
var rowEliminar;
var cambios;
var cambiosForm;
var eventoRegresar;

var TIPO_MOVIMIENTO_INCREMENTAR = 9;
var TIPO_MOVIMIENTO_DISMINUIR = 10;

var API_FICHA = "/almacenes/almacenes/inventariosajustes/";

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();

    //Deshabilitamos los botones de acciones
    habilitaComponentes();

    //Respaldamos el modelo vacio del Form
    modeloVacio = $.extend(true, {}, dxForm.option('formData'));
});

var inicializaVariables = function () {
    dxCboAlmacen = $('#dxCboAlmacen').dxSelectBox("instance");
    dxCboProducto = $('#dxCboProducto').dxDropDownBox("instance");
    dxCboTipoMovimiento = $('#dxCboTipoMovimiento').dxSelectBox("instance");
    dxCboConceptoAjuste = $('#dxCboConceptoAjuste').dxSelectBox("instance");
    dxTxtCantidad = $('#dxTxtCantidad').dxNumberBox("instance");
    dxTxtComentarios = $('#dxTxtComentarios').dxTextArea("instance");

    dxTxtUM = $('#dxTxtUM').dxTextBox("instance");
    dxTxtCostoPromedio = $('#dxTxtCostoPromedio').dxNumberBox("instance");
    dxTxtExistenciaActual = $('#dxTxtExistenciaActual').dxNumberBox("instance");
    dxTxtExistenciaFinal = $('#dxTxtExistenciaFinal').dxNumberBox("instance");

    dxFileZone = $('#dxDropZone').dxFileUploader("instance");

    dxButtonCancelarAgregar = $('#dxButtonCancelarAgregar').dxButton("instance");
    dxButtonGuardarAgregar = $('#dxButtonGuardarAgregar').dxButton("instance");

    modalComentario = $('#modalComentario');
    modalCancelarAgregar = $('#modalCancelarAgregar');
    modalConfirmaEliminarArticulo = $('#modalConfirmaEliminarArticulo');
    modalConfirmaDeshacerCambios = $('#modalConfirmaDeshacerCambios');
    modalConfirmaGuardar = $('#modalConfirmaGuardar');

    dxButtonDeshacer = $('#dxButtonDeshacer').dxButton("instance");
    dxButtonCancelar = $('#dxButtonCancelar').dxButton("instance");
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");

    dxForm = $("#dxForm").dxForm("instance");
    dxGridDetalles = $('#dxGridDetalles').dxDataGrid("instance");
    modalModelo = $('#modalModelo');
    dxFormModalComentario = $("#dxFormModalComentario").dxForm("instance");

    cambios = false;
    cambiosForm = false;
    eventoRegresar = true;
    contadorRegistrosNuevos = -1;

    if (_inventarioAjusteId == 0 && !_fechaOperacion) {
        //Mostramos mensaje
        toast("Es necesario realizar la Configuración de Fechas correspondiente.", 'warning');
    }
}

var verComentarios = function (event) {
    return event.row.data.Comentarios;
}

var setCambios = function () {
    cambios = true;
    habilitaComponentes();
}

var habilitaComponentes = function () {
    dxButtonDeshacer.option("disabled", !cambios);
}

var nuevoArticulo = function () {
    //Inicializamos el modelo del Form
    dxForm.option("formData", $.extend(true, {}, modeloVacio));

    //Inicializamos los campos del Form
    dxForm.resetValues();  

    //Inicializamos el Archivo
    ignoraEventos = true;
    dxFileZone.option("value", []);
    ignoraEventos = false;

    //Inicializamos las variables de control
    cambiosForm = false;

    //Deshabilitamos el combo de artículos
    dxCboProducto.option("disabled", true);

    //Mostramos el modal 
    modalModelo.modal('show');
}

var editaArticulo = function (event) {
    //Obtenemos una copia del objeto a modificar
    var modelo = $.extend(true, {}, event.row.data);

    //Le pasamos el objeto al Form para que cargue sus valores
    dxForm.option("formData", modelo);

    //Inicializamos el Archivo
    ignoraEventos = true;
    dxFileZone.option("value", modelo.Archivo || []);
    ignoraEventos = false;

    //Inicializamos las variables de control
    cambiosForm = false;

    //Mostramos el modal
    modalModelo.modal('show');
}

var editaComentario = function (event) {
    //Obtenemos una copia del objeto a modificar
    var modelo = $.extend(true, {}, event.row.data);

    //Le pasamos el objeto al Form para que cargue sus valores
    dxFormModalComentario.option("formData", modelo);

    //Mostramos el modal
    modalComentario.modal('show');
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
        store.remove(rowEliminar.InventarioAjusteDetalleId)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxGridDetalles.getDataSource().reload();

                cambiosForm = true;
            })
            .fail(function () {
                toast("No se pudo eliminar el registro de la tabla.", "error");
            });

        rowEliminar = null;
    }

    setCambios();
}

var existeAlmacenProducto = function (inventarioAjusteDetalleId, almacenProductoId) {
    var existenciasTemp;

    //Obtenemos todos los registros que hay en el dxGridDetalles
    dxGridDetalles.getDataSource().store().load().done((res) => { existenciasTemp = res; });

    //Buscamos un registro que tenga el mismo AlmacenProductoId del registro que estamos validando
    var repetido = existenciasTemp.find(x => x.InventarioAjusteDetalleId !== inventarioAjusteDetalleId && x.AlmacenProductoId === almacenProductoId);

    //Retornamos <true> si se encontro un registro , <false> de lo contrario
    return repetido;
}

var guardaCambiosModal = function () {
    //Validamos que la informacion requerida del Formulario este completa
    if (!dxForm.validate().isValid) {
        toast("Favor de completar los campos requeridos.", 'error');

        return;
    }    

    //Obtenemos el Objeto que se esta creando/editando en el Form 
    var modelo = $.extend(true, {}, dxForm.option('formData'));

    //Validamos que no se repita el AlmacenProductoId
    if (existeAlmacenProducto(modelo.InventarioAjusteDetalleId || contadorRegistrosNuevos, modelo.AlmacenProductoId)) {
        toast("Ya existe un registro con el mismo Almacén y Artículo. Favor de verificar.", 'error');

        return;
    }

    //Agregamos el Costo del Movimiento al modelo
    modelo.CostoMovimiento = modelo.Cantidad * modelo.CostoPromedio * (modelo.TipoMovimientoId == TIPO_MOVIMIENTO_DISMINUIR ? -1 : 1);

    //Obtenemos el ConceptoAjuste para validar si es necesaria la evidencia
    var conceptoAjuste = ((dxCboConceptoAjuste.option("dataSource")).filter(item => { return (item.ConceptoAjusteInventarioId == modelo.ConceptoAjusteId) }))[0];
    var solicitaEvidencia = conceptoAjuste ? conceptoAjuste.SolicitaEvidencia : false;

    if (solicitaEvidencia && !modelo.NombreArchivoTmp) {
        toast("Favor de agregar el documento de evidencia.", "error");

        return;
    }

    //Obtenemos la instancia store del DataSource
    var store = dxGridDetalles.getDataSource().store();

    //Si es un registro nuevo, lo insertamos en el DataSource
    if (!modelo.InventarioAjusteDetalleId) {
        //Asignamos un Id al Form
        modelo.InventarioAjusteDetalleId = contadorRegistrosNuevos;

        //Decrementamos el contador de Registros para el siguiente nuevo registro
        contadorRegistrosNuevos -= 1;

        store.insert(modelo)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxGridDetalles.getDataSource().reload();

                //Habilitamos los botones de acciones
                habilitaComponentes(true);

                //Ocultamos el modal
                modalModelo.modal('hide');
            })
            .fail(function () {
                toast("No se pudo agregar el nuevo registro a la tabla.", "error");
            });
    }
    //Si es un registro que se esta editando, actualizamos su informacion en el DataSource
    else {
        store.update(modelo.InventarioAjusteDetalleId, modelo)
            .done(function () {
                //Recargamos la informacion de la tabla
                dxGridDetalles.getDataSource().reload();

                //Habilitamos los botones de acciones
                habilitaComponentes(true);

                //Ocultamos el modal
                modalModelo.modal('hide');
            })
            .fail(function () {
                toast("No se pudo actualizar el registro en la tabla.", "error");
            });
    }

    setCambios();
}

var guardaCambiosComentario = function () {
    //Obtenemos el Objeto que se esta creando/editando en el Form 
    var modelo = dxFormModalComentario.option("formData");    

    //Obtenemos la instancia store del DataSource
    var store = dxGridDetalles.getDataSource().store();

    store.update(modelo.InventarioAjusteDetalleId, modelo)
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

var validaDeshacer = function (regresar) {
    eventoRegresar = regresar;

    if (cambios) {
        //Mostramos el modal de confirmacion
        modalConfirmaDeshacerCambios.modal('show');
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

var validaGuardaCambios = function (regresar) {
    var existencias;

    //Obtenemos todos los registros que hay en el dxGridDetalles
    dxGridDetalles.getDataSource().store().load().done((res) => { existencias = res; });

    //Validamos que la informacion requerida del Formulario este completa
    if (!existencias || !existencias.length) {
        toast("Favor de agregar artículos para ajustar.", 'error');

        return;
    }

    //Mostramos el modal de confirmacion
    modalConfirmaGuardar.modal('show');
}

var guardaCambios = function () {
    var existenciasTemp;

    //Obtenemos todos los registros que hay en el dxGridDetalles
    dxGridDetalles.getDataSource().store().load().done((res) => { existenciasTemp = res; });

    //Quitamos del arreglo la propiedad Archivo para poder guardar
    var existencias = [];

    existenciasTemp.forEach(element => {
        if (element.Archivo) {
            element.Archivo = null;
        }

        existencias.push(element);
    });

    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "guardaCambios",
        data: { existencias: existencias },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("El registro se guardó con éxito con número de póliza [" + response.poliza + "].", 'success');

            inventarioId = response.inventarioAjusteId;

            //Recargamos la ficha
            recargarFicha();
        },
        error: function (response, status, error) {
            //Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al guadar:\n" + response.responseText, 'error');
        }
    });
}

var regresarListado = function () {
    window.location.href = API_FICHA + "listar";
}

var recargarFicha = function () {
    //Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA + (!inventarioId ? "nuevo" : "editar/" + inventarioId);
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}

var cboAlmacenChange = function (e) {
    var value = e.value;

    dxForm.updateData("Almacen", dxCboAlmacen.option("text"));

    dxCboProducto.option("value", null);
    dxCboProducto.option("dataSource", null);
    dxCboProducto.option("contentTemplate", null);
    dxCboProducto.option("contentTemplate", $("#EmbeddedDataGrid"));
    dxCboProducto.option("disabled", true);

    dxCboTipoMovimiento.option("value", null);
    dxCboConceptoAjuste.option("value", null);
    dxCboConceptoAjuste.option("dataSource", null);


    if (value && value.length) {
        //Mostramos Loader
        dxLoaderPanel.show();

        //Enviamos la informacion al controlador
        $.ajax({
            type: "POST",
            url: API_FICHA + "buscaProductosPorAlmacenId",
            data: { almacenId: value },
            success: function (response) {
                //Ocultamos Loader
                dxLoaderPanel.hide();

                dxCboProducto.option("dataSource", response);
                dxCboProducto.option("disabled", false);
            },
            error: function (response, status, error) {
                //Ocultamos Loader
                dxLoaderPanel.hide();

                //Ocultamos el modal
                modalModelo.modal('hide');

                //Mostramos mensaje de error
                toast("Error:\n" + response.responseText, 'error');
            }
        });
    }

    cambiosForm = true;
}

var gridBox_displayExpr = function (item) {
    if (item) {
        dxForm.updateData("InventarioAjusteId", 0);
        dxForm.updateData("AlmacenProductoId", item.AlmacenProductoId);
        dxForm.updateData("FuenteFinanciamientoId", item.FuenteFinanciamientoId);
        dxForm.updateData("FuenteFinanciamiento", item.FuenteFinanciamiento);
        dxForm.updateData("ProyectoId", item.ProyectoId);
        dxForm.updateData("Proyecto", item.Proyecto);
        dxForm.updateData("UnidadAdministrativaId", item.UnidadAdministrativaId);
        dxForm.updateData("UnidadAdministrativa", item.UnidadAdministrativa);
        dxForm.updateData("TipoGastoId", item.TipoGastoId);
        dxForm.updateData("TipoGasto", item.TipoGasto);
        dxForm.updateData("ProductoId", item.ProductoId);
        dxForm.updateData("Descripcion", item.Descripcion);
        dxForm.updateData("Producto", item.Producto);
        dxForm.updateData("CuentaPresupuestalId", item.CuentaPresupuestalId);
        dxForm.updateData("UnidadDeMedidaId", item.UnidadDeMedidaId);

        dxTxtUM.option("value", item.UnidadDeMedida);
        dxTxtCostoPromedio.option("value", item.CostoPromedio);
        dxTxtExistenciaActual.option("value", item.ExistenciaActual);

        return item.Producto;
    } else {
        return null;
    }
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

var cboProductoChange = function (e) {
    var value = e.value;

    var dataGrid = $("#embedded-datagrid");

    if (dataGrid.length) {
        dataGrid = dataGrid.dxDataGrid("instance");
        dataGrid.selectRows(value, false);
        dataGrid.option("grouping.autoExpandAll", false);
    }

    if (!value) {        
        dxTxtUM.option("value", null);
        dxTxtCostoPromedio.option("value", null);
        dxTxtExistenciaActual.option("value", null);
    }

    dxCboTipoMovimiento.option("value", null);
    dxCboConceptoAjuste.option("value", null);
    dxCboConceptoAjuste.option("dataSource", null);

    dxCboProducto.close();

    setExistenciaFinal();
}

var cboTipoMovimientoChange = function (e) {
    var value = e.value;

    dxForm.updateData("TipoMovimiento", dxCboTipoMovimiento.option("text"));

    dxCboConceptoAjuste.option("value", null);
    dxCboConceptoAjuste.option("dataSource", null);    

    if (value) {
        //Mostramos Loader
        dxLoaderPanel.show();

        //Enviamos la informacion al controlador
        $.ajax({
            type: "POST",
            url: API_FICHA + "buscaConceptosAjustePorTipoMovimientoId",
            data: { tipoMovimientoId: value },
            success: function (response) {
                //Ocultamos Loader
                dxLoaderPanel.hide();
                    
                //console.log(dxCboProducto.option("text"));
                console.log(response);

                if (value == TIPO_MOVIMIENTO_INCREMENTAR) {
                    var articulo = dxCboProducto.option("text");
                    //Si el articulo seleccionado es del capitulo 5000, quitamos de las opcion la de Sobrante de Obra
                    if (articulo.substring(0, 1) === '5') {
                        response.splice(response.findIndex(item => item.ConceptoAjusteInventarioId === 7), 1);                            
                    }
                }


                dxCboConceptoAjuste.option("dataSource", response);
            },
            error: function (response, status, error) {
                //Ocultamos Loader
                dxLoaderPanel.hide();

                //Mostramos mensaje de error
                toast("Error:\n" + response.responseText, 'error');
            }
        });
    }

    setExistenciaFinal();

    cambiosForm = true;
}

var cboConceptoAjusteChange = function (e) {
    dxForm.updateData("ConceptoAjuste", dxCboConceptoAjuste.option("text"));    

    cambiosForm = true;
}

var txtCantidadChange = function (e) {
    var cantidad = e.value;

    if (cantidad) {
        if (cantidad <= 0) {
            dxTxtCantidad.option("value", null);

            toast("Error: \n" + "La cantidad de ajuste debe ser mayor a 0", 'error');
        }
    }

    setExistenciaFinal();
}

var setExistenciaFinal = function () {
    var existenciaActual = dxTxtExistenciaActual.option("value");
    var cantidad = dxTxtCantidad.option("value");
    var existenciaFinal;
    var tipoMovimientoId = ($.extend(true, {}, dxForm.option('formData'))).TipoMovimientoId;

    if (tipoMovimientoId && existenciaActual != null && cantidad != null) {       
        if (tipoMovimientoId == TIPO_MOVIMIENTO_INCREMENTAR) {
            existenciaFinal = existenciaActual + cantidad;
        } else if (tipoMovimientoId == TIPO_MOVIMIENTO_DISMINUIR) {
            existenciaFinal = existenciaActual - cantidad;

            if (existenciaFinal < 0) {
                existenciaFinal = null;
                dxTxtCantidad.option("value", null);                

                toast("Error: \n" + "La existencia final no puede ser menor a 0", 'error');
            }
        }       
    }

    dxTxtExistenciaFinal.option("value", existenciaFinal);

    cambiosForm = true;
}

var validaCancelarAgregar = function () {
    if (cambiosForm) {
        //Mostramos el modal de confirmacion
        modalCancelarAgregar.modal('show');
    } else {
        cancelarAgregar();
    }
}

var cancelarAgregar = function (e) {
    //Ocultamos el modal
    modalModelo.modal('hide');
}

var cambiarArchivo = function (e) {
    if (!ignoraEventos) {
        var archivoTemp = e.value[0];

        //Agregamos el Archivo
        dxForm.updateData("Archivo", e.value);

        //Inicializamos el Archivo
        ignoraEventos = true;
        dxFileZone.option("value", []);
        ignoraEventos = false;

        if (archivoTemp) {
            //Create FormData object
            var formData = new FormData();

            //Looping over all files and add it to FormData object
            formData.append("file", archivoTemp);

            //Mostramos Loader
            dxLoaderPanel.show();

            //Enviamos la informacion al controlador
            $.ajax({
                type: "POST",
                url: API_FICHA + "guardaArchivoTemporal",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    //Ocultamos Loader
                    dxLoaderPanel.hide();

                    dxForm.updateData("NombreArchivoTmp", response);

                    //Inicializamos el Archivo
                    ignoraEventos = true;
                    dxFileZone.option("value", e.value);
                    ignoraEventos = false;
                },
                error: function (response, status, error) {
                    //Ocultamos Loader
                    dxLoaderPanel.hide();

                    eliminaArchivo();

                    //Mostramos mensaje de error
                    toast("Error:\n" + response.responseText, 'error');
                }
            });
        } else {
            eliminaArchivo();
        }

        cambiosForm = true;
    }
}

var eliminaArchivo = function () {
    dxForm.updateData("NombreArchivoTmp", null);

    cambiosForm = true;    
}