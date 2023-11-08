
var dxFormLogin;

$(document).ready(function () {
    //Obtenemos la instancia del Form
    dxFormLogin = $("#dxFormLogin").dxForm("instance");

    //Verificamos si hay usuario por recordar
    var recordarUsuario = localStorage.getItem("recordarUsuario");

    //De haberlo, llenamos los campos de Usuario y Chk de Recordar
    if (recordarUsuario != null) {
        $('#chkRecordarme').dxCheckBox("instance").option('value', true);
    }
});

var iniciarSesion = function () {

    if (!dxFormLogin.validate().isValid) {
        return;
    }

    //Obtenemos la informacion del formulario
    var $dataGrid = $("#dxDataGridTemplate");
    var dataGrid = $dataGrid.dxDataGrid("instance");    
    var enteId = dataGrid.getSelectedRowsData()[0].EntidadId;
    var ejercicio = dataGrid.getSelectedRowsData()[0].Ejercicio;
    var usuario = $.extend(true, {}, dxFormLogin.option('formData')).UsuarioSesion;
    usuario.EnteId = enteId;
    usuario.Ejercicio = ejercicio;

    $.ajax({
        type: "POST",
        url: '/Login/IniciarSesion',
        data: usuario,
        success: function (result) {
            //Verificamos si esta seleccionado el check de Recordar
            var recordar = $('#chkRecordarme').dxCheckBox("instance").option("value");
            //Eliminamos el dato anterior a recordar, en caso de que exists
            localStorage.removeItem("recordarUsuario");
            if (recordar) {
                //Agregamos el nuevo usuario a recordar
                localStorage.setItem("recordarUsuario", result.usuario);
            }
            // Guardamos los menus en Storage
            localStorage.setItem("listaMenuPrincipal", result.listaMenuPrinicpal);
            //Guardamos el nombre del usuario en Storage
            localStorage.setItem("usuario", result.usuario);
            //Guardamos la base seleccionada
            localStorage.setItem("entidad", dataGrid.getSelectedRowsData()[0].EntidadId + ' - ' + dataGrid.getSelectedRowsData()[0].Nombre + ' - ' + dataGrid.getSelectedRowsData()[0].Ejercicio);
            //Redireccionamos al home
            window.location.href = '/home/index';
        },
        error: function (response, status, error) {
            //Mostramos mensaje de error
            //var jsonError = $.parseJSON(response.responseJSON);            
            toast(response.responseJSON, 'info');
        }
    });
}

var llenaUsuario = function () {
    //Verificamos si hay usuario por recordar
   return localStorage.getItem("recordarUsuario");
}

var enteValueChanged = function (e) {
    var $dataGrid = $("#dxDataGridTemplate");

    if ($dataGrid.length) {
        var dataGrid = $dataGrid.dxDataGrid("instance");
        dataGrid.selectRows(e.value, false);
    }
}

var enteDisplayExpr = function (item) {
    return item && item.EntidadId + " - " + item.Nombre + " - " + item.Ejercicio;
}

var toast = function (message, type) {
    DevExpress.ui.notify({ message: message, width: "auto"}, type, 5000);
}

