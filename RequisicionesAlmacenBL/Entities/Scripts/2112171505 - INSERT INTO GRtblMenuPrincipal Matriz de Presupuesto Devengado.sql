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
	55, -- NodoMenuId
	'Matriz de Presupuesto Devengado', -- Etiqueta
	'Ficha de Matriz de Presupuesto Devengado', -- Descripcion
	7, -- TipoNodoId
	33, -- NodoPadreId
	8, -- SistemaAccesoId
	'mir/mir/matrizpresupuestodevengado/listar', -- Url
	null,
	1, -- AdmitePermiso
	3, -- Orden
	1 -- EstatusId
)

SET IDENTITY_INSERT GRtblMenuPrincipal OFF