SET IDENTITY_INSERT GRtblMenuPrincipal ON

INSERT INTO GRtblMenuPrincipal(
	NodoMenuId,
	Etiqueta,
	Descripcion,
    TipoNodoId,
	NodoPadreId,
	SistemaAccesoId,
	Url,
	Icono,
	AdmitePermiso,
	Orden,
	EstatusId
) VALUES(
	50, -- NodoMenuId
	'Sistemas', -- Etiqueta
	'Menú Sistemas', -- Descripcion
	6, -- TipoNodoId
	null, -- NodoPadreId
	8, -- SistemaAccesoId
	null, -- Url
	null,
	0, -- AdmitePermiso
	5, -- Orden
	1 -- EstatusId
),
(
	51, -- NodoMenuId
	'Catálogos', -- Etiqueta
	'SubMenú Catálogos', -- Descripcion
	98, -- TipoNodoId
	50, -- NodoPadreId
	8, -- SistemaAccesoId
	null, -- Url
	'icon ion-ios-photos-outline',
	0, -- AdmitePermiso
	1, -- Orden
	1 -- EstatusId
),
(
	52, -- NodoMenuId
	'Roles', -- Etiqueta
	'Ficha de Roles', -- Descripcion
	7, -- TipoNodoId
	51, -- NodoPadreId
	8, -- SistemaAccesoId
	'sistemas/catalogos/rol/listar', -- Url
	null,
	1, -- AdmitePermiso
	1, -- Orden
	1 -- EstatusId
)
,
(
	53, -- NodoMenuId
	'Usuarios', -- Etiqueta
	'Ficha de Usuarios', -- Descripcion
	7, -- TipoNodoId
	51, -- NodoPadreId
	8, -- SistemaAccesoId
	'sistemas/catalogos/usuario/listar', -- Url
	null,
	1, -- AdmitePermiso
	2, -- Orden
	1 -- EstatusId
)

SET IDENTITY_INSERT GRtblMenuPrincipal OFF