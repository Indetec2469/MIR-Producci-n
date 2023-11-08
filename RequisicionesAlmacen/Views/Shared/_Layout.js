//Variables de Control
var listaAlerta = [];
var autorizacionesTab;
var notificacionesTab;

//Variables Estaticas
var TIPO_AUTORIZACION = ControlMaestroMapeo.TipoNotificacionAlerta.AUTORIZACION;
var TIPO_NOTIFICACION = ControlMaestroMapeo.TipoNotificacionAlerta.NOTIFICACION;

$(document).ready(function () {
    //Asignamos el nombre del Usuario logueado
    $(".logged-name").text(localStorage.getItem("usuario"));
    $(".logged-entidad").text(localStorage.getItem("entidad"));

    let entidadShort = localStorage.getItem("entidad").split("-");
    $(".logged-entidad-short").text(entidadShort[0] + ' - ' + entidadShort[2]);
    //Eliminamos el Item seleccionado del menu
    localStorage.removeItem("selectedItem");

    //Cargamos las tabs de las Alertas
    autorizacionesTab = $('#list-autorizaciones');
    notificacionesTab = $('#list-notificaciones');

    autorizacionesTab.show();
    notificacionesTab.hide();

    //Obtenemos las Alertas
    iniciarPromiseAlertas();
});

var signout = function signout() {
    //Eliminamos la cookie
    document.cookie = "QKIE=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";

    //Eliminamos Storage
    if (localStorage.getItem("listaMenuPrincipal")) {
        localStorage.removeItem("listaMenuPrincipal");
    }

    //Eliminamos el usuario logueado
    localStorage.removeItem("usuario");

    //Eliminamos el Ente seleccionado
    localStorage.removeItem("ente");

    //Redireccionamos a la pagina de Login
    window.location.href = '/Login/Login';

    localStorage.setItem('logout-event', 'logout' + Math.random());
}

var iniciarPromiseAlertas = function () {
    new Promise((resolve, reject) => {
        setInterval(() => {
            $.ajax({
                type: 'GET',
                url: '/notificaciones/getListadoAlertas',
                success: function (response) {
                    if (response.length > 0) {
                        $('#alerto-rojo').addClass('square-8 bg-danger pos-absolute t-15 r-5 rounded-circle');

                        if (JSON.stringify(listaAlerta) != JSON.stringify(response)) {
                            listaAlerta = response;

                            //Limpiamos los listados
                            autorizacionesTab.empty();
                            notificacionesTab.empty();

                            listaAlerta.forEach(alerta => {
                                //Crear un div para Autorizaciones o Notificaciones
                                $(alerta.TipoAlertaId == TIPO_AUTORIZACION ? autorizacionesTab : notificacionesTab).append(
                                    $('<a>', {
                                        class: 'media-list-link',
                                        href: alerta.RutaAccion
                                    }).append(
                                        $('<div>', {
                                            class: 'media'
                                        }).append(
                                            $('<div>', {
                                                class: 'media-body'
                                            }).append(
                                                $('<b>', {
                                                    class: 'noti-text',
                                                    text: alerta.TipoMovimiento
                                                })
                                            ).append(
                                                $('<p>', {
                                                    class: 'noti-text',
                                                    text: alerta.Tramite
                                                })
                                            ).append(
                                                $('<span>', {
                                                    class: 'float-right text-primary',
                                                    text: alerta.Fecha
                                                })
                                            )
                                        )
                                    )
                                );
                            });
                        }
                    } else {
                        $('#alerto-rojo').removeClass('square-8 bg-danger pos-absolute t-15 r-5 rounded-circle');

                        //Limpiamos los listados
                        autorizacionesTab.empty();
                        notificacionesTab.empty();
                    }

                    resolve(response);
                },
                error: function (response, status, error) {
                    reject(response);
                }
            })
        }, 15000); //Tiempo para recargar en milisegundos.
    });
}

var cambiarVista = function (mostrar) {
    if (mostrar === 'autorizaciones') {
        autorizacionesTab.show();
        notificacionesTab.hide();
    } else if (mostrar === 'notificaciones') {
        autorizacionesTab.hide();
        notificacionesTab.show();
    }
}