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
) VALUES
(
	43, -- NodoMenuId
	'Artículos Invitación', -- Etiqueta
	'Ficha Artículos Invitación', -- Descripcion
	7, -- TipoNodoId
	11, -- NodoPadreId
	8, -- SistemaAccesoId
	'compras/compras/invitacionarticulo', -- Url
	NULL, -- Icono
	1, -- AdmitePermiso
	4, -- Orden
	1 -- EstatusId
),
(
	44, -- NodoMenuId
	'Invitación de Compra', -- Etiqueta
	'Ficha Invitación de Compra', -- Descripcion
	7, -- TipoNodoId
	11, -- NodoPadreId
	8, -- SistemaAccesoId
	'compras/compras/invitacioncompra', -- Url
	NULL, -- Icono
	1, -- AdmitePermiso
	5, -- Orden
	1 -- EstatusId
),
(
	45, -- NodoMenuId
	'Recibo de Cortesías', -- Etiqueta
	'Ficha Recibo de Cortesías', -- Descripcion
	7, -- TipoNodoId
	11, -- NodoPadreId
	8, -- SistemaAccesoId
	'compras/compras/cortesia', -- Url
	NULL, -- Icono
	1, -- AdmitePermiso
	6, -- Orden
	1 -- EstatusId
),
(
	46, -- NodoMenuId
	'Transferencias', -- Etiqueta
	'Ficha de Transferencias', -- Descripcion
	7, -- TipoNodoId
	18, -- NodoPadreId
	8, -- SistemaAccesoId
	'inventarios/inventarios/transferencia', -- Url
	NULL, -- Icono
	1, -- AdmitePermiso
	3, -- Orden
	1 -- EstatusId
),
(
	47, -- NodoMenuId
	'Recursos Humanos', -- Etiqueta
	'Menú Recursos Humanos', -- Descripcion
	6, -- TipoNodoId
	NULL, -- NodoPadreId
	8, -- SistemaAccesoId
	NULL, -- Url
	NULL, -- Icono
	0, -- AdmitePermiso
	1, -- Orden
	1 -- EstatusId
),
(
	48, -- NodoMenuId
	'Ingreso', -- Etiqueta
	'SubMenú Ingreso', -- Descripcion
	98, -- TipoNodoId
	47, -- NodoPadreId
	8, -- SistemaAccesoId
	NULL, -- Url
	'icon ion-gear-a', -- Icono
	0, -- AdmitePermiso
	1, -- Orden
	1 -- EstatusId
),
(
	49, -- NodoMenuId
	'Empleados', -- Etiqueta
	'Ficha de Empleados', -- Descripcion
	7, -- TipoNodoId
	48, -- NodoPadreId
	8, -- SistemaAccesoId
	'rh/ingreso/empleados', -- Url
	NULL, -- Icono
	1, -- AdmitePermiso
	1, -- Orden
	1 -- EstatusId
)
SET IDENTITY_INSERT GRtblMenuPrincipal OFF

UPDATE GRtblMenuPrincipal SET Orden = 4 WHERE NodoMenuId = 24
GO

UPDATE GRtblMenuPrincipal SET Orden = 3 WHERE NodoMenuId = 15
GO

UPDATE GRtblMenuPrincipal SET Orden = 2 WHERE NodoMenuId = 1
GO