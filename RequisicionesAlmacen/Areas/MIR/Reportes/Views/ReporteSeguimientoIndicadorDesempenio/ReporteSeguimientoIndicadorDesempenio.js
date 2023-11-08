// VARIABLES GLOBALES //
// Form
var dxForm,
    // API
    API_FICHA = '/mir/reportes/reporteseguimientoindicadordesempenio/';
//////////////////////

// Function Default //
$(() => {
    // Inicializamos las variables para la ficha
    inicializaVariables();
});
//////////////////////

inicializaVariables = () => {
    // Form
    dxForm = $('#dxForm').dxForm({
       
        onInitialized: function (e) {
            dxForm = e.component;
        }
    }).dxForm("instance");
    // Window resize cuando cambia el tamanio de la pantalla como escritorio o movil o tablet
    window.onresize = () => {
        dxForm.option('labelLocation', $(window).width() > 768 ? 'left' : 'top');
    }
}

displayExpr = (event) => {
    return event ? event.ProgramaGobiernoId + ' - ' + event.Nombre : null;
}
//// Evento change del campo de selección CheckBox
//$("#SemaforoId").change(function () {
//    semaforoIdValue = $(this).prop("checked");
//});


onClickBusca = () => {
    // Validamos que la informacion requerida del Formulario este completa
    if (!dxForm.validate().isValid)
        return;
    // Mostramos Loader
    dxLoaderPanel.show();
    // Obtenemos el Objeto que se esta creando/editando en el Form
    
    var data = dxForm.option("formData");
    var semaforo = $('#chkSemaforo').dxCheckBox("instance").option("value");
    var fechaValue = $('#dtFecha').dxDateBox('instance').option("value");
    var fechaFormateada = fechaValue.toLocaleDateString("es-ES", { day: '2-digit', month: '2-digit', year: 'numeric' });
    $.ajax({
        type: "POST",
        url: API_FICHA + "buscarreporte",
        data: { mirId: data.MIRId, fechaReporte: fechaFormateada, semaforoId: semaforo },
        cache: false,
        success: function (response) {
            console.log(response);
            // Ocultamos Loader
            dxLoaderPanel.hide();
            // Mostramos mensaje de cuanto tiene reporte
            toast(response, 'success');
            //if (response == 1) {
            //    toast('Ha encontrado el reporte', 'success');
            //} else {
            //    toast('Han encontrado los reportes', 'success');
            //}
            // redirecciona para descargar el reporte en excel 
            window.location = API_FICHA + 'DescargarExcel';
        },
        error: function (response, status, error) {
            // Ocultamos Loader
            dxLoaderPanel.hide();
            // Mostramos mensaje de error
            //toast(response.responseText, "error");
            toast(response.responseText, "error");
        }
    })
}

// Toast //
toast = (message, type) => {
    DevExpress.ui.notify({ message: message, width: "auto" }, type, 3500);
}
///////////