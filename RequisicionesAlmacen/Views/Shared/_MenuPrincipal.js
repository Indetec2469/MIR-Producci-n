$(() => {
    // carga los menus con permiso desde iniciar sesion
    cargaMenu();
    // creamos un evento click la seleccion
    $(".sub-link").click(function () {
        var id = $(this).attr("nodoMenuId");
        if (id != null) {
            if (localStorage.getItem("selectedItem") != null) {
                localStorage.removeItem("selectedItem");
            }
            localStorage.setItem("selectedItem", id);
        }
    });
    // carga la selecciona de menu (existe o no)
    cargaSeleccionaMenu();
});

cargaMenu = () => {
    var listaMenuPrincipal = JSON.parse(localStorage.getItem('listaMenuPrincipal'));
    // Verificar existe el menu que carga la almacena desde en iniciar sesion si no y regresa el login
    if (listaMenuPrincipal) {
        if (listaMenuPrincipal.length > 0) {
            var menuPrincipal = $('#menu-principal');

            // Buscamos el tipo nodo de Menu para crear
            listaMenuPrincipal.filter(mp => mp.TipoNodoId == 6).map(mp => {
                menuPrincipal.append($('<label/>', {
                    class: 'sidebar-label pd-x-10 mg-t-25 tx-info tx-12',
                    text: mp.Etiqueta
                })).append($('<ul/>', {
                    class: 'br-sideleft-menu',
                    id: 'menu-principal-' + mp.NodoMenuId
                }));
                // Buscamos el tipo nodo de SubMenu para crear
                listaMenuPrincipal.filter(_mp => _mp.NodoPadreId == mp.NodoMenuId).map(_mp => {
                    $('#menu-principal-' + mp.NodoMenuId).append($('<li/>', {
                        class: 'br-menu-item'
                    }).append($('<a/>', {
                        href: '#',
                        class: 'br-menu-link with-sub',
                    }).append($('<i/>', {
                        class: 'menu-item-icon tx-20 ' + _mp.Icono
                    })).append($('<span/>', {
                        class: 'menu-item-label',
                        text: _mp.Etiqueta
                    }))).append($('<ul/>', {
                        class: 'br-menu-sub',
                        id: 'menu-prinicpal-sub-' + _mp.NodoMenuId
                    })));
                    // Buscamos el tipo nodo de Ficha para crear
                    listaMenuPrincipal.filter(__mp => __mp.NodoPadreId == _mp.NodoMenuId).map(__mp => {
                        $('#menu-prinicpal-sub-' + _mp.NodoMenuId).append($('<li/>', {
                            class: 'sub-item'
                        }).append($('<a/>', {
                            href: '/../' + __mp.Url,
                            class: 'sub-link text-wrap',
                            nodoMenuId: __mp.NodoMenuId,
                            text: __mp.Etiqueta
                        })));
                    });
                });
            });
        } else {
            console.log('no tienes permiso para los menus');
        }
    } else {
        // Cerra la sesion y regresa a login
        signout();
    }
}

cargaSeleccionaMenu = () => {
    var id = localStorage.getItem("selectedItem");

    if (id != null) {
        var menuItem = $("[nodoMenuId =" + id + "]");
        var menuItemParent = menuItem.closest(".br-menu-sub").siblings().closest("a.br-menu-link.with-sub")

        menuItem.siblings().find(".active").removeClass("active");
        menuItem.addClass("active");
        menuItemParent.addClass("active");
    }
}

signout = () => {
    //Eliminamos la cookie
    document.cookie = "QKIE=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    // Eliminamos la almacena de menu
    localStorage.removeItem("listaMenuPrincipal");
    //Eliminamos el Item seleccionado del menu
    localStorage.removeItem("selectedItem");
    //Redireccionamos a la pagina de Login
    window.location.href = '/Login/Login';
}

