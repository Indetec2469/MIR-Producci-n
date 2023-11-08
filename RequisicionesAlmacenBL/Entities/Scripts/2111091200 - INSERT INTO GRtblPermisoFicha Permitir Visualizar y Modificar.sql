SET IDENTITY_INSERT GRtblPermisoFicha ON
INSERT INTO GRtblPermisoFicha
(
	PermisoFichaId,
	Etiqueta,
	Descripcion,
	NodoMenuId,
	EstatusId,
	FechaCreacion,
	CreadoPorId
)
VALUES
(1, 'Permitir Visualizar y Modificar Proyecto', 'Permiso Visualizar y Modificar Proyecto en la ficha de MIR', 34, 1, GETDATE(), 2)
SET IDENTITY_INSERT GRtblPermisoFicha OFF