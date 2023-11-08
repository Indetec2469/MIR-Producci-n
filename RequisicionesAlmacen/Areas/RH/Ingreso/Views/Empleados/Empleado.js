//Componentes
var modalConfirmaDeshacer;
var modalConfirmaEliminar;
var dxButtonEliminarImagen;
var dxButtonEliminar;
var dxButtonGuardaCambios;
var dxButtonCancelar;
var dxButtonDeshacer;
var dxChkActivo;
var dropZoneText;
var dropZoneImage;
var dxEmailInstitucional;
var dxEmailPersonal;

//Variables de Control
var cambios;
var cambioImagen;
var eventoRegresar;
var empleadoId;
var dxFormEmpleado;
var nombreArchivoTemporal;
var file;
var eventoRegresar;

$(document).ready(function () {
    //Inicializamos las variables para la Ficha
    inicializaVariables();    

    //Respaldamos el modelo del Form
    getFormEmpleado();

    //Deshabilitamos los botones de acciones
    habilitaComponentes();    
});

var inicializaVariables = function () {   
    modalConfirmaDeshacer = $('#modalConfirmaDeshacer');
    modalConfirmaEliminar = $('#modalConfirmaEliminar');
    dxButtonEliminarImagen = $('#dxButtonEliminarImagen').dxButton("instance");
    dxButtonEliminar = $('#dxButtonEliminar').dxButton("instance");
    dxButtonGuardaCambios = $('#dxButtonGuardaCambios').dxButton("instance");    
    dxButtonCancelar = $('#dxButtonCancelar').dxButton("instance");    
    dxButtonDeshacer = $('#dxButtonDeshacer').dxButton("instance");    
    dxFormEmpleado = $("#dxFormEmpleado").dxForm("instance");
    dxChkActivo = $('#dxChkActivo').dxCheckBox("instance");
    dropZoneText = $("#dropzone-text")[0];
    dropZoneImage = $("#dropzone-image")[0];
    dxEmailInstitucional = $("#dxEmailInstitucional").dxValidator("instance");
    dxEmailPersonal = $("#dxEmailPersonal").dxValidator("instance");

    dropZoneText.style.display = dropZoneImage.src != "" ? "none" : "";
    dxButtonEliminarImagen.option("visible", dropZoneImage.src != "");

    cambios = false;
    cambioImagen = false;
    eventoRegresar = true;
}

var getFormEmpleado = function () {
    var modelo = $.extend(true, {}, dxFormEmpleado.option('formData'));
    modelo.EmpleadoId = modelo.EmpleadoId == null ? 0 : modelo.EmpleadoId;
    empleadoId = modelo.EmpleadoId;

    return modelo;
}

var setCambios = function () {
    cambios = true;
    habilitaComponentes();
    validaEmail();
}

var habilitaComponentes = function () {  
    dxButtonEliminar.option("visible", empleadoId > 0 && !cambios);
    dxButtonDeshacer.option("disabled", !cambios);
}

var validaEmail = function () {
    var empleado = getFormEmpleado();

    if (isNullOrEmpty(empleado.EmailInstitucional) && isNullOrEmpty(empleado.EmailPersonal)) {
        dxEmailInstitucional.option("validationRules", [{ type: "required", message: "Email Institucional Requerido" }]);
        dxEmailPersonal.option("validationRules", [{ type: "required", message: "Email Personal Requerido" }]);

        return false;
    } else {
        var mensaje = "El Email no es válido";
        var formato = "^[\\d\\w\\._\\-]+@([\\d\\w\\._\\-]+\\.)+[\\w]+$";

        dxEmailInstitucional.option("validationRules", [{ type: "pattern", message: mensaje, pattern: formato }]);
        dxEmailPersonal.option("validationRules", [{ type: "pattern", message: mensaje, pattern: formato }]);

        return true;
    }
}

var isNullOrEmpty = function (value) {
    return value == null || value == "";
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

var validaEliminar = function () {
    //Mostramos el modal de confirmacion
    modalConfirmaEliminar.modal('show');
}

var eliminaEmpleado = function () {
    //Mostramos Loader
    dxLoaderPanel.show();

    //Enviamos la informacion al controlador
    $.ajax({
        type: "POST",
        url: "/rh/ingreso/empleados/eliminar",
        data: { empleado: getFormEmpleado() },
        success: function () {
            // Ocultamos Loader
            dxLoaderPanel.hide();

            //Mostramos mensaje de Exito
            toast("Registro borrado con exito!", 'success');

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

var guardaCambios = function () {
    validaEmail();

    //Validamos que la informacion requerida del Formulario este completa
    if (!dxFormEmpleado.validate().isValid) {
        toast("Favor de completar los campos requeridos.", 'error');

        return;
    }

    if (cambios) {
        // Create FormData object
        var formData = new FormData();

        // Looping over all files and add it to FormData object
        formData.append("file", file);

        var empleado = getFormEmpleado();

        formData.append("empleado", JSON.stringify(empleado));

        formData.append("cambioImagen", cambioImagen);

        //Mostramos Loader
        dxLoaderPanel.show();

        $.ajax({
            type: "POST",
            url: '/rh/ingreso/empleados/guardaCambios',
            contentType: false,
            processData: false,
            data: formData,
            success: function (result) {
                // Ocultamos Loader
                dxLoaderPanel.hide();

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
    } else {
        //Mostramos mensaje de Exito
        toast("Registro guardado con exito!", 'success');

        //Regresamos al listado
        regresarListado();
    }
}

var regresarListado = function () {
    window.location.href = '/rh/ingreso/empleados/listar';
}

var recargarFicha = function () {
    // Recargamos la ficha según si es registro nuevo o se está editando
    window.location.href = '/rh/ingreso/empleados/' + (empleadoId == 0 ? 'nuevo' : 'editar/' + empleadoId);
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 5000);
}

var eliminaFotografia = function () {
    file = null;

    dropZoneImage.src = "";
    dropZoneText.style.display = "";

    dxButtonEliminarImagen.option("visible", false);

    setCambios();
    cambioImagen = true;
}

var cambiarFotografia = function (e) {
    file = e.value[0];

    const fileReader = new FileReader();

    fileReader.onload = function () {
        toggleDropZoneActive($("#dropzone-external")[0], false);
        dropZoneImage.src = fileReader.result;
    }

    fileReader.readAsDataURL(file);

    dropZoneText.style.display = "none";

    toggleImageVisible(false);

    dxButtonEliminarImagen.option("visible", true);

    setCambios();
    cambioImagen = true;
}

function dropZoneEnter(e) {
    if (e.dropZoneElement.id === "dropzone-external") {
        toggleDropZoneActive(e.dropZoneElement, true);
    }
}

function dropZoneLeave(e) {
    if (e.dropZoneElement.id === "dropzone-external") {
        toggleDropZoneActive(e.dropZoneElement, false);
    }
}

function toggleDropZoneActive(dropZone, activo) {
    if (activo) {
        dropZone.classList.add("dx-theme-accent-as-border-color");
        dropZone.classList.remove("dx-theme-border-color");
        dropZone.classList.add("dropzone-active");
    } else {
        dropZone.classList.remove("dx-theme-accent-as-border-color");
        dropZone.classList.add("dx-theme-border-color");
        dropZone.classList.remove("dropzone-active");
    }
}

function toggleImageVisible(visible) {
    $("#dropzone-image")[0].hidden = !visible;
}