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
	54, -- NodoMenuId
	'Importar Almacén/Productos', -- Etiqueta
	'Ficha Importar Almacén/Productos', -- Descripcion
	7, -- TipoNodoId
	16, -- NodoPadreId
	8, -- SistemaAccesoId
	'inventarios/catalogos/importaralmacen', -- Url
	NULL, -- Icono
	1, -- AdmitePermiso
	2, -- Orden
	1 -- EstatusId
)
SET IDENTITY_INSERT GRtblMenuPrincipal OFF