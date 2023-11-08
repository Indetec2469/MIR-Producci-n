// VARIABLES GLOBALES //
// Form
var dxForm,
    // API
    API_FICHA = '/mir/reportes/reportefichatecnicaindicador/';
//////////////////////

// Function Default //
$(() => {
    // Inicializamos las variables para la ficha
    inicializaVariables();
});
//////////////////////

inicializaVariables = () => {
    // Form
    dxForm = $('#dxForm').dxForm('instance');
}

displayExpr = (event) => {
    return event ? event.Codigo : null;
    //return event ? event.Codigo + ' - ' + event.MItblPlanDesarrollo[0].Nombre : null;
}

onClickBusca = () => {
    // Validamos que la informacion requerida del Formulario este completa
    if (!dxForm.validate().isValid)
        return;
    // Mostramos Loader
    dxLoaderPanel.show();
    // Obtenemos el Objeto que se esta creando/editando en el Form
    var data = dxForm.option("formData");

    $.ajax({
        type: "POST",
        url: API_FICHA + "buscarreporte",
        data: { MIRId: data.MIR },
        success: function (response) {
            if (response.esNoReporte) {
                // Ocultamos Loader
                dxLoaderPanel.hide();
                // Mostramos mensaje
                //toast('No hay reporte ni el MIR', 'error');
                // Carga el reporte con limpiar
                $("#reporte").empty();
            } else {
                // Ocultamos Loader
                dxLoaderPanel.hide();
                // Mostramos mensaje de Exito
                //toast('Ha encontrado el reporte', 'success');
                // Carga el reporte
                $("#reporte").html(response);
            }
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();
            // Mostramos mensaje de error
            //toast(response.responseText, "error");
            toast('Error al cargar el reporte, inténtalo de nuevo más tarde', "error");
        }
    });
}

// Toast //
toast = (message, type) => {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 3000);
}
///////////