//Modales
var modalConfirmaDeshacer;
var modalConfirmaAfectar;

//Botones
var dxButtonDeshacer;
var dxButtonGuardaCambios;

//Forms
var dxForm;
var dxGridDetalles;
var dxFileZone;

//Variables de Control
var ignoraEventos;
var cambios;
var eventoRegresar;

var API_FICHA = "/almacenes/catalogos/importaralmacen/";

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();

    //Deshabilitamos los botones de acciones
    habilitaComponentes();
});

var inicializaVariables = function () {
    modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    modalConfirmaAfectar = $('#modalConfirmaAfectar');

    dxButtonDeshacer = $('#dxButtonDeshacer').dxButton("instance");
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");

    dxForm = $("#dxForm").dxForm("instance");
    dxGridDetalles = $("#dxGridDetalles").dxDataGrid("instance");
    dxFileZone = $('#dxDropZone').dxFileUploader("instance");

    //Asignamos el source a la tabla
    dxGridDetalles.option("dataSource", []);

    ignoraEventos = false;
    cambios = false;
    eventoRegresar = true;

    if (!_fechaOperacion) {
        //Mostramos mensaje
        toast("Es necesario realizar la Configuración de Fechas correspondiente.", 'warning');
    }
}

var habilitaComponentes = function () {
    dxButtonDeshacer.option("disabled", !cambios);
}

var setCambios = function () {
    cambios = true;
    habilitaComponentes();
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

var validaAfectar = function () {
    //Obtenemos todos los registros que hay en el dxGridDetalles
    var detalles;
    dxGridDetalles.getDataSource().store().load().done((res) => { detalles = res; });

    //Validamos que la informacion requerida del Formulario este completa
    if (!detalles || !detalles.length) {
        toast("Favor de agregar artículos para importar.", 'error');

        return;
    }

    var mensajes = false;

    detalles.forEach(registro => {
        mensajes = registro.Errores ? true : mensajes;
    });

    if (mensajes) {
        toast("No es posible importar, se encontraron problemas en los artículos.", 'error');

        return;
    }

    //Mostramos el modal de confirmacion
    modalConfirmaAfectar.modal('show');
}

var guardaCambios = function () {
    //Obtenemos todos los registros que hay en el dxGridDetalles
    var productos;
    dxGridDetalles.getDataSource().store().load().done((res) => { productos = res; });

    //Mostramos Loader
    dxLoaderPanel.show();

    $.ajax({
        type: "POST",
        url: API_FICHA + "guardaCambios",
        data: { productos: productos },
        success: function (response) {
            //Mostramos mensaje de Exito
            toast("Registro guardado con exito!", 'success');

            //Recargamos la ficha
            recargarFicha();
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de error
            toast("Error al guadar:\n" + response.responseText, 'error');
        }
    });
}

var cambiarArchivo = function (e) {
    if (!ignoraEventos) {
        var archivoTemp = e.value[0];

        var extensiones = [".xls", ".xlsx", ".csv" ];
        var extension = archivoTemp.name.substring(archivoTemp.name.indexOf("."), archivoTemp.name.length);

        if (!extensiones.includes(extension)) {
            //Inicializamos el Archivo
            ignoraEventos = true;
            dxFileZone.option("value", []);
            ignoraEventos = false;

            toast("No es posible leer el archivo seleccionado.", "error");

            //Limpiamos y recargamos la informacion de la tabla
            var dataSource = dxGridDetalles.getDataSource();
            dataSource.store().clear();
            dataSource.reload();

            return;
        }

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
                url: API_FICHA + "leerArchivo",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    //Ocultamos Loader
                    dxLoaderPanel.hide();

                    //Asignamos el source a la tabla
                    dxGridDetalles.option("dataSource", response);
                },
                error: function (response, status, error) {
                    //Ocultamos Loader
                    dxLoaderPanel.hide();

                    //Inicializamos el Archivo
                    ignoraEventos = true;
                    dxFileZone.option("value", []);
                    ignoraEventos = false;

                    //Mostramos mensaje de error
                    toast("Error:\n" + response.responseText, 'error');
                }
            });
        }

        setCambios();
    }
}

var regresarListado = function () {
    window.location.href = API_FICHA;
}

var recargarFicha = function () {
    // Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = API_FICHA;
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}