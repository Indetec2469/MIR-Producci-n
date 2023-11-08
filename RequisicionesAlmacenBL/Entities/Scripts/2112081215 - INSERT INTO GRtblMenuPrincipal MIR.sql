SET IDENTITY_INSERT GRtblMenuPrincipal ON

INSERT INTO GRtblMenuPrincipal(
	NodoMenuId,
	Etiqueta,
	Descripcion,
    TipoNodoId,
	NodoPadreId,
	SistemaAccesoId,
	Url,
	AdmitePermiso,
	Orden,
	EstatusId
) VALUES(
	42, -- NodoMenuId
	'Relación de Resultados de Indicadores', -- Etiqueta
	'Reporte Relación de Resultados de Indicadores', -- Descripcion
	7, -- TipoNodoId
	37, -- NodoPadreId
	8, -- SistemaAccesoId
	'mir/reportes/reporterelacionresultadoindicador', -- Url
	1, -- AdmitePermiso
	5, -- Orden
	1 -- EstatusId
)

SET IDENTITY_INSERT GRtblMenuPrincipal OFF